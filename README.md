# ErazerCQRS
My learning steps into the world of CQRS and EventSourcing

## Features

- CQRS
  - Read side -> **MongoDb**
    - TODO Validation of 'Queries'
    - TODO Logging with aid of [Mediatr Behaviours](https://github.com/jbogard/MediatR/wiki/Behaviors)
  - Write side -> Eventsourcing -> [**GetEventStore**](https://geteventstore.com)
    - TODO Validation of 'Commands'
    - Caching of Aggregate -> **REDIS**
	- DDD
  - Decoupling Read side & Write side
    - Azure Servicebus
    - Consequence: Eventual Consistency 

- TODO Angular (> 4)
  - Currently in development on a seperate git repo.
  	- Will become public soon.
  - Using Angular CLI
  - Using ngrx-store
  - Using websockets
  	- Check issue [#7](/../../issues/7)

- TODO Docker
- TODO Authentication and Authorization

## Resources

- [https://github.com/gautema/CQRSlite](https://github.com/gautema/CQRSlite)
