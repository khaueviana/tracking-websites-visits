# Tracking websites visits project

## About The Project

Simple solution for tracking website visits.

## Built With
- ASP.NET Web API (.NET 8.0).
- Background Worker: for managing long-running tasks in the Consumer Service.
- RabbitMQ: robust messaging for applications.
- XUnit: testing tool for .NET.
- Moq: flexible mocking framework for unit tests.
- Fluent Assertions: provides a set of extension methods for more readable assertions.

## Releases

### [Alpha Version](https://github.com/khaueviana/tracking-websites-visits/releases/tag/alpha-version)

* The simplest version following the main requirements.

### [Beta Version](https://github.com/khaueviana/tracking-websites-visits/releases/tag/beta-version)

* A few improvements were added:
    * An event naming convention should be established along with a base contract named `IEvent`.
    * Application Service orchestrating the dependencies.
    * Encapsulate domain rules in the Domain Model layer, including Tracking Summary property and a Factory Method, to create the model correctly (taking the required field into account).
    * Dockerfile refactoring including multi-stages builds in Docker.

## Getting Started

To get a local copy up and running, follow these simple steps.

#### Prerequisites
- Visual Studio 2022 or later: for development and running the application locally.
- .NET 8.0 SDK or later: core framework for building and running the application.
- Docker: required for containerization and managing dependencies such as RabbitMQ.
- Docker Compose: for defining and running multi-container Docker applications.
- Make: for managing application and Docker commands.

#### Installation

Clone the repo:

```
git clone https://github.com/your_repo/payment-gateway.git
```

#### Running the Application

Use these commands to run the API and service:

```
make run-api
make run-service
```

#### Managing Dependencies

To set up infrastructure dependencies like RabbitMQ:

```
make infra
```

#### Running Unit Tests
```
dotnet test
```

## Usage

### Presentation.Api project
**[GET] /track**: Tracks user information based on the HTTP context and sends the data to the Message Broker.

### Consumer.Service project
Consumes and persists tracking messages sent by the Presentation.Api service.

## Roadmap
A few topics that could be discussed to improve this project:

### Technical Enhancements
- ~~Integrate code analysis tools, editor config and directory build properties for better code quality.~~ [DONE]
- ~~Classical Domain Driven Design: the project is not following the entire DDD structure, but it is prepared for it.~~ [DONE]
- [MassTransit](https://masstransit.io/documentation/transports/rabbitmq) can be used to manage the RabbitMQ broker rather than the current vanilla version, or even something like the [Kafka Flow](https://github.com/Farfetch/kafkaflow), which is a very good one for Kafka Events.
- [Renovate Bot](https://docs.renovatebot.com/) can be an option to manage the dependencies version.
- Input Validation: strengthen input validation mechanisms.
- Scheduled Events: implement a scheduling service like [Hangfire](https://www.hangfire.io/) for job management and resilience.
- Retry Patterns: integrate retry patterns for robust event processing.
- Error Handling for both projects.
- There are many shared components between the API and the Consumer, but we would have distinct solutions in a real-life scenario.

### Testing
- Expand Test Coverage: implement unit and integration tests as much as possible.

### Observability

- Logging: enhance logging capabilities for better traceability.
- Tracing and Correlation IDs: implement distributed tracing and correlation IDs for tracking transactions.
- Transaction Instrumentation: use tools like New Relic for detailed transaction monitoring.