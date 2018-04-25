using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFBasicWebClient.Models;

namespace XFBasicWebClient.Views
{
    public partial class DetailPage : ContentPage
    {
        Person _person;

        public DetailPage(Person person)
        {
            InitializeComponent();

            if (person != null)
                _person = person;
            else
                _person = new Person();

            this.BindingContext = _person;

            saveButton.Clicked += async (sender, e) =>
            {
                var updatePerson = new Person
                {
                    Id = int.Parse(IdData.Text),
                    Name = NameData.Text,
                    Birthday = BirthdayData.Date
                };

                if (updatePerson.Id == 0)
                {
                    await WebApiClient.Instance.PostPersonAsync(updatePerson);
                }
                else
                {
                    await WebApiClient.Instance.UpdatePersonAsync(updatePerson);
                }

                await Navigation.PopAsync();
            };

            deleteButton.Clicked += async (sender, e) =>
            {
                await WebApiClient.Instance.DeletePersonAsync(_person);
                await Navigation.PopAsync();
            };
        }
    }
}
