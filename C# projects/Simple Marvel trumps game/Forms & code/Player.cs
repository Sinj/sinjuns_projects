using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S.Dassiemnt2
{
   public class Player
    {
       private int[] hand = new int[10];
        public int[] cardHand
        {
            get { return hand; }
            set { hand = value; }
        }
}
}
