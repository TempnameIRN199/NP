using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace HTTP_Fifth_hw
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            string url = UrlTextBox.Text;
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Please enter a valid URL.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string content = GetWebContent(url);
                var wordFrequency = GetWordFrequency(content);
                DisplayWordFrequency(wordFrequency);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetWebContent(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        private Dictionary<string, int> GetWordFrequency(string content)
        {
            string text = Regex.Replace(content, "<.*?>", string.Empty);
            string[] words = text.Split(new[] { ' ', '\r', '\n', '\t', '.', ',', ';', ':', '-', '_', '/', '\\', '!', '?', '(', ')', '[', ']', '{', '}', '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> wordFrequency = new Dictionary<string, int>();

            foreach (string word in words)
            {
                string lowerWord = word.ToLower();
                if (wordFrequency.ContainsKey(lowerWord))
                {
                    wordFrequency[lowerWord]++;
                }
                else
                {
                    wordFrequency[lowerWord] = 1;
                }
            }

            return wordFrequency;
        }

        private void DisplayWordFrequency(Dictionary<string, int> wordFrequency)
        {
            ResultTextBox.Clear();
            var sortedWordFrequency = wordFrequency.OrderByDescending(pair => pair.Value);

            foreach (var pair in sortedWordFrequency)
            {
                ResultTextBox.AppendText($"{pair.Key}: {pair.Value}\n");
            }
        }
    }
}
