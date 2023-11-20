using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvergenceRandomizer
{
    class Helper
    {
        public static HashSet<string> GetAllRooms(List<Door> allDoors)
        {
            HashSet<string> allRooms = new HashSet<string>();
            foreach(Door door in allDoors)
            {
                allRooms.Add(door.Name);
            }
            return allRooms;
        }

        public static bool CanDoRoute<T>(Route<T> route, PlayerState player, Difficulty difficulty = Difficulty.Godlike)
        {
            
            foreach(Combination combination in route.Combinations)
            {
                if(combination.Difficulty <= difficulty && combination.AbilitiesRequired.All(abilityRequired => player.Abilities.Contains(abilityRequired)))
                {
                    return true;
                }
                
            }

            return false;
        }

        public static HashSet<string> ComputeDirectReachableDoorsName(Door door, PlayerState player, Difficulty difficulty = Difficulty.Godlike)
        {
            HashSet<string> directReachableDoors = new HashSet<string>();

            foreach(Route<string> routeToDirectDoors in door.ToDoors)
            {
                if (CanDoRoute(routeToDirectDoors, player, difficulty)){
                    directReachableDoors.Add(routeToDirectDoors.LeadsTo);
                }
            }

            return directReachableDoors;
        }

        public static HashSet<string> ComputeReachableDoorsName(Door door, PlayerState player, Dictionary<string,string> transitionMap, Dictionary<string,Door> doorsData,bool isFirstIteration = true, Difficulty difficulty = Difficulty.Godlike)
        {
            HashSet<string> reachableDoors = new HashSet<string>();

            if (isFirstIteration)
            {
                HashSet<string> reachableDirectDoors = ComputeDirectReachableDoorsName(door, player, difficulty);
                foreach(string directDoorName in reachableDirectDoors)
                {
                    if (doorsData.TryGetValue(directDoorName, out Door directDoor))
                        reachableDoors.UnionWith(ComputeReachableDoorsName(directDoor, player, transitionMap, doorsData, false, difficulty));
                }
            }

            if(transitionMap.TryGetValue(door.Name,out string transitionDoorName))
            {
                if(doorsData.TryGetValue(transitionDoorName,out Door transitionDoor))
                {
                    HashSet<string> reachableDirectDoors = ComputeDirectReachableDoorsName(transitionDoor, player, difficulty);
                    foreach (string directDoorName in reachableDirectDoors)
                    {
                        if (doorsData.TryGetValue(directDoorName, out Door directDoor))
                            reachableDoors.UnionWith(ComputeReachableDoorsName(directDoor, player, transitionMap, doorsData, false, difficulty));
                    }
                }
            }
            else
            {
                reachableDoors.Add(door.Name);
            }

            return reachableDoors;
        }

    }
}
