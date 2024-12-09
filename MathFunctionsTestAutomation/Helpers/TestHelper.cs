using System.Net;
using Newtonsoft.Json;
using NLog;
using static RestAssured.Dsl;
using System.Diagnostics;
using NUnit.Framework;

namespace MathFunctionsTestAutomation.Helpers
{
    public static class TestHelper
    {
        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static async Task ExecuteTest(string testName, string baseUrl, string endpoint, object requestBody, int expectedHTTPResponseCode, Action<string> assertAction, Func<string, object, int, Task<string>> sendRequest)
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Info($"Starting Test: {testName}");
            bool testPassed = true;

            try
            {
                var responseContent = await sendRequest($"{baseUrl}/{endpoint}", requestBody, expectedHTTPResponseCode);
                assertAction(responseContent);
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

        public static Task ExecutePostTest(string testName, string baseUrl, string endpoint, object requestBody, int expectedHTTPResponseCode, Action<string> assertAction)
        {
            return ExecuteTest(testName, baseUrl, endpoint, requestBody, expectedHTTPResponseCode, assertAction, SendPostRequest);
        }

        public static Task ExecuteGetTest(string testName, string baseUrl, string endpoint, object requestBody, int expectedHTTPResponseCode, Action<string> assertAction)
        {
            return ExecuteTest(testName, baseUrl, endpoint, requestBody, expectedHTTPResponseCode, assertAction, SendGetRequest);
        }

        public static async Task<string> SendPostRequest(string url, object requestBody, int expectedHTTPResponseCode)
        {
            var response = Given()
                .ContentType("application/json")
                .Body(JsonConvert.SerializeObject(requestBody))
                .When()
                .Post(url)
                .Then()
                .StatusCode(expectedHTTPResponseCode)
                .Extract()
                .Response();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        public static async Task<string> SendGetRequest(string url, object requestBody, int expectedHTTPResponseCode)
        {
            var response = Given()
                .ContentType("application/json")
                .Body(JsonConvert.SerializeObject(requestBody))
                .When()
                .Get(url)
                .Then()
                .StatusCode(expectedHTTPResponseCode)
                .Extract()
                .Response();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        public static void LogTestResult(string testName, bool testPassed, long elapsedMilliseconds)
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
    }
}