using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    public class Tutorial_Stage //stage object for the tutorial of the game
    {
        protected int terminalVelocity;
        protected List<Platform> platforms = new List<Platform>();
        protected char PlatformChar;
        protected Player player;
        protected int width;
        protected int height;
        protected char[,] map;
        protected char empty = ' ';
        public Tutorial_Stage(Player Iplayer)   //Initializes the Tutorial Stage
        {
            terminalVelocity = 7;
            player = Iplayer;

        }

        public void ShowTutorial()  //gives the player information on how to play the game.
        {
            Console.Clear();
            Console.WriteLine($"Hello {player.GetName()}.");
            Console.WriteLine("Welcome to the tutorial where you can learn and practice the basics of the game.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("There are three movement controls in this game:");
            Console.WriteLine("(1). The right arrow key moves your character to the right.");
            Console.WriteLine("(2). The left arrow key moves your character to the left.");
            Console.WriteLine("(3). The up arrow key moves your character upwards.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("One last thing! The aim of this game is to get to end of it, this will require you to combine controls and make your way through the level to progress.");
            Console.WriteLine("If you understand press any key.");
            LoadingSequence();
            Console.Clear();

            Console.Clear();
        }

        private static void LoadingSequence()
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
        private static void LoadingExitSequence()
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


        public void GenerateTutorialMap()
        {
            Console.SetWindowSize(205, 48);
            width = Console.WindowWidth;
            height = Console.WindowHeight;

            map = new char[width, height];
            for (int row = 0; row < width; row++) //makes an empty map covering the entire console.
            {
                for (int col = 0; col < height; col++)
                {
                    map[row, col] = empty;
                }
            }
            GeneratePlatforms();
            SpawnPlayer();
        }

        public void SpawnPlayer()
        {
            player.SetPos(platforms[0].getX(), platforms[0].getY() - 1);
        }

        public void DisplayMap()       //Shows the onto the console
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            for (int col = 0; col < cols; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    Console.Write(map[row, col]);
                }
                Console.WriteLine();
            }
            player.Show(map);
        }

        public void GeneratePlatforms() //Adds these platforms to the map
        {
            InitializePlatforms();
            foreach (Platform platform in platforms)
            {
                platform.AddPlatform(map, height, width);
            }
            PlatformChar = platforms[0].getChar();
        }

        private void InitializePlatforms()   //Adds these platforms to an array of platform objects
        {
            platforms.Add(new Platform(2, 10, 5, 45));
            platforms.Add(new Platform(2, 10, 25, 40));
            platforms.Add(new Platform(2, 10, 45, 35));
            platforms.Add(new Platform(2, 10, 65, 30));
            platforms.Add(new Platform(2, 10, 85, 25));
            platforms.Add(new Platform(2, 10, 105, 20));
            platforms.Add(new Platform(2, 10, 125, 15));
            platforms.Add(new Platform(2, 10, 145, 10));
            platforms.Add(new Platform(2, 10, 165, 5));
        }
        public void BeginGame() //runs all the game logic in the tutorial
        {
            int lastPlatformIndex = platforms.Count - 1;
            bool quitGame = false;

            while (quitGame == false)
            {
                if (Console.KeyAvailable == true)
                {
                    ConsoleKeyInfo input = Console.ReadKey(true);
                    HandleInput(input);
                }
                player.Delete(map);
                HandleGravity();
                if (CheckTouchingPlatform() == true)   //Ensures the player does not fall through the platform
                {
                    player.SetInitialVelocity(0.0);
                }
                player.Show(map);

                if (player.GetPosY() == platforms[lastPlatformIndex].getY() - 1 && player.GetPosX() >= platforms[lastPlatformIndex].getX() && player.GetPosX() <= platforms[lastPlatformIndex].getX() + platforms[lastPlatformIndex].getWidth() - 1)
                {
                    Console.Clear();
                    Console.WriteLine("Congratulations! You've completed the tutorial!");
                    Console.WriteLine("Please enter any key to exit.");
                    System.Threading.Thread.Sleep(1000);
                    Console.ReadKey(true);
                    quitGame = true;
                }




                System.Threading.Thread.Sleep(50); //Makes sure the game logic is not running too quickly
            }

        }
        private void HandleInput(ConsoleKeyInfo input)   //Handles the player controls and the right response
        {
            if (input.Key == ConsoleKey.LeftArrow && player.GetPosX() > 0)
            {
                if (player.GetPosX() > 0 && CheckCollisionLeft() == false)
                {
                    player.MoveLeft(map);
                }

            }
            else if (input.Key == ConsoleKey.RightArrow && player.GetPosX() < width - 1)
            {
                if (player.GetPosX() < width - 1 && CheckCollisionRight() == false)
                {
                    player.MoveRight(map);
                }

            }
            else if (input.Key == ConsoleKey.UpArrow)
            {
                if (player.GetPosY() <= height - 1)
                {
                    player.MoveUp();
                }
                else if (map[player.GetPosX(), player.GetPosY() + 1] == PlatformChar)
                {
                    player.MoveUp();
                }
            }
        }
        public bool CheckTouchingPlatform()    //Checks if the player and platform are touching
        {
            // Check if player's position is within map bounds
            if (player.GetPosX() >= 0 && player.GetPosX() < width && player.GetPosY() >= 0 && player.GetPosY() < height - 1)
            {
                return map[player.GetPosX(), player.GetPosY() + 1] == PlatformChar;
            }
            return false;
        }

        protected bool CheckCollisionLeft()
        {
            return map[player.GetPosX() - 1, player.GetPosY()] == PlatformChar;
        }

        protected bool CheckCollisionRight()
        {
            return map[player.GetPosX() + 1, player.GetPosY()] == PlatformChar;
        }
        public bool CheckCollisionUp(Player player, char[,] map)
        {
            int newXPos = player.GetPosX(); // Get the player's current X position
            int newYPos = player.GetPosY() - 1; // Calculate the Y position one step above the player

            // Check if the new Y position is within the map bounds
            if (newYPos >= 0 && newYPos < map.GetLength(1))
            {
                // Check if the player's new position collides with the platform from below
                return map[newXPos, newYPos] == '=';
            }

            return false;
        }
        protected void HandleGravity()
        {
            double deltaTime = 0.1;
            double velocity = player.GetInitialVelocity() + (player.GetAcceleration() * deltaTime);

            if (velocity >= terminalVelocity)
            {
                velocity = terminalVelocity;
            }

            int newYPos = player.GetPosY() + (int)Math.Round(velocity * deltaTime);

            if (newYPos < 0)
            {
                newYPos = 0;
                velocity = 0; // Reset velocity when hitting the top of the screen
            }
            else if (CheckCollisionUp(player, map)) // Check collision with platform from below
            {
                // Set the player's position just above the platform's top
                newYPos = platforms[0].getY() - 1;
                velocity = 0; // Reset velocity to prevent passing through the platform

                // Reset initial velocity to allow falling down
                player.SetInitialVelocity(0.0);
            }
            else if (newYPos >= height - 1)
            {
                newYPos = height - 1;
                velocity = 0; // Reset velocity if player hits the bottom of the screen
            }

            player.SetPosY(newYPos);
            player.SetInitialVelocity(velocity);
        }
    }
}
