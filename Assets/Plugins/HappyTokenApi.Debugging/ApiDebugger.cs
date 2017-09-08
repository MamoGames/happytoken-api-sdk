using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using HappyTokenApi.Client;
using HappyTokenApi.Models;
using Newtonsoft.Json;

namespace HappyTokenApi.Debugging
{
    public class ApiDebugger : MonoBehaviour
    {
        private const string USER_AUTH_PAIR_KEY = "user_auth_pair";
        private const string USER_EMAIL_LOGIN_KEY = "user_email_login_key";

        // Available API environment URLs
        private const string API_URL_PROD = "http://api.happytoken.io";
        private const string API_URL_STAGING = "http://staging.api.happytoken.io";
        private const string API_URL_DEV = "http://dev.api.happytoken.io";
        private const string API_URL_LOCAL = "http://localhost:50456";

        // Use this URL for all API Web Requests
        private const string API_URL = API_URL_LOCAL;

        public static ApiDebugger Instance { get; private set; }

        public string ApiUrl { get; private set; }
        public string DeviceId { get; private set; }

        public bool IsAuthenticated { get; private set; }
        public bool IsUserDataLoaded { get; private set; }
        public bool IsConfigDataLoaded { get; private set; }

        public WebRequest WebRequest { get; private set; }
        public UserAuthPair UserAuthPair { get; private set; }
        public UserEmailLogin UserEmailLogin { get; private set; }

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

            ApiUrl = API_URL;

            DeviceId = SystemInfo.deviceUniqueIdentifier;

            UserAuthPair = LoadUserAuthPair();
            UserEmailLogin = LoadUserEmailLogin();

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

        public void SetConfigDataLoaded(bool isConfigDataLoaded)
        {
            IsConfigDataLoaded = isConfigDataLoaded;
        }

        public void SetUserDataLoaded(bool isUserDataLoaded)
        {
            IsUserDataLoaded = isUserDataLoaded;
        }

        public void SetUserAuthPair(UserAuthPair userAuthPair, bool saveToPlayerPrefs)
        {
            UserAuthPair = userAuthPair;

            if (saveToPlayerPrefs)
            {
                SaveUserAuthPair(userAuthPair);
            }
        }

        public void SetUserEmailLogin(UserEmailLogin userEmailLogin, bool saveToPlayerPrefs)
        {
            UserEmailLogin = userEmailLogin;

            if (saveToPlayerPrefs)
            {
                SaveUserEmailLogin(userEmailLogin);
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

        public UserEmailLogin LoadUserEmailLogin()
        {
            var userEmailJson = PlayerPrefs.GetString(USER_EMAIL_LOGIN_KEY, string.Empty);

            if (!string.IsNullOrEmpty(userEmailJson))
            {
                return JsonConvert.DeserializeObject<UserEmailLogin>(userEmailJson);
            }

            return new UserEmailLogin();
        }

        public void SaveUserEmailLogin(UserEmailLogin userEmailLogin)
        {
            var userEmailJson = JsonConvert.SerializeObject(userEmailLogin);

            PlayerPrefs.SetString(USER_EMAIL_LOGIN_KEY, userEmailJson);

            PlayerPrefs.Save();
        }

        public string HashPassword(string password)
        {
            var sha256 = new SHA256Managed();
            sha256.Initialize();

            var bytes = Encoding.ASCII.GetBytes(password);
            var authHashBytes = sha256.ComputeHash(bytes);

            var sb = new StringBuilder();

            foreach (var b in authHashBytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }

            return sb.ToString();
        }
    }
}
