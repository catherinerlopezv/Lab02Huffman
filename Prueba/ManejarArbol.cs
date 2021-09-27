using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba
{
    [Serializable]
    public class ManejarArbol
    {
        //nodo 
        public enum NodeDirection
        {
            Izquierdo,
            Derecho
        }
        public ManejarArbol() { }


        public ManejarArbol Padre = null;
        public ManejarArbol HijoIzqu = null;
        public ManejarArbol HijoDer = null;

        public bool BitVal;
        public int? Dato { get; set; }
        public ulong Valor { get; set; }

        public void AgregandoHijos(ManejarArbol hijoIzqNodo, ManejarArbol hijoDerNodo)
        {
            AgregandoHijo(hijoIzqNodo, NodeDirection.Izquierdo);
            AgregandoHijo(hijoDerNodo, NodeDirection.Derecho);
        }

        public void AgregandoHijo(ManejarArbol btn, NodeDirection nd)
        {
            btn.Padre = this;

            if (nd == NodeDirection.Izquierdo)
                this.HijoIzqu = btn;
            else
                this.HijoDer = btn;
        }

        public char MostrarLlave
        {
            get
            {
                return Convert.ToChar(this.Dato);
            }
        }

        public void Guardar(string path)
        {
            var fit = new FileInfo(path);

            path = path.Replace(fit.Extension, ".hdef");

            Serializar.GuardarArchivoBinario<ManejarArbol>(path, this);
        }
    }
}


