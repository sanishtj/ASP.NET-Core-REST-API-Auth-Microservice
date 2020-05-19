# Auth-Microservice a .NET Core API Boilerplate

> ***Hello there!*** *This is my try to give a complete example of a .NET Core Rest API which I am using as an OAuth Microservice in my project. I have tried to  cover all the features required in a production-ready API.*

### **Features**

 - API for multi-tenant authentication and authorization
 - OAuth, Resource Owner Password Credentials Grant
 - JWT token and Microsoft Identity
 - EF Core Code-First with MS SQL (DB Migration code and DB scripts included)
 - Repository and Unit of Work patterns
 - File-based logging using Serilog
 - Automapper for mapping models to entities
 - JSONPatch for http-patch operation
 - SendGrid - For email templates and emailing
 - Unit tests & integration tests using NUnit, Moq and In-memory EF core

> You will need to set up your secrets.json with a few values I am using in the code. Here is a sample  [link](https://gist.github.com/sanishtj/03d9d9f66bc18d8f0f8241d6f9b89f44)

 
## API for multi-tenant authentication and authorization
I have enhanced the IdentityUser object to having a relationship with Tenants. If you don't want multi-tenancy, you can replace the custom `HRSIdentityUser` with `IdentityUser`. There are endpoints to manage tenants. Feel free to extend as per your needs. I am using *role-based authorization* in this code.

## OAuth, Resource Owner Password Credentials Grant
Since I am going to use this API for own apps this grand type is enough. You will have to extend the API if your requirements are different. Users can log in with their user name and password and they will get a token which can be validated in other microservices for a valid user. 

## JWT token and Microsoft Identity
For authentication, I am using Microsoft Identity with all its built-in features. After successful authentication, a JWT token will be returned which contains a payload of tenant details and role details along with expiry and other JWT related data.

## EF Core with Code-First with MS SQL
I am using MS SQL in this code. You can use any DB. EF Core as an ORM supports several DBs like MySQL, Postgress etc. Here is the full list: [link](https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli)
You don't have to use code first approach if you are a DB first developer. The good thing about using Code-First with IdentityManager is that it will auto-generate all DBs needed. Since I am extending `IdentityUser` in `HRSIdentityUser`, generated tables will use that model.

## Repository and Unit of Work patterns
I am a big fan of this approach as it will make life easy to mock your DBs or just replace the implementation with another type of DB. Unit of work is the one holds the DB context and share with other repositories. 

## File-based logging using Serilog
Serilog is a robust logging system. I have added Serilog to enhance the default logging of .NET Core. I am splitting the log files on dates so each day I can get a new log. *`It's a third-party library which can be added from Nuget.`*

## Automapper for mapping models to entities
This is something we can use to map your models with entities. Models are those classes which we want to use as request and response of the API and entities are the classes which will have different or more properties to be used in DB First or DB Context. Automapper helps to map these two items easily. *`It's a third-party library which can be added from Nuget.`*

## JSONPatch for http-patch operation
This is a standard relatively new to the development world. I am not using this in my NodeJS APIs but .NET Core has good support for this standard and easy to use.

## SendGrid - For email templates and emailing
I am using SendGrid for managing all my email communications. I loved the idea and it's pretty easy to use. For this code, I am just using send email method in SendGrid API with sample links but I have a separate microservice which handles SendGrid email templates and send emails. In my product, I am calling 

## Unit tests & integration tests using NUnit and In-memory EF core

I have added many unit tests for the controller and integration tests for most of the positive scenarios. Feel free to add more tests. I am using EF Core In-memory DB for integration tests. This will avoid hitting the real DB. Seed data is also provided in code. 
