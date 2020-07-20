using System;
using UnityEngine;

namespace YouTubeDataAPI
{
    [Serializable]
    [CreateAssetMenu(menuName = "YouTube Data API/Create Configs", fileName = "YouTubeDataAPIConfigs")]
    public class YouTubeDataAPIConfigs : ScriptableObject
    {
        public string APIKey;
        public string VideoId;
    }
}
