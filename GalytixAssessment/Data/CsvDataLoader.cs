namespace GalytixAssessment.Data
{
    public class CsvDataLoader : IDataLoader
    {
        private readonly ILogger<CsvDataLoader> _logger;
        private readonly string _filePath;
        public CsvDataLoader(ILogger<CsvDataLoader> logger)
        {
            _logger = logger;
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "gwpByCountry.csv");
        }

        public List<string[]> LoadData()
        {
            try
            {
                return File.ReadLines(_filePath)
                    .Skip(1)
                    .Select(line => line.Split(','))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading data: {ex.Message}");
                return new List<string[]>();
            }
        }
    }
}
