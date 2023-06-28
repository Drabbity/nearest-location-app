using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class CordinatesData : ICordinatesData
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public CordinatesData(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<Cordinate>> GetCordinates()
        {
            string sql = "SELECT * FROM dbo.Cordinates";

            return await _sqlDataAccess.GetData<Cordinate, dynamic>(sql, new { });
        }

        public async Task SetCordinates(List<Cordinate> cordinates)
        {
            await _sqlDataAccess.SetData("DELETE FROM dbo.Cordinates", new { });

            string sql = "INSERT INTO dbo.Cordinates (Latitude, Longitude) VALUES (@Latitude, @Longitude)";

            foreach (var cordinate in cordinates)
            {
                await _sqlDataAccess.SetData(sql, cordinate);
            }
        }
    }
}
