using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using System.Xml.Linq;

namespace API_Sixth_Lab
{
    public partial class MainWindow : Window
    {
        public class WeatherData
        {
            public List<HourlyWeather> Hourly { get; set; }
        }

        public class HourlyWeather
        {
            public long Dt { get; set; }
            public double Temp { get; set; }
        }
        
        private static string ApiKey = ("");
        public SeriesCollection TemperatureValues { get; set; }
        public List<string> TimeLabels { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            TemperatureValues = new SeriesCollection();
            TimeLabels = new List<string>();
        }

        private async void GetWeatherButton_Click(object sender, RoutedEventArgs e)
        {
            string latitude = LatitudeTextBox.Text;
            string longitude = LongitudeTextBox.Text;

            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/onecall?lat={latitude}&lon={longitude}&units=metric&lang=ua&appid={ApiKey}";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(response);
                    UpdateChart(weatherData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void UpdateChart(WeatherData weatherData)
        {
            var tempValues = new ChartValues<double>();
            var timeLabels = new List<string>();

            foreach (var hourly in weatherData.Hourly)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(hourly.Dt);
                tempValues.Add(hourly.Temp);
                timeLabels.Add(dateTimeOffset.ToString("HH:mm"));
            }

            TemperatureValues.Clear();
            TemperatureValues.Add(new LineSeries
            {
                Title = "Temperature",
                Values = tempValues
            });

            TimeLabels.Clear();
            TimeLabels.AddRange(timeLabels);
        }
    }
}
