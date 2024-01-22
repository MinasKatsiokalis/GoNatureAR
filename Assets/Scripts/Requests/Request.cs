using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;
using UnityEngine;


namespace GoNatureAR.Requests
{
    public class Request: IRequest
    {
        private string url;
        public string URL
        {
            get { return url; }
            set { url = value; }
        }

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

        public Request(string url, Method method)
        {   
            this.url = url;
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

        public virtual async Task<Response> ExecuteRequestAsync(Action<string> onFailure)
        {
            Response response = new Response();
            response.RestResponse = await RestClient.ExecuteAsync(RestRequest);
            if (response.RestResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Debug.Log(response.RestResponse.StatusCode);
                Debug.Log(RestClient.BaseUrl);

                onFailure?.Invoke($"Request Error: {response.RestResponse.ErrorMessage}");
                return null;
            }
            response.ResponseData = response.RestResponse.Content;

            return response;
        }
    }
}
