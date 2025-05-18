using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MizeProject.Utils;

namespace MizeProject.Storages
{
    public class MemoryStorage<T> : IStorage<T>
    {
        private T _value;
        private DateTime _fetchedAt;
        private bool _holdsValue = false;
        private static readonly ILogger _logger = LoggerManager.CreateLogger<MemoryStorage<T>>();


        public bool CanWrite => true;
        public TimeSpan ExpirationTime { get; }

        public MemoryStorage(TimeSpan expirationTime)
        {
            ExpirationTime = expirationTime;
        }

        public Task<(T value, DateTime FetchTime)> ReadAsync()
        {
            //Reading the held value and return it
            if (_holdsValue && DateTime.UtcNow - _fetchedAt < ExpirationTime) 
            {
                _logger.LogInformation("Reading from memory storage");
                return Task.FromResult((_value, _fetchedAt));
            }

            //in case the fetched time expired we return null
            return Task.FromResult((default(T), DateTime.MinValue));
        }

        public Task WriteAsync(T value)
        {
            _logger.LogInformation("Writing to memory storage");
            _value = value;
            _fetchedAt = DateTime.UtcNow;
            _holdsValue = true;
            return Task.CompletedTask;
        }
    }
}
