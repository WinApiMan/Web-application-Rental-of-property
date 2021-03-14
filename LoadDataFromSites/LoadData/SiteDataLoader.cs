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

        public void LoadData()
        {
            bool isPagesAreNotOver = true;
            int index = 0;

            var ads = new List<AdDTO>();

            while (isPagesAreNotOver)
            {
                var pageHtmlDocument = _htmlWeb.Load($"{SitePage}{index}");

                var adLinkCollection = pageHtmlDocument.DocumentNode.SelectNodes("//a[@class='name__link']");

                foreach (var adLink in adLinkCollection)
                {
                    string link = $"{SiteHead}{adLink.Attributes["href"].Value}";
                    var adHtmlDocument = _htmlWeb.Load(link);

                    try
                    {
                        ads.Add(new AdDTO
                        {
                            ContactPerson = GetContactPerson(adHtmlDocument),
                            HousingPhotos = GetHousingPhotos(adHtmlDocument),
                            RentalAd = GetLongTermRentalAd(adHtmlDocument, link)
                        });
                    }
                    catch (Exception)
                    {
                        isPagesAreNotOver = false;
                    }
                }

                index += NextPageCoefficient;
            }
        }

        public ContactPersonDTO GetContactPerson(HtmlDocument htmlDocument)
        {
            const int NameIndex = 0, PositionIndex = 1;

            var nameArray = htmlDocument.DocumentNode
                .SelectSingleNode("//div[@class='username']").InnerText
                .Replace("\r\n", string.Empty)
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return new ContactPersonDTO
            {
                Name = $"{nameArray[NameIndex]} ({nameArray[PositionIndex]})",
                Email = htmlDocument.DocumentNode.SelectSingleNode("//a[@class='phone__link email']").InnerText,
                PhoneNumber = htmlDocument.DocumentNode.SelectSingleNode("//a[@class='phone__link']").InnerText
            };
        }

        public IEnumerable<HousingPhotoDTO> GetHousingPhotos(HtmlDocument htmlDocument)
        {
            var photos = new List<HousingPhotoDTO>();

            var photoLinkCollection = htmlDocument.DocumentNode
                .SelectNodes("//div[@class='w-advertisement-images']//a[@data-fancybox='ad_view']");

            foreach (var photoLink in photoLinkCollection)
            {
                photos.Add(new HousingPhotoDTO
                {
                    PathToPhoto = $"{SiteHead}{photoLink.Attributes["href"].Value}"
                });
            }

            return photos;
        }

        public LongTermRentalAdDTO GetLongTermRentalAd(HtmlDocument htmlDocument, string sourceLink)
        {
            const int NameIndex = 0, AdditionalNameIndex = 1, RentAllHouse = 1;

            var regionArray = htmlDocument.DocumentNode
                .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[2]/li[1]/div[2]").InnerText
                .Replace("\r\n", string.Empty)
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var districtArray = htmlDocument.DocumentNode
                .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[2]/li[2]/div[2]").InnerText
                .Replace("\r\n", string.Empty)
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string address;

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

            var floorsArray = Regex.Split(htmlDocument.DocumentNode
                .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[3]/li[4]/div[2]").InnerText, @"\D+")
                .Where(tempString => !string.IsNullOrEmpty(tempString))
                .ToArray();

            return new LongTermRentalAdDTO
            {
                SourceLink = sourceLink,

                RentalAdNumber = Convert.ToInt32(htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[1]/li[1]/div[2]").InnerText),

                UpdateDate = Convert.ToDateTime(htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[1]/li[2]/div[2]").InnerText),

                Region = $"{regionArray[NameIndex]} {regionArray[AdditionalNameIndex]}",

                District = $"{districtArray[NameIndex]} {districtArray[AdditionalNameIndex]}",

                Locality = htmlDocument.DocumentNode
                .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[2]/li[3]/div[2]/a").InnerText,

                Address = address,

                TotalCountOfRooms = totalCountOfRooms,

                RentCountOfRooms = rentCountOfRooms,

                TotalArea = Convert.ToDouble(htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[3]/li[1]/div[2]").InnerText
                    .Split(new char[] { ' ' })[NameIndex].Replace('.', ',')),

                LivingArea = Convert.ToDouble(htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[3]/li[2]/div[2]").InnerText
                    .Split(new char[] { ' ' })[NameIndex].Replace('.', ',')),

                KitchenArea = Convert.ToDouble(htmlDocument.DocumentNode
                    .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[3]/li[3]/div[2]").InnerText
                    .Split(new char[] { ' ' })[NameIndex].Replace('.', ',')),

                TotalFloors = Convert.ToInt32(floorsArray[AdditionalNameIndex]),

                Floor = Convert.ToInt32(floorsArray[NameIndex]),

                XMapCoordinate = Convert.ToDouble(htmlDocument.DocumentNode
                    .SelectSingleNode("//input[@id='map_latitude']").Attributes["value"].Value.Replace('.', ',')),

                YMapCoordinate = Convert.ToDouble(htmlDocument.DocumentNode
                    .SelectSingleNode("//input[@id='map_longitude']").Attributes["value"].Value.Replace('.', ',')),
            };
        }
    }
}
