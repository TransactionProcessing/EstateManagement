@base @shared
Feature: Merchant

Background: 
	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |

@PRTest
Scenario: Create Merchant
	When I create the following merchants
	| MerchantName            | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
