using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;

using System.Threading.Tasks;
using System.Timers;
using Common;
using HtmlAgilityPack;

namespace Parser
{
    public class Program
    {
        public static float TimeUpdate = 60;
        public static int Month = 1;
        static string Url = "https://pspu.ru/about_the_university/news/";
        public static List<News> News = new List<News>();
        public static string DebugFile = "debug.txt";
        static Stream StreamFile = File.Create(DebugFile);
        static TextWriterTraceListener WriterTraceListener = new TextWriterTraceListener(StreamFile);
        static Timer Timer;
        static async Task Main(string[] args)
        {
            Trace.Listeners.Add(WriterTraceListener);
            Trace.AutoFlush= true;
            Console.Write("укажите частоту обновления");
            string timeUpdate = Console.ReadLine();
            if(string.IsNullOrEmpty(timeUpdate)== false)
             TimeUpdate = Convert.ToInt32(timeUpdate);
            Console.Write("укажите период контента");
            string period = Console.ReadLine();
            if (string.IsNullOrEmpty(period) == false)
             Month = Convert.ToInt32(period);
            SyncGetContent();
            Timer = new Timer(TimeUpdate * 1000);
            Timer.Elapsed += TimerTrick;
            Timer.Start();
            StartServer();
            Console.Read();
            Trace.Flush();       
        }
        private static void TimerTrick(object sender, EventArgs e) =>SyncGetContent();
        public static async void SyncGetContent()
        {
            string Content = await GetContent();
            ParseContent(Content);
        }
        public static async Task<string> GetContent()
        {
            Trace.WriteLine(DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy") + "выполнение запроса");
            try
            {
                WebRequest Request = WebRequest.Create(Url);
                using (HttpWebResponse Response = (HttpWebResponse)Request.GetResponse())
                { 
                    string Content = new StreamReader(Response.GetResponseStream()).ReadToEnd();
                    Trace.WriteLine(DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy") + "контент получен");
                    return Content;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy") + "ошибка получения контента");
                return null;
            }
        }
        public static void StartServer()
        {

        }
        public static void ParseContent (string content)
        {
            try
            {
                Trace.WriteLine(DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy") + "парсинг полученного контента");
                News.Clear();
                var Html = new HtmlDocument();
                Html.LoadHtml(content);
                var Document = Html.DocumentNode;
                //HtmlNodeCollection ContentNews = Document.SelectNodes("//div[contains"@class = ]");
                foreach (var ContentNew in ContentNews) 
                {
                    string Img = ContentNew.SelectSingleNode("").InnerText;
                    string Date = ContentNew.SelectSingleNode("").InnerText;
                    string Badge = ContentNew.SelectSingleNode("").InnerText;
                    string Title = ContentNew.SelectSingleNode("").InnerText;
                    DateTime DateNew = DateTime.Parse(Date);
                    if (DateNew.Month != Month) continue;
                    News.Add(new Common.News(Img, DateNew, Badge, Title));
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy") + "ошибка парсинга");
            }
        }

    }
}
