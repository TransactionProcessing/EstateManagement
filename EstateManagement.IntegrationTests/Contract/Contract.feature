@base @shared
Feature: Contract

Background: 
	Given the following security roles exist
	| RoleName |
	| Estate   |

	Given the following api resources exist
	| ResourceName     | DisplayName            | Secret  | Scopes           | UserClaims                 |
	| estateManagement | Estate Managememt REST | Secret1 | estateManagement | MerchantId, EstateId, role |

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

@PRTest
Scenario: Create Contract
	Given I create a contract with the following values
	| EstateName    | OperatorName    | ContractDescription |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract |

	When I create the following Products
	| EstateName    | OperatorName    | ContractDescription | ProductName    | DisplayText | Value  |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | 100 KES     | 100.00 |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | Variable Topup | Custom      |        |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | 100 KES     | 100.00 |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract | Variable Topup | Custom      |        |

	When I add the following Transaction Fees
	| EstateName    | OperatorName    | ContractDescription | ProductName    | CalculationType | FeeDescription      | Value |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | Fixed           | Merchant Commission | 2.00  |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | Variable Topup | Fixed           | Merchant Commission | 2.00  |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | Percentage      | Merchant Commission | 0.75  |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract | Variable Topup | Percentage      | Merchant Commission | 0.75  |

Scenario: Get Transaction Fees for a Product

	Given I create a contract with the following values
	| EstateName    | OperatorName    | ContractDescription |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract |

	When I create the following Products
	| EstateName    | OperatorName    | ContractDescription | ProductName    | DisplayText | Value  |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | 100 KES     | 100.00 |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | Variable Topup | Custom      |        |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | 100 KES     | 100.00 |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract | Variable Topup | Custom      |        |

	When I add the following Transaction Fees
	| EstateName    | OperatorName    | ContractDescription | ProductName    | CalculationType | FeeDescription      | Value |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | Fixed           | Merchant Commission | 2.00  |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | Percentage      | Merchant Commission | 0.025 |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | Variable Topup | Fixed           | Merchant Commission | 2.50  |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | Percentage      | Merchant Commission | 0.85  |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract | Variable Topup | Percentage      | Merchant Commission | 0.85  |

	Then I get the Transaction Fees for '100 KES Topup' on the 'Operator 1 Contract' contract for 'Test Estate 1' the following fees are returned
	| CalculationType | FeeDescription      | Value |
	| Fixed           | Merchant Commission | 2.00  |
	| Percentage      | Merchant Commission | 0.025 |

	Then I get the Transaction Fees for 'Variable Topup' on the 'Operator 1 Contract' contract for 'Test Estate 1' the following fees are returned
	| CalculationType | FeeDescription      | Value |
	| Fixed           | Merchant Commission | 2.50  |

	Then I get the Transaction Fees for '100 KES Topup' on the 'Operator 1 Contract' contract for 'Test Estate 2' the following fees are returned
	| CalculationType | FeeDescription      | Value |
	| Percentage      | Merchant Commission | 0.85  |

	Then I get the Transaction Fees for 'Variable Topup' on the 'Operator 1 Contract' contract for 'Test Estate 2' the following fees are returned
	| CalculationType | FeeDescription      | Value |
	| Percentage      | Merchant Commission | 0.85  |

Scenario: Get Merchant Contracts

	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	| Test Merchant 2 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant2.co.uk | Test Estate 2 |
	
	Given I create a contract with the following values
	| EstateName    | OperatorName    | ContractDescription |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract |

	When I create the following Products
	| EstateName    | OperatorName    | ContractDescription | ProductName    | DisplayText | Value  |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | 100 KES     | 100.00 |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | Variable Topup | Custom      |        |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | 100 KES     | 100.00 |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract | Variable Topup | Custom      |        |

	When I add the following Transaction Fees
	| EstateName    | OperatorName    | ContractDescription | ProductName    | CalculationType | FeeDescription      | Value |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | Fixed           | Merchant Commission | 2.00  |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | Percentage      | Merchant Commission | 0.025 |
	| Test Estate 1 | Test Operator 1 | Operator 1 Contract | Variable Topup | Fixed           | Merchant Commission | 2.50  |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract | 100 KES Topup  | Percentage      | Merchant Commission | 0.85  |
	| Test Estate 2 | Test Operator 1 | Operator 1 Contract | Variable Topup | Percentage      | Merchant Commission | 0.85  |

	Then I get the Merchant Contracts for 'Test Merchant 1' for 'Test Estate 1' the following contract details are returned
	| ContractDescription | ProductName    |
	| Operator 1 Contract | 100 KES Topup  |
	| Operator 1 Contract | Variable Topup |

	Then I get the Merchant Contracts for 'Test Merchant 2' for 'Test Estate 2' the following contract details are returned
	| ContractDescription | ProductName    |
	| Operator 1 Contract | 100 KES Topup  |
	| Operator 1 Contract | Variable Topup |

	

	