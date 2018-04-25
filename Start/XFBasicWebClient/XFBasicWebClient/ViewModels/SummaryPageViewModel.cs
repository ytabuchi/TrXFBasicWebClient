using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XFBasicWebClient.Models;

namespace XFBasicWebClient.ViewModels
{
    public class SummaryPageViewModel : ViewModelBase
    {
        //public ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();

        //private bool _isBusy;
        //public bool IsBusy
        //{
        //    get { return _isBusy; }
        //    set
        //    {
        //        _isBusy = value;
        //        OnPropertyChanged();
        //        AddCommand.ChangeCanExecute();
        //        ClearCommand.ChangeCanExecute();
        //        RefreshCommand.ChangeCanExecute();
        //    }
        //}

        //public Command AddCommand { get; private set; }
        //public Command ClearCommand { get; private set; }
        //public Command RefreshCommand { get; private set; }

        //public SummaryPageViewModel()
        //{
        //    this.AddCommand = new Command(
        //        async () => await AddPersonAsync(),
        //        () => !IsBusy);

        //    this.ClearCommand = new Command(
        //        async () => await ClearPeopleAsync(),
        //        () => !IsBusy);

        //    this.RefreshCommand = new Command(
        //        async () => await RefreshAsync(),
        //        () => !IsBusy);
        //}

        //private async Task AddPersonAsync()
        //{
        //    IsBusy = true;

        //    var person = new Person
        //    {
        //        Name = "sample",
        //        Birthday = DateTime.Now
        //    };
        //    await WebApiClient.Instance.PostPersonAsync(person);

        //    var people = await WebApiClient.Instance.GetPeopleAsync();
        //    People.Clear();
        //    foreach (var p in people)
        //    {
        //        People.Add(p);
        //    }

        //    IsBusy = false;
        //}

        //private async Task ClearPeopleAsync()
        //{
        //    IsBusy = true;

        //    var people = await WebApiClient.Instance.GetPeopleAsync();
        //    foreach (var p in people)
        //    {
        //        await WebApiClient.Instance.DeletePersonAsync(p);
        //    }
        //    People.Clear();

        //    IsBusy = false;
        //}

        //private async Task RefreshAsync()
        //{
        //    IsBusy = true;

        //    var people = await WebApiClient.Instance.GetPeopleAsync();
        //    People.Clear();
        //    foreach (var p in people)
        //    {
        //        People.Add(p);
        //    }

        //    IsBusy = false;
        //}
    }
}
