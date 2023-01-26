# Successful Startup
A tool for entrepreneurs to easily create useful business plans for small startups.

## Overview
I created Successful Startup to practice using the following principles and technologies in a real test case:

### Principles
* Agile Methodology
* Clean Architecture
* SOLID
* Test-Driven Development

### Technologies
* ASP.NET Core 6.0
* Blazor Server
* SQL Server
* Entity Framework
* Swagger
* Identity
* Bootstrap
* NUnit
* BUnit
* Moq

## Architecture
I used a 4 layer approach, with each layer residing in a separate project within the "src" folder. I also used test driven development (TDD), with test projects for each layer, residing in the "test" folder. Following clean architecture principles, the inner layers do not reference the outer layers.

### Domain
Class library. Inner-most layer. Business logic. Contains entity base classes, repository interfaces, and API interfaces. Creates blueprints that data layers must follow, ensuring that all requirements are met and making it easier to substitute databases as needed without risking errors.

### Data
Class library. Inner-middle layer. Database access. Contains Identity authentication tools, database contexts and factories, entities, code-first database migrations, repositories for CRUD operations, and a mapping profile to map data and domain entities. 

### API
Web API app. Outer-middle layer. Data exchange. Provides API endpoints for the presentation layer to indirectly read and write to the database using HttpClient requests. Set as the secondary startup project. 

### Presentation
Blazer Server app. Outer-most layer. User interface. Contains Razor components, pages, and layouts. Displays content and allows for user input. This is set as the primary startup project and contains Program.cs for app build and service configuration, so when the application runs, this project will be loaded first.

### Domain Tests
NUnit test project. Contains unit tests for the methods in the Domain layer.

### Data Tests
NUnit test project. Contains unit tests for the methods in the Data layer.

### API Tests
NUnit test project. Contains unit tests for the methods in the API layer.

### Presentation Tests
BUnit test project. Contains unit tests for the methods in the Presentation layer.

## Learning Process
As a junior developer, I created this application to integrate and reinforce my understanding of various technologies and principles. Therefore, I decided to document the process, including my mistakes and lessons learned. I also used explanatory comments as often as possible for why classes and lines of code are necessary. These can serve as references when building projects in the future.

### API
At first, I injected 2 API classes (read-only and write-only) into the presentation layer, with parameterized context factory and mapper constructors that were also injected in the configuration class. Each API implemented all the necessary repositories and called their methods. Later, I decided it was better to create a separate API layer between the presentation and data layers. I created a service so the presentation pages could make API calls.

### ASP.NET Core Identity
I used a single database context for Identity tables and other application tables. The context inherits from IdentityDbContext. I added the DbContext and DefaultIdentity to services in Program.cs and the database connection string to appsettings.json. I learned that Identity works differently on a Blazor Server project than on an ASP.NET Core Web App project; use AddDefaultIdentity<AppUser> rather than AddIdentity and use the built-in Identity.UI instead of manually creating Login, Logout, and Register pages and view models. It is still possible to custom override those pages by creating a Pages/Account folder. Also, a _LoginPartial page is required for the Identity.UI. At first, I had the Authentication-related files in the Presentation layer, but I ultimately moved them to the Data layer. I tried several ways to get the current user's ID in order to use it as a foreign key in other tables. The best way I found was to inject AuthenticationStateProvider into the Razor page, use it to get the current user's username (must be unique), then use it in an API call that searches for and returns the Id of that username.

### BUnit Testing
Before testing how a page or component renders, it's necessary to create a BUnit TestContext. I ended up creating a helper class with a GetTestContext method to avoid duplication. It's also necessary to configure services into the test context if the pages contain dependency injections. Actual instances need to be injected, unlike a typical service configuration. It's even possible to inject objects of mocks. It's also possible to mock authentication and authorization to see how a component renders under different scenarios. I created a helper method that returns a TestAuthorizationContext.

### Database Context Factory
I initially tried implementing the context directly, but I found that it worked better to use a factory. One, it is more thread safe. Two, it is more loosely coupled. Three, it was actually easier to configure.

### Dependency Injection
Presentation level service configurations take place in Program.cs. I created a separate static class in the data layer containing additional configurations for that layer. Program.cs calls the AddDataScope method of that configuration class in order to add those services.

### Email Send Service
If Identity is configured to SignIn.RequiredConfirmedAccount = true, it's necessary to set up an email service in order for users to verify their accounts. I used the IEmailSender interface with an SMTP client set up to a Gmail account's app password, stored in appsettings.json. 

### Mapping Profiles
I wanted the benefits of domain entities (defining and enforcing rules of data entities) without the complexity of AutoMapper. I tried creating domain entity interfaces, but interfaces cannot contain fields or properties, only methods. Instead I used abstract classes and derived the data entities from those, but the problem was the application could still not implicitly cast the child class to the parent class type and abstract classes cannot be implemented. So I switched the domain entities to a normal base class and used mapping. I started with a custom mapper, but realized that was more complex than using Automapper. I used AutoMapper, however I created an EntityConverter class containing an overloaded Convert method. Depending on the object type passed in, it returns the corresponding data or domain typec, making it unnecessary to use the mapper in the presentation layer.

### Migrations
After setting up the DbContext, I used the Package Manager Console to Add-Migration and Update-Database. This created the database and later on updated the schema as I made changes to the entities. I learned that it's not necessary to delete the database and previous migrations when you want to add a new migration. Entity Framework will update the existing database, using the new migration.

### NUnit Testing
To make class members accessible to tests, I used the [assembly: InternalsVisibleTo("test_project_name")] attribute and changed certain private members to internal. I also divided certain methods (i.e. EmailSender.SendEmailAsync) into multiple methods so I could test each part individually.  I used Moq for contexts and factories, which often requires the "virtual" keyword in the original method in order to override them in Moq's Setup and Verify methods. I used GenFu to generate mock data for the mock context could return. I used Shouldly for assertion, because I find it more intuitive. I separated the Arrange, Act, and Assert areas with an empty line.

### Projects
Initially, I used the ASP.NET Core Web App template for the Domain and Data projects, but I later switched to the Class Library template, deciding that only the Presentation layer, which is the startup project, needs a Program.cs file. I had to delete the ASP.NET Core Web App projects, both in Visual Studio and in the repository, in order to free up the namespaces for the Class Library projects.

### Razor Code Section
I was confused by errors that said @code didn't exist in the current context, but it's because I was using .cshtml pages. I used .razor pages instead and the
problem went away.

