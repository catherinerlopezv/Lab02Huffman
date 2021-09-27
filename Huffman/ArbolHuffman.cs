using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;



namespace Huffman
{
    public class PrepararArchivo : ArbolHuffman
    {
        public PrepararArchivo(string ubicacion, string ubicacionArbolHuffman) : base(ubicacion, ubicacionArbolHuffman)
        {
            if (ubicacion != String.Empty)
            {
                CargarTexto(new StreamReader(new FileStream(ubicacion, FileMode.Open, FileAccess.Read)));
            }
        }
    }

    public class CodificarTexto : ArbolHuffman
    {
        public CodificarTexto(string texto)
        {
            byte[] textoArr = Encoding.ASCII.GetBytes(texto);
            var temp = new MemoryStream(textoArr);

            CargarTexto(new StreamReader(temp));
        }
    }

    //Arbol
    public abstract class ArbolHuffman
    {
        private List<ManejarArbol> Arbol = new List<ManejarArbol>();
        private LeafList hojas = new LeafList();
        private List<bool> textoL = new List<bool>();
        private string ubicacionHuffman = String.Empty;
        private string ubicacionArchivo = String.Empty;

        private Dictionary<int?, List<bool>> tablas = new Dictionary<int?, List<bool>>();

        public ManejarArbol ArbolHuffmanI { get; set; }

        public string UbicacionDeArchivo
        {
            get { return ubicacionArchivo; }
        }

        public string UbicacionDeHuffman
        {
            get { return ubicacionHuffman; }
        }

        internal ArbolHuffman() { }

        internal ArbolHuffman(string ubicacionA, string ubicacionH)
        {
            ubicacionArchivo = ubicacionA;
            ubicacionHuffman = ubicacionH;
        }
        private ManejarArbol Iniciar(List<ManejarArbol> colaPrioridad)
        {
            if (colaPrioridad.Count == 0)
            {
                throw new ArgumentException("La cola esta vacia", "Cola de prioridad!");
            }
            //Se tiene un arbol Huffman
            while (colaPrioridad.Count > 1)
            {
                //Ordena de forma ascendente en orden de los valores, los valores pequeños son los primeros uwu e idfentica de forma descendente los datos
                //para que los valores nuevos seran puesto atras de los valores
                colaPrioridad = colaPrioridad.OrderBy(x => x.Valor).ThenByDescending(x => x.Dato).ToList();
                var fondoIz = colaPrioridad[0];
                fondoIz.BitVal = false;
                var fondoDer = colaPrioridad[1];
                fondoDer.BitVal = true;
                //Nuevo padre nodo tiene los valores de los 2 nodos combinados
                var padre = new ManejarArbol() { Dato = null, Valor = fondoIz.Valor + fondoDer.Valor };
                padre.AgregandoHijos(fondoIz, fondoDer);
                colaPrioridad.RemoveRange(0, 2);
                colaPrioridad.Add(padre);
                //Se pone las hojas en una tabla para no recorrer el texto binario idk
                hojas.Rango(fondoIz, fondoDer);
            }

            return colaPrioridad[0];
        }
        private byte ConvertirAByte(BitArray bits)
        {
            if (bits.Count > 8)
            {
                throw new ArgumentException("ERROR; Solo se puede convertir si tiene mas de 8 bits");
            }
            byte resultado = 0;
            for (byte i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                {
                    resultado |= (byte)(1 << i);
                }
            }
            return resultado;
        }
        public byte[] Codificar()
        {
            ArbolHuffmanI = Iniciar(Arbol);

            ConstruirTablas();

            //Recorre el texto leyendo un caracter a la vez reemplazando el token de los caracters con su equivalente a bit

            var bits = new List<bool>();
            var salidasB = new List<byte>();

            Action procesarBytes = () =>
            {
                salidasB.Add(ConvertirAByte(new BitArray(bits.GetRange(0, 8).ToArray())));
                bits.RemoveRange(0, 8);
            };

            foreach (var b in ManejoDeArchivo.GetArchivoBytes(ubicacionArchivo))
            {
                bits.AddRange(tablas[b]);

                if (bits.Count > 8)
                    procesarBytes();
            }

            //Agregar los bits hasta el final del archivo
            bits.AddRange(tablas[-1]);
            while (bits.Count > 8)
            {
                procesarBytes();
            }

            bool[] byteF = new bool[8];

            if (bits.Count > 0)
            {
                Array.Copy(bits.ToArray(), byteF, bits.Count);
            }
            salidasB.Add(ConvertirAByte(new BitArray(byteF)));

            if (ubicacionHuffman != String.Empty)
            {
                ArbolHuffmanI.Guardar(ubicacionHuffman);
            }
            return salidasB.ToArray();
        }

        public static string Decodificar(byte[] bytes, string direccion)
        {
            return Decodificar(bytes, Serializar.DeserializarArchivoBinario<ManejarArbol>(direccion));
        }

        public static string Decodificar(byte[] bytes, ManejarArbol huffmanTree)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("El arreglo no puede ser null");
            }
            if (huffmanTree == null)
            {
                throw new ArgumentNullException("El arbol no puede ser null");
            }
            var texto = new StringBuilder();
            var nodoT = huffmanTree;
            var bits = new BitArray(bytes);

            foreach (bool bit in bits)
            {
                if (!bit)
                {
                    nodoT = nodoT.HijoIzqu;
                }
                else
                {
                    nodoT = nodoT.HijoDer;
                }
                if (nodoT.Dato == -1)
                {
                    break;
                }
                if (nodoT.Dato == null)
                {
                    continue;
                }
                texto.Append(nodoT.MostrarLlave);

                nodoT = huffmanTree;
            }

            return texto.ToString();
        }

        private void ConstruirTablas()
        {
            foreach (var btn in hojas)
            {
                RecorrerArbol(btn);
                tablas.Add(btn.Dato, new List<bool>(textoL));
                textoL.Clear();
            }
        }


        //Recorre las hojas hacia el padre y cuenta el numero de bits hasta llegar a la raiz
        private void RecorrerArbol(ManejarArbol fondo)
        {
            if (fondo.Padre == null)
            {
                return;
            }
            RecorrerArbol(fondo.Padre);
            //Agrega los datos recursivamente para leer correctamente desde el fondo hasta la raiz
            textoL.Add(fondo.BitVal);
        }



        internal void CargarTexto(StreamReader leer)
        {
            int codigo = 0;
            var revisar = new Dictionary<int, ulong>();

            for (int i = 0; i < 128; i++)
            {
                revisar.Add(i, 0);
            }
            using (leer)
            {
                while (leer.Peek() != -1)
                {
                    codigo = leer.Read();
                    //Revisar si el caracter es ASCII
                    if (codigo > 127)
                    {
                        throw new Exception("Solo debe ser caracter ascii: " + codigo + "en  Char: " + Convert.ToChar(codigo));
                    }
                    revisar[codigo]++;
                }

                leer.Close();
            }
            //Si son 0 caracteres se ignoran
            foreach (var llave in revisar)
            {
                if (llave.Value > 0)
                {
                    Arbol.Add(new ManejarArbol() { Dato = llave.Key, Valor = llave.Value });
                }
            }
            //Final de archivo
            Arbol.Add(new ManejarArbol() { Dato = -1, Valor = 1 });
        }
    }

    internal class LeafList : List<ManejarArbol>
    {
        private void Agregar(ManejarArbol hoja)
        {
            if (hoja.Dato != null && !Exists(x => x.Dato == hoja.Dato))
            {
                base.Add(hoja);
            }
        }

        public void Rango(params ManejarArbol[] coleccion)
        {
            foreach (var btn in coleccion)
            {
                Agregar(btn);
            }
        }
    }
}