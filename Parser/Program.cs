using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net.Http;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string car = "Volkswagen";
                    string url = $"https://en.wikipedia.org/wiki/{car}";


                    var html = httpClient.GetStringAsync(url);
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html.Result);
                    Console.WriteLine($"Headquarter is located in {GetCountry(htmlDocument)}");
                    Console.Write($"Image of brand {car} is : {GetCarImage(htmlDocument)}\n");
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }


            }
        }

        static string GetCountry(HtmlDocument htmlDocument)
        {
            string country = null;
            string path = "//div[@class='country-name']";
            LabelGenerall:
            var generall = htmlDocument.DocumentNode.SelectNodes(path);
            if (generall != null)
            {
                country = generall[0].InnerText;
            }
            else
            {
                path = "//tr//td[@class='label']";
                goto LabelGenerall;

            }
            var locality = generall[0].InnerText;

            var countrySplit = locality.Split(" ");
            country = countrySplit[^1];
            return country;
        }
        static string GetCarImage(HtmlDocument htmlDocument)
        {
            string path = "//table[@class='infobox vcard']//tr//td[@class='logo']";

            var generall = htmlDocument.DocumentNode.SelectNodes(path);

            string image = generall[0].ChildNodes.FirstOrDefault(x => x.Name == "a")
                                     .ChildNodes.FirstOrDefault(x => x.Name == "img")
                                     .Attributes["src"]
                                     .Value;
            return image;
        }
        static string GetHeadquartersFromLink(HtmlDocument html)
        {
            string locality = "";
            var city = html.DocumentNode.SelectNodes("//div[@class='locality']");
            var country = html.DocumentNode.SelectNodes("//div[@class='country-name']");

            locality = city == null
                ? null
                : city[0].ChildNodes.Where(a => a.Name == "a").First().Attributes["title"].Value + " ";
            locality += country == null ? null : country[0].InnerText;
            return locality;
        }
    }
}
