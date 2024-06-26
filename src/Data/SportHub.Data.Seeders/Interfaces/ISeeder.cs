namespace SportHub.Data.Seeders.Interfaces;

public interface ISeeder
{
    Task SeedAsync(IServiceProvider services);
}