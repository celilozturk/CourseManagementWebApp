using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Services
{
    public class BlobStorage : IBlobStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobStorage()
        {
            _blobServiceClient = new BlobServiceClient(ConnectionStrings.AzureStorageConnectionString);
        }
        public string BlobUrl => "http://127.0.0.1:10000/devstoreaccount1";

        public async Task DeleteAsync(string fileName, EContainerName eContainerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(eContainerName.ToString());
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public async Task<Stream> DownloadAsync(string fileName, EContainerName eContainerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(eContainerName.ToString());
            var blobClient=containerClient.GetBlobClient(fileName);
            var ms=new MemoryStream();
            var info = await blobClient.DownloadStreamingAsync();
            return info.Value.Content;
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

        public async Task UploadAsync(Stream fileStream, string fileName, EContainerName eContainerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(eContainerName.ToString());
            await containerClient.CreateIfNotExistsAsync();
            containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            var blobClient=containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, true);
        }
    }
}
