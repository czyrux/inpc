using System;
using System.Collections.Generic;
using System.Text;

namespace ico
{
    public class Nodo
    {
        /// <summary>
        /// Costructor por defecto
        /// </summary>
        public Nodo()
        {
            _g = _h = _f = 0;
            _direccion = (Encaramiento)1;
            _padre = null;
        }

        /// <summary>
        /// devuelve el costo, en puntos de movimiento, acumulado para llegar a este nodo
        /// </summary>
        /// <returns>costo acumulado; tipo Int</returns>
        public int g() { return _g; }
        /// <summary>
        /// establece el csto acumulado del nodo
        /// </summary>
        /// <param name="n">costo acumulado; tipo Int</param>
        public void g(int n) { _g = n; }

        /// <summary>
        /// devuelve la distancia (en numero de casillas) desde la casilla hasta el destino. 
        /// Dicha distancia es la minima a traves del camino mas rapido en numero de casillas, es decir "en linea recta".
        /// Dicha medicion es heuristica, pero exacta aunque el camino con el que cuenta las casillas no tine por que ser posible.
        /// </summary>
        /// <returns>distancia heuristica entre el destion y la casilla; tipo Int</returns>
        public int h() { return _h; }
        /// <summary>
        /// establece la distancia (en numero de casillas) desde la casilla hasta el destino. 
        /// Dicha distancia es la minima a traves del camino mas rapido en numero de casillas, es decir "en linea recta".
        /// Dicha medicion es heuristica, pero exacta aunque el camino con el que cuenta las casillas no tine por que ser posible.
        /// </summary>
        /// <param name="n">distancia heuristica entre el destion y la casilla; tipo Int</param>
        public void h(int n) { _h = n; }

        /// <summary>
        /// devuelve la suma de g y h
        /// </summary>
        /// <returns>g+h; tipo Int</returns>
        public int f() { return _f; }
        /// <summary>
        /// establece la suma de h+g
        /// </summary>
        /// <param name="n">h+g; tipo Int</param>
        public void f(int n) { _f = n; }

        /// <summary>
        /// deveuve la direccion el cual el mech esta mirandoantes de moverce a la siguiente casilla
        /// </summary>
        /// <returns>dirrecion; tipo Encaramiento</returns>
        public Encaramiento direccion() { return _direccion; }
        /// <summary>
        /// establece la direccion el cual el mech esta mirandoantes de moverce a la siguiente casilla
        /// </summary>
        /// <param name="n">direccion; tipo Encaramiento</param>
        public void direccion(Encaramiento n) { _direccion = n; }

        /// <summary>
        /// devuelve la casilla que contine el nodo
        /// </summary>
        /// <returns>casilla; tipo casilla</returns>
        public Casilla casilla() { return _casilla; }
        /// <summary>
        /// establece la casilla que contine el nodo
        /// </summary>
        /// <param name="n">casilla; tipo Casilla</param>
        public void casilla(Casilla n) { _casilla = n; }

        /// <summary>
        /// devuelve el nodo por donde se llego a este nodo
        /// </summary>
        /// <returns>nodo de procedencia de este nodo(this); tipo Nodo</returns>
        public Nodo padre() { return _padre; }
        /// <summary>
        /// establece el nodo por donde se llego a esta casilla
        /// </summary>
        /// <param name="n">nodo de procedencia de este nodo(this); tipo Nodo</param>
        public void padre(Nodo n) { _padre = n; }


        /// <summary>
        /// el costo, en puntos de movimiento, acumulado para llegar a este nodo
        /// </summary>
        private int _g;
        /// <summary>
        /// la distancia (en numero de casillas) desde la casilla hasta el destino. 
        /// Dicha distancia es la minima a traves del camino mas rapido en numero de casillas, es decir "en linea recta".
        /// Dicha medicion es heuristica, pero exacta aunque el camino con el que cuenta las casillas no tine por que ser posible.
        /// </summary>
        private int _h;
        /// <summary>
        /// la suma de g y h
        /// </summary>
        private int _f;
        /// <summary>
        /// la direccion el cual el mech esta mirandoantes de moverce a la siguiente casilla
        /// </summary>
        private Encaramiento _direccion;
        /// <summary>
        /// la casilla que contine el nodo
        /// </summary>
        private Casilla _casilla;
        /// <summary>
        /// el nodo por donde se llego a esta casilla
        /// </summary>
        private Nodo _padre;

        /// <summary>
        /// transforma el nodo en texto.
        /// </summary>
        /// <returns>el nodo en forma de cadena; tipo String</returns>
        public override string ToString()
        {
            return _padre.casilla().ToString() + "->" + _casilla.ToString() + " <=> " + _g.ToString() + "g + " + _h.ToString() + "h = " + _f.ToString() + "f";
        }
    }
    
}
