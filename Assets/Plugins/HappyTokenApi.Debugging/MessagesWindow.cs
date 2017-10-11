using System;
using HappyTokenApi.Models;
using UnityEngine;

namespace HappyTokenApi.Debugging
{
    public class MessagesWindow : DebugWindow
    {
        private bool m_IsSending;
        private string m_ToUserId;

        public MessagesWindow(int id, string title) : base(id, title) { }

        public override void Draw()
        {
            GUILayout.Space(10);

            if (GUILayout.Button("Get Messages"))
            {
                GetMessages();
            }

            GUILayout.BeginHorizontal();
            m_ToUserId = GUILayout.TextField(m_ToUserId);
            if (GUILayout.Button("Send Cake"))
            {
                SendCake(m_ToUserId, CakeType.Cookie);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            DrawMessages();

            GUILayout.EndHorizontal();
        }

        private void SendCake(string toUserId, CakeType cakeType)
        {
            if (m_IsSending)
            {
                return;
            }

            m_IsSending = true;

            var sendCakeMessage = new UserSendCakeMessage
            {
                CakeType = cakeType,
                ToUserId = toUserId,
            };

            ApiDebugger.Instance.WebRequest.SendCakeMessage(sendCakeMessage, (result) =>
            {
                Debug.Log("SendCakeMessage: Success!");
                m_IsSending = false;
            }, s =>
            {
                Debug.LogError("SendCakeMessage: Failed");
                m_IsSending = false;
            });
        }

        private void GetMessages()
        {
            if (m_IsSending)
            {
                return;
            }

            m_IsSending = true;

            ApiDebugger.Instance.WebRequest.GetMessages((userMessages) =>
            {
                ApiDebugger.Instance.CoreDataStore.Messages = userMessages;

                Debug.Log("GetMessages: Success!");
                m_IsSending = false;
            }, s =>
            {
                Debug.LogError("GetMessages: Failed");
                m_IsSending = false;
            });
        }

        private void DeleteMessage(string userMessageId)
        {
            if (m_IsSending)
            {
                return;
            }

            m_IsSending = true;

            ApiDebugger.Instance.WebRequest.DeleteMessage(userMessageId, (userMessages) =>
            {
                ApiDebugger.Instance.CoreDataStore.Messages = userMessages;

                Debug.Log("DeleteMessage: Success!");
                m_IsSending = false;
            }, s =>
            {
                Debug.LogError("DeleteMessage: Failed");
                m_IsSending = false;
            });
        }

        private void ClaimCake(string userMessageId)
        {
            if (m_IsSending)
            {
                return;
            }

            m_IsSending = true;

            ApiDebugger.Instance.WebRequest.ClaimCakeMessage(userMessageId, (userCakes) =>
            {
                ApiDebugger.Instance.CoreDataStore.Cakes = userCakes;

                Debug.Log("ClaimCake: Success!");
                m_IsSending = false;
            }, s =>
            {
                Debug.LogError("ClaimCake: Failed");
                m_IsSending = false;
            });
        }

        private void DrawMessages()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("Messages");

            var messages = ApiDebugger.Instance.CoreDataStore.Messages;

            if (messages != null)
            {
                foreach (var message in messages)
                {
                    GUILayout.BeginVertical(GUIContent.none, "box");

                    var duration = message.ExpiryDate - DateTime.UtcNow;
                    GUILayout.Label($"UserMessageId:{message.UsersMessageId}");
                    GUILayout.Label($"From:{message.FromUserId}, To:{message.FromUserId}, Expires:{duration.TotalDays:N}days");
                    GUILayout.Label($"Subject:{message.Subject}");
                    GUILayout.Label($"Subject:{message.Preview}");
                    GUILayout.Label($"Subject:{message.Body}");

                    switch (message.MessageType)
                    {
                        case MessageType.Cake:
                            GUILayout.BeginHorizontal(GUIContent.none, "box");
                            GUILayout.Label($"Pinky Cakes:{message.PinkyCakes}");
                            if (GUILayout.Button("Claim"))
                            {
                                ClaimCake(message.UsersMessageId);
                            }
                            GUILayout.EndHorizontal();
                            break;
                    }


                    if (GUILayout.Button("Delete"))
                    {
                        DeleteMessage(message.UsersMessageId);
                    }

                    GUILayout.EndVertical();
                }
            }

            GUILayout.EndVertical();
        }
    }
}
