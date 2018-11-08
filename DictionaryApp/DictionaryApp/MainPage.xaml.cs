using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Plugin.Connectivity;
using Newtonsoft.Json;
using System.Net.Http;


namespace DictionaryApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            CheckInternetConnection();
        }

        public bool CheckInternetConnection()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }

            //If no internet connection
            DisplayAlert("No Internet", "No internet connection detected", "OK");
            return false;
        }

        private void Handle_ProcessWord(object sender, EventArgs e)
        {
           string WordToBeProcessed = WordBoxInput.Text;

            if (!WordToBeProcessed.All(Char.IsLetter))
            {
                ErrorLabel.IsVisible = true;
                return;
            }

           //Convert word to lower case
           ErrorLabel.IsVisible = false;
           WordToBeProcessed = WordToBeProcessed.ToLower();          
           Handle_GetWordDefinition(WordToBeProcessed);
        }

        async void Handle_GetWordDefinition(string WordToBeProcessed)
        {
            var client = new HttpClient();

            //using Owl Api to get word definitions
            var OwlApiAddress = "https://owlbot.info/api/v2/dictionary/" + WordToBeProcessed;
            var uri = new Uri(OwlApiAddress);

            List<Word> wordData = new List<Word>();
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                wordData = JsonConvert.DeserializeObject<List<Word>>(jsonContent);
            }

            WordDefinitionView.ItemsSource = wordData;
        }
    }
}
