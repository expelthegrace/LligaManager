using LligaManager.Application.Abstractions;
using LligaManager.Domain.Entities;
using LligaManager.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LligaManager.Application.UseCases.Convocatorias;

public sealed record CreateConvocatoriaRequest(DateOnly Date, string? Observations = null);

public sealed class CreateConvocatoria(
    IConvocatoriaRepository convocatoriaRepository,
    ILogger<CreateConvocatoria>? logger = null)
{
    private readonly ILogger<CreateConvocatoria> logger = logger ?? NullLogger<CreateConvocatoria>.Instance;

    public async Task<Convocatoria> ExecuteAsync(
        CreateConvocatoriaRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var date = new ConvocatoriaDate(request.Date);
        if (await convocatoriaRepository.ExistsByDateAsync(date, cancellationToken))
            throw new InvalidOperationException($"A convocatoria already exists for {date}.");

        var convocatoria = Convocatoria.Create(date, new Observations(request.Observations));
        await convocatoriaRepository.AddAsync(convocatoria, cancellationToken);
        logger.LogInformation("Convocatoria {ConvocatoriaId} created.", convocatoria.Id);
        return convocatoria;
    }
}
