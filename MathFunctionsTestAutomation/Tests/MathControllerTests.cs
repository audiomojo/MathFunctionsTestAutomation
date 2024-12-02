using NUnit.Framework;
using System.Net;
using Newtonsoft.Json;
using NLog;
using static RestAssured.Dsl;

namespace MathFunctionsTestAutomation.Tests
{
    public class MathControllerTests
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        string BaseUrl = "http://localhost:7000/api/math";
        
        [SetUp]
        public void Setup()
        {
            Logger.Info("Testing Base URL: " + BaseUrl);
        }

        [Test]
        public void IsLogWorking()
        {
            Logger.Info("The log file is working correctly.\n");
            Assert.Pass();
        }


        // [Test]
        // public void Add_ShouldReturnCorrectSum()
        // {
        //     var requestBody = new
        //     {
        //         numbers = new[] { 1, 2, 3, 4 }
        //     };

        //     var response = Given()
        //         .ContentType("application/json")
        //         .Body(JsonConvert.SerializeObject(requestBody))
        //         .When()
        //         .Post($"{BaseUrl}/add")
        //         .Then()
        //         .StatusCode(200);


        //     // Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        //     // var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);
        //     // Assert.That((int)responseBody.Result, Is.EqualTo(10));
        // }

        // [Test]
        // public void Subtract_ShouldReturnCorrectDifference()
        // {
        //     var requestBody = new
        //     {
        //         numbers = new[] { 10, 2, 3 }
        //     };

        //     var response = Given()
        //         .Header("Content-Type", "application/json")
        //         .Body(JsonConvert.SerializeObject(requestBody))
        //         .Post($"{BaseUrl}/subtract");

        //     Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        //     var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);
        //     Assert.That((int)responseBody.Result, Is.EqualTo(5));
        // }

        // [Test]
        // public void Multiply_ShouldReturnCorrectProduct()
        // {
        //     var requestBody = new
        //     {
        //         numbers = new[] { 2, 3, 4 }
        //     };

        //     var response = Given()
        //         .Header("Content-Type", "application/json")
        //         .Body(JsonConvert.SerializeObject(requestBody))
        //         .Post($"{BaseUrl}/multiply");

        //     Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        //     var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);
        //     Assert.That((int)responseBody.Result, Is.EqualTo(24));
        // }

        // [Test]
        // public void Divide_ShouldReturnCorrectQuotient()
        // {
        //     var requestBody = new
        //     {
        //         numbers = new[] { 20, 4, 2 }
        //     };

        //     var response = Given()
        //         .Header("Content-Type", "application/json")
        //         .Body(JsonConvert.SerializeObject(requestBody))
        //         .Post($"{BaseUrl}/divide");

        //     Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        //     var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);
        //     Assert.That((int)responseBody.Result, Is.EqualTo(2));
        // }
    }
}