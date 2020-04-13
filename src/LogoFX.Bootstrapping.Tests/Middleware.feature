Feature: Middleware
	In order to build modular and flexible apps
	As an app developer
	I want to be able to compose modules via middleware

Scenario: Composing module which registers a dependency in singleton fashion for bootstrapper with container adapter should register it accordingly
	When The container adapter is created
	And The bootstrapper with container adapter and composition modules is created
	And The composition modules middleware is applied onto the bootstrapper with container adapter
	Then The registered dependency should be of correct type
	And The registered dependency should be transient

Scenario: Composing module which registers a dependency in singleton fashion for bootstrapper with container should register it accordingly
	When The container adapter is created
	And The container is created
	And The bootstrapper with container adapter and container and composition modules is created
	And The composition modules middleware is applied onto the bootstrapper with container adapter and container
	Then The registered dependency should be of correct type
	And The registered dependency should be transient

Scenario: Applying collection registration middleware should register the correspondent dependencies as collection
	When The container adapter is created
	And The bootstrapper with current assembly is created
	And The collection registration middleware is applied onto the bootstrapper
	Then The dependencies are registered as a collection
