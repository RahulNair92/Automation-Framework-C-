using RestSharp;
using System;

namespace UdemyRestSharpNetCore.Base
{
    public class Settings
    {
        public Uri? BaseUrl { get; set; }
        public RestResponse Response { get; set; }
        public RestRequest? Request { get; set; }
        public RestClient? RestClient { get; set; } 
    }
}
