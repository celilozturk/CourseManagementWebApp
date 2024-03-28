using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Services
{
    public class BlobStorage : IBlobStorage
    {
        public string BlobUrl => throw new NotImplementedException();

        public Task DeleteAsync(string fileName, EContainerName eContainerName)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadAsync(string fileName, EContainerName eContainerName)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetLogAsync(string blobName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetNames(EContainerName eContainerName)
        {
            throw new NotImplementedException();
        }

        public Task SetLogAsync(string text, string blobName)
        {
            throw new NotImplementedException();
        }

        public Task UploadAsync(Stream fileStream, string fileName, EContainerName eContainerName)
        {
            throw new NotImplementedException();
        }
    }
}
