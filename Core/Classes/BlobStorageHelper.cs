using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes
{
    public class BlobStorageHelper
    {
        private static string connectionString = "BlobEndpoint=https://sitetiming.blob.core.windows.net/;QueueEndpoint=https://sitetiming.queue.core.windows.net/;FileEndpoint=https://sitetiming.file.core.windows.net/;TableEndpoint=https://sitetiming.table.core.windows.net/;SharedAccessSignature=sv=2020-08-04&ss=bfqt&srt=sco&sp=rwdlacupitfx&se=2032-01-26T20:40:07Z&st=2012-01-26T12:40:07Z&spr=https,http&sig=yj8jk2mhZ58EVBAFb60nVvSTuH8EFWvkwsgCzFuiJQg%3D";
        public static async Task UploadPageBlob(string localFilePath, string fileName, string containerName, IDictionary<string, string> metadata)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            //Create a unique name for the container
            //string containerName = "quickstartblobs" + Guid.NewGuid().ToString();

            // Create the container and return a container client object
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // Upload data from the local file
            await blobClient.UploadAsync(localFilePath, true);
        }

        public static async Task UploadBlob(string localFilePath, string fileName, string containerName, IDictionary<string, string> metadata)
        {

            //if (metadata.ContainsKey(nameof(YoutubeDownloadedFileInfo.UniqueKey)))
            //{
            //    var uniqueKey = metadata[nameof(YoutubeDownloadedFileInfo.UniqueKey)];
            //    //var uniqueKeyEncoded = uniqueKey.Base64StringEncode();

            //    var query = $"\"UniqueKey\" = '{uniqueKey}'";
            //    cachedData = await BlobStorageHelper.GetFirstBlobContent("media", query, false);
            //}


                // Create a BlobServiceClient object which will be used to create a container client
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                //Create a unique name for the container
                //string containerName = "media";

                // Create the container and return a container client object
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(fileName);


                // Upload data from the local file
                await blobClient.UploadAsync(localFilePath, true);

                await AddBlobMetadataAsync(blobClient, metadata);
        }

        // https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-properties-metadata?tabs=dotnet

        public static async Task AddBlobMetadataAsync(BlobBaseClient blob, IDictionary<string, string> metadata)
        {
            try
            {
                var keysWithNull = metadata.Where(s => s.Value == null).Select(s => s.Key);

                foreach (var key in keysWithNull)
                {
                    metadata.Remove(key);
                }
                // Set the blob's metadata.
                await blob.SetMetadataAsync(metadata);
                // Azure.RequestFailedException: 'Blob tags are only supported on General Purpose v2 storage accounts.
                //RequestId: 007f3960 - 201e-0086 - 2d67 - bfbb8c000000
                //Time: 2021 - 10 - 12T12: 48:32.7572505Z
                //Status: 400(Blob tags are only supported on General Purpose v2 storage accounts.)
                //ErrorCode: BlobTagsNotSupportedForAccountType
                await blob.SetTagsAsync(metadata);

            }
            catch (RequestFailedException e)
            {
                //telemetryClient.TrackException(e);
            }
        }

        public static async Task<IDictionary<string, string>> ReadBlobMetadataAsync(BlobBaseClient blob)
        {
            try
            {
                // Get the blob's properties and metadata.
                BlobProperties properties = await blob.GetPropertiesAsync();

                return properties.Metadata;

            }
            catch (RequestFailedException e)
            {
                //telemetryClient.TrackException(e);
            }

            return new Dictionary<string, string>();

        }

        private static async Task GetBlobPropertiesAsync(BlobClient blob)
        {
            try
            {
                // Get the blob properties
                BlobProperties properties = await blob.GetPropertiesAsync();

                // Display some of the blob's property values
                Console.WriteLine($" ContentLanguage: {properties.ContentLanguage}");
                Console.WriteLine($" ContentType: {properties.ContentType}");
                Console.WriteLine($" CreatedOn: {properties.CreatedOn}");
                Console.WriteLine($" LastModified: {properties.LastModified}");
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine($"HTTP error code {e.Status}: {e.ErrorCode}");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        public static async Task SetBlobPropertiesAsync(BlobClient blob)
        {
            Console.WriteLine("Setting blob properties...");

            try
            {
                // Get the existing properties
                BlobProperties properties = await blob.GetPropertiesAsync();

                BlobHttpHeaders headers = new BlobHttpHeaders
                {
                    // Set the MIME ContentType every time the properties 
                    // are updated or the field will be cleared
                    ContentType = "text/plain",
                    ContentLanguage = "en-us",

                    // Populate remaining headers with 
                    // the pre-existing properties
                    CacheControl = properties.CacheControl,
                    ContentDisposition = properties.ContentDisposition,
                    ContentEncoding = properties.ContentEncoding,
                    ContentHash = properties.ContentHash
                };

                // Set the blob's properties.
                await blob.SetHttpHeadersAsync(headers);
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine($"HTTP error code {e.Status}: {e.ErrorCode}");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
