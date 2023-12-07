using DataAccessLibraryCraftVerify;

class Program
{
    static async Task Main()
    {
        string connectionString = "your_connection_string_here";
        BlobStorageService blobStorageService = new BlobStorageService(connectionString);

        string containerName = "your-container-name";
        string blobName = "example.txt";

        // Upload a blob
        using (var content = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Hello, Azure Blob Storage!")))
        {
            await blobStorageService.UploadBlobAsync(containerName, blobName, content);
        }

        // Download a blob
        Stream downloadedContent = await blobStorageService.DownloadBlobAsync(containerName, blobName);
        using (var reader = new StreamReader(downloadedContent))
        {
            Console.WriteLine(await reader.ReadToEnd());
        }

        // Delete a blob
        await blobStorageService.DeleteBlobAsync(containerName, blobName);
    }
}
