namespace AngularStandaloneDemo.Models
{
    // Add this class outside the controller
    public class MedicineCheckResult
    {
        public bool Exists { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? ExistingMedicineId { get; set; }
        public int? CreatedMedicineId { get; set; }
    }
}
