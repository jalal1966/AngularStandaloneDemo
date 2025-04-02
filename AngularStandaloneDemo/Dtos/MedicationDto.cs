namespace AngularStandaloneDemo.Dtos
{
    public class MedicationDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? PrescribingProvider { get; set; }
        public string? Purpose { get; set; }
    }




}
