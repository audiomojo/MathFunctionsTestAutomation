using NUnit.Framework;
using System.Net;
using Newtonsoft.Json;
using NLog;
using static RestAssured.Dsl;
using Microsoft.Extensions.Logging.Configuration;
using System.Diagnostics;

namespace MathFunctionsTestAutomation.Tests
{
    public class MathAddEndpointTests
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
        public async Task Add_200_ShouldReturnCorrectSum()
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info("Starting Test: Add_ShouldReturnCorrectSum");
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
                    .Post($"{BaseUrl}/math/add")
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
                    Assert.That((int)responseBody.result, Is.EqualTo(10));
                }

            } catch(AssertionException) {
                Logger.Info("Test Add_ShouldReturnCorrectSum: Failed.");
                testPassed = false;
            } catch(Exception exception) {
                Logger.Error("Test Add_ShouldReturnCorrectSum: Unexpected Exception: " + exception.Message.ToString());
                testPassed = false;
                throw;
            } finally {
                stopwatch.Stop();
                if (testPassed)
                {
                    Logger.Info("Test Add_ShouldReturnCorrectSum: Passed.");
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Logger.Info($"Test Add_ShouldReturnCorrectSum: Test run time: {elapsedMilliseconds} ms\n");
                } else {
                    Logger.Info("Test Add_ShouldReturnCorrectSum: Failed.");
                }   

            }
        }

        [Test]
        [Category("P1")]
        public async Task Add_400_InvalidJSON()
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info("Starting Test: Add_400InvalidJSON");
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
                    .Post($"{BaseUrl}/math/add")
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
                Logger.Info("Test Add_400InvalidJSON: Failed.");
                testPassed = false;
            } catch(Exception exception) {
                Logger.Error("Test Add_400InvalidJSON: Unexpected Exception: " + exception.Message.ToString());
                testPassed = false;
                throw;
            } finally {
                stopwatch.Stop();
                if (testPassed)
                {
                    Logger.Info("Test Add_400InvalidJSON: Passed.");
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Logger.Info($"Test Add_400InvalidJSON: Test run time: {elapsedMilliseconds} ms\n");
                } else {
                    Logger.Info("Test Add_400InvalidJSON: Failed.");
                }   
            }
        }

        [Test]
        [Category("P2")]
        public async Task Add_400_GarbageInput()
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info("Starting Test: Add_400InvalidJSON");
            bool testPassed = true;

            String body = "This is garbage input";

            try {
                var response = Given()
                    .ContentType("application/json")
                    .Body(JsonConvert.SerializeObject(body))
                    .When()
                    .Post($"{BaseUrl}/math/add")
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
                Logger.Info("Test Add_400InvalidJSON: Failed.");
                testPassed = false;
            } catch(Exception exception) {
                Logger.Error("Test Add_400InvalidJSON: Unexpected Exception: " + exception.Message.ToString());
                testPassed = false;
                throw;
            } finally {
                stopwatch.Stop();
                if (testPassed)
                {
                    Logger.Info("Test Add_400InvalidJSON: Passed.");
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Logger.Info($"Test Add_400InvalidJSON: Test run time: {elapsedMilliseconds} ms\n");
                } else {
                    Logger.Info("Test Add_400InvalidJSON: Failed.");
                }   
            }
        }
   }
}