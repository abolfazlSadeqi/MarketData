
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entites;

namespace Infra.EF.Persistence.Configurations;

public class SymbolAverageConfiguration : IEntityTypeConfiguration<SymbolAverage>
{
    public void Configure(EntityTypeBuilder<SymbolAverage> builder)
    {
        builder.ToTable("SymbolAverages");
        builder.HasKey(x => x.Id);
    }
}

