﻿using System.Collections.Generic;

using Microsoft.WindowsAzure.MediaServices.Client;

namespace AzureSkyMedia.PlatformServices
{
    public class MediaJobTask
    {
        public MediaJobTask()
        {
            this.ProcessorConfigString = new Dictionary<string, string>();
            this.ProcessorConfigBoolean = new Dictionary<string, bool>();
            this.ProcessorConfigInteger = new Dictionary<string, int>();
        }

        public MediaJobTask CreateCopy()
        {
            return (MediaJobTask)this.MemberwiseClone();
        }

        public int? ParentIndex { get; set; }

        public string Name { get; set; }

        public string[] InputAssetIds { get; set; }

        public MediaProcessor MediaProcessor { get; set; }

        public string ProcessorConfig { get; set; }

        public string ProcessorDocumentId { get; set; }

        public string OutputAssetName { get; set; }

        public AssetFormatOption OutputAssetFormat { get; set; }

        public AssetCreationOptions OutputAssetEncryption { get; set; }

        public Dictionary<string, string> ProcessorConfigString { get; set; }

        public Dictionary<string, bool> ProcessorConfigBoolean { get; set; }

        public Dictionary<string, int> ProcessorConfigInteger { get; set; }

        public ContentProtection ContentProtection { get; set; }

        public IndexerConfig IndexerConfig { get; set; }

        public TaskOptions Options { get; set; }
    }

    public class IndexerConfig
    {
        public string LanguageId { get; set; }

        public string SearchPartition { get; set; }

        public string VideoDescription { get; set; }

        public string VideoMetadata { get; set; }

        public bool VideoPublic { get; set; }

        public bool AudioOnly { get; set; }
    }
}