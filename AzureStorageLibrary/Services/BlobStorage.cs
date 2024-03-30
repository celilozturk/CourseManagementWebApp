using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
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

        public async Task<List<string>> GetLogAsync(string blobName)
        {
            List<string> logs = new List<string>();
            var containerClient = _blobServiceClient.GetBlobContainerClient(EContainerName.logs.ToString());
            await containerClient.CreateIfNotExistsAsync();
            var appendBlobClient = containerClient.GetAppendBlobClient(blobName);
            await appendBlobClient.CreateIfNotExistsAsync();
            var info = await appendBlobClient.DownloadStreamingAsync();
            using (StreamReader sr = new StreamReader(info.Value.Content))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    logs.Add(line);
                }
            }
            return logs;
        }

        public List<string> GetNames(EContainerName eContainerName)
        {
           List<string> blobNames=new ();
            var containerClient = _blobServiceClient.GetBlobContainerClient(eContainerName.ToString());
            var blobs = containerClient.GetBlobs();
            blobs.ToList().ForEach(b =>
            {
                blobNames.Add(b.Name);
            });
            return blobNames;
        }

        public async Task SetLogAsync(string text, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(EContainerName.logs.ToString());

            var appendBlobClient = containerClient.GetAppendBlobClient(blobName);

            await appendBlobClient.CreateIfNotExistsAsync();

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    sw.Write($"{DateTime.Now}: {text} \n");
                    sw.Flush();
                    ms.Position = 0;

                    await appendBlobClient.AppendBlockAsync(ms);
                }
            }
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
