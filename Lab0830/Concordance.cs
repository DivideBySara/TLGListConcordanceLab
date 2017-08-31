using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Console;

/*
 * Sara Jade
 * 8/30/17
 * 
 * class Concordance has been updated with the following features:
 * 1) using static System.Console;
 * 2) A method to show all words
 * 3) A method that creates a word list with excludedWords removed
 * 
 * TODO: a method adds distinct words to a set.
 * 
 */ 

namespace Lab0830
{
    class Concordance
    {
        static string inPath = string.Empty;
        static string outPath = string.Empty;
        static string excludedWordsPath = string.Empty;
        static string inputText = string.Empty;
        static string outputText = string.Empty;
        static string[] excludedWords;

        static void Main(string[] args)
        {

            Analyzer anlz = new Analyzer();
            List<string> words = new List<string>();
            SortedSet<string> distinctSortedWords = new SortedSet<string>();

            // Set up paths from args
            if (args.Length != 3)
            {
                Console.WriteLine("Invalid number of path specifications");
                return;
            }
            GetPaths(args);
            // Get the input data
            ReadInputs();
            
            // Identify paragraphs and sentences
            // Identify words and their location
            
            anlz.Analyze(inputText);

            // TODO: Display an alphabetical list of distinct words to the console. 
           
            CreateWordList(anlz.paragraphs, excludedWords, words);
            CreateDistinctWordList(words, distinctSortedWords);
            ShowWords(distinctSortedWords);

            ReadKey();
        } // End Main()

        private static void CreateDistinctWordList(List<string> words, SortedSet<string> distinctSortedWords)
        {
            foreach (string word in words)
            {
                distinctSortedWords.Add(word);
            }
        }

        // CreateWordList() creates a List<string> of words to analyze.
        // It first removes the excludedWords.
        private static void CreateWordList(List<Paragraph> paragraphs, string[] excludedWords, List<string> words)
        {           
            foreach (Paragraph paragraph in paragraphs)
            {
                foreach (Sentence sentence in paragraph.sentences)
                {
                    foreach (Wordref wordref in sentence.words)
                    {
                        if (!excludedWords.Contains<string>(wordref.word))
                        {
                            words.Add(wordref.word);
                        } // else don't add the word                  
                    }
                }
            }
        }
        
        private static void ShowWords(IEnumerable<string> words)
        {
            foreach (string word in words)
            {
                Write(word + " ");
            }
        }

        static void ReadInputs()
        {
            try
            {
                inputText = File.ReadAllText(inPath);
                excludedWords = File.ReadAllLines(excludedWordsPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading file " + e.Message);
            }
        }

        static void GetPaths(string[] args)
        {
            // Get the path to the input text file
            try
            {
                inPath = Path.GetFullPath(args[0]);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Invalid text input path {args[0]}");
            }

            // Get the path for the output file
            try
            {
                outPath = Path.GetFullPath(args[1]);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Invalid text output path {args[1]}");
            }

            // Get the path to the excluded words file
            try
            {
                excludedWordsPath = Path.GetFullPath(args[2]);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Invalid excluded words input path {args[1]}");
            }
        }

    }
}
