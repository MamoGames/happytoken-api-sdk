using HappyTokenApi.Models;
using UnityEngine;

namespace HappyTokenApi.Debugging
{
    public class UserWindow : DebugWindow
    {
        public UserWindow(int id, string title) : base(id, title) { }

        public override void Draw()
        {
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            if (ApiDebugger.Instance.IsUserDataLoaded)
            {

                DrawProfile();

                DrawAvatars();

                DrawBuildings();

                DrawCakes();
            }
            else
            {
                GUILayout.Label("User data is not loaded. Please Authenticate and Get User.");
            }

            GUILayout.EndHorizontal();
        }

        private void DrawAvatars()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("Avatars");

            var avatars = ApiDebugger.Instance.CoreDataStore.Avatars;

            if (avatars != null)
            {
                foreach (var avatar in avatars)
                {
                    GUILayout.Label($"  Avatar:{avatar.AvatarType}, Level:{avatar.Level}, Pieces:{avatar.Pieces}");
                }
            }

            GUILayout.EndVertical();
        }

        private void DrawBuildings()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("Buildings");

            var buildings = ApiDebugger.Instance.CoreDataStore.Buildings;

            if (buildings != null)
            {
                foreach (var building in buildings)
                {
                    GUILayout.Label($"  Building:{building.BuildingType}, Level:{building.Level}");
                }
            }

            GUILayout.EndVertical();
        }

        private void DrawCakes()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("Cakes");

            var cakes = ApiDebugger.Instance.CoreDataStore.Cakes;

            if (cakes != null)
            {
                foreach (var cake in cakes)
                {
                    GUILayout.Label($"  Cake:{cake.CakeType}, IsBaked:{cake.IsBaked}, BakeDate:{cake.BakedDate}");
                }
            }

            GUILayout.EndVertical();
        }

        private void DrawProfile()
        {
            GUILayout.BeginVertical(GUIContent.none, "box");

            GUILayout.Label("Profile");

            var profile = ApiDebugger.Instance.CoreDataStore.Profile;

            if (profile != null)
            {
                GUILayout.Label($"  Name:{profile.Name}");
                GUILayout.Label($"  CreateDate:{profile.CreateDate}");
                GUILayout.Label($"  LastSeenDate:{profile.LastSeenDate}");
                GUILayout.Label($"  Xp:{profile.Xp}");
            }

            GUILayout.Label("Wallet");

            var wallet = ApiDebugger.Instance.CoreDataStore.Wallet;

            if (wallet != null)
            {
                GUILayout.Label($"  Gems:{wallet.Gems}");
                GUILayout.Label($"  Gold:{wallet.Gold}");
                GUILayout.Label($"  HappyTokens:{wallet.HappyTokens}");
            }

            GUILayout.Label("Happiness");

            var happiness = ApiDebugger.Instance.CoreDataStore.Happiness;

            if (happiness != null)
            {
                GUILayout.Label($"  Experience:{happiness.Experience}");
                GUILayout.Label($"  Health:{happiness.Health}");
                GUILayout.Label($"  Skill:{happiness.Skill}");
                GUILayout.Label($"  Social:{happiness.Social}");
                GUILayout.Label($"  Wealth:{happiness.Wealth}");
                GUILayout.Label($"  Total:{happiness.Total}");
            }

            GUILayout.EndVertical();
        }
    }
}
