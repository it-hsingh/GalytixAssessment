using System.Collections.Generic;
using System.Threading.Tasks;
using GalytixAssessment.Controllers;
using GalytixAssessment.Data;
using GalytixAssessment.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GalytixAssessment.Tests
{
    public class CountryGwpControllerTests
    {
        private readonly Mock<IGwpRepository> _mockRepository;
        private readonly CountryGwpController _controller;

        public CountryGwpControllerTests()
        {
            _mockRepository = new Mock<IGwpRepository>();
            _controller = new CountryGwpController(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAverageGwp_ValidRequest_ReturnsOkWithData()
        {
            // Arrange
            var request = new GwpRequest
            {
                Country = "ae",
                Lob = new List<string> { "property", "transport" }
            };
            int startYear = 2008;
            int endYear = 2015;
            var expectedData = new Dictionary<string, double>
            {
                { "property", 446001906.1 },
                { "transport", 231441262.7 }
            };

            _mockRepository.Setup(repo => repo.GetAverageGwpAsync("ae", request.Lob, startYear, endYear))
                           .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.GetAverageGwp(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Dictionary<string, double>>(okResult.Value);
            Assert.Equal(expectedData, responseData);
        }

        [Fact]
        public async Task GetAverageGwp_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new GwpRequest { Country = "", Lob = null };

            // Act
            var result = await _controller.GetAverageGwp(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAverageGwp_NoMatchingData_ReturnsEmptyDictionary()
        {
            // Arrange
            var request = new GwpRequest
            {
                Country = "xx", // Non-existent country
                Lob = new List<string> { "unknown_lob" }
            };
            int startYear = 2008;
            int endYear = 2015;

            _mockRepository.Setup(repo => repo.GetAverageGwpAsync("xx", request.Lob, startYear, endYear))
                           .ReturnsAsync(new Dictionary<string, double>()); // No results

            // Act
            var result = await _controller.GetAverageGwp(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Dictionary<string, double>>(okResult.Value);
            Assert.Empty(responseData);
        }
    }
}
