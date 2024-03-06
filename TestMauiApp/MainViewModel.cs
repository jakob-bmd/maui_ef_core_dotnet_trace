using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
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

                    //outcomment the following two lines so dotnet-trace works
                    await db.Blogs.AddAsync(blog);
                    await db.SaveChangesAsync();
                    //--------------------------------------------------------
                    counter++;
                    ResultText = $"Done {counter} time/s";
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
    }
}
