using HappyTokenApi.Models;
using UnityEngine;

namespace HappyTokenApi.Debugging
{
    public class FriendWindow : DebugWindow
    {
        private bool m_IsBusy;

        private string m_AddFriendUserId;

        public FriendWindow(int id, string title) : base(id, title) { }

        public override void Draw()
        {
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            if (ApiDebugger.Instance.IsUserDataLoaded)
            {
                GUI.enabled = !m_IsBusy;

                DrawButtons();

                DrawFriends();

                DrawSuggestedFriends();

                GUI.enabled = true;
            }
            else
            {
                GUILayout.Label("User data is not loaded. Please Authenticate and Get User.");
            }

            GUILayout.EndHorizontal();
        }

        private void DrawButtons()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("Actions");

            if (GUILayout.Button("Get Suggested"))
            {
                GetSuggestedFriends();
            }

            if (GUILayout.Button("Get Friends"))
            {
                GetFriends();
            }

            m_AddFriendUserId = GUILayout.TextField(m_AddFriendUserId);

            if (GUILayout.Button("Add Friend"))
            {
                AddFriendByUserId(m_AddFriendUserId);
            }

            GUILayout.EndVertical();
        }

        private void GetSuggestedFriends()
        {
            m_IsBusy = true;

            ApiDebugger.Instance.WebRequest.GetSuggestedFriends(friends =>
            {
                ApiDebugger.Instance.CoreDataStore.SuggestedFriends = friends;

                Debug.Log("GetSuggestedFriends: Success!");
                m_IsBusy = false;
            }, s =>
            {
                Debug.LogError("GetSuggestedFriends: Failed");
                m_IsBusy = false;
            });
        }

        private void GetFriends()
        {
            m_IsBusy = true;

            ApiDebugger.Instance.WebRequest.GetFriends(friends =>
            {
                ApiDebugger.Instance.CoreDataStore.Friends = friends;

                Debug.Log("GetFriends: Success!");
                m_IsBusy = false;
            }, s =>
            {
                Debug.LogError("GetFriends: Failed");
                m_IsBusy = false;
            });
        }

        private void AddFriendByUserId(string friendUserId)
        {
            m_IsBusy = true;

            ApiDebugger.Instance.WebRequest.AddFriend(friendUserId, friends =>
            {
                ApiDebugger.Instance.CoreDataStore.Friends = friends;

                Debug.Log("AddFriendByUserId: Success!");
                m_IsBusy = false;
            }, s =>
            {
                Debug.LogError("AddFriendByUserId: Failed");
                m_IsBusy = false;
            });
        }

        private void DrawFriends()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("Friends");

            var items = ApiDebugger.Instance.CoreDataStore.Friends;

            if (items != null)
            {
                foreach (var item in items)
                {
                    GUILayout.Label($"Name:{item.Name}, Level:{item.Level}, Last Seen:{item.LastSeenDate}");
                }
            }

            GUILayout.EndVertical();
        }

        private void DrawSuggestedFriends()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("Suggested Friends");

            var items = ApiDebugger.Instance.CoreDataStore.SuggestedFriends;

            if (items != null)
            {
                foreach (var item in items)
                {
                    GUILayout.Label($"Name:{item.Name}, Level:{item.Level}, Last Seen:{item.LastSeenDate}");

                    if (GUILayout.Button("Add"))
                    {
                        AddFriendByUserId(item.FriendUserId);
                    }
                }
            }

            GUILayout.EndVertical();
        }
    }
}
