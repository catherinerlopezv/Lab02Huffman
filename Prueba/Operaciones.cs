using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace Prueba
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
            string enviarU = ubicacionRaiz + @"\\Upload\\Escritoriovpn.hdef";
            PrepararArchivo hef = new PrepararArchivo(@ubicacion, @enviarU);
            byte[] b = hef.Codificar();
            enviarU = ubicacionRaiz + @"\\Upload\\Comprimido.huff";
            File.WriteAllBytes(@enviarU, b);
        }

        public void Descomporimir()
        {
            string enviarU = ubicacionRaiz + @"\\Upload\\Comprimido.huff" ;
            byte[] bytes = ManejoDeArchivo.GetArchivoBytes(enviarU);
            enviarU = ubicacionRaiz + @"\\Upload\\Escritoriovpn.hdef";
            string bb = PrepararArchivo.Decodificar(bytes, enviarU);
            enviarU = ubicacionRaiz + @"\\Upload\\descomprimidoHuff.txt";
            File.WriteAllText(enviarU, bb);
        }
    }
}
