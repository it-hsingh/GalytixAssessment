namespace GalytixAssessment.Data
{
    public interface IGwpRepository
    {
        void SetData(List<string[]> data);
        Task<Dictionary<string, double>> GetAverageGwpAsync(string country, List<string> lobs, int startYear = 2008, int endYear = 2015);
    }
}
