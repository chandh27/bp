Feature: Blood pressure category classification 

  As a user 

  I want to check my blood pressure category 

  So that I can understand my health condition 

 

  Scenario: Ideal blood pressure 

    Given the systolic value is 110 

    And the diastolic value is 70 

    When I calculate the blood pressure category 

    Then the result should be "Ideal Blood Pressure" 

 

  Scenario: Low blood pressure 

    Given the systolic value is 85 

    And the diastolic value is 55 

    When I calculate the blood pressure category 

    Then the result should be "Low Blood Pressure" 

 

  Scenario: High blood pressure 

    Given the systolic value is 150 

    And the diastolic value is 95 

    When I calculate the blood pressure category 

    Then the result should be "High Blood Pressure" 

 

  Scenario: Invalid blood pressure (diastolic >= systolic) 

    Given the systolic value is 100 

    And the diastolic value is 120 

    When I calculate the blood pressure category 

    Then an error should be thrown 