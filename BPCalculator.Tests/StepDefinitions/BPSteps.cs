using TechTalk.SpecFlow;
using Xunit;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BPCalculator.Tests.StepDefinitions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this BPCategory category)
        {
            var field = category.GetType().GetField(category.ToString());
            var attr = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
            return attr?.Name ?? category.ToString();
        }
    }

    [Binding]
    public class BPSteps
    {
        private BloodPressure _bp;
        private string _result;
        private Exception _exception;

        // GIVEN STEPS
     
        [Given(@"the systolic value is (.*)")]
        public void GivenTheSystolicValueIs(int systolic)
        {
            if (_bp == null) _bp = new BloodPressure();
            _bp.Systolic = systolic;
        }

        [Given(@"the diastolic value is (.*)")]
        public void GivenTheDiastolicValueIs(int diastolic)
        {
            if (_bp == null) _bp = new BloodPressure();
            _bp.Diastolic = diastolic;
        }

        // WHEN 

        [When(@"I calculate the blood pressure category")]
        public void WhenICalculateTheBloodPressureCategory()
        {
            try
            {
                _result = _bp.Category.GetDisplayName();
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        // THEN STEPS

        [Then(@"the result should be ""(.*)""")]
        public void ThenTheResultShouldBe(string expected)
        {
            Assert.Equal(expected, _result);
        }

        [Then(@"an error should be thrown")]
        public void ThenAnErrorShouldBeThrown()
        {
            Assert.NotNull(_exception);
        }
    }
}
