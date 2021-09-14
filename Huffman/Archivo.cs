using System;
using System.IO;
using System.Text;


namespace Huffman
{
    public class ManejoDeArchivo
    {
        static Encoding cod = Encoding.GetEncoding("us-ascii", new EncoderExceptionFallback(), new DecoderExceptionFallback());
        public static void EscribirArchivoBinario(byte[] bytes, string nombreArchivo)
        {
            using (var crearArch = File.Create(nombreArchivo))
            {
                using (var escribirbin = new BinaryWriter(crearArch, cod))
                {
                    escribirbin.Write(bytes);
                    escribirbin.Close();
                }
                crearArch.Close();
            }
        }
        public static byte[] GetArchivoBytes(string nombreArchivo)
        {
            byte[] bytes;
            using (var crearArch = File.Open(nombreArchivo, FileMode.Open, FileAccess.Read))
            {
                using (var leerbin = new BinaryReader(crearArch, cod))
                {
                    bytes = leerbin.ReadBytes(Convert.ToInt32(crearArch.Length));

                    leerbin.Close();
                }
                crearArch.Close();
            }
            return bytes;
        }

    }
}
