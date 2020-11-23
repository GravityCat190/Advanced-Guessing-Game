using System;

namespace GuessingGame
{
    static class Puzzle
    {
        private static string[] answers = { "egg", "sleeping", "cat", "addiction"};
        private static string[] secretHints = 
            {"Have you had breakfast today?",
            "Although many would like to hug Keanu Reeves, this puzzle is about hugging his friend handing out pills to people.",
            "Let's face it, Jerry was the bad guy.",
            "How often did your mom tell you that playing games are unhealthy for you?" };
        private static string[,] hints = {
            {"You need to break it before you can use it.", // hints for first puzzle
             "A container without hinges, lock or a key, yet a golden treasure lies inside.",
             "It is not unusual and yet philosophers were fascinated with the chronological relationship between this and its creator." },
            {"If someone asks you if you are doing this at this moment, you cannot answer truthfully.", // hints for second puzzle
             "It seems to shorten your life. However, if you gave it up, you would really shorten your life.",
             "You do it every day, but you never see yourself doing it."},
            {"You wanted to get rid of rodents, and now you get them as gifts.", // hints for third puzzle
             "Most people love it or hate it - rarely something in the middle.",
             "Why do you need an owl or a toad when you can have it?"},
            {"Pavlov could call it conditioning, but many will call it a different way.", // hints for fourth puzzle
             "It always starts innocently and often also enjoyable.",
             "Habits are good, unless you become their slave."} };
        private static int whichPuzzle = 0; // which puzzle is currently being solved
        private static int whichHint = 0; // which hint WILL be given

        public static string GiveHint()
        {
            /*If the player is given the same hint, he/she gets nothing -> After the last hint, instead of getting another one, 
              the player will rest*/
            if (whichHint <= 2)
            {
                whichHint++;
                if (whichHint == 3) // Player will get now the third and the last hint, so he/she should know that the next one will not be given
                {
                    Console.WriteLine("It's the last hint. You'll not get another one!");
                }
                // 'whichHint - 1' because I need to increment the counter before return
                return "Your hint: " + hints[whichPuzzle, whichHint - 1]; 

            } else
            {
                return "You already know everything that you need! Take a nap or something.";
            }
        }

        public static string GiveSecretHint()
        {
            /*If the player is given the secret hint again, he/she gets nothing -> After getting it once, it will be replaced with an empty
            string and the usual hint will be given when the secret hint is drawn again*/
            if (secretHints[whichPuzzle] != "")
            { // first time
                string secretHint = "Wild secret hint appeared :O " + secretHints[whichPuzzle];
                secretHints[whichPuzzle] = "";
                return secretHint;
            } else
            { // not first time
                return GiveHint();
            }
        }

        public static int CheckPlayerGuess(string playerGuess)
        {
            /* first method checks if playerGuess is the correct answer
            yes -> player solved the puzzle
            no -> if the char on a specific position in the answer for the current puzzle, and the player's guess is the same, 
            validCharsCounter will be increment */
            /* if method return >= 0 it means that the player's guess is not the correct answer. 
             * If method return -1 it means that the player has given the correct answer */
            string currentAnswer = answers[whichPuzzle];

            if (currentAnswer == playerGuess)
            {
                whichPuzzle++;
                whichHint = 0;
                return -1;
            }

            int validCharsCounter = 0;
            // playerGuess.Length-1 -> I'll use i to go through the entire array. The returned length will be greater than the last index
            for (int i = 0; i <= playerGuess.Length-1; i++)
            {
                try
                {
                    if (currentAnswer[i] == playerGuess[i]) { validCharsCounter++; }
                }
                catch (IndexOutOfRangeException)
                {
                    break;
                }
            }
            return validCharsCounter;
        }

        public static int GiveAnswerLength()
        {
            return answers[whichPuzzle].Length;
        }

        public static int WhichPuzzle
        {
            get { return whichPuzzle; }
        }
    }
}
