using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using UdemyRestSharpNetCore.Base;
using System;
using System.Configuration;
using System.IO;
using TechTalk.SpecFlow;
using RestSharp;
using Newtonsoft.Json;
using System.Linq;
using AventStack.ExtentReports.MarkupUtils;
using TechTalk.SpecFlow.Infrastructure;

namespace UdemyRestSharpNetCore.Hooks
{
    [Binding]
    public class TestInitialize
    {
  
        private readonly ScenarioContext? _scenarioContext;
        private Settings _settings;
        private ISpecFlowOutputHelper _logger;

        public TestInitialize(Settings settings, FeatureContext featureContext, ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _scenarioContext = scenarioContext;
            _settings = settings;
            _logger = specFlowOutputHelper;
        }

        [BeforeScenario]
        public void TestSetup()
        {
            _settings.BaseUrl = new Uri("https://reqres.in/");
            _settings.RestClient = new RestClient(_settings.BaseUrl);
        }


        [AfterStep]
        public void InsertReportingSteps()
        {

            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            var logDetails = LogAPIDetails(_settings);
            

            if (_scenarioContext.TestError == null)
            {
                createLogFileattachToReport(AttachmentsType.Response, logDetails.responseToLog);
      

            }
            else if (_scenarioContext.TestError != null)
            { 
                createLogFileattachToReport(AttachmentsType.Response, logDetails.responseToLog);
                createLogFileattachToReport(AttachmentsType.Request, logDetails.responseToLog);
                createLogFileattachToReport(AttachmentsType.Stacktrace, logDetails.requestToLog);

            }
        }

        private APILogDetails LogAPIDetails(Settings _settings)

        {
            var requestToLog = new
            {
                resource = _settings.Request.Resource,
                parameters = _settings.Request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                method = _settings.Request.Method.ToString(),
                uri = _settings.RestClient.BuildUri(_settings.Request),
            };

            var responseToLog = new
            {
                statusCode = _settings.Response.StatusCode,
                content = _settings.Response.Content,
                headers = _settings.Response.Headers,
                responseUri = _settings.Response.ResponseUri,
                errorMessage = _settings.Response.ErrorMessage,
            };

            APILogDetails logDetails = new APILogDetails();
            logDetails.requestToLog = JsonConvert.SerializeObject(requestToLog);
            logDetails.responseToLog = JsonConvert.SerializeObject(responseToLog);

            return logDetails;

        }

        private void createLogFileattachToReport(AttachmentsType type, String content)
        {
            _logger.WriteLine($"{type.ToString()}: ");
            if (type.Equals(AttachmentsType.Screenshot))
            {
                //ToDo winapp driver or selenium screenshot. Probably use Ashot
            }
            else
            {
                String response_file = Path.GetRandomFileName();
                File.WriteAllText(response_file, content);
                _logger.AddAttachment(Path.GetFullPath(response_file));
            }
        }

     
        public class APILogDetails
        {
            public string requestToLog { get; set; }
            public string responseToLog { get; set; }
            public string requestTime { get; set; }
        }

        private enum AttachmentsType
        {
            Request, Response, Screenshot,
            Stacktrace
        }



    }
}
   
