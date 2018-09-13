using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HolaMoviles.Modelos;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HolaMoviles
{
    
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<object> Items { get; set; } = new ObservableCollection<object> { 1, "2", true, false };
        public Command AgregarComando { get; set; }

        public MainPage()
        {
            AgregarComando = new Command(async () => await CargaItems());
            InitializeComponent();
            ButtonAgregar.Clicked += ButtonAgregar_Click;
        }
        protected async override void OnAppearing()
        {
            await CargaItems();
        }

        private async void ButtonAgregar_Click (object sender, EventArgs arg)
        {
           await CargaItems();
        }   
        private async Task CargaItems()
        {
            if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Advertencia", "No hay internet", "Cerrar");
            }

            IsBusy = true;
            Items.Clear();
            //await Task.Delay(2500);
            //Items.Add($"Elemento #{Items.Count}");
            //await DisplayAlert("Tutulo", "Hola", "Cerrar"); 

            var productos = await CargaProductos();
            foreach(var items in productos) 
            {
                Items.Add(items);
            }

            IsBusy = false;

        }

        private async Task<Producto[]> CargaProductos()
        {
            var cliente = new HttpClient();

            cliente.BaseAddress = new Uri(App.WebServiceUrl);
             
            var json = await cliente.GetStringAsync("api/products");
            var resultado = JsonConvert.DeserializeObject<Producto[]>(json);
            return resultado;
        }
    }
}
