using HappyTokenApi.Models;
using UnityEngine;

namespace HappyTokenApi.Debugging
{
    public class ConfigWindow : DebugWindow
    {
        public ConfigWindow(int id, string title) : base(id, title) { }

        public override void Draw()
        {
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            if (ApiDebugger.Instance.IsConfigDataLoaded)
            {
                DrawAppDefaults();

                DrawAvatars();

                DrawBuildings();

                DrawCakes();

                DrawPromotions();
            }
            else
            {
                GUILayout.Label("Config data is not loaded. Please Authenticate and Get Config.");
            }

            GUILayout.EndHorizontal();
        }

        private void DrawAvatars()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("Avatars");

            var avatars = ApiDebugger.Instance.ConfigDataStore.Avatars;

            if (avatars != null)
            {
                foreach (var avatar in avatars)
                {
                    GUILayout.BeginVertical(GUIContent.none, "box");
                    GUILayout.Label($"  Avatar:{avatar.AvatarType}");
                    GUILayout.Label($"  HappinessType:{avatar.HappinessType}");
                    GUILayout.Label($"  LevelsMax:{avatar.Levels.Count}");
                    GUILayout.Label($"  Rarity:{avatar.RarityType}");
                    GUILayout.EndVertical();
                }
            }

            GUILayout.EndVertical();
        }

        private void DrawBuildings()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("Buildings");

            var buildings = ApiDebugger.Instance.ConfigDataStore.Buildings;

            if (buildings != null)
            {
                foreach (var building in buildings)
                {
                    GUILayout.BeginVertical(GUIContent.none, "box");
                    GUILayout.Label($"  Building:{building.BuildingType}");
                    GUILayout.Label($"  HappinessType:{building.HappinessType}");
                    GUILayout.Label($"  LevelsMax:{building.Levels.Count}");
                    GUILayout.Label($"  Rarity:{building.RarityType}");
                    GUILayout.Label($"  Avatar:{building.AvatarType}");
                    GUILayout.Label($"  AvatarAlt:{building.AvatarTypeAlt}");
                    GUILayout.EndVertical();
                }
            }

            GUILayout.EndVertical();
        }

        private void DrawPromotions()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("Promotions");

            var promotions = ApiDebugger.Instance.ConfigDataStore.Promotions;

            if (promotions != null)
            {
                foreach (var promotion in promotions)
                {
                    GUILayout.BeginVertical(GUIContent.none, "box");
                    GUILayout.Label($"  Code:{promotion.Code}");
                    GUILayout.Label($"  Start:{promotion.StartDate}");
                    GUILayout.Label($"  End:{promotion.EndDate}");
                    GUILayout.Label($"  Gems:{promotion.Gems}");
                    GUILayout.Label($"  Gems:{promotion.Gems}");
                    GUILayout.Label($"  HappyTokens:{promotion.HappyTokens}");
                    GUILayout.Label($"  Price:{promotion.Price}");
                    GUILayout.EndVertical();
                }
            }

            GUILayout.EndVertical();
        }

        private void DrawCakes()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("Cakes");

            var cakes = ApiDebugger.Instance.ConfigDataStore.Cakes;

            if (cakes != null)
            {
                foreach (var cake in cakes)
                {
                    GUILayout.BeginVertical(GUIContent.none, "box");
                    GUILayout.Label($"  Avatar:{cake.CakeType}");
                    GUILayout.Label($"  BakeTimeMins:{cake.BakeTimeMins}");
                    GUILayout.Label($"  Gems:{cake.Gems}");
                    GUILayout.Label($"  Gold:{cake.Gold}");
                    GUILayout.Label($"  Value:{cake.Value}");
                    GUILayout.EndVertical();
                }
            }

            GUILayout.EndVertical();
        }

        private void DrawAppDefaults()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("App Defaults");

            var appDefaults = ApiDebugger.Instance.ConfigDataStore.AppDefaults;

            if (appDefaults != null)
            {
                GUILayout.Label($"  Friends Max Count:{appDefaults.FriendsMaxCount}");
                GUILayout.Label($"  Message Max Chars:{appDefaults.MessageMaxChars}");
                GUILayout.Label($"  Name Max Chars:{appDefaults.NameMaxChars}");
            }

            GUILayout.EndVertical();
        }
    }
}
