using HtmlAgilityPack;
using System;

namespace LoadData
{
    public static class Program
    {
        static void Main(string[] args)
        {

            var web = new HtmlWeb();

            var htmlDocument = web.Load("https://gohome.by/ads/view/516796/");

            string bynPrice = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='price big']/span").InnerText;

            Console.WriteLine(bynPrice);
            Console.ReadKey();
        }
    }
}
