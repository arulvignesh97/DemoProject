using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CTS.Applens.Azure
{
    public class UploadDownload:IDisposable
    {
       public static Stream memoryStream = new MemoryStream();
        public static async Task<Stream> DownloadStreamAsync(string azureConnection, string containerName, string fileName)
        {
            BlobContainerClient container = new(azureConnection, containerName);
            BlobClient blob = container.GetBlobClient(fileName);
            if (await blob.ExistsAsync())
            {       
                    await blob.DownloadToAsync(memoryStream);
                    return memoryStream;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public static async Task<string> DownloadStringAsync(string azureConnection, string containerName, string fileName)
        {
            string content = string.Empty;
            BlobContainerClient container = new(azureConnection, containerName);
            BlobClient blob = container.GetBlobClient(fileName);
            if (await blob.ExistsAsync())
            {
                BlobDownloadResult downloadResult = await blob.DownloadContentAsync();
                content = downloadResult.Content.ToString();
                return content;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public static async Task<bool> UploadStringAsync(string azureConnection, string containerName, string path)
        {
            BlobContainerClient container = new BlobContainerClient(azureConnection, containerName);
            BlobClient blob = container.GetBlobClient(Path.GetFileName(path));
            if (await blob.ExistsAsync())
            {
                await blob.UploadAsync(path);
                return true;
            }
            else
            {
                return false;
            }

        }
        public static async Task<bool> UploadStreamAsync(string azureConnection, string containerName, FileStream file)
        {
            BlobContainerClient container = new BlobContainerClient(azureConnection, containerName);
            BlobClient blob = container.GetBlobClient(file.Name);
            if (await blob.ExistsAsync())
            {
                await blob.UploadAsync(file);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async Task<IList<string>> GetBlobsAsync(string azureConnection, string containerName)
        {
            BlobContainerClient container = new BlobContainerClient(azureConnection, containerName);
            var blob = container.GetBlobsAsync();
            IList<string> result = new List<string>();
            await foreach (BlobItem blobPage in blob)
            {
                result.Add(blobPage.Name);
            }
            return result;
        }

        public void Dispose()
        {
            if (memoryStream != null)
            {
                memoryStream.Dispose();
            }
        }
    }
}