namespace Temperaturas
{
    public partial class MainPage : ContentPage
    {
       
        public List<Predefinidos> seteosPredefinidos {  get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        //creo un metodo publico para poder modificar el listview que es private
        public void ActualizarListaPredefinidos()
        {
            SecuenciaSeleccionada.ItemsSource = null;
            SecuenciaSeleccionada.ItemsSource = seteosPredefinidos;
        }


        // evento que maneja la secuencia de trabajo seleccionada



        private void SecuenciaSeleccionada_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Valido si hay algun elemento seleccionado
            if (e.SelectedItem is Predefinidos seleccionado)
            {
                tipoTrabajo.Text = seleccionado.Nombre;
                tiempoCoccion.Text = seleccionado.TiempoDeCoccion.ToString();
                temperaturaCoccion.Text = seleccionado.TempFuncionamiento.ToString() ;
                alarmaPreaviso.Text = seleccionado.AlarmaPrevia.ToString() ;                
            }
            else
            {
                DisplayAlert("Aviso", "No hay un elemento seleccionado.", "OK");
            }
        }

        private void Ingresar_Clicked(object sender, EventArgs e)
        {
            //con this le paso una referencia del main al constructor
            //Navigation.PushAsync(new ingresoPage(this));
            var ingreso = new ingresoPage((nuevoElemento) =>
            {
                seteosPredefinidos.Add(nuevoElemento);
                //ActualizarListaPredefinidos();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ActualizarListaPredefinidos();
                });
                ManejodeArchivos.GuardarDatos("secuencias.json", seteosPredefinidos);
            });

            Navigation.PushAsync(ingreso);

        }

        //controlo la secuencia de coccion y todos sus parametros
        private async void comenzarSecuencia_Clicked(object sender, EventArgs e)
        {
            int duracionTotal = Convert.ToInt16(tiempoCoccion.Text);
            int tiempoTranscurrido = 0;
            verTiempoTranscurrido.Text = tiempoTranscurrido.ToString();

            //establezco la visualizacion del progreso
            progresoCoccion.Progress = tiempoTranscurrido;
            
            if ( Convert.ToInt16(temperatuaActual.Text) < Convert.ToInt16(temperaturaCoccion.Text) * 0.95 )
            {
                await DisplayAlert("aviso", "La temperatrura no es adecuada", "Cerrar");
            }
            else if (Convert.ToInt16(temperatuaActual.Text) > Convert.ToInt16(temperaturaCoccion.Text) * 1.05 )
            {
                await DisplayAlert("aviso", "La temperatura no es adecuada", "Cerrar");
            }
            else
            {
                //controlo el avance min a min
                while ( tiempoTranscurrido <= duracionTotal )
                {
                    double progreso = (double) tiempoTranscurrido / (double) duracionTotal;
                    progresoCoccion.Progress = progreso;

                    verTiempoTranscurrido.Text = tiempoTranscurrido.ToString();
                    
                    //progresoCoccion.ProgressTo(progreso, 10000, Easing.Linear);

                    enActividad.IsRunning = true;

                    //genero un reloj que avanza 1 minuto por vuelta
                    await Task.Delay(6000);

                    tiempoTranscurrido++;
                }
                enActividad.IsRunning = false;

            }

        }

        private void eliminarSecuencia_Clicked(object sender, EventArgs e)
        {
            if (SecuenciaSeleccionada.SelectedItem is Predefinidos itempredefinido)
            {
                seteosPredefinidos.Remove(itempredefinido);

                DisplayAlert("Cuidado", "Usted eliminara esta secuencia", "Cerrar");

                // Actualizo el ItemsSource del ListView
                SecuenciaSeleccionada.ItemsSource = null;
                SecuenciaSeleccionada.ItemsSource = seteosPredefinidos;

                ManejodeArchivos.GuardarDatos("secuencias.json", seteosPredefinidos);
            }
            else DisplayAlert("Aviso", "No hay item seleccionado", "Cerrar");
        }

    }

}
