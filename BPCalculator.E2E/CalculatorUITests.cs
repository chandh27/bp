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

        // QA URL
        private const string QA_URL =
            "https://bp-calculator-qa-cbgkg0bfdrdbf4c8.norwayeast-01.azurewebsites.net/";

        public CalculatorUITests()
        {
            var options = new ChromeOptions();

            // Stable headless mode
            options.AddArgument("--headless=new");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                options.BinaryLocation = "/usr/bin/chromium-browser";
            }

            _driver = new ChromeDriver(options);
        }

        // ======================================================
        //  MAIN HELPER – FULLY FIXED AGAINST STALE ELEMENTS
        // ======================================================
        private string SubmitAndGetResult(string systolic, string diastolic)
        {
            _driver.Navigate().GoToUrl(QA_URL);

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));

            // 1. WAIT FOR FULL PAGE + FORM INPUTS (prevents stale elements)
            wait.Until(d =>
            {
                var elements = d.FindElements(By.CssSelector("form input#BP_Systolic"));
                return elements.Count == 1;   // Razor form completely loaded
            });

            // Interact AFTER DOM is stable
            var sys = _driver.FindElement(By.Id("BP_Systolic"));
            var dia = _driver.FindElement(By.Id("BP_Diastolic"));

            sys.Clear();
            sys.SendKeys(systolic);

            dia.Clear();
            dia.SendKeys(diastolic);

            _driver.FindElement(By.CssSelector("input[type='submit']")).Click();

            // 2. WAIT FOR THE NEW RESULT BLOCK AFTER POSTBACK
            wait.Until(d =>
            {
                var blocks = d.FindElements(By.XPath("//form/div[last()]"));
                return blocks.Count == 1;
            });

            // 3. Retrieve final result text
            var result = _driver.FindElement(By.XPath("//form/div[last()]")).Text;

            return result.Trim();
        }

        // ======================================================
        //  POSITIVE CATEGORY TESTS
        // ======================================================

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

        // CLEAN SHUTDOWN
        public void Dispose()
        {
            try { _driver.Quit(); } catch { }
            try { _driver.Dispose(); } catch { }
        }
    }
}
