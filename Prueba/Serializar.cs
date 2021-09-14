using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Prueba
{
    internal class Serializar
    {
        internal static void GuardarArchivoBinario<T>(string ubicacionArchivo, T Contenido)
        {
            try
            {
                using (var cadenaM = new MemoryStream())
                {
                    var formatBin = new BinaryFormatter();

                    formatBin.Serialize(cadenaM, Contenido);

                    string ubicacion = Path.GetDirectoryName(ubicacionArchivo);

                    if (!Directory.Exists(ubicacion))
                    {
                        Directory.CreateDirectory(ubicacion);
                    }
                    File.WriteAllBytes(ubicacionArchivo, cadenaM.ToArray());

                    cadenaM.Close();
                }
            }
            catch
            {
                throw;
            }
        }
        internal static T DeserializarArchivoBinario<T>(string ubicacionArchivo)
        {
            BinaryFormatter formato = new BinaryFormatter();
            using (var cadenaM = new MemoryStream(File.ReadAllBytes(ubicacionArchivo)))
            {
                cadenaM.Position = 0;
                return (T)formato.Deserialize(cadenaM);
            }
        }
    }
}
