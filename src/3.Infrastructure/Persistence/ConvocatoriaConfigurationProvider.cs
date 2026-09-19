using System.Text.Json;
using LligaManager.Domain.Services;
using LligaManager.Application.Abstractions;

namespace LligaManager.Infrastructure.Persistence;

public sealed class ConvocatoriaConfigurationProvider : IConvocatoriaConfigurationProvider
{
    private readonly string path;
    private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web);

    public ConvocatoriaConfigurationProvider(string path)
    {
        this.path = path;
    }

    public LligaManager.Domain.Services.ConvocatoriaConfiguration Load() => Load(path);

    public LligaManager.Domain.Services.ConvocatoriaConfiguration Load(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Convocatoria configuration file was not found.", path);

        var json = File.ReadAllText(path);
        var configuration =
            JsonSerializer.Deserialize<LligaManager.Domain.Services.ConvocatoriaConfiguration>(
                json,
                jsonOptions);
        return configuration ?? throw new InvalidDataException("Convocatoria configuration is empty.");
    }
}
