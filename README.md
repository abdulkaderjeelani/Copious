# Copious
Copious is an extensible framework / app base / whatever you name it.
Based on domain driven design and micro service based (distributed) development in .NET Core. 
Copious uses CQRS effectively.
Common CRUD operations are implemented in EF and exposed via handlers.

# System Architecture.
System design is based on principles of DDD Domain Driven Design.
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

# Technologies 
PostgreSQL - Database
EF Core - ORM used to access database from backend
ASP.NET Core - API development which is our backend
Angular - Front end of our application (Front end not included here)
Redis - Caching
RabbitMQ - MQ
Kafka - MQ

# Flow:
1. Angular Client request an action to Back End Controller
2. Security process takes place
3. Application Layer is invoked by the controller (Command Handler / Query Handler)
4. Domain functions are executed for command handler
5. Persistance layer stores or gets the data for query handler
6. Infralayer is invoked if needed e.g to send mail or other this process is usually async
7. Result is sent back to client
 
# Security:
Security is implemented at the controller level, which is the first point of back end. There is a three layerd security provided.
1. XSRF Antiforgery  - To prevent cross site attacks
2. Authentication - To prevent un known uses access our systm. Implemented using JWT Authentication.
3. Authorization - To protect user functions. Implemented using Policy based authorization (roles and policies)

How is this policy organized? User->Role->Policies
i.e. An user can be in multiple roles and a policy can involve multiple roles.

E.g. let there be a policy called mypolicy, this policy involves roles roleX and roleY
If a function (api) let it be myfunction, needs mypolicy. only the uses who are in both roles (roleX and roleY) can access the myfunction.

# Foundation elements:
0. Component (Every thing is a component in Copious)
1. Commands
2. Queries
3. Events

1. Commands are used to do any state change is a system. or to perform any action in a system. E.g. Signup a new user
Queries are used to retrieve any data from the system,
E.g. Get all agencies of users
Events are usually result of Commands used to perform soem side effects.
Each element has its own pipeline.

# Command pipeline,
Controller creates a command ->

 puts it on command bus ->
 
  command bus identify the relevant handler for the command and delegates ->
  
   command handler validate the command - domain logic is executed - gives the control back to command handler ->
   
     command handler calls repository in peristance layer to save the data ->
     
       emits events if needed 
     
# Query pipeline
Controller creates a query ->

 sends the query to a query processor ->
 
  query processor identify the relevant handler for the query and delegates ->
  
    query gets executed in the persistance layer - data is returned.

# System
System is divided into 2 major parts
1. Framework / Base (this repository)
2. Business Contexts (BC / Modules) (the one you can build on top of it)

# Framework components
1. Foundation - Foundation elements of our architecure 
2. Shared Kernel - Bae of a domain layer 
3. Application and Interface - App. layer consisting command handlers (Gates, only through this the ext. world access our domain)
4. Peristance and Interface - Peristance layer containing repository and query handlers
                            - on read side of cqrs we allow to expose our DTO / State directly via query handlers
5. Workflow - To support any future workflows
6. Utilities - Common utils for our app.

# Module design
There will be 1 main module for each system called as Core Module.This is the hear of our system.
There can be as many modules depends on the requirement.
Note: Modules are same as Bounded Context in domain driven design.

# Components of a module / context
State - Classes that contain the state (Entity framework poco classes)  (Core.State)
Main - Domain logic (Core)
Application - Core.Application
Perisistance - Core.Perisistance

# Highlights:
Clear separation of concerns. 
Async facility throught out
Application is horizontally scalable, 
Circuit breaker using polly
Multitenancy support

# Security flow:
-User tries to access application ->

  -Server issues a XSRF token ->
  
    -User gets Redericted to login ->
    
      -User login with credentials ->
      
       - Server authenticates with the provided credentials and issues a JWT token if valid ->
       
          -The token contains the user identity - This token is then used along with the XSRF toke
          n in subsequent request to the  application ->
          
           - Application first checks the XSRF token to pervent forgery (ANTI FORGERY) ->
           
             - After that application check for identity token (AUTHENTICATION) - >
             
               - Then based upon the requested action the server check the user rights (AUTHORIZATION) -> 
               
                 - Application can now perform the action.


# COMMAND REFERENCE

PACKAGGE MANAGER EF https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell

for core 1.1  use: EF Add-Migration -Name Initial-Create -Context MyContext -Project My.Core.Persistance -StartupProject My.Web

DOT NET

dotnet --version
dotnet build
dotnet test
dotnet run

ENTITY FRAMEWORK https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet

dotnet ef migrations add SystemIdTypeChange
dotnet ef database update
dotnet ef database update <previous-migration-name>

To completely remove all migrations and start all over again, do the following:

dotnet ef database update 0
dotnet ef migrations remove

To script
dotnet ef migrations script Migrationname AspNetIdentity -o d:\patch_script_for_migrationname.txt

