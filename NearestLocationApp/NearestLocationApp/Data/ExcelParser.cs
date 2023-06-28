using Microsoft.AspNetCore.Components.Forms;
using OfficeOpenXml;
using DataAccessLibrary;

namespace NearestLocationApp.Data
{
    public class ExcelParser
    {
        public async Task<List<Cordinate>> ParseCordinates(InputFileChangeEventArgs e)
        {
            List <Cordinate> Cordinates = new List <Cordinate>();

            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using (MemoryStream memoryString = new MemoryStream())
                    {
                        await file.OpenReadStream().CopyToAsync(memoryString);
                        memoryString.Position = 0;

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        using (ExcelPackage package = new ExcelPackage(memoryString))
                        {
                            ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault();
                            
                            var startRow = workSheet.Dimension.Start.Row;
                            var startColumn = workSheet.Dimension.Start.Column;
                            var endRow = workSheet.Dimension.End.Row;

                            for (int row = startRow; row <= endRow; row++)
                            {
                                var latitude = workSheet.GetValue(row, startColumn);
                                var longitude = workSheet.GetValue(row, startColumn + 1);

                                if(latitude != null && longitude != null)
                                {
                                    Cordinate cordinate = new Cordinate();
                                    cordinate.Latitude = latitude.ToString();
                                    cordinate.Longitude = longitude.ToString();

                                    Cordinates.Add(cordinate);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return Cordinates;
        }
    }
}
