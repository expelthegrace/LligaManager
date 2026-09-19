using LligaManager.Domain.Entities;
using LligaManager.Domain.ValueObjects;

namespace LligaManager.Application.Abstractions;

public interface IConvocatoriaRepository
{
    Task<bool> ExistsByDateAsync(ConvocatoriaDate date, CancellationToken cancellationToken = default);
    Task<Convocatoria?> GetByIdAsync(ConvocatoriaId id, bool includeRaffleDetails = false, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Convocatoria>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Convocatoria convocatoria, CancellationToken cancellationToken = default);
    Task UpdateAsync(Convocatoria convocatoria, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
