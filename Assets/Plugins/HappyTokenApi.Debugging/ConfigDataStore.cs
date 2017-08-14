using HappyTokenApi.Models;
using System.Collections.Generic;

namespace HappyTokenApi.Debugging
{
    public class ConfigDataStore
    {
        public AppDefaults AppDefaults { get; set; }

        public List<Avatar> Avatars { get; set; }

        public List<Building> Buildings { get; set; }

        public List<Cake> Cakes { get; set; }

        public Store Store { get; set; }
    }
}
