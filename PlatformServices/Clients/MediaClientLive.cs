﻿using System;
using System.Net;

namespace AzureSkyMedia.PlatformServices
{
    internal partial class MediaClient
    {
        //private void CreateProgram(string programName, IChannel channel, IAsset asset, int? archiveWindowMinutes)
        //{
        //    if (!archiveWindowMinutes.HasValue)
        //    {
        //        string settingKey = Constant.AppSettingKey.MediaChannelProgramArchiveMinutes;
        //        string programArchiveMinutes = AppSetting.GetValue(settingKey);
        //        archiveWindowMinutes = int.Parse(programArchiveMinutes);
        //    }
        //    ProgramCreationOptions programOptions = new ProgramCreationOptions()
        //    {
        //        Name = string.IsNullOrEmpty(programName) ? asset.Name : programName,
        //        AssetId = asset.Id,
        //        ArchiveWindowLength = new TimeSpan(0, archiveWindowMinutes.Value, 0)
        //    };
        //    channel.Programs.Create(programOptions);
        //}

        //private void CreatePrograms(IChannel channel, int? archiveWindowMinutes, bool archiveEncryptionClear,
        //                            bool archiveEncryptionAes, bool archiveEncryptionDrm)
        //{
        //    if (archiveEncryptionClear)
        //    {
        //        string assetName = string.Concat(channel.Name, Constant.Media.Live.ProgramSuffixClear);
        //        IAsset asset = _media2.Assets.Create(assetName, AssetCreationOptions.None);
        //        CreateProgram(null, channel, asset, archiveWindowMinutes);
        //    }
        //    if (archiveEncryptionAes)
        //    {
        //        string assetName = string.Concat(channel.Name, Constant.Media.Live.ProgramSuffixAes);
        //        IAsset asset = _media2.Assets.Create(assetName, AssetCreationOptions.StorageEncrypted);
        //        ContentProtection contentProtection = new ContentProtection()
        //        {
        //            ContentAuthTypeToken = true,
        //            Aes = true
        //        };
        //        CreateProgram(null, channel, asset, archiveWindowMinutes);
        //        SetDeliveryPolicies(asset, contentProtection);
        //    }
        //    if (archiveEncryptionDrm)
        //    {
        //        string assetName = string.Concat(channel.Name, Constant.Media.Live.ProgramSuffixDrm);
        //        IAsset asset = _media2.Assets.Create(assetName, AssetCreationOptions.StorageEncrypted);
        //        ContentProtection contentProtection = new ContentProtection()
        //        {
        //            ContentAuthTypeToken = true,
        //            DrmPlayReady = true,
        //            DrmWidevine = true
        //        };
        //        CreateProgram(null, channel, asset, archiveWindowMinutes);
        //        SetDeliveryPolicies(asset, contentProtection);
        //    }
        //}

        //private IChannel CreateChannel(string channelName, ChannelEncodingType channelType, StreamingProtocol channelProtocol,
        //                               string inputAddressAuthorized, int? inputSubnetPrefixLength,
        //                               string previewAddressAuthorized, int? previewSubnetPrefixLength)
        //{
        //    IPRange inputAddressRange = new IPRange();
        //    if (string.IsNullOrEmpty(inputAddressAuthorized))
        //    {
        //        inputAddressRange.Name = Constant.Media.Live.AllowAnyAddress;
        //        inputAddressRange.Address = new IPAddress(0);
        //        inputAddressRange.SubnetPrefixLength = 0;
        //    }
        //    else
        //    {
        //        inputAddressRange.Name = Constant.Media.Live.AllowAuthorizedAddress;
        //        inputAddressRange.Address = IPAddress.Parse(inputAddressAuthorized);
        //        inputAddressRange.SubnetPrefixLength = inputSubnetPrefixLength.Value;
        //    }

        //    IPRange previewAddressRange = new IPRange();
        //    if (string.IsNullOrEmpty(previewAddressAuthorized))
        //    {
        //        previewAddressRange.Name = Constant.Media.Live.AllowAnyAddress;
        //        previewAddressRange.Address = new IPAddress(0);
        //        previewAddressRange.SubnetPrefixLength = 0;
        //    }
        //    else
        //    {
        //        previewAddressRange.Name = Constant.Media.Live.AllowAuthorizedAddress;
        //        previewAddressRange.Address = IPAddress.Parse(previewAddressAuthorized);
        //        previewAddressRange.SubnetPrefixLength = previewSubnetPrefixLength.Value;
        //    }

        //    ChannelCreationOptions channelOptions = new ChannelCreationOptions()
        //    {
        //        Name = channelName,
        //        EncodingType = channelType,
        //        Input = new ChannelInput()
        //        {
        //            StreamingProtocol = channelProtocol,
        //            AccessControl = new ChannelAccessControl()
        //            {
        //                IPAllowList = new IPRange[] { inputAddressRange }
        //            }
        //        },
        //        Preview = new ChannelPreview()
        //        {
        //            AccessControl = new ChannelAccessControl()
        //            {
        //                IPAllowList = new IPRange[] { previewAddressRange }
        //            }
        //        }
        //    };

        //    if (channelType != ChannelEncodingType.None)
        //    {
        //        channelOptions.Encoding = new ChannelEncoding()
        //        {
        //            SystemPreset = Constant.Media.Live.ChannelEncodingPreset
        //        };
        //    }

        //    return _media2.Channels.Create(channelOptions);
        //}

        //public string CreateChannel(string channelName, MediaEncoding channelEncoding, MediaProtocol inputProtocol,
        //                            string inputAddressAuthorized, int? inputSubnetPrefixLength,
        //                            string previewAddressAuthorized, int? previewSubnetPrefixLength,
        //                            int? archiveWindowMinutes, bool archiveEncryptionClear,
        //                            bool archiveEncryptionAes, bool archiveEncryptionDrm)
        //{
        //    ChannelEncodingType channelType = (ChannelEncodingType)channelEncoding;
        //    StreamingProtocol channelProtocol = (StreamingProtocol)inputProtocol;
        //    IChannel channel = CreateChannel(channelName, channelType, channelProtocol, inputAddressAuthorized, inputSubnetPrefixLength, previewAddressAuthorized, previewSubnetPrefixLength);
        //    CreatePrograms(channel, archiveWindowMinutes, archiveEncryptionClear, archiveEncryptionAes, archiveEncryptionDrm);
        //    foreach (IProgram program in channel.Programs)
        //    {
        //        CreateLocator(LocatorType.OnDemandOrigin, program.Asset);
        //    }
        //    return channel.Id;
        //}

        public bool InsertMarker(string channelName, int? durationSeconds, int cueId, bool showSlate)
        {
            bool inserted = false;
            //IChannel channel = GetEntityByName(MediaEntity.Channel, channelName) as IChannel;
            //if (channel != null)
            //{
            //    if (!durationSeconds.HasValue)
            //    {
            //        string settingKey = Constant.AppSettingKey.MediaChannelAdDurationSeconds;
            //        string adDurationSeconds = AppSetting.GetValue(settingKey);
            //        durationSeconds = int.Parse(adDurationSeconds);
            //    }
            //    TimeSpan adDuration = new TimeSpan(0, 0, 0, durationSeconds.Value);
            //    channel.StartAdvertisement(adDuration, cueId, showSlate);
            //    inserted = true;
            //}
            return inserted;
        }
    }
}