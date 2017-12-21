using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace jekyll_manager
{
    public class DataRecord
    {
        public string location { get; set; }
        public string location_post { get; set; }
        public ObservableCollection<PostModel> files { get; set; }
        public string content { get; set; }

        public DataRecord()
        {
            location = "(Empty)";
            location_post = string.Empty;
            files = new ObservableCollection<PostModel>();
            content = string.Empty;
        }

        public void BindFiles(IEnumerable<string> list)
        {
            files.Clear();
            foreach (string filename in list)
            {
                files.Add(new PostModel(filename));
            }
        }

        public void ClearFiles()
        {
            files.Clear();
        }
    }
}
