using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vid
{
    class CooString
    {
        public String Coordinates_to_string(Coordinates n)
        {
            string text = "ball position: x " + n.x + ", y " + n.y + Environment.NewLine; //dvi eilut4s tekstui
            return text;
        }
    }
}
