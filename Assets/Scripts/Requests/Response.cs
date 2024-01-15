using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;

namespace GoNatureAR.Requests
{
    public class Response: IResponse
    {   
        private IRestResponse restResponse;
        public IRestResponse RestResponse
        {
            get { return restResponse; }
            set { restResponse = value; }
        }

        private JObject[] responseData;
        public JObject[] ResponseData
        {
            get { return responseData; }
            set { responseData = value; }
        }

        public Response()
        {
            restResponse = null;
            responseData = null;
        }
    }
}
