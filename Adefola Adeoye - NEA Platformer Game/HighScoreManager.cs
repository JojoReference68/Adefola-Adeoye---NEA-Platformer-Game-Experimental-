using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.IO;
using System.Security.Cryptography;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    public class HighScoreManager
    {
        private List<HighScoreEntry> HighScoresList;
        protected string fileName = "HighScores.txt";
        private string filePath = AppDomain.CurrentDomain.BaseDirectory;

        public HighScoreManager()
        {
            HighScoresList = new List<HighScoreEntry>();
            filePath = Path.Combine(filePath, fileName);
            // Checks if the high scores file exists, and create it if it doesn't
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) { } // This line creates the file
            }

        }

        public void AddHighScore(string playerName, int score)
        {
            // Create a new high score entry
            HighScoreEntry entry = new HighScoreEntry(playerName, score);
            HighScoresList.Add(entry);
            SortDescending();

            // If the list has more than 20 entries, remove the extras
            if (HighScoresList.Count > 20)
            {
                HighScoresList = HighScoresList.Take(20).ToList();
            }
        }
        public void ReadHighScoresFromFile() // fetches data from a text file
        {
            filePath = Path.Combine(filePath, fileName);
            if (!File.Exists(filePath))
            {
                Console.WriteLine("High scores file not found.");
                return; // Exit the method if the file doesn't exist.
            }

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length == 0)
                {
                    return; // Exit the method if the file is empty.
                }

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    parts[0] = parts[0].Trim();
                    parts[1] = parts[1].Trim();
                    HighScoresList.Add(new HighScoreEntry(parts[0], int.Parse(parts[1])));
                }
            }
            catch (Exception)
            {
                Console.WriteLine("An error has occurred while reading high scores.");
            }
        }

        public void SaveHighScoresToFile()
        {
            try
            {
                SortDescending();

                // Convert the high scores to a list of strings in the format "Username,Score"
                List<string> highScoreStrings = HighScoresList.Select(entry => $"{entry.getUsername()},{entry.getScore()}").ToList();

                // Write the high scores to the file
                File.WriteAllLines(filePath, highScoreStrings);

                Console.WriteLine("High scores saved successfully.");
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred while saving high scores.");
            }
        }

        public bool IsFileEmpty(string xfilePath)
        {
            xfilePath = Path.Combine(filePath, fileName);
            FileInfo fileInfo = new FileInfo(xfilePath);
            return fileInfo.Length == 0;         //checks if the file is empty
        }

        public void PrintHighScores()
        {
            Console.Clear();
            if (HighScoresList.Count > 0)
            {
                Console.Write("HighScores:\n\n");

                foreach (HighScoreEntry scoreEntry in HighScoresList)
                {
                    Console.WriteLine(scoreEntry.getUsername() + " : " + scoreEntry.getScore());
                }
            }
            else
            {
                Console.WriteLine("No high scores found. Be the first to set a high score!");
            }
        }
        public void SortDescending() //abstraction
        {
            MergeSortHighScoresDescending(HighScoresList, 0, HighScoresList.Count - 1);
        }
        private void MergeSortHighScoresDescending(List<HighScoreEntry> list, int left, int right)// MergeSort function: Sorts an array using Merge Sort
        {
            if (left < right)
            {
                int middle = (left + right) / 2;
                MergeSortHighScoresDescending(list, left, middle); //Sort the first half
                MergeSortHighScoresDescending(list, middle + 1, right); //Sort the second half
                Merge(list, left, middle, right); //Merge the two halves
            }
        }

        private void Merge(List<HighScoreEntry> list, int left, int middle, int right)
        {
            // Find sizes of two subarrays to be merged
            int sizeLeft = middle - left + 1;
            int sizeRight = right - middle;

            //Create temporary arrays to hold the data
            List<HighScoreEntry> leftArray = new List<HighScoreEntry>(sizeLeft);
            List<HighScoreEntry> rightArray = new List<HighScoreEntry>(sizeRight);

            //Copy data into the arrays
            for (int i = 0; i < sizeLeft; i++)
                leftArray.Add(list[left + i]);

            for (int i = 0; i < sizeRight; i++)
                rightArray.Add(list[middle + 1 + i]);

            // Merge the temporary arrays back into the original array
            int leftIndex = 0;  // Index of the first subarray
            int rightIndex = 0; // Index of the second subarray
            int mergedIndex = left; // Index of the merged subarray

            while (leftIndex < sizeLeft && rightIndex < sizeRight)
            {
                if (leftArray[leftIndex].getScore() >= rightArray[rightIndex].getScore())
                {
                    list[mergedIndex] = leftArray[leftIndex];
                    leftIndex++;
                }
                else
                {
                    list[mergedIndex] = rightArray[rightIndex];
                    rightIndex++;
                }
                mergedIndex++;
            }

            while (leftIndex < sizeLeft)
            {
                list[mergedIndex] = leftArray[leftIndex];
                leftIndex++;
                mergedIndex++;
            }

            while (rightIndex < sizeRight)
            {
                list[mergedIndex] = rightArray[rightIndex];
                rightIndex++;
                mergedIndex++;
            }
        }
    }
}
