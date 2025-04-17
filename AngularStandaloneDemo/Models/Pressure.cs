using System.Text.Json.Serialization;

namespace AngularStandaloneDemo.Models
{
    public class Pressure

    {
        public int Id { get; set; }
        public int VisitId { get; set; }
    
        // Blood pressure properties
        public int? SystolicPressure { get; set; }
        public int? DiastolicPressure { get; set; }
        public decimal? BloodPressureRatio { get; set; }
        public bool IsBloodPressureNormal { get; set; }

        // Treatment related properties
        // Navigation properties
        [JsonIgnore]
        public virtual Visit? Visit { get; set; }

       
        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        // Methods to calculate BP ratio and normality based on age, weight, and gender
        public void CalculateBloodPressureRatio()
        {
            if (SystolicPressure.HasValue && DiastolicPressure.HasValue && DiastolicPressure.Value > 0)
            {
                BloodPressureRatio = (decimal)SystolicPressure.Value / DiastolicPressure.Value;
            }
        }

        public void EvaluateBloodPressure(int age, decimal weight, string gender)
        {
            if (!SystolicPressure.HasValue || !DiastolicPressure.HasValue)
            {
                IsBloodPressureNormal = false;
                return;
            }

            // Default ranges
            int maxSystolic = 120;
            int minSystolic = 90;
            int maxDiastolic = 80;
            int minDiastolic = 60;

            // Adjust based on age
            if (age < 18)
            {
                // Pediatric adjustments
                maxSystolic = 110 + age;
                minSystolic = 80 + (age / 2);
                maxDiastolic = 70 + (age / 3);
                minDiastolic = 50 + (age / 3);
            }
            else if (age > 60)
            {
                // Elderly adjustments
                maxSystolic += (age - 60) / 10 * 5;
                minSystolic -= (age - 60) / 20 * 5;
                maxDiastolic += (age - 60) / 10 * 2;
            }

            // Adjust based on gender (simplified)
            if (gender.ToLower() == "male")
            {
                maxSystolic += 5;
                maxDiastolic += 3;
            }

            // Adjust based on weight (BMI consideration would be more accurate)
            // This is a simplified adjustment
            if (weight > 100)
            {
                maxSystolic += (int)((weight - 100) / 10) * 2;
                maxDiastolic += (int)((weight - 100) / 10);
            }

            // Evaluate if blood pressure is within normal range
            IsBloodPressureNormal =
                SystolicPressure.Value >= minSystolic &&
                SystolicPressure.Value <= maxSystolic &&
                DiastolicPressure.Value >= minDiastolic &&
                DiastolicPressure.Value <= maxDiastolic;
        }
    }
}
