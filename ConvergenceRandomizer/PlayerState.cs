using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvergenceRandomizer
{
    class PlayerState
    {
        public HashSet<Ability?> Abilities;
        public List<Part> Parts;
        public Ability LastAbilityObtained;
        public Difficulty Difficulty = Difficulty.Godlike;

        public int GetPartNumber(Color color)
        {
            return Parts.Count(part => part.Color == color);
        }

        public void UpdateAbilities(Door door, Dictionary<string, string> transitionMap, Dictionary<string, Door> doorsData)
        {
            if (door.ToAbility != null && door.ToAbility.CanBeDone(this))
            {
                Abilities.Add(door.ToAbility.LeadsTo);
            }
            if(!(Abilities.Contains(Ability.NitroModule)))
                TryCraftNitro(door, transitionMap, doorsData);
        }

        public void TryCraftNitro(Door door, Dictionary<string, string> transitionMap, Dictionary<string, Door> doorsData)
        {
            Door lastDoorObtainedPart = UpdateGadget(door, transitionMap, doorsData);
            int currentGreenParts = GetPartNumber(Color.Green);

            if (currentGreenParts >= 2)
            {
                if (CanReachBench(lastDoorObtainedPart ?? door, transitionMap, doorsData))
                    Abilities.Add(Ability.NitroModule);
            }
        }

        public Door UpdateGadget(Door door, Dictionary<string, string> transitionMap, Dictionary<string, Door> doorsData, bool isFirstIteration = true)
        {
            Door doorHavingGadget = null;
            foreach(Route<Part> toPart in door.ToParts)
            {
                if (!Parts.Contains(toPart.LeadsTo) && toPart.CanBeDone(this))
                {
                    Parts.Add(toPart.LeadsTo);
                    doorHavingGadget = door;
                }
            }

            HashSet<string> reachableDirectDoors = door.GetDirectReachableDoorsName(this);
            if (isFirstIteration)
                reachableDirectDoors.Add(door.Name);

            foreach (string directDoorName in reachableDirectDoors)
            {
                if (transitionMap.TryGetValue(directDoorName,out string transitionDoorName) && doorsData.TryGetValue(transitionDoorName,out Door transitionDoor))
                {
                    Door newDoor = UpdateGadget(transitionDoor, transitionMap, doorsData, false);
                    if (!(newDoor is null))
                        doorHavingGadget = newDoor;
                }
            }

            return doorHavingGadget;
        }

        public bool CanReachBench(Door door, Dictionary<string, string> transitionMap, Dictionary<string, Door> doorsData, bool isFirstIteration = true)
        {
            if(!(door.ToBench is null) && door.ToBench.CanBeDone(this)){
                return true;
            }

            HashSet<string> reachableDirectDoors = door.GetDirectReachableDoorsName(this);
            if (isFirstIteration)
                reachableDirectDoors.Add(door.Name);

            bool canReachBench = false;
            foreach (string directDoorName in reachableDirectDoors)
            {
                if (transitionMap.TryGetValue(directDoorName, out string transitionDoorName) && doorsData.TryGetValue(transitionDoorName, out Door transitionDoor))
                {
                    canReachBench |= CanReachBench(transitionDoor, transitionMap, doorsData, false);
                }
            }
            return canReachBench;
        }
    }
}
