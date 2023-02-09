# Successful Startup
A tool for entrepreneurs to easily create useful business plans for small startups.

## Contents
* [Overview](https://github.com/tim-honchel/successful-startup/edit/master/README.md#overview)
* [Architecture](https://github.com/tim-honchel/successful-startup/edit/master/README.md#architecture)
* [Learning Process](https://github.com/tim-honchel/successful-startup/edit/master/README.md#learning-process)
* [Resources](https://github.com/tim-honchel/successful-startup/edit/master/README.md#resources)
* [Local Deployment](https://github.com/tim-honchel/successful-startup/edit/master/README.md#local-deployment)

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
Class library. Inner-most layer. Business logic. Contains entity base classes, and repository interfaces. Creates blueprints that data layers must follow, ensuring that all requirements are met and making it easier to substitute databases as needed without risking errors.

### Data
Class library. Inner-middle layer. Database access. Contains database context and factory, entities, code-first database migrations, repositories for CRUD operations, and a mapping profile to map data and domain entities. 

### API
Web API app. Outer-middle layer. Data exchange. Provides API endpoints for the presentation layer to indirectly read and write to the database using HttpClient requests. Set as the secondary startup project and contains Program.cs for app build and service configuration. 

### Presentation
Blazer Server app. Outer-most layer. User interface. Contains Razor components, pages, and layouts. Displays content and allows for user input. Contains Identity. This is set as the primary startup project and contains Program.cs for app build and service configuration.

### Domain Tests
NUnit test project. Contains unit tests for the methods in the Domain layer.

### Data Tests
NUnit test project. Contains unit tests for the methods in the Data layer.

### API Tests
NUnit test project. Contains unit tests for the methods in the API layer.

### Presentation Tests
BUnit test project. Contains unit tests for the methods in the Presentation layer.

## Learning Process
As a developer, I created this application to integrate and reinforce my understanding of various technologies and principles. Therefore, I decided to document the process, including my mistakes and lessons learned. I also used explanatory comments as often as possible for why classes and lines of code are necessary. These can serve as references when building projects in the future.

### API
At first, I created 2 API classes (read-only and write-only) and injected them into the presentation layer. Each API implemented all the necessary repositories and called their methods. Later, I decided it was better to create a separate API layer between the presentation and data layers, with controllers and endpoints. I created a service in the presentation layer so the pages could make API calls indirectly through the injected service. I used an interface for that service, which made it possible to mock in my unit tests. I also added API tokens to authorize each request. The user-specific string was sent in the request header and verified by the API.

### ASP.NET Core Identity
At first, I used a single database context for Identity tables and other application tables. The context inherited from ApiAuthorizationDbContext, which inherits from IDbContext. Later, I decided it was better to have 2 separate databases: Identity on the frontend, and the application database on the backend. I added the DbContext and DefaultIdentity to services in Program.cs and the database connection string to appsettings.json. I learned that Identity works differently on a Blazor Server project than on an ASP.NET Core Web App project; use AddDefaultIdentity<AppUser> rather than AddIdentity, create the required _LoginPartial page, and use the built-in Identity.UI instead of manually creating Login, Logout, and Register pages and view models. It is still possible to custom override those pages by creating a Pages/Account folder, or as I found best, add Identity pages as a scaffolded item. I tried several ways to get the current user's ID in order to use it as a foreign key in other tables. One way I found was to inject AuthenticationStateProvider into the Razor page, use it to get the current user's username (must be unique), then query the AspNetUsers table to get the ID. However, the process takes several seconds, resulting in a delay, and it's best not to query Identity tables directly. Later on, I discovered it was faster to use AuthenticationStateProvider or UserManager to get the default claim containing the user ID. Additional claims can be set up during the registration process or at other times, for authorization. I created a simplified user table in the backend database to use for verifying each user's API token.

### Bootstrap
I found Bootstrap to be simpler and more intuitive than I anticipated. Essentially, it is a collection of classes that you can use for styling. The number of classes is not too overwhelming, they are well-organized and defined, and you can easily tweak them by changing a number or word in the class name.

### BUnit Testing
Before testing how a page or component renders, it's necessary to create a BUnit TestContext. I ended up creating a helper class with a GetTestContext method to avoid duplication. It's also necessary to configure services into the test context if the pages contain dependency injections. Actual instances need to be injected, unlike a typical service configuration. It's even possible to inject objects of mocks. You can mock authentication and authorization to see how a component renders under different scenarios. I created a helper method that returns a TestAuthorizationContext. I also learned it's possible to mock API calls by mocking a HttpMessageHandler and passing it into the HttpClient. It's also possible to test links and navigation by getting the NavigationManager from registered services and checking its URI.

### Database Context Factory
I initially tried implementing the context directly, but I found that it worked better to use a factory. One, it is more thread safe. Two, it is more loosely coupled. Three, it was actually easier to configure. At first, I used the IDesignTimeDbContextFactory interface when I had one combined context for Identity and the application. Later on, I just used a custom factory class and registered it in services.

### Dependency Injection
Presentation-level service configurations take place in its Program.cs and API-level service configurations take place in its Program.cs. I created separate static classes in the data layer containing additional configurations for that layer. As needed, the API Program.cs calls the AddDataScope methods of those configuration classes in order to add those services.

### Email Send Service
If Identity is configured to SignIn.RequiredConfirmedAccount = true, it's necessary to set up an email service in order for users to verify their accounts. I used the IEmailSender interface with an SMTP client set up to a Gmail account's app password, stored in appsettings.json.

### Mapping Profiles
I wanted the benefits of domain entities (defining and enforcing rules of data entities) without the complexity of AutoMapper. I tried creating domain entity interfaces, but interfaces cannot contain fields or properties, only methods. Instead I used abstract classes and derived the data entities from those, but the problem was the application could still not implicitly cast the child class to the parent class type and abstract classes cannot be implemented. So I switched the domain entities to a normal base class and used mapping. I started with a custom mapper, but realized that was more complex than using Automapper. I used AutoMapper, however I created an EntityConverter class containing an overloaded Convert method. Depending on the object type passed in, it returns the corresponding data or domain type, making it unnecessary to use the mapper in the presentation layer. I created a similar converter to convert API ViewModels to Data entities.

### Migrations
After setting up the DbContext, I used the Package Manager Console to Add-Migration and Update-Database. This created the database and later on updated the schema as I made changes to the entities. I learned that it's not necessary to delete the database and previous migrations when you want to add a new migration. Entity Framework will update the existing database, using the new migration.

### NUnit Testing
To make class members accessible to tests, I used the [assembly: InternalsVisibleTo("test_project_name")] attribute and changed certain private members to internal. I also divided certain methods (i.e. EmailSender.SendEmailAsync) into multiple methods so I could test each part individually.  I used Moq for contexts, factories, HttpMessageHandler, services, and repositories, which often requires the "virtual" keyword in the original method in order to override them in Moq's Setup and Verify methods and the use of interfaces rather than instances. I used GenFu to generate mock data for the mock context could return. I used Shouldly for assertion, because I find it more intuitive. I separated the Arrange, Act, and Assert areas with an empty line.

### Projects
Initially, I used the ASP.NET Core Web App template for the Domain and Data projects, but I later switched to the Class Library template, deciding that only the Presentation layer, which is the startup project, needs a Program.cs file. I had to delete the ASP.NET Core Web App projects, both in Visual Studio and in the repository, in order to free up the namespaces for the Class Library projects.

### Razor Code Section
I was confused by errors that said @code didn't exist in the current context, but it's because I was using .cshtml pages. I used .razor pages instead and the problem went away.

## Resources
I found these articles and videos especially useful in creating this project:

### Bootstrap
* "Complete List of All Bootstrap Classes" (https://www.w3schools.com/bootstrap/bootstrap_ref_all_classes.asp)

### BUnit
* "Faking authentication and authorization" (https://bunit.dev/docs/test-doubles/faking-auth.html)
* "Injecting services into components under test" (https://bunit.dev/docs/providing-input/inject-services-into-components.html)
* "How-To: Verify Blazor Navigation in bUnit" (https://darthpedro.net/2021/06/03/how-to-verify-blazor-navigation-in-bunit/)
* "Verifying markup from a component" (https://bunit.dev/docs/verification/verify-markup.html)

### Entity Framework
* "DevOps Database Delivery: State-Based vs. Migration Based" (https://www.red-gate.com/simple-talk/devops/database-devops/introduction-to-devops-database-delivery/)
* "Index Attribute" (https://www.entityframeworktutorial.net/entityframework6/index-attribute-in-code-first.aspx)
* "Why First Query is slow?" (https://entityframework.net/why-first-query-slow)

### Identity
* "Adding Identity to Existing Blazor Server Apps" (https://www.youtube.com/watch?v=Lp-0JtQbj84&list=PL8h4jt35t1wjvwFnvcB2LlYL4jLRzRmoz&index=57)
* "ASP.NET Core Security" (https://www.youtube.com/playlist?list=PLgRlicSxjeMOxypAEL2XqIc2m_gPmoVN-)
* "Claims based authorization in Blazor Server" (https://www.youtube.com/watch?v=3pOVrXim4OQ)
* "Scaffold Identity in ASP.NET Core projects" (https://learn.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-7.0&tabs=visual-studio)

### Moq
* "How do I verify a method was called exactly once with Moq?" (https://stackoverflow.com/questions/4206193/how-do-i-verify-a-method-was-called-exactly-once-with-moq)
* "Unit Test and Mock HttpContext in ASP.NET Core Controller" (https://www.thecodebuzz.com/unit-test-and-mock-httpcontext-in-asp-net-core-controller/)
* "Moq: Invalid setup on a non-overridable member" (https://stackoverflow.com/questions/1962010/moq-invalid-setup-on-a-non-overridable-member-x-x-getbytitleasdf)
* "Using Moq to Stub an interface method" (https://stackoverflow.com/questions/10505704/using-moq-to-stub-an-interface-method)
* "Why use It.Is<> or It.IsAny<> if I could just define a variable" (https://stackoverflow.com/questions/37051371/why-use-it-is-or-it-isany-if-i-could-just-define-a-variable)
* "You are mocking the HttpClient the wrong way" (https://www.youtube.com/watch?v=7OFZZAHGv9o)

## Local Deployment
Follow these instructions to build the solution on your machine:

1) Clone the git repository
2) Accept the prompt to trust the ASP.NET Core SSL Certificate
3) Create appsettings.json files in the Presentation project and API project. Add a connection string named "IdentityConnectionString" with the parameters for your local instance of MySqlServer.
4) In order to verify user emails, set up a Gmail account with an app password, and add connection strings to the appsettings.json files named "GmailAddress" and GmailPassword." Alternatively, in DataLayerConfiguration.cs, change options.SignInRequireConfirmedAccount to false.
5) Run the add-migration and update-database commands in the Package Console Manager. (will need to temporarily manually enter the connection string on PlanDbContext.cs when adding migration)
