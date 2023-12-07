namespace DataAccessLibraryCraftVerify
{
    using Azure;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task UploadBlobAsync(string containerName, string blobName, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var response = await blobClient.UploadAsync(content, true);

            if (response.Status != 201)
            {
                throw new Exception($"Failed to upload blob. Status code: {response.Status}");
            }
        }

        public async Task<Stream> DownloadBlobAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            Response<BlobDownloadInfo> response = await blobClient.DownloadAsync();

            if (response.Status != 200)
            {
                throw new Exception($"Failed to download blob. Status code: {response.Status}");
            }

            return response.Value.Content;
        }

        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteAsync();
        }
    }
}