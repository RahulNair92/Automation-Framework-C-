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
    
    public class TestInitialize1
    {
        //Global Variable for Extend report
        private static ExtentTest? featureName;
        private static ExtentTest? scenario;
        private static ExtentReports? extent;

        private readonly FeatureContext? _featureContext;
        private readonly ScenarioContext? _scenarioContext;
        private Settings _settings;
        private ISpecFlowOutputHelper _logger;

        public TestInitialize1(Settings settings, FeatureContext featureContext, ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
            _settings = settings;
            _logger = specFlowOutputHelper;
        }

     //   [BeforeScenario]
        public void TestSetup()
        {
            _settings.BaseUrl = new Uri("https://reqres.in/");
            _settings.RestClient = new RestClient(_settings.BaseUrl);
        }

       // [BeforeTestRun]
        public static void InitializeReport()
        {
            string file = "ExtentReport.html";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            //Initialize Extent report before test starts
            var htmlReporter = new ExtentHtmlReporter(path);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            //Attach report to reporter
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        //[BeforeFeature]
        public static void CreateFeatureFile()
        {

        }

        //[AfterTestRun]
        public static void TearDownReport()
        {
            //Flush report once test completes
            extent.Flush();
        }

        //[AfterStep]
        public void InsertReportingSteps()
        {

            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();

            var logDetails = LogAPIDetails(_settings, 0);
            

            if (_scenarioContext.TestError == null)
            {
                
                    _logger.WriteLine("Response: ");
                String response_file = Path.GetRandomFileName();
                File.WriteAllText(response_file,  logDetails.responseToLog);
                _logger.AddAttachment(Path.GetFullPath(response_file));


                /* //scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Info(MarkupHelper.CreateCodeBlock(logDetails.requestToLog + logDetails.responseToLog, CodeLanguage.Json));
                 else if (stepType == "When")
                     _specFlowOutputHelper.WriteLine($"Response: {logDetails.responseToLog}");
                 //  scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Info(MarkupHelper.CreateCodeBlock(logDetails.requestToLog, CodeLanguage.Json));
                 else if (stepType == "Then")
                     _specFlowOutputHelper.WriteLine($"Response: {logDetails.responseToLog}");
                 //scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Info(MarkupHelper.CreateCodeBlock(logDetails.requestToLog, CodeLanguage.Json));
                 else if (stepType == "And")
                     _specFlowOutputHelper.WriteLine($"Response: {logDetails.responseToLog}");
                // scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text).Info(MarkupHelper.CreateCodeBlock(logDetails.requestToLog, CodeLanguage.Json));*/
            }
            else if (_scenarioContext.TestError != null)
            {
                //To Do timing of api is hardcoded at 0 This needs to be corrected
                // var logDetails = LogAPIDetails(_settings, 0);

                _logger.WriteLine("Response: ");
                String response_file = Path.GetRandomFileName();
                File.WriteAllText(response_file, logDetails.responseToLog);
                _logger.AddAttachment(Path.GetFullPath(response_file));

                _logger.WriteLine($"Response: {logDetails.responseToLog}");
                String error_file = Path.GetRandomFileName();
                File.WriteAllText(error_file,_scenarioContext.TestError.StackTrace);
                _logger.AddAttachment(Path.GetFullPath(error_file));




            }


            /*scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text)
                                .Fail(_scenarioContext.TestError.Message).
                               .Info(logDetails.requestTime);
                            scenario.CreateNode("Request", "Request details")
                              .Info(MarkupHelper.CreateCodeBlock(logDetails.requestToLog, CodeLanguage.Json));*/

        }


        //[BeforeScenario]
        public void Initialize()
        {

            //Get feature Name
            if (featureName == null)
                featureName = extent.CreateTest<Feature>(_featureContext.FeatureInfo.Title);

            //Create dynamic scenario name
            scenario = featureName.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
        }

        private APILogDetails LogAPIDetails(Settings _settings, long durationMs)

        {


            var requestToLog = new
            {
                resource = _settings.Request.Resource,
                // Parameters are custom anonymous objects in order to have the parameter type as a nice string
                // otherwise it will just show the enum value
                parameters = _settings.Request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                // ToString() here to have the method as a nice string otherwise it will just show the enum value
                method = _settings.Request.Method.ToString(),
                // This will generate the actual Uri used in the request
                uri = _settings.RestClient.BuildUri(_settings.Request),
            };

            var responseToLog = new
            {
                statusCode = _settings.Response.StatusCode,
                content = _settings.Response.Content,
                headers = _settings.Response.Headers,
                // The Uri that actually responded (could be different from the requestUri if a redirection occurred)
                responseUri = _settings.Response.ResponseUri,
                errorMessage = _settings.Response.ErrorMessage,
            };

            APILogDetails logDetails = new APILogDetails();
            logDetails.requestToLog = JsonConvert.SerializeObject(requestToLog);
            logDetails.responseToLog = JsonConvert.SerializeObject(responseToLog);
            logDetails.requestTime = string.Format("Request completed in {0} ms",durationMs);

            return logDetails;

        }

     
        public class APILogDetails
        {
            public string requestToLog { get; set; }
            public string responseToLog { get; set; }
            public string requestTime { get; set; }
        }



    }
}
   
