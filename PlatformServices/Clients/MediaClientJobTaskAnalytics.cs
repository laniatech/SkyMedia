﻿using System.Globalization;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace AzureSkyMedia.PlatformServices
{
    public partial class MediaClient
    {
        private static MediaJobTask[] GetVideoAnnotationTasks(MediaClient mediaClient, MediaJobTask jobTask, MediaAssetInput[] inputAssets)
        {
            List<MediaJobTask> jobTasks = new List<MediaJobTask>();
            jobTask.MediaProcessor = MediaProcessor.VideoAnnotation;
            string settingKey = Constant.AppSettingKey.MediaProcessorVideoAnnotationDocumentId;
            string documentId = AppSetting.GetValue(settingKey);
            JObject processorConfig = GetProcessorConfig(documentId);
            jobTask.ProcessorConfig = processorConfig.ToString();
            MediaJobTask[] tasks = SetJobTasks(mediaClient, jobTask, inputAssets, false);
            jobTasks.AddRange(tasks);
            return jobTasks.ToArray();
        }

        private static MediaJobTask[] GetVideoSummarizationTasks(MediaClient mediaClient, MediaJobTask jobTask, MediaAssetInput[] inputAssets)
        {
            List<MediaJobTask> jobTasks = new List<MediaJobTask>();
            jobTask.MediaProcessor = MediaProcessor.VideoSummarization;
            string settingKey = Constant.AppSettingKey.MediaProcessorVideoSummarizationDocumentId;
            string documentId = AppSetting.GetValue(settingKey);
            JObject processorConfig = GetProcessorConfig(documentId);
            JToken processorOptions = processorConfig["options"];
            processorOptions["maxMotionThumbnailDurationInSecs"] = jobTask.ProcessorConfigInteger[MediaProcessorConfig.SummarizationDurationSeconds];
            jobTask.ProcessorConfig = processorConfig.ToString();
            MediaJobTask[] tasks = SetJobTasks(mediaClient, jobTask, inputAssets, false);
            jobTasks.AddRange(tasks);
            return jobTasks.ToArray();
        }

        private static MediaJobTask[] GetSpeechToTextTasks(MediaClient mediaClient, MediaJobTask jobTask, MediaAssetInput[] inputAssets)
        {
            List<MediaJobTask> jobTasks = new List<MediaJobTask>();
            jobTask.MediaProcessor = MediaProcessor.SpeechToText;
            string settingKey = Constant.AppSettingKey.MediaProcessorSpeechToTextDocumentId;
            string documentId = AppSetting.GetValue(settingKey);
            JObject processorConfig = GetProcessorConfig(documentId);
            JToken processorOptions = processorConfig["Features"][0]["Options"];
            JArray captionFormats = new JArray();
            if (jobTask.ProcessorConfigBoolean[MediaProcessorConfig.CaptionFormatWebVtt])
            {
                captionFormats.Add("WebVTT");
            }
            if (jobTask.ProcessorConfigBoolean[MediaProcessorConfig.CaptionFormatTtml])
            {
                captionFormats.Add("TTML");
            }
            if (captionFormats.Count > 0)
            {
                processorOptions["Formats"] = captionFormats;
            }
            processorOptions["Language"] = jobTask.ProcessorConfigString[MediaProcessorConfig.TranscriptLanguage];
            jobTask.ProcessorConfig = processorConfig.ToString();
            MediaJobTask[] tasks = SetJobTasks(mediaClient, jobTask, inputAssets, false);
            jobTasks.AddRange(tasks);
            return jobTasks.ToArray();
        }

        private static MediaJobTask[] GetFaceDetectionTasks(MediaClient mediaClient, MediaJobTask jobTask, MediaAssetInput[] inputAssets)
        {
            string mode = jobTask.ProcessorConfigString[MediaProcessorConfig.FaceDetectionMode];
            List<MediaJobTask> jobTasks = new List<MediaJobTask>();
            jobTask.MediaProcessor = MediaProcessor.FaceDetection;
            string settingKey = Constant.AppSettingKey.MediaProcessorFaceDetectionDocumentId;
            string documentId = AppSetting.GetValue(settingKey);
            JObject processorConfig = GetProcessorConfig(documentId);
            JToken processorOptions = processorConfig["options"];
            processorOptions["mode"] = mode;
            jobTask.ProcessorConfig = processorConfig.ToString();
            MediaJobTask[] tasks = SetJobTasks(mediaClient, jobTask, inputAssets, false);
            jobTasks.AddRange(tasks);
            return jobTasks.ToArray();
        }

        private static MediaJobTask[] GetFaceRedactionTasks(MediaClient mediaClient, MediaJobTask jobTask, MediaAssetInput[] inputAssets)
        {
            string mode = jobTask.ProcessorConfigString[MediaProcessorConfig.FaceRedactionMode];
            List<MediaJobTask> jobTasks = new List<MediaJobTask>();
            jobTask.MediaProcessor = MediaProcessor.FaceRedaction;
            jobTask.Name = string.Concat(jobTask.Name, " ", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mode));
            string settingKey = Constant.AppSettingKey.MediaProcessorFaceRedactionDocumentId;
            string documentId = AppSetting.GetValue(settingKey);
            JObject processorConfig = GetProcessorConfig(documentId);
            JToken processorOptions = processorConfig["options"];
            processorOptions["mode"] = mode;
            jobTask.ProcessorConfig = processorConfig.ToString();
            MediaJobTask[] tasks = SetJobTasks(mediaClient, jobTask, inputAssets, false);
            jobTasks.AddRange(tasks);
            return jobTasks.ToArray();
        }

        private static MediaJobTask[] GetMotionDetectionTasks(MediaClient mediaClient, MediaJobTask jobTask, MediaAssetInput[] inputAssets)
        {
            List<MediaJobTask> jobTasks = new List<MediaJobTask>();
            jobTask.MediaProcessor = MediaProcessor.MotionDetection;
            string settingKey = Constant.AppSettingKey.MediaProcessorMotionDetectionDocumentId;
            string documentId = AppSetting.GetValue(settingKey);
            JObject processorConfig = GetProcessorConfig(documentId);
            jobTask.ProcessorConfig = processorConfig.ToString();
            MediaJobTask[] tasks = SetJobTasks(mediaClient, jobTask, inputAssets, false);
            jobTasks.AddRange(tasks);
            return jobTasks.ToArray();
        }

        private static MediaJobTask[] GetMotionHyperlapseTasks(MediaClient mediaClient, MediaJobTask jobTask, MediaAssetInput[] inputAssets)
        {
            List<MediaJobTask> jobTasks = new List<MediaJobTask>();
            jobTask.MediaProcessor = MediaProcessor.MotionHyperlapse;
            string settingKey = Constant.AppSettingKey.MediaProcessorMotionHyperlapseDocumentId;
            string documentId = AppSetting.GetValue(settingKey);
            JObject processorConfig = GetProcessorConfig(documentId);
            JToken processorSources = processorConfig["Sources"][0];
            processorSources["StartFrame"] = jobTask.ProcessorConfigInteger[MediaProcessorConfig.MotionHyperlapseStartFrame];
            processorSources["NumFrames"] = jobTask.ProcessorConfigInteger[MediaProcessorConfig.MotionHyperlapseFrameCount];
            JToken processorOptions = processorConfig["Options"];
            processorOptions["Speed"] = jobTask.ProcessorConfigInteger[MediaProcessorConfig.MotionHyperlapseSpeed];
            jobTask.ProcessorConfig = processorConfig.ToString();
            MediaJobTask[] tasks = SetJobTasks(mediaClient, jobTask, inputAssets, false);
            jobTasks.AddRange(tasks);
            return jobTasks.ToArray();
        }

        private static MediaJobTask[] GetMotionStabilizationTasks(MediaClient mediaClient, MediaJobTask jobTask, MediaAssetInput[] inputAssets)
        {
            List<MediaJobTask> jobTasks = new List<MediaJobTask>();
            jobTask.MediaProcessor = MediaProcessor.MotionStabilization;
            string settingKey = Constant.AppSettingKey.MediaProcessorMotionStabilizationDocumentId;
            string documentId = AppSetting.GetValue(settingKey);
            JObject processorConfig = GetProcessorConfig(documentId);
            jobTask.ProcessorConfig = processorConfig.ToString();
            MediaJobTask[] tasks = SetJobTasks(mediaClient, jobTask, inputAssets, false);
            jobTasks.AddRange(tasks);
            return jobTasks.ToArray();
        }

        private static MediaJobTask[] GetCharacterRecognitionTasks(MediaClient mediaClient, MediaJobTask jobTask, MediaAssetInput[] inputAssets)
        {
            List<MediaJobTask> jobTasks = new List<MediaJobTask>();
            jobTask.MediaProcessor = MediaProcessor.CharacterRecognition;
            string settingKey = Constant.AppSettingKey.MediaProcessorCharacterRecognitionDocumentId;
            string documentId = AppSetting.GetValue(settingKey);
            JObject processorConfig = GetProcessorConfig(documentId);
            jobTask.ProcessorConfig = processorConfig.ToString();
            MediaJobTask[] tasks = SetJobTasks(mediaClient, jobTask, inputAssets, false);
            jobTasks.AddRange(tasks);
            return jobTasks.ToArray();
        }

        private static MediaJobTask[] GetContentModerationTasks(MediaClient mediaClient, MediaJobTask jobTask, MediaAssetInput[] inputAssets)
        {
            List<MediaJobTask> jobTasks = new List<MediaJobTask>();
            jobTask.MediaProcessor = MediaProcessor.ContentModeration;
            string settingKey = Constant.AppSettingKey.MediaProcessorContentModerationDocumentId;
            string documentId = AppSetting.GetValue(settingKey);
            JObject processorConfig = GetProcessorConfig(documentId);
            jobTask.ProcessorConfig = processorConfig.ToString();
            MediaJobTask[] tasks = SetJobTasks(mediaClient, jobTask, inputAssets, false);
            jobTasks.AddRange(tasks);
            return jobTasks.ToArray();
        }
    }
}
