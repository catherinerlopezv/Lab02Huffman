using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace Huffman
{
    public class Operaciones
    {

        public string ubicacion = "";
        public string ubicacionRaiz = "";

        public Operaciones(string ubicacion, string ubicacionRaiz)
        {
            this.ubicacion = ubicacion;
            this.ubicacionRaiz = ubicacionRaiz;
        }

        public void Comprimir()
        {
            string enviarU = ubicacionRaiz + @"\\Upload\\cifrado.hdef";
            PrepararArchivo hef = new PrepararArchivo(@ubicacion, @enviarU);
            byte[] b = hef.Codificar();
            enviarU = ubicacionRaiz + @"\\Upload\\" + Path.GetFileNameWithoutExtension(ubicacion) + ".huff";
            File.WriteAllBytes(@enviarU, b);
        }

        public void Descomprimir()
        {
            string enviarU = ubicacionRaiz + @"\\Upload\\" + Path.GetFileNameWithoutExtension(ubicacion) + ".huff";
            byte[] bytes = ManejoDeArchivo.GetArchivoBytes(enviarU);
            enviarU = ubicacionRaiz + @"\\Upload\\cifrado.hdef";
            string bb = PrepararArchivo.Decodificar(bytes, enviarU);
            enviarU = ubicacionRaiz + @"\\Upload\\" + Path.GetFileNameWithoutExtension(ubicacion) + "descomprimido" + ".txt";
            File.WriteAllText(enviarU, bb);
        }
    }
}
