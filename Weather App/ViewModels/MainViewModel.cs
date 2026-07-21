using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input; 
using Weather_App.Models;

namespace Weather_App.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private static readonly HttpClient client = new HttpClient();
        public event PropertyChangedEventHandler? PropertyChanged;

        private string apiKey = "a47c465cff916aa7ad081df5ebda8127";
        private string apiUrl = "https://api.openweathermap.org/data/2.5/weather?units=metric&q=";

        private string _cityName = "---";
        private string _temperature = "--°C";
        private string _humidity = "--%";
        private string _windSpeed = "-- km/h";
        private string _countryName = "---";
        private string _searchQuery = ""; 

        public string CityName
        {
            get => _cityName;
            set { _cityName = value; OnPropertyChanged(); }
        }

        public string CountryName
        {
            get => _countryName;
            set { _countryName = value; OnPropertyChanged(); }
        }

        public string Temperature
        {
            get => _temperature;
            set { _temperature = value; OnPropertyChanged(); }
        }

        public string Humidity
        {
            get => _humidity;
            set { _humidity = value; OnPropertyChanged(); }
        }

        public string WindSpeed
        {
            get => _windSpeed;
            set { _windSpeed = value; OnPropertyChanged(); }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(); }
        }

        public ICommand SearchCommand { get; }

        public MainViewModel()
        {
            SearchCommand = new RelayCommand(() =>
            {
                if (!string.IsNullOrWhiteSpace(SearchQuery))
                {
                    GetJsonAsync(SearchQuery);
                }
            });
        }

        public async Task GetJsonAsync(string city)
        {
            string url = $"{apiUrl}{city}&appid={apiKey}";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<WeatherResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (data != null)
                    {
                        CityName = data.Name;
                        CountryName = data.Sys?.Country ?? "---";
                        Temperature = $"{Math.Round(data.Main.Temp)}°C";
                        Humidity = $"{data.Main.Humidity}%";

                        WindSpeed = $"{data.Wind.WindSpeed} km/h";
                    }
                }
                else
                {
                    MessageBox.Show("City doesnt find", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}