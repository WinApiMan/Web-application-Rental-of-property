﻿using AutoMapper;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LoadData
{
    public class GoHomeByDataLoader
    {
        public const string SiteHead = "https://gohome.by", SiteApartmentsLongTermPage = "https://gohome.by/rent/index/",
            SiteApartmentsDailyTermPage = "https://gohome.by/rent/flat/one-day/", SiteHousesDailyTermPage = "https://gohome.by/houses_one_day/page/",
            SiteHousesLongTermPage = "https://gohome.by/houses_rent/page/";

        public const string DigitsPattern = @"\D+", NewLinePattern = "\r\n", FloatDigitsPatter = @"^[0-9]*[.,]?[0-9]+$";

        public const char Point = '.', Comma = ',', EmptyEntrie = ' ', Semicolon = ';';

        public const int PageIndex = 30, ThreadCount = 15, DefaultValue = 0;

        public const int NameIndex = 0, AdditionalNameIndex = 1, RentAllHouse = 1;

        private RentalAdMenu _rentalAdMenu;

        private readonly object _locker;

        private readonly HtmlWeb _htmlWeb;

        private readonly IMapper _mapper;

        public List<AdDTO> Ads { get; set; } = new List<AdDTO>();

        public GoHomeByDataLoader(HtmlWeb htmlWeb, IMapper mapper)
        {
            _htmlWeb = htmlWeb;
            _mapper = mapper;
            _locker = new object();
        }

        public void LoadDataInAds(RentalAdMenu rentalAdMenu)
        {
            _rentalAdMenu = rentalAdMenu;

            var indexList = new List<int>();

            for (int index = 0; index < ThreadCount; index++)
            {
                indexList.Add(index * PageIndex);
            }

            indexList.AsParallel().ForAll(LoadDataFromPage);
        }

        public void LoadDataFromPage(int index)
        {
            int nextPageCoefficient = PageIndex * ThreadCount;

            bool isPagesAreNotOver = true;

            var ads = new List<AdDTO>();

            while (isPagesAreNotOver)
            {
                var pageHtmlDocument = _rentalAdMenu switch
                {
                    RentalAdMenu.RealtApartmentsByLongTermRentalAd => _htmlWeb.Load($"{SiteApartmentsLongTermPage}{index}"),
                    RentalAdMenu.RealtApartmentsByDailyRentalAd => _htmlWeb.Load($"{SiteApartmentsDailyTermPage}{index}"),
                    RentalAdMenu.RealtHousesByDailyRentalAd => _htmlWeb.Load($"{SiteHousesDailyTermPage}{index}"),
                    RentalAdMenu.RealtHouseByLongTermRentalAd => _htmlWeb.Load($"{SiteHousesLongTermPage}{index}"),
                    _ => _htmlWeb.Load($"{SiteApartmentsLongTermPage}{index}"),
                };

                var adLinkCollection = pageHtmlDocument.DocumentNode.SelectNodes("//a[@class='name__link']");

                if (!(adLinkCollection is null))
                {
                    foreach (var adLink in adLinkCollection)
                    {
                        string link = $"{SiteHead}{adLink.Attributes["href"].Value}";
                        var adHtmlDocument = _htmlWeb.Load(link);

                        var ad = new AdDTO
                        {
                            ContactPerson = GetContactPerson(adHtmlDocument.DocumentNode),
                            HousingPhotos = GetHousingPhotos(adHtmlDocument.DocumentNode),
                            AditionalAdData = new AditionalAdDataDTO
                            {
                                UpdateDate = GetUpdateDate(adHtmlDocument.DocumentNode),
                            }                            
                        };

                        GetAdViews(adHtmlDocument.DocumentNode, ad.AditionalAdData);

                        ad.RentalAd = _rentalAdMenu switch
                        {
                            RentalAdMenu.RealtApartmentsByLongTermRentalAd => GetApartmentLongTermRentalAd(adHtmlDocument.DocumentNode, link),
                            RentalAdMenu.RealtApartmentsByDailyRentalAd => GetApartmentDailyRentalAd(adHtmlDocument.DocumentNode, link),
                            RentalAdMenu.RealtHousesByDailyRentalAd => GetHouseDailyRentalAd(adHtmlDocument.DocumentNode, link),
                            RentalAdMenu.RealtHouseByLongTermRentalAd => GetHouseLongTermRentalAd(adHtmlDocument.DocumentNode, link),
                            _ => GetApartmentLongTermRentalAd(adHtmlDocument.DocumentNode, link),
                        };
                        ads.Add(ad);
                    }
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
        public ContactPersonDTO GetContactPerson(HtmlNode htmlNode)
        {
            const int NameIndex = 0, PositionIndex = 1;
            const int MainPhoneNumber = 0, AdditionalPhoneNumber = 1;

            var phones = GetPhoneNodes(htmlNode);

            //Get any person parametrs and return person model

            if (phones.Length == MainPhoneNumber)
            {
                return new ContactPersonDTO
                {
                    Name = GetContactName(htmlNode),
                    Email = GetContactEmail(htmlNode),
                };
            }
            else if (phones.Length == PositionIndex)
            {
                return new ContactPersonDTO
                {
                    Name = GetContactName(htmlNode),
                    Email = GetContactEmail(htmlNode),
                    PhoneNumber = phones[NameIndex].InnerText,
                };
            }
            else
            {
                return new ContactPersonDTO
                {
                    Name = GetContactName(htmlNode),
                    Email = GetContactEmail(htmlNode),
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
        public IEnumerable<HousingPhotoDTO> GetHousingPhotos(HtmlNode htmlNode)
        {
            var photos = new List<HousingPhotoDTO>();

            //Get photo nodes
            var photoLinkCollection = htmlNode
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
        public RentalAdDTO GetRentalAd(HtmlNode htmlNode, string sourceLink)
        {
            (int totalCountOfRooms, int rentCountOfRooms) = GetTotalAndRentCountOfRooms(htmlNode);

            (int floor, int totalFloor) = GetTotalAndCurrentFloors(htmlNode);

            //Get parametrs and return ad model
            return new LongTermRentalAdDTO
            {
                SourceLink = sourceLink,

                RentalAdNumber = GetRentalAdNumber(htmlNode),

                UpdateDate = DateTime.Now,

                TotalViews = DefaultValue,

                MonthViews = DefaultValue,

                WeekViews = DefaultValue,

                Region = GetRegion(htmlNode),

                District = GetDistrict(htmlNode),

                Locality = GetLocality(htmlNode),

                Address = GetAddress(htmlNode),

                TotalCountOfRooms = totalCountOfRooms,

                RentCountOfRooms = rentCountOfRooms,

                TotalArea = GetArea(htmlNode, "//div[contains(@class,'w-fetures')]/ul[3]/li[1]/div[2]"),

                LivingArea = GetArea(htmlNode, "//div[contains(@class,'w-fetures')]/ul[3]/li[2]/div[2]"),

                KitchenArea = GetArea(htmlNode, "//div[contains(text(),'Площадь кухни')]/../div[2]"),

                TotalFloors = totalFloor,

                Floor = floor,

                XMapCoordinate = GetMapCoordinate(htmlNode, "//input[@id='map_latitude']"),

                YMapCoordinate = GetMapCoordinate(htmlNode, "//input[@id='map_longitude']"),

                Bathroom = GetSingleNodeInnerText(htmlNode, "//div[contains(@class,'w-fetures')]/ul[4]/li[1]/div[2]"),

                Description = GetSingleNodeInnerText(htmlNode, "//article/p"),

                Facilities = GetFacilities(htmlNode),

                RentalType = (int)_rentalAdMenu,
            };
        }

        /// <summary>
        /// Get long term rental ad model.
        /// </summary>
        /// <param name="htmlDocument">Source html document.</param>
        /// <param name="sourceLink">Soruce html link.</param>
        /// <returns>Long term rental ad model.</returns>
        public LongTermRentalAdDTO GetApartmentLongTermRentalAd(HtmlNode htmlNode, string sourceLink)
        {
            var rentalAd = _mapper.Map<LongTermRentalAdDTO>(GetRentalAd(htmlNode, sourceLink));

            (double uSDPrice, double bYNPrice) = GetPrices(htmlNode);

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
        public DailyRentalAdDTO GetApartmentDailyRentalAd(HtmlNode htmlNode, string sourceLink)
        {
            var rentalAd = _mapper.Map<DailyRentalAdDTO>(GetRentalAd(htmlNode, sourceLink));
            GetDailyPrices(htmlNode, rentalAd);

            return rentalAd;
        }

        public DailyRentalAdDTO GetHouseDailyRentalAd(HtmlNode htmlNode, string sourceLink)
        {
            var rentalAd = GetApartmentDailyRentalAd(htmlNode, sourceLink);

            var noteCollection = htmlNode.SelectNodes("//div[contains(@class,'w-fetures')]/ul[4]/li/div[@class='description']");

            if (!(noteCollection is null))
            {
                var notes = noteCollection.First().InnerText
                    .Replace(NewLinePattern, string.Empty)
                    .Split(Semicolon, StringSplitOptions.RemoveEmptyEntries);

                string notesString = string.Empty;

                foreach(var item in notes)
                {
                    notesString = string.Concat(notesString, $"{item.Trim()} ");
                }

                rentalAd.Notes = notesString;
            }

            return rentalAd;
        }

        public LongTermRentalAdDTO GetHouseLongTermRentalAd(HtmlNode htmlNode, string sourceLink)
        {
            var rentalAD = GetApartmentLongTermRentalAd(htmlNode, sourceLink);

            var landAreaCollection = htmlNode.SelectNodes("//section[contains(@class,'s-line')]/div[@class='container']/div/ul/li[2]/div[@class='feature']");

            if (!(landAreaCollection is null))
            {
                rentalAD.LandArea = Convert.ToDouble(landAreaCollection.First().InnerText
                    .Split(EmptyEntrie).First()
                    .Replace(Point, Comma));
            }

            return rentalAD;
        }

        /// <summary>
        /// Get contact name.
        /// </summary>
        /// <param name="htmlNode">Html node.</param>
        /// <returns>Name string.</returns>
        public string GetContactName(HtmlNode htmlNode)
        {
            var name = string.Empty;

            var names = htmlNode
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

            return name;
        }

        /// <summary>
        /// Get phone nodes.
        /// </summary>
        /// <param name="htmlNode">Html node.</param>
        /// <returns>Phone nodes array</returns>
        public HtmlNode[] GetPhoneNodes(HtmlNode htmlNode)
        {
            var phones = htmlNode
                .SelectNodes("//a[@class='phone__link']");

            if (!(phones is null))
            {
                return phones.ToArray();
            }
            else
            {
                return new HtmlNode[DefaultValue];
            }
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

        /// <summary>
        /// Get contact email.
        /// </summary>
        /// <param name="htmlNode">Html node.</param>
        /// <returns>Email string.</returns>
        public string GetContactEmail(HtmlNode htmlNode)
        {
            string email = string.Empty;

            var emails = htmlNode
                .SelectNodes("//a[@class='phone__link email']");

            if (!(emails is null))
            {
                email = emails.First().InnerText;
            }

            return email;
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

        public void GetDailyPrices(HtmlNode htmlNode, DailyRentalAdDTO dailyRentalAdDTO)
        {
            const int NegotiatedPrice = -1;

            var priceCollection = htmlNode.SelectNodes("//div[@class='price-row']/div");

            if (!(priceCollection is null))
            {
                var uSDPriceCollection = htmlNode.SelectNodes("//div[@class='price-row']/div/div[@class='price']");

                try
                {
                    dailyRentalAdDTO.BYNPricePerDay = Convert.ToDouble(htmlNode.SelectSingleNode("//div[@class='price big']/span").InnerText);

                    dailyRentalAdDTO.USDPricePerDay = Convert.ToDouble(Regex.Split(uSDPriceCollection.First().InnerText, DigitsPattern)
                        .Where(tempString => !string.IsNullOrEmpty(tempString))
                        .ToArray()[NameIndex].Replace(Point, Comma));
                }
                catch (Exception)
                {
                    dailyRentalAdDTO.BYNPricePerDay = dailyRentalAdDTO.USDPricePerDay = NegotiatedPrice;
                }

                try
                {
                    dailyRentalAdDTO.BYNPricePerPerson = Convert.ToDouble(Regex.Split(htmlNode.SelectSingleNode("//div[@class='price-row']/div[2]/div[@class='price big']").InnerText, DigitsPattern)
                        .Where(tempString => !string.IsNullOrEmpty(tempString))
                        .ToArray()[NameIndex].Replace(Point, Comma));

                    dailyRentalAdDTO.USDPricePerPerson = Convert.ToDouble(Regex.Split(htmlNode.SelectSingleNode("//div[@class='price-row']/div[2]/div[@class='price']").InnerText, DigitsPattern)
                        .Where(tempString => !string.IsNullOrEmpty(tempString))
                        .ToArray()[NameIndex].Replace(Point, Comma));
                }
                catch (Exception)
                {
                    dailyRentalAdDTO.BYNPricePerPerson = dailyRentalAdDTO.USDPricePerPerson = NegotiatedPrice;
                }
            }
        }

        public void GetAdViews(HtmlNode htmlNode, AditionalAdDataDTO aditionalAdDataDTO)
        {
            const int TotalViewIndex = 0, MonthViewIndex = 1, WeekViewIndex = 2;

            var viewCollection = htmlNode.SelectNodes("//div[@class='w-page-view-row']");

            if (!(viewCollection is null))
            {
                var views = Regex.Split(viewCollection.First().InnerText, DigitsPattern)
                        .Where(tempString => !string.IsNullOrEmpty(tempString))
                        .ToArray();

                aditionalAdDataDTO.TotalViews = Convert.ToInt32(views[TotalViewIndex]);
                aditionalAdDataDTO.MonthViews = Convert.ToInt32(views[MonthViewIndex]);
                aditionalAdDataDTO.WeekViews = Convert.ToInt32(views[WeekViewIndex]);
            }
        }
    }
}
