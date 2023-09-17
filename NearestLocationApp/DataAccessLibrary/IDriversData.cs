namespace DataAccessLibrary
{
    public interface IDriversData
    {
        Task AddDriver(Driver driver);
        Task<List<Driver>> GetDrivers();
    }
}