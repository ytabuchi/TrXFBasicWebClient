using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFBasicWebClient.Models;

namespace XFBasicWebClient.Views
{
    public partial class SummaryPage : ContentPage
    {
        ObservableCollection<Person> _people = new ObservableCollection<Person>();

        public SummaryPage()
        {
            InitializeComponent();
            this.BindingContext = _people;

            addButton.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new DetailPage(null));
            };

            clearButton.Clicked += async (sender, e) =>
            {
                var people = await WebApiClient.Instance.GetPeopleAsync();
                foreach (var person in people)
                {
                    await WebApiClient.Instance.DeletePersonAsync(person);
                }

                _people.Clear();
            };

            peopleList.Refreshing += async (sender, e) =>
            {
                var webPeople = await WebApiClient.Instance.GetPeopleAsync();

                _people.Clear();
                foreach (var person in webPeople)
                {
                    _people.Add(person);
                }

                peopleList.IsRefreshing = false;
            };

            peopleList.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
            {
                var person = e.SelectedItem as Person;
                if (person == null)
                    return;

                await Navigation.PushAsync(new DetailPage(person));

                peopleList.SelectedItem = null;
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var webPeople = await WebApiClient.Instance.GetPeopleAsync();

            _people.Clear();
            foreach (var person in webPeople)
            {
                _people.Add(person);
            }
        }
    }
}
