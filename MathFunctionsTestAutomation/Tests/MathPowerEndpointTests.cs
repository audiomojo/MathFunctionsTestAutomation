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
    public class MathPowerEndpointTests
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        string BaseUrl = "http://localhost:7000/api";
    
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Category("P0")]
        public async Task Power_200_ShouldReturnCorrectResult()
        {
            var requestBody = new { Base = 2, Power = 10};
            await TestHelper.ExecutePostTest("Power_200_ShouldReturnCorrectResult", BaseUrl, "math/power", requestBody, 200, responseContent =>
            {
                var responseBody = JsonConvert.DeserializeObject<dynamic>(responseContent);
                Assert.That(responseBody, Is.Not.Null, "Expected responseBody but responseBody was null.");
                Assert.That((int)responseBody.result, Is.EqualTo(1024));
            });
        }

        [Test]
        [Category("P1")]
        public async Task Power_400_BadRequest()
        {
            var requestBody = new { Base = 2, Power = 10000 };
            await TestHelper.ExecutePostTest("Power_400_BadRequest", BaseUrl, "math/power", requestBody, 400, responseContent =>
            {
                Assert.That(responseContent, Does.Contain("The result is too large to be represented as a valid JSON number."));
            });
        }

        [Test]
        [Category("P1")]
        public async Task Power_400_InvalidJSON()
        {
            var requestBody = new { hello = "there" };

            await TestHelper.ExecutePostTest("Power_400_InvalidJSON", BaseUrl, "math/power", requestBody, 400, responseContent =>
            {
                Assert.That(responseContent, Does.Contain("0 to the power of 0 is undefined."));
            });
        }

        [Test]
        [Category("P2")]
        public async Task Power_400_GarbageInput()
        {
            String requestBody = "This is garbage input";

            await TestHelper.ExecutePostTest("Power_400_InvalidJSON", BaseUrl, "math/power", requestBody, 400, responseContent =>
            {
                Assert.That(responseContent, Does.Contain("The JSON value could not be converted to MathFunctions.Models.PowerRequest"));
            });
        }
   }
}