﻿using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.Management.Media.Models;

using Newtonsoft.Json.Linq;

using AzureSkyMedia.PlatformServices;

namespace AzureSkyMedia.WebApp.Controllers
{
    public class JobController : Controller
    {
        private static SelectListItem[] GetTransforms(MediaClient mediaClient)
        {
            List<SelectListItem> transforms = new List<SelectListItem>
            {
                new SelectListItem()
            };
            Transform[] mediaTransforms = mediaClient.GetAllEntities<Transform>(MediaEntity.Transform);
            foreach (Transform mediaTransform in mediaTransforms)
            {
                SelectListItem transform = new SelectListItem()
                {
                    Text = Constant.TextFormatter.FormatValue(mediaTransform.Name),
                    Value = mediaTransform.Name
                };
                transforms.Add(transform);
            }
            return transforms.ToArray();
        }

        private static SelectListItem[] GetStreamingPolicies(MediaClient mediaClient)
        {
            List<SelectListItem> policies = new List<SelectListItem>
            {
                new SelectListItem()
            };
            StreamingPolicy[] streamingPolicies = mediaClient.GetAllEntities<StreamingPolicy>(MediaEntity.StreamingPolicy);
            foreach (StreamingPolicy streamingPolicy in streamingPolicies)
            {
                SelectListItem policy = new SelectListItem()
                {
                    Text = Constant.TextFormatter.FormatValue(streamingPolicy.Name),
                    Value = streamingPolicy.Name
                };
                policies.Add(policy);
            }
            return policies.ToArray();
        }

        public JsonResult Create(string transformName, string jobName, string jobDescription, Priority jobPriority, string jobData,
                                 string inputFileUrl, string inputAssetName, MediaJobOutputMode outputAssetMode, string streamingPolicyName)
        {
            Job job = null;
            string authToken = HomeController.GetAuthToken(Request, Response);
            using (MediaClient mediaClient = new MediaClient(authToken))
            {
                bool videoAnalyzerPreset = false;
                bool audioAnalyzerPreset = false;
                Transform transform = mediaClient.GetEntity<Transform>(MediaEntity.Transform, transformName);
                foreach (TransformOutput transformOutput in transform.Outputs)
                {
                    if (transformOutput.Preset is VideoAnalyzerPreset)
                    {
                        videoAnalyzerPreset = true;
                    }
                    else if (transformOutput.Preset is AudioAnalyzerPreset)
                    {
                        audioAnalyzerPreset = true;
                    }
                }
                string indexId = null;
                Asset inputAsset = null;
                string assetDescription = null;
                if (!string.IsNullOrEmpty(inputAssetName))
                {
                    inputAsset = mediaClient.GetEntity<Asset>(MediaEntity.Asset, inputAssetName);
                    assetDescription = inputAsset.Description;
                }
                if (mediaClient.IndexerEnabled() && (videoAnalyzerPreset || audioAnalyzerPreset))
                {
                    bool audioOnly = !videoAnalyzerPreset && audioAnalyzerPreset;
                    indexId = mediaClient.IndexerUploadVideo(mediaClient.MediaAccount, inputAsset, null, audioOnly);
                }
                if (!string.IsNullOrEmpty(transformName))
                {
                    if (string.IsNullOrEmpty(inputFileUrl) && inputAsset != null)
                    {
                        StorageBlobClient blobClient = new StorageBlobClient(mediaClient.MediaAccount, inputAsset.StorageAccountName);
                        MediaAsset mediaAsset = new MediaAsset(mediaClient.MediaAccount, inputAsset);
                        string fileName = mediaAsset.Files[0].Name;
                        inputFileUrl = blobClient.GetDownloadUrl(inputAsset.Container, fileName, false);
                    }
                    string[] assetDescriptions = new string[] { assetDescription };
                    string[] assetAlternateIds = new string[] { indexId };
                    job = mediaClient.CreateJob(authToken, transformName, jobName, jobDescription, jobPriority, JObject.Parse(jobData), inputFileUrl, inputAssetName, outputAssetMode, assetDescriptions, assetAlternateIds, streamingPolicyName);
                }
            }
            return Json(job);
        }

        public void Cancel(string jobName, string transformName)
        {
            string authToken = HomeController.GetAuthToken(Request, Response);
            using (MediaClient mediaClient = new MediaClient(authToken))
            {
                mediaClient.CancelJob(transformName, jobName);
            }
        }

        public JsonResult Publish(string jobName)
        {
            MediaJobPublish jobPublish = MediaClient.PublishJobOutput(jobName, null);
            return Json(jobPublish);
        }

        public JsonResult List()
        {
            Job[] jobs;
            string authToken = HomeController.GetAuthToken(Request, Response);
            using (MediaClient mediaClient = new MediaClient(authToken))
            {
                jobs = mediaClient.GetAllEntities<Job, Transform>(MediaEntity.TransformJob, MediaEntity.Transform);
            }
            return Json(jobs);
        }

        public IActionResult Index()
        {
            string authToken = HomeController.GetAuthToken(Request, Response);
            using (MediaClient mediaClient = new MediaClient(authToken))
            {
                ViewData["transformJobs"] = mediaClient.GetAllEntities<Job, Transform>(MediaEntity.TransformJob, MediaEntity.Transform);
                ViewData["transforms"] = GetTransforms(mediaClient);
                ViewData["streamingPolicies"] = GetStreamingPolicies(mediaClient);
            }
            return View();
        }
    }
}