# Chat Room Interface

Chat room interface in which the user can view chat history at varying levels of time-based aggregation.
The project was built as a recruitment task.
The assumptions will be described later in the documentation.


Application has been built as a Web API with the use of the .NET 6.
The idea behind the recruitment task was to present the coding style and I assumed that it is the best way of presenting my coding skills.
The assumption of the task was to devote about 6 hours of work.
The result was not supposed to be a production-ready application.
As described earlier, the application is to show the skills and approach to writing code.
Therefore, there are many things to do before an application is declared production-ready.
The things that I see to be finished first are described in the further part of the documentation.
## Features

* Users can view chat history in descending order.
* Users can aggregate chat history at varying levels of time.
* Users can add 4 types of chat events:
    * enter-the-room,
    * leave-the-room,
    * comment,
    * high-five-another-user.

## Data manipulation

### Persistance
The data is stored in memory, but it is ready to be extended.
There is a memory storage interface to be implemented and a new way of persistence can be used after the change of the DI registration.

### Sorting
Data is sorted without additional sorting because of a way of storing it.

### Aggregation
Aggregation is done with the use of the LINQ.

### Rendered
Rendering is prepared to be clear and easy to extend.

## Architecture

The application architecture is inspired by [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
The application consists of 4 projects with source code and 2 with tests (one with unit tests and one with integration tests).
The project structure is as presented below:

```
                             Domain
                                ^
                                |
                            Application <----------- Application.UnitTests
                                ^
                                |
                            Infrastracture
                                ^
                                |
                              WebAPI <-------------- WebAPI.IntegrationTests
```

## Starting application

To start the application the Web.API project must be started.
The application interface is exposed on the Swagger page.
## TODOs
* Aggregation is not done exactly the same as it was described. It needed additional time to finish it and based on the time spent I decided that having the current architecture complexity is not big and I left it as a TODO.
* There is no monitoring and proper, consistent logging in the application.
* Model validation needs to be added.
* Exception handling middleware would be a great enhancement for the application.
* Tests are prepared only to be representative and don't cover the code well. There is a need to add more tests for the project.
* The inMemory (singleton collection) persistence layer is good for fast proof of concept. To make the app production-ready it would be needed to create some other way of storing data.
* Storing data interface is created without asynchronous signatures because for that implementation it was not needed. It should be changed when connecting with a database or storing data in a file.
## Authors

- [Mateusz Kapiszewski](https://github.com/MatKapp)

