# Blood Pressure Category Calculator – CI Pipeline & Quality Automation  
This project implements a full CI pipeline for the **Blood Pressure Category Calculator** using:
- .NET 8  
- XUnit Unit Tests  
- SpecFlow BDD Tests  
- SonarCloud Code Quality  
- Trivy Security Scan  
- CodeQL Security Analysis  
- GitHub Actions CI Workflows  
The goal is to build, test, validate, and secure the application automatically as part of continuous integration.
---
# 1. Getting Started
## Clone the Repository
```bash
git clone https://github.com/<your-username>/bp.git
cd bp
```
## Verify .NET Installation
```bash
dotnet --version
```
---
# 2. Implementing Business Logic
The original repository contained placeholder logic:
```csharp
return new BPCategory();
```
### Updated Logic (Based on WHO Guidelines)
```csharp
if (Systolic < 90 || Diastolic < 60)
    return BPCategory.Low;
if ((Systolic >= 90 && Systolic <= 120) && (Diastolic >= 60 && Diastolic <= 80))
    return BPCategory.Ideal;
if ((Systolic > 120 && Systolic < 140) || (Diastolic > 80 && Diastolic < 90))
    return BPCategory.PreHigh;
if (Systolic >= 140 || Diastolic >= 90)
    return BPCategory.High;
```
### Added Validations
- Throw `ArgumentOutOfRangeException` for invalid ranges  
- Throw `ArgumentException` if `Diastolic >= Systolic`
---
# 3. Unit Testing (XUnit)
Created a new test project:
```bash
dotnet new xunit -n BPCalculator.Tests
dotnet add BPCalculator.Tests reference BPCalculator/BPCalculator.csproj
dotnet sln add BPCalculator.Tests/BPCalculator.Tests.csproj
```
Example Unit Test:
```csharp
[Theory]
[InlineData(110, 70, BPCategory.Ideal)]
[InlineData(150, 95, BPCategory.High)]
public void Calculates_Correct_Category(int sys, int dia, BPCategory expected)
{
    var bp = new BloodPressure { Systolic = sys, Diastolic = dia };
    Assert.Equal(expected, bp.Category);
}
```
Run tests:
```bash
dotnet test
```
---
# 4. BDD Testing (SpecFlow)
Install packages:
```bash
dotnet add BPCalculator.Tests package SpecFlow.xUnit
dotnet add BPCalculator.Tests package SpecFlow.Tools.MsBuild.Generation
```
Create feature file:
```
BPCalculator.Tests/Features/BloodPressureCategory.feature
```
Example Scenario:
```
Scenario: Ideal blood pressure
  Given the systolic value is 110
  And the diastolic value is 70
  When I calculate the blood pressure category
  Then the result should be "Ideal Blood Pressure"
```
---
# 5. SonarCloud Integration
Installed scanner:
```bash
dotnet tool install --global dotnet-sonarscanner
```
Added to CI workflow.
---
# 6. Trivy Security Scan (CI Security Check)
Installed in workflow and run using:
```yaml
trivy fs --severity HIGH,CRITICAL --exit-code 1 --no-progress .
```
✔ Fails pipeline if severe vulnerabilities exist.
---
# 7. GitHub Actions CI Workflow
Performs:
- Build  
- Unit Tests  
- BDD Tests  
- Code Coverage  
- SonarCloud Analysis  
- Trivy Vulnerability Scan  
- Upload Test Artifacts  
---
# 8. CodeQL Security Analysis
Runs static code scanning on every push, PR, and weekly schedule.
---
# 9. Git Commands
```bash
git add .
git commit -m "Add CodeQL security analysis workflow"
git push origin acceptance-bdd
```
---
# 10. Status Summary
| Task | Status |
|------|--------|
| Build code | ✔ Done |
| Unit tests | ✔ Done |
| BDD tests | ✔ Done |
| Code quality | ✔ SonarCloud |
| Vulnerability scan | ✔ Trivy |
| Static security analysis | ✔ CodeQL |
| Telemetry integration | ✔ App Insights |
---
# ✔ CI Implementation Complete
