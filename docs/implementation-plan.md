#architecture
arquitectura monolitica de clean arquitecture basada en 3 capas:
- infrastructure
- application
- domain

diseño basado en DDD

el proyecto incluye tests

#layers:
infrastructure:
- UI. debe tener un sistema de localization.
- class read Repository pattern for SQLite. one per entity, ubiquitous names for methods (example: GetAllAvailablePlayers)
- todo relacionado con librerias

application:
- usecases: they load entities, orquestrate calling the domain without adding business logic, persists and the end if OK.

domain:
- entities
- value objects, smart value objects
- services (functional classes that orquestrate business logic actions that involves calls to different entities and add business logic rules).

#logs:
ui: no logs in UI code. 
persistence: all changes saved have to be logged. not verbose.
usecases: log happy path, calls to domain, result from the domain actions, log result/error. error logs should be popupped en el UI
domain: raffle inputs and result is logged

logs into different files:
- persistence logs
- all the rest

#persistence
use of SQLite. Usecases call IRepository to load entities. to create entities said entities will have its Factory methods with obiquitous language.
.log file to log all persistence changes.


#1.Domain
>entities
player: name, status (available, unavailable, removed), role multichoice (interior, exterior, base), priority: normal (default) and high, observations (editable text)
convocatoria: fecha, actual players , raffle winners, raffle losers, status:pending, played, descanso. observations (editable text)
- invariants: raffle winners or losers cant be changed once assigned. only can be assigned trough domain raffle service.
>value objects
all atributes of entitiesG
ConvocatoriaConfiguration: maximum players. stored in ConvocatoriaConfig.json

>services
Raffle:
- takes a list of player candidates, a convocatoria, loads COnvocatoriaConfiguration, modifies (public method calls) convocatoria's current players, winner players and loser players.
- priority: 1) available players marked as high prio, random if they are more than max players per convocatoria. 2) rest of the available players also random between them.
- winners get their prio set to normal
- losers get their prio set to  high

 #2.application
 usecases, these usecases should be the orquestration for the user inputs. they also should load entities, orquestrate domain calls, catch exceptions (and replicate them up) and manage persistence. 
 note: UI has petitions to modify raw values. I don't want public setters for entities. always use ubiquitous language methods and ensure invariants if needed. 

## Application use cases

A use case represents a user intention or an application workflow, not every
individual property that can be edited. When a screen edits several related
values, they should be handled by one atomic use case that loads the entity,
invokes its ubiquitous-language methods, and persists the complete change.

The initial use cases for the current domain are:

### Player

- `CreatePlayer`
- `UpdatePlayer`
- `GetPlayers`
- `GetPlayerDetails`

### Convocatoria

- `CreateConvocatoria`
- `UpdateConvocatoria`
- `GetConvocatorias`
- `GetConvocatoriaDetails`
- `ExecuteConvocatoriaRaffle`

`ExecuteConvocatoriaRaffle` is a separate workflow because it loads candidates,
invokes the Domain `Raffle` service, changes several entities, and persists the
result. Player priority changes are not exposed as an independent UI use case;
they are controlled by the raffle rules.

Use cases must:

- receive raw input models suitable for the UI;
- load entities through Application repository abstractions;
- create entities through Domain factories;
- call Domain methods instead of using public setters;
- persist successful changes atomically;
- log the application flow and propagate errors to the UI.

Repository implementations, SQLite, EF Core, and persistence logging belong to
Infrastructure. The same grouping principle will be applied later to
competition, team, matchday, match, result, and standings workflows when those
Domain entities exist.

### Implemented vertical slice

The current Application layer implements:

- `CreatePlayer`
- `UpdatePlayer`
- `GetPlayers`
- `GetPlayerDetails`
- `CreateConvocatoria`
- `UpdateConvocatoria`
- `GetConvocatorias`
- `GetConvocatoriaDetails`
- `ExecuteConvocatoriaRaffle`

The player workflows validate duplicate names and use the Domain methods for
editable details and status. The convocatoria workflows validate duplicate
dates and use the Domain factory and update methods. The raffle workflow loads
available candidates and the convocatoria, obtains its configuration through
an Application abstraction, invokes the Domain `Raffle`, and persists the
result in one repository save operation.