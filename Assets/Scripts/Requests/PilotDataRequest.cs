using Newtonsoft.Json.Linq;
using RestSharp;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GoNatureAR.Requests
{
    public class PilotDataRequest : Request
    {
        private Pilot pilot;

        private const string _domain = ".varcities.eu/v2/entities";
        private const string _prefix = "https://varcities-api.";
        private string pilotName;
        private string url;
        public string URL
        {
            get { return url; }
            set { url = value; }
        }

        public Request Request;

        public PilotDataRequest(string token, Pilot pilot)
        {   
            this.pilot = pilot;
            this.pilotName = pilot.ToString().Replace("_", "-");
            this.url = $"{_prefix}{pilotName}{_domain}";

            RestClient = new RestClient(url);
            RestClient.Timeout = -1;
            RestClient.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            RestRequest = new RestRequest(Method.GET);
            RestRequest.AddHeader("Fiware-Service", "openiot");
            RestRequest.AddHeader("X-Auth-Token", token);
            RestRequest.AddHeader("Fiware-ServicePath", "/");
        }

        public async Task<Response> ExecuteDataRequest()
        {
            Response response = new Response();
            response.RestResponse = await RestClient.ExecuteAsync(RestRequest);

            JArray jsonArray = JArray.Parse(response.RestResponse.Content);
            JObject[] data = new JObject[jsonArray.Count];

            for(int i= 0; i<jsonArray.Count;i++)
                data[i] = (JObject)jsonArray[i];

            response.ResponseData = data;

            return response;
        }
    }
}
