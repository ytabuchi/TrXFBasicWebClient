using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
