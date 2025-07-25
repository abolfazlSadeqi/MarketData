
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entites;

namespace Infra.EF.Persistence.Configurations;

public class SymbolConfiguration : IEntityTypeConfiguration<Symbol>
{
    public void Configure(EntityTypeBuilder<Symbol> builder)
    {
        builder.ToTable("Symbols");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
      
    }
}

