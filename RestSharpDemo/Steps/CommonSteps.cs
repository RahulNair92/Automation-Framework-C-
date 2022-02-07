using RestSharp;
using RestSharp.Authenticators;
using UdemyRestSharpNetCore.Base;
using UdemyRestSharpNetCore.Utilities;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using NUnit.Framework;
using System.Net;
using TechTalk.SpecFlow.Infrastructure;

namespace UdemyRestSharpNetCore.Steps
{

    [Binding]
    public class CommonSteps
    {

        //Context injection
        private Settings _settings;
        private ISpecFlowOutputHelper _logger;
        public CommonSteps(Settings settings, ISpecFlowOutputHelper logger)
        {
            _settings = settings;
            _logger = logger;
        }

        [Given(@"I get JWT authentication of User with following details")]
        public void GivenIGetJWTAuthenticationOfUserWithFollowingDetails(Table table)
        {
            dynamic data = table.CreateDynamicInstance();

            _settings.Request = new RestRequest("api/login", Method.Post)
                .AddJsonBody(new { email = data.Email, password = data.Password });

            //Get access token
            _settings.Response = _settings.RestClient.ExecutePostAsync(_settings.Request).GetAwaiter().GetResult();
            /* var access_token = _settings.Response.GetResponseObject("token");

             //Authentication
             var authenticator = new JwtAuthenticator(access_token);
             _settings.RestClient.Authenticator = authenticator;

             Assert.Equals(_settings.Response.StatusCode, HttpStatusCode.OK);*/
        }

        [StepDefinition(@"I get JWT authentication of User with following detailss")]
        public void WhenIGetJWTAuthenticationOfUserWithFollowingDetailss(Table table)
        {
            _logger.WriteLine("this is a log written inside step definition");
            dynamic data = table.CreateDynamicInstance();

            _settings.Request = new RestRequest("api/login", Method.Post)
                .AddJsonBody(new { email = data.Email, password = data.Password });

            //Get access token
            _settings.Response = _settings.RestClient.ExecutePostAsync(_settings.Request).GetAwaiter().GetResult();

        }



        [Given(@"I get JWT authentication of User with following detailsss")]
        public void GivenIGetJWTAuthenticationOfUserWithFollowingDetailsss(Table table)
        {
            dynamic data = table.CreateDynamicInstance();

            _settings.Request = new RestRequest("api/login", Method.Post)
                .AddJsonBody(new { email = data.Email, password = data.Password });

            //Get access token
            _settings.Response = _settings.RestClient.ExecutePostAsync(_settings.Request).GetAwaiter().GetResult();
            /* var access_token = _settings.Response.GetResponseObject("token");

             //Authentication
             var authenticator = new JwtAuthenticator(access_token);
             _settings.RestClient.Authenticator = authenticator;

             Assert.Equals(_settings.Response.StatusCode, HttpStatusCode.OK);*/
        }





    }
}
