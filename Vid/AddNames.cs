using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vid
{
    public class AddNames
    {
        public static String Ret_names(String first_name, String second_name)
        {
            String names;
            names = first_name + "^" + second_name;
            return names;
        }
    }
}
