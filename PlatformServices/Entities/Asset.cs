﻿using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure.Management.Media.Models;

namespace AzureSkyMedia.PlatformServices
{
    public class MediaAsset : Asset
    {
        internal MediaAsset(MediaClient mediaClient, Asset asset) : base(asset.Id, asset.Name, asset.Type, asset.AssetId, asset.Created, asset.LastModified, asset.AlternateId, asset.Description, asset.Container, asset.StorageAccountName, asset.StorageEncryptionFormat)
        {
            StorageBlobClient blobClient = new StorageBlobClient(mediaClient.MediaAccount, asset.StorageAccountName);
            Files = GetAssetFiles(blobClient, asset.Container, null);
            Filters = mediaClient.GetAllEntities<AssetFilter>(MediaEntity.FilterAsset, null, asset.Name);
            StreamingLocators = mediaClient.GetStreamingUrls(asset.Name);
        }

        internal static string GetAssetName(StorageBlobClient blobClient, string containerName, string directoryPath, out MediaFile[] assetFiles)
        {
            string assetName = null;
            assetFiles = GetAssetFiles(blobClient, containerName, directoryPath);
            if (assetFiles.Length == 1)
            {
                assetName = assetFiles[0].Name;
            }
            else
            {
                foreach (MediaFile assetFile in assetFiles)
                {
                    if (assetFile.Name.EndsWith(Constant.Media.Stream.ManifestExtension, StringComparison.OrdinalIgnoreCase))
                    {
                        assetName = assetFile.Name;
                    }
                }
            }
            if (!string.IsNullOrEmpty(assetName))
            {
                assetName = Path.GetFileNameWithoutExtension(assetName);
            }
            return assetName;
        }

        internal static MediaFile[] GetAssetFiles(StorageBlobClient blobClient, string containerName, string directoryPath)
        {
            BlobContinuationToken continuationToken = null;
            List<MediaFile> files = new List<MediaFile>();
            CloudBlobContainer blobContainer = blobClient.GetBlobContainer(containerName);
            do
            {
                BlobResultSegment blobList;
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    CloudBlobDirectory blobDirectory = blobContainer.GetDirectoryReference(directoryPath);
                    blobList = blobDirectory.ListBlobsSegmentedAsync(continuationToken).Result;
                }
                else
                {
                    blobList = blobContainer.ListBlobsSegmentedAsync(continuationToken).Result;
                }
                foreach (IListBlobItem blobItem in blobList.Results)
                {
                    string fileName = Path.GetFileName(blobItem.Uri.ToString());
                    string fileSize = blobClient.GetBlobSize(containerName, directoryPath, fileName, out long byteCount, out string contentType);
                    MediaFile file = new MediaFile()
                    {
                        Name = fileName,
                        Size = fileSize,
                        ByteCount = byteCount,
                        ContentType = contentType,
                        DownloadUrl = blobClient.GetDownloadUrl(containerName, fileName, false)
                    };
                    files.Add(file);
                }
                continuationToken = blobList.ContinuationToken;
            } while (continuationToken != null);
            return files.ToArray();
        }

        public MediaFile[] Files { get; }

        public AssetFilter[] Filters { get; }

        public string Size
        {
            get
            {
                long byteCount = 0;
                foreach (MediaFile assetFile in Files)
                {
                    byteCount = byteCount + assetFile.ByteCount;
                }
                return StorageBlobClient.MapByteCount(byteCount);
            }
        }

        public string[] StreamingLocators { get; }
    }

    public class MediaFile
    {
        public string Name { get; set; }

        public string Size { get; set; }

        public long ByteCount { get; set; }

        public string ContentType { get; set; }

        public string DownloadUrl { get; set; }
    }
}