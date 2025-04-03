using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalytixAssessment.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GalytixAssessment.Tests
{
    public class GwpRepositoryTests
    {
        private readonly GwpRepository _repository;
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly Mock<ILogger<GwpRepository>> _mockLogger;

        public GwpRepositoryTests()
        {
            _mockCache = new Mock<IMemoryCache>();
            _mockLogger = new Mock<ILogger<GwpRepository>>();

            var cache = new MemoryCache(new MemoryCacheOptions()); // Real memory cache instance
            _repository = new GwpRepository(cache, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAverageGwpAsync_InvalidRequest_ReturnsEmptyDictionary()
        {
            // Act
            var result1 = await _repository.GetAverageGwpAsync(null, new List<string> { "property" });
            var result2 = await _repository.GetAverageGwpAsync("ae", null);
            var result3 = await _repository.GetAverageGwpAsync("ae", new List<string> { }, 2010, 2005); // Invalid range

            // Assert
            Assert.Empty(result1);
            Assert.Empty(result2);
            Assert.Empty(result3);
        }
    }
}
