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
se añadirá dentro de Infrastructure desde Visual Studio usando la plantilla
**Blank App, Packaged (WinUI 3 in Desktop)**.

## Desarrollo

```powershell
dotnet restore
dotnet build
```

Repositorio: https://github.com/expelthegrace/LligaManager
