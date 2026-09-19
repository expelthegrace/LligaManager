using LligaManager.Domain.Entities;
using LligaManager.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LligaManager.Infrastructure.Persistence;

public sealed class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("Players");
        builder.HasKey(player => player.Id);
        builder.Property(player => player.Id)
            .HasConversion(id => id.Value, value => new PlayerId(value));
        builder.Property(player => player.Name)
            .HasConversion(name => name.Value, value => new PlayerName(value))
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(player => player.Roles).HasConversion<int>().IsRequired();
        builder.Property(player => player.Status).HasConversion<string>().IsRequired();
        builder.Property(player => player.Priority).HasConversion<string>().IsRequired();
        builder.Property(player => player.Observations)
            .HasConversion(value => value.Value, value => new Observations(value))
            .IsRequired();
    }
}
