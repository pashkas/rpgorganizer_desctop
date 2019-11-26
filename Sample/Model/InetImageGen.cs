using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Model
{
    public static class InetImageGen
    {
        static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public static byte[] ImageByWord(string id)
        {
            // Поиск в базе
            string pathToImgBase = @"Images\ElementsImages";
            Fastenshtein.Levenshtein lev = new Fastenshtein.Levenshtein(id.ToLower());
            var fls = Directory.GetFiles(pathToImgBase).Select(n => new { name = Path.GetFileNameWithoutExtension(n), path = n, dist = lev.DistanceFrom(Path.GetFileNameWithoutExtension(n).ToLower()) }).OrderBy(n => n.dist).ToList();

            var fod = fls.FirstOrDefault();
            if (fod?.dist <= 2)
            {
                return StaticMetods.pathToImage(fod.path);
            }
            else
            {
                var tryes = 0;
                bool isSuccess = false;
                byte[] img = null;

                while (!isSuccess && tryes < 10)
                {
                    try
                    {
                        string html = GetHtmlCode(id);
                        List<string> urls = GetUrls(html).ToList();

                        int randomUrl = rnd.Next(0, urls.Count - 1);

                        string luckyUrl = urls[randomUrl];

                        img = GetImage(luckyUrl);

                        isSuccess = true;
                    }
                    catch
                    {
                        tryes++;
                    }
                }

                return img;
            }
        }

        private static string GetHtmlCode(string word)
        {
            string url = "https://www.google.com/search?q=" + word + "&tbm=isch";
            string data = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return "";
                using (var sr = new StreamReader(dataStream))
                {
                    data = sr.ReadToEnd();
                }
            }
            return data;
        }

        private static byte[] GetImage(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return null;
                using (var sr = new BinaryReader(dataStream))
                {
                    byte[] bytes = sr.ReadBytes(100000000);

                    return bytes;
                }
            }
        }

        private static List<string> GetUrls(string html)
        {
            var urls = new List<string>();

            int ndx = html.IndexOf("\"ou\"", StringComparison.Ordinal);

            while (ndx >= 0)
            {
                ndx = html.IndexOf("\"", ndx + 4, StringComparison.Ordinal);
                ndx++;
                int ndx2 = html.IndexOf("\"", ndx, StringComparison.Ordinal);
                string url = html.Substring(ndx, ndx2 - ndx);
                urls.Add(url);
                ndx = html.IndexOf("\"ou\"", ndx2, StringComparison.Ordinal);
            }
            return urls;
        }
    }
}
