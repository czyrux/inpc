using System;
using System.Collections.Generic;
using System.Text;

namespace ico
{
    public class Heuristica
    {

        public Heuristica()
        {
            g = h = f = 0;
            direccion = (Encaramiento)1;
            padre = null;
        }

        public int g;
        public int h;
        public int f;
        public Encaramiento direccion;
        public Casilla casilla;
        public Heuristica padre;


        public override string ToString()
        {
            return padre.ToString() + "->" + casilla.ToString() + " <=> " + g.ToString() + "g + " + h.ToString() + "h = " + f.ToString() + "f";
        }
    }
    
}
