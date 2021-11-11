using System;
using System.IO;
using LZW;

namespace Prueba
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Escribir texto: ");
            string texto = Console.ReadLine();
            using (StreamWriter outputFile = new StreamWriter("Prueba.txt"))
            {
                outputFile.WriteLine(texto);
            }
            OperacionesH imp = new OperacionesH("Prueba.txt", "Arbol");
            imp.Comprimir();
            imp.Descomprimir();



            Console.Write("LZW");
            OperacionesL impLZW = new OperacionesL("Prueba.txt", "Arbol");

            impLZW.Comprimir();
            impLZW.Descomprimir();

        }
    }
}
