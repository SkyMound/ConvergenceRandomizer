using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvergenceRandomizer
{
    public class Part
    {
        public string Name;
        public Color Color;

        public override bool Equals(object obj)
        {
            Part otherRoute = (Part)obj;
            return this.Name.Equals(otherRoute.Name);
        }
    }
    public enum Color
    {
        Green = 0,
        Yellow = 1,
        Red = 2
    }

    

}
