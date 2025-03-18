using AngularStandaloneDemo.Models;

namespace AngularStandaloneDemo.Dtos
{
    public class WaitingListDto
    {

        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public  required string Status { get; set; }
        public required string Notes { get; set; }
        public string? PatientFisrtName { get; internal set; }
        public string? PatientLastName { get; internal set; }
        public string? ProviderFirstName { get; internal set; }
        public string? ProviderLastName { get; internal set; }
    }
}
