using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Lab3
{
    public sealed class InfoLZW
    {
        private readonly static InfoLZW _instance = new InfoLZW();
        public List<Jsonlzw> infolzw { get; set; }
        public int LastId = 5;

        private InfoLZW()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\ .txt");
            var lines = File.ReadAllLines(path);
            long ArchO = lines.Length;
            string path2 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\ .lzw");
            var lines2 = File.ReadAllLines(path);
            long ArchC = lines.Length;

            long razon = ArchC / ArchO;
            long factor = ArchO / ArchC;
            long porcentaje = factor * 100;

            /* for (var i = 0; i < lines.Length; i += 1)
             {
                 razon = Convert.ToDouble(lines[0]);
                 factor = Convert.ToDouble(lines[1]);
                 porcentaje = Convert.ToDouble(lines[2]);
                 break;
             }*/


            infolzw = new List<Jsonlzw>();
            Jsonlzw newExchangeRate = new Jsonlzw
            {
                nombreArchivolzw = "",
                ubicacionComprimidolzw = path2,
                factorCompresionlzw = factor,
                razonCompresionlzw = razon,
                porcentajeCompresionlzw = porcentaje

            };
            infolzw.Add(newExchangeRate);



        }

        public static InfoLZW Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}

