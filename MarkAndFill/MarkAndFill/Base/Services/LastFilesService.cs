using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MarkAndFill.Model;

namespace MarkAndFill.Base.Services;

public static class LastFilesService
{
    private static readonly string _pathToDirectory = Path.Combine(AppContext.BaseDirectory, "Temp");
    private static readonly string _pathToFile = Path.Combine(_pathToDirectory, "lastFiles.json");

    private static bool _isInitialized;

    private static readonly List<FileTemplate> _files = new();

    public static async Task InitAsync()
    {
        if (_isInitialized)
            return;
        if (!Directory.Exists(_pathToDirectory))
            Directory.CreateDirectory(_pathToDirectory);
        if (File.Exists(_pathToFile))
            await using (var file = File.OpenRead(_pathToFile))
            {
                var fileText = await JsonSerializer.DeserializeAsync<List<FileTemplate>>(file);
                if (fileText != null && fileText.Any())
                    _files.AddRange(fileText);
            }

        _isInitialized = true;
    }

    public static ReadOnlyCollection<FileTemplate> GetAllPaths()
    {
        return _files.AsReadOnly();
    }

    public static void SetFile(FileTemplate file)
    {
        EnsureInitialized();

        if (!string.IsNullOrWhiteSpace(file.FilePath) && !_files.Contains(file))
            _files.Add(file);
    }

    public static void RemoveFile(FileTemplate file)
    {
        EnsureInitialized();

        if (!string.IsNullOrWhiteSpace(file.FilePath) && _files.Contains(file))
            _files.Remove(file);
    }

    private static void EnsureInitialized()
    {
        if (!_isInitialized)
            throw new InvalidOperationException("Service is not initialized");
    }

    public static async Task SaveAsync()
    {
        EnsureInitialized();

        await using (var file = File.Create(_pathToFile))
        {
            await JsonSerializer.SerializeAsync(file, _files);
        }
    }
}