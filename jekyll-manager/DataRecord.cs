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
        public ObservableCollection<string> files { get; set; }
        public string content { get; set; }

        public DataRecord()
        {
            location = "(Empty)";
            location_post = string.Empty;
            files = new ObservableCollection<string>();
            content = string.Empty;
        }

        public void BindFiles(IEnumerable<string> list)
        {
            files = new ObservableCollection<string>(list);
        }

        public void ClearFiles()
        {
            files.Clear();
        }
    }
}
