using System;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
namespace ico
{
    public class Camino
    {
#region Constructores
        

        public Camino(ArrayList camino)
        {
            _camino= new List<Nodo>();
             foreach (Nodo i in camino) {
                _camino.Add((Nodo)i);
            }
            _ldv =_cobertura= false;
            _nldv = _movimientos = 0;
        }

        public Camino(Casilla de,Mech ich, Casilla a, Tablero tablero) {

           ArrayList camino = pathFinder(de, ich, a, tablero);
           int j = 0;

           _camino = new List<Nodo>();
          foreach (Nodo i in camino) {
              _camino.Add(i);
              j++;
          }

          //Rellenamos el resto de atributos privados
          _ldv = _cobertura = false;
          _nldv = _movimientos = 0;
        }

#endregion

#region Propiedades

        public int longitud() {
            return _camino.Count;
        }
        public List<Nodo> getCamino() {
            return _camino;
        }
        public Boolean ldv() {
            return _ldv;
        }
        public Boolean cobertura() {
            return _cobertura;
        }
#endregion

#region Funciones
     
        public  ArrayList  pathFinder(Casilla a, Mech ich, Casilla b, Tablero Tablero)
        {
            ArrayList cerradas = new ArrayList();
            ArrayList abiertas = new ArrayList();
            ArrayList camino = new ArrayList();
            Nodo elemento = new Nodo();
            int aux = 0, iaux=0, mejor=-1, gAcumulada=0;
            Boolean nueva = false;

            elemento.casilla(a);
            elemento.g(0);
            elemento.h(a.posicion().distancia(b.posicion()));
            elemento.f(elemento.h());
            elemento.direccion((Encaramiento)ich.ladoEncaramiento());
            elemento.padre(elemento);

            Nodo actual = elemento;

            cerradas.Add(elemento);
            do {
                for (int i = 1; i < 7; i++) {
                    elemento = new Nodo();
                    try
                    {
                        elemento.casilla(Tablero.colindante(actual.casilla().posicion(), (Encaramiento)i));//<-- hay que revisar en caso de que salga del tablero, aunque con el try funciona.
                    }
                    catch (Exception e) {
                        continue;
                    }
                    // Ignoro las casillas que ya estan en la lista de cerradas.
                    if (esta(cerradas,elemento.casilla()))
                        continue;

                    // Precalculo el costo de movimiento relacional, para no hacerlo varias veces
                    aux = actual.casilla().costoMovimiento(elemento.casilla()) + costoEncaramiento(actual.casilla(), (Encaramiento)i, elemento.casilla(), Tablero);
                    //aux = actual.posicion().distancia(elemento.casilla.posicion());
                    if (aux > 100)
                        continue;
                    // verifico si es intrancitable
                    if (/*elemento.casilla.costoMovimiento() >= 0 ||*/  aux >= 0){

                        // precalculo el costo de movimiento total a esa casilla desde la actual
                        elemento.g(elemento.casilla().costoMovimiento() + aux + gAcumulada);

                        // verifico si la casilla actual ya esta entre las abiertas
                        if ((iaux = estaEn(abiertas, elemento.casilla())) >= 0)
                        {

                            // compruebo si desde la aterior era mejor que esta
                            if (elemento.g() >= ((Nodo)abiertas[iaux]).g())
                            {
                                // si, si continuo sin hacer cambios
                                continue;
                            }
                            //sino modifico la casilla antigua
                            else
                            {
                                nueva = true;
                                elemento.h(((Nodo)abiertas[iaux]).h());
                                elemento.f(elemento.g() + elemento.h());
                                elemento.padre(actual);
                                elemento.direccion((Encaramiento)i);
                                abiertas[iaux] = elemento;
                            }

                        }
                        else {//si no estaba entre las abiertas inserto la nueva casilla
                            nueva = true;
                            elemento.h(heuristica(elemento.casilla(), b));
                            elemento.f(elemento.g() + elemento.h());
                            elemento.padre(actual);
                            elemento.direccion((Encaramiento)i);
                            abiertas.Add(elemento);
                        }
                        
                    }
                 
                    nueva = false;
              
                    
                }

                //buesca la mejor casilla entre las abiertas
                mejor = mejorCasillaAbierta(abiertas, actual.casilla(), b);
                //actualizo la acomulacion de la g
                gAcumulada = ((Nodo)abiertas[mejor]).g();
                //agrego la mejor casilla
                cerradas.Add(abiertas[mejor]);
                //pongo la mejor como la siguiente actual
                actual = (Nodo)abiertas[mejor];
                //borro la mejor de las abiertas
                abiertas.RemoveAt(mejor);

            } while (actual.casilla() != b);

            elemento = new Nodo();

            elemento.casilla(b);
            elemento.g(0);
            elemento.h(0);
            elemento.f(0);
            elemento.direccion((Encaramiento)ich.ladoEncaramiento());
            elemento.padre((Nodo)cerradas[cerradas.Count - 1]);
            camino.Add(elemento);

            Nodo padre = (Nodo)cerradas[cerradas.Count - 1];
           do{
               camino.Add(padre.padre());
               padre = padre.padre();
           } while (padre.casilla() != a);
           //aux = caminoReal(camino, a, ich, Tablero);
          // camino = camino.GetRange(0, aux+1);

            camino.Reverse();

            return camino;
        }

        private int caminoReal(ArrayList camino, Casilla destino, Mech ich, Tablero t) {

            int puntos = ich.puntosAndar()/2, j=0,tmpC=0,tmpJ=0;
            Boolean flag=false, flagj=false;
            List<int> l;

            for (int i = 0; i < camino.Count - (1 + puntos); i++) {
                if (puntos - i >= camino.Count)
                    puntos = camino.Count - 1;

                if (((Nodo)camino[puntos - i]).g() < puntos)
                {
                    l = posiblesEncaramientos((Nodo)camino[puntos - i], destino, t);

                    for (int c = 1; c < l.Count; c++) {
                        tmpC = costoEncaramiento(((Nodo)camino[puntos - i]).casilla(), ((Nodo)camino[puntos - i]).direccion(), (Encaramiento)l[c]);
                        tmpJ = costoEncaramiento(((Nodo)camino[puntos - i]).casilla(), ((Nodo)camino[puntos - i]).direccion(), (Encaramiento)l[j]);

                        if (((Nodo)camino[puntos - i]).g() - tmpC < ich.puntosAndar())
                            flag = true;

                        if (tmpJ>tmpC){
                            flagj = true;
                            j = c;
                        } 
                    }

                    if (!flag)
                        flagj = false;
                    else
                    {
                        ((Nodo)camino[puntos - i]).direccion((Encaramiento)l[j]);
                        return puntos - i;
                    }

                    j = 0;
                }
            }

            return -1;
        }
        private List<int> posiblesEncaramientos(Nodo o, Casilla destino, Tablero t)
        {
            int min=10000, tmp=0;
            List<int> l= new List<int>();

            for (int i = 1; i < 7; i++){
                tmp = t.colindante(o.casilla().posicion(), (Encaramiento)i).posicion().distancia(destino.posicion());
                if (tmp <= min) {
                    l.Add(i);
                }
            }

            return l;
        }
        
        private int costoEncaramiento(Casilla origenCasilla, Encaramiento direccion, Casilla destinoCasilla, Tablero t) {
            if (t.colindante(origenCasilla.posicion(), direccion) == destinoCasilla)
                return 0;
            else if (t.colindante(origenCasilla.posicion(), direccion + 1) == destinoCasilla || t.colindante(origenCasilla.posicion(), direccion + 5) == destinoCasilla)
                return 1;
            else if (t.colindante(origenCasilla.posicion(), direccion + 2) == destinoCasilla || t.colindante(origenCasilla.posicion(), direccion + 4) == destinoCasilla)
                return 2;
            else 
                return 3;
        }
        private int costoEncaramiento(Casilla origenCasilla, Encaramiento o, Encaramiento des)
        {
           switch (Math.Abs(o-des)){
               case 0:
                   return 0;
               case 1:
                   return 1;
               case 2:
                   return 2;
               case 3:
                   return 3;
               case 4:
                   return 2;
               case 5:
                   return 1;
           }
           return -1;
        }
        /*private heuristica quienEsMiPadre(heuristica hijo, ArrayList lista) {
            foreach (heuristica i in lista) {
                if (hijo.padre == i.casilla)
                    return i;
            }
            return hijo;
        }*/
        public int costoMovimiento() 
        {    
                for (int i = 0; i < _camino.Count; i++){

                    _movimientos = _camino[i].casilla().costoMovimiento();

                    if (i < _camino.Count && _camino[i].casilla().nivel() < _camino[i + 1].casilla().nivel())
                    {

                        _movimientos += _camino[i].casilla().costoMovimiento(_camino[i + 1].casilla());

                        if (_movimientos < 0)//si hay intransitables en el camino
                            return _movimientos;
                    }
                }

                return _movimientos;
        }

        public Casilla casilla(int i) {
            return _camino[i].casilla();
        }

        public Nodo nodo(int i)
        {
            return _camino[i];
        }

        public int movimentos() { return _movimientos; }

        public void print()
        { 
            string str="El camino es: ";
            foreach (Nodo i in _camino)
                str += "("+i.casilla().posicion().ToString()+", "+ i.direccion().ToString() +")"+ "->";
            str += "FIN";
            Console.WriteLine(str);
        }

#endregion
        #region Privado
        //private int _length;
        private List<Nodo> _camino;
        private Boolean _ldv;
        private Boolean _cobertura;
        private int _nldv;
        private int _movimientos;



        // funcion que calcula los valores euristicos siendo menos el mejor.
        private int heuristica(Casilla a, Casilla b)
        {
            int h = 0;

            //Distancia aproximada
            //h = Posicion.distancia(a.posicion(), b.posicion());// Calculo de la distancia aproximada al objetivo.
            //Distancia real
            h = a.posicion().distancia(b.posicion());

            return h;
        }
        
        private int mejorCasillaAbierta(ArrayList abiertas,Casilla padre, Casilla destino) {
            int max = 0;
            for (int i = 1; i < abiertas.Count; i++ )
            {
               /* if (max < 0){
                    if (((heuristica)abiertas[i]).padre == padre)
                        max = i;
                }
                else
                {*/
                if (/*((heuristica)abiertas[i]).g<20 && */((Nodo)abiertas[i]).casilla() == destino && ((Nodo)abiertas[i]).padre().casilla() == padre)
                        return i;
                if (((Nodo)abiertas[max]).f() > ((Nodo)abiertas[i]).f() /*&& ((heuristica)abiertas[i]).padre == padre*/)
                        max = i;
                //}
            }
            return max;
        }

        private int estaEn(ArrayList lista, Casilla  elem) {
            int n = 0;
            foreach (Nodo i in lista)
            {
                if (i.casilla() == elem)
                    return n;
                n++;
            }
            return -1;
        }

        private Boolean esta(ArrayList lista, Casilla elem) {
            foreach (Nodo i in lista)
            {
                if (i.casilla() == elem)
                    return true;
            }
            return false;
        }
        #endregion

    }
}
