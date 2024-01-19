# Banking Demo with EF Core :bank:

This repository contains the example project demonstrated in the EF Core session for the PÓS TECH audience at FIAP. 
This simple banking application showcases the use of EF Core in a .NET 8 environment for handling banking operations like credit and debit transactions using PIX keys. 
The primary focus of this project is to demonstrate various EF Core features and best practices.

## Getting Started :checkered_flag:

What you'll need:

- .NET 8 SDK
- Docker :whale:

## Features Demonstrated :building_construction:

- Grouping [EF Core's configuration](https://learn.microsoft.com/en-us/ef/core/modeling/#grouping-configuration) `IEntityTypeConfiguration<T>` to map complex domains into database tables.
- Implementing [Shadow Properties](https://learn.microsoft.com/en-us/ef/core/modeling/shadow-properties) for database-specific requirements that do not directly map to the domain.
- Mapping value objects and enumeration classes to primitive data columns using [EF Core converters](https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations).
- Using [EF Core Interceptors](https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors) to modify control properties in the database.
- Creating an `IPipelineBehavior<TRequest, TResponse>` with [MediatR](https://github.com/jbogard/MediatR) to ensure commands are executed within a database transaction.
- Techniques for [querying](https://learn.microsoft.com/en-us/ef/core/modeling/shadow-properties#accessing-shadow-properties) Shadow Properties.
- Integrating EF Core with [Testcontainers](https://github.com/testcontainers/testcontainers-dotnet) library to create robust integration tests with independent databases without altering the EF Core Database Provider.

## License :balance_scale:
This project is licensed under the MIT License.

## Acknowledgments :trophy:

Special thanks to Thiago da Silva Adriano and Douglas Gomes for their trust and opportunity to share this knowledge. 
Also, thank you to everyone who attended the live session and contributed to the discussion.

## Contribute :wave:

Feel free to open an issue or a pull request. 