using Newtonsoft.Json;
using RecognizeDog.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecognizeDog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModalPage : ContentPage
    {
        private string _imageName;
        private MainPage _mainPage;
        private string base64Image;
        public string address = "";
        public ModalPage(string imagePath, string imageName, string imageBreed, MainPage mainPage)
        {
            InitializeComponent();
            image.Source = ImageSource.FromFile(imagePath);
            base64Image = DecodePicture(imagePath);
            _imageName = imageName;
            _mainPage = mainPage;
            breedLabel.Text = $"Rasa: {imageBreed}";
        }
        private string DecodePicture(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            return Convert.ToBase64String(bytes);
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
        async void OnTestButtonClicked(object sender, EventArgs args)
        {
            string breed = "";
            try
            {
                HttpClient client = new HttpClient();
                RequestParam param = new RequestParam();
                param.picture = base64Image;
                var data = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
                string url = $"{address}/getbreed";
                var response = await client.PostAsync(url, data);
                breed = await response.Content.ReadAsStringAsync();
                breed = breed.Substring(10, breed.Length-10);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            //SPRAWDZANIE RASY
            JsonManager.SaveImageBreed(_imageName, breed);
            _mainPage.LoadTreeViewItems();
            await Navigation.PopModalAsync();
        }
        async void OnEnhanceButtonClicked(object sender, EventArgs args)
        {
            
            //DOUCZENIE SIECI
            _mainPage.LoadTreeViewItems();
            await Navigation.PopModalAsync();
        }
    }
    class RequestParam
    {
        public string picture { get; set; }
    }
}