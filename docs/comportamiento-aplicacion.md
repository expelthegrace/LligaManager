# Descripción del comportamiento de la aplicación

Este documento describe el comportamiento esperado de la aplicación de gestión de una liga de fútbol, sin depender de una tecnología concreta ni de un framework específico. Su objetivo es definir la lógica de negocio, los flujos de usuario y las reglas de funcionamiento para que puedan implementarse en cualquier stack técnico.

## 1. Propósito de la aplicación

La aplicación debe permitir gestionar de forma centralizada una liga deportiva, con especial enfoque en:

- la creación y mantenimiento de equipos,
- la organización de jornadas y partidos,
- la gestión de resultados, puntos y clasificaciones,
- el seguimiento del estado general de la competición,
- y la consulta rápida de información relevante para usuarios y administradores.

La herramienta debe ser útil tanto para una administración sencilla como para un entorno más completo, manteniendo un modelo claro y consistente de datos.

## 2. Usuarios previstos

La solución puede estar orientada a varios perfiles:

- Administrador de la liga: crea equipos, organiza el calendario, gestiona jornadas y valida resultados.
- Usuario de consulta: revisa clasificaciones, próximos partidos y datos generales.
- Usuario de edición o gestión: registra resultados y actualiza información de jugadores o equipos si se habilita ese nivel de acceso.

En cualquier caso, la lógica de la aplicación debe ser clara, segura y fácil de validar visualmente.

## 3. Principios de comportamiento

### 3.1. Agnosticidad tecnológica

La aplicación debe definirse por sus reglas de negocio, no por una implementación concreta. Esto implica que:

- no depende de un lenguaje de programación específico,
- no exige un tipo concreto de base de datos,
- no asume una interfaz gráfica concreta,
- ni una arquitectura particular.

La implementación puede ser web, escritorio, móvil o local, siempre que cumpla las mismas reglas y ofrezca la misma experiencia funcional.

### 3.2. Consistencia de datos

Toda operación que modifique el estado de la liga debe producir un resultado coherente. No puede haber:

- equipos duplicados,
- partidos con fechas inconsistentes,
- resultados incompatibles con el formato de competición,
- clasificaciones calculadas con información desactualizada.

### 3.3. Transparencia

El sistema debe mostrar la información relevante sin ambigüedad. Si hay un cambio de estado, debe quedar reflejado en la interfaz o en la capa de presentación correspondiente.

## 4. Entidades principales

### 4.1. Equipo

Un equipo representa una entidad deportiva participante en la liga.

Debe incluir, como mínimo:

- identificador único,
- nombre,
- posible abreviatura o alias,
- color o distintivo visual,
- estado activo/inactivo,
- y una referencia a su participación en la competición.

El sistema debe evitar duplicados de nombre o identificadores equivalentes.

### 4.2. Jugador

Un jugador puede pertenecer a un equipo. Dependiendo del alcance del producto, puede incluirse en el modelo o mantenerse como dato opcional.

Debe poder registrarse:

- nombre,
- apellidos,
- posible número,
- equipo al que pertenece,
- estado de alta/baja.

### 4.3. Jornada

Una jornada agrupa partidos que se juegan en un mismo bloque de competición.

Debe permitir:

- definir el número o nombre de la jornada,
- establecer fechas de inicio y fin,
- asociar todos los partidos que la componen,
- indicar si está pendiente, en curso o finalizada.

### 4.4. Partido

Un partido representa un enfrentamiento entre dos equipos.

Debe contener:

- identificador único,
- equipo local,
- equipo visitante,
- jornada asociada,
- fecha y hora,
- estado del partido,
- resultado final o marcador.

El sistema debe asegurar que un equipo no pueda jugar contra sí mismo y que no se puedan generar partidos inválidos.

### 4.5. Competición

La competición define el marco general de la liga.

Debe permitir:

- definir el nombre de la liga,
- establecer la modalidad (todos contra todos, grupos, eliminatorias, etc.),
- configurar reglas de puntuación,
- indicar el estado actual de la competición.

## 5. Reglas de negocio principales

### 5.1. Creación de equipos

- Un equipo debe crearse con un nombre válido y único.
- El sistema debe permitir activar o desactivar un equipo sin perder su historial.
- Si un equipo participa en partidos programados, no debe permitirse borrarlo de forma irreversible sin una confirmación explícita.

### 5.2. Organización de jornadas

- Una jornada debe poder crearse sin depender del estado del resto.
- La aplicación debe permitir asignar partidos a una jornada concreta.
- No debe permitirse un partido sin dos equipos distintos.
- Una jornada cerrada no debe poder modificarse en sus resultados sin un proceso explícito de edición autorizada.

### 5.3. Registro de resultados

- El resultado debe reflejar el marcador final del partido.
- El sistema debe validar que la edición del marcador se realice solo en estados permitidos.
- Si un resultado se registra, la clasificación debe actualizarse automáticamente.
- Si se corrige un resultado previo, la clasificación debe recalcularse en consecuencia.

### 5.4. Clasificación

La tabla general debe calcularse a partir de los resultados de la competición.

Debe incluir, como mínimo:

- posición,
- equipo,
- partidos jugados,
- ganados,
- empatados,
- perdidos,
- goles a favor,
- goles en contra,
- diferencia de goles,
- puntos.

La clasificación debe mantenerse consistente con los resultados actuales y debe reflejar el orden correcto según las reglas definidas por la competición.

### 5.5. Validación de datos

El sistema debe impedir:

- fechas de partidos vacías o incoherentes,
- nombres duplicados,
- equipos no existentes asociados a partidas,
- resultados incompletos o inconsistentes,
- clasificaciones calculadas con información incompleta.

## 6. Flujos de uso esperados

### 6.1. Alta de una nueva liga

1. El administrador crea una nueva competición.
2. Define el nombre y la configuración base.
3. Añade los equipos participantes.
4. Genera las jornadas o el calendario.
5. Verifica el estado general y deja la liga preparada para jugar.

### 6.2. Programación de partidos

1. El administrador crea una jornada.
2. Selecciona dos equipos válidos.
3. Asigna fecha y hora.
4. Guarda la programación.
5. El sistema refleja el partido en el calendario y en la vista de la jornada.

### 6.3. Cierre de una jornada

1. Se registran todos los resultados.
2. El sistema calcula la clasificación actualizada.
3. La jornada queda marcada como finalizada.
4. La aplicación presenta el estado nuevo sobre la tabla general.

### 6.4. Consulta de resultados

- Un usuario puede consultar la clasificación general.
- Puede ver partidos de una jornada concreta.
- Puede revisar datos históricos de resultados anteriores.
- Puede diferenciar entre competiciones activas y cerradas.

## 7. Comportamiento en errores y casos límite

La aplicación debe manejar con elegancia situaciones no esperadas:

- resultado no válido o incompleto,
- equipo inexistente al intentar registrar un partido,
- jornada duplicada,
- modificación de datos en una competición cerrada,
- registro de un partido con fecha en el pasado si la regla de negocio no lo permite,
- cambios repetidos en los mismos datos.

En estos casos, el sistema debe mostrar un mensaje claro y evitar que se almacene información inválida.

## 8. Experiencia de usuario

Aunque esta especificación es agnóstica a la tecnología, la experiencia real debe ser intuitiva y segura:

- las acciones críticas deben requerir confirmación,
- la información debe presentarse con claridad,
- los estados deben ser visibles (pendiente, en curso, finalizado),
- las clasificaciones y resultados deben ser fáciles de interpretar,
- la navegación entre competiciones, jornadas y partidos debe ser directa.

## 9. Requisitos funcionales resumidos

La aplicación debe poder:

- crear y gestionar competiciones,
- registrar equipos,
- crear jornadas,
- programar partidos,
- registrar resultados,
- recalcular clasificaciones,
- consultar estado de la liga,
- mantener la integridad de los datos,
- y ofrecer una visión clara del desarrollo de la competición.

## 10. Objetivo final

La aplicación debe actuar como una herramienta fiable para gestionar una liga de fútbol, con una lógica transparente, consistente y extensible. Su valor no reside en la tecnología elegida, sino en la calidad de las reglas y del flujo de trabajo que permitan controlar la competición de manera ordenada y entendible.
