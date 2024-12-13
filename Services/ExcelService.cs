using OfficeOpenXml;
using RateCharts.BO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace RateCharts.Services
{
    public class ExcelService
    {
        public List<RateChart> ReadExcelData(Stream fileStream, int clientId)
        {
            List<RateChart> rateCharts = new List<RateChart>();

            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;
                int columnCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++)
                {
                    for (int col = 2; col <= columnCount; col++)
                    {
                        RateChart rateChart = new RateChart
                        {
                            ClientId = clientId,
                            SNF = Convert.ToDecimal(worksheet.Cells[row, 1].Value, CultureInfo.InvariantCulture),
                            FAT = Convert.ToDecimal(worksheet.Cells[1, col].Value, CultureInfo.InvariantCulture),
                            Price = Convert.ToDecimal(worksheet.Cells[row, col].Value, CultureInfo.InvariantCulture)
                        };

                        rateCharts.Add(rateChart);
                    }
                }
            }

            return rateCharts;
        }
    }
}
