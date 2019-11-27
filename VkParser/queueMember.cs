using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkParser
{
    public class queueMember
    {
        public int f1;
        public int f2;
        public int f3;

        public queueMember(int i1, int i2, int i3)
        {
            f1 = i1;
            f2 = i2;
            f3 = i3;
        }

        public bool Exist4()
        {
            return f1 == 4 || f2 == 4 || f3 == 4;
        }
    }

}
