using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Threading;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    internal class Game_Stage
    {
        private int width = 205;
        private int gameMapHeight = 48; // Set the height of your game world
        private int minTerrainHeight = 1; // Minimum height for the terrain
        private int heightmultiplier;
        //Maximum height for the terrain
        private float persistence = 0.5f;
        private int octaves = 5;
        private Terrain_Generator terrainGenerator;
        private float[] terrain;
        private char[,] gameMap; // 2D array to represent the game world
        private Player player;
        private Random randomizer = new Random();
        private int terminalVelocity;
        private char terrainChar;
        bool isMusicPlaying;
        private List<Level> levels; // List to store different levels
        private int currentLevelIndex; // Index of the current level


        public Game_Stage()
        {
            levels = new List<Level>();
            Level level1 = CreateLevel1();
            levels.Add(level1);

            Level level2 = CreateLevel2();
            levels.Add(level2);

            Level level3 = CreateLevel3();
            levels.Add(level3);

            currentLevelIndex = 0; // Start with the first level

            Console.SetWindowSize(width, gameMapHeight);
            terrainGenerator = new Terrain_Generator(width, persistence, octaves);
            terrain = terrainGenerator.GeneratePerlinNoise();
            gameMap = new char[width, gameMapHeight]; // Initialize the game map array
            heightmultiplier = randomizer.Next(20, 30);
            terminalVelocity = 7;
            terrainChar = '█';
            player = new Player(" ", 0, 0, 0, 0);

        }

        private Level CreateLevel1()
        {
            int heightMultiplier1 = heightmultiplier;
            // Customize and create the first level here
            return new Level(width, gameMapHeight, heightMultiplier1, terrainChar, persistence, octaves, player);
        }

        private Level CreateLevel2()
        {
            int heightMultiplier2 = heightmultiplier + 5;
            // Customize and create the second level here
            return new Level(width, gameMapHeight, heightMultiplier2, terrainChar, persistence, octaves, player);
        }

        private Level CreateLevel3()
        {
            int heightMultiplier3 = heightmultiplier + 10;
            // Customize and create the third level here
            return new Level(width, gameMapHeight, heightMultiplier3, terrainChar, persistence, octaves, player );
        }


        public void SwitchToNextLevel()
        {
            // Switch to the next level
            currentLevelIndex++;
            if (currentLevelIndex >= levels.Count)
            {
                // You can handle what happens when all levels are completed here
                // For example, end the game or show a victory message
            }
            else
            {
                // Load the new level
                LoadCurrentLevel();
            }
        }

        private void LoadCurrentLevel()
        {
            // Load the current level from the list
            Level currentLevel = levels[currentLevelIndex];

            // Other level loading and initialization logic can go here

            // Example: Display level intro
            currentLevel.LevelIntro(currentLevelIndex+1);

            // Example: Generate the game world for the current level
            currentLevel.LevelSetUp();

            // Example: Initialize the player for the current level
        }

        //Initialize Player object
        private void InitializePlayer()
        {
            Random rnd = new Random();
            int randomX = rnd.Next(0, 6);
            int playerY = gameMapHeight - heightmultiplier;
            player = new Player("PlayerName", randomX, playerY, 0, 0); // Adjust player's initial velocity and displacement as needed
            player.Show(gameMap);
        }
        //Method to populate the game world map with terrain
        private void GenerateGameWorld()
        {
            for (int x = 0; x < width; x++)
            {
                int terrainHeight = (int)(terrain[x] * heightmultiplier); // Adjust the multiplier for terrain height
                for (int y = 0; y < gameMapHeight; y++)
                {
                    if (y >= gameMapHeight - Math.Max(terrainHeight, minTerrainHeight))
                    {
                        gameMap[x, y] = '█'; // Set the map cell to represent terrain
                    }
                    else
                    {
                        gameMap[x, y] = ' '; // Set empty space for non-terrain cells
                    }
                }
            }
        }

        // Display the game world map
        public void DisplayGameWorld()
        {

            int screenHeight = Console.WindowHeight;

            // Calculate the starting Y position for rendering the terrain
            int startY = Math.Max(0, screenHeight - gameMapHeight);

            for (int y = startY; y < screenHeight; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int adjustedY = y - startY; // Adjust the Y coordinate for rendering
                    char mapChar = (adjustedY < gameMapHeight) ? gameMap[x, adjustedY] : ' '; // Use space if outside gameMap

                    Console.SetCursorPosition(x, y);
                    Console.Write(mapChar);
                }
            }

        }

        public void HandleGravity()
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

            player.SetPosY(newYPos);
            player.SetInitialVelocity(velocity);
        }

        public void BeginGame()
        {
            LoadCurrentLevel();
            LevelIntro();
            GenerateGameWorld();
            InitializePlayer();
            DisplayGameWorld();
            bool quitGame = false;

            //seperate thread for music playback
            Thread musicThread = new Thread(PlayBackgroundMusic);
            musicThread.Start();

            while (quitGame == false)
            {
                if (Console.KeyAvailable == true)
                {
                    ConsoleKeyInfo input = Console.ReadKey(true);
                    HandleInput(input);
                }
                player.Delete(gameMap);
                HandleGravity();
                if (CheckTouchingTerrain() == true)
                {
                    player.SetInitialVelocity(0.0);
                }
                player.Show(gameMap);
                System.Threading.Thread.Sleep(50);
            }
            isMusicPlaying = false;
            musicThread.Join();
        }

        public void PlayBackgroundMusic()
        {
            string filepath = "C:\\Users\\Adefola\\Documents\\Projects\\Adefola Adeoye - NEA Platformer Game\\Adefola Adeoye - NEA Platformer Game\\bin\\Debug\\Megalovania.wav";
            //plays megalovania
            SoundPlayer soundPlayer = new SoundPlayer(filepath);
            soundPlayer.Play();

            isMusicPlaying = true;

            while (isMusicPlaying)
            {
                soundPlayer.PlaySync(); // Play the music synchronously
            }
        }

        public void HandleInput(ConsoleKeyInfo input)   //Handles the player controls and the right response
        {
            if (input.Key == ConsoleKey.LeftArrow && player.GetPosX() > 0)
            {
                if (player.GetPosX() > 0 && CheckCollisionLeft() == false)
                {
                    player.MoveLeft(gameMap);
                }

            }
            else if (input.Key == ConsoleKey.RightArrow && player.GetPosX() < width - 1)
            {
                if (player.GetPosX() < width - 1 && CheckCollisionRight() == false)
                {
                    player.MoveRight(gameMap);
                }

            }
            else if (input.Key == ConsoleKey.UpArrow && CheckTouchingTerrain() == true && player.GetPosY() <= gameMapHeight - 1)
            {
                player.MoveUp();
            }
        }

        protected bool CheckCollisionLeft()
        {
            return gameMap[player.GetPosX() - 1, player.GetPosY()] == terrainChar;
        }
        protected bool CheckCollisionRight()
        {
            return gameMap[player.GetPosX() + 1, player.GetPosY()] == terrainChar;
        }

        public bool CheckTouchingTerrain()    //Checks if the player and terrain are touching
        {
            // Check if player's position is within map bounds
            if (player.GetPosX() >= 0 && player.GetPosX() < width && player.GetPosY() >= 0 && player.GetPosY() < gameMapHeight - 1)
            {
                return gameMap[player.GetPosX(), player.GetPosY() + 1] == terrainChar;
            }
            return false;
        }


        public void LevelIntro()
        {
            Console.Clear();
            player.ChangeName(GetUsername());
            Console.WriteLine($"Hello {player.GetName()} welcome! I'd say good luck but this isn't much of a challenge unless you're challenged.");
            Console.WriteLine("Cue the music.");
            Console.ReadKey(true);
            Console.Clear();
        }
        static string GetUsername()
        {
            Console.WriteLine("Enter your username.");
            LoadingSequence();
            string userName = Console.ReadLine();
            Console.Clear();
            return userName;

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

        public Player ReturnPlayer()
        {
            return player;
        }


    }
}
