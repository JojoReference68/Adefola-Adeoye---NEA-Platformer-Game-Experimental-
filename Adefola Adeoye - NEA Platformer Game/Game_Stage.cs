﻿using System;
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
using System.Text.RegularExpressions;

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
        private string playerusername;
        private int totalScore;
        private HighScoreManager highScoreManager;


        public Game_Stage()
        {
            highScoreManager = new HighScoreManager();
            currentLevelIndex = 0; // Start with the first level
            heightmultiplier = randomizer.Next(20, 30);
            terrainChar = '█';
            totalScore = 0;
            Console.SetWindowSize(width, gameMapHeight);

            levels = new List<Level>();
            Level level1 = CreateLevel1();
            levels.Add(level1);

            Level level2 = CreateLevel2();
            levels.Add(level2);

            Level level3 = CreateLevel3();
            levels.Add(level3);

            


        }

        private Level CreateLevel1()
        {
            int heightMultiplier1 = heightmultiplier;
            int Octaves1 = randomizer.Next(octaves, 11);
            // Customize and create the first level here
            return new Level(width, gameMapHeight, heightMultiplier1, terrainChar, persistence, Octaves1);
        }

        private Level CreateLevel2()
        {
            int heightMultiplier2 = heightmultiplier;
            int Octaves2 = randomizer.Next(octaves, 11);
            // Customize and create the second level here
            return new Level(width, gameMapHeight, heightMultiplier2, terrainChar, persistence, Octaves2);
        }

        private Level CreateLevel3()
        {
            int heightMultiplier3 = heightmultiplier;
            int Octaves3 = randomizer.Next(octaves, 11);
            // Customize and create the third level here
            return new Level(width, gameMapHeight, heightMultiplier3, terrainChar, persistence, Octaves3);
        }

        private void AddNewHighScore()
        {
            highScoreManager.ReadHighScoresFromFile();
            highScoreManager.AddHighScore(playerusername, totalScore);
        }
        public void SwitchToNextLevel()
        {
            // Switch to the next level
            currentLevelIndex++;
            if (currentLevelIndex >= levels.Count)
            {
                //handle what happens when all levels are completed here
                VictoryMessage();

            }
            else
            {
                // Load the new level
                LoadCurrentLevel();
            }
        }

        private void VictoryMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("       _      _                   \r\n      (_)    | |                  \r\n__   ___  ___| |_ ___  _ __ _   _ \r\n\\ \\ / / |/ __| __/ _ \\| '__| | | |\r\n \\ V /| | (__| || (_) | |  | |_| |\r\n  \\_/ |_|\\___|\\__\\___/|_|   \\__, |\r\n                             __/ |\r\n                            |___/ \r\n");
            Console.ReadKey(true);
            Console.Clear();
        }



        private void LoadCurrentLevel()
        {
            // Load the current level from the list
            Level currentLevel = levels[currentLevelIndex];

            //level loading/initialization logic can goes here
            currentLevel.SetScore(totalScore);
            currentLevel.LevelIntro(currentLevelIndex+1);
            currentLevel.LevelSetUp();
            currentLevel.BeginGame();
            totalScore = currentLevel.GetScore();
        }

        public void BeginGame()
        {
            GameIntro();
            LoadCurrentLevel();
            SwitchToNextLevel();
            SwitchToNextLevel();
            AddNewHighScore();
        }


        public void GameIntro()
        {
            Console.Clear();
            playerusername = GetUsername();
            Console.WriteLine($"Hello {playerusername} welcome! I'd say good luck but this isn't much of a challenge unless you're challenged.");
            Console.ReadKey(true);
            Console.Clear();
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
