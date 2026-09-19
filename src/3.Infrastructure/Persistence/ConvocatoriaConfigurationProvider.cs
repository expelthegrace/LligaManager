using System.Text.Json;
using LligaManager.Domain.Services;

namespace LligaManager.Infrastructure.Persistence;

public sealed class ConvocatoriaConfigurationProvider
{
    private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web);

    public ConvocatoriaConfiguration Load(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Convocatoria configuration file was not found.", path);

        var json = File.ReadAllText(path);
        var configuration = JsonSerializer.Deserialize<ConvocatoriaConfiguration>(json, jsonOptions);
        return configuration ?? throw new InvalidDataException("Convocatoria configuration is empty.");
    }
}
