using Core.Entites;
using Core.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infra.EF;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Symbol> Symbols => Set<Symbol>();
    public DbSet<SymbolPrice> SymbolPrice => Set<SymbolPrice>();
    public DbSet<SymbolPriceHistory> SymbolPriceHistory => Set<SymbolPriceHistory>();

    public DbSet<SymbolAverage> SymbolAverage => Set<SymbolAverage>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.Entity<Symbol>().HasIndex(x => x.Name).IsUnique();
    }
}

