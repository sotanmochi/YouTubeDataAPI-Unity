using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace YouTubeDataAPI
{
    public class LiveChatMessage
    {
        public string DisplayMessage { get; private set; }
        public string AuthorName { get; private set; }
        public string PublishedAt { get; private set; }

        public LiveChatMessage(string message, string authorName, string publishedAt)
        {
            this.DisplayMessage = message;
            this.AuthorName = authorName;
            this.PublishedAt = publishedAt;
        }
    }

    // Reference: https://qiita.com/iroiro_bot/items/ad0f3901a2336fe48e8f
    public class YouTubeLiveChatService
    {
        private string _APIKey;
        private string _BaseUrl_Videos = "https://www.googleapis.com/youtube/v3/videos";
        private string _BaseUrl_LiveChatMessages = "https://www.googleapis.com/youtube/v3/liveChat/messages";

        public YouTubeLiveChatService(string key)
        {
            _APIKey = key;
        }

        // Reference: https://developers.google.com/youtube/v3/docs/videos
        public async UniTask<string> GetActiveLiveChatId(string liveVideoId)
        {
            StringBuilder url = new StringBuilder(50);
            url.Append(_BaseUrl_Videos);
            url.Append("?key=").Append(_APIKey);
            url.Append("&id=").Append(liveVideoId);
            url.Append("&part=").Append("liveStreamingDetails");

            using (var uwr = UnityWebRequest.Get(url.ToString()))
            {
                await uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    throw new Exception(uwr.error);
                }

                JsonNode data = JsonNode.Parse(uwr.downloadHandler.text);
                JsonNode liveStreamingDetails = data["items"][0]["liveStreamingDetails"];
                string liveChatId = liveStreamingDetails["activeLiveChatId"].Get<string>();

                return liveChatId;
            }
        }

        // Reference: https://developers.google.com/youtube/v3/live/docs/liveChatMessages
        public async UniTask<List<LiveChatMessage>> GetLiveChatMessages(string liveChatId)
        {
            List<LiveChatMessage> liveChatMessages = new List<LiveChatMessage>();

            StringBuilder url = new StringBuilder(50);
            url.Append(_BaseUrl_LiveChatMessages);
            url.Append("?key=").Append(_APIKey);
            url.Append("&liveChatId=").Append(liveChatId);
            url.Append("&part=").Append("id");
            url.Append("&part=").Append("snippet");
            url.Append("&part=").Append("authorDetails");

            using (var uwr = UnityWebRequest.Get(url.ToString()))
            {
                await uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    throw new Exception(uwr.error);
                }

                JsonNode data = JsonNode.Parse(uwr.downloadHandler.text);
                JsonNode items = data["items"];

                foreach (JsonNode item in items)
                {
                    string message = item["snippet"]["displayMessage"].Get<string>();
                    string authorName = item["authorDetails"]["displayName"].Get<string>();
                    string publishedAt = item["snippet"]["publishedAt"].Get<string>();
                    liveChatMessages.Add(new LiveChatMessage(message, authorName, publishedAt));
                }

                return liveChatMessages;
            }
        }
    }
}
