using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            string sql = "INSERT INTO dbo.Cars (CarName, ZipCode) VALUES (@CarName, @ZipCode)";

            foreach (var car in cars)
            {
                await _sqlDataAccess.SetData(sql, car);
            }
        }
    }
}
