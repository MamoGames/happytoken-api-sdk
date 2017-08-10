using HappyTokenApi.Client;
using HappyTokenApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class ApiTester : MonoBehaviour
{
    private const string USER_AUTH_PAIR_KEY = "user_auth_pair";

    private bool m_IsAuthenticated;

    private Client m_Client;

    private string m_ApiUrl;
    private string m_DeviceId;

    private UserAuthPair m_UserAuthPair;
    private Profile m_UserProfile;
    private Wallet m_Wallet;
    private Happiness m_Happiness;
    private List<UserAvatar> m_Avatars;
    private List<UserBuilding> m_Buildings;
    private List<UserCake> m_Cakes;

    private string m_HttpResponse = "Nothing";

    private void Start()
    {
        m_ApiUrl = "http://localhost:50456";

        m_Client = new Client()
            .SetApiUrl(m_ApiUrl)
            .SetMonoBehaviour(this);

        m_DeviceId = SystemInfo.deviceUniqueIdentifier;

        m_UserAuthPair = new UserAuthPair();

        LoadUserAuthPair();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(5, 5, Screen.width - 10, Screen.height - 10), GUIContent.none, "window");

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.MaxWidth(300));

        GUILayout.Label("ApiUrl");
        m_ApiUrl = GUILayout.TextField(m_ApiUrl);

        GUILayout.Label("DeviceId");
        m_DeviceId = GUILayout.TextField(m_DeviceId);

        GUILayout.Label("UserId");
        m_UserAuthPair.UserId = GUILayout.TextField(m_UserAuthPair.UserId);

        GUILayout.Label("AuthToken");
        m_UserAuthPair.AuthToken = GUILayout.TextField(m_UserAuthPair.AuthToken);

        if (GUILayout.Button("Authenticate"))
        {
            m_Client.Authenticate(m_UserAuthPair, (jsonWebToken) =>
            {
                m_Client.SetJsonWebToken(jsonWebToken);

                m_IsAuthenticated = true;

                Debug.Log($"SUCCESS! JWT AccessToken:{jsonWebToken.AccessToken}, Expires:{jsonWebToken.ExpiresInSecs}");
            }, s =>
            {
                Debug.LogError("Client.Authenticate: Failed");
            });
        }

        if (GUILayout.Button("Create User"))
        {
            var userDevice = new UserDevice
            {
                DeviceId = m_DeviceId
            };

            m_Client.CreateUser(userDevice, (userAuthPair) =>
            {
                m_UserAuthPair = userAuthPair;

                SaveUserAuthPair(userAuthPair);

                Debug.Log($"UserAuthPair UserId:{userAuthPair.UserId}, AuthToken:{userAuthPair.AuthToken}");
            }, s =>
            {
                m_UserAuthPair.UserId = "";
                m_UserAuthPair.AuthToken = "";

                Debug.LogError("Client.CreateUser: Failed");
            });
        }

        GUI.enabled = m_IsAuthenticated;

        if (GUILayout.Button("Get User"))
        {
            m_Client.GetUser(m_UserAuthPair.UserId, (userLogin) =>
            {
                m_UserProfile = userLogin.Profile;
                m_Wallet = userLogin.Wallet;
                m_Happiness = userLogin.Happiness;
                m_Avatars = userLogin.UserAvatars;
                m_Buildings = userLogin.UserBuildings;
                m_Cakes = userLogin.UserCakes;
            }, s =>
            {
                Debug.LogError("Client.GetUser: Failed");
            });
        }

        GUI.enabled = true;

        GUILayout.EndVertical();

        // Console
        GUILayout.BeginVertical();
        GUILayout.TextArea(m_HttpResponse);

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void LoadUserAuthPair()
    {
        var userAuthPairJson = PlayerPrefs.GetString(USER_AUTH_PAIR_KEY, string.Empty);

        if (!string.IsNullOrEmpty(userAuthPairJson))
        {
            m_UserAuthPair = JsonConvert.DeserializeObject<UserAuthPair>(userAuthPairJson);
        }
    }

    private void SaveUserAuthPair(UserAuthPair userAuthPair)
    {
        var userAuthPairJson = JsonConvert.SerializeObject(userAuthPair);

        PlayerPrefs.SetString(USER_AUTH_PAIR_KEY, userAuthPairJson);

        PlayerPrefs.Save();
    }
}
