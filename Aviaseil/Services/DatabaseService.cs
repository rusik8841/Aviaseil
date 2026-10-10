using SQLite;
using AirlinesApp.Models;

namespace AirlinesApp.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _db;

    public DatabaseService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "airlines.db3");
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<Route>().Wait();
        _db.CreateTableAsync<Aircraft>().Wait();
        _db.CreateTableAsync<Commander>().Wait();
        _db.CreateTableAsync<Passenger>().Wait();
        _db.CreateTableAsync<Flight>().Wait();
    }

    public Task<List<Route>> GetRoutesAsync() => _db.Table<Route>().ToListAsync();
    public Task<int> SaveRouteAsync(Route r) => r.id == 0 ? _db.InsertAsync(r) : _db.UpdateAsync(r);
    public Task<int> DeleteRouteAsync(Route r) => _db.DeleteAsync(r);

    public Task<List<Flight>> GetFlightsAsync() => _db.Table<Flight>().ToListAsync();
    public Task<int> SaveFlightAsync(Flight f) => f.id == 0 ? _db.InsertAsync(f) : _db.UpdateAsync(f);
    public Task<int> DeleteFlightAsync(Flight f) => _db.DeleteAsync(f);

    public Task<List<Passenger>> GetPassengersAsync() => _db.Table<Passenger>().ToListAsync();
    public Task<int> SavePassengerAsync(Passenger p) => p.id == 0 ? _db.InsertAsync(p) : _db.UpdateAsync(p);
    public Task<int> DeletePassengerAsync(Passenger p) => _db.DeleteAsync(p);

    public Task<List<Aircraft>> GetAircraftsAsync() => _db.Table<Aircraft>().ToListAsync();
    public Task<int> SaveAircraftAsync(Aircraft a) => a.id == 0 ? _db.InsertAsync(a) : _db.UpdateAsync(a);
    public Task<int> DeleteAircraftAsync(Aircraft a) => _db.DeleteAsync(a);

    public Task<List<Commander>> GetCommandersAsync() => _db.Table<Commander>().ToListAsync();
    public Task<int> SaveCommanderAsync(Commander c) => c.id == 0 ? _db.InsertAsync(c) : _db.UpdateAsync(c);
    public Task<int> DeleteCommanderAsync(Commander c) => _db.DeleteAsync(c);
}