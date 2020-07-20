using System;
using System.Collections.Generic;
using UnityEngine;

namespace YouTubeDataAPI.Examples
{
    public enum Color
    {
        Red,
        Green,
        Blue,
    }

    public class ChatMessageAggregationExample : MonoBehaviour
    {
        [SerializeField] YouTubeDataAPIConfigs _Configs;

        YouTubeLiveChatService _LiveChatService;

        Dictionary<string, Color> _ColorDict = new Dictionary<string, Color>()
        {
            {"あか", Color.Red}, {"赤", Color.Red},
            {"みどり", Color.Green}, {"緑", Color.Green},
            {"青", Color.Blue},
        };

        Dictionary<Color, int> _Counters = new Dictionary<Color, int>();

        async void Start()
        {
            InitCounters();

            _LiveChatService = new YouTubeLiveChatService(_Configs.APIKey);

            string liveChatId = await _LiveChatService.GetActiveLiveChatId(_Configs.VideoId);
            List<LiveChatMessage> liveChatMessages = await _LiveChatService.GetLiveChatMessages(liveChatId);
            foreach (LiveChatMessage chatMsg in liveChatMessages)
            {
                Debug.Log(chatMsg.AuthorName + ": " + chatMsg.DisplayMessage + " - " + chatMsg.PublishedAt);

                Color color;
                if (_ColorDict.TryGetValue(chatMsg.DisplayMessage, out color))
                {
                    _Counters[color]++;
                }
            }

            // Check result
            foreach (var counter in _Counters)
            {
                Debug.Log("Counter[" + counter.Key + "]: " + counter.Value);
            }
        }

        void InitCounters()
        {
            foreach (Color color in Enum.GetValues(typeof(Color)))
            {
                _Counters[color] = 0;
            }
        }
    }
}
