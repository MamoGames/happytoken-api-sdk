using HappyTokenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace HappyTokenApi.Client
{
    public class Client
    {
        private string m_ApiUrl;
        private JsonWebToken m_JsonWebToken;
        private MonoBehaviour m_MonoBehaviour;

        public Client()
        {
        }

        public Client SetApiUrl(string apiUrl)
        {
            m_ApiUrl = apiUrl;

            return this;
        }

        public Client SetMonoBehaviour(MonoBehaviour monoBehaviour)
        {
            m_MonoBehaviour = monoBehaviour;

            return this;
        }

        public Client SetJsonWebToken(JsonWebToken jsonWebToken)
        {
            m_JsonWebToken = jsonWebToken;

            return this;
        }

        #region Api Requests

        public void CreateUser(UserDevice userDevice, Action<UserAuthPair> onSuccess, Action<string> onFail)
        {
            var routeUrl = $"{m_ApiUrl}/users";
            var data = JsonConvert.SerializeObject(userDevice);

            m_MonoBehaviour.StartCoroutine(StartWebRequest(routeUrl, data, onSuccess, onFail));
        }

        public void Authenticate(UserAuthPair userAuthPair, Action<JsonWebToken> onSuccess, Action<string> onFail)
        {
            var routeUrl = $"{m_ApiUrl}/authentication";
            var data = JsonConvert.SerializeObject(userAuthPair);

            m_MonoBehaviour.StartCoroutine(StartWebRequest(routeUrl, data, onSuccess, onFail));
        }

        public void GetUser(string userId, Action<UserLogin> onSuccess, Action<string> onFail)
        {
            var routeUrl = $"{m_ApiUrl}/users/{userId}";

            m_MonoBehaviour.StartCoroutine(StartWebRequest(routeUrl, null, onSuccess, onFail, useJwt: true));
        }

        #endregion

        #region Helpers

        private IEnumerator StartWebRequest<T>(string url, string data, Action<T> onSuccess, Action<string> onFail, bool useJwt = false)
        {
            // Using scope will ensure the UnityWebRequest is disposed after use
            using (var webRequest = string.IsNullOrEmpty(data) ? UnityWebRequest.Get(url) : UnityWebRequest.Post(url, data))
            {
                if (useJwt)
                {
                    if (!string.IsNullOrEmpty(m_JsonWebToken?.AccessToken))
                    {
                        var jwtHeader = $"Bearer {m_JsonWebToken.AccessToken}";
                        webRequest.SetRequestHeader("Authorization", jwtHeader);
                    }
                    else
                    {
                        Debug.LogError($"The JWT has not been set. Likely the user is not authneticatied.");
                        yield break;
                    }
                }

                if (webRequest.method == "POST")
                {
                    var uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(data))
                    {
                        contentType = "application/json"
                    };

                    webRequest.uploadHandler = uploadHandler;
                }

                webRequest.timeout = 60;
                webRequest.Send();

                var time = 0f;

                while (!webRequest.isDone)
                {
                    time += Time.deltaTime;

                    yield return new WaitForEndOfFrame();
                }

                Debug.LogFormat("WebRequest to {0} took {1:N2}s (Response:{2}, Error:{3})", url, time, webRequest.responseCode, webRequest.error);

                if (webRequest.isNetworkError)
                {
                    onFail?.Invoke($"WebRequest Network Error:{webRequest.error}");
                }
                else
                {
                    // Did the server return an HTTP error response code?
                    if (webRequest.responseCode >= 400)
                    {
                        onFail?.Invoke($"WebRequest HTTP Error:{webRequest.error}");
                    }
                    else
                    {
                        var downloadHandler = webRequest.downloadHandler;

                        if (!string.IsNullOrEmpty(downloadHandler.text))
                        {
                            var dataObject = JsonConvert.DeserializeObject<T>(downloadHandler.text);

                            if (dataObject != null)
                            {
                                onSuccess?.Invoke(dataObject);
                            }
                            else
                            {
                                onFail?.Invoke($"WebRequest Json Deserialization Error: Could not deserialize payload to type {typeof(T)}.");
                            }
                        }
                        else
                        {
                            onFail?.Invoke("WebRequest Payload Error: Data was null or empty.");
                        }
                    }
                }
            }
        }

        #endregion
    }
}