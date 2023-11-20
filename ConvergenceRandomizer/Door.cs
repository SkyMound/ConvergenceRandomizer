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
        public bool IsNoReturn;
        public List<Route<string>> ToDoors;
        public Route<Ability> ToAbility;
        public List<Route<Part>> ToParts;
        public Route<string> ToBench;
    }
}
