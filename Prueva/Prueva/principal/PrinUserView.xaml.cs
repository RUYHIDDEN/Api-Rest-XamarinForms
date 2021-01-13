using Newtonsoft.Json;
using Prueva.modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prueva.principal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrinUserView : ContentPage
    {
        public PrinUserView()
        {
            InitializeComponent();
            ListUsuarios.ItemTapped += ListUsuarios_ItemTapped;
        }

        private void ListUsuarios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var obj = (User)e.Item;
            var Id_user = obj.Id_User;
            try
            {
                Navigation.PushAsync(new principal.DetaUserView(Id_user));
            }
            catch (Exception) { throw; }
        }

        private void MenuItem1_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new principal.AddUserView());
        }

        private void MenuItem2_Clicked(object sender, EventArgs e)
        {
            OnAppearing();
        }
        protected override void OnAppearing()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("http://192.168.1.69:3000/user");
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("Accpet", "application/json");
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<List<User>>(content);
                        ListUsuarios.ItemsSource = resultado;
                    }
                    else
                    {
                        await DisplayAlert("Notificación", "Error al conectar", "OK");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("Notificación", "Error al conectar", "OK");
                    await Navigation.PopToRootAsync();
                }
            });
        }
    }
}