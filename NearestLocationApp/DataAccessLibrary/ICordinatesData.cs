namespace DataAccessLibrary
{
    public interface ICordinatesData
    {
        Task<List<Cordinate>> GetCordinates();
        Task SetCordinates(List<Cordinate> cordinates);
    }
}