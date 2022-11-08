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
                ArrayList shapeData = new ArrayList();
                ArrayList colorData = new ArrayList();
                int numberOfLines = 0;

                //Below 2 objects are used to maintain the count of the colours
                ArrayList colorNames = new ArrayList();
                List<int> colorsCounter = new List<int>();//Preventing Arraylist for Int due to unwanted boxing and unboxing 

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

                                if (numberOfLines == 0)
                                {//First Line - read headers
                                    headers = line.Split(_algoCSVFileSplitChar);
                                    numberOfLines++;
                                    continue; //Move to next line without increamenting number of records
                                }

                                //Store the Coloumns
                                string[] rows = line.Split(_algoCSVFileSplitChar);
                                shapeData.Add(rows[0]);
                                colorData.Add(rows[1]);

                                //End of storing the data from CSV to an object


                                //Now Populate the data to maintain Occurance count of each colors
                                if (colorNames.IndexOf(rows[1]) < 0)//First Time
                                {
                                    colorNames.Add(rows[1]);//Add entry to Arraylist for new colour
                                    colorsCounter.Add(1); // Default 1
                                }
                                else
                                {//Already present
                                    var idx = colorNames.IndexOf(rows[1]); //Find index position of the color
                                    colorsCounter[idx] = colorsCounter[idx] + 1;//increment the value by 1
                                }

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
                //Build Heap
                var halfSize = (int)Math.Floor((float)(totalNumOfRecords) / 2);
                while (halfSize >= 0)
                {
                    MaxHeapify(colorNames, colorsCounter, halfSize, totalNumOfRecords - 1, shapeData, colorData);
                    halfSize--;
                }

                for (int i = 0; i < numberOfLines; i++)
                {
                    ExtractMax(colorNames, colorsCounter, halfSize, totalNumOfRecords - 1, shapeData, colorData);
                }

                await _algoChallengeBusinessProvider.SolveChallenge(ref shapeData, ref colorData);
                _logger.LogInfo(_logTitle + " End of ComputAlgorithm", null);


                //Manually Flush -using statement Disposes the object prematurely
                MemoryStream streamWr = new MemoryStream();
                StreamWriter writer = new StreamWriter(streamWr);
                {
                    writer.WriteLine(string.Join(_algoCSVFileSplitChar, headers)); //Write header
                    for (int i = 0; i < numberOfLines - 1; i++)
                    {
                        writer.WriteLine(string.Join(_algoCSVFileSplitChar, shapeData[i], colorData[i]));
                    }
                }
                writer.Flush();
                streamWr.Position = 0;


                _logger.LogTrace(_logTitle + " - ComputAlgorithm()- API Response", new[] { shapeData, colorData });
                return File(streamWr, "text/csv", "SortedList.csv");

            }
            catch (Exception ex)
            {
                _logger.LogError(_logTitle + " Error in ComputAlgorithm", new[] { ex as object });
                return HandleException(_logTitle, null, ex);
            }
        }

        private void ExtractMax(ArrayList colorNames, List<int> colorsCounter, int position, int recordsCount, ArrayList shapeData, ArrayList colorData)
        {
        }
        private void MaxHeapify(ArrayList colorNames, List<int> colorsCounter, int position, int recordsCount, ArrayList shapeData, ArrayList colorData)
        {
            var leftPos = position * 2 + 1;
            var rightPos = position * 2 + 2;
            var largest = position; //Largest at the center

            if (leftPos < recordsCount && colorsCounter[leftPos] > colorsCounter[position])
                largest = leftPos;
            if (rightPos < recordsCount && colorsCounter[rightPos] > colorsCounter[largest])
                largest = rightPos;
            if (largest != position)
            {
                //Get values to temp variable
                var tempPos = colorNames[position];
                var tempCtr = colorsCounter[position];

                //Now swap data
                colorNames[position] = colorNames[largest];
                colorsCounter[position] = colorsCounter[largest];

                colorNames[largest] = tempPos;
                colorsCounter[largest] = tempCtr;

                MaxHeapify(colorNames, colorsCounter, largest, recordsCount, shapeData, colorData);
            }

        }
    }
}
