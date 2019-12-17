using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KnowledgeHUB.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SigninPage : ContentPage
    {
        string _dbPath;
        public SigninPage()
        {
            InitializeComponent();
            _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalizedResources), "The1DB.db3");

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var db = new SQLiteConnection(_dbPath);
                db.CreateTable<Account>();
                var MaxPk = db.Table<Account>().OrderByDescending(c => c.id).Where(c => c.UserName == UsernameEntry.Text).FirstOrDefault();
                int? UserNameError = db.Table<Account>().OrderByDescending(c => c.id).Where(c => c.UserName == UsernameEntry.Text).Count();
                if (UserNameError == null)
                {
                    error1.IsVisible = true;
                    return;
                }
                else
                {
                    error1.IsVisible = false;
                    string hint = db.Table<Account>().Where(rd => rd.UserName == UsernameEntry.Text).Select(ss => ss.Hint).ToString();
                    if (UsernameEntry.Text == MaxPk.UserName && PasswordEntry.Text != MaxPk.Password)
                    {
                        error2.Text = MaxPk.Hint;
                        error2.IsVisible = true;
                    }
                    else
                    {
                        error2.IsVisible = false;
                        Application.Current.Properties["AccountID"] = MaxPk.id;
                        Application.Current.Properties["UserName"] = MaxPk.UserName;
                        Application.Current.Properties["F"] = MaxPk.FirstName;
                        Application.Current.Properties["L"] = MaxPk.LastName;
                        await Navigation.PushAsync(new HomeMasterDetailPage());
                    }
                }
            }
            catch (Exception)
            {


            }





        }
    }
}