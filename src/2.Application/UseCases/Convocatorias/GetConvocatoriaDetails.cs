using LligaManager.Application.Abstractions;
using LligaManager.Domain.Entities;
using LligaManager.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LligaManager.Application.UseCases.Convocatorias;

public sealed class GetConvocatoriaDetails(
    IConvocatoriaRepository convocatoriaRepository,
    ILogger<GetConvocatoriaDetails>? logger = null)
{
    private readonly ILogger<GetConvocatoriaDetails> logger =
        logger ?? NullLogger<GetConvocatoriaDetails>.Instance;

    public async Task<Convocatoria> ExecuteAsync(
        ConvocatoriaId convocatoriaId,
        CancellationToken cancellationToken = default)
    {
        var convocatoria = await convocatoriaRepository.GetByIdAsync(
                convocatoriaId,
                true,
                cancellationToken)
            ?? throw new KeyNotFoundException($"Convocatoria '{convocatoriaId}' was not found.");
        logger.LogInformation("Loaded convocatoria {ConvocatoriaId}.", convocatoria.Id);
        return convocatoria;
    }
}
