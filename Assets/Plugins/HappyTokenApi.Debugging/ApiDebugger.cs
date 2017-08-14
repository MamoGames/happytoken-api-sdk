﻿using UnityEngine;
using HappyTokenApi.Client;
using HappyTokenApi.Models;
using Newtonsoft.Json;

namespace HappyTokenApi.Debugging
{
    public class ApiDebugger : MonoBehaviour
    {
        private const string USER_AUTH_PAIR_KEY = "user_auth_pair";

        public static ApiDebugger Instance { get; private set; }

        public string ApiUrl { get; private set; }
        public string DeviceId { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public WebRequest WebRequest { get; private set; }
        public UserAuthPair UserAuthPair { get; private set; }

        public CoreDataStore CoreDataStore { get; private set; }
        public ConfigDataStore ConfigDataStore { get; private set; }

        private void Awake()
        {
            // Init singleton static instance
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError($"Trying to initialized the ApiDebugger singleton twice!");
            }

            DeviceId = SystemInfo.deviceUniqueIdentifier;

            ApiUrl = "http://localhost:50456";

            UserAuthPair = LoadUserAuthPair();

            CoreDataStore = new CoreDataStore();
            ConfigDataStore = new ConfigDataStore();

            WebRequest = new WebRequest()
                .SetApiUrl(ApiUrl)
                .SetMonoBehaviour(this);
        }

        public void SetAuthenticated(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }

        public void SetUserAuthPair(UserAuthPair userAuthPair, bool saveToPlayerPrefs)
        {
            UserAuthPair = userAuthPair;

            if (saveToPlayerPrefs)
            {
                SaveUserAuthPair(userAuthPair);
            }
        }

        public UserAuthPair LoadUserAuthPair()
        {
            var userAuthPairJson = PlayerPrefs.GetString(USER_AUTH_PAIR_KEY, string.Empty);

            if (!string.IsNullOrEmpty(userAuthPairJson))
            {
                return JsonConvert.DeserializeObject<UserAuthPair>(userAuthPairJson);
            }

            return new UserAuthPair();
        }

        public void SaveUserAuthPair(UserAuthPair userAuthPair)
        {
            var userAuthPairJson = JsonConvert.SerializeObject(userAuthPair);

            PlayerPrefs.SetString(USER_AUTH_PAIR_KEY, userAuthPairJson);

            PlayerPrefs.Save();
        }
    }
}