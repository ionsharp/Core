namespace Imagin.Common.Data
{
    public class SearchInfo
    {
        public string Searching
        {
            get; set;
        }

        public double Duration
        {
            get; set;
        }

        public int Total
        {
            get; set;
        }

        public SearchInfo(string Searching, double Duration, int Total)
        {
            this.Searching = Searching;
            this.Duration = Duration;
            this.Total = Total;
        }
    }
}
