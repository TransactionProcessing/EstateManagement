@base @shared
Feature: Estate

Background: 

@PRTest
Scenario: Create Estate
	When I create the following estates
	| EstateName    |
	| Test Estate 1 |