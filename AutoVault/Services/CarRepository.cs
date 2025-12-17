using System.Text.Json;
using AutoVault.Models;

namespace AutoVault.Services;

public sealed class CarRepository : ICarRepository
{
    private const string FileName = "cars.json";
    private IReadOnlyList<Car>? _cache;

    private string WritableFilePath => Path.Combine(FileSystem.AppDataDirectory, FileName);

    private async Task EnsureFileExistsAsync()
    {
        if (!File.Exists(WritableFilePath))
        {
            try
            {
                using var packageStream = await FileSystem.OpenAppPackageFileAsync(FileName);
                using var fileStream = File.Create(WritableFilePath);
                await packageStream.CopyToAsync(fileStream);
            }
            catch
            {
                await File.WriteAllTextAsync(WritableFilePath, "[]");
            }
        }
    }

    private async Task<List<Car>> LoadCarsAsync()
    {
        await EnsureFileExistsAsync();
        var json = await File.ReadAllTextAsync(WritableFilePath);
        var cars = JsonSerializer.Deserialize<List<Car>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Car>();

        _cache = cars;
        return cars;
    }

    private async Task SaveToFileAsync(List<Car> cars)
    {
        var json = JsonSerializer.Serialize(cars, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(WritableFilePath, json);
        _cache = cars;
    }

    public async Task<IReadOnlyList<Car>> GetCarsAsync()
    {
        if (_cache != null) return _cache;
        return await LoadCarsAsync();
    }

    public async Task<Car?> GetCarByIdAsync(Guid id)
    {
        var cars = await GetCarsAsync();
        return cars.FirstOrDefault(c => c.Id == id);
    }

    public async Task UpdateCarAsync(Car car)
    {
        var carsList = (_cache ?? await LoadCarsAsync()).ToList();
        var index = carsList.FindIndex(c => c.Id == car.Id);
        if (index >= 0)
            carsList[index] = car;
        else
            carsList.Add(car);

        await SaveToFileAsync(carsList);
    }

    public async Task AddCarAsync(Car car)
    {
        var carsList = (_cache ?? await LoadCarsAsync()).ToList();
        carsList.Add(car);
        await SaveToFileAsync(carsList);
    }

    public async Task DeleteCarAsync(Guid id)
    {
        var carsList = (_cache ?? await LoadCarsAsync()).ToList();
        carsList.RemoveAll(c => c.Id == id);
        await SaveToFileAsync(carsList);
    }
}