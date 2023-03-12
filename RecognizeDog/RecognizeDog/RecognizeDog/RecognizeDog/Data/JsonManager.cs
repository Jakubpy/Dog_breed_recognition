using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RecognizeDog.Data
{
    public class JsonManager
    {
        private static string JsonPath;
        public static List<BreedImageDTO> GetImagesBreedInfo()
        {
            try
            {
                if (!File.Exists(JsonPath))
                {
                    CreateFile(JsonPath);
                }
                string serialized = File.ReadAllText(JsonPath);
                return JsonConvert.DeserializeObject<List<BreedImageDTO>>(serialized);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<BreedImageDTO>();
            }
        }
        public static bool SaveImageBreed(string image, string breed)
        {
            try
            {
                if (!File.Exists(JsonPath))
                {
                    CreateFile(JsonPath);
                }
                string serialized = File.ReadAllText(JsonPath);
                List<BreedImageDTO>  breeds = JsonConvert.DeserializeObject<List<BreedImageDTO>>(serialized);
                BreedImageDTO newBreed = new BreedImageDTO();
                newBreed.breed = breed;
                newBreed.image = image;
                breeds.Add(newBreed);
                File.WriteAllText(JsonPath, JsonConvert.SerializeObject(breeds));
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            
        }
        public static bool UpdateImageBreed(string imageName, string newBreed)
        {
            try
            {
                if (!File.Exists(JsonPath))
                {
                    CreateFile(JsonPath);
                }
                string serialized = File.ReadAllText(JsonPath);
                List<BreedImageDTO> breeds = JsonConvert.DeserializeObject<List<BreedImageDTO>>(serialized);
                var breed = breeds.Where(p => p.image == imageName).FirstOrDefault();
                if(breed != null)
                {
                    breed.breed = newBreed;
                    File.WriteAllText(JsonPath, JsonConvert.SerializeObject(breeds));
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        private static void CreateFile(string path)
        {
            List<BreedImageDTO> empty = new List<BreedImageDTO>();
            File.WriteAllText(path, JsonConvert.SerializeObject(empty));
        }
        public static void SetPath(string path)
        {
            JsonPath = path + "/ImagesBreed.txt";
        }
    }
    public class BreedImageDTO
    {
        public string image { get; set; }
        public string breed { get; set; }
    }
}
