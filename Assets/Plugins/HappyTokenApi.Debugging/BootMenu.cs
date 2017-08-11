using UnityEngine;
using Newtonsoft.Json;
using System;

namespace HappyTokenApi.Debugging
{

    public class BootMenu : MonoBehaviour
    {
        //private string m_Title;

        //private Vector2 m_WindowSize;
        //private Vector2 m_WindowPosition;
        //private Vector2 m_ScrollPosition;

        //private int m_SelectedAppConfig;
        //private string m_SelectedAppConfigJson;
        //private string[] m_AppConfigs;
        //private bool m_InvalidAppConfigJson;

        //private RegionToken m_CustomPhotonRegion;
        //private string m_CustomGameServerAddress;

        //private int m_SelectedRegion;
        //private string[] m_PhotonRegions;

        //private void OnEnable()
        //{
        //    m_Title = string.Format("Boot Menu v{0} ({1}) Rev:{2}", Settings.Global.Version, Settings.Global.BuildNumber, Settings.Global.GitRevision);
        //    m_SelectedAppConfigJson = Settings.CustomAppConfigContainers[0].AppConfigJson;
        //    m_CustomPhotonRegion = Settings.CustomPhotonRegion;
        //    m_CustomGameServerAddress = Settings.CustomGameServerAddress;

        //    m_AppConfigs = new string[Settings.CustomAppConfigContainers.Length];
        //    for (int i = 0; i < m_AppConfigs.Length; i++)
        //    {
        //        m_AppConfigs[i] = Settings.CustomAppConfigContainers[i].name;
        //    }

        //    m_PhotonRegions = Enum.GetNames(typeof(RegionToken));
        //}

        //private void Update()
        //{
        //    m_WindowSize = new Vector2(Mathf.RoundToInt(Screen.width * 0.75f), Mathf.RoundToInt(Screen.height * 0.75f));
        //    m_WindowPosition = new Vector2(Mathf.RoundToInt((Screen.width * 0.5f) - (m_WindowSize.x * 0.5f)), Mathf.RoundToInt((Screen.height * 0.5f) - (m_WindowSize.y * 0.5f)));
        //}

        //private void OnGUI()
        //{
        //    GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none);
        //    GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none);

        //    GUILayout.BeginArea(new Rect(m_WindowPosition, m_WindowSize), m_Title, "window");

        //    m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);

        //    GUILayout.Label("Select a Version Status");

        //    DrawVersionStatus();

        //    GUILayout.Label("Add any custom overrides you need.");

        //    DrawCustomFields();

        //    if (GUILayout.Button("OK"))
        //    {
        //        AppConfig testConfig = JsonConvert.DeserializeObject<AppConfig>(m_SelectedAppConfigJson);

        //        m_InvalidAppConfigJson = testConfig == null;

        //        if (!m_InvalidAppConfigJson)
        //        {
        //            AppConfig appConfig = Settings.CustomAppConfigContainers[m_SelectedAppConfig].GetAsAppConfig();

        //            Settings.SetCustomGameServerAddress(m_CustomGameServerAddress);
        //            Settings.SetCustomPhotonRegion(m_CustomPhotonRegion);

        //            CloseBootMenu(appConfig);
        //        }
        //    }

        //    if (m_InvalidAppConfigJson)
        //    {
        //        GUI.color = Color.red;
        //        GUILayout.Label("Invalid config Json. Cannot proceed with authentication.");
        //        GUI.color = Color.white;

        //        GUILayout.BeginHorizontal();

        //        if (GUILayout.Button("Reset to Default"))
        //        {
        //            m_SelectedAppConfigJson = Settings.CustomAppConfigContainers[m_SelectedAppConfig].AppConfigJson;
        //            m_InvalidAppConfigJson = false;
        //        }

        //        if (GUILayout.Button("Clear"))
        //        {
        //            m_InvalidAppConfigJson = false;
        //        }

        //        GUILayout.EndHorizontal();
        //    }

        //    GUILayout.EndScrollView();

        //    GUILayout.EndArea();
        //}

        //private void DrawVersionStatus()
        //{
        //    m_SelectedAppConfig = GUILayout.SelectionGrid(m_SelectedAppConfig, m_AppConfigs, 3);

        //    if (GUI.changed)
        //    {
        //        m_SelectedAppConfigJson = Settings.CustomAppConfigContainers[m_SelectedAppConfig].AppConfigJson;
        //        m_InvalidAppConfigJson = false;
        //    }

        //    m_SelectedAppConfigJson = GUILayout.TextArea(m_SelectedAppConfigJson);

        //    GUILayout.Label("Note. Edit the above text to customize.", "box");
        //}

        //private void DrawCustomFields()
        //{
        //    GUILayout.BeginHorizontal();
        //    GUILayout.Label("Custom Photon Region", GUILayout.MaxWidth(200));
        //    m_SelectedRegion = GUILayout.SelectionGrid(m_SelectedRegion, m_PhotonRegions, 6);

        //    if (GUI.changed)
        //    {
        //        m_CustomPhotonRegion = (RegionToken)m_SelectedRegion;
        //    }

        //    GUILayout.EndHorizontal();

        //    GUILayout.BeginHorizontal();
        //    GUILayout.Label("Custom Game Server IP", GUILayout.MaxWidth(200));
        //    m_CustomGameServerAddress = GUILayout.TextField(m_CustomGameServerAddress);
        //    GUILayout.EndHorizontal();
        //}

        //private void CloseBootMenu(AppConfig appConfig)
        //{
        //    AuthenticationManager.Instance.Initialize(appConfig);

        //    Destroy(this);
        //}
    }
}
