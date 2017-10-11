using HappyTokenApi.Models;
using System.Collections.Generic;

namespace HappyTokenApi.Debugging
{
    public class CoreDataStore
    {
        public Profile Profile { get; set; }
        public Wallet Wallet { get; set; }
        public Happiness Happiness { get; set; }
        public List<UserAvatar> Avatars { get; set; }
        public List<UserBuilding> Buildings { get; set; }
        public List<UserCake> Cakes { get; set; }
        public DailyRewards DailyRewards { get; set; }

        public List<FriendInfo> Friends { get; set; }
        public List<FriendInfo> SuggestedFriends { get; set; }
        public List<UserMessage> Messages { get; set; }
    }
}
