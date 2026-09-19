# LligaManager

Proyecto personal para gestionar una liga de fútbol desde Windows 11.

## Tecnologías

- C# sobre .NET 10
- SQLite mediante `Microsoft.Data.Sqlite`
- Interfaz prevista: WinUI 3 con Windows App SDK

La solución se organiza en tres capas:

- `src/1.Domain`: entidades, objetos de valor y servicios de dominio.
- `src/2.Application`: casos de uso y abstracciones de aplicación.
- `src/3.Infrastructure`: persistencia SQLite, UI, localización y logging.

Los tests se encuentran en `tests/LligaManager.Tests`. La aplicación WinUI 3
se encuentra en `src/3.Infrastructure/LligaManager.UI` y usa Windows App SDK.
Para compilarla desde Visual Studio es necesario instalar la carga de trabajo
**Desktop development with C++** y el Windows 10/11 SDK.

Para trabajar desde Visual Studio, abre `LligaManager.sln` y selecciona la
plataforma `x64`. `LligaManager.slnx` se conserva como formato XML de solución,
pero la solución `.sln` incluye las configuraciones tradicionales de Visual
Studio necesarias para compilar la aplicación WinUI.

Rider puede compilar la solución usando su MSBuild del SDK de .NET. El proyecto
UI incluye una ruta condicional a las tareas AppX/PRI instaladas por Visual
Studio para que también funcione con ese MSBuild.

## Desarrollo

```powershell
dotnet restore
dotnet build
```

Repositorio: https://github.com/expelthegrace/LligaManager
