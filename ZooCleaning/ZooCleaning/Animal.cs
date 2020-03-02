using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZooCleaning
{
    enum Priority
    {
        Highest,
        High,
        Medium,
        Low
    }
    abstract class Animal
    {
        public string Name { get; private set; }
        public decimal ShitsPerDay { get; private set; }
        public double TimeToRemoveShit { get; private set; }
        public Priority ShitPriority { get; set; }

        //private int oneHour = 1000;

        public Animal(string name, decimal shitsPerDay, double timeToRemoveShit, Priority shitPriority)
        {
            Name = name;
            ShitsPerDay = shitsPerDay;
            TimeToRemoveShit = timeToRemoveShit;
            ShitPriority = shitPriority;
        }

        public virtual AnimalShit TakeShit()
        {
            Printer.Print($"{Name} took a shit!");
            return new AnimalShit(this);
        }


    }
}
