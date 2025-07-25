
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entites;

namespace Infra.EF.Persistence.Configurations;

public class SymbolPriceConfiguration : IEntityTypeConfiguration<SymbolPrice>
{
    public void Configure(EntityTypeBuilder<SymbolPrice> builder)
    {
        builder.ToTable("SymbolPrices");
        builder.HasKey(x => x.Id);
    }
}

