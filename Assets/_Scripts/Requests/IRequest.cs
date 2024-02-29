using RestSharp;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Requests
{
    public interface IRequest
    {
        public RestClient RestClient { set; get; }
        public RestRequest RestRequest { set; get; }
    }
}
