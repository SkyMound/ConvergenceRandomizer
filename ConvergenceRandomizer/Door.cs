using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvergenceRandomizer
{
    class Door
    {
        public string Name;
        public string RoomName;
        private bool isNoReturn;
        public List<Route<string>> ToDoors;
        public Route<Ability> ToAbility;
        public List<Route<Part>> ToParts;
        public Route<string> ToBench;

        public override bool Equals(object obj)
        {
            Door otherDoor = (Door)obj;
            return this.Name.Equals(otherDoor.Name);
        }

        public bool IsNoReturn(PlayerState player, Dictionary<string, Door> doorsData)
        {
            if (isNoReturn)
            {
                // Check no backward
                return true;
            }
            else
            {
                return false;
            }
        }

        public HashSet<string> GetDirectReachableDoorsName(PlayerState player)
        {
            HashSet<string> directReachableDoors = new HashSet<string>();

            foreach (Route<string> routeToDirectDoors in this.ToDoors)
            {
                if (routeToDirectDoors.CanBeDone(player))
                {
                    directReachableDoors.Add(routeToDirectDoors.LeadsTo);
                }
            }

            return directReachableDoors;
        }
    }
}
