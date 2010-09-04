using System;
using System.Collections.Generic;
using System.Text;

namespace ico
{
    public class Nodo
    {

        public Nodo()
        {
            _g = _h = _f = 0;
            _direccion = (Encaramiento)1;
            _padre = null;
        }


        public int g() { return _g; }
        public void g(int n) { _g = n; }

        public int h() { return _h; }
        public void h(int n) { _h = n; }

        public int f() { return _f; }
        public void f(int n) { _f = n; }

        public Encaramiento direccion() { return _direccion; }
        public void direccion(Encaramiento n) { _direccion = n; }

        public Casilla casilla() { return _casilla; }
        public void casilla(Casilla n) { _casilla = n; }

        public Nodo padre() { return _padre; }
        public void padre(Nodo n) { _padre = n; }

        private int _g;
        private int _h;
        private int _f;
        private Encaramiento _direccion;
        private Casilla _casilla;
        private Nodo _padre;


        public override string ToString()
        {
            return _padre.casilla().ToString() + "->" + _casilla.ToString() + " <=> " + _g.ToString() + "g + " + _h.ToString() + "h = " + _f.ToString() + "f";
        }
    }
    
}
