using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MizeProject.Storages;

namespace MizeProject.Core
{
    public class ChainResource<T>
    {
        private readonly List<IStorage<T>> _availableStorages;

        public ChainResource(IEnumerable<IStorage<T>> storages)
        {
            _availableStorages = new List<IStorage<T>>(storages);
        }

        public async Task<T> GetValue()
        {
            for (int i = 0; i < _availableStorages.Count; i++)
            {
                var storage = _availableStorages[i];
                var (value, fetchedAt) = await storage.ReadAsync();
                bool isValueValid = storage.ExpirationTime == TimeSpan.Zero || DateTime.UtcNow - fetchedAt < storage.ExpirationTime;

                if (value != null && isValueValid)
                {
                    // copy value upwards to previous writeable storages
                    for (int j = 0; j < i; j++)
                    {
                        if (_availableStorages[j].CanWrite)
                        {
                            await _availableStorages[j].WriteAsync(value);
                        }
                    }

                    return value;
                }
            }

            // If nothing found in chain
            return default(T);
        }
    }
}
