using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S.Dassiemnt2
{
    class ChanceCard
    {
        
       private int[] chance = new int[16];
        public int[] Chancedeck
        {
            get { return chance; }
            set { chance = value; }
        }
    }
}
