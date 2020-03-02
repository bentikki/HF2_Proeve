using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZooCleaning
{
    class Zoo
    {
        private List<List<Animal>> AnimalsInZoo = new List<List<Animal>>();
        private List<Animal> elephantPlatform = new List<Animal>();
        private List<Animal> giraffePlatform = new List<Animal>();
        private List<Animal> foxPlatform = new List<Animal>();
        private List<Animal> wolfPlatform = new List<Animal>();
        private List<Animal> rabbitPlatform = new List<Animal>();
        public static Stack<AnimalShit> HobedShit = new Stack<AnimalShit>();
        private int oneHour = 6000;
        private int hourOfDay = 8;
        private int customerSatisfaction = 100;
        private List<AnimalShit> shitTakenInDay = new List<AnimalShit>();


        public Zoo()
        {

            for (int i = 0; i < 6; i++)
            {
                elephantPlatform.Add(new Elephant());
            }
            AnimalsInZoo.Add(elephantPlatform);
            for (int i = 0; i < 12; i++)
            {
                giraffePlatform.Add(new Giraffe());
            }
            AnimalsInZoo.Add(giraffePlatform);

            for (int i = 0; i < 20; i++)
            {
                foxPlatform.Add(new Fox());
            }
            AnimalsInZoo.Add(foxPlatform);

            for (int i = 0; i < 20; i++)
            {
                wolfPlatform.Add(new Wolf());
            }
            AnimalsInZoo.Add(wolfPlatform);

            for (int i = 0; i < 10; i++)
            {
                rabbitPlatform.Add(new Rabbit());
            }
            AnimalsInZoo.Add(rabbitPlatform);

            Life();
            
        }

        public void Life()
        {
            while (true)
            {
                Thread animalThread =  new Thread(HourPassedAnimal);
                animalThread.Start();

                List<Thread> employeeThreads = new List<Thread>();
                for (int i = 0; i < 2; i++)
                {
                    Thread employee = new Thread(HourPassedEmployee);
                    employee.Start();
                    employeeThreads.Add(employee);
                }
                PassHour();
                animalThread.Abort();
                foreach (Thread t in employeeThreads)
                {
                    t.Abort();
                }
            }            
        }

        private void HourPassedAnimal()
        {
            for (int i = 0; i < AnimalsInZoo.Count; i++)
            {
                for (int j = 0; j < AnimalsInZoo[i].Count; j++)
                {
                    Animal animal = AnimalsInZoo[i][j];
                    //Math.Ceiling (up) or Math.Floor
                    decimal shitsPerHour = (animal.ShitsPerDay / 24);
                    int shitMin = Convert.ToInt16(Math.Floor(shitsPerHour));
                    int shitMax = Convert.ToInt16(Math.Ceiling(shitsPerHour));
                    int hourlyShitCount = StaticRandom.Rand(shitMin, (shitMax + 1));

                    for (int k = 0; k < hourlyShitCount; k++)
                    {
                        AnimalShit shitTaken = animal.TakeShit();
                        lock (HobedShit)
                        {
                            HobedShit.Push(shitTaken);
                            Monitor.Pulse(HobedShit);
                        }
                    }
                }
            }
        }

        private void HourPassedEmployee()
        {
            int elapsedTime = 0;

            while (elapsedTime < oneHour)
            {
                lock (HobedShit)
                {
                    if (HobedShit.Count < 1)
                    {
                        Printer.Print("Hobed shit is empty, Employee is waiting.");
                        Monitor.Wait(HobedShit);
                    }
                    else
                    {
                        HobedShit.OrderBy(i => i.ShitPriority);
                        AnimalShit removedShit = HobedShit.Peek();
                        int timeToRemove = Convert.ToInt16(removedShit.TimeToRemoveShit * 100);
                        Printer.Print($"Started removing {removedShit.ShitOwner.Name} shit, taking {removedShit.TimeToRemoveShit} minutes ({timeToRemove} ms) Currently hobed shit[{HobedShit.Count}]");
                        HobedShit.Pop();
                        lock (shitTakenInDay)
                        {
                            shitTakenInDay.Add(removedShit);
                        }
                        
                        Thread.Sleep(timeToRemove);
                        elapsedTime += timeToRemove;
                        Printer.Print($"......Finished removing {removedShit.ShitOwner.Name} shit. Currently hobed shit[{HobedShit.Count}] elapsed time[{elapsedTime}]");
                    }
                    
                }
            }
            
        }

        private void PassHour()
        {
            Thread.Sleep(oneHour);
            Thread.Sleep(4000);
            if (hourOfDay == 24)
            {
                
                Thread.Sleep(1000);
                Printer.Print($".................Statistics per day.................");
                Printer.Print($".................[{ customerSatisfaction }] Customer satisfaction.................");
                Printer.Print($".................[{ shitTakenInDay.Count }] Shits removed by employees.................");
                Printer.Print($".................[{ shitTakenInDay.FindAll(i => i.ShitOwner is Elephant).Count }] Shits taken by Elephants.................");
                Printer.Print($".................[{ shitTakenInDay.FindAll(i => i.ShitOwner is Giraffe).Count }] Shits taken by Giraffes.................");
                Printer.Print($".................[{ shitTakenInDay.FindAll(i => i.ShitOwner is Fox).Count }] Shits taken by Foxes.................");
                Printer.Print($".................[{ shitTakenInDay.FindAll(i => i.ShitOwner is Wolf).Count }] Shits taken by Wolves.................");
                Printer.Print($".................[{ shitTakenInDay.FindAll(i => i.ShitOwner is Rabbit).Count }] Shits taken by Rabbits.................");
                Thread.Sleep(5000);

                hourOfDay = 0;
                customerSatisfaction = 100;
                shitTakenInDay = new List<AnimalShit>();
            }
            else
            {
                hourOfDay++;

            }

            lock (HobedShit)
            {
                Printer.Print($".......................One hour has passed the time is now {hourOfDay}:00. Hobed Shit is at {HobedShit.Count}.......................");
                if (HobedShit.Count > 0)
                {
                    foreach (AnimalShit shit in HobedShit)
                    {
                        switch (shit.ShitPriority)
                        {
                            case Priority.Highest:
                                customerSatisfaction -= 4;
                                break;
                            case Priority.High:
                                customerSatisfaction -= 3;
                                break;
                            case Priority.Medium:
                                customerSatisfaction -= 2;
                                break;
                            case Priority.Low:
                                customerSatisfaction -= 1;
                                break;
                            default:
                                break;
                        }

                    }
                }
                if (hourOfDay > 10 && hourOfDay < 20)
                {
                    Printer.Print($"........................................Customer satisfaction at {customerSatisfaction}%............................................");
                }

            }
            
            
            Thread.Sleep(4000);
        }

    }
}
