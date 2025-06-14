using Temperaturas;
namespace Temperaturas;

public partial class ingresoPage : ContentPage
{
    //private MainPage copiaMainPage;
    private readonly Action<Predefinidos> nuevoElementoAgregado;

    //paso una referencia de la pagina principal
    public ingresoPage( Action<Predefinidos> llamada )
	{
		InitializeComponent();

        nuevoElementoAgregado = llamada;
        //guardo esta referencia
        //copiaMainPage = mainPage;
	}

    private async void nuevoIngreso_Clicked(object sender, EventArgs e)
    {
        //Valido las entradas
        if(String.IsNullOrWhiteSpace(nombreIngresado.Text) ||
           String.IsNullOrWhiteSpace(temperaturaIngresado.Text) ||
           String.IsNullOrWhiteSpace(tiempoIngresado.Text) ||
           String.IsNullOrWhiteSpace(preavisoIngresado.Text) )
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
            return;
        }

        // Limito a 18 caracteres el nombre
        if( nombreIngresado.Text.Length > 18 )
        {
            await DisplayAlert("Aviso", "El nombre excede los 18 caracteres", "Cerrar");
            return;
        }

        // Limito el rango de coccion
        if (!int.TryParse(temperaturaIngresado.Text, out int temperatura) || temperatura < 50 || temperatura > 350)
        {
            await DisplayAlert("Error", "Ingrese una temperatura válida entre 50°C y 350°C.", "OK");
            return;
        }

        // Limito el tiempo de coccion
        if ( !int.TryParse(tiempoIngresado.Text, out int tiempo ) || tiempo < 1 || tiempo > 240 )
        {
            await DisplayAlert("Error", "Ingrese un tiempo válido entre 1 y 240 minutos", "OK");
            return;
        }

        // Limito el preaviso
        if ( !int.TryParse(preavisoIngresado.Text, out int preaviso) || preaviso < 0 || preaviso >= tiempo )
        {
            await DisplayAlert("Error", "El preaviso debe ser menor al tiempo total de cocción y mayor o igual a 0.", "OK");
            return;
        }

        try
        {
            //creo un elemento con los datos ingresados por el usuario
            Predefinidos nuevoElemento = new Predefinidos
            {
                Nombre = nombreIngresado.Text.Trim(),
                TempFuncionamiento = Convert.ToInt16(temperaturaIngresado.Text),
                TiempoDeCoccion = Convert.ToInt16(tiempoIngresado.Text),
                AlarmaPrevia = Convert.ToInt16(preavisoIngresado.Text),
            };

            //agrego el nuevo elemento a la lista referenciada en la copia
            //copiaMainPage.seteosPredefinidos.Add(nuevoElemento);

            //copiaMainPage.ActualizarListaPredefinidos();

            //Invoco a la funcion y le paso el nuevo elemento
            nuevoElementoAgregado?.Invoke( nuevoElemento );

            //Permanencia de datos
            //ManejodeArchivos.GuardarDatos( "secuencias.json" , copiaMainPage.seteosPredefinidos );

            var navigation = this.Navigation;

            await DisplayAlert("Nueva secuencia", "Se ha ingresado la nueva secuencia correctamente", "Cerrar");

            await Navigation.PopAsync();
        }
        //En caso de falla capturo el error
        catch (Exception ex)
        {
            await DisplayAlert( "Error",$"Ocurrio un problema al ingresar la secuencia: {ex.Message}", "Ok");
        }
    }

    private void ayuda_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Ayuda());
    }
}