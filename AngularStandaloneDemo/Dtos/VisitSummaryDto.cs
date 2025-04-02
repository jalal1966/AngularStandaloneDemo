namespace AngularStandaloneDemo.Dtos
{
    public class VisitSummaryDto
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public string? ProviderName { get; set; }
        public string? VisitType { get; set; }
        public string? Reason { get; set; }
    }
}
