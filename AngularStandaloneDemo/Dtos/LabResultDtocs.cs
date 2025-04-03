namespace AngularStandaloneDemo.Dtos
{
    public class LabResultDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime TestDate { get; set; }
        public string? TestName { get; set; }
        public string? Result { get; set; }
        public string? ReferenceRange { get; set; }
    }
}
