using System.IO;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    ///     Interface for building StreamReader and StreamWriter
    /// </summary>
    public interface IFileHelper
    {
        Task<StreamReader> BuildStreamReader(string filepath);
        Task<StreamWriter> BuildStreamWriter(string filepath);
    }

    internal class FileHelper : IFileHelper
    {
        public async Task<StreamReader> BuildStreamReader(string filepath)
        {
            Require.NotEmpty(filepath, nameof(filepath));

            return await Task.FromResult(File.OpenText(filepath)).ConfigureAwait(false);
        }

        public async Task<StreamWriter> BuildStreamWriter(string filepath)
        {
            Require.NotEmpty(filepath, nameof(filepath));

            return await Task.FromResult(File.CreateText(filepath)).ConfigureAwait(false);
        }
    }
}