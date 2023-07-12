namespace DataAccessLibrary
{
    public interface ICarsData
    {
        Task<List<Car>> GetCars();
        Task SetCars(List<Car> cars);
    }
}