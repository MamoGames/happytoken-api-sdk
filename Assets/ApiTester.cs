using HappyTokenApi.Client;
using HappyTokenApi.Models;
using UnityEngine;

public class ApiTester : MonoBehaviour
{
    private Client m_Client;

    private string m_ApiUrl;
    private string m_DeviceId;
    private string m_UserId;
    private string m_AuthToken;

    private string m_HttpResponse = "Nothing";

    private void Start()
    {
        m_ApiUrl = "http://localhost:50456";

        m_Client = new Client()
            .SetApiUrl(m_ApiUrl)
            .SetMonoBehaviour(this);

        m_DeviceId = SystemInfo.deviceUniqueIdentifier;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(5, 5, Screen.width - 10, Screen.height - 10));

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("DeviceId");
        m_DeviceId = GUILayout.TextField(m_DeviceId);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("UserId");
        m_UserId = GUILayout.TextField(m_UserId);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("AuthToken");
        m_AuthToken = GUILayout.TextField(m_AuthToken);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Create new User"))
        {
            var userDevice = new UserDevice
            {
                DeviceId = m_DeviceId
            };

            m_Client.CreateUser(userDevice, (userAuthPair) =>
            {
                m_UserId = userAuthPair.UserId;
                m_AuthToken = userAuthPair.AuthToken;

                Debug.Log($"UserAuthPair UserId:{userAuthPair.UserId}, AuthToken:{userAuthPair.AuthToken}");
            }, s =>
            {
                m_UserId = "";
                m_AuthToken = "";

                Debug.LogError("Failed");
            });
        }

        if (GUILayout.Button("Login"))
        {
            var userAuthPair = new UserAuthPair
            {
                UserId = m_UserId,
                AuthToken = m_AuthToken
            };

            m_Client.LoginUser(userAuthPair, (jsonWebToken) =>
            {
                m_Client.SetJsonWebToken(jsonWebToken);

                Debug.Log($"SUCCESS! JWT AccessToken:{jsonWebToken.AccessToken}, Expires:{jsonWebToken.ExpiresInSecs}");
            }, s =>
            {
                Debug.LogError("Failed");
            });
        }

        GUILayout.EndVertical();

        // Console
        GUILayout.BeginVertical();
        GUILayout.TextArea(m_HttpResponse);

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }
}
