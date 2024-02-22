using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GoNatureAR.Sensors;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace GoNatureAR.Requests
{
    public class PilotDataRequestManager : MonoBehaviour
    {
        public static PilotDataRequestManager Instance { private set; get; }

        public static Action<Pilot> OnPilotSelect;
        public static Action<Dictionary<EnumKey, double?>> OnThermalDataRecieved;
        public static Action<Dictionary<EnumKey, double?>> OnAirDataRecieved;
        public static Action<Dictionary<EnumKey, double?>> OnNoiseDataRecieved;
        public static Action OnDataReceived;
        public static Action<string> OnError;

        private string accessToken;
        private Pilot currentPilot;
        private TimeSpan expiresIn = new TimeSpan(0, 55, 0);
        private DateTime accessTokenStart;

        private CancellationTokenSource tokenSource;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
        }

        public async Task SendRequestsForData(Pilot pilot)
        {
            tokenSource = new CancellationTokenSource();
            CancellationToken cancellationTokent = tokenSource.Token;

            OnPilotSelect?.Invoke(pilot);

            try
            {
                var airAverageData = await RequestData(pilot, SensorType.air, cancellationTokent);
                if(airAverageData != null && airAverageData.Count > 0)
                    OnAirDataRecieved?.Invoke(airAverageData);

                currentPilot = pilot;

                var thermalAverageData = await RequestData(pilot, SensorType.thermal, cancellationTokent);
                if (thermalAverageData != null && thermalAverageData.Count > 0)
                    OnThermalDataRecieved?.Invoke(thermalAverageData);

                var noiseAverageData = await RequestData(pilot, SensorType.noise, cancellationTokent);
                if (noiseAverageData != null && noiseAverageData.Count > 0)
                    OnNoiseDataRecieved?.Invoke(noiseAverageData);

                OnDataReceived?.Invoke();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Cancel Requests");
            }

            tokenSource.Dispose();
        }

        /// <summary>
        /// Request access token for autorizetion.
        /// </summary>
        /// <param name="pilot"></param>
        /// <returns></returns>
        private async Task GetAccessToken(Pilot pilot)
        {   
            Response oauth2Response = await new OAuth2Request(pilot).ExecuteRequestAsync(DisplayErrorMessage);
            JObject data = JObject.Parse(oauth2Response.ResponseData);

            accessToken = (string)data["access_token"];
            accessTokenStart = DateTime.Now;

            Debug.Log($"Access Token: {accessToken}");
        }

        /// <summary>
        /// Checks wether the access token has been expired.
        /// </summary>
        /// <returns>True/False</returns>
        private bool AccessTokenExpired()
        {
            if (accessToken == null || (DateTime.Now - accessTokenStart) > expiresIn)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Requests the data of the given <paramref name="pilot"/>.
        /// </summary>
        /// <param name="pilot"></param>
        private async Task<Dictionary<EnumKey, double?>> RequestData(Pilot pilot, SensorType sensorType, CancellationToken cancellationToken)
        {
            if (AccessTokenExpired() || currentPilot != pilot)
                await GetAccessToken(pilot);

            if (cancellationToken.IsCancellationRequested)
            {
                Debug.Log($"{sensorType} Request Canceled");
                cancellationToken.ThrowIfCancellationRequested();
            }

            List<string> sensorNames = GetSensorsNames(pilot, sensorType);

            if (sensorNames == null)
                return null;

            var dict = new Dictionary<EnumKey, double?>();
            foreach (var sensor in sensorNames)
            {
                var data = await RequestSensorData(pilot, sensor);
                if (data == null)
                    continue;

                ISensorData pilotSensorsData = SetSensorsData(pilot);
                switch (sensorType)
                {
                    case SensorType.thermal: 
                    {
                        foreach (var attr in pilotSensorsData.ThermalComfortData.Measurements)
                        {
                            AssignValues(new EnumKey(attr), dict, data);
                        }
                        break;
                    }
                    case SensorType.air:
                    {
                        foreach (var attr in pilotSensorsData.AirQualityData.Measurements)
                        {
                            AssignValues(new EnumKey(attr), dict, data);
                        }
                        break;
                    }
                    case SensorType.noise:
                    {
                        foreach (var attr in pilotSensorsData.NoiseData.Measurements)
                        {
                            AssignValues(new EnumKey(attr), dict, data);
                        }
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
            return dict;
        }

        /// <summary>
        /// Request the data of a specific pilot sensor. 
        /// </summary>
        /// <param name="pilot"></param>
        /// <param name="sensorName"></param>
        /// <returns></returns>
        private async Task<JObject> RequestSensorData(Pilot pilot,string sensorName)
        {
            Response response = await new PilotDataRequest(accessToken, pilot, sensorName).ExecuteRequestAsync(DisplayErrorMessage);

            if (response == null)
                return null;

            JObject data = JObject.Parse(response.ResponseData);
            return data;
        }

        /// <summary>
        /// Assignes values to a dictioanry.
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="dict"></param>
        /// <param name="data"></param>
        private void AssignValues(EnumKey attr, Dictionary<EnumKey, double?> dict, JObject data)
        {
            var value = GetAttributeValue(data, attr.GetEnumValue().ToString());
            if (value == null)
                return;

            if (dict.ContainsKey(new EnumKey(attr.GetEnumValue())))
                dict[new EnumKey(attr.GetEnumValue())] = (dict[new EnumKey(attr.GetEnumValue())] + GetAttributeValue(data, attr.GetEnumValue().ToString())) / 2;
            else
                dict.Add(new EnumKey(attr.GetEnumValue()), GetAttributeValue(data, attr.GetEnumValue().ToString()));
        }

        /// <summary>
        /// Get the <paramref name="type"/> names of each <paramref name="pilot"/>'s sensor.
        /// </summary>
        /// <param name="pilot"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<string> GetSensorsNames(Pilot pilot, SensorType type)
        {
            switch (pilot) 
            {
                case Pilot.chania:
                    return ProcessSensors<ChaniaSensors>(type);
                case Pilot.castelfranco_veneto:
                    return ProcessSensors<CastelfrancoSensors>(type);
                case Pilot.novo_mesto:
                    return ProcessSensors<NovoMestoSensors>(type);
                case Pilot.gzira:
                    return ProcessSensors<GziraSensors>(type);
                case Pilot.leuven:
                    return ProcessSensors<LeuvenSensors>(type);
                case Pilot.skelleftea:
                    return ProcessSensors<SkellefteaSensors>(type);
                case Pilot.dundalk:
                    return ProcessSensors<DundalkSensors>(type);
                default: 
                    return null;
            }
        }

        /// <summary>
        /// Sets the sensor's data measurements for each <paramref name="pilot"/>.
        /// </summary>
        /// <param name="pilot"></param>
        /// <returns></returns>
        private ISensorData SetSensorsData(Pilot pilot)
        {
            switch (pilot)
            {
                case Pilot.chania:
                    return new ChaniaSensorData();
                case Pilot.castelfranco_veneto:
                    return new CastelfrancoVenetoSensorData();
                case Pilot.novo_mesto:
                    return new NovoMestoSensorData();
                case Pilot.gzira:
                    return new GziraSensorData();
                case Pilot.leuven:
                    return new LeuvenSensorData();
                case Pilot.skelleftea:
                    return new SkellefteaSensorData();
                case Pilot.dundalk:
                    return new DundalkSensorData();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets a generic type <typeparamref name="T"/> and returs the string names of <paramref name="sensorType"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sensorType"></param>
        /// <returns></returns>
        private List<string> ProcessSensors<T>(SensorType sensorType) where T : ISensor, new()
        {
            T sensors = new T();
            if (sensors.SensorTypes.TryGetValue(sensorType, out var sensorTypes))
                return sensorTypes;
            else
                return null;
        }

        /// <summary>
        /// Deserialize the value of the given <paramref name="attributeName"/> from a <paramref name="jsonObject"/>.
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        private double? GetAttributeValue(JObject jsonObject, string attributeName)
        {
            var attributeToken = jsonObject["values"][0]["attributes"]?.FirstOrDefault(attr => attr["attrName"]?.ToString() == attributeName);
            if (attributeToken == null)
                return null;

            return attributeToken["values"].FirstOrDefault().Value<double>();
        }
        
        /// <summary>
        /// Prints out the error <paramref name="message"/> of a failed request.
        /// </summary>
        /// <param name="message"></param>
        private void DisplayErrorMessage(string message)
        {
            Debug.Log(message);
            tokenSource.Cancel();
            OnError?.Invoke(message);
        }
    }
}