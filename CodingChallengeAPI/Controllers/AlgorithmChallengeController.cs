using CodingChallenge.Business;
using CodingChallenge.Business.Interfaces;
using CodingChallenge.Models;
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

                if (csvFile == null || !csvFile.FileName.EndsWith(".csv"))
                {
                    _logger.LogWarning(_logTitle + " Invalid file format provided, please upload a csv file with shape and color columns", null);
                    hasValidationEror = true;
                    sbValidation.AppendLine("Invalid file format provided, please upload a csv file with shape and color columns");

                }

                if (hasValidationEror)
                    return BadRequest(sbValidation.ToString());
                #endregion Validation

                string[] headers = new string[1];
                ArrayList shapeObj = new ArrayList();
                int numberOfLines = 0;

                //maintain the count of the colours
                Hashtable colorsWithCount = new Hashtable();

                using (var stream = csvFile.OpenReadStream())
                {
                    using (var reader = new StreamReader(stream, true))
                    {
                        bool isEndOfData = false;//Read till last row
                        while (!reader.EndOfStream && !isEndOfData)
                        {
                            var line = reader.ReadLine();
                            if (!string.IsNullOrWhiteSpace(line))
                            {

                                #region PopulateShapeObj
                                if (numberOfLines == 0)
                                {//First Line - read headers
                                    headers = line.Split(_algoCSVFileSplitChar);
                                    numberOfLines++;
                                    continue; //Move to next line without increamenting number of records
                                }

                                //Store the Coloumns
                                string[] rows = line.Split(_algoCSVFileSplitChar);
                                //shapeData.Add(rows[0]);
                                //colorData.Add(rows[1]);
                                shapeObj.Add(new ShapeObjects()
                                {
                                    Shape= rows[0],
                                    Color= rows[1]
                                });

                                //End of storing the data from CSV to an object
                                #endregion PopulateShapeObj

                                #region maintianCounter
                                //Now Populate the data to maintain Occurance count of each colors
                                if (!colorsWithCount.ContainsKey(rows[1]))//First Time
                                {
                                    colorsWithCount.Add(rows[1],1);//Add entryfor new colour
                                    
                                }
                                else
                                {//Already present
                                    var color = rows[1];
                                    colorsWithCount[color] = Convert.ToInt32(colorsWithCount[color]) + 1;//increment the value by 1
                                }
                                #endregion maintianCounter

                                numberOfLines++;
                                
                            }
                            else
                            {
                                isEndOfData = true;
                            }

                        }
                    }
                }
                var totalNumOfRecords = numberOfLines - 1;//Number of lines -1 to exclue header


                //await _algoChallengeBusinessProvider.SolveChallenge(totalNumOfRecords, ref shapeObj);
               var result= await new AlgoChallengeBusinessProvider().SolveChallenge(totalNumOfRecords, shapeObj, colorsWithCount);

                _logger.LogInfo(_logTitle + " End of ComputAlgorithm", null);


                //Manually Flush -using statement Disposes the object prematurely
                MemoryStream streamWr = new MemoryStream();
                StreamWriter writer = new StreamWriter(streamWr);
                {
                    writer.WriteLine(string.Join(_algoCSVFileSplitChar, headers)); //Write header
                    for (int i = 0; i < numberOfLines - 1; i++)
                    {
                        var item = result[i] as ShapeObjects;
                        writer.WriteLine(string.Join(_algoCSVFileSplitChar, item.Shape, item.Color));
                    }
                }
                writer.Flush();
                streamWr.Position = 0;


                //_logger.LogTrace(_logTitle + " - ComputAlgorithm()- API Response", new[] { shapeData, colorData });
                return File(streamWr, "text/csv", "SortedList.csv");

            }
            catch (Exception ex)
            {
                _logger.LogError(_logTitle + " Error in ComputAlgorithm", new[] { ex as object });
                return HandleException(_logTitle, null, ex);
            }
        }

   
    }
}
