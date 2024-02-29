using System;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.Xml;

public class CPHInline
{
    public bool Execute()
    {
        if (!args.ContainsKey("videoUrl"))
        {
            CPH.LogError("videoUrl argument is missing");
            return true;
        }

        var videoUrl = args["videoUrl"].ToString();
        if (!args.ContainsKey("scene"))
        {
            CPH.LogError("scene argument is missing");
            return true;
        }

        var scene = args["scene"].ToString();
        if (!args.ContainsKey("source"))
        {
            CPH.LogError("source agument is missing");
            return true;
        }

        var source = args["source"].ToString();
        var apiKey = CPH.GetGlobalVar<string>("YOUTUBE_API_KEY", true);
        if (String.IsNullOrEmpty(apiKey))
        {
            CPH.LogError("Missing youtube api key");
        }

        var videoId = ExtractVideoId(videoUrl);
        if (videoId == null)
        {
            CPH.LogError("Video Id extraction failed");
        }

        CPH.LogDebug("VideoId=" + videoId);
        var youtubeService = new YouTubeService(new BaseClientService.Initializer() { ApiKey = apiKey, ApplicationName = "Stream" });
        var videoRequest = youtubeService.Videos.List("contentDetails");
        videoRequest.Id = videoId;
        var response = videoRequest.ExecuteAsync().Result;
        if (response.Items.Count > 0)
        {
            var item = response.Items[0];
            var durationStr = response.Items[0].ContentDetails.Duration;
            var durationTS = XmlConvert.ToTimeSpan(durationStr);
            CPH.LogDebug("Duration " + durationTS.TotalMilliseconds);
            CPH.ObsSetBrowserSource(scene, source, videoUrl);
            CPH.ObsSetSourceVisibility(scene, source, true);
            var delay = (int)durationTS.TotalMilliseconds;
            delay += 2 * 1000; // Add 2 sec extra delay
            CPH.Wait(delay);
            CPH.ObsSetBrowserSource(scene, source, "about:blank");
            CPH.ObsSetSourceVisibility(scene, source, false);
        }
        else
        {
            CPH.LogError("Video not found");
        }

        return true;
    }

    public string ExtractVideoId(string url)
    {
        if (url.Contains("youtu.be"))
        {
            // Shortened URL
            var startIndex = url.LastIndexOf("/") + 1;
            return url.Substring(startIndex);
        }
        else if (url.Contains("youtube.com/embed/"))
        {
            // Embed URL
            var startIndex = url.LastIndexOf("embed/") + 6;
            url = url.Substring(startIndex, url.Length - startIndex);
            var endIndex = url.IndexOf('?');
            if (endIndex == -1)
                endIndex = url.Length;
            return url.Substring(0, endIndex);
        }
        else if (url.Contains("watch?v="))
        {
            // Standard URL
            var startIndex = url.IndexOf("watch?v=") + 8;
            var endIndex = url.IndexOf("&", startIndex);
            if (endIndex == -1)
                endIndex = url.Length; // No additional URL parameters
            return url.Substring(startIndex, endIndex - startIndex);
        }

        return null;
    }
}
