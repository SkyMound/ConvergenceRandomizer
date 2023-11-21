using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvergenceRandomizer
{
    class Route<T>
    {
        public T LeadsTo;
        public List<Combination> Combinations;

        public bool CanBeDone(PlayerState player)
        {
            foreach (Combination combination in this.Combinations)
            {
                if (combination.Difficulty <= player.Difficulty && combination.AbilitiesRequired.All(abilityRequired => player.Abilities.Contains(abilityRequired)))
                {
                    return true;
                }

            }

            return false;
        }
    }
}
