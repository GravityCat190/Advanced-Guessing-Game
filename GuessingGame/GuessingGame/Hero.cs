using System;

namespace GuessingGame
{
    static class Hero
    {
        private static int energy = 100; // hero have 100% energy at the beginning
        private static int timeLeft = 24; // hero have 24h time left at the beginning 
        private static int[] statusesCounters = // hero doesn't have any active status at the beginning
        {
            0, // AHA Moment - Player will get hint about answer's length
            0, // Motivation - extra energy after guessing the puzzle
            0, // Narcolepsy - chance that rest will not give you energy
            0, // Effective antidote - more time after guessing the puzzle
            0, // Slow moves - resting and taking hint will take more time
            0, // Dizziness - failed guessing may return a modified result of valid chars
            0, // Exhaustion - resting will give only half energy
            0  // Student syndrome - resting and taking hint will take the maximum possible amount of time 
        };
        private static bool alive = true; // the flag that changes if the player loses

        public static void ChangeTimeLeft(int timeChangeAmount)
        {
            timeLeft = timeLeft + timeChangeAmount;
            if (timeLeft >= 24) { timeLeft = 24; } //hero should not have more than 24 hours except for Effective antidote status
            if (timeLeft <= 0) { HeroDie("time"); } 
        }
        public static void ChangeEnergy(int energyChangAmount)
        {
            energy = energy + energyChangAmount;
            if (energy >= 100) { energy = 100; } //hero should not have more than 100 energy except for Motivation status
            if (energy <= 0) { HeroDie("energy"); }

        }
        
        public static void ChangeStatusesCounters(int whichStatus, int counterChangeAmount)
        {
            try
            {
                /* Question: can I block the passing of some parameters in the method? Just as the compiler will show an error 
                if it tries to enter int into a parameter that should be a string, is it possible to block the passing of an argument 
                of the appropriate type, but, for example, greater than X?*/
                statusesCounters[whichStatus] = statusesCounters[whichStatus] + counterChangeAmount;
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("WTF Hero.ChangeStatusesCounters: " + e.Message);
            }
        }

        public static void HeroDie(string reason) 
        {
            alive = false;
            if (reason == "time")
            {
                Console.WriteLine("You die: The venom killed you :c");
            }
            else if (reason == "energy")
            {
                Console.WriteLine("You die: You died of exhaustion :c");
            }
            else
            {
                Console.WriteLine("Amm... to be honest I don't know why you're here");
                Console.WriteLine("Hmm... OK");
                Console.ReadLine();
                Console.WriteLine("You die: A pebble fell on your head and it killed you");
                Console.ReadLine();
                Console.WriteLine("...");
                Console.WriteLine("...");
                Console.WriteLine("...");
                Console.ReadLine();
                Console.WriteLine(":c");
            }
        }

        public static void PlayerSolvedPuzzle()
        {
            /* after solving a puzzle the player will not get extra energy (unless motivation status is active),
            but he/she will get more time*/
            if (statusesCounters[1] != 0) // second counter is Motivation counter
            {
                energy = 150;
                Console.WriteLine("After that success, you feel full of energy! You have 150% energy now.");
            }
            Console.WriteLine("After solving that puzzle, an antidote appears in the room where you are. " +
                "It temporarily helps your body to fight the venom.");
            if (statusesCounters[3] != 0) // fourth counter is Effective antidote counter
            {
                timeLeft = 36;
                Console.WriteLine("That antidote was really effective. You have 36 hours instead of 24 hours to solve the next puzzle!");
            } else
            {
                if (TimeLeft < 24)
                {
                    timeLeft = 24;
                    Console.WriteLine("You have 24 hours to solve the next puzzle");
                }
            }

            // AHA Moment counter needs to be reset so player can get this again for the next puzzle
            statusesCounters[0] = 0;
        }

        public static int Energy
        {
            get { return energy; }
        }

        public static int TimeLeft
        {
            get { return timeLeft; }
        }

        public static int[] StatusesCounters
        {
            get { return statusesCounters; }
        }

        public static bool Alive
        {
            get { return alive; }
        }
    }
}
