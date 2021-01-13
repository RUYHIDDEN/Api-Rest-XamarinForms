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
    public partial class AddUserView : ContentPage
    {
        public AddUserView()
        {
            InitializeComponent();
            btnGuardar.Clicked += BtnGuardar_Clicked;
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(txtUsuario.Text))
                {
                    await DisplayAlert("Advertencia", "El campo Usuario es obligatorio", "OK");
                }
                else if (String.IsNullOrWhiteSpace(txtContrasena.Text))
                {
                    await DisplayAlert("Advertencia", "El campo Contraseña es obligatorio", "OK");
                }
                else if (String.IsNullOrWhiteSpace(txtTipo.Text))
                {
                    await DisplayAlert("Advertencia", "El campo Tipoa es obligatorio", "OK");
                }
                else
                {
                    var user = new User();
                    user.Id_User = 0;
                    user.Usuario = txtUsuario.Text;
                    user.Contrasena = txtContrasena.Text;
                    user.Tipo = txtTipo.Text;

                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("http://192.168.1.69:3000/user");
                    request.Method = HttpMethod.Post;
                    request.Headers.Add("Accpet", "application/json");
                    var payload = JsonConvert.SerializeObject(user);
                    HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
                    request.Content = c;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        await DisplayAlert("Notificación", "El usuario se a creado con éxito :" + txtUsuario.Text, "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Notificación", "Error al conectar", "OK");
                        await Navigation.PopToRootAsync();
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Notificación", "Error al conectar", "OK");
                await Navigation.PopToRootAsync();
            }
        }
    }
}