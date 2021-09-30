using System;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Hangman
{
    class Program
    {
        public static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return path;
        }
        static void Main(string[] args)
        {
            string userInput;
            string[] wordsArray ;

            Console.Write(" __________\n");
            Console.Write(" || /   │\n");
            Console.Write(" ||/    @\n");
            Console.Write(" ||    /|\\\n");
            Console.Write(" ||    / \\\n");
            Console.Write(" ||\n");
            Console.Write(" ||========|\n");
            try 
            {
                //If the program was unable to read the words from the text file:
                //1.Create a text file in same program.cs directory for ex  \your\project\name\bin\Debug\netcoreapp3.1\ and name it ListOfWords.txt
                //2.Write down some words separated with a comma ','

                // strExeFilePath => directory = @"path\to\sourcecode"
                string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                //This to get path name:
                string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
                // merage the path with the txt file name
                string strSettingsTxtFilePath = System.IO.Path.Combine(strWorkPath, "ListOfWords.txt");
                using (StreamReader filereader = new StreamReader(strSettingsTxtFilePath))
                {
                   string  tempWordsArray = filereader.ReadLine();
                    if (tempWordsArray == null)
                        throw new ArgumentNullException();
                    wordsArray = tempWordsArray.ToUpper().Split(',');
                }
                Console.WriteLine("Game loaded successfully and it will start shortly..");
                Thread.Sleep(4000);
            }
            catch (Exception e )
            {
                Console.Beep();
                Console.WriteLine("Error occurred while trying to reach the txt file, the Game will start shortly by using the temporary word list. \n" + e);
                Thread.Sleep(4000);
                
                wordsArray = new string[] {"VATTEN","SVENSKA","IDAG","HEJ" };
            }

            do { 
                
            int wrongGuessesCount = 10 , rightLettersCount = 0;
            Random random = new Random();
            string wordToGuess = wordsArray [random.Next(0, wordsArray.Length)];
            char[] wordToGuessLetters = wordToGuess.ToCharArray(); // pickup a word to play with
            char[] playerGuessLetters = new char[wordToGuess.Length]; // the right letter from the player
            StringBuilder pridectedLetters = new StringBuilder();      // all the letters saved 

            Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n\t\t ¤¤¤¤¤ Welcome to Hangman Game ¤¤¤¤¤\n\n" +
                $"\tTry to find The mystery word that has {wordToGuess.Length} letters. ");
                Console.ForegroundColor = ConsoleColor.Blue;
                DrawEmptyRow();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("_________________________________________________________________________________");
            
            do
            {
                //Console.WriteLine("_________________________________________________________________________________");
                Console.Write($"The mystery word is a ({wordToGuess.Length}) letters word and you found ({rightLettersCount}) av ({wordToGuess.Length}) letters. ");
                //ShowGuessedLetters(playerGuessLetters);
                Console.WriteLine();

                Console.Write($"Enter a letter or a whole word to guess the mystery word, You have ({wrongGuessesCount}) attempt(s) left: ");
                userInput = Console.ReadLine().ToUpper();
                while (!IsAlphabets(userInput) )
                {
                    Console.Write($"Enter a letter or a whole word to guess the mystery word, You have ({wrongGuessesCount}) attempt(s) left: ");
                    userInput = Console.ReadLine().ToUpper();
                }
                
                if (!IsARepeatedLetter(userInput))
                    IsEntryExists(userInput);
                Console.WriteLine();
            }
            while (wrongGuessesCount != 0 && wordToGuess.Length != rightLettersCount);

            Console.WriteLine("_________________________________________________________________________________");
            Console.Beep();
            if (rightLettersCount == wordToGuess.Length)
                Console.WriteLine($"Congratulations !!!, you save the man :)  it took you ({-wrongGuessesCount+10}) worng gesse(s) to find the word : {wordToGuess}");
            else if (rightLettersCount == 0)
                Console.WriteLine($"Sorry to lost without finding any letter :(  the mystery word is : {wordToGuess}");
            else
                Console.WriteLine($"You were nearly to find the mystery word :| found ({rightLettersCount}) av ({wordToGuess.Length}) letters , The mystery word is : {wordToGuess}");

            Console.WriteLine("\n Would you like to play again! press enter key for yes or any other key to exit ");

            //////////////////////Used local methods////////////////////////////////

            //// Method to check if the letter the user entered is used before 
            bool IsARepeatedLetter(string input)
            {
                if (input.Length > 1)
                    return false;
                for (int i =0; i< pridectedLetters.Length;i+=2)
                {
                    if (pridectedLetters[i] == char.Parse(input))
                    {
                            Console.Beep();
                            Console.Clear();
                        Console.Write($"\nThe letter [ {userInput} ] is already used. ");
                        ShowGuessedLetters(playerGuessLetters);
                        return true;
                    }
                }
                return false;
            }

            //////  method to check if the letter is exisit in the word
            bool IsEntryExists(string input)
            {
                switch (input)
                {
                    case var value when input.Length > 1:
                        if (input == wordToGuess)
                        {
                            rightLettersCount = wordToGuess.Length;
                            return true;
                        }
                        else
                        {
                            Console.Beep();
                            Console.Clear();
                            Console.Write($"\nWrong guessing of word [ {input} ]. ");
                            ShowGuessedLetters(playerGuessLetters);
                            wrongGuessesCount--;
                            return false;
                        }

                    case var value when input.Length == 1:
                        bool letterNotFound = true;
                        for (int i = 0; i <= wordToGuess.Length-1; i++)
                        {
                            if (wordToGuessLetters[i] == char.Parse(input))
                            {
                                playerGuessLetters[i] = char.Parse(input);
                                rightLettersCount++;
                                letterNotFound = false;
                            } 
                        }
                        if(!letterNotFound)
                        { 
                            pridectedLetters.Append(input).Append(",");
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write($"\nIt's a right guessing [{input}] ;) keep going. ");
                                Console.ForegroundColor = ConsoleColor.White;
                                ShowGuessedLetters(playerGuessLetters);
                        }
                        else
                        { 
                            pridectedLetters.Append(input).Append(",");
                                Console.Beep();
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                Console.Write($"\nIt's a wrong guessing [ {input} ] :( Try again. ");
                                Console.ForegroundColor = ConsoleColor.White;
                                ShowGuessedLetters(playerGuessLetters);
                            wrongGuessesCount--;
                        }

                        //ShowGuessedLetters(playerGuessLetters);

                        if (rightLettersCount == wordToGuessLetters.Length)
                            return true;
                        else
                            return false;

                    default:
                        Console.WriteLine("Wrong input , try again !!");
                        return false;
                }
            }
           
            ////  Method to instantiate the mystery word and print out an empty place for the letters to be filled from the user

            void DrawEmptyRow() 
            {
                Console.Write("{ ");
                for (int x = 0; x < wordToGuess.Length; x++)
            {
                playerGuessLetters[x] = '_';
                Console.Write(playerGuessLetters[x] + " ");
            }
                Console.WriteLine("}");
            }

                /// Method print out the letters that the user used and the letters of the mystery word that the user could found  
                void ShowGuessedLetters(char[] wordList)
            {
                Console.WriteLine("\n_________________________________________________________________________________");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nLetters you guessed: {{ {pridectedLetters} }}");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Letters of the mystery word that found: { ");
                foreach (char letter in wordList)
                {
                    Console.Write(letter + " ");
                }
                Console.WriteLine("}");
                Console.ForegroundColor = ConsoleColor.White;
                //Console.WriteLine("\n_________________________________________________________________________________");
            }
      
            // Method to check if input from user is empty or has a digit and accept only alphabet letters.
            bool IsAlphabets(string inputTxt)
            {
                if (string.IsNullOrEmpty(inputTxt))
                {
                    Console.Beep();
                    Console.WriteLine("Empty entry !! a letter or a word must be entered.");
                return false;
                }
                for (int i = 0; i < inputTxt.Length; i++)
                    if (!char.IsLetter(inputTxt[i]))
                    {
                        Console.Beep();
                        Console.WriteLine("Invalid entry !! only alphabetic letters are accepted.");
                        return false;
                    }
                return true;
            }
            } while (Console.ReadKey(true).Key == ConsoleKey.Enter);

        }
    }
}
