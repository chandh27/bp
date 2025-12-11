using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Runtime.InteropServices;
namespace BPCalculator.E2E
{
    public class CalculatorUITests : IDisposable
    {
        private readonly IWebDriver _driver;
        private const string QA_URL =
            "https://bp-calculator-qa-cbgkg0bfdrdbf4c8.norwayeast-01.azurewebsites.net/";
        public CalculatorUITests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                options.BinaryLocation = "/usr/bin/chromium-browser";
            _driver = new ChromeDriver(options);
        }
        // ===== POSITIVE PATH HELPER (already working) =====
        private string SubmitAndGetResult(string systolic, string diastolic)
        {
            _driver.Navigate().GoToUrl(QA_URL);
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            // Wait for input fields to exist
            wait.Until(d => d.FindElements(By.Id("BP_Systolic")).Count == 1);
            var sys = _driver.FindElement(By.Id("BP_Systolic"));
            var dia = _driver.FindElement(By.Id("BP_Diastolic"));
            sys.Clear();
            sys.SendKeys(systolic);
            dia.Clear();
            dia.SendKeys(diastolic);
            _driver.FindElement(By.CssSelector("input[type='submit']")).Click();
            // Wait for the NEW result DIV after POST reload
            var resultElement = wait.Until(d =>
            {
                var list = d.FindElements(By.XPath("//form/div[last()]"));
                return list.Count == 1 ? list[0] : null;
            });
            // Now safe to check Displayed
            wait.Until(_ => resultElement.Displayed);
            return resultElement.Text.Trim();
        }
        // ===== NEGATIVE / VALIDATION HELPER =====
        private string SubmitAndGetValidationMessage(string systolic, string diastolic)
        {
            _driver.Navigate().GoToUrl(QA_URL);
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            // Wait for form fields
            wait.Until(d => d.FindElements(By.Id("BP_Systolic")).Count == 1);
            var sys = _driver.FindElement(By.Id("BP_Systolic"));
            var dia = _driver.FindElement(By.Id("BP_Diastolic"));
            sys.Clear();
            if (!string.IsNullOrEmpty(systolic))
                sys.SendKeys(systolic);
            dia.Clear();
            if (!string.IsNullOrEmpty(diastolic))
                dia.SendKeys(diastolic);
            _driver.FindElement(By.CssSelector("input[type='submit']")).Click();
            // Wait for any validation message (summary or field-level)
            var errorText = wait.Until(d =>
            {
                // Validation summary (standard ASP.NET Core)
                var summaries = d.FindElements(By.CssSelector(".validation-summary-errors li, .validation-summary-valid li"));
                foreach (var s in summaries)
                {
                    var t = s.Text?.Trim();
                    if (!string.IsNullOrWhiteSpace(t))
                        return t;
                }
                // Field-level validation messages (e.g. <span class="text-danger">)
                var fieldErrors = d.FindElements(By.CssSelector("span.text-danger, span.field-validation-error"));
                foreach (var e in fieldErrors)
                {
                    var t = e.Text?.Trim();
                    if (!string.IsNullOrWhiteSpace(t))
                        return t;
                }
                return null; // keep waiting
            });
            return errorText;
        }
        // ===== POSITIVE TESTS (categories) =====
        [Fact]
        public void Calculate_Low_BloodPressure_UI()
        {
            string result = SubmitAndGetResult("85", "55");
            Assert.Contains("Low", result);
        }
        [Fact]
        public void Calculate_Ideal_BloodPressure_UI()
        {
            string result = SubmitAndGetResult("110", "70");
            Assert.Contains("Ideal", result);
        }
        [Fact]
        public void Calculate_PreHigh_BloodPressure_UI()
        {
            string result = SubmitAndGetResult("130", "85");
            Assert.Contains("Pre-High", result);
        }
        [Fact]
        public void Calculate_High_BloodPressure_UI()
        {
            string result = SubmitAndGetResult("150", "95");
            Assert.Contains("High", result);
        }
        // ===== NEW NEGATIVE / VALIDATION TESTS =====
        [Fact]
        public void Calculate_BlankFields_ShowsValidationError()
        {
            string error = SubmitAndGetValidationMessage("", "");
            Assert.False(string.IsNullOrWhiteSpace(error));
        }
        [Fact]
        public void Calculate_NonNumericInput_ShowsValidationError()
        {
            string error = SubmitAndGetValidationMessage("abc", "xyz");
            Assert.False(string.IsNullOrWhiteSpace(error));
        }
        [Fact]
        public void Calculate_OutOfRangeValues_ShowsValidationError()
        {
            // Example: very high / invalid values
            string error = SubmitAndGetValidationMessage("500", "400");
            Assert.False(string.IsNullOrWhiteSpace(error));
        }
        public void Dispose()
        {
            try { _driver.Quit(); } catch { }
            try { _driver.Dispose(); } catch { }
        }
    }
}