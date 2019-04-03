using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicLayer.Model;

namespace LogicLayer
{
    public interface IJsonParser
    {
        Task<IEnumerable<ProcessedData>> ParseFile(string filepath, int rowsToSkip);
    }

    internal class JsonParser : IJsonParser
    {
        private readonly IJsonDataProcessor dataProcessor;
        private readonly IJsonFileReader fileReader;

        public JsonParser(IJsonDataProcessor dataProcessor, IJsonFileReader fileReader)
        {
            this.dataProcessor = dataProcessor;
            this.fileReader = fileReader;
        }

        public async Task<IEnumerable<ProcessedData>> ParseFile(string filepath, int rowsToSkip)
        {
            Require.NotEmpty(filepath, nameof(filepath));
            if (rowsToSkip < 0)
            {
                throw new ArgumentException("rowsToSkip have to be more than 0", nameof(rowsToSkip));
            }

            var allData = await fileReader.ParseFileToJsonData(filepath, rowsToSkip).ConfigureAwait(false);
            var myData = allData[0];
            var parsedData = new List<ProcessedData>();
            var toProcess = new List<JsonData>();


            foreach (var data in allData)
            {
                if (Utilities.IsDateEqual(myData.Date, data.Date) && Utilities.IsHourEqual(myData.Date, data.Date))
                {
                    toProcess.Add(data);
                }
                else
                {
                    parsedData.Add(await dataProcessor.Process(toProcess.ToArray()).ConfigureAwait(false));
                    toProcess = new List<JsonData>();
                    myData = data;
                    toProcess.Add(data);
                }
            }

            parsedData.Add(await dataProcessor.Process(toProcess.ToArray()).ConfigureAwait(false));


            return parsedData;
        }
    }
}