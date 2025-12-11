using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BPCalculator
{
    public enum BPCategory
    {
        [Display(Name="Low Blood Pressure")] Low,
        [Display(Name="Ideal Blood Pressure")] Ideal,
        [Display(Name="Pre-High Blood Pressure")] PreHigh,
        [Display(Name ="High Blood Pressure")] High
    };

    public class BloodPressure
    {
        public const int SystolicMin = 70;
        public const int SystolicMax = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 100;

        [Range(SystolicMin, SystolicMax, ErrorMessage = "Invalid Systolic Value")]
        public int Systolic { get; set; }

        [Range(DiastolicMin, DiastolicMax, ErrorMessage = "Invalid Diastolic Value")]
        public int Diastolic { get; set; }

        public BPCategory Category
        {
            get
            {
                if (Systolic < SystolicMin || Systolic > SystolicMax)
                    throw new ArgumentOutOfRangeException(nameof(Systolic));
                if (Diastolic < DiastolicMin || Diastolic > DiastolicMax)
                    throw new ArgumentOutOfRangeException(nameof(Diastolic));
                if (Diastolic >= Systolic)
                    throw new ArgumentException("Diastolic pressure cannot be greater than or equal to systolic.");

                if (Systolic < 90 || Diastolic < 60) return BPCategory.Low;
                if (Systolic <= 120 && Diastolic <= 80) return BPCategory.Ideal;
                if (Systolic < 140 || Diastolic < 90) return BPCategory.PreHigh;
                return BPCategory.High;
            }
        }

        // NEW FEATURE (11 lines)
        public string RiskMessage
        {
            get
            {
                return Category switch
                {
                    BPCategory.Low     => "Risk: Hypotension – may cause dizziness.",
                    BPCategory.Ideal   => "Risk: Normal – maintain a healthy lifestyle.",
                    BPCategory.PreHigh => "Risk: Elevated – monitor your BP regularly.",
                    BPCategory.High    => "Risk: Hypertension – medical attention may be required.",
                    _ => "Risk: Unknown"
                };
            }
        }
    }
}
