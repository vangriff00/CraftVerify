namespace AzureDataService
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Microsoft.Data.SqlClient;
    using Microsoft.Azure.Cosmos;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using DataAccessLibraryCraftVerify;

    public class AzureDataService
    {
        private BlobServiceClient _blobServiceClient;
        private SqlConnection _sqlConnection;
        private CosmosClient _cosmosClient;

        public AzureDataService(string blobStorageConnectionString, string sqlDatabaseConnectionString, string cosmosDbConnectionString)
        {
            _blobServiceClient = new BlobServiceClient(blobStorageConnectionString);
            _sqlConnection = new SqlConnection(sqlDatabaseConnectionString);
            _cosmosClient = new CosmosClient(cosmosDbConnectionString);
        }

        // Blob Storage CRUD operations
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
        }p

        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteAsync();
        }


        // SQL Database CRUD operations
        public void InsertRecordToSql(string query)
        {
            using (var sqlCommand = new SqlCommand(query, _sqlConnection))
            {
                _sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                _sqlConnection.Close();
            }
        }

        public void UpdateRecordInSql(string query)
        {
            using (var sqlCommand = new SqlCommand(query, _sqlConnection))
            {
                _sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                _sqlConnection.Close();
            }
        }

        public void DeleteRecordInSql(string query)
        {
            using (var sqlCommand = new SqlCommand(query, _sqlConnection))
            {
                _sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                _sqlConnection.Close();
            }
        }

        // Cosmos DB CRUD operations
        public async Task CreateDocumentAsync(string databaseId, string containerId, MyCosmosDocument document)
        {
            var container = _cosmosClient.GetContainer(databaseId, containerId);
            await container.CreateItemAsync(document);
        }

        public async Task ReplaceDocumentAsync(string databaseId, string containerId, MyCosmosDocument document)
        {
            var container = _cosmosClient.GetContainer(databaseId, containerId);
            await container.ReplaceItemAsync(document, document.Id, new PartitionKey(document.PartitionKey));
        }

        public async Task DeleteDocumentAsync(string databaseId, string containerId, string documentId, string partitionKey)
        {
            var container = _cosmosClient.GetContainer(databaseId, containerId);
            await container.DeleteItemAsync<string>(documentId, new PartitionKey(partitionKey));
        }
    }
}
