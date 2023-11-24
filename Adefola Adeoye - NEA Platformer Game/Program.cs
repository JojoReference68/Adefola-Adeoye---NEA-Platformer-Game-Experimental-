using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Threading;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    internal class Program
    {
        public const string arrow = ">";
        public static Tutorial_Stage tutorial_Stage;
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight); //adjusts console size
            Console.Title = "NEA Platformer Game";
            Console.CursorVisible = false;
            bool exit = false;
            while (exit == false) //Starter menu
            {
                Console.CursorVisible = false;
                Console.Title = "NEA Platformer Game";
                int startMenuOption = DisplayStartMenu(4);
                switch (startMenuOption)
                {
                    case 0:
                        StartNewGame();
                        Console.ReadKey(true);
                        break;
                    case 1:
                        StartTutorial();
                        Console.ReadKey(true);
                        break;
                    case 2:
                        ShowHighScores();
                        Console.ReadKey(true);
                        break;
                    case 3:
                        exitGame();
                        exit = true;
                        break;

                }
            }




            Console.ReadKey(true);
        }

        static void FancyDialogue(string sentence)
        {
            foreach (char c in sentence)
            {
                Console.Write(c);
                Thread.Sleep(50);
            }
            Console.WriteLine();
        }
        static int DisplayStartMenu(int totalOptions) //Shows the Start menu
        {

            int option = 0;
            bool exit = false;


            Console.WriteLine("  New Game");
            Console.WriteLine("  Play Tutorial");
            Console.WriteLine("  View High Scores");
            Console.Write("  Close Game");
            initializeArrow(arrow);
            while (exit == false)//Responsive menu GUI
            {
                ConsoleKeyInfo input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.UpArrow && option > 0)//Moves the cursor up when the up arrow is pressed AND it is not at the top of the list
                {
                    Up(arrow);
                    option--;
                }
                else if (input.Key == ConsoleKey.DownArrow && option < totalOptions - 1)//Moves the cursor down when the down arrow is pressed AND it is not at the end of the list
                {
                    option++;
                    Down(arrow);
                }
                else if (input.Key == ConsoleKey.Enter)//Chooses an option and completes the option in main
                {
                    exit = true;
                }

            }
            return option;
        }

        static void StartNewGame()
        {
            Console.Clear();
            //string userName = GetUsername();
            StartLevel();

            Console.Clear();
        }

        static void StartLevel()
        {
            Game_Stage game_Stage = new Game_Stage();
            game_Stage.BeginGame();
        }
        static void StartTutorial()
        {
            Console.Clear();
            string userName = GetUsername();
            Player player = new Player(userName, 90, 20, 0, 0);
            tutorial_Stage = new Tutorial_Stage(player);
            ShowTutorial();
            LoadTutorialStage();
            BeginGame();
            Console.ReadKey();
            Console.Clear();
        }
        static void ShowTutorial()
        {
            tutorial_Stage.ShowTutorial();
        }

        static void BeginGame()
        {
            tutorial_Stage.BeginGame();
        }
        static void LoadTutorialStage()
        {
            tutorial_Stage.GenerateTutorialMap();
            tutorial_Stage.DisplayMap();
            System.Threading.Thread.Sleep(2000);
        }

        static string GetUsername()
        {
            Console.WriteLine("Enter your username.");
            LoadingSequence();
            string userName;
            string regex = @"^[a-zA-Z][a-zA-Z0-9]*$";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter your username:");
                LoadingSequence();
                userName = Console.ReadLine();

                //Checks if the username matches the regular expression
                if (Regex.IsMatch(userName, regex))
                {
                    Console.Clear();
                    return userName;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid username. Usernames must start with a letter, contain only letters and numbers, and have at least one character.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

            static void ShowHighScores()//prints out the highscores from a txt file
        {
            Console.Clear();
            HighScoreManager highScoreManager = new HighScoreManager();
            highScoreManager.ReadHighScoresFromFile();
            highScoreManager.SortDescending();
            highScoreManager.PrintHighScores();
            LoadingExitSequence();
            Console.Clear();
        }


        static void Down(string character)
        {

            Console.CursorLeft = 0;
            Console.Write(' ');
            Console.CursorTop++;
            Console.CursorLeft = 0;
            Console.Write(character);
        }
        static void Up(string character)
        {
            Console.CursorLeft = 0;
            Console.Write(' ');
            Console.CursorTop--;
            Console.CursorLeft = 0;
            Console.Write(character);
        }
        static void initializeArrow(string character)
        {
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            Console.Write(character);
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
        }

        static void exitGame()
        {
            Console.Clear();
            Console.WriteLine("Thanks for playing!!");
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.WriteLine("               *    *\r\n   *         '       *       .  *   '     .           * *\r\n                                                               '\r\n       *                *'          *          *        '\r\n   .           *               |               /\r\n               '.         |    |      '       |   '     *\r\n                 \\*        \\   \\             /\r\n       '          \\     '* |    |  *        |*                *  *\r\n            *      `.       \\   |     *     /    *      '\r\n  .                  \\      |   \\          /               *\r\n     *'  *     '      \\      \\   '.       |\r\n        -._            `                  /         *\r\n  ' '      ``._   *                           '          .      '\r\n   *           *\\*          * .   .      *\r\n*  '        *    `-._                       .         _..:='        *\r\n             .  '      *       *    *   .       _.:--'\r\n          *           .     .     *         .-'         *\r\n   .               '             . '   *           *         .\r\n  *       ___.-=--..-._     *                '               '\r\n                                  *       *\r\n                *        _.'  .'       `.        '  *             *\r\n     *              *_.-'   .'            `.               *\r\n                   .'                       `._             *  '\r\n   '       '                        .       .  `.     .\r\n       .                      *                  `\r\n               *        '             '                          .\r\n     .                          *        .           *  *\r\n             *        .                                    '");

        }

        static void LoadingExitSequence()
        {
            int count = 0;
            Console.Write("\n\nEnter any key to exit");
            while (Console.KeyAvailable == false)
            {

                if (count < 30)
                {
                    Console.Write('.');
                    count++;
                }
                System.Threading.Thread.Sleep(250);
            }
        }

        static void LoadingSequence()
        {
            int count = 0;
            while (Console.KeyAvailable == false)
            {

                if (count < 30)
                {
                    Console.Write('.');
                    count++;
                }
                System.Threading.Thread.Sleep(250);
            }
        }
    }
}
