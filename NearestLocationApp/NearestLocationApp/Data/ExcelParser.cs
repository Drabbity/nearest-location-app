﻿using Microsoft.AspNetCore.Components.Forms;
using OfficeOpenXml;
using DataAccessLibrary;

namespace NearestLocationApp.Data
{
    public class ExcelParser
    {
        public async Task<List<Car>> ParseCars(InputFileChangeEventArgs e)
        {
            List <Car> Cars = new List <Car>();

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
                                var carName = workSheet.GetValue(row, startColumn);
                                var zipCode = workSheet.GetValue(row, startColumn + 1);

                                if(zipCode != null && carName != null)
                                {
                                    Car car = new Car();
                                    car.CarName = carName.ToString(); ;
                                    car.ZipCode = zipCode.ToString();

                                    Cars.Add(car);
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

            return Cars;
        }
    }
}
