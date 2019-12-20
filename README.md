# ErazerCQRS
My learning steps into the world of CQRS and EventSourcing

## Features

- CQRS
  - Read side -> **MongoDb**
    - TODO Validation of 'Queries'
    - TODO Logging with aid of [Mediatr Behaviours](https://github.com/jbogard/MediatR/wiki/Behaviors)
  - Write side -> Eventsourcing -> [**SqlStreamStore**](https://github.com/SQLStreamStore/SQLStreamStore)
    - TODO Validation of 'Commands'
    - DDD
    - Caching of Aggregate -> **REDIS**
  - Decoupling Read side & Write side
    - Subscription based
  - Communication 
    - RabbitMQ -> Multiple applications without tight coupling
      - MassTransit
      - Events
      - Commands
    - gRPC -> TODO
      - RPC calls

- Websockets
  - SignalR

- Angular (> 4)
  - TODO Upgrade to Angular 9
  - TODO Enable Ivy

- TODO Docker
- TODO Authentication and Authorization

## Resources

- [https://github.com/gautema/CQRSlite](https://github.com/gautema/CQRSlite)
