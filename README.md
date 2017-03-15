# Xamarinトレーニング用レポジトリ

### コードスニペット

Webサイトまたはmarkdownプレビューとして表示している場合は、それぞれの枠内をコピー＆ペーストしてください。

Web API

### Person.cs


```csharp
[JsonProperty(PropertyName = "id")]
public int Id { get; set; }
[JsonProperty(PropertyName = "name")]
public string Name { get; set; }
[JsonProperty(PropertyName = "birthday")]
public DateTimeOffset Birthday { get; set; }
```

### WebApiClient.cs


```csharp
class AuthResult
{
    [JsonProperty(PropertyName = "access_token")]
    public string AccessToken { get; set; }
}
```


```csharp
public static WebApiClient Instance { get; set; } = new WebApiClient();
private static HttpClient client = new HttpClient();

private WebApiClient()
{
}
```

```csharp
private Uri baseAddress = Helpers.ApiKeys.baseAddress;
private string Token = "";
private object locker = new object();

private readonly string _name = "admin";
private readonly string _password = "p@ssw0rd";
```

```csharp
private void Initialize(string name, string password)
{
    lock (locker)
    {
        if (string.IsNullOrEmpty(Token))
        {
            try
            {
                client.BaseAddress = baseAddress;

                var authContent = new StringContent($"grant_type=password&username={name}&password={password}");
                authContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var authResponse = client.PostAsync("/Token", authContent).Result;
                authResponse.EnsureSuccessStatusCode();
                var authResult = authResponse.Content.ReadAsStringAsync().Result;

                Token = JsonConvert.DeserializeObject<AuthResult>(authResult).AccessToken;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"【InitializeError】{ex.Source},{ex.Message},{ex.InnerException}");
            }
        }
    }
}
```

```csharp
public async Task<ObservableCollection<Person>> GetPeopleAsync()
{
    Initialize(_name, _password);

    client.BaseAddress = baseAddress;
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

    try
    {
        var response = await client.GetAsync("api/People");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ObservableCollection<Person>>(json);
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"【GetError】{ex.Source},{ex.Message},{ex.InnerException}");

        return null;
    }
}
```

```csharp
public async Task<int> PostPersonAsync(Person person)
{
    Initialize(_name, _password);

    client.BaseAddress = baseAddress;
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

    try
    {
        var content = new StringContent(JsonConvert.SerializeObject(person));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await client.PostAsync("api/People", content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        var id = JsonConvert.DeserializeObject<Person>(result).Id;

        return id;
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"【PostError】{ex.Source},{ex.Message},{ex.InnerException}");
        throw;
    }
}
```

```csharp
public async Task<bool> UpdatePersonAsync(Person person)
{
    Initialize(_name, _password);

    client.BaseAddress = baseAddress;
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

    try
    {
        var content = new StringContent(JsonConvert.SerializeObject(person));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await client.PutAsync($"api/People/{person.Id}", content);
        response.EnsureSuccessStatusCode();

        return true;
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"【UpdateError】{ex.Source},{ex.Message},{ex.InnerException}");

        return false;
    }
}
```

```csharp
public async Task<bool> DeletePersonAsync(Person person)
{
    Initialize(_name, _password);

    client.BaseAddress = baseAddress;
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

    try
    {
        var response = await client.DeleteAsync($"api/People/{person.Id}");
        response.EnsureSuccessStatusCode();

        return true;
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"【DeleteError】{ex.Source},{ex.Message},{ex.InnerException}");

        return false;
    }
}
```

### WebApiClient.cs完成形(現時点)

```csharp
namespace XFBasicWebClient.Models
{
    public class WebApiClient
    {
        public static WebApiClient Instance { get; set; } = new WebApiClient();
        private static HttpClient client = new HttpClient();

        private Uri baseAddress = Helpers.ApiKeys.BaseAddress;
        private string Token = "";
        private object locker = new object();

        private readonly string _name = "admin";
        private readonly string _password = "p@ssw0rd";

        private WebApiClient()
        {
        }

        private void Initialize(string name, string password)
        {
            lock (locker)
            {
                if (string.IsNullOrEmpty(Token))
                {
                    try
                    {
                        client.BaseAddress = baseAddress;

                        var authContent = new StringContent($"grant_type=password&username={name}&password={password}");
                        authContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        var authResponse = client.PostAsync("/Token", authContent).Result;
                        authResponse.EnsureSuccessStatusCode();
                        var authResult = authResponse.Content.ReadAsStringAsync().Result;

                        Token = JsonConvert.DeserializeObject<AuthResult>(authResult).AccessToken;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"【InitializeError】{ex.Source},{ex.Message},{ex.InnerException}");
                    }
                }
            }
        }

        public async Task<ObservableCollection<Person>> GetPeopleAsync()
        {
            Initialize(_name, _password);

            client.BaseAddress = baseAddress;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            try
            {
                var response = await client.GetAsync("api/People");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Person>>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"【GetError】{ex.Source},{ex.Message},{ex.InnerException}");

                return null;
            }
        }

        public async Task<int> PostPersonAsync(Person person)
        {
            Initialize(_name, _password);

            client.BaseAddress = baseAddress;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(person));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync("api/People", content);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var id = JsonConvert.DeserializeObject<Person>(result).Id;

                return id;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"【PostError】{ex.Source},{ex.Message},{ex.InnerException}");
                throw;
            }
        }

        public async Task<bool> UpdatePersonAsync(Person person)
        {
            Initialize(_name, _password);

            client.BaseAddress = baseAddress;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(person));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PutAsync($"api/People/{person.Id}", content);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"【UpdateError】{ex.Source},{ex.Message},{ex.InnerException}");

                return false;
            }
        }

        public async Task<bool> DeletePersonAsync(Person person)
        {
            Initialize(_name, _password);

            client.BaseAddress = baseAddress;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            try
            {
                var response = await client.DeleteAsync($"api/People/{person.Id}");
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"【DeleteError】{ex.Source},{ex.Message},{ex.InnerException}");

                return false;
            }
        }
    }

    class AuthResult
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
    }
}
```

### SummaryPage.xaml

```xml
<ContentPage x:Class="XFBasicWebClient.Views.SummaryPage"
              xmlns="http://xamarin.com/schemas/2014/forms"
              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
              Title="Summary Page">
    <AbsoluteLayout>
    </AbsoluteLayout>
</ContentPage>
```

```xml
<AbsoluteLayout>
    <ListView x:Name="peopleList"
              AbsoluteLayout.LayoutFlags="All"
              AbsoluteLayout.LayoutBounds="0,0,1,1"
              HasUnevenRows="True"
              IsPullToRefreshEnabled="True"
              ItemsSource="{Binding}">
    </ListView>

    <Button x:Name="addButton"
            AbsoluteLayout.LayoutFlags="PositionProportional,WidthProportional"
            AbsoluteLayout.LayoutBounds="0,1,0.5,AutoSize"
            Margin="8"
            Style="{DynamicResource ButtonStyleTransparent}"
            Text="Add" />
    <Button x:Name="clearButton"
            AbsoluteLayout.LayoutFlags="PositionProportional,WidthProportional"
            AbsoluteLayout.LayoutBounds="1,1,0.5,AutoSize"
            Margin="8"
            Style="{DynamicResource ButtonStyleTransparent}"
            Text="Clear all data" />

</AbsoluteLayout>
```


```xml
<ListView.ItemTemplate>
    <DataTemplate>
        <ViewCell>
        </ViewCell>
    </DataTemplate>
</ListView.ItemTemplate>
```

```xml
<StackLayout Orientation="Horizontal">
    <Label Style="{DynamicResource LabelStyleId}" Text="{Binding Id}" />
    <StackLayout Padding="5">
        <Label Text="{Binding Name}" VerticalTextAlignment="Center" />
        <Label Text="{Binding Birthday, StringFormat='{0:yyyy/MM/dd HH:mm}'}"
                TextColor="Gray"
                VerticalTextAlignment="End" />
    </StackLayout>
</StackLayout>
```

### SummaryPage.xaml完成形(現時点)

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage x:Class="XFBasicWebClient.Views.SummaryPage"
             xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             Title="Summary Page">
    <AbsoluteLayout>
        <ListView x:Name="peopleList"
                  AbsoluteLayout.LayoutFlags="All"
                  AbsoluteLayout.LayoutBounds="0,0,1,1"
                  HasUnevenRows="True"
                  IsPullToRefreshEnabled="True"
                  ItemsSource="{Binding}">
          <ListView.ItemTemplate>
              <DataTemplate>
                  <ViewCell>
                      <StackLayout Orientation="Horizontal">
                          <Label Style="{DynamicResource LabelStyleId}" Text="{Binding Id}" />
                          <StackLayout Padding="5">
                              <Label Text="{Binding Name}" VerticalTextAlignment="Center" />
                              <Label Text="{Binding Birthday, StringFormat='{0:yyyy/MM/dd HH:mm}'}"
                                      TextColor="Gray"
                                      VerticalTextAlignment="End" />
                          </StackLayout>
                      </StackLayout>
                  </ViewCell>
              </DataTemplate>
          </ListView.ItemTemplate>
        </ListView>

        <Button x:Name="addButton"
                AbsoluteLayout.LayoutFlags="PositionProportional,WidthProportional"
                AbsoluteLayout.LayoutBounds="0,1,0.5,AutoSize"
                Margin="8"
                Style="{DynamicResource ButtonStyleTransparent}"
                Text="Add" />
        <Button x:Name="clearButton"
                AbsoluteLayout.LayoutFlags="PositionProportional,WidthProportional"
                AbsoluteLayout.LayoutBounds="1,1,0.5,AutoSize"
                Margin="8"
                Style="{DynamicResource ButtonStyleTransparent}"
                Text="Clear all data" />

    </AbsoluteLayout>
</ContentPage>
```


### SummaryPage.xaml.cs

```csharp
ObservableCollection<Person> _people = new ObservableCollection<Person>();
```

```csharp
this.BindingContext = _people;
```

```csharp
addButton.Clicked += async (sender, e) =>
{
    var p = new Person
    {
        Name = "sample",
        Birthday = DateTime.Now
    };
    await WebApiClient.Instance.PostPersonAsync(p);

    var webPeople = await WebApiClient.Instance.GetPeopleAsync();
    _people.Clear();
    foreach (var person in webPeople)
    {
        _people.Add(person);
    }
};
```

```csharp
clearButton.Clicked += async (sender, e) =>
{
    var people = await WebApiClient.Instance.GetPeopleAsync();
    foreach (var person in people)
    {
        await WebApiClient.Instance.DeletePersonAsync(person);
    }

    _people.Clear();
};
```

```csharp
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
```

```csharp
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
```


### SummaryPage.xaml.cs完成形(現時点)

```csharp
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
                var p = new Person
                {
                    Name = "sample",
                    Birthday = DateTime.Now
                };
                await WebApiClient.Instance.PostPersonAsync(p);

                var webPeople = await WebApiClient.Instance.GetPeopleAsync();
                _people.Clear();
                foreach (var person in webPeople)
                {
                    _people.Add(person);
                }
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
```


### DetailPage.xaml

```xml
<StackLayout VerticalOptions="FillAndExpand">
</StackLayout>
```


```xml
<Grid RowSpacing="8" VerticalOptions="FillAndExpand">
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto" />
        <RowDefinition Height="Auto" />
        <RowDefinition Height="Auto" />
        <RowDefinition Height="Auto" />
    </Grid.RowDefinitions>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="120" />
        <ColumnDefinition Width="*" />
    </Grid.ColumnDefinitions>

    <Label Grid.Row="0"
            Grid.Column="0"
            Text="Id"
            VerticalTextAlignment="Center" />
    <Label x:Name="IdData"
            Grid.Row="0"
            Grid.Column="1"
            Text="{Binding Id}" />
    <Label Grid.Row="1"
            Grid.Column="0"
            Text="Name"
            VerticalTextAlignment="Center" />
    <Entry x:Name="NameData"
            Grid.Row="1"
            Grid.Column="1"
            Text="{Binding Name}" />
    <Label Grid.Row="2"
            Grid.Column="0"
            Text="Birthday"
            VerticalTextAlignment="Center" />
    <DatePicker x:Name="BirthdayData"
                Grid.Row="2"
                Grid.Column="1"
                Date="{Binding Birthday}"
                Format="yyyy/MM/dd" />
</Grid>
```

```xml
<StackLayout Padding="4"
              Orientation="Horizontal"
              Spacing="8">
    <Button x:Name="saveButton"
            HorizontalOptions="FillAndExpand"
            Style="{DynamicResource ButtonStyleGreen}"
            Text="Save" />
    <Button x:Name="deleteButton"
            HorizontalOptions="FillAndExpand"
            Style="{DynamicResource ButtonStyleRed}"
            Text="Delete" />
</StackLayout>
```

### DetailPage.xaml完成形(現時点)

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<ContentPage x:Class="XFBasicWebClient.Views.DetailPage"
             xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             Title="Detail Page"
             Padding="12">
    <StackLayout VerticalOptions="FillAndExpand">
        <Grid RowSpacing="8" VerticalOptions="FillAndExpand">
            <Grid.RowDefinitions>
                <RowDefinition Height="Auto" />
                <RowDefinition Height="Auto" />
                <RowDefinition Height="Auto" />
                <RowDefinition Height="Auto" />
            </Grid.RowDefinitions>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="120" />
                <ColumnDefinition Width="*" />
            </Grid.ColumnDefinitions>

            <Label Grid.Row="0"
                   Grid.Column="0"
                   Text="Id"
                   VerticalTextAlignment="Center" />
            <Label x:Name="IdData"
                   Grid.Row="0"
                   Grid.Column="1"
                   Text="{Binding Id}" />
            <Label Grid.Row="1"
                   Grid.Column="0"
                   Text="Name"
                   VerticalTextAlignment="Center" />
            <Entry x:Name="NameData"
                   Grid.Row="1"
                   Grid.Column="1"
                   Text="{Binding Name}" />
            <Label Grid.Row="2"
                   Grid.Column="0"
                   Text="Birthday"
                   VerticalTextAlignment="Center" />
            <DatePicker x:Name="BirthdayData"
                        Grid.Row="2"
                        Grid.Column="1"
                        Date="{Binding Birthday}"
                        Format="yyyy/MM/dd" />
        </Grid>

        <StackLayout Padding="4"
                     Orientation="Horizontal"
                     Spacing="8">
            <Button x:Name="saveButton"
                    HorizontalOptions="FillAndExpand"
                    Style="{DynamicResource ButtonStyleGreen}"
                    Text="Save" />
            <Button x:Name="deleteButton"
                    HorizontalOptions="FillAndExpand"
                    Style="{DynamicResource ButtonStyleRed}"
                    Text="Delete" />
        </StackLayout>

    </StackLayout>
</ContentPage>
```


### DetailPage.xaml.cs

```csharp
Person _person;
```


```csharp
public DetailPage(Person person)
{
    InitializeComponent();

    if (person != null)
        _person = person;
    else
        _person = new Person();

    this.BindingContext = _person;
```

```csharp
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
```


### DetailPage.xaml.cs完成形(現時点)

```csharp
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
```

### SummaryPage.xaml.cs

```csharp
addButton.Clicked += async (sender, e) =>
{
    await Navigation.PushAsync(new DetailPage(null));
};
```

```csharp
peopleList.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
{
    var person = e.SelectedItem as Person;
    if (person == null)
        return;

    await Navigation.PushAsync(new DetailPage(person));

    peopleList.SelectedItem = null;
};
```
