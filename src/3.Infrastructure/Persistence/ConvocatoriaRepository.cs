using LligaManager.Application.Abstractions;
using LligaManager.Domain.Entities;
using LligaManager.Domain.Enums;
using LligaManager.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LligaManager.Infrastructure.Persistence;

public sealed class ConvocatoriaRepository(
    LligaManagerDbContext dbContext,
    ILogger<ConvocatoriaRepository>? logger = null) : IConvocatoriaRepository
{
    private readonly ILogger<ConvocatoriaRepository> logger =
        logger ?? NullLogger<ConvocatoriaRepository>.Instance;

    public Task<bool> ExistsByDateAsync(
        ConvocatoriaDate date,
        CancellationToken cancellationToken = default) =>
        dbContext.Convocatorias.AnyAsync(convocatoria => convocatoria.Date == date, cancellationToken);

    public async Task<Convocatoria?> GetByIdAsync(
        ConvocatoriaId id,
        bool includeRaffleDetails = false,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Convocatorias.AsQueryable();
        if (includeRaffleDetails)
        {
            query = query
                .Include(convocatoria => convocatoria.CurrentPlayers)
                .Include(convocatoria => convocatoria.RaffleWinners)
                .Include(convocatoria => convocatoria.RaffleLosers);
        }

        return await query.SingleOrDefaultAsync(convocatoria => convocatoria.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Convocatoria>> GetAllAsync(
        CancellationToken cancellationToken = default) =>
        await dbContext.Convocatorias
            .OrderByDescending(convocatoria => convocatoria.Date.Value)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(
        Convocatoria convocatoria,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Convocatorias.AddAsync(convocatoria, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Convocatoria {ConvocatoriaId} persisted.", convocatoria.Id);
    }

    public async Task UpdateAsync(
        Convocatoria convocatoria,
        CancellationToken cancellationToken = default)
    {
        dbContext.Convocatorias.Update(convocatoria);
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Convocatoria {ConvocatoriaId} updated.", convocatoria.Id);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Convocatoria changes persisted.");
    }
}
