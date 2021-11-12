using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Lab3
{
    public class Jsonlzw:Json
    {

        public static List<Jsonlzw> CargarHistorial()
        {
            var Linea = string.Empty;
            var listaArchivo = new List<Jsonlzw>();
            using (var Reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "compressions.json")))
            {
                while (!Reader.EndOfStream)
                {
                    var historialtemp = new Jsonlzw();
                    Linea = Reader.ReadLine();

                    historialtemp.NombreOriginal = Linea;
                    Linea = Reader.ReadLine();
                    historialtemp.Nombre = Linea;
                    Linea = Reader.ReadLine();
                    historialtemp.RutaArchivo = Linea;
                    Linea = Reader.ReadLine();
                    historialtemp.RazonCompresion = Convert.ToDouble(Linea);
                    Linea = Reader.ReadLine();
                    historialtemp.FactorCompresion = Convert.ToDouble(Linea);
                    Linea = Reader.ReadLine();
                    historialtemp.Porcentaje = Convert.ToDouble(Linea);
                    listaArchivo.Add(historialtemp);
                }
            }
            return listaArchivo;
        }

        //Carga el historial que se muestra en pantalla por medio de una lista
        public static void ManejarCompressions(Jsonlzw Actual)
        {
            var Linea = string.Empty;
            var listaArchivo = CargarHistorial();
            using (var Writer = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "compressions.json")))
            {
                foreach (var item in listaArchivo)
                {
                   
                    Writer.WriteLine(item.NombreOriginal);
                    Writer.WriteLine(item.Nombre);
                    Writer.WriteLine(item.RutaArchivo);
                    Writer.WriteLine(item.RazonCompresion);
                    Writer.WriteLine(item.FactorCompresion);
                    Writer.WriteLine(item.Porcentaje);
                }
               
                Writer.WriteLine(Actual.NombreOriginal);
                Writer.WriteLine(Actual.Nombre);
                Writer.WriteLine(Actual.RutaArchivo);
                Writer.WriteLine(Actual.RazonCompresion);
                Writer.WriteLine(Actual.FactorCompresion);
                Writer.WriteLine(Actual.Porcentaje);

            }
        }
    }
}
