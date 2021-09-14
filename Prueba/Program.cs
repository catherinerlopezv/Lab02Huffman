using System;
using System.IO;

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
            Operaciones imp = new Operaciones("Prueba.txt", "Arbol");
            imp.Comprimir();
            imp.Descomporimir();
        }
    }
}
