using HappyTokenApi.Models;
using UnityEngine;

namespace HappyTokenApi.Debugging
{
    public class DashboardWindow : DebugWindow
    {
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
            GUILayout.BeginVertical();

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
