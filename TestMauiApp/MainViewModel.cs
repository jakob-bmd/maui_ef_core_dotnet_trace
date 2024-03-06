using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using TestMauiApp.Model;

namespace TestMauiApp
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isBusy = false;

        [ObservableProperty]
        private string _resultText = "Not run yet.";

        private int counter = 0;

        public MainViewModel() : base()
        {
            using var db = new BloggingContext();
            db.Database.Migrate();
        }

        [RelayCommand]
        public async Task SaveEfCoreData()
        {
            IsBusy = true;

            try
            {
                using (var db = new BloggingContext())
                {
                    Console.WriteLine("Adding a new blog");
                    Blog blog = new Blog { Url = "http://blogs.msdn.com/adonet" };
                    await db.Blogs.AddAsync(blog);
                    await db.SaveChangesAsync();
                    counter++;
                    ResultText = $"Done {counter} time/s";

                    // Create
                    //db.Add(blog);
                    //db.SaveChanges();

                    // Read
                    /*Console.WriteLine("Querying for a blog");
                    var blog = db.Blogs
                        .OrderBy(b => b.BlogId)
                        .First();

                    // Update
                    Console.WriteLine("Updating the blog and adding a post");
                    blog.Url = "https://devblogs.microsoft.com/dotnet";
                    blog.Posts.Add(
                        new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
                    db.SaveChanges();

                    // Delete
                    Console.WriteLine("Delete the blog");
                    db.Remove(blog);
                    db.SaveChanges();*/
                }
            }
            catch (Exception ex)
            {
                ResultText = "Failed: " + ex.Message;
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }

        }

        //[RelayCommand]
        //public async Task ShowDialogPromp()
        //{
        //    await MainThread.InvokeOnMainThreadAsync(() => Application.Current.MainPage.DisplayAlert("This is a test", "to check dotnet-trace", "OK"));
        //}
    }
}
