using DynamicData;
using MarkAndFill.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MarkAndFill.Base.Services
{
    public class FileGroupService
    {
        private static readonly string _pathToDirectory = Path.Combine(AppContext.BaseDirectory, "Temp");
        private static readonly string _pathToFile = Path.Combine(_pathToDirectory, "FileGroups.json");

        private static bool _isInitialized = false;

        private static List<FileTempGroup> _groups = new List<FileTempGroup>();

        public static async Task InitAsync()
        {
            if (_isInitialized)
                return;
            if (!Directory.Exists(_pathToDirectory))
                Directory.CreateDirectory(_pathToDirectory);
            if (File.Exists(_pathToFile))
            {
                await using (var file = File.OpenRead(_pathToFile))
                {
                    var fileText = await JsonSerializer.DeserializeAsync<List<FileTempGroup>>(file);
                    if (fileText != null && fileText.Any())
                        _groups.AddRange(fileText);
                }
            }
            _isInitialized = true;
        }

        public static ReadOnlyCollection<FileTempGroup> GetAllGroups() => _groups.AsReadOnly();

        public static void NewGroup(FileTempGroup group)
        {
            EnsureInitialized();

            if (!string.IsNullOrWhiteSpace(group.GroupName) && !_groups.Contains(group))
                _groups.Add(group);
        }

        public static void RemoveGroup(FileTempGroup group)
        {
            EnsureInitialized();

            if (!string.IsNullOrWhiteSpace(group.GroupName) && _groups.Contains(group))
                _groups.Remove(group);
        }

        public static async Task SaveAsync()
        {
            EnsureInitialized();

            await using (var file = File.Create(_pathToFile))
            {
                await JsonSerializer.SerializeAsync(file, _groups);
            }
        }

        private static void EnsureInitialized()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Service is not initialized");
        }
    }
}
