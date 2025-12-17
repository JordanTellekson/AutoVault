using AutoVault.Models;

namespace AutoVault.Services;

public interface ICarRepository
{
    Task<IReadOnlyList<Car>> GetCarsAsync();
    Task<Car?> GetCarByIdAsync(Guid id);
    Task UpdateCarAsync(Car car);
    Task AddCarAsync(Car car);
    Task DeleteCarAsync(Guid id);
}