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
    public class OpenForBusinessTests
    {
        string BaseUrl = "http://localhost:7000/api";
    
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Category("Smoke")]
        public async Task Add_200_OpenForBusiness()
        {
            var requestBody = new { };
            await TestHelper.ExecuteGetTest("Add_200_OpenForBusiness", BaseUrl, "open", requestBody, 200, responseContent =>
            {
                Assert.That(responseContent, Does.Contain("open for business"));
            });
        }
   }
}