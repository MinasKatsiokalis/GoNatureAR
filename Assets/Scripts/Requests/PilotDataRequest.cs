using Newtonsoft.Json.Linq;
using RestSharp;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Policy;
using System.Linq;

namespace GoNatureAR.Requests
{
    public class PilotDataRequest : Request
    {
        public PilotDataRequest(string token, Pilot pilot, string sensorType) : base(PrepareUrl(pilot,sensorType), Method.GET)
        {
            AddHeaders(token);
        }

        private void AddHeaders(string token)
        {
            RestRequest.AddHeader("Fiware-Service", "openiot");
            RestRequest.AddHeader("X-Auth-Token", token);
            RestRequest.AddHeader("Fiware-ServicePath", "/");
        }

        private static string PrepareUrl(Pilot pilot, string sensorType)
        {
            var domain = $".varcities.eu/v2/types/{sensorType}/value?&lastN=10";
            var prefix = "https://varcities-api.";

            var pilotName = pilot.ToString().Replace("_", "-");
            string url = $"{prefix}{pilotName}{domain}";

            if (pilot == Pilot.gzira)
            {
                domain = $"213.165.170.189:1030/v2/types/{sensorType}/value?&lastN=10";
                prefix = "https://";
                url = $"{prefix}{domain}";
            }
            return url ;
        }
 
    }
}
