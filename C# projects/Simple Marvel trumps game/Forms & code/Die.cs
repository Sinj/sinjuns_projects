using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S.Dassiemnt2
{
    class Die
    {
        int topNumber;
        Random rng;
        
        public Die(Random randomGenerator)
        {
            //initialise
            rng = randomGenerator;
        }

        public int roll()
        {
            //makes a random number
            topNumber = rng.Next(1, 7);

            //returns it to caller
            return topNumber;
        }
    }
}
