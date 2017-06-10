# ErazerCQRS
My learning steps into the world of CQRS and EventSourcing

## Features

- CQRS
  - Read side -> **MongoDb**
    - TODO Validation of 'Queries'
    - TODO Logging with aid of [Mediatr Behaviours](https://github.com/jbogard/MediatR/wiki/Behaviors)
  - Write side -> Eventsourcing -> **Marten** (PostgreSQL)
    - TODO Validation of 'Commands'
    - TODO Caching of Aggregate -> **REDIS**
 - Decoupling Read side & Write side
    - Azure Servicebus
    - Consequence: Eventual Consistency 

- TODO Angular (> 4)
  - Using Angular CLI
  - Using ngrx-store
  - Using websockets

## Resources

- [https://github.com/gautema/CQRSlite](https://github.com/gautema/CQRSlite)
