using System;

namespace GuessingGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
        }

        static void Start()
        {
            bool playerAreDumb = true;
            while (playerAreDumb == true)
            {
                Console.WriteLine("");
                Console.WriteLine("================================================");
                Console.WriteLine("Hello in Guessing Game");
                Console.WriteLine("Type \"s\" to start a new game");
                Console.WriteLine("");
                Console.Write("What do you want to do: ");
                string playerInput = Console.ReadLine();
                switch (playerInput)
                {
                    case "s":
                        playerAreDumb = false;
                        StartNewGame();
                        break;
                    default:
                        Console.WriteLine("Well yea... try again ok?");
                        Console.WriteLine("...");
                        Console.WriteLine("...");
                        Console.WriteLine("...");
                        break;
                }
            }
        }

        static void StartNewGame()
        {
            // Introduction
            Console.WriteLine("You started a new game");
            Console.WriteLine("...");
            Console.WriteLine("...");
            Console.WriteLine("...");
            Console.WriteLine("Hello Hero! You forgot to wear your plot armor and some random snake bites you. You will die in 24 hours.");
            Console.WriteLine("Yea well that's all. Good luck!");

            bool gameOver = false;
            while (gameOver == false)
            {
                /*Game loop
                In the beginning, the player will see hero condition, then I'll show all of the activities that he/she can do.
                Depending on his/her input I'll start the method. On the end I'll check if he/she is still alive. If yes another loop will happen */
                Console.WriteLine("");
                Console.WriteLine("================================================");
                CheckHeroCondition();
                Console.WriteLine("Type \"s\" to try to solve the puzzle");
                Console.WriteLine("Type \"r\" to rest");
                Console.WriteLine("Type \"h\" to get a hint");
                Console.WriteLine("Type \"l\" to try your luck");
                Console.WriteLine("");
                Console.Write("What do you want to do: ");
                string playerInput = Console.ReadLine();

                switch (playerInput)
                {
                    case "s":
                        TryToSolvePuzzle();
                        break;
                    case "r":
                        Rest();
                        break;
                    case "h":
                        GetHint();
                        break;
                    case "l":
                        DiceRoll();
                        break;
                    default:
                        Console.WriteLine("Well yea... try again ok?");
                        break;
                }
                // Everytime when energy or timeLeft is changed, Hero.alive can change. If that happens, the game is over and player loses
                if (Hero.Alive == false) { gameOver = true; }
                // If player solves a puzzle, whichPuzzle is incremented. If WhichPuzzle returns 4 it means that the player solved all of the puzzles
                if (Puzzle.WhichPuzzle == 4) { gameOver = true; }
                Console.ReadLine();
            }
        }

        static void CheckHeroCondition()
        {
            int heroEnergy = Hero.Energy;
            int heroTimeLeft = Hero.TimeLeft;
            int[] heroStatuesCounters = Hero.StatusesCounters;

            string messageFirstLine; // line for energy and timeLeft
            string messageSecondLine = ""; // line for statues counters

            messageFirstLine = "Hero energy: " + heroEnergy + "%, Hero time left: " + heroTimeLeft + " hours";

            string statusMessagePart = "";

            /* I'll skip i=0 because the first status counter is for AHA Moment and I use this counter only to 
               determine what I should do if AHA Moment happens again in DiceRoll */
            // heroStatuesCounters.Length-1 -> I'll use i to go through the entire array. The returned length will be greater than the last index
           for (int i = 1; i <= heroStatuesCounters.Length - 1; i++)
           {
               /* messageSecondLine should look like e.g. "Motivation (2), Dizzines (4)" 
                first I need to check if a status is active (counter is not equal to 0), than write down the name of the status and add its counter*/
            if (heroStatuesCounters[i] != 0)
                {
                    switch (i)
                    {
                        case 1:
                            statusMessagePart = "Motivation";
                            break;
                        case 2:
                            statusMessagePart = "Narcolepsy";
                            break;
                        case 3:
                            statusMessagePart = "Effective antidote";
                            break;
                        case 4:
                            statusMessagePart = "Slow moves";
                            break;
                        case 5:
                            statusMessagePart = "Dizziness";
                            break;
                        case 6:
                            statusMessagePart = "Exhaustion";
                            break;
                        case 7:
                            statusMessagePart = "Student syndrome";
                            break;
                    }
                    // I'll need to remove the comma after the last status
                    messageSecondLine = messageSecondLine + statusMessagePart + " (" + heroStatuesCounters[i] + "), ";
                }
            }
            Console.WriteLine(messageFirstLine);
            // removing comma on the end of the message and showing second line of message (if variable is "" there are no active effects)
            if (messageSecondLine != "")
            {
                messageSecondLine = messageSecondLine.Substring(0, messageSecondLine.Length - 2); // -2 -> because I need to remove: ", "
                Console.WriteLine(messageSecondLine);
            }
        }

        static void TryToSolvePuzzle()
        {
            /* Statues that may effect that action
               Motivation - extra energy after guessing the puzzle
               Effective antidote - more time after guessing the puzzle
               Dizziness - failed guessing may return a modified result of valid chars */
            int[] statusesCounters = Hero.StatusesCounters;

            Console.Write("Your guess: ");
            string playerGuess = Console.ReadLine();

            /* Puzzle.CheckPlayerGuess returns counter of valid chars in player's guess.
             If method returns >= 0 it means that the player's guess is not the correct answer. 
             If method returns -1 it means that the player has given the correct answer. */
            int validCharsCounter;
            validCharsCounter = Puzzle.CheckPlayerGuess(playerGuess);

            if (validCharsCounter == -1)
            {
                PlayerSolvedPuzzle();
            }
            else
            {
                Random rnd = new Random();

                // Dizziness - failed guessing may return a modified result of valid chars
                // Dizziness counter is the sixth one
                if (statusesCounters[5] != 0)
                {
                    // 1+1 -> Random.Next excludes maxValue (2 argument)
                    int counterChange = rnd.Next(-1, 1 + 1); // number that will determine if and how the counter will be modified
                    validCharsCounter = validCharsCounter + counterChange;

                    // if validCharsCounter was equal to 0 before being modified, after modifying it may be equal to -1
                    if (validCharsCounter == -1) { validCharsCounter = 0; }
                }

                Console.WriteLine("It's not the correct answer. In your guess " + validCharsCounter + " chars were on the right place");

                // subtracting energy
                // -20+1 -> Random.Next excludes maxValue (2 argument)
                int lostEnergy = rnd.Next(-30, -20 + 1); // the amount of energy that the player will lose after that guess

                string messageLostEnergy = Convert.ToString(lostEnergy); // e.g. -25
                messageLostEnergy = messageLostEnergy.Substring(1); // I'll cut the "-"
                Console.WriteLine("You lost " + messageLostEnergy + "% energy after that guess");

                Hero.ChangeEnergy(lostEnergy);
            }

            // statuses counters change
            // Motivation counter is the second one
            if (statusesCounters[1] != 0) { statusesCounters[1]--; }
            // Effective antidote counter is the fourth one
            if (statusesCounters[3] != 0) { statusesCounters[3]--; }
            // Dizziness counter is the sixth one
            if (statusesCounters[5] != 0) { statusesCounters[5]--; }

        }

        static void PlayerSolvedPuzzle()
        {
            /*if player solves a puzzle, whichPuzzle is incremented. 
            If WhichPuzzle returns 4 it means that the player has solved all of the puzzles */
            if (Puzzle.WhichPuzzle != 4)
            {
                Console.WriteLine("Congratulation! You solved the puzzle!");
                Hero.PlayerSolvedPuzzle();
            }
            else
            {
                Console.WriteLine("Congratulation! You solved all of the puzzles!");
                Console.WriteLine("");
                Console.WriteLine("For that achievement, the medic in your village has finally let you out from his basement and healed you.");
                Console.ReadLine();
                Console.WriteLine("I think you should report him somewhere...");
                Console.ReadLine();
                Console.WriteLine("Anyway good job :3 I hope that you enjoyed it");
            }
        }

        static void Rest()
        {
            /* Statues that may effect that action
               Narcolepsy - chance that rest will not give you energy */
            /* While resting, the player can recover between 30 and 50 energy but he/she will lose 2 or 3 hours. 
            In addition, he/she has a 1% chance to sleep for 8 hours -> this may happen only if the player has more than 8 hours left
            First, I'll check if he/she has more than 8 hours left. If yes, I'll check if long nap will happen. 
            If no, I'll check amount of hours that player will lose. After that, I'll check the amount of energy that the player will recover*/
            int[] statusesCounters = Hero.StatusesCounters;
            Random rnd = new Random();
            int chance;

            // 2+1 -> Random.Next excludes maxValue (2 argument)
            chance = rnd.Next(1, 2 + 1); // chance == 1 -> player will not restore energy
            //Narcolepsy counter is the third one
            if (chance == 1 && statusesCounters[2] != 0) // chance needs to be equal to 1 and Narcolepsy needs to be active
            {
                Console.Write("Because of narcolepsy you don't feel better after trying to rest.");
            }
            else
            {
                // adding energy
                if (Hero.Energy >= 100)
                {
                    Console.Write("Well you're already full of the energy.");
                }
                else
                {
                    int recoveredEnergy;
                    // Exhaustion - resting will give only half energy
                    // Exhaustion counter is the seventh one
                    if (statusesCounters[6] != 0)
                    {
                        // 25+1 -> Random.Next excludes maxValue (2 argument)
                        recoveredEnergy = rnd.Next(15, 25 + 1); // the amount of energy that the player will recover after that rest 
                    }
                    else
                    {
                        // 50+1 -> Random.Next excludes maxValue (2 argument)
                        recoveredEnergy = rnd.Next(30, 50 + 1); // the amount of energy that the player will recover after that rest
                    }

                    // checking to see if the player should get more energy than possible (energy > 100 after Hero.ChangeEnergy)
                    int energy = Hero.Energy;
                    if (energy + recoveredEnergy > 100)
                    {
                        // e.g. the hero has 90 energy and recoveredEnergy = 20 -> 90 + 20 = 110 > 100 
                        recoveredEnergy = 100 - energy; // e.g. -> recoveredEnergy = 100 - 90 = 10
                    }

                    Hero.ChangeEnergy(recoveredEnergy);
                    Console.Write("You have rested and you restored " + recoveredEnergy + "% of the energy!");
                }
            }
            // substract time
            // 100+1 -> Random.Next excludes maxValue (2 argument)
            chance = rnd.Next(1, 100 + 1); // chance == 1 -> long nap
            if (Hero.TimeLeft > 8 && chance == 1) // may happen only if the player has more than 8 hours and chance will be equal to 1
            {
                Hero.ChangeTimeLeft(-8);
                Console.Write(" But you fell asleep and slept for 8 hours. " +
                    "After that \"long nap\" you're not even sure in which solar system you're.");
            }
            else
            {
                int timeCost;
                // Student syndrome - resting and taking a hint will take the maximum possible amount of time
                // Student syndrome counter is the eighth one
                if (statusesCounters[7] != 0)
                {
                    timeCost = -3;
                }
                else
                {
                    // -2+1 -> Random.Next excludes maxValue (2 argument)
                    timeCost = rnd.Next(-3, -2 + 1); // number that will determine how much time will this action cost
                }

                // Slow moves - resting and taking a hint will take more time
                // Slow moves counter is the fifth one
                if (statusesCounters[4] != 0)
                {
                    // -1+1 -> Random.Next excludes maxValue (2 argument)
                    int extraTimeCost = rnd.Next(-3, -1 + 1);
                    timeCost = timeCost + extraTimeCost;
                }

                string messageTimeCost = Convert.ToString(timeCost); // e.g. -2
                messageTimeCost = messageTimeCost.Substring(1); // I'll cut the "-"
                Console.WriteLine(" But it costed you " + messageTimeCost + " hours.");

                Hero.ChangeTimeLeft(timeCost);
            }

            // statuses counters change
            //Narcolepsy counter is the third one
            if (statusesCounters[2] != 0) { statusesCounters[2]--; }
            // Slow moves counter is the fifth one
            if (statusesCounters[4] != 0) { statusesCounters[4]--; }
            // Exhaustion counter is the seventh one
            if (statusesCounters[6] != 0) { statusesCounters[6]--; }
            // Student syndrome counter is the eighth one
            if (statusesCounters[7] != 0) { statusesCounters[7]--; }
        }

        static void GetHint()
        {
            /* Statues that may effect that action
               Slow moves - rest and taking hint will take more time */
            /* Player can get up to 3 hints. In addition, he/she has a 10 % chance to get a secret hint.
             First, I'll check if he/she will get the secret hint or the usual hint. After, that he/she will lose some time.
             At the end, I'll check if he/she didn't already get all of the hints. */

            int[] statusesCounters = Hero.StatusesCounters;

            string hint = "";
            Random rnd = new Random();
            // 10+1 -> Random.Next excludes maxValue (2 argument)
            int chance = rnd.Next(1, 10 + 1); // chance == 1 -> secret hint
            if (chance == 1)
            {
                hint = Puzzle.GiveSecretHint();
            }
            else
            {
                hint = Puzzle.GiveHint();
            }

            // checking if he/she didn't already get all of the hints.
            if (hint != "You already know everything that you need! Take a nap or something.")
            {
                Console.WriteLine(hint);

                int timeCost;
                // Student syndrome - resting and taking hint will take the maximum possible amount of time
                // Student syndrome counter is the eighth one
                if (statusesCounters[7] != 0)
                {
                    timeCost = -4;
                }
                else
                {
                    // -2+1 -> Random.Next excludes maxValue (2 argument)
                    timeCost = rnd.Next(-4, -1 + 1); // number that will determine how much time will this action cost
                }

                // Slow moves - resting and taking a hint will take more time
                // Slow moves counter is the fifth one
                if (statusesCounters[4] != 0)
                {
                    // -1+1 -> Random.Next excludes maxValue (2 argument)
                    int extraTimeCost = rnd.Next(-3, -1 + 1);
                    timeCost = timeCost + extraTimeCost;
                }

                string messageTimeCost = Convert.ToString(timeCost); // e.g. -2
                messageTimeCost = messageTimeCost.Substring(1); // I'll cut the "-"
                Console.WriteLine("That action costed you " + messageTimeCost + " hours.");
                Hero.ChangeTimeLeft(timeCost);

            }
            else
            {
                Console.WriteLine(hint);
                Rest();
            }
            // statuses counters change
            // Slow moves counter is the fifth one
            if (statusesCounters[4] != 0) { statusesCounters[4]--; }
        }

        static void DiceRoll()
        {
            Random rnd = new Random();
            //6+1 -> Random.Next excludes maxValue (2 argument)
            int dice = rnd.Next(1, 6 + 1);

            switch (dice)
            {
                case 1:
                    /* AHA Moment: The player will get a hint about answer's length. 
                     If the player already got this hint once he/she, will get the exact length */
                    // correctCounter1 -> 1 because it's the first case
                    int correctCounter1 = 0; // AHA Moment counter it's the first one
                    int ahaMomentCounter = Hero.StatusesCounters[correctCounter1];
                    if (ahaMomentCounter == 0)
                    {
                        /*I need a number between the puzzle answer length -2 and the puzzle answer length 
                        for example, if the answer is 5 chars long, then hint may be: (3,5), (4,6), (5,7)*/
                        int answerLength = Puzzle.GiveAnswerLength();
                        int substractedAnswerLength = answerLength - 2;
                        // if the answer has only 3 or 4 chars I don't want to show hint: 1-3 or 2-4
                        if (substractedAnswerLength < 3) { substractedAnswerLength = 3; }

                        // answerLength + 1 -> Random.Next excludes maxValue (2 argument)
                        int setBeginning = rnd.Next(substractedAnswerLength, answerLength + 1);
                        int setEnd = setBeginning + 2;

                        Console.WriteLine("AHA Moment: The answer for the current puzzle has between " + setBeginning + " and " + setEnd + " chars");

                        Hero.ChangeStatusesCounters(correctCounter1, 1);
                    }
                    else if (ahaMomentCounter == 1)
                    {
                        int answerLength = Puzzle.GiveAnswerLength();
                        Console.WriteLine("AHA Moment: The answer for the current puzzle has " + answerLength + " chars");

                        Hero.ChangeStatusesCounters(correctCounter1, 1);
                    }
                    else
                    {
                        Console.WriteLine("Really? 1 on a dice roll this many times?");
                        Console.ReadLine();
                        Console.WriteLine("Okay... I'll give you one more hour for that");
                        if (Hero.TimeLeft < 24)
                        {
                            Hero.ChangeTimeLeft(1);
                        }
                        else
                        {
                            Console.WriteLine("OHH FOR FUCK'S SAKE! Seriously you already have the maximum amount of time left...");
                            Console.ReadLine();
                            Console.WriteLine("You know what...");
                            Console.ReadLine();
                            Console.WriteLine("You die: A pebble fell on your head and killed you");
                            Console.ReadLine();
                            Console.WriteLine("...");
                            Console.WriteLine("...");
                            Console.WriteLine("...");
                            Console.ReadLine();
                            Console.WriteLine(":c");
                            Console.ReadLine();
                            Console.WriteLine("Nah, just joking. Keep playing :3");
                        }
                    }
                    break;

                case 2:
                    /*Refreshment: The player will get 100 energy. If he/she already has 100 energy a positive or a negative effect will happen:
                     Motivation - if the player will solve a puzzle in the next few guesses he/she will have more energy at the beginning of the next puzzle
                     Narcolepsy - the chance that resting will not give the player energy*/
                    if (Hero.Energy < 100)
                    {
                        Hero.ChangeEnergy(100); // I can only add/substract energy so now I'm sure that the player will have 100 energy
                        Console.WriteLine("Refreshment: You're full of energy!");
                    }
                    else // hero has 100 energy or more, so I'll check if Motivation or Narcolepsy happens
                    {
                        // 2+1 -> Random.Next excludes maxValue (2 argument)
                        int refreshmentChance = rnd.Next(1, 2 + 1);
                        if (refreshmentChance == 1) // refreshmentChance == 1 -> Motivation
                        {
                            // Currently I have 4 puzzles (0-3). If the current puzzle is the last one, this status will not give the player anything
                            if (Puzzle.WhichPuzzle == 3)
                            {
                                Console.WriteLine("You should get a bonus for the the next puzzle but it's the last one," +
                                    " so the only thing that I can do is wishing you luck!");
                            }
                            else
                            {
                                // correctCounter2 -> 2 because it's the second case
                                int correctCounter2 = 1; // Motivation counter is the second one
                                Hero.ChangeStatusesCounters(correctCounter2, 2);
                                int motivationCounter = Hero.StatusesCounters[correctCounter2];
                                if (motivationCounter == 1)
                                {
                                    Console.WriteLine("Motivation: If you will solve the puzzle in the next" +
                                    " attempt you will get 150% energy at the beginning of the next puzzle.");
                                }
                                else
                                {
                                    Console.WriteLine("Motivation: If you will solve the puzzle in the next " + motivationCounter +
                                    " attempts you will get 150% energy at the beginning of the next puzzle.");
                                }
                            }
                        }
                        else // refreshmentChance == 2 -> Narcolepsy
                        {
                            // correctCounter2 -> 2 because it's the second case
                            int correctCounter2 = 2; //Narcolepsy counter is the third one
                            Hero.ChangeStatusesCounters(correctCounter2, 1);
                            int narcolepsyCounter = Hero.StatusesCounters[correctCounter2];
                            if (narcolepsyCounter == 1)
                            {
                                Console.WriteLine("Narcolepsy: Within the next rest there is a 50% chance that you will not get any energy.");
                            }
                            else
                            {
                                Console.WriteLine("Narcolepsy: Within the next " + narcolepsyCounter +
                                    " rests its a 50% chance that you will not get any energy.");
                            }
                        }
                    }

                    break;

                case 3:
                    /*Immune reaction: The player will get 24 hours. If he/she already has 24 hours, a positive or a negative effect will happen:
                     Effective antidote - if the player will solve a puzzle in the next few guesses he/she will have more time at the beginning of the next puzzle
                     Slow moves - resting and taking a hint will take more time*/
                    if (Hero.TimeLeft < 24)
                    {
                        Hero.ChangeTimeLeft(24); // I can only add/substract energy so now I'm sure that the player will have 24 hours left
                        Console.WriteLine("Immune reaction: You have 24 hours left again!");
                    }
                    else // hero has 24 hours or more so I'll check if Effective antidote or Slow moves happens
                    {
                        // 2+1 -> Random.Next excludes maxValue (2 argument)
                        int immuneReactionChance = rnd.Next(1, 2 + 1);
                        if (immuneReactionChance == 1) // immuneReactionChance == 1 -> Effective antidote
                        {
                            // Currently I have 4 puzzles (0-3). If the current puzzle is the last one this status will not give the player anything
                            if (Puzzle.WhichPuzzle == 3)
                            {
                                Console.WriteLine("You should get a bonus for the next puzzle, but it's the last one," +
                                    " so the only thing that I can do is wishing you luck!");
                            }
                            else
                            {
                                // correctCounter3 -> 3 because it's the third case
                                int correctCounter3 = 3; // Effective antidote counter is the fourth one
                                Hero.ChangeStatusesCounters(correctCounter3, 2);
                                int effectiveAntidoteCounter = Hero.StatusesCounters[correctCounter3];
                                if (effectiveAntidoteCounter == 1)
                                {
                                    Console.WriteLine("Effective antidote: If you will solve the puzzle in the next" +
                                        " attempt you will have 36 hours at the beginning of the next puzzle.");
                                }
                                else
                                {
                                    Console.WriteLine("Effective antidote: If you will solve the puzzle in the next " + effectiveAntidoteCounter +
                                    " attempts you will have 36 hours at the beginning of the next puzzle.");
                                }
                            }
                        }
                        else // immuneReactionChance == 2 -> Slow moves
                        {
                            // correctCounter3 -> 3 because it's the third case
                            int correctCounter3 = 4; // Slow moves counter is the fifth one
                            Hero.ChangeStatusesCounters(correctCounter3, 1);
                            int slowMovesCounter = Hero.StatusesCounters[correctCounter3];
                            if (slowMovesCounter == 1)
                            {
                                Console.WriteLine("Slow moves: Within the next resting or taking a hint you will lose bettwen 1 and 3 more hours.");
                            }
                            else
                            {
                                Console.WriteLine("Slow moves: Within the next " + slowMovesCounter +
                                    " rests and taking hints you will lose bettwen 1 and 3 more hours.");
                            }
                        }
                    }

                    break;

                case 4:
                    // Dizziness - failed guessing may return a modified result of valid chars
                    // correctCounter4 -> 4 because it's the fourth case
                    int correctCounter4 = 5; // Dizziness counter is the sixth one
                    Hero.ChangeStatusesCounters(correctCounter4, 2);
                    int dzizzinessCounter = Hero.StatusesCounters[correctCounter4];
                    if (dzizzinessCounter == 1)
                    {
                        Console.WriteLine("Dizziness: Within the next guessing, you may get a hint about valid characters, modified by -1 or +1.");
                    }
                    else
                    {
                        Console.WriteLine("Dizziness: Within the next " + dzizzinessCounter +
                            " guesses, you may get a hint about valid characters, modified by -1 or +1.");
                    }

                    break;

                case 5:
                    // Exhaustion - resting will give only half energy
                    // correctCounter5 -> 5 because it the fifth case
                    int correctCounter5 = 6; // Exhaustion counter is the seventh one
                    Hero.ChangeStatusesCounters(correctCounter5, 2);
                    int exhaustionCounter = Hero.StatusesCounters[correctCounter5];
                    if (exhaustionCounter == 1)
                    {
                        Console.WriteLine("Exhaustion: Within the next rest, you will recover only half of the energy.");
                    }
                    else
                    {
                        Console.WriteLine("Exhaustion: Within the next " + exhaustionCounter +
                            " rests, you will recover only half of the energy.");
                    }

                    break;

                case 6:
                    /* Mobile games - The player will lose 6 hours. If he/she has less than 7 hours another status will happen
                    Student syndrome - resting and taking a hint will take the maximum possible amount of time */
                    if (Hero.TimeLeft > 6)
                    {
                        Hero.ChangeTimeLeft(-6);
                        Console.WriteLine("You played on your phone and lost 6 hours,");
                        Console.ReadLine();
                        Console.WriteLine("You have also run out of battery so you cannot use it now,");
                        Console.ReadLine();
                        Console.WriteLine("Seriously, you could use it in many other good ways,");
                        Console.ReadLine();
                        Console.WriteLine("In the future, perhaps you should think twice before you do something like this.");
                    }
                    else
                    {
                        // correctCounter6 -> 6 because it's the sixth case
                        int correctCounter6 = 7; // Student syndrome counter is the eighth one
                        Hero.ChangeStatusesCounters(correctCounter6, 2);
                        int studentSyndromeCounter = Hero.StatusesCounters[correctCounter6];
                        if (studentSyndromeCounter == 1)
                        {
                            Console.WriteLine("Student syndrome: The next rest or taking a hint will take the maximum possible amount of time.");
                        }
                        else
                        {
                            Console.WriteLine("Student syndrome: The next " + studentSyndromeCounter +
                                    " rests and taking hints will take the maximum possible amount of time.");
                        }
                    }
                    break;
            }
        }
    }
}