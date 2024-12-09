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
    public class MathMultiplyEndpointTests
    {
        string BaseUrl = "http://localhost:7000/api";
    
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Category("P0")]
        public async Task Multiply_200_ShouldReturnCorrectProduct()
        {
            var requestBody = new { numbers = new[] { 1, 2, 3, 4 } };
            await TestHelper.ExecutePostTest("Multiply_200_ShouldReturnCorrectProduct", BaseUrl, "math/multiply", requestBody, 200, responseContent =>
            {
                var responseBody = JsonConvert.DeserializeObject<dynamic>(responseContent);
                Assert.That(responseBody, Is.Not.Null, "Expected responseBody but responseBody was null.");
                Assert.That((int)responseBody.result, Is.EqualTo(24));
            });
        }

        [Test]
        [Category("P1")]
        public async Task Multiply_400_InvalidJSON()
        {
            var requestBody = new { hello = "there" };
            await TestHelper.ExecutePostTest("Multiply_400_InvalidJSON", BaseUrl, "math/multiply", requestBody, 400, responseContent =>
            {
                Assert.That(responseContent, Is.EqualTo("The numbers list cannot be empty."));
            });
        }

        [Test]
        [Category("P2")]
        public async Task Multiply_400_GarbageInput()
        {
            string requestBody = "This is garbage input";
            await TestHelper.ExecutePostTest("Multiply_400_GarbageInput", BaseUrl, "math/multiply", requestBody, 400, responseContent =>
            {
                Assert.That(responseContent, Does.Contain("The JSON value could not be converted to MathFunctions.Models.MathRequest."));
            });
        }
   }
}