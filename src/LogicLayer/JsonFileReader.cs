using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogicLayer.Model;
using Newtonsoft.Json;

namespace LogicLayer
{
    /// <summary>
    ///     Json file reading logic
    /// </summary>
    public interface IJsonFileReader
    {
        Task<JsonData[]> ParseFileToJsonData(string filepath, int rowsToSkip);
    }

    internal class JsonFileReader : IJsonFileReader
    {
        private readonly IFileHelper fileHelper;

        public JsonFileReader(IFileHelper fileHelper)
        {
            this.fileHelper = fileHelper;
        }

        public async Task<JsonData[]> ParseFileToJsonData(string filepath, int rowsToSkip)
        {
            Require.NotEmpty(filepath, nameof(filepath));
            if (rowsToSkip < 0)
            {
                throw new ArgumentException("rowsToSkip have to be more than 0", nameof(rowsToSkip));
            }

            var data = new List<JsonData>();
            using (var jsonReader = new JsonTextReader(await fileHelper.BuildStreamReader(filepath)))
            {
                for (var i = 0; i < rowsToSkip; i++)
                {
                    jsonReader.Read();
                }

                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.EndObject)
                    {
                        continue;
                    }

                    var date = jsonReader.Value.ToString();
                    jsonReader.Read();
                    var value = jsonReader.Value.ToString();
                    data.Add(new JsonData
                    {
                        Date = DateTime.Parse(date).ToUniversalTime(),
                        Value = double.Parse(value)
                    });
                }
            }

            return await Task.FromResult(data.OrderBy(d => d.Date).ToArray());
        }
    }
}