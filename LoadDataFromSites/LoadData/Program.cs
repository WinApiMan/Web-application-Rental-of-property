using HtmlAgilityPack;
using System;

namespace LoadData
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var a = new SiteDataLoader(new HtmlWeb());

            a.LoadDataInAds();
            Console.ReadKey();
        }
    }
}
