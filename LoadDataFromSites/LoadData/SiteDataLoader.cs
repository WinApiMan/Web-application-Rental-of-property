using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

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
                        var contactPerson = GetContactPerson(adHtmlDocument);
                        var housingPhotos = GetHousingPhotos(adHtmlDocument);
                        var rentalAd = GetLongTermRentalAd(adHtmlDocument, link);
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

            var nameArray = htmlDocument.DocumentNode.
                SelectSingleNode("//div[@class='username']").InnerText
                .Replace("\r\n", string.Empty).Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

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
            return new LongTermRentalAdDTO
            {
                SourceLink = sourceLink,
                UpdateDate = Convert.ToDateTime(htmlDocument.DocumentNode
                .SelectSingleNode("//div[contains(@class,'w-fetures')]/ul[1]/li[2]/div[2]").InnerText),

            };
        }
    }
}
