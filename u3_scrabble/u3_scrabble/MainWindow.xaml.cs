/**
 * Nolan Meehan
 * April 20, 2018
 * gets words that are able to be made with the hand.
 * 
 */
 using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Net;

namespace u3_scrabble
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ScrabbleGame sg = new ScrabbleGame();
            string hand = sg.drawInitialTiles();
            MessageBox.Show(hand);
            string[] words = getWords();
            string[] newWords = checkWords(words, hand.ToCharArray());
            StreamWriter sw = new StreamWriter("makable words.txt");
            foreach(string word in newWords)
            {
                sw.WriteLine(word);
            }
            sw.Close();
            Process.Start("notepad.exe", "makable words");
        }


        /// <summary>
        /// gets words that are able to be made with the tiles passed.
        /// </summary>
        /// <param name="words">the dictionary</param>
        /// <param name="tiles"></param>
        /// <returns></returns>
        private string[] checkWords(string[] words, char[] tiles)
        {
            int temp = 0;
            int[] letters = new int[26];
            int blanks = 0;
            char[] wordChecking;
            string highScore="";
            int highestScore=0;
            List<string> s = new List<string>();
            for (int i = 0; i < letters.Length; i++)
            {
                letters[i] = 0;
            }
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].Equals(' ')) blanks++;
                else letters[tiles[i] - 65]++;
            }
            foreach (string word in words)
            {
                wordChecking = word.ToCharArray();
                int[] wordCheckingChars = new int[26];
                for (int i = 0; i < wordCheckingChars.Length; i++) wordCheckingChars[i] = 0;
                for (int i = 0; i < wordChecking.Length; i++)
                {
                    wordCheckingChars[wordChecking[i] - 65]++;
                }
                int extraLetters = 0;
                for (int i = 0; i < wordCheckingChars.Length; i++)
                {
                    if (wordCheckingChars[i] - letters[i] > 0) extraLetters += wordCheckingChars[i] - letters[i];
                    if(wordCheckingChars[i]>0) temp += new ScrabbleLetter(Convert.ToChar(i+65)).Points;
                }
                if (extraLetters <= blanks)
                {
                    s.Add(word);
                    if (temp > highestScore)
                    {
                        highestScore = temp;
                        highScore = word;
                    }
                }
                temp = 0;
            }
            MessageBox.Show("best word: " + highScore);
            return s.ToArray();
        }

        /// <summary>
        /// filters out all words over 7 letters, as well as the headers for the different letters.
        /// </summary>
        /// <returns></returns>
        private string[] getWords()
        {
            List<string> a = new List<string>();
            StreamReader sr = new StreamReader(new WebClient().OpenRead("http://darcy.rsgc.on.ca/ACES/ICS4U/SourceCode/Words.txt"));
            string t;
            while (!sr.EndOfStream)
            {
                t = sr.ReadLine();
                if (t.Length <= 7)
                {
                    if (t.Length == 1 && t.ToCharArray()[0] <= 90 && t.ToCharArray()[0] >= 65) continue;
                    a.Add(t.ToUpper());
                }
            }
            sr.Close();
            return a.ToArray();
        }

    }
}