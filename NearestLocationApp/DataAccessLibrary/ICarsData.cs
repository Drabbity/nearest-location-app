namespace DataAccessLibrary
{
    public interface ICarsData
    {
        Task<List<Car>> GetCars();
        Task AddCar(Car car);
    }
}