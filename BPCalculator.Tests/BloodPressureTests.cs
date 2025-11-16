using Xunit;
using System;
namespace BPCalculator.Tests
{
    public class BloodPressureTests
    {
        [Theory]
        [InlineData(85,55,BPCategory.Low)]
        [InlineData(110,70,BPCategory.Ideal)]
        [InlineData(130,85,BPCategory.PreHigh)]
        [InlineData(150,95,BPCategory.High)]
        public void Calculates_Correct_Category(int sys, int dia, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = sys, Diastolic = dia };
            Assert.Equal(expected, bp.Category);
        }
//test1
        [Fact]
        public void Throws_When_Diastolic_GTE_Systolic()
        {
            var bp = new BloodPressure { Systolic = 100, Diastolic = 120 };
            Assert.Throws<ArgumentOutOfRangeException>(() => bp.Category);
        }
//test2
        [Fact]
        public void Throws_When_Systolic_Below_Min()
        {
            var bp = new BloodPressure { Systolic = 60, Diastolic = 70 };
            Assert.Throws<ArgumentOutOfRangeException>(() => bp.Category);
        }
    }
}
