using Microsoft.Extensions.Caching.Memory;

namespace GalytixAssessment.Data
{
    public class GwpRepository : IGwpRepository
    {
        private readonly IMemoryCache _cache;
        private List<string[]> _rawData;
        private readonly ILogger<GwpRepository> _logger;

        public GwpRepository(IMemoryCache cache, ILogger<GwpRepository> logger)
        {
            _cache = cache;
            _logger = logger;
            _rawData = new List<string[]>();
        }

        public void SetData(List<string[]> data)
        {
            _rawData = data;
        }

        public async Task<Dictionary<string, double>> GetAverageGwpAsync(string country, List<string> lobs, int startYear = 2008, int endYear = 2015)
        {
            var output = new Dictionary<string, double>();
            if (string.IsNullOrEmpty(country) || lobs == null || lobs.Count == 0 || startYear > endYear)
                return output;

            output =  await Task.Run(() =>
            {
                var cacheKey = $"{country}-{string.Join(",", lobs)}-{startYear}-{endYear}";
                if (_cache.TryGetValue(cacheKey, out Dictionary<string, double> cachedResult))
                    return cachedResult;

                var result = new Dictionary<string, double>();
                int startColumn = 4 + (startYear - 2000); // Calculate start column index
                int columnCount = (endYear - startYear) + 1;

                var relevantRows = _rawData
                    .Where(row => row.Length >= startColumn + columnCount
                        && row[0].Equals(country, StringComparison.OrdinalIgnoreCase)
                        && lobs.Contains(row[3], StringComparer.OrdinalIgnoreCase))
                    .ToList();

                foreach (var row in relevantRows)
                {
                    string lob = row[3]; // Line of Business

                    // Extract values dynamically based on startYear and endYear
                    var values = row.Skip(startColumn).Take(columnCount)
                        .Select(value => double.TryParse(value, out var num) ? num : 0)
                        .ToList();

                    if (values.Count == columnCount) // Ensure all selected years are considered
                        result[lob] = values.Sum() / columnCount;
                }

                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                return result;
            });
            return output ?? new();
        }
    }
}
