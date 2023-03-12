using Plugin.Media;
using Plugin.Media.Abstractions;
using RecognizeDog.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace RecognizeDog
{
    public partial class MainPage : ContentPage
    {
        private static MainPage page;
        public static string imagesPath;
        public MainPage()
        {
            InitializeComponent();
            page = this;
            LoadTreeViewItems();
        }
        public void LoadTreeViewItems()
        {
            ImagesListView.Intent = TableIntent.Form;
            var tableRoot = new TableRoot();
            var tableSection = new TableSection();
            ImagesListView.Root = tableRoot;
            tableRoot.Add(tableSection);
            List<BreedImageDTO> breeds = JsonManager.GetImagesBreedInfo();
            string[] images = Directory.GetFiles(imagesPath);
            for(int i= 1;i<images.Length;i++)
            {
                string[] splited = images[i].Split('/');
                string text = splited[splited.Length-1];
                string breed = breeds.Where(p => p.image == text).Select(p => p.breed).FirstOrDefault();
                ImageCell cell = new ImageCell
                {
                    // Some differences with loading images in initial release.
                    ImageSource = ImageSource.FromFile(images[i]),
                    Text = text,
                    Detail = string.IsNullOrEmpty(breed)? "Nie wykonano testu" : breed,
                    CommandParameter = new Tuple<string, string>(text, string.IsNullOrEmpty(breed) ? "Nie wykonano testu" : breed),
                    Command = new Command(OnListItemTap),
                };
                tableSection.Add(cell);
            }
        }
        public async void OnListItemTap(object param)
        {
            Tuple<string, string> image = (Tuple<string, string>)param;
            var detailPage = new ModalPage($"{imagesPath}/{image.Item1}", image.Item1, image.Item2, this);
            detailPage.address = inputAddress.Text;
            await Navigation.PushModalAsync(detailPage);
        }
        public void OnCameraButtonClicked(object sender, EventArgs args)
        {
            _ = TakePhoto();
        }
        public static void DisplayAlert(string msg)
        {
            MainPage.page.DisplayAlert("", msg, "OK");
        }
        private async Task TakePhoto()
        {
            var cameraMediaOptions = new StoreCameraMediaOptions
            {
                DefaultCamera = CameraDevice.Rear,
                SaveToAlbum = true,
                Directory = "RecognizeDog",
                Name = null,
                CompressionQuality = 100
            };
            MediaFile photo = await CrossMedia.Current.TakePhotoAsync(cameraMediaOptions);
            if (photo == null) return;

            LoadTreeViewItems();
        }
    }
}
