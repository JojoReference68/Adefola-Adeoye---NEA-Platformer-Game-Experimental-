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
        protected string fileName = "Test.txt";
        private string filePath = "C:/Users/Adefola/Documents/GitHub/NEA---Platformer-Game/Adefola Adeoye - NEA Platformer Game/Adefola Adeoye - NEA Platformer Game/bin/Debug/";

        public HighScoreManager(string IfileName)
        {
            HighScoresList = new List<HighScoreEntry>();
            fileName = IfileName;
            filePath = filePath + fileName;
        }
        public void ReadHighScoresFromFile()
        {


            if (IsFileEmpty(filePath) == false)
            {
                try
                {

                    string[] lines = File.ReadAllLines(filePath);
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

                    Console.WriteLine("An error has occured");
                }


            }

        }

        public bool IsFileEmpty(string xfilePath)
        {
            FileInfo fileInfo = new FileInfo(xfilePath);
            return fileInfo.Length == 0;        //checks if the file is empty
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
                Console.Write("No Highscores were found");
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
