using LligaManager.Domain.Entities;
using LligaManager.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LligaManager.Infrastructure.Persistence;

public sealed class ConvocatoriaConfiguration : IEntityTypeConfiguration<Convocatoria>
{
    public void Configure(EntityTypeBuilder<Convocatoria> builder)
    {
        builder.ToTable("Convocatorias");
        builder.HasKey(convocatoria => convocatoria.Id);
        builder.Property(convocatoria => convocatoria.Id)
            .HasConversion(id => id.Value, value => new ConvocatoriaId(value));
        builder.Property(convocatoria => convocatoria.Date)
            .HasConversion(date => date.Value, value => new ConvocatoriaDate(value))
            .IsRequired();
        builder.Property(convocatoria => convocatoria.Status).HasConversion<string>().IsRequired();
        builder.Property(convocatoria => convocatoria.Observations)
            .HasConversion(value => value.Value, value => new Observations(value))
            .IsRequired();

        builder.HasMany(convocatoria => convocatoria.CurrentPlayers)
            .WithMany()
            .UsingEntity(join => join.ToTable("ConvocatoriaCurrentPlayers"));
        builder.HasMany(convocatoria => convocatoria.RaffleWinners)
            .WithMany()
            .UsingEntity(join => join.ToTable("ConvocatoriaRaffleWinners"));
        builder.HasMany(convocatoria => convocatoria.RaffleLosers)
            .WithMany()
            .UsingEntity(join => join.ToTable("ConvocatoriaRaffleLosers"));
    }
}
