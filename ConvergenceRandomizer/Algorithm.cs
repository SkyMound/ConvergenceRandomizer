using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvergenceRandomizer
{
    abstract class Algorithm
    {
        public abstract Dictionary<string, string> ComputeTransitionMap(Dictionary<string, Door> doorsData);
    }
}
