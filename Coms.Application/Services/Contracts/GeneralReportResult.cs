namespace Coms.Application.Services.Contracts
{
    public class GeneralReportResult
    {
        public int Total { get; set; }
        public int Status { get; set; }
        public string StatusString { get; set; }
        public int Percent { get; set; }
        public string Title { get; set; }
    }
}
