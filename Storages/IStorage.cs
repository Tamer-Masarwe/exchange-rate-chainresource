using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MizeProject.Storages
{

    /*
     * Why Use async in IStorage<T>?
        Because some storages — especially the file system and the web API — can take time to respond, 
        and we don’t want to block the program while waiting.
     */
    public interface IStorage<T>
    {
        Task<(T  value, DateTime FetchTime)> ReadAsync();

        Task WriteAsync(T value);
        bool CanWrite { get; }
        TimeSpan ExpirationTime { get; }
    }
}
