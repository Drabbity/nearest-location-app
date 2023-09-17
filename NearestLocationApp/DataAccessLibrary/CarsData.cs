
namespace DataAccessLibrary
{
    public class CarsData : ICarsData
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        private const string _TABLE_NAME = "Cars";

        public CarsData(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<Car>> GetCars()
        {
            string sql = $"SELECT * FROM dbo.{_TABLE_NAME}";

            return await _sqlDataAccess.GetData<Car, dynamic>(sql, new { });
        }

        public async Task SetCars(List<Car> cars)
        {
            await _sqlDataAccess.SetData($"DELETE FROM dbo.{_TABLE_NAME}", new { });

            string sql = $"INSERT INTO dbo.{_TABLE_NAME} (Name, Location) VALUES (@Name, @Location)";

            foreach (var car in cars)
            {
                await _sqlDataAccess.SetData(sql, car);
            }
        }
    }
}
