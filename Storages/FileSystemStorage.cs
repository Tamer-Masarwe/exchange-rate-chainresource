using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MizeProject.Utils;

namespace MizeProject.Storages
{
    public class FileSystemStorage<T> : IStorage<T>
    {
        private readonly string _filePathOfValue;
        private readonly string _metaPathOfFetchTime;
        private static readonly ILogger _logger = LoggerManager.CreateLogger<FileSystemStorage<T>>();


        public TimeSpan ExpirationTime { get; }

        public bool CanWrite => true;

        public FileSystemStorage(string filePath, TimeSpan expirationTime) 
        {
            _filePathOfValue = filePath;
            _metaPathOfFetchTime = _filePathOfValue + ".meta";
            ExpirationTime = expirationTime;
        }

        public async Task<(T value, DateTime FetchTime)> ReadAsync()
        {
            //check if the json file exist first
            if(!File.Exists(_filePathOfValue) || !File.Exists(_metaPathOfFetchTime))
                return (default(T), DateTime.MinValue);
            
            try
            {
                _logger.LogInformation("Reading from file system");
                var json = await File.ReadAllTextAsync(_filePathOfValue);
                var fetchTimeStr = await File.ReadAllTextAsync(_metaPathOfFetchTime);

                var value = JsonSerializer.Deserialize<T>(json);
                var fetchedAt = DateTime.Parse(fetchTimeStr);

                if(DateTime.UtcNow - fetchedAt < ExpirationTime)
                    return (value, fetchedAt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Error reading value file or metadata files");
            }

            //in this case the value fetchedAt is expired
            return (default(T), DateTime.MinValue);
        }

        public async Task WriteAsync(T value)
        {
            var jsonStr = JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                WriteIndented = true,
            });

            try
            {
                _logger.LogInformation("Writing data to json and meta files");
                await File.WriteAllTextAsync(_filePathOfValue, jsonStr);
                await File.WriteAllTextAsync(_metaPathOfFetchTime, DateTime.UtcNow.ToString("O"));
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, $"Failed to write data or timestamp to file");
            }
        }
    }
}
