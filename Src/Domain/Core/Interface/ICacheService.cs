using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface;

public interface ICacheService
{
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task<T?> GetAsync<T>(string key);
    Task RemoveAsync(string key);

    Task AppendToListAsync<T>(string key, T value, int? maxLength = null);
    Task<List<T>> GetListAsync<T>(string key, int? count = null);

    List<string> GetAllSymbolNamesFromHistoryAsync();


}
