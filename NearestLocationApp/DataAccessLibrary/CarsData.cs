
namespace DataAccessLibrary
{
    public class CarsData : ICarsData
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public CarsData(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<Car>> GetCars()
        {
            string sql = "SELECT * FROM dbo.Cars";

            return await _sqlDataAccess.GetData<Car, dynamic>(sql, new { });
        }

        public async Task SetCars(List<Car> cars)
        {
            await _sqlDataAccess.SetData("DELETE FROM dbo.Cars", new { });

            string sql = "INSERT INTO dbo.Cars (Name, Location) VALUES (@Name, @Location)";

            foreach (var car in cars)
            {
                await _sqlDataAccess.SetData(sql, car);
            }
        }
    }
}
