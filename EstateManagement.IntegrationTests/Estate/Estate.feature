@base @shared
Feature: Estate

Background: 

Scenario: Create Estate
	When I create the following estates
	| EstateName    |
	| Test Estate 1 |

@PRTest
Scenario: Create Operator
	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |
	When I create the following operators
	| EstateName    | OperatorName    | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate 1 | Test Operator 1 | True                        | True                        |

Scenario: Create Security User
	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |
	When I create the following security users
	| EmailAddress                  | Password | GivenName  | FamilyName | EstateName    |
	| estateuser1@testestate1.co.uk | 123456   | TestEstate | User1      | Test Estate 1 |