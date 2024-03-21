using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary
{
    public enum EContainerName
    {
        pictures,
        pdfs,
        logs
    }
    public interface IBlobStorage
    {
         public string BlobUrl { get; }
        Task UploadAsync(Stream fileStream, string fileName,EContainerName eContainerName);
        Task<Stream> DownloadAsync(string fileName, EContainerName eContainerName);

        Task DeleteAsync(string fileName, EContainerName eContainerName);   

        Task SetLogAsync(string text,string blobName);

        Task<List<string>> GetLogAsync(string blobName);

        List<string> GetNames(EContainerName eContainerName);
    }
}
