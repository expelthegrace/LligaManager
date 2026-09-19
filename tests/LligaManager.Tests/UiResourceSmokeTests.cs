using System.Xml.Linq;
using Xunit;

namespace LligaManager.Tests;

public sealed class UiResourceSmokeTests
{
    [Fact]
    public void MainWindow_references_only_application_resources_defined_in_app_xaml()
    {
        var repositoryRoot = FindRepositoryRoot();
        var app = XDocument.Load(Path.Combine(
            repositoryRoot,
            "src", "3.Infrastructure", "LligaManager.UI", "App.xaml"));
        var mainWindow = XDocument.Load(Path.Combine(
            repositoryRoot,
            "src", "3.Infrastructure", "LligaManager.UI", "MainWindow.xaml"));

        var definedResources = app
            .Descendants()
            .Where(element => element.Attribute(XName.Get("Key", "http://schemas.microsoft.com/winfx/2006/xaml")) is not null)
            .Select(element => element.Attribute(XName.Get("Key", "http://schemas.microsoft.com/winfx/2006/xaml"))!.Value)
            .Where(key => key.StartsWith("LligaManager", StringComparison.Ordinal))
            .ToHashSet(StringComparer.Ordinal);

        var referencedApplicationResources = mainWindow
            .Descendants()
            .Attributes()
            .Where(attribute => attribute.Value.StartsWith("{ThemeResource ", StringComparison.Ordinal))
            .Select(attribute => attribute.Value["{ThemeResource ".Length..^1])
            .Where(key => key.StartsWith("LligaManager", StringComparison.Ordinal))
            .ToHashSet(StringComparer.Ordinal);

        Assert.Subset(referencedApplicationResources, definedResources);
    }

    [Fact]
    public void App_defines_light_and_dark_brushes_for_the_application_cards()
    {
        var repositoryRoot = FindRepositoryRoot();
        var app = XDocument.Load(Path.Combine(
            repositoryRoot,
            "src", "3.Infrastructure", "LligaManager.UI", "App.xaml"));
        var appText = app.ToString(SaveOptions.DisableFormatting);

        Assert.Contains("x:Key=\"Light\"", appText);
        Assert.Contains("x:Key=\"Dark\"", appText);
        Assert.Contains("x:Key=\"LligaManagerSidebarBrush\"", appText);
        Assert.Contains("x:Key=\"LligaManagerCardBrush\"", appText);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "LligaManager.sln")))
            directory = directory.Parent;

        return directory?.FullName
            ?? throw new DirectoryNotFoundException("Repository root could not be found.");
    }
}
