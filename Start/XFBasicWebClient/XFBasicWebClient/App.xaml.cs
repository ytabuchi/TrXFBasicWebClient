using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFBasicWebClient.Views;

using Xamarin.Forms;

namespace XFBasicWebClient
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // App.xamlの設定で、AppThemeが読み込まれます。
            MainPage = new NavigationPage(new SummaryPage());
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {

        }
    }
}
