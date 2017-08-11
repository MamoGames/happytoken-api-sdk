using UnityEngine;

namespace HappyTokenApi.Debugging
{
    public class Console : MonoBehaviour
    {
        private Vector2 m_ScrollRect;

        private string[] m_DebugWindowTitles;
        private DebugWindow[] m_DebugWindows;
        private DebugWindow m_CurrentDebugWindow;

        private bool m_IsDebugModeEnabled = true;

        private void Start()
        {
            // Setup Debug Windows
            m_DebugWindows = new DebugWindow[]
            {
                new DashboardWindow(0, "Dashboard"),
                new UserWindow(1,"User"), 
            };

            // Setup window titles for GUI SelectionGrid
            m_DebugWindowTitles = new string[m_DebugWindows.Length];
            for (var i = 0; i < m_DebugWindows.Length; i++)
            {
                m_DebugWindowTitles[i] = m_DebugWindows[i].Title;
            }

            // Activate the first debug window
            m_CurrentDebugWindow = m_DebugWindows[0];
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.BackQuote))
            {
                m_IsDebugModeEnabled = !m_IsDebugModeEnabled;
            }
        }

        private void OnGUI()
        {
            if (!m_IsDebugModeEnabled)
            {
                return;
            }

            GUI.depth = -1;

            var rect = new Rect(20, 20, Screen.width - 40, Screen.height - 40);

            GUILayout.BeginArea(rect, "Debug Console", "window");

            DrawToolbar();

            GUILayout.Space(4);

            m_ScrollRect = GUILayout.BeginScrollView(m_ScrollRect);

            m_CurrentDebugWindow.Draw();

            GUILayout.EndScrollView();

            GUILayout.EndArea();
        }

        private void DrawToolbar()
        {
            var isAuthenticated = ApiDebugger.Instance.IsAuthenticated;

            GUILayout.BeginHorizontal();
            var text = isAuthenticated ? "[O]" : "[X]";
            GUI.color = isAuthenticated ? Color.green : Color.red;
            GUILayout.Label(text);
            GUI.color = Color.white;

            if (isAuthenticated)
            {
                var userName = ApiDebugger.Instance.CoreDataStore.Profile?.Name ?? "None.";
                var userId = ApiDebugger.Instance.UserAuthPair.UserId;

                GUILayout.Label($"Name:{userName} ({userId})");
            }
            else
            {
                GUILayout.Label("User not authenticated. Authenticate or Create User to access the API.");
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUIContent.none, "box");

            // Debug Windows Toolbar
            var id = GUILayout.SelectionGrid(m_CurrentDebugWindow.Id, m_DebugWindowTitles, m_DebugWindows.Length);

            if (GUI.changed)
            {
                m_CurrentDebugWindow = m_DebugWindows[id];
            }

            GUILayout.FlexibleSpace();

            // Close Button
            if (GUILayout.Button("Close"))
            {
                m_IsDebugModeEnabled = Debug.developerConsoleVisible = false;
            }

            GUILayout.EndHorizontal();
        }
    }
}
