using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvergenceRandomizer
{
    class Helper
    {
        private static Random rng = new Random();

        public static string FirstDoorName = "P1L1_TUT_Gameplay_RoomDoor_Edge_ToEntresol";
        public static string LastDoorName = "P7L1C3_CAR_Gameplay_RoomDoor_Edge_ToC2";

        public static List<T> Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        public static HashSet<string> GetAllRooms(List<Door> allDoors)
        {
            return new HashSet<string>(allDoors.ConvertAll(door => door.Name));
        }

        

        public static HashSet<string> GetReachableDoorsNameNotMapped(Door door, PlayerState player, Dictionary<string,string> transitionMap, Dictionary<string,Door> doorsData,bool isFirstIteration = true)
        {
            HashSet<string> reachableDoors = new HashSet<string>();

            if (isFirstIteration)
            {
                HashSet<string> reachableDirectDoors = door.GetDirectReachableDoorsName(player);
                foreach(string directDoorName in reachableDirectDoors)
                {
                    if (doorsData.TryGetValue(directDoorName, out Door directDoor))
                        reachableDoors.UnionWith(GetReachableDoorsNameNotMapped(directDoor, player, transitionMap, doorsData, false));
                }
            }

            if(transitionMap.TryGetValue(door.Name,out string transitionDoorName))
            {
                if(doorsData.TryGetValue(transitionDoorName,out Door transitionDoor))
                {
                    HashSet<string> reachableDirectDoors = transitionDoor.GetDirectReachableDoorsName(player);
                    foreach (string directDoorName in reachableDirectDoors)
                    {
                        if (doorsData.TryGetValue(directDoorName, out Door directDoor))
                            reachableDoors.UnionWith(GetReachableDoorsNameNotMapped(directDoor, player, transitionMap, doorsData, false));
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
