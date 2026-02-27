using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class News
    {
        public string Img { get; set; }
        public DateTime Date { get; set; }
        public string Badge { get; set; }
        public string Title { get; set; }
        public News(string img, DateTime date, string badge, string title)
        {
            this.Img = img;
            this.Date = date;
            this.Badge = badge;
            this.Title = title;
        }
    }
}
