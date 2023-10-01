using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

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
                int startMenuOption = DisplayStartMenu(5);
                switch (startMenuOption)
                {
                    case 0:
                        StartNewGame();
                        break;
                    case 1:
                        Console.Clear();
                        Console.WriteLine("  Not implemented yet");
                        System.Threading.Thread.Sleep(1000);
                        Console.Clear();
                        break;
                    case 2:
                        StartTutorial();
                        break;
                    case 3:
                        ShowHighScores();
                        break;
                    case 4:
                        exitGame();
                        exit = true;
                        break;

                }
            }




            Console.ReadKey(true);
        }


        static int DisplayStartMenu(int totalOptions) //Shows the Start menu
        {

            int option = 0;
            bool exit = false;


            Console.WriteLine("  New Game");
            Console.WriteLine("  Continue Game");
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
            string userName = Console.ReadLine();
            Console.Clear();
            return userName;

        }
        static void ShowHighScores()//prints out the highscores from a txt file
        {
            Console.Clear();
            HighScoreManager highScoreManager = new HighScoreManager("Test.txt");
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
