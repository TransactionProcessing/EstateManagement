@base @shared
Feature: Merchant

Background: 
	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |

	Given I have created the following operators
	| EstateName    | OperatorName    | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate 1 | Test Operator 1 | True                        | True                        |

	Given the following security roles exist
	| RoleName |
	| Merchant   |

Scenario: Create Merchant	
	When I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |

@PRTest	
Scenario: Assign Operator To Merchant	
	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	
	When I assign the following  operator to the merchants
	| OperatorName    | MerchantName    | MerchantNumber | TerminalNumber |
	| Test Operator 1 | Test Merchant 1 | 00000001       | 10000001       |

@PRTest
Scenario: Create Security User	
	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	
	When I create the following security users
	| EmailAddress                      | Password | GivenName    | FamilyName | MerchantName    |
	| merchantuser1@testmerchant1.co.uk | 123456   | TestMerchant | User1      | Test Merchant 1 |

