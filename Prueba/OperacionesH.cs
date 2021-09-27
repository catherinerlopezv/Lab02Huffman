using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba
{
    //Implementacion Huffman
    public class OperacionesH
    {

        public string ubicacion = "";
        public string ubicacionRaiz = "";

        public OperacionesH(string ubicacion, string ubicacionRaiz)
        {
            this.ubicacion = ubicacion;
            this.ubicacionRaiz = ubicacionRaiz;
        }

        public void Comprimir()
        {
            string enviarU = ubicacionRaiz + @"\\Upload\\Escritoriovpn.hdef";
            PrepararArchivo hef = new PrepararArchivo(@ubicacion, @enviarU);
            byte[] b = hef.Codificar();
            enviarU = ubicacionRaiz + @"\\Upload\\Comprimido.huff";
            File.WriteAllBytes(@enviarU, b);
        }

        public void Descomprimir()
        {
            string enviarU = ubicacionRaiz + @"\\Upload\\Comprimido.huff";
            byte[] bytes = ManejoDeArchivo.GetArchivoBytes(enviarU);
            enviarU = ubicacionRaiz + @"\\Upload\\Escritoriovpn.hdef";
            string bb = PrepararArchivo.Decodificar(bytes, enviarU);
            enviarU = ubicacionRaiz + @"\\Upload\\descomprimidoHuff.txt";
            File.WriteAllText(enviarU, bb);
        }
    }
}
