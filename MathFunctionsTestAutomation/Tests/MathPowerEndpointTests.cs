using NUnit.Framework;
using System.Net;
using Newtonsoft.Json;
using NLog;
using static RestAssured.Dsl;
using Microsoft.Extensions.Logging.Configuration;
using System.Diagnostics;

namespace MathFunctionsTestAutomation.Tests
{
    public class MathPowerEndpointTests
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
        public async Task Power_200_ShouldReturnCorrectResult()
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info("Starting Test: Power_ShouldReturnCorrectResult");
            bool testPassed = true;

            var requestBody = new
            {
                Base = 2,
                Power = 10
            };

            try {
                var response = Given()
                    .ContentType("application/json")
                    .Body(JsonConvert.SerializeObject(requestBody))
                    .When()
                    .Post($"{BaseUrl}/math/power")
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
                    Assert.That((int)responseBody.result, Is.EqualTo(1024));
                }

            } catch(AssertionException) {
                Logger.Info("Test Power_ShouldReturnCorrectResult: Failed.");
                testPassed = false;
            } catch(Exception exception) {
                Logger.Error("Test Power_ShouldReturnCorrectResult: Unexpected Exception: " + exception.ToString());
                testPassed = false;
                throw;
            } finally {
                stopwatch.Stop();
                if (testPassed)
                {
                    Logger.Info("Test Power_ShouldReturnCorrectResult: Passed.");
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Logger.Info($"Test Power_ShouldReturnCorrectResult: Test run time: {elapsedMilliseconds} ms\n");
                } else {
                    Logger.Info("Test Power_ShouldReturnCorrectResult: Failed.\n");
                } 
            }
        }

        [Test]
        [Category("P1")]
        public async Task Power_400_BadRequest()
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info("Starting Test: Power_400BadRequest");
            bool testPassed = true;

            var requestBody = new
            {
                Base = 2,
                Power = 10000
            };

            try {
                var response = Given()
                    .ContentType("application/json")
                    .Body(JsonConvert.SerializeObject(requestBody))
                    .When()
                    .Post($"{BaseUrl}/math/power")
                    .Then()
                    .StatusCode(400)
                    .Extract()
                    .Response();

                var responseContent = await response.Content.ReadAsStringAsync();

                if (responseContent == null)
                {
                    Logger.Error($"Expected valid responseContent but responseContent was null.");
                    Assert.Fail("responseContent is null");
                } else {
                    Assert.That(responseContent, Is.EqualTo("The result is too large to be represented as a valid JSON number."));
                }

            } catch(AssertionException) {
                Logger.Info("Test Power_400BadRequest: Failed.");
                testPassed = false;
            } catch(Exception exception) {
                Logger.Error("Test Power_400BadRequest: Unexpected Exception: " + exception.ToString());
                testPassed = false;
                throw;
            } finally {
                stopwatch.Stop();
                if (testPassed)
                {
                    Logger.Info("Test Power_400BadRequest: Passed.");
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Logger.Info($"Test Power_400BadRequest: Test run time: {elapsedMilliseconds} ms\n");
                } else {
                    Logger.Info("Test Power_400BadRequest: Failed.\n");
                } 
            }
        }
        
        [Test]
        [Category("P1")]
        public async Task Power_400_InvalidJSON()
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info("Starting Test: Power_400_InvalidJSON");
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
                    .Post($"{BaseUrl}/math/power")
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
                    Assert.That(responseContent, Is.EqualTo("0 to the power of 0 is undefined."));
                }

            } catch(AssertionException) {
                Logger.Info("Test Power_400_InvalidJSON: Failed.");
                testPassed = false;
            } catch(Exception exception) {
                Logger.Error("Test Power_400_InvalidJSON: Unexpected Exception: " + exception.Message.ToString());
                testPassed = false;
                throw;
            } finally {
                stopwatch.Stop();
                if (testPassed)
                {
                    Logger.Info("Test Power_400_InvalidJSON: Passed.");
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Logger.Info($"Test Power_400_InvalidJSON: Test run time: {elapsedMilliseconds} ms\n");
                } else {
                    Logger.Info("Test Power_400_InvalidJSON: Failed.");
                }   
            }
        }

        [Test]
        [Category("P2")]
        public async Task Power_400_GarbageInput()
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info("Starting Test: Power_400_GarbageInput");
            bool testPassed = true;

            String body = "This is garbage input";

            try {
                var response = Given()
                    .ContentType("application/json")
                    .Body(JsonConvert.SerializeObject(body))
                    .When()
                    .Post($"{BaseUrl}/math/power")
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
                    Assert.That(responseContent, Does.Contain("The JSON value could not be converted to PowerRequest"));
                }

            } catch(AssertionException) {
                Logger.Info("Test Power_400_GarbageInput: Failed.");
                testPassed = false;
            } catch(Exception exception) {
                Logger.Error("Test Power_400_GarbageInput: Unexpected Exception: " + exception.Message.ToString());
                testPassed = false;
                throw;
            } finally {
                stopwatch.Stop();
                if (testPassed)
                {
                    Logger.Info("Test Power_400_GarbageInput: Passed.");
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Logger.Info($"Test Power_400_GarbageInput: Test run time: {elapsedMilliseconds} ms\n");
                } else {
                    Logger.Info("Test Power_400_GarbageInput: Failed.");
                }   
            }
        }
   }
}