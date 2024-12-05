using NUnit.Framework;
using System.Net;
using Newtonsoft.Json;
using NLog;
using static RestAssured.Dsl;
using Microsoft.Extensions.Logging.Configuration;
using System.Diagnostics;

namespace MathFunctionsTestAutomation.Tests
{
    public class OpenForBusinessTests
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        string BaseUrl = "http://localhost:7000/api";
    
        [SetUp]
        public void Setup()
        {
            Logger.Info("Testing Base URL: " + BaseUrl);
        }

        [Test]
        [Category("Smoke")]
        public void OpenForBusiness()
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info("Starting Test: OpenForBusiness");
            bool testPassed = true;

            try
            {
                var response = Given()
                    .When()
                    .Get($"{BaseUrl}/open")
                    .Then()
                    .Extract()
                    .Response();

                var statusCode = (int)response.StatusCode;
                var responseContent = response.Content.ReadAsStringAsync().Result;

                if (responseContent != "open for business")
                {
                    Logger.Error($"Expected response text 'open for business' but got '{responseContent}'");
                    Assert.Fail($"Expected response text 'open for business' but got '{responseContent}'");
                }
            }
            catch (AssertionException)
            {
                Logger.Error("Test OpenForBusiness: Failed");
                testPassed = false;
                throw;
            }
            catch (Exception exception)
            {
                Logger.Error("Test OpenForBusiness: Unexpected Exception: " + exception.ToString());
                testPassed = false;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                if (testPassed)
                {
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Logger.Info("Test OpenForBusiness: Passed");
                    Logger.Info($"Test OpenForBusiness: Test run time: {elapsedMilliseconds} ms\n");
                } else {
                    Logger.Info("Test OpenForBusiness: Passed\n");
                }
            }
        }
   }
}