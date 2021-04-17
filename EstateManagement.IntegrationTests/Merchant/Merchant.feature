@base @shared
Feature: Merchant

Background: 

	Given the following security roles exist
	| RoleName |
	| Estate   |
	| Merchant |

	Given I create the following api scopes
	| Name             | DisplayName                  | Description                        |
	| estateManagement | Estate Managememt REST Scope | A scope for Estate Managememt REST |

	Given the following api resources exist
	| ResourceName     | DisplayName            | Secret  | Scopes           | UserClaims                 |
	| estateManagement | Estate Managememt REST | Secret1 | estateManagement | merchantId, estateId, role |

	Given the following clients exist
	| ClientId      | ClientName     | Secret  | AllowedScopes    | AllowedGrantTypes  |
	| serviceClient | Service Client | Secret1 | estateManagement | client_credentials |
	| estateClient  | Estate Client  | Secret1 | estateManagement | password           |

	Given I have a token to access the estate management resource
	| ClientId      | 
	| serviceClient | 

	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |
	| Test Estate 2 |

	Given I have created the following operators
	| EstateName    | OperatorName    | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate 1 | Test Operator 1 | True                        | True                        |
	| Test Estate 2 | Test Operator 1 | True                        | True                        |

	Given I have created the following security users
	| EmailAddress                  | Password | GivenName  | FamilyName | EstateName    |
	| estateuser1@testestate1.co.uk | 123456   | TestEstate | User1      | Test Estate 1 |
	| estateuser1@testestate2.co.uk | 123456   | TestEstate | User1      | Test Estate 2 |

Scenario: Get Invalid Merchant - System Login
	When I get the merchant "Test Merchant 1" for estate "Test Estate 1" an error is returned

Scenario: Get Invalid Merchant - Estate User
	Given I am logged in as "estateuser1@testestate1.co.uk" with password "123456" for Estate "Test Estate 1" with client "estateClient"
	When I get the merchant "Test Merchant 1" for estate "Test Estate 1" an error is returned
	
Scenario: Create Merchant - System Login	 
	When I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |

Scenario: Create Merchant - Estate User	
	Given I am logged in as "estateuser1@testestate1.co.uk" with password "123456" for Estate "Test Estate 1" with client "estateClient"
	When I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	
Scenario: Assign Operator To Merchant - System Login	
	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	
	When I assign the following  operator to the merchants
	| OperatorName    | MerchantName    | MerchantNumber | TerminalNumber | EstateName    |
	| Test Operator 1 | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |

Scenario: Assign Operator To Merchant - Estate User	
	Given I am logged in as "estateuser1@testestate1.co.uk" with password "123456" for Estate "Test Estate 1" with client "estateClient"

	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	
	When I assign the following  operator to the merchants
	| OperatorName    | MerchantName    | MerchantNumber | TerminalNumber | EstateName    |
	| Test Operator 1 | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |

Scenario: Create Security User - System Login	
	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	
	When I create the following security users
	| EmailAddress                      | Password | GivenName    | FamilyName | MerchantName    | EstateName    |
	| merchantuser1@testmerchant1.co.uk | 123456   | TestMerchant | User1      | Test Merchant 1 | Test Estate 1 |

Scenario: Create Security User - Estate User	
	Given I am logged in as "estateuser1@testestate1.co.uk" with password "123456" for Estate "Test Estate 1" with client "estateClient"

	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	
	When I create the following security users
	| EmailAddress                      | Password | GivenName    | FamilyName | MerchantName    | EstateName    |
	| merchantuser1@testmerchant1.co.uk | 123456   | TestMerchant | User1      | Test Merchant 1 | Test Estate 1 |

Scenario: Add Device To Merchant - Estate User	
	Given I am logged in as "estateuser1@testestate1.co.uk" with password "123456" for Estate "Test Estate 1" with client "estateClient"

	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	
	When I add the following devices to the merchant
	| DeviceIdentifier | MerchantName    | EstateName    |
	| TestDevice1      | Test Merchant 1 | Test Estate 1 |

Scenario: Add Device To Merchant - System Login	
	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	
	When I add the following devices to the merchant
	| DeviceIdentifier | MerchantName    | EstateName    |
	| TestDevice1      | Test Merchant 1 | Test Estate 1 |

@PRTest
Scenario: Make Manual Merchant Deposit - Estate User
	Given I am logged in as "estateuser1@testestate1.co.uk" with password "123456" for Estate "Test Estate 1" with client "estateClient"

	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	
	When I make the following manual merchant deposits 
	| Reference | Amount  | DateTime  | MerchantName    | EstateName    |
	| Deposit1  | 1000.00 | Yesterday | Test Merchant 1 | Test Estate 1 |
	| Deposit2  | 1000.00 | Today     | Test Merchant 1 | Test Estate 1 |

	Then the merchant balances are as follows
	| Balance | AvailableBalance | MerchantName    | EstateName    |
	| 2000.00 | 2000.00          | Test Merchant 1 | Test Estate 1 |

	Then the following entries appear in the merchants balance history
	| DateTime  | Reference        | EntryType | In      | Out | ChangeAmount | Balance | MerchantName    | EstateName    |
	| Yesterday | Opening Balance  | D         | 0       | 0   | 0            | 0       | Test Merchant 1 | Test Estate 1 |
	| Yesterday | Merchant Deposit | C         | 1000.00 | 0   | 1000.00      | 1000.00 | Test Merchant 1 | Test Estate 1 |
	| Today     | Merchant Deposit | C         | 1000.00 | 0   | 1000.00      | 2000.00 | Test Merchant 1 | Test Estate 1 |

@PRTest
Scenario: Get Merchants for Estate - System Login
	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	| Test Merchant 2 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant2.co.uk | Test Estate 1 |
	| Test Merchant 3 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant3.co.uk | Test Estate 1 |
	| Test Merchant 4 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant4.co.uk | Test Estate 2 |
	| Test Merchant 5 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant5.co.uk | Test Estate 2 |

	When I assign the following  operator to the merchants
	| OperatorName    | MerchantName    | MerchantNumber | TerminalNumber | EstateName    |
	| Test Operator 1 | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |
	| Test Operator 1 | Test Merchant 2 | 00000001       | 10000001       | Test Estate 1 |
	| Test Operator 1 | Test Merchant 3 | 00000001       | 10000001       | Test Estate 1 |
	| Test Operator 1 | Test Merchant 4 | 00000001       | 10000001       | Test Estate 2 |
	| Test Operator 1 | Test Merchant 5 | 00000001       | 10000001       | Test Estate 2 |

	When I add the following devices to the merchant
	| DeviceIdentifier | MerchantName    | EstateName    |
	| TestDevice1      | Test Merchant 1 | Test Estate 1 |
	| TestDevice2      | Test Merchant 2 | Test Estate 1 |
	| TestDevice3      | Test Merchant 3 | Test Estate 1 |
	| TestDevice4      | Test Merchant 4 | Test Estate 2 |
	| TestDevice5      | Test Merchant 5 | Test Estate 2 |

	When I create the following security users
	| EmailAddress                      | Password | GivenName    | FamilyName | MerchantName    | EstateName    |
	| merchantuser1@testmerchant1.co.uk | 123456   | TestMerchant | User1      | Test Merchant 1 | Test Estate 1 |
	| merchantuser1@testmerchant2.co.uk | 123456   | TestMerchant | User1      | Test Merchant 2 | Test Estate 1 |
	| merchantuser1@testmerchant3.co.uk | 123456   | TestMerchant | User1      | Test Merchant 3 | Test Estate 1 |
	| merchantuser1@testmerchant4.co.uk | 123456   | TestMerchant | User1      | Test Merchant 4 | Test Estate 2 |
	| merchantuser1@testmerchant5.co.uk | 123456   | TestMerchant | User1      | Test Merchant 5 | Test Estate 2 |

	When I get the merchants for 'Test Estate 1' then 3 merchants will be returned

	When I get the merchants for 'Test Estate 2' then 2 merchants will be returned

@PRTest
Scenario: Get Merchants for Estate - Estate Login
	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	| Test Merchant 2 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant2.co.uk | Test Estate 1 |
	| Test Merchant 3 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant3.co.uk | Test Estate 1 |
	| Test Merchant 4 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant4.co.uk | Test Estate 2 |
	| Test Merchant 5 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant5.co.uk | Test Estate 2 |

	When I assign the following  operator to the merchants
	| OperatorName    | MerchantName    | MerchantNumber | TerminalNumber | EstateName    |
	| Test Operator 1 | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |
	| Test Operator 1 | Test Merchant 2 | 00000001       | 10000001       | Test Estate 1 |
	| Test Operator 1 | Test Merchant 3 | 00000001       | 10000001       | Test Estate 1 |
	| Test Operator 1 | Test Merchant 4 | 00000001       | 10000001       | Test Estate 2 |
	| Test Operator 1 | Test Merchant 5 | 00000001       | 10000001       | Test Estate 2 |

	When I add the following devices to the merchant
	| DeviceIdentifier | MerchantName    | EstateName    |
	| TestDevice1      | Test Merchant 1 | Test Estate 1 |
	| TestDevice2      | Test Merchant 2 | Test Estate 1 |
	| TestDevice3      | Test Merchant 3 | Test Estate 1 |
	| TestDevice4      | Test Merchant 4 | Test Estate 2 |
	| TestDevice5      | Test Merchant 5 | Test Estate 2 |

	When I create the following security users
	| EmailAddress                      | Password | GivenName    | FamilyName | MerchantName    | EstateName    |
	| merchantuser1@testmerchant1.co.uk | 123456   | TestMerchant | User1      | Test Merchant 1 | Test Estate 1 |
	| merchantuser1@testmerchant2.co.uk | 123456   | TestMerchant | User1      | Test Merchant 2 | Test Estate 1 |
	| merchantuser1@testmerchant3.co.uk | 123456   | TestMerchant | User1      | Test Merchant 3 | Test Estate 1 |
	| merchantuser1@testmerchant4.co.uk | 123456   | TestMerchant | User1      | Test Merchant 4 | Test Estate 2 |
	| merchantuser1@testmerchant5.co.uk | 123456   | TestMerchant | User1      | Test Merchant 5 | Test Estate 2 |

	Given I am logged in as "estateuser1@testestate1.co.uk" with password "123456" for Estate "Test Estate 1" with client "estateClient"

	When I get the merchants for 'Test Estate 1' then 3 merchants will be returned

	Given I am logged in as "estateuser1@testestate2.co.uk" with password "123456" for Estate "Test Estate 2" with client "estateClient"

	When I get the merchants for 'Test Estate 2' then 2 merchants will be returned


