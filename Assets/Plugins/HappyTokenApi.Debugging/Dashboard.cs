using HappyTokenApi.Models;
using UnityEngine;

namespace HappyTokenApi.Debugging
{
    public class DashboardWindow : DebugWindow
    {
        private bool m_UseSaved;
        private string m_Email;
        private string m_Password;

        public DashboardWindow(int id, string title) : base(id, title) { }

        public override void Draw()
        {
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            DrawRequestList();

            GUILayout.Space(16);

            DrawMiscControlsList();

            GUILayout.EndHorizontal();
        }

        private void DrawMiscControlsList()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.BeginHorizontal();
            GUILayout.Label("ApiUrl", GUILayout.MaxWidth(128));
            GUILayout.TextField(ApiDebugger.Instance.ApiUrl);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("DeviceId", GUILayout.MaxWidth(128));
            GUILayout.TextField(ApiDebugger.Instance.DeviceId);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("UserId", GUILayout.MaxWidth(128));
            GUILayout.TextField(ApiDebugger.Instance.UserAuthPair.UserId);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("AuthToken", GUILayout.MaxWidth(128));
            GUILayout.TextField(ApiDebugger.Instance.UserAuthPair.AuthToken);
            GUILayout.EndHorizontal();

            DrawEmail();

            GUILayout.EndVertical();
        }

        private void DrawEmail()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Email", GUILayout.MaxWidth(128));
            if (m_UseSaved)
            {
                GUILayout.Label(ApiDebugger.Instance.UserEmailLogin.Email);
            }
            else
            {
                m_Email = GUILayout.TextField(m_Email);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Password", GUILayout.MaxWidth(128));
            if (m_UseSaved)
            {
                GUILayout.Label(ApiDebugger.Instance.UserEmailLogin.Password);
            }
            else
            {
                m_Password = GUILayout.TextField(m_Password);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (ApiDebugger.Instance.IsAuthenticated)
            {
                m_UseSaved = false;

                if (GUILayout.Button("Update Email"))
                {
                    var userEmailLogin = new UserEmailLogin
                    {
                        Email = m_Email,
                        Password = ApiDebugger.Instance.HashPassword(m_Password)
                    };

                    var userId = ApiDebugger.Instance.UserAuthPair.UserId;

                    ApiDebugger.Instance.WebRequest.UpdateEmail(userId, userEmailLogin, result =>
                    {
                        Debug.Log($"ApiDebugger.UpdateEmail: Successfully updated Email {userEmailLogin.Email} & Password {userEmailLogin.Password} Result:{result.Content}");
                    }, s =>
                    {
                        Debug.LogError("ApiDebugger.UpdateEmail: Failed");
                    });
                }
            }
            else
            {
                m_UseSaved = GUILayout.Toggle(m_UseSaved, "Use Saved Email");

                if (GUILayout.Button("Login"))
                {
                    // By default, use the saved email/password
                    var userEmailLogin = ApiDebugger.Instance.UserEmailLogin;

                    // Otherwise, overwrite with the above email/password (and hash)
                    if (!m_UseSaved)
                    {
                        userEmailLogin = new UserEmailLogin
                        {
                            Email = m_Email,
                            Password = ApiDebugger.Instance.HashPassword(m_Password)
                        };
                    }

                    ApiDebugger.Instance.WebRequest.LoginByEmail(userEmailLogin, (userAuthPair) =>
                    {
                        ApiDebugger.Instance.SaveUserEmailLogin(userEmailLogin);
                        ApiDebugger.Instance.SetUserAuthPair(userAuthPair, true);
                    }, s =>
                    {
                        Debug.LogError("ApiDebugger.LoginByEmail: Failed");
                    });
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void DrawRequestList()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("API Requests");

            GUI.enabled = !ApiDebugger.Instance.IsAuthenticated;

            if (GUILayout.Button("Authenticate"))
            {
                var userAuthPair = ApiDebugger.Instance.UserAuthPair;
                ApiDebugger.Instance.WebRequest.Authenticate(userAuthPair, (jsonWebToken) =>
                {
                    ApiDebugger.Instance.WebRequest.SetJsonWebToken(jsonWebToken);
                    ApiDebugger.Instance.SetAuthenticated(true);

                }, s =>
                {
                    Debug.LogError("ApiDebugger.Authenticate: Failed");
                });
            }

            GUI.enabled = !ApiDebugger.Instance.IsUserDataLoaded;

            if (GUILayout.Button("Create User"))
            {
                var userDevice = new UserDevice
                {
                    DeviceId = ApiDebugger.Instance.DeviceId
                };

                ApiDebugger.Instance.WebRequest.CreateUser(userDevice, (userAuthPair) =>
                {
                    ApiDebugger.Instance.SetUserAuthPair(userAuthPair, saveToPlayerPrefs: true);

                    Debug.Log($"UserAuthPair UserId:{userAuthPair.UserId}, AuthToken:{userAuthPair.AuthToken}");
                }, s =>
                {
                    Debug.LogError("ApiDebugger.CreateUser: Failed");
                });
            }

            GUI.enabled = ApiDebugger.Instance.IsAuthenticated;

            if (GUILayout.Button("Get AppConfig"))
            {
                ApiDebugger.Instance.WebRequest.GetAppConfig((appConfig) =>
                {
                    ApiDebugger.Instance.ConfigDataStore.AppDefaults = appConfig.AppDefaults;
                    ApiDebugger.Instance.ConfigDataStore.Avatars = appConfig.Avatars;
                    ApiDebugger.Instance.ConfigDataStore.Buildings = appConfig.Buildings;
                    ApiDebugger.Instance.ConfigDataStore.Cakes = appConfig.Cakes;
                    ApiDebugger.Instance.ConfigDataStore.Store = appConfig.Store;
                    ApiDebugger.Instance.SetConfigDataLoaded(true);
                }, s =>
                {
                    Debug.LogError("ApiDebugger.GetAppConfig: Failed");
                });
            }

            if (GUILayout.Button("Get User"))
            {
                var userId = ApiDebugger.Instance.UserAuthPair.UserId;

                ApiDebugger.Instance.WebRequest.GetUser(userId, (userLogin) =>
                {
                    ApiDebugger.Instance.CoreDataStore.Profile = userLogin.Profile;
                    ApiDebugger.Instance.CoreDataStore.Wallet = userLogin.Wallet;
                    ApiDebugger.Instance.CoreDataStore.Happiness = userLogin.Happiness;
                    ApiDebugger.Instance.CoreDataStore.Avatars = userLogin.UserAvatars;
                    ApiDebugger.Instance.CoreDataStore.Buildings = userLogin.UserBuildings;
                    ApiDebugger.Instance.CoreDataStore.Cakes = userLogin.UserCakes;
                    ApiDebugger.Instance.CoreDataStore.DailyRewards = userLogin.DailyRewards;
                    ApiDebugger.Instance.SetUserDataLoaded(true);
                }, s =>
                {
                    Debug.LogError("ApiDebugger.GetUser: Failed");
                });
            }

            GUI.enabled = true;

            GUILayout.EndVertical();
        }
    }
}
