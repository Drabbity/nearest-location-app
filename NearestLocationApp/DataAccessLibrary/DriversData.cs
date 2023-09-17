
namespace DataAccessLibrary
{
    public class DriversData : IDriversData
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        private const string _TABLE_NAME = "Drivers";

        public DriversData(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<Driver>> GetDrivers()
        {
            string sql = $"SELECT * FROM dbo.{_TABLE_NAME}";

            return await _sqlDataAccess.GetData<Driver, dynamic>(sql, new { });
        }

        public async Task AddDriver(Driver driver)
        {
            string sql = $"INSERT INTO dbo.{_TABLE_NAME} "
                + "(Name, PlateNumber, CompanyName, TruckType, TruckLength, TruckWidth, TruckHeight, Payload) "
                + "VALUES (@Name, @PlateNumber, @CompanyName, @TruckType, @TruckLength, @TruckWidth, @TruckHeight, @Payload)";

            await _sqlDataAccess.SetData(sql, driver);
        }
    }
}
