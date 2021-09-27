using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace Lab02
{
	public class LZW
	{
		/// Comprime el texto y lo convierte en un listado de simbolos
		public static List<int> Comprimir(string texto)
		{

			// Contruye el diccionario guardando los caractes unicos al principio del archivo y el contenido de lzw despues
			Dictionary<string, int> diccionario = new Dictionary<string, int>();
			//Limite de caracteres ascii
			for (int i = 0; i < 256; i++)
			{
				diccionario.Add(((char)i).ToString(), i);
			}
			string temp = string.Empty;
			List<int> comprimir = new List<int>();
			//Cada caracter en el texto del archivo
			foreach (char letras in texto)
			{
				//Caracteres unicos
				string guardarL = temp + letras;
				if (diccionario.ContainsKey(guardarL))
				{
					temp = guardarL;
				}
				else
				{
					//Guardar comprimido
					comprimir.Add(diccionario[temp]);
					//Si es diferente, se guarda en el diccionario
					diccionario.Add(guardarL, diccionario.Count);
					temp = letras.ToString();
				}
			}

			//Archivos vacios
			if (!string.IsNullOrEmpty(temp))
			{
				comprimir.Add(diccionario[temp]);
			}
			return comprimir;

		}

		public static string Descomprimir(List<int> texto)
		{
			// Se vuelve a construir la biblioteca
			int tamañodeDiccionario = 256;
			Dictionary<int, string> diccionario = new Dictionary<int, string>();
			for (int i = 0; i < 256; i++)
			{
				diccionario[i] = "" + (char)i;
			}

			string temp = "" + (char)(int)texto.ElementAt(0);
			texto.RemoveAt(0);
			StringBuilder resultado = new StringBuilder(temp);
			foreach (int letras in texto)
			{
				string entrada;
				if (diccionario.ContainsKey(letras))
				{
					entrada = diccionario[letras];
				}
				else if (letras == tamañodeDiccionario)
				{
					entrada = temp + temp[0];
				}
				else
				{
					throw new System.ArgumentException("Caracter desconocido: " + letras);
				}

				resultado.Append(entrada);

				// Agregar al diccionario
				diccionario[tamañodeDiccionario++] = temp + entrada[0];

				temp = entrada;
			}
			return resultado.ToString();
		}


	}
}
