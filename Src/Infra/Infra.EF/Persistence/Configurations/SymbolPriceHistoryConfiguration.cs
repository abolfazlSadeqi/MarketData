
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entites;

namespace Infra.EF.Persistence.Configurations;

public class SymbolPriceHistoryConfiguration : IEntityTypeConfiguration<SymbolPriceHistory>
{
    public void Configure(EntityTypeBuilder<SymbolPriceHistory> builder)
    {
        builder.ToTable("SymbolPriceHistorys");
        builder.HasKey(x => x.Id);
    }
}

