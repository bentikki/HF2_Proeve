using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooCleaning
{
    class AnimalShit
    {
        public Animal ShitOwner { get; private set; }
        public Priority ShitPriority { get; private set; }
        public double TimeToRemoveShit { get; private set; }

        public AnimalShit(Animal animal)
        {
            ShitPriority = animal.ShitPriority;
            TimeToRemoveShit = animal.TimeToRemoveShit;
            ShitOwner = animal;
        }
    }
}
