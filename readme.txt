->  Initial Setup Database Query available in "SqlQuery.sql" file
->  Authentication 
    Login/Registration done
    Demo Accounts
    Admin
        email: "admin@example.com"
        password: "admin"
    User
        email: "test@example.com"
        password: "admin"
-> Authorization
    User & Admin Roles
    
# Clean Architecture Design    
-> Presentation/API Layer
    File Logging via Serilog
    Global Error handling via ExceptionHandler("/error")
    Define API endpoint here
    Call the Application Layer via MediatR.
-> Application Layer
    MediatR/CQRS pattern
    to call the logic and call the Infrastructure Layer
    define Dtos here
    Using Data Annotation for validation but validation handle on Presentation Layer.
    #we can also add validation logic here via the MediatR Pipeline Behaviours.
-> Infrastructure Layer
    implement the Service Interface of Application Layer like JwtTokenGenerator, DateTimeProvider, EnumExtensions, etc...
    Dapper
    Using Generic Repository with UnitOfWork
    Using transaction for writing and reading from database
-> Domain Layer
    Define table Entities and decorate with the Attributes
    Using IEntity Interface and Derived most of the Entities from Entity class that implement the IEntity
    Define Enums here.