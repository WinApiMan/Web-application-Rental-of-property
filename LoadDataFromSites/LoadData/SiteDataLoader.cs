using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LoadData
{
    public class SiteDataLoader
    {
        private const string SiteHead = "https://gohome.by", SitePage = "https://gohome.by/rent/index/";

        private const int NextPageCoefficient = 30;

        private readonly HtmlWeb _htmlWeb;

        public SiteDataLoader(HtmlWeb htmlWeb)
        {
            _htmlWeb = htmlWeb;
        }

        public IEnumerable<AdDTO> LoadData()
        {
            bool isPagesAreNotOver = true;
            int index = 0;

            var ads = new List<AdDTO>();

            while (isPagesAreNotOver)
            {
                try
                {
                    var pageHtmlDocument = _htmlWeb.Load($"{SitePage}{index}");

                    var adLinkCollection = pageHtmlDocument.DocumentNode.SelectNodes("//a[@class='name__link']");

                    foreach (var adLink in adLinkCollection)
                    {
                        try
                        {
                            string link = $"{SiteHead}{adLink.Attributes["href"].Value}";
                            var adHtmlDocument = _htmlWeb.Load(link);

                            ads.Add(new AdDTO
                            {
                                ContactPerson = GetContactPerson(adHtmlDocument),
                                HousingPhotos = GetHousingPhotos(adHtmlDocument),
                                RentalAd = GetLongTermRentalAd(adHtmlDocument, link)
                            });
                        }
                        catch(Exception) { }
                    }

                    index += NextPageCoefficient;
                }
                catch (Exception)
                {
                    isPagesAreNotOver = false;
                }
            }
            return ads;
        }

        /// <summary>
        /// Get contact person.
        /// </summary>
        /// <param name="htmlDocument">Current html document.</param>
        /// <returns>Contact person model.</returns>
        public ContactPersonDTO GetContactPerson(HtmlDocument htmlDocument)
        {
            const int NameIndex = 0, PositionIndex = 1;
            const int MainPhoneNumber = 0, AdditionalPhoneNumber = 1;

            //Get person names
            var nameArray = htmlDocument.DocumentNode
                .SelectSingleNode("//div[@class='username']").InnerText
                .Replace("\r\n", string.Empty)
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var name = string.Empty;

            foreach (var item in nameArray)
            {
                name = string.Concat(name, $"{item} ");
            }

            var phones = htmlDocument.DocumentNode.SelectNodes("//a[@class='phone__link']").ToArray();

            string email = string.Empty;

            try
            {
                email = htmlDocument.DocumentNode.SelectSingleNode("//a[@class='phone__link email']").InnerText;
            }
            catch (Exception) { }

            //Get any person parametrs and return person model

            if (phones.Length == MainPhoneNumber)
            {
                return new ContactPersonDTO
                {
                    Name = name,
                    Email = email,
                };
            }
            else if (phones.Length == PositionIndex)
            {
                return new ContactPersonDTO
                {
                    Name = name,
                    Email = email,
                    PhoneNumber = phones[NameIndex].InnerText,
                };
            }
            else
            {
                return new ContactPersonDTO
                {
                    Name = name,
                    Email = email,
                    PhoneNumber = phones[MainPhoneNumber].InnerText,
                    AdditionalPhoneNumber = phones[AdditionalPhoneNumber].InnerText
                };
            }
        }

        /// <summary>
        /// Get ad photos.
        /// </summary>
        /// <param name="htmlDocument">Current html document.</param>
        /// <returns>Ad photos.</returns>
        public IEnumerable<HousingPhotoDTO> GetHousingPhotos(HtmlDocument htmlDocument)
        {
            var photos = new List<HousingPhotoDTO>();

            try
            {
                //Get photo nodes
                var photoLinkCollection = htmlDocument.DocumentNode
                    .SelectNodes("//div[@class='w-advertisement-images']//a[@data-fancybox='ad_view']");

                //Get photos links
                foreach (var photoLink in photoLinkCollection)
                {
                    photos.Add(new HousingPhotoDTO
                    {
                        PathToPhoto = $"{SiteHead}{photoLink.Attributes["href"].Value}"
                    });
                }
            }
            catch (Exception) { }

            return photos;
        }

        /// <summary>
        /// Get long term rental ad model.
        /// </summary>
        /// <param name="htmlDocument">Source html document.</param>
        /// <param name="sourceLink">Soruce html link.</param>
        /// <returns>Long term rental ad model.</returns>
        public LongTermRentalAdDTO GetLongTermRentalAd(HtmlDocument htmlDocument, string sourceLink)
        {
            const int NameIndex = 0, AdditionalNameIndex = 1, RentAllHouse = 1;

            //Region words in array
            var regionArray = htmlDocument.DocumentNode
                .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[2]/li[1]/div[2]").InnerText
                .Replace("\r\n", string.Empty)
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //District word in array
            var districtArray = htmlDocument.DocumentNode
                .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[2]/li[2]/div[2]").InnerText
                .Replace("\r\n", string.Empty)
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string address;

            //Get address
            try
            {
                address = htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(text(),'Улица')]/../div[2]/a").InnerText
                    .Replace("\r\n", string.Empty).Trim();
            }
            catch (Exception)
            {
                address = htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(text(),'Улица')]/../div[2]").InnerText
                    .Replace("\r\n", string.Empty).Trim();
            }

            string locality;
            //Get locality

            try
            {
                locality = htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[2]/li[3]/div[2]/a").InnerText;
            }
            catch (Exception)
            {
                locality = htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(text(),'Район:')]/../div[2]").InnerText
                    .Replace("\r\n", string.Empty).Trim();
            }

            //Get all rooms and rent rooms
            string countRoomString;

            try
            {
                countRoomString = htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(text(),'Комнат')]/../div[2]/a").InnerText;
            }
            catch (Exception)
            {
                countRoomString = htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(text(),'Комнат')]/../div[2]").InnerText;
            }

            var countRoomArray = Regex.Split(countRoomString, @"\D+").Where(tempString => !string.IsNullOrEmpty(tempString)).ToArray();

            int totalCountOfRooms, rentCountOfRooms;

            if (countRoomArray.Length == RentAllHouse)
            {
                totalCountOfRooms = rentCountOfRooms = Convert.ToInt32(countRoomArray[NameIndex]);
            }
            else
            {
                totalCountOfRooms = Convert.ToInt32(countRoomArray[RentAllHouse]);
                rentCountOfRooms = Convert.ToInt32(countRoomArray[NameIndex]);
            }

            ///////

            //Get all floors and current floor
            var floorsArray = Regex.Split(htmlDocument.DocumentNode
                .SelectSingleNode("//div[contains(text(),'Этаж')]/../div[2]").InnerText, @"\D+")
                .Where(tempString => !string.IsNullOrEmpty(tempString))
                .ToArray();

            //Get kitchen area square
            double kitchenArea = 0;

            try
            {
                kitchenArea = Convert.ToDouble(htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(text(),'Площадь кухни')]/../div[2]").InnerText
                    .Split(new char[] { ' ' })[NameIndex].Replace('.', ','));
            }
            catch (Exception) { }

            //Get facilities
            string facilities = string.Empty;

            try
            {
                var facilitiesNodeCollection = htmlDocument.DocumentNode.SelectNodes("//div[@class='left']/div[contains(@class,'w-features')]/div/div[@class='text']");

                foreach (var facilitie in facilitiesNodeCollection)
                {
                    facilities = string.Concat(facilities, $"{facilitie.InnerText}, ");
                }
            }
            catch (Exception) { }

            //Get any parametrs and return ad model
            return new LongTermRentalAdDTO
            {
                SourceLink = sourceLink,

                RentalAdNumber = Convert.ToInt32(htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[1]/li[1]/div[2]").InnerText),

                UpdateDate = Convert.ToDateTime(htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[1]/li[2]/div[2]").InnerText),

                Region = $"{regionArray[NameIndex]} {regionArray[AdditionalNameIndex]}",

                District = $"{districtArray[NameIndex]} {districtArray[AdditionalNameIndex]}",

                Locality = locality,

                Address = address,

                TotalCountOfRooms = totalCountOfRooms,

                RentCountOfRooms = rentCountOfRooms,

                TotalArea = Convert.ToDouble(htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[3]/li[1]/div[2]").InnerText
                    .Split(new char[] { ' ' })[NameIndex].Replace('.', ',')),

                LivingArea = Convert.ToDouble(htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[3]/li[2]/div[2]").InnerText
                    .Split(new char[] { ' ' })[NameIndex].Replace('.', ',')),

                KitchenArea = kitchenArea,

                TotalFloors = Convert.ToInt32(floorsArray[AdditionalNameIndex]),

                Floor = Convert.ToInt32(floorsArray[NameIndex]),

                XMapCoordinate = Convert.ToDouble(htmlDocument.DocumentNode
                    .SelectSingleNode("//input[@id='map_latitude']").Attributes["value"].Value.Replace('.', ',')),

                YMapCoordinate = Convert.ToDouble(htmlDocument.DocumentNode
                    .SelectSingleNode("//input[@id='map_longitude']").Attributes["value"].Value.Replace('.', ',')),

                Bathroom = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[4]/li[1]/div[2]").InnerText,

                Description = htmlDocument.DocumentNode.SelectSingleNode("//article/p").InnerText,

                Facilities = facilities,

                USDPrice = Convert.ToDouble(Regex.Split(htmlDocument.DocumentNode
                    .SelectSingleNode("//div[@class='price']").InnerText, @"\D+")
                    .Where(tempString => !string.IsNullOrEmpty(tempString))
                    .ToArray()[NameIndex].Replace('.', ',')),

                BYNPrice = Convert.ToDouble(htmlDocument.DocumentNode
                    .SelectSingleNode("//div[@class='price big']/span").InnerText.Replace('.', ',')),
            };
        }
    }
}
