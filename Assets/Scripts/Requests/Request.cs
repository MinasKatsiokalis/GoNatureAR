using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;

namespace GoNatureAR.Requests
{
    public class Request: IRequest
    {   
        private RestClient restClient;
        public RestClient RestClient
        {
            get { return restClient; }
            set { restClient = value; }
        }

        private RestRequest restRequest;
        public RestRequest RestRequest
        {
            get { return restRequest; }
            set { restRequest = value; }
        }

        public Request(string url, string token, Method method)
        {
            restClient = new RestClient(url);
            restClient.Timeout = -1;
            restClient.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            restRequest = new RestRequest(method);
            restRequest.AddHeader("Fiware-Service", "openiot");
            restRequest.AddHeader("X-Auth-Token", token);
            restRequest.AddHeader("Fiware-ServicePath", "/");
        }

        public Request(string url, Method method)
        {
            restClient = new RestClient(url);
            restClient.Timeout = -1;
            restClient.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            restRequest = new RestRequest(method);
        }

        public Request()
        {
            restClient = new RestClient();
            restRequest = new RestRequest();
        }
    }
}
