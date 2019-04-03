using System.Threading.Tasks;
using LogicLayer.Model;
using Newtonsoft.Json;

namespace LogicLayer
{
    /// <summary>
    ///     Interface to write data after processing to json
    /// </summary>
    public interface IJsonFileWriter
    {
        Task Write(string filepath, ProcessedDataToWrite toWrite);
    }

    internal class JsonFileWriter : IJsonFileWriter
    {
        private readonly IFileHelper fileHelper;

        public JsonFileWriter(IFileHelper fileHelper)
        {
            this.fileHelper = fileHelper;
        }

        public async Task Write(string filepath, ProcessedDataToWrite toWrite)
        {
            Require.NotEmpty(filepath, nameof(filepath));
            Require.NotNull(toWrite, nameof(filepath));

            var writer = new JsonTextWriter(await fileHelper.BuildStreamWriter(filepath));
            var serializer = new JsonSerializer {Formatting = Formatting.Indented};
            serializer.Serialize(writer, toWrite);
        }
    }
}