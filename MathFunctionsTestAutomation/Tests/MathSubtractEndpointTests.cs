using NUnit.Framework;
using System.Net;
using Newtonsoft.Json;
using NLog;
using static RestAssured.Dsl;
using Microsoft.Extensions.Logging.Configuration;
using System.Diagnostics;

namespace MathFunctionsTestAutomation.Tests
{
    public class MathSubtractEndpointTests
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        string BaseUrl = "http://localhost:7000/api";
    
        [SetUp]
        public void Setup()
        {
            Logger.Info("Testing Base URL: " + BaseUrl);
        }
    
        [Test]
        [Category("P0")]
        public async Task Subtract_200_ShouldReturnCorrectDifference()
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info("Starting Test: Subtract_ShouldReturnCorrectDifference");
            bool testPassed = true;

            var requestBody = new
            {
                numbers = new[] { 1, 2, 3, 4 }
            };

            try {
                var response = Given()
                    .ContentType("application/json")
                    .Body(JsonConvert.SerializeObject(requestBody))
                    .When()
                    .Post($"{BaseUrl}/math/subtract")
                    .Then()
                    .StatusCode(200)
                    .Extract()
                    .Response();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseBody = JsonConvert.DeserializeObject<dynamic>(responseContent);

                if (responseBody == null)
                {
                    Logger.Error($"Expected responseBody but responseBody was null.");
                    Assert.Fail("responseBody is null");
                } else {
                    Assert.That((int)responseBody.result, Is.EqualTo(-8));
                }

            } catch(AssertionException) {
                Logger.Info("Test Subtract_ShouldReturnCorrectDifference: Failed.");
                testPassed = false;
            } catch(Exception exception) {
                Logger.Error("Test Subtract_ShouldReturnCorrectDifference: Unexpected Exception: " + exception.ToString());
                testPassed = false;
                throw;
            } finally {
                stopwatch.Stop();
                if (testPassed)
                {
                    Logger.Info("Test Subtract_ShouldReturnCorrectDifference: Passed.");
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Logger.Info($"Test Subtract_ShouldReturnCorrectDifference: Test run time: {elapsedMilliseconds} ms\n");
                } else {
                    Logger.Info("Test Subtract_ShouldReturnCorrectDifference: Failed.\n");
                } 
            }
        }

        [Test]
        [Category("P1")]
        public async Task Subtract_400_InvalidJSON()
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info("Starting Test: Subtract_400_InvalidJSON");
            bool testPassed = true;

            var requestBody = new
            {
                hello = "there"
            };

            try {
                var response = Given()
                    .ContentType("application/json")
                    .Body(JsonConvert.SerializeObject(requestBody))
                    .When()
                    .Post($"{BaseUrl}/math/subtract")
                    .Then()
                    .StatusCode(400)
                    .Extract()
                    .Response();

                var responseContent = await response.Content.ReadAsStringAsync();

                if (responseContent == null)
                {
                    Logger.Error($"Expected responseBody but responseBody was null.");
                    Assert.Fail("responseBody is null");
                } else {
                    Assert.That(responseContent, Is.EqualTo("The numbers list cannot be empty."));
                }

            } catch(AssertionException) {
                Logger.Info("Test Subtract_400_InvalidJSON: Failed.");
                testPassed = false;
            } catch(Exception exception) {
                Logger.Error("Test Subtract_400_InvalidJSON: Unexpected Exception: " + exception.Message.ToString());
                testPassed = false;
                throw;
            } finally {
                stopwatch.Stop();
                if (testPassed)
                {
                    Logger.Info("Test Subtract_400_InvalidJSON: Passed.");
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Logger.Info($"Test Subtract_400_InvalidJSON: Test run time: {elapsedMilliseconds} ms\n");
                } else {
                    Logger.Info("Test Subtract_400_InvalidJSON: Failed.");
                }   
            }
        }

        [Test]
        [Category("P2")]
        public async Task Subtract_400_GarbageInput()
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info("Starting Test: Subtract_400_GarbageInput");
            bool testPassed = true;

            String body = "This is garbage input";

            try {
                var response = Given()
                    .ContentType("application/json")
                    .Body(JsonConvert.SerializeObject(body))
                    .When()
                    .Post($"{BaseUrl}/math/subtract")
                    .Then()
                    .StatusCode(400)
                    .Extract()
                    .Response();

                var responseContent = await response.Content.ReadAsStringAsync();

                if (responseContent == null)
                {
                    Logger.Error($"Expected responseBody but responseBody was null.");
                    Assert.Fail("responseBody is null");
                } else {
                    Assert.That(responseContent, Does.Contain("The JSON value could not be converted to MathFunctions.Models.MathRequest."));
                }

            } catch(AssertionException) {
                Logger.Info("Test Subtract_400_GarbageInput: Failed.");
                testPassed = false;
            } catch(Exception exception) {
                Logger.Error("Test Subtract_400_GarbageInput: Unexpected Exception: " + exception.Message.ToString());
                testPassed = false;
                throw;
            } finally {
                stopwatch.Stop();
                if (testPassed)
                {
                    Logger.Info("Test Subtract_400_GarbageInput: Passed.");
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Logger.Info($"Test Subtract_400_GarbageInput: Test run time: {elapsedMilliseconds} ms\n");
                } else {
                    Logger.Info("Test Subtract_400_GarbageInput: Failed.");
                }   
            }
        }
   }
}