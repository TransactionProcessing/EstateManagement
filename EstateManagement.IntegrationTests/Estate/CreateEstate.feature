@base @shared
Feature: CreateEstate

Background: 

@PRTest
Scenario: Create Estate
	When I create the following estates
	| EstateName   |
	| Test Estate 1 |