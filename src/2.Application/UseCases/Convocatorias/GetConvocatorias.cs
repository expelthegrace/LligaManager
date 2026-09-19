using LligaManager.Application.Abstractions;
using LligaManager.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LligaManager.Application.UseCases.Convocatorias;

public sealed class GetConvocatorias(
    IConvocatoriaRepository convocatoriaRepository,
    ILogger<GetConvocatorias>? logger = null)
{
    private readonly ILogger<GetConvocatorias> logger = logger ?? NullLogger<GetConvocatorias>.Instance;

    public async Task<IReadOnlyCollection<Convocatoria>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        var convocatorias = await convocatoriaRepository.GetAllAsync(cancellationToken);
        logger.LogInformation("Loaded {ConvocatoriaCount} convocatorias.", convocatorias.Count);
        return convocatorias;
    }
}
