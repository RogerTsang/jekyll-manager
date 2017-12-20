using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace jekyll_manager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DataRecord data;

        public MainPage()
        {
            this.InitializeComponent();
            this.InitializeDataRecord();
            this.InitialzeView();
        }

        /// <summary>
        /// Find the jekyll project folder
        /// </summary>
        /// <param name="sender">The activator</param>
        /// <param name="e">The click event</param>
        private async void Browse_Button_Click(object sender, RoutedEventArgs e)
        {
            /* Picking jekyll directory */
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                /* Store jekyll location in access token */
                Windows.Storage.AccessCache.StorageApplicationPermissions
                    .FutureAccessList.AddOrReplace("Jekyll_Directory_Token", folder);

                /* Update jekyll project location */
                data.location = folder.Path;

                /* Update post browser component */
                Post_View_Update(data.location);
            }

            Path_Text.Text = data.location;
        }

        /// <summary>
        /// Load a post file into content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Post_View_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            data.content = string.Empty;
            if (e.AddedItems != null && e.AddedItems.Count >= 1)
            {
                string post_path = Path.Combine(data.location_post, (string)e.AddedItems[0]);
                try
                {
                    StorageFile post_file = await StorageFile.GetFileFromPathAsync(post_path);
                    data.content = await FileIO.ReadTextAsync(post_file);
                }
                catch {
                    return;
                }
            }

            Content_Text.Text = data.content;
        }

        /// <summary>
        /// Update the post list windows according to jeklly folder
        /// </summary>
        /// <param name="path">The jekyll project path</param>
        private async void Post_View_Update(string path)
        {
            string post_path = Path.Combine(path, "_posts");
            StorageFolder post_folder = null;
            try
            {
                post_folder = await StorageFolder.GetFolderFromPathAsync(post_path);
            }
            catch
            {
                data.content = string.Empty;
                Content_Text.Text = data.content;
                Post_View.ItemsSource = null;
                return;
            }

            /* Update jekyll post location */
            data.location_post = post_folder.Path;

            /* Query files */
            List<string> file_filter = new List<string>();
            file_filter.Add(".markdown");
            file_filter.Add(".md");
            file_filter.Add(".html");
            QueryOptions query_option = new QueryOptions(CommonFileQuery.OrderByName, file_filter);
            StorageFileQueryResult post_query = post_folder.CreateFileQueryWithOptions(query_option);
            IReadOnlyList<StorageFile> post_files = await post_query.GetFilesAsync();

            /* Update data model and application view */
            data.BindFiles(post_files.Select(z => z.Name));
            Post_View.ItemsSource = data.files;
        }

        private void InitializeDataRecord()
        {
            data = new DataRecord();
        }

        private void InitialzeView()
        {
            Path_Text.Text = data.location;
            Content_Text.Text = data.content;
        }
    }
}
