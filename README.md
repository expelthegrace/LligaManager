# LligaManager

Proyecto personal para gestionar una liga de fútbol desde Windows 11.

## Tecnologías

- C# sobre .NET 10
- SQLite mediante `Microsoft.Data.Sqlite`
- Interfaz prevista: WinUI 3 con Windows App SDK

La solución contiene actualmente `LligaManager.Core`, donde se centralizarán el
dominio y el acceso a datos. La aplicación WinUI 3 se añadirá desde Visual
Studio usando la plantilla **Blank App, Packaged (WinUI 3 in Desktop)**.

## Desarrollo

```powershell
dotnet restore
dotnet build
```

Repositorio: https://github.com/expelthegrace/LligaManager
