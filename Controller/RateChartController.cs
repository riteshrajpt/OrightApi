using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using RateCharts.BO;
using RateCharts.Repository;
using RateCharts.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using HttpMultipartParser;

namespace RateCharts.Controller
{
    public class RateChartController : WebApiController
    {
        private readonly RateChartRepository _rateChartRepository;
        private readonly ExcelService _excelService;

        public RateChartController(RateChartRepository rateChartRepository, ExcelService excelService)
        {
            _rateChartRepository = rateChartRepository ?? throw new ArgumentNullException(nameof(rateChartRepository));
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
        }

        [Route(HttpVerbs.Get, "/getPrice")]
        public async Task<decimal?> GetPriceForClient([QueryField] int clientId, [QueryField] decimal snf, [QueryField] decimal fat)
        {
            try
            {
                return await _rateChartRepository.GetPriceForClientAsync(clientId, snf, fat);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPriceForClient: {ex.Message}");
                throw HttpException.InternalServerError("Error retrieving price.");
            }
        }

        [Route(HttpVerbs.Post, "/upload")]
        public async Task InsertRateCharts([QueryField] int clientId)
        {
            var request = HttpContext.Request;

            // Check if the request has the correct content type
            if (!request.ContentType.Contains("multipart/form-data"))
            {
                throw HttpException.BadRequest("Expected form data.");
            }

            // Parse the multipart form data
            MultipartFormDataParser parser = null;
            try
            {
                parser = await MultipartFormDataParser.ParseAsync(request.InputStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing multipart form data: {ex.Message}");
                throw HttpException.BadRequest("Error parsing form data.");
            }

            // Check if any files were uploaded
            if (parser.Files.Count == 0)
            {
                throw HttpException.BadRequest("No file uploaded.");
            }

            // Retrieve the first file from the form data
            var file = parser.Files[0];
            if (file == null || file.Data == null || file.Data.Length == 0)
            {
                throw HttpException.BadRequest("No file uploaded or file is empty.");
            }

            // Process the uploaded file
            try
            {
                using (var stream = file.Data)
                {
                    var rateCharts = _excelService.ReadExcelData(stream, clientId);
                    await _rateChartRepository.InsertRateChartsAsync(rateCharts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing uploaded file: {ex.Message}");
                throw HttpException.InternalServerError("Error processing uploaded file.");
            }
        }
    }
}
