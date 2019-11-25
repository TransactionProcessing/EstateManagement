﻿@base @shared
Feature: Estate

Background: 

@PRTest
Scenario: Create Estate
	When I create the following estates
	| EstateName    |
	| Test Estate 1 |

Scenario: Create Operator
	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |
	When I create the following operators
	| EstateName    | OperatorName    | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate 1 | Test Operator 1 | True                        | True                        |