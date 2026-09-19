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