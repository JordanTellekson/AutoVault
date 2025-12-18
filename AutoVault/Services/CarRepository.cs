using System.Text.Json;
using AutoVault.Models;

namespace AutoVault.Services;

/// <summary>
/// Repository responsible for persisting and retrieving Car data
/// using a local JSON file stored in AppDataDirectory.
/// </summary>
public sealed class CarRepository : ICarRepository
{
    // Name of the JSON data file
    private const string FileName = "cars.json";

    // In-memory cache to avoid unnecessary file reads
    private IReadOnlyList<Car>? _cache;

    // Writable path where the app stores the JSON file at runtime
    private string WritableFilePath =>
        Path.Combine(FileSystem.AppDataDirectory, FileName);

    /// <summary>
    /// Ensures a writable cars.json exists.
    /// Copies the seeded file from the app package on first run.
    /// </summary>
    private async Task EnsureFileExistsAsync()
    {
        if (!File.Exists(WritableFilePath))
        {
            try
            {
                // Copy seeded JSON from Resources/Raw into AppDataDirectory
                using var packageStream =
                    await FileSystem.OpenAppPackageFileAsync(FileName);

                using var fileStream = File.Create(WritableFilePath);
                await packageStream.CopyToAsync(fileStream);

                Console.WriteLine($"Copied {FileName} to AppDataDirectory.");
            }
            catch
            {
                // Fallback: create an empty JSON file if seed is missing
                await File.WriteAllTextAsync(WritableFilePath, "[]");
                Console.WriteLine($"Created empty {FileName} in AppDataDirectory.");
            }
        }
    }

    /// <summary>
    /// Loads cars from disk and updates the in-memory cache.
    /// </summary>
    private async Task<List<Car>> LoadCarsAsync()
    {
        await EnsureFileExistsAsync();

        var json = await File.ReadAllTextAsync(WritableFilePath);
        var cars = JsonSerializer.Deserialize<List<Car>>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Car>();

        _cache = cars;
        return cars;
    }

    /// <summary>
    /// Persists the car list to disk and refreshes the cache.
    /// </summary>
    private async Task SaveToFileAsync(List<Car> cars)
    {
        var json = JsonSerializer.Serialize(
            cars,
            new JsonSerializerOptions { WriteIndented = true });

        await File.WriteAllTextAsync(WritableFilePath, json);
        _cache = cars;
    }

    /// <summary>
    /// Returns all cars, using cache when available.
    /// </summary>
    public async Task<IReadOnlyList<Car>> GetCarsAsync()
    {
        if (_cache != null)
            return _cache;

        return await LoadCarsAsync();
    }

    /// <summary>
    /// Retrieves a single car by its ID.
    /// </summary>
    public async Task<Car?> GetCarByIdAsync(Guid id)
    {
        var cars = await GetCarsAsync();
        return cars.FirstOrDefault(c => c.Id == id);
    }

    /// <summary>
    /// Updates an existing car or adds it if missing.
    /// </summary>
    public async Task UpdateCarAsync(Car car)
    {
        var carsList = (_cache ?? await LoadCarsAsync()).ToList();

        // Sanitize ImagePath to avoid invisible characters breaking image loading
        if (!string.IsNullOrWhiteSpace(car.ImagePath))
        {
            car.ImagePath = car.ImagePath
                .Trim()
                .Trim('"')
                .Replace("\u00A0", "")
                .Replace("\u200B", "");
        }

        var index = carsList.FindIndex(c => c.Id == car.Id);

        if (index >= 0)
            carsList[index] = car;
        else
            carsList.Add(car);

        await SaveToFileAsync(carsList);
    }

    /// <summary>
    /// Adds a new car to the collection.
    /// </summary>
    public async Task AddCarAsync(Car car)
    {
        var carsList = (_cache ?? await LoadCarsAsync()).ToList();

        // Sanitize ImagePath before saving
        if (!string.IsNullOrWhiteSpace(car.ImagePath))
        {
            car.ImagePath = car.ImagePath
                .Trim()
                .Trim('"')
                .Replace("\u00A0", "")
                .Replace("\u200B", "");
        }

        carsList.Add(car);
        await SaveToFileAsync(carsList);
    }

    /// <summary>
    /// Removes a car by ID.
    /// </summary>
    public async Task DeleteCarAsync(Guid id)
    {
        var carsList = (_cache ?? await LoadCarsAsync()).ToList();
        carsList.RemoveAll(c => c.Id == id);

        await SaveToFileAsync(carsList);
    }
}