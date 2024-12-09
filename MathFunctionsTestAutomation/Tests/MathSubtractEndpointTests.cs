using NUnit.Framework;
using System.Net;
using Newtonsoft.Json;
using NLog;
using static RestAssured.Dsl;
using Microsoft.Extensions.Logging.Configuration;
using System.Diagnostics;
using MathFunctionsTestAutomation.Helpers;

namespace MathFunctionsTestAutomation.Tests
{
    public class MathSubtractEndpointTests
    {
        string BaseUrl = "http://localhost:7000/api";
    
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Category("P0")]
        public async Task Sub_200_ShouldReturnCorrectSum()
        {
            var requestBody = new { numbers = new[] { 1, 2, 3, 4 } };
            await TestHelper.ExecutePostTest("Sub_200_ShouldReturnCorrectSum", BaseUrl, "math/subtract", requestBody, 200, responseContent =>
            {
                var responseBody = JsonConvert.DeserializeObject<dynamic>(responseContent);
                Assert.That(responseBody, Is.Not.Null, "Expected responseBody but responseBody was null.");
                Assert.That((int)responseBody.result, Is.EqualTo(-8));
            });
        }

        [Test]
        [Category("P1")]
        public async Task Subtract_400_InvalidJSON()
        {
            var requestBody = new { bad = "JSON" };
            await TestHelper.ExecutePostTest("Subtract_400_InvalidJSON", BaseUrl, "math/subtract", requestBody, 400, responseContent =>
            {
                Assert.That(responseContent, Is.EqualTo("The numbers list cannot be empty."));
            });
        }

        [Test]
        [Category("P2")]
        public async Task Subtract_400_GarbageInput()
        {
            string requestBody = "This is garbage input";
            await TestHelper.ExecutePostTest("Subtract_400_GarbageInput", BaseUrl, "math/subtract", requestBody, 400, responseContent =>
            {
                Assert.That(responseContent, Does.Contain("The JSON value could not be converted to MathFunctions.Models.MathRequest."));
            });
        }
   }
}