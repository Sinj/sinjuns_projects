using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace S.Dassiemnt2
{
   public class Card
    {

       public string Name;
       public int Ccard;
        public int Prop1;
        public int Prop2;
        public int Prop3;
        public int Prop4;
        public Image CardImageLoc;
        public string Info;
        public Card(string card, int cnum, int Pr1, int Pr2, int Pr3, int Pr4, Image loc, string fo)
        {
            Name = card;
            Ccard = cnum;
            Prop1 = Pr1;
            Prop2 = Pr2;
            Prop3 = Pr3;
            Prop4 = Pr4;
            CardImageLoc = loc;
                Info = fo;
        }
    }
}
