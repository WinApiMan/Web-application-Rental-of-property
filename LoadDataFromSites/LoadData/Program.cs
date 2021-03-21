using AutoMapper;
using HtmlAgilityPack;
using System;

namespace LoadData
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DataAccessProfile());
            }).CreateMapper();
            var a = new GoHomeByDataLoader(new HtmlWeb(), config);

            a.LoadDataInAds(RentalAdMenu.RealtHouseByLongTermRentalAd);
            Console.ReadKey();
        }
    }
}
