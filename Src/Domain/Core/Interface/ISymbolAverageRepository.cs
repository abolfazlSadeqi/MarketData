using Core.Entites;

namespace Core.Interface;

public interface ISymbolAverageRepository
{
    Task BulkAsync(List<SymbolAverage> SymbolAverages);

    Task<List<SymbolAverage>> GetAll();
}

