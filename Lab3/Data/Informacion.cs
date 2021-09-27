using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Lab3
{
   public sealed class Informacion
    {
        private readonly static Informacion _instance = new Informacion();
        public List<Json> informacion { get; set; }
        public int LastId = 5;

        private Informacion()
        {
           
            double factor=0,razon=0,porcentaje=0;         
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\wwwroot\Upload\FactorRazon.txt");
            var lines = File.ReadAllLines(path);
   
            for (var i = 0; i < lines.Length; i += 1)
            {
                razon = Convert.ToDouble(lines[0]);
                factor = Convert.ToDouble(lines[1]);
                porcentaje = Convert.ToDouble(lines[2]);
                break;
            }


            informacion = new List<Json>();
                Json newExchangeRate = new Json
                {
                    nombreArchivo = "",
                    ubicacionComprimido= "",
                    factorCompresion = factor,
                    razonCompresion = razon,
                    porcentajeCompresion=porcentaje

                };
                informacion.Add(newExchangeRate);

               
        }

        public static Informacion Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
