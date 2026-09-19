using LligaManager.Application.Abstractions;
using LligaManager.Domain.Entities;
using LligaManager.Domain.Enums;
using LligaManager.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LligaManager.Application.UseCases.Convocatorias;

public sealed record UpdateConvocatoriaRequest(
    ConvocatoriaId ConvocatoriaId,
    DateOnly Date,
    ConvocatoriaStatus Status,
    string? Observations = null);

public sealed class UpdateConvocatoria(
    IConvocatoriaRepository convocatoriaRepository,
    ILogger<UpdateConvocatoria>? logger = null)
{
    private readonly ILogger<UpdateConvocatoria> logger = logger ?? NullLogger<UpdateConvocatoria>.Instance;

    public async Task<Convocatoria> ExecuteAsync(
        UpdateConvocatoriaRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var convocatoria = await convocatoriaRepository.GetByIdAsync(request.ConvocatoriaId, true, cancellationToken)
            ?? throw new KeyNotFoundException($"Convocatoria '{request.ConvocatoriaId}' was not found.");
        var date = new ConvocatoriaDate(request.Date);

        if (date != convocatoria.Date &&
            await convocatoriaRepository.ExistsByDateAsync(date, cancellationToken))
            throw new InvalidOperationException($"A convocatoria already exists for {date}.");

        convocatoria.UpdateDetails(date, new Observations(request.Observations));
        convocatoria.SetStatus(request.Status);
        await convocatoriaRepository.UpdateAsync(convocatoria, cancellationToken);
        logger.LogInformation("Convocatoria {ConvocatoriaId} updated.", convocatoria.Id);
        return convocatoria;
    }
}
