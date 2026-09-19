using LligaManager.Application.Abstractions;
using LligaManager.Domain.Entities;
using LligaManager.Domain.Services;
using LligaManager.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LligaManager.Application.UseCases.Convocatorias;

public sealed class ExecuteConvocatoriaRaffle(
    IConvocatoriaRepository convocatoriaRepository,
    IPlayerRepository playerRepository,
    IConvocatoriaConfigurationProvider configurationProvider,
    ILogger<ExecuteConvocatoriaRaffle>? logger = null)
{
    private readonly ILogger<ExecuteConvocatoriaRaffle> logger =
        logger ?? NullLogger<ExecuteConvocatoriaRaffle>.Instance;

    public async Task<Convocatoria> ExecuteAsync(
        ConvocatoriaId convocatoriaId,
        CancellationToken cancellationToken = default)
    {
        var convocatoria = await convocatoriaRepository.GetByIdAsync(
                convocatoriaId,
                true,
                cancellationToken)
            ?? throw new KeyNotFoundException($"Convocatoria '{convocatoriaId}' was not found.");
        var candidates = await playerRepository.GetAllAvailableAsync(cancellationToken);
        var configuration = configurationProvider.Load();

        new Raffle().Execute(candidates, convocatoria, configuration);
        await convocatoriaRepository.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Raffle executed for convocatoria {ConvocatoriaId}: {WinnerCount} winners, {LoserCount} losers.",
            convocatoria.Id,
            convocatoria.RaffleWinners.Count,
            convocatoria.RaffleLosers.Count);
        return convocatoria;
    }
}
