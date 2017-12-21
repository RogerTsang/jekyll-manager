using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace jekyll_manager
{
    public class PostModel : DataTemplate
    {
        public string date { get; set; }
        public string title { get; set; }
        public string ext { get; set; }
        public string filename { get; set; }

        public PostModel(string filename)
        {
            string[] elements = filename.Split('.').First().Split('-');
            this.date = elements[0] + "-" + elements[1] + "-" + elements[2];
            for (int i = 3; i < elements.Length; ++i)
            {
                this.title += elements[i] + " ";
            }
            this.ext = filename.Split('.').Last();
            this.filename = filename;
        }
    }
}
