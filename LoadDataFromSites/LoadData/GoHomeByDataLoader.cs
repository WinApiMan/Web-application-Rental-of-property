using AutoMapper;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace LoadData
{
    public class GoHomeByDataLoader
    {
        public const string SiteHead = "https://gohome.by", SiteLongTermPage = "https://gohome.by/rent/index/", SiteDailyPage = "https://gohome.by/rent/flat/one-day/";

        public const string DigitsPattern = @"\D+", NewLinePattern = "\r\n";

        public const char Point = '.', Comma = ',', EmptyEntrie = ' ';

        public const int PageIndex = 30, ThreadCount = 15, DefaultValue = 0;

        public const int NameIndex = 0, AdditionalNameIndex = 1, RentAllHouse = 1;

        private readonly HtmlWeb _htmlWeb;

        private readonly IMapper _mapper;

        private readonly object _locker;

        public List<AdDTO> Ads { get; set; } = new List<AdDTO>();

        public GoHomeByDataLoader(HtmlWeb htmlWeb, IMapper mapper)
        {
            _htmlWeb = htmlWeb;
            _mapper = mapper;
            _locker = new object();
        }

        public void LoadDataInAds(RentalAdMenu rentalAdMenu)
        {
            var threads = new List<Thread>();

            for (int index = 0; index < ThreadCount; index++)
            {
                var thread = new Thread(() => LoadDataFromPage(index * PageIndex, rentalAdMenu));
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        public void LoadDataFromPage(int index, RentalAdMenu rentalAdMenu)
        {
            int nextPageCoefficient = PageIndex * ThreadCount;

            bool isPagesAreNotOver = true;

            var ads = new List<AdDTO>();

            while (isPagesAreNotOver)
            {
                var pageHtmlDocument = rentalAdMenu == RentalAdMenu.GoHomeByLongTermRentalAd ?
                    _htmlWeb.Load($"{SiteLongTermPage}{index}") : _htmlWeb.Load($"{SiteDailyPage}{index}");

                var adLinkCollection = pageHtmlDocument.DocumentNode.SelectNodes("//a[@class='name__link']");

                if (!(adLinkCollection is null))
                {
                    foreach (var adLink in adLinkCollection)
                    {
                        //try
                        //{
                            string link = $"{SiteHead}{adLink.Attributes["href"].Value}";
                            var adHtmlDocument = _htmlWeb.Load(link);

                            var ad = new AdDTO
                            {
                                ContactPerson = GetContactPerson(adHtmlDocument),
                                HousingPhotos = GetHousingPhotos(adHtmlDocument),
                            };

                            if (rentalAdMenu == RentalAdMenu.GoHomeByLongTermRentalAd)
                            {
                                ad.RentalAd = GetLongTermRentalAd(adHtmlDocument, link);
                            }
                            else
                            {
                                ad.RentalAd = GetDailyRentalAd(adHtmlDocument, link);
                            }

                            ads.Add(ad);
                        //}
                        //catch (Exception) { }
                    }

                    index += nextPageCoefficient;
                }
                else
                {
                    isPagesAreNotOver = false;
                }
            }

            lock (_locker)
            {
                Ads.AddRange(ads);
            }
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
            var name = string.Empty;

            var names = htmlDocument.DocumentNode
                .SelectNodes("//div[@class='username']");

            if (!(names is null))
            {
                var nameArray = names.First().InnerText
                    .Replace(NewLinePattern, string.Empty)
                    .Split(EmptyEntrie, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in nameArray)
                {
                    name = string.Concat(name, $"{item} ");
                }
            }

            var phones = htmlDocument.DocumentNode
                .SelectNodes("//a[@class='phone__link']").ToArray();

            string email = string.Empty;

            var emails = htmlDocument.DocumentNode
                .SelectNodes("//a[@class='phone__link email']");

            if (!(emails is null))
            {
                email = emails.First().InnerText;
            }

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

            //Get photo nodes
            var photoLinkCollection = htmlDocument.DocumentNode
                .SelectNodes("//div[@class='w-advertisement-images']//a[@data-fancybox='ad_view']");

            if (!(photoLinkCollection is null))
            {
                //Get photos links
                foreach (var photoLink in photoLinkCollection)
                {
                    photos.Add(new HousingPhotoDTO
                    {
                        PathToPhoto = $"{SiteHead}{photoLink.Attributes["href"].Value}"
                    });
                }
            }

            return photos;
        }

        /// <summary>
        /// Get rental ad model.
        /// </summary>
        /// <param name="htmlDocument">Source html document.</param>
        /// <param name="sourceLink">Soruce html link.</param>
        /// <returns>Rental ad model.</returns>
        public RentalAdDTO GetRentalAd(HtmlDocument htmlDocument, string sourceLink)
        {
            (int totalCountOfRooms, int rentCountOfRooms) = GetTotalAndRentCountOfRooms(htmlDocument.DocumentNode);

            (int floor, int totalFloor) = GetTotalAndCurrentFloors(htmlDocument.DocumentNode);

            //Get parametrs and return ad model
            return new LongTermRentalAdDTO
            {
                SourceLink = sourceLink,

                RentalAdNumber = GetRentalAdNumber(htmlDocument.DocumentNode),

                UpdateDate = GetUpdateDate(htmlDocument.DocumentNode),

                Region = GetRegion(htmlDocument.DocumentNode),

                District = GetDistrict(htmlDocument.DocumentNode),

                Locality = GetLocality(htmlDocument.DocumentNode),

                Address = GetAddress(htmlDocument.DocumentNode),

                TotalCountOfRooms = totalCountOfRooms,

                RentCountOfRooms = rentCountOfRooms,

                TotalArea = GetArea(htmlDocument.DocumentNode, "//div[contains(@class,'w-fetures')]/ul[3]/li[1]/div[2]"),

                LivingArea = GetArea(htmlDocument.DocumentNode, "//div[contains(@class,'w-fetures')]/ul[3]/li[2]/div[2]"),

                KitchenArea = GetArea(htmlDocument.DocumentNode, "//div[contains(text(),'Площадь кухни')]/../div[2]"),

                TotalFloors = totalFloor,

                Floor = floor,

                XMapCoordinate = GetMapCoordinate(htmlDocument.DocumentNode, "//input[@id='map_latitude']"),

                YMapCoordinate = GetMapCoordinate(htmlDocument.DocumentNode, "//input[@id='map_longitude']"),

                Bathroom = GetSingleNodeInnerText(htmlDocument.DocumentNode, "//div[contains(@class,'w-fetures')]/ul[4]/li[1]/div[2]"),

                Description = GetSingleNodeInnerText(htmlDocument.DocumentNode, "//article/p"),

                Facilities = GetFacilities(htmlDocument.DocumentNode),
            };
        }

        /// <summary>
        /// Get long term rental ad model.
        /// </summary>
        /// <param name="htmlDocument">Source html document.</param>
        /// <param name="sourceLink">Soruce html link.</param>
        /// <returns>Long term rental ad model.</returns>
        public LongTermRentalAdDTO GetLongTermRentalAd(HtmlDocument htmlDocument, string sourceLink)
        {
            var rentalAd = _mapper.Map<LongTermRentalAdDTO>(GetRentalAd(htmlDocument, sourceLink));

            (double uSDPrice, double bYNPrice) = GetPrices(htmlDocument.DocumentNode);

            rentalAd.USDPrice = uSDPrice;

            rentalAd.BYNPrice = bYNPrice;

            return rentalAd;
        }

        /// <summary>
        /// Get daily rental ad model.
        /// </summary>
        /// <param name="htmlDocument">Source html document.</param>
        /// <param name="sourceLink">Soruce html link.</param>
        /// <returns>Daily rental ad model.</returns>
        public DailyRentalAdDTO GetDailyRentalAd(HtmlDocument htmlDocument, string sourceLink)
        {
            var rentalAd = _mapper.Map<DailyRentalAdDTO>(GetRentalAd(htmlDocument, sourceLink));

            return rentalAd;
        }

        public double GetMapCoordinate(HtmlNode htmlNode, string xPath)
        {
            var coordinates = htmlNode.SelectNodes(xPath);

            if (!(coordinates is null))
            {
                return Convert.ToDouble(coordinates.First()
                    .Attributes["value"].Value.Replace(Point, Comma));
            }
            else
            {
                return DefaultValue;
            }
        }

        public string GetRegion(HtmlNode htmlNode)
        {
            var regions = htmlNode.SelectNodes("//div[contains(@class,'w-fetures')]/ul[2]/li[1]/div[2]");

            if (!(regions is null))
            {
                var regionArray = regions.First().InnerText
                    .Replace(NewLinePattern, string.Empty)
                    .Split(EmptyEntrie, StringSplitOptions.RemoveEmptyEntries);

                string region = string.Empty;

                foreach (string item in regionArray)
                {
                    region = string.Concat(region, $"{item} ");
                }

                return region;
            }
            else
            {
                regions = htmlNode.SelectNodes("//div[contains(@class,'w-fetures')]/ul[2]/li[1]/div[2]/a");

                if (!(regions is null))
                {
                    return regions.First().InnerText;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string GetDistrict(HtmlNode htmlNode)
        {
            var districts = htmlNode.SelectNodes("//div[contains(@class,'w-fetures')]/ul[2]/li[2]/div[2]");

            if (!(districts is null))
            {
                var districtArray = districts.First().InnerText
                    .Replace(NewLinePattern, string.Empty)
                    .Split(EmptyEntrie, StringSplitOptions.RemoveEmptyEntries);

                string district = string.Empty;

                foreach (string item in districtArray)
                {
                    district = string.Concat(district, $"{item} ");
                }

                return district;
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetAddress(HtmlNode htmlNode)
        {
            var addresses = htmlNode.SelectNodes("//div[contains(text(),'Улица')]/../div[2]/a");

            if (!(addresses is null))
            {
                return addresses.First().InnerText
                    .Replace(NewLinePattern, string.Empty).Trim();
            }
            else
            {
                addresses = htmlNode.SelectNodes("//div[contains(text(),'Улица')]/../div[2]");

                if (!(addresses is null))
                {
                    return addresses.First().InnerText
                        .Replace(NewLinePattern, string.Empty).Trim();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string GetLocality(HtmlNode htmlNode)
        {
            var localityNodes = htmlNode
                .SelectNodes("//div[contains(@class,'w-fetures')]/ul[2]/li[3]/div[2]/a");

            if (!(localityNodes is null))
            {
                return localityNodes.First().InnerText;
            }
            else
            {
                localityNodes = htmlNode
                    .SelectNodes("//div[contains(text(),'Район:')]/../div[2]");

                if (!(localityNodes is null))
                {
                    return localityNodes.First().InnerText
                        .Replace(NewLinePattern, string.Empty).Trim();
                }
                else
                {
                    localityNodes = htmlNode
                        .SelectNodes("//div[contains(text(),'Район')]/../div[2]");

                    if (!(localityNodes is null))
                    {
                        return localityNodes.First().InnerText
                            .Replace(NewLinePattern, string.Empty).Trim();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
        }

        public (int, int) GetTotalAndRentCountOfRooms(HtmlNode htmlNode)
        {
            const int RentalPart = 2;

            var countRoomStrings = htmlNode.SelectNodes("//div[contains(text(),'Комнат')]/../div[2]/a");

            string countRoomString;

            if (!(countRoomStrings is null))
            {
                countRoomString = countRoomStrings.First().InnerText;
            }
            else
            {
                countRoomStrings = htmlNode.SelectNodes("//div[contains(text(),'Комнат')]/../div[2]");

                if (!(countRoomStrings is null))
                {
                    countRoomString = countRoomStrings.First().InnerText;
                }
                else
                {
                    return (DefaultValue, DefaultValue);
                }
            }

            var countRoomArray = Regex.Split(countRoomString, DigitsPattern)
                .Where(tempString => !string.IsNullOrEmpty(tempString)).ToArray();

            if (countRoomArray.Length == RentAllHouse)
            {
                int countOfRooms = Convert.ToInt32(countRoomArray[NameIndex]);
                return (countOfRooms, countOfRooms);
            }
            else if (countRoomArray.Length == RentalPart)
            {
                return (Convert.ToInt32(countRoomArray[RentAllHouse]), Convert.ToInt32(countRoomArray[NameIndex]));
            }
            else
            {
                return (DefaultValue, DefaultValue);
            }
        }

        public (int, int) GetTotalAndCurrentFloors(HtmlNode htmlNode)
        {
            const int CorrectFloorItems = 2, OneFloorItem = 1;
            var floorsStrings = htmlNode.SelectNodes("//div[contains(text(),'Этаж')]/../div[2]");

            if (!(floorsStrings is null))
            {
                var floorsArray = Regex.Split(floorsStrings.First().InnerText, DigitsPattern)
                    .Where(tempString => !string.IsNullOrEmpty(tempString))
                    .ToArray();

                if (floorsArray.Length == CorrectFloorItems)
                {
                    return (Convert.ToInt32(floorsArray[NameIndex]), Convert.ToInt32(floorsArray[AdditionalNameIndex]));
                }
                else if (floorsArray.Length == OneFloorItem)
                {
                    int floor = Convert.ToInt32(floorsArray[NameIndex]);
                    return (floor, floor);
                }
            }

            return (DefaultValue, DefaultValue);
        }

        public double GetArea(HtmlNode htmlNode, string xPath)
        {
            var kitchenAreas = htmlNode.SelectNodes(xPath);

            if (!(kitchenAreas is null))
            {
                var itemArray = kitchenAreas.First().InnerText
                    .Split(EmptyEntrie);

                if (!(itemArray is null))
                {
                    return Convert.ToDouble(itemArray[NameIndex].Replace(Point, Comma));
                }

            }

            return DefaultValue;
        }

        public string GetFacilities(HtmlNode htmlNode)
        {

            var facilitiesNodeCollection = htmlNode.SelectNodes("//div[@class='left']/div[contains(@class,'w-features')]/div/div[@class='text']");

            if (!(facilitiesNodeCollection is null))
            {
                string facilities = string.Empty;

                foreach (var facilitie in facilitiesNodeCollection)
                {
                    facilities = string.Concat(facilities, $"{facilitie.InnerText}, ");
                }

                return facilities;
            }
            else
            {
                return string.Empty;
            }
        }

        public (double, double) GetPrices(HtmlNode htmlNode)
        {
            var uSDPrices = htmlNode.SelectNodes("//div[@class='price']");
            var bYNPrices = htmlNode.SelectNodes("//div[@class='price big']/span");

            double uSDPrice = DefaultValue, bYNPrice = DefaultValue;

            if (!(uSDPrices is null))
            {
                uSDPrice = Convert.ToDouble(Regex.Split(uSDPrices.First().InnerText, DigitsPattern)
                    .Where(tempString => !string.IsNullOrEmpty(tempString))
                    .ToArray()[NameIndex].Replace(Point, Comma));
            }

            if (!(bYNPrices is null))
            {
                bYNPrice = Convert.ToDouble(bYNPrices.First().InnerText.Replace(Point, Comma));
            }

            return (uSDPrice, bYNPrice);
        }

        public int GetRentalAdNumber(HtmlNode htmlNode)
        {
            var rentalAdNumbers = htmlNode.SelectNodes("//div[contains(@class,'w-fetures')]/ul[1]/li[1]/div[2]");

            if (!(rentalAdNumbers is null))
            {
                return Convert.ToInt32(rentalAdNumbers.First().InnerText);
            }
            else
            {
                return DefaultValue;
            }
        }

        public DateTime GetUpdateDate(HtmlNode htmlNode)
        {
            var updateDates = htmlNode.SelectNodes("//div[contains(@class,'w-fetures')]/ul[1]/li[2]/div[2]");

            if (!(updateDates is null))
            {
                return Convert.ToDateTime(updateDates.First().InnerText);
            }
            else
            {
                return new DateTime();
            }
        }

        public string GetSingleNodeInnerText(HtmlNode htmlNode, string xPath)
        {
            var htmlItems = htmlNode.SelectNodes(xPath);

            if (!(htmlItems is null))
            {
                return htmlItems.First().InnerText;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
