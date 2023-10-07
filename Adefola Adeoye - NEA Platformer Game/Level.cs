using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    public class Level
    {
        private int width;
        private int gameMapHeight;
        private Terrain_Generator terrainGenerator;
        private char[,] gameMap;
        private float[] terrain;
        private int heightmultiplier;
        private char terrainChar;
        private float persistence;
        private int octaves;
        private Player player;
        private int terminalVelocity;
        private int minTerrainHeight; // Minimum height for the terrain
        private bool isMusicPlaying;


        public Level(int Width, int GameMapHeight, int HeightMultiplier, char TerrainChar, float Persistence, int Octaves, Player Player )
        {
            width = Width;
            gameMapHeight = GameMapHeight;
            heightmultiplier = HeightMultiplier;
            terrainChar = TerrainChar;
            persistence = Persistence;
            octaves = Octaves;
            player = Player;
            minTerrainHeight = 1;
            Console.SetWindowSize(width, gameMapHeight);

            // Initialize the player
            player = new Player("PlayerName", 0, gameMapHeight - heightmultiplier, 0, 0);

            // Initialize the terrain generator and generate the game world
            terrainGenerator = new Terrain_Generator(width, persistence, octaves);
            terrain = terrainGenerator.GeneratePerlinNoise();

            // Initialize the game map based on terrain
            gameMap = new char[width, gameMapHeight];
            GenerateGameWorld(); // You can customize this method
        }

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

        public void BeginGame()
        {
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

        // Other level-specific methods and logic
        public void LevelSetUp()
        {
            GenerateGameWorld();
            InitializePlayer();
            DisplayGameWorld();
        }

        public void LevelIntro(int CurrentLevel)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to Level {CurrentLevel}");
            Console.WriteLine("Cue the music.");
            LoadingSequence();
            Console.Clear();
        }

        private void InitializePlayer()
        {
            Random rnd = new Random();
            int randomX = rnd.Next(0, 6);
            int playerY = gameMapHeight - heightmultiplier;
            player = new Player("PlayerName", randomX, playerY, 0, 0); // Adjust player's initial velocity and displacement as needed
            player.Show(gameMap);
        }

        private void LoadingSequence()
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
