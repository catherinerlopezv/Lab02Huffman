using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba
{
    public class OperacionesL
    {

        public string ubicacion = "";
        public string ubicacionRaiz = "";

        public OperacionesL(string ubicacion, string ubicacionRaiz)
        {
            this.ubicacion = ubicacion;
            this.ubicacionRaiz = ubicacionRaiz;
        }
        public void Comprimir()
        {
            string texto = File.ReadAllText(ubicacion);
            List<int> textoComprimido = LZW.Comprimir(texto);
            string enviarU = ubicacionRaiz + @"\\Upload\\Comprimido.lzw";
            using StreamWriter ArchivoComprimido = new StreamWriter(enviarU);
            foreach (char caracter in textoComprimido)
            {
                ArchivoComprimido.Write(caracter.ToString());
            }
        }

        public void Descomprimir()
        {

            const int bufferLength = 100;
            List<int> bytedecompress = new List<int>();

            var buffer = new char[bufferLength];
            using (var file = new FileStream(ubicacion, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadChars(bufferLength);
                        foreach (var item in buffer)
                        {

                            bytedecompress.Add((int)Convert.ToChar(item));
                        }


                    }

                }

            }

            string decompressed = LZW.Descomprimir(bytedecompress);
          
            string enviarU = ubicacionRaiz + @"\\Upload\\DesComprimido.lzw";
            File.WriteAllText(enviarU, decompressed);
        }


    }
}
