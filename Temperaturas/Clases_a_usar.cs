using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Temperaturas
{
    // Este objeto guardara los datos de las secuencias de trabajo

    public class Predefinidos
    {
        public string? Nombre { get; set; }
        public double TempFuncionamiento { get; set; }
        public double TiempoDeCoccion { get; set; }
        public double AlarmaPrevia { get; set; }

    }

    // Clase para guardar y recuperar los datos desde un archivo persistente

    public static class ManejodeArchivos
    {
        // Obtener la ruta
        private static string TraerRuta( string NombreArchivoGuardado ) =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), NombreArchivoGuardado);

        // Guardar los datos
        public static void GuardarDatos<T> ( string NombreArchivoGuardado , T datos  )
        {
            string rutaalArchivo = TraerRuta( NombreArchivoGuardado );
            string archivoJson = JsonSerializer.Serialize ( datos );
            File.WriteAllText( rutaalArchivo , archivoJson );
        }

        // Recupero los datos
        public static T LeerDatos <T> ( string NombreArchivoGuardado )
        {
            string rutaalArchivo = TraerRuta ( NombreArchivoGuardado );
            if ( File.Exists( rutaalArchivo ) )
            {
                string archivoJson = File.ReadAllText( rutaalArchivo );
                return JsonSerializer.Deserialize<T>(archivoJson);
            }
            return default! ;
        }
    }
    
}
