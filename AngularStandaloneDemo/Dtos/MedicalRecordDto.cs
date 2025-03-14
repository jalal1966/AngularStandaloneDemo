namespace AngularStandaloneDemo.Dtos
{
    public class MedicalRecordDto
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? Medications { get; set; }
        public string? Notes { get; set; }
        public bool IsFollowUpRequired { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public int PatientId { get; set; }
    }
}
