using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvergenceRandomizer
{
    class DefaultAlgorithm : Algorithm
    {
        public override Dictionary<string, string> ComputeTransitionMap(Dictionary<string, Door> doorsData)
        {
            Dictionary<string, string> transitionMap = new Dictionary<string, string>();
            List<Door> allRemainingDoors = Helper.Shuffle(doorsData.Values.ToList());
            HashSet<string> allRemainingRooms = Helper.GetAllRooms(allRemainingDoors);

            PlayerState playerState = new PlayerState();

            HashSet<string> reachableDoors = new HashSet<string>();
            reachableDoors.Add(Helper.FirstDoorName);
            Random random = new Random();

            while(allRemainingRooms.Count != 0)
            {
                string currentDoorNane = reachableDoors.ElementAt(random.Next(reachableDoors.Count));
                
                bool canBeNoReturn = false;
                Door nextDoor = FindNextDoor(playerState, allRemainingDoors, allRemainingRooms, canBeNoReturn, doorsData);
                if (nextDoor == null)
                    break;

                allRemainingDoors.Remove(nextDoor);
                allRemainingRooms.Remove(nextDoor.RoomName);
                transitionMap.Add(currentDoorNane, nextDoor.Name);

                playerState.UpdateAbilities(nextDoor, transitionMap, doorsData);

                reachableDoors = Helper.GetReachableDoorsNameNotMapped(nextDoor, playerState, transitionMap, doorsData);
                
            }

            string penultimateDoorName = reachableDoors.ElementAt(random.Next(reachableDoors.Count));

            transitionMap.Add(penultimateDoorName, Helper.LastDoorName);
            reachableDoors.Remove(penultimateDoorName);

            

            return transitionMap;
        }

        private Door FindNextDoor(PlayerState player, List<Door> allowedDoors, HashSet<string> allowedRooms, bool canBeNoReturn, Dictionary<string, Door> doorsData)
        {
            foreach(Door door in allowedDoors)
            {
                if(allowedRooms.Contains(door.RoomName))
                {
                    if(door.GetDirectReachableDoorsName(player).Count != 0 || door.ToAbility != null)
                    {
                        if (canBeNoReturn)
                        {
                            return door;
                        }
                        else if(!door.IsNoReturn(player, doorsData))
                        {
                            return door;
                        }
                    }
                }
            }
            return null;
        }
    }
}
