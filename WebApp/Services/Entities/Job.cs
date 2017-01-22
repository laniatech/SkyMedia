﻿namespace AzureSkyMedia.Services
{
    public class MediaJob
    {
        public string Name { get; set; }

        public int Priority { get; set; }

        public bool AutoScale { get; set; }

        public MediaJobTask[] Tasks { get; set; }
    }
}
