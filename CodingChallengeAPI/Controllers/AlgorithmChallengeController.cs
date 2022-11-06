using CodingChallenge.Business;
using CodingChallenge.Business.Interfaces;
using CodingChallenge.Models.Response;
using CodingChallengeAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Text;

namespace CodingChallengeAPI
{
    [Route("[controller]")]
    [ApiController]
    [ResponseCache(NoStore = true)]
    public class AlgorithmChallengeController : BaseController
    {

        private readonly CodingChallenge.Logging.Interface.ILogging<AlgorithmChallengeController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly string _logTitle = "CodingChallengeAPI.Controllers.ZipLookupController";
        private readonly IAlgoChallengeBusinessProvider _algoChallengeBusinessProvider = null;
        private char _algoCSVFileSplitChar;
        public AlgorithmChallengeController(CodingChallenge.Logging.Interface.ILogging<AlgorithmChallengeController> logger, IConfiguration config, IAlgoChallengeBusinessProvider algoChallengeBusinessProvider) : base(config)
        {
            if (logger == null)
                throw new Exception("Logger object is null");

            if (algoChallengeBusinessProvider == null)
                throw new Exception("City Business Provider object is null");

            _logger = logger;
            _algoChallengeBusinessProvider = algoChallengeBusinessProvider;
            _algoCSVFileSplitChar = Convert.ToChar(config["Values:AlgoCSVFileSplitChar"]);

        }



        [HttpPost(Name = "ComputAlgorithm")]
        public async Task<IActionResult> ComputAlgorithm(IFormFile csvFile)
        {
            _logger.LogInfo(_logTitle + " Begin of ComputAlgorithm", null);
            StringBuilder sbValidation = new StringBuilder();//Using string builder incase of multiple errors
            try
            {
                #region Validation
                bool hasValidationEror = false;

                if (csvFile == null || !csvFile.FileName.EndsWith(".csv") || csvFile.Length < 0)
                {
                    _logger.LogWarning(_logTitle + " Invalid file format provided, please upload a csv file with shape and color columns", null);
                    hasValidationEror = true;
                    sbValidation.AppendLine("Invalid file format provided, please upload a csv file with shape and color columns");

                }

                if (hasValidationEror)
                    return BadRequest(sbValidation.ToString());
                #endregion Validation

                string[] headers;
                //string[] shape;
                //string[] color;
                ArrayList shapeData = new ArrayList();
                ArrayList colorData = new ArrayList();
                int numberOfRecords=0;
                using (var stream = csvFile.OpenReadStream())
                {
                    using (var reader = new StreamReader(csvFile.OpenReadStream()))
                    {
                        headers = reader.ReadLine().Split(_algoCSVFileSplitChar);
                        //shape = new string[length];
                        //color = new string[length];
                        
                        bool isEndOfData = false;//Read till last row
                        while (!reader.EndOfStream && !isEndOfData)                        
                        {
                            var line = reader.ReadLine();
                            if(!string.IsNullOrWhiteSpace(line))
                            {
                                string[] rows = line.Split(_algoCSVFileSplitChar);
                                shapeData.Add(rows[0].ToString());
                                colorData.Add(rows[1].ToString());
                                //shape[i]=(rows[0].ToString());
                                //color[i]=rows[1].ToString();
                                numberOfRecords++;

                            }
                            else
                            {
                                isEndOfData = true;
                            }

                        }
                    }


                }
                string[] shape = (string[])shapeData.ToArray(typeof(string));
                string[] color = (string[])colorData.ToArray(typeof(string));

                //await _algoChallengeBusinessProvider.SolveChallenge(ref shape, ref color);


                _logger.LogInfo(_logTitle + " End of ComputAlgorithm", null);
                



                MemoryStream streamWr = new MemoryStream();
                using (StreamWriter writer = new StreamWriter(streamWr))
                {
                    writer.WriteLine(string.Join(_algoCSVFileSplitChar, headers)); //Write header
                    for (int i = 0; i < numberOfRecords; i++)
                    {
                        writer.WriteLine(string.Join(_algoCSVFileSplitChar, shape[i], color[i]));
                    }
                }
                //streamWr.Position = 0;


                //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                //result.Content = new StreamContent(streamWr);
                //result.Content.Headers.ContentType =
                //    new MediaTypeHeaderValue("text/csv");
                //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "SortedList.csv" };
                //return result;


                _logger.LogTrace(_logTitle + " - ComputAlgorithm()- API Response", new[] { shape, color });

                return File(streamWr, "text/csv", "SortedList.csv");
                //return BuildFileActionResult(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(_logTitle + " Error in ComputAlgorithm", new[] { ex as object });
                return HandleException(_logTitle, null, ex);
            }






        }

    }
}
