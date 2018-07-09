﻿using Microsoft.AspNetCore.Mvc;

using AzureSkyMedia.PlatformServices;

namespace AzureSkyMedia.WebApp.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            string authToken = HomeController.GetAuthToken(Request, Response);
            using (MediaClient mediaClient = new MediaClient(authToken))
            {
                if (string.IsNullOrEmpty(mediaClient.MediaAccount.VideoIndexerKey))
                {
                    ViewData["accountMessage"] = string.Format(Constant.Message.VideoIndexerKeyMissing, "media search");
                    ViewData["accountUrl"] = "/account/profileEdit";
                }
            }
            return View();
        }
    }
}