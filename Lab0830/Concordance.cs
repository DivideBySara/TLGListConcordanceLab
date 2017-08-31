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
 * 4) A method that adds words and counts to a SortedDictionary<string, int>
 * 5) A method that starts the word count!
 * 6) A method that displays word counts in descending order of appearance
 * 
 * TODO: a method that replaces the first list's appearance count with a list of locations
 * identified by paragraph # and sentence # within the paragraph.
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
            SortedDictionary<string, int> wordCounts = new SortedDictionary<string, int>();

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

            // DONE: Display an alphabetical list of distinct words to the console
            // along with number of times it appears in the document
            // DONE: Display wordCounts in descending order of appearance count
            StartWordList(anlz.paragraphs, excludedWords, words, wordCounts);        
            
            ReadKey();
        } // End Main()

        private static void StartWordList(List<Paragraph> paragraphs, string[] excludedWords, 
            List<string> words, SortedDictionary<string, int> wordCounts)
        {
            CreateWordList(paragraphs, excludedWords, words);
            CreateWordCounts(words, wordCounts);
            ShowWordCounts(wordCounts);
            ShowWordCountsDescOrderOfAppearance(wordCounts);
        }

        private static void ShowWordCountsDescOrderOfAppearance
            (SortedDictionary<string, int> wordCounts)
        {
            WriteLine("\nWord counts in descending order of appearance: ");
            foreach (KeyValuePair<string, int> wordCount 
                in wordCounts.OrderByDescending(key => key.Value))
            {
                WriteLine($"{wordCount.Key}: {wordCount.Value}");
            }
        }

        private static void ShowWordCounts(SortedDictionary<string, int> wordCounts)
        {
            WriteLine("\nWord counts sorted alphabetically by word:");
            foreach (KeyValuePair<string, int> wordCount in wordCounts)
            {
                WriteLine($"{wordCount.Key}: {wordCount.Value}");
            }
        }

        private static void CreateWordCounts
            (List<string> words, SortedDictionary<string, int> wordCounts)
        {
            foreach (string word in words)
            {
                if (!wordCounts.ContainsKey(word))
                {
                    wordCounts.Add(word, 1);
                }
                else
                {
                    ++wordCounts[word];
                }
            }
        }

        // CreateWordList() creates a List<string> of words to analyze.
        // It first removes the excludedWords.
        private static void CreateWordList
            (List<Paragraph> paragraphs, string[] excludedWords, List<string> words)
        {           
            foreach (Paragraph paragraph in paragraphs)
            {
                foreach (Sentence sentence in paragraph.sentences)
                {
                    foreach (Wordref wordref in sentence.words)
                    {
                        if (!excludedWords.Contains(wordref.word) 
                            && wordref.word != string.Empty)
                        {
                            words.Add(wordref.word);
                        } // else don't add the word                  
                    }
                }
            }
        }

        private static void ShowWords(IEnumerable<string> words)
        {
            WriteLine("List of words in document: ");

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
