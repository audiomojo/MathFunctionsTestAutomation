using NUnit.Framework;
using System.Net;
using Newtonsoft.Json;
using NLog;
using static RestAssured.Dsl;
using Microsoft.Extensions.Logging.Configuration;
using System.Diagnostics;

namespace MathFunctionsTestAutomation.Tests
{
    public class MathFibonacciEndpointTests
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
        public async Task Fibonacci_200_CountOf1()
        {
            await TestFibonacciEndpoint(new { Count = 1 }, HttpStatusCode.OK, "0", "Fibonacci_200_CountOf1");
        }

        [Test]
        [Category("P1")]
        public async Task Fibonacci_400_CountOf0()
        {
            await TestFibonacciEndpoint(new { Count = 0 }, HttpStatusCode.BadRequest, "The count must be between 1 and 100.", "Fibonacci_400_CountOf0");
        }

        [Test]
        [Category("P1")]
        public async Task Fibonacci_400_CountOfNegativeNumber()
        {
            await TestFibonacciEndpoint(new { Count = -5 }, HttpStatusCode.BadRequest, "The count must be between 1 and 100.", "Fibonacci_400_CountOfNegativeNumber");
        }

        [Test]
        [Category("P0")]
        public async Task Fibonacci_200_CountOf100()
        {
            var expectedFibonacci = string.Join(",", GenerateFibonacci(100));
            await TestFibonacciEndpoint(new { Count = 100 }, HttpStatusCode.OK, expectedFibonacci, "Fibonacci_200_CountOf100");
        }

        [Test]
        [Category("P1")]
        public async Task Fibonacci_400_CountOf101()
        {
            await TestFibonacciEndpoint(new { Count = 101 }, HttpStatusCode.BadRequest, "The count must be between 1 and 100.", "Fibonacci_400_CountOf101");
        }

        [Test]
        [Category("P1")]
        public async Task Fibonacci_400_BadJson()
        {
            await TestFibonacciEndpoint("{ \"Bad\": \"JSON\" }", HttpStatusCode.BadRequest, "Invalid JSON format.", "Fibonacci_400_BadJson");
        }

        [Test]
        [Category("P1")]
        public async Task Fibonacci_400_GarbageRequestBody()
        {
            await TestFibonacciEndpoint("this is garbage", HttpStatusCode.BadRequest, "Invalid request body.", "Fibonacci_400_GarbageRequestBody");
        }

        [Test]
        [Category("P1")]
        public async Task Fibonacci_400_EmptyRequestBody()
        {
            await TestFibonacciEndpoint("{}", HttpStatusCode.BadRequest, "The count must be between 1 and 100.", "Fibonacci_400_EmptyRequestBody");
        }

        private async Task TestFibonacciEndpoint(object requestBody, HttpStatusCode expectedStatusCode, string expectedResponse, string testName)
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info($"Starting Test: {testName}");
            bool testPassed = true;

            try
            {
                var response = await SendPostRequest($"{BaseUrl}/math/fibonacci", requestBody);
                var responseContent = await response.Content.ReadAsStringAsync();

                Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode), $"Expected status code {expectedStatusCode} but got {response.StatusCode}.");
                Assert.That(responseContent, Is.EqualTo(expectedResponse), $"Expected response '{expectedResponse}' but got '{responseContent}'.");
            }
            catch (AssertionException ex)
            {
                Logger.Error($"Test {testName}: Assertion failed. {ex.Message}");
                testPassed = false;
            }
            catch (Exception ex)
            {
                Logger.Error($"Test {testName}: Unexpected exception. {ex}");
                testPassed = false;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                LogTestResult(testName, testPassed, stopwatch.ElapsedMilliseconds);
            }
        }

        private async Task<HttpResponseMessage> SendPostRequest(string url, object requestBody)
        {
            return await Given()
                .ContentType("application/json")
                .Body(JsonConvert.SerializeObject(requestBody))
                .When()
                .Post(url)
                .Then()
                .Extract()
                .Response();
        }

        private void LogTestResult(string testName, bool testPassed, long elapsedMilliseconds)
        {
            if (testPassed)
            {
                Logger.Info($"Test {testName}: Passed.");
                Logger.Info($"Test {testName}: Test run time: {elapsedMilliseconds} ms\n");
            }
            else
            {
                Logger.Info($"Test {testName}: Failed.\n");
            }
        }

        private List<BigInteger> GenerateFibonacci(int count)
        {
            var fibonacciNumbers = new List<BigInteger> { 0, 1 };
            for (int i = 2; i < count; i++)
            {
                fibonacciNumbers.Add(fibonacciNumbers[i - 1] + fibonacciNumbers[i - 2]);
            }
            return fibonacciNumbers.Take(count).ToList();
        }
    }
}
