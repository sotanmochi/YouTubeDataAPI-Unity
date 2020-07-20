using System.Collections.Generic;
using UnityEngine;

namespace YouTubeDataAPI
{
    public class LiveChatExample : MonoBehaviour
    {
        [SerializeField] YouTubeDataAPIConfigs _Configs;

        private YouTubeLiveChatService _LiveChatService;

        async void Start()
        {
            _LiveChatService = new YouTubeLiveChatService(_Configs.APIKey);

            string liveChatId = await _LiveChatService.GetActiveLiveChatId(_Configs.VideoId);
            Debug.Log("GetChatId : " + liveChatId);

            List<LiveChatMessage> liveChatMessages = await _LiveChatService.GetLiveChatMessages(liveChatId);
            foreach (LiveChatMessage chatMsg in liveChatMessages)
            {
                Debug.Log(chatMsg.AuthorName + ": " + chatMsg.DisplayMessage + " - " + chatMsg.PublishedAt);
            }
        }
    }
}
