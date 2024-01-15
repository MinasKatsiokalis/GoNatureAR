using RestSharp;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Requests
{
    public interface IResponse
    {
        public IRestResponse RestResponse { get; set; }
        public JObject[] ResponseData { get; set; }
    }
}
