# Copious
Copious is an extensible framework / app base / whatever you name it.
Based on domain driven design and micro service based (distributed) development in .NET Core. 
Copious uses CQRS effectively.
Common CRUD operations are implemented in EF and exposed via handlers.

App Architecture.
App is based on principles of Domain Driven Design.
Domain is the problem area we are addressing.
Application is built on principles of Hexagonal Architecture.
At the heart of the hexagon is the domain layer surrounded by application layer, surroounded by persistance layer surrounded by infrastructure layer. 

Domain Layer - Consists of all the business logic and rules.
Application Layer - Interaction point for applications. Only through application layer any one can access the domain.
Persistance Layer - Manages the data of the application,
Infrastructure Layer - Contains the infrastrucutre services like email, sms, security and others.

Layers linked via ports and adapters.
Ports are abstractins (intefaces), Adapters are concretions (class /implemntations).
Dependancy chain runs from outer to inner, means the Application layer can refere the Domain layer, but the reverse is not directly possible.
To achieve this we use Inversion of Control (dependancy injection)

Technologies used
PostgreSQL - Database
EF Core - ORM used to access database from backend
ASP.NET Core - API development which is our backend
Angular - Front end of our application (Front end not included here)

Flow:
1. Angular Client request an action to Back End Controller
2. Security process takes place
3. Application Layer is invoked by the controller (Command Handler / Query Handler)
4. Domain functions are executed for command handler
5. Persistance layer stores or gets the data for query handler
6. Infralayer is invoked if needed e.g to send mail or other this process is usually async
7. Result is sent back to client
 
Security:
Security is implemented at the controller level, which is the first point of back end. There is a three layerd security provided.
1. XSRF Antiforgery  - To prevent cross site attacks
2. Authentication - To prevent un known uses access our systm. Implemented using JWT Authentication.
3. Authorization - To protect user functions. Implemented using Policy based authorization (roles and policies)

How is this policy organized? User->Role->Policies
i.e. An user can be in multiple roles and a policy can involve multiple roles.

E.g. let there be a policy called mypolicy, this policy involves roles roleX and roleY
If a function (api) let it be myfunction, needs mypolicy. only the uses who are in both roles (roleX and roleY) can access the myfunction.

Foundation elements:
1. Commands
2. Queries
3. Events

1. Commands are used to do any state change is a system. or to perform any action in a system. E.g. Signup a new user
Queries are used to retrieve any data from the system,
E.g. Get all agencies of users
Events are usually result of Commands used to perform soem side effects.
Each element has its own pipeline.

Command pipeline,
Controller creates a command - puts it on command bus - command bus identify the relevant handler for the command and delegates
- command handler validate the command - domain logic is executed - gives the control back to command handler -
- command handler calls repository in peristance layer to save the data - emits events if needed - 

Query pipeline,
Controller creates a query - sends the query to a query processor - query processor identify the relevant handler for the query and delegates
- query gets executed in the persistance layer - data is returned.

App Structure
App is divided into 2 major parts
1. Framework (this repository)
2. Business Contexts (BC / Modules) (the one you can build on top of it)

Frame work involves
1. Foundation - Foundation elements of our architecure 
2. Shared Kernel - Bae of a domain layer 
3. Application and Interface - App. layer consisting command handlers
4. Peristance and Interface - Peristance layer containing repository and query handlers
5. Workflow - To support any future workflows
6. Utilities - Common utils for our app.

Module Design
There will be 1 main module for each application called as Core Module, There can be as many modules depends on the requirement.
Modules are same as Bounded Context in domain driven design.

Each Module Conains
State - Classes that contain the state (Entity framework poco classes)  (Core.State)
Main - Domain logic (Core)
Application - Core.Application
Perisistance - Core.Perisistance

Highlights:
Clear separation of concerns. 
Async facility throught out
Application is horizontally scalable, 
Circuit breaker using polly
Multitenancy support

Security flow:
-User tries to access application ->
  -Server issues a XSRF token ->
    -User gets Redericted to login ->
      -User login with credentials ->
       - Server authenticates with the provided credentials and issues a JWT token if valid ->
          -The token contains the user identity - This token is then used along with the XSRF token in subsequent request to the  application ->
           - Application first checks the XSRF token to pervent forgery (ANTI FORGERY) ->
             - After that application check for identity token (AUTHENTICATION) - >
               - Then based upon the requested action the server check the user rights (AUTHORIZATION) -> 
                 - Application can now perform the action.
