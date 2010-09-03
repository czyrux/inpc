using System;
using System.Collections.Generic;
using System.Text;

namespace ico
{
    public class Heuristica
    {

        public Heuristica()
        {
            _g = _h = _f = 0;
            _direccion = (Encaramiento)1;
            _padre = null;
        }

        /*public Heuristica nuevo(Heuristica cpy) {
            _f = cpy.f();
            _g = cpy.g();
            _h = cpy.h();
            _direccion = cpy.direccion();

        }*/

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

        public Heuristica padre() { return _padre; }
        public void padre(Heuristica n) { _padre = n; }

        private int _g;
        private int _h;
        private int _f;
        private Encaramiento _direccion;
        private Casilla _casilla;
        private Heuristica _padre;


        public override string ToString()
        {
            return _padre.casilla().ToString() + "->" + _casilla.ToString() + " <=> " + _g.ToString() + "g + " + _h.ToString() + "h = " + _f.ToString() + "f";
        }
    }
    
}
