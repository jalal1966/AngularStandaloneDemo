namespace AngularStandaloneDemo.Dtos
{
    public class WaitingListCreateDto
    {
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public DateTime RequestedDate { get; set; }
        public required string Notes { get; set; }
    }
}
