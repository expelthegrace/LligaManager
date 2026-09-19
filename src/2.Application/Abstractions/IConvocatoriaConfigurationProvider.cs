using LligaManager.Domain.Services;

namespace LligaManager.Application.Abstractions;

public interface IConvocatoriaConfigurationProvider
{
    ConvocatoriaConfiguration Load();
}
