using System;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.IO;
namespace ico
{
    public class Camino
    {
#region Constructores
        public Camino(Mech p1, Mech p2,Tablero tablero) {
            Process proc = new Process();
            _movimientos = _nldv = 0;
            String[] nodos;
            proc.StartInfo.WorkingDirectory = @".";
            proc.StartInfo.FileName = "LDVyC.exe";
            string str= "mapaJ"+p1.numeroJ().ToString()+".sbt " + p1.posicion().ToString() + " ";
            if (p1.enSuelo()) {
                str += "0 ";
            } else
                str += "1 ";
            str += p2.posicion().ToString() + " ";
            if (p2.enSuelo()) {
                str += "0";
            } else
                str += "1";

            proc.StartInfo.Arguments = str ;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();
            proc.WaitForExit();
            proc.Close();
            StreamReader fich = new StreamReader("LDV.sbt");
            nodos = fich.ReadLine().Split(' '); 
            _ldv = Convert.ToBoolean(fich.ReadLine());
            _cobertura = Convert.ToBoolean(fich.ReadLine());
            fich.Close();

            _camino = new Casilla[nodos.Length + 2];


            _camino[0] = tablero.Casilla(p1.posicion());
            for (int i = 1; i <= nodos.Length; i++) {
                _camino[i] = tablero.Casilla(new Posicion(nodos[i - 1]));
            }
            _camino[nodos.Length+1] = tablero.Casilla(p2.posicion());
            _length = _camino.Length;
            //rellenamos heuristica?�
            /*for (int i = 0; i < _length; i++) {
                if (_camino[i].tipoTerreno() == 2) {

                }
            }*/
        }

        public Camino(ArrayList camino)
        {
            _length = camino.Count;
            _camino = new Casilla[_length];
            for (int i = 0; i < _length; i++)
            {
                _camino[i] = (Casilla)camino[i];
            }
            _ldv =_cobertura= false;
            _nldv = _movimientos = 0;
        }

        public Camino(Casilla de,Mech ich, Casilla a, Tablero tablero) {

           ArrayList camino = pathFinder(de, ich, a, tablero);
           int j = 0;
          _length = camino.Count;
          _camino = new Casilla[_length];
          foreach (Casilla i in camino) {
              _camino[j] = i;
              j++;
          }

          //Rellenamos el resto de atributos privados
          _ldv = _cobertura = false;
          _nldv = _movimientos = 0;
        }

#endregion

#region Propiedades

        public int longitud() {
            return _length;
        }
        public Casilla[] getCamino() {
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
        public  ArrayList  pathFinder(Casilla a, Mech yo, Casilla b, Tablero Tablero)
        {
            ArrayList cerradas = new ArrayList();
            ArrayList abiertas = new ArrayList();
            ArrayList camino = new ArrayList();
            heuristica elemento;
            int aux = 0, iaux=0, mejor=-1, gAcumulada=0;
            Casilla actual = a;
            Boolean nueva = false;
            elemento.casilla = a;
            elemento.g = 0;
            elemento.h = a.posicion().distancia(b.posicion());
            elemento.f = elemento.h;
            elemento.direccion = (Encaramiento)yo.ladoEncaramiento();
            elemento.padre = null;

            cerradas.Add(elemento);
            do {
                for (int i = 1; i < 7; i++) {
                    try
                    {
                        elemento.casilla = Tablero.colindante(actual.posicion(), (Encaramiento)i);//<-- hay que revisar en caso de que salga del tablero, aunque con el try funciona.
                    }
                    catch (Exception e) {
                        continue;
                    }
                    // Ignoro las casillas que ya estan en la lista de cerradas.
                    if (esta(cerradas,elemento.casilla))
                        continue;

                    // Precalculo el costo de movimiento relacional, para no hacerlo varias veces
                    aux=actual.costoMovimiento(elemento.casilla);
                    //aux = actual.posicion().distancia(elemento.casilla.posicion());
                    if (aux > 100)
                        continue;
                    // verifico si es intrancitable
                    if (/*elemento.casilla.costoMovimiento() >= 0 ||*/  aux >= 0){

                        // precalculo el costo de movimiento total a esa casilla desde la actual
                        elemento.g = elemento.casilla.costoMovimiento() + aux + gAcumulada;

                        // verifico si la casilla actual ya esta entre las abiertas
                        if ((iaux = estaEn(abiertas, elemento.casilla)) >= 0){

                            // compruebo si desde la aterior era mejor que esta
                            if (elemento.g > ((heuristica)abiertas[iaux]).g){
                                // si, si continuo sin hacer cambios
                                continue;
                            }
                            //sino modifico la casilla antigua
                            else
                            {
                                nueva = true;
                                elemento.h = ((heuristica)abiertas[iaux]).h;
                                elemento.f = elemento.g + elemento.h;
                                elemento.padre = actual;
                                elemento.direccion = (Encaramiento)i;
                                abiertas[iaux] = elemento;
                            }

                        }
                        else {//si no estaba entre las abiertas inserto la nueva casilla
                            nueva = true;
                            elemento.h = heuristica(elemento.casilla,b);
                            elemento.f = elemento.g + elemento.h;
                            elemento.padre = actual;
                            elemento.direccion = (Encaramiento)i;
                            abiertas.Add(elemento);
                        }
                        
                    }
                 
                    nueva = false;
              
                    
                }

                //buesca la mejor casilla entre las abiertas
                mejor = mejorCasillaAbierta(abiertas, actual,b);
                //actualizo la acomulacion de la g
                gAcumulada = ((heuristica)abiertas[mejor]).g;
                //agrego la mejor casilla
                cerradas.Add(abiertas[mejor]);
                //pongo la mejor como la siguiente actual
                actual = ((heuristica)abiertas[mejor]).casilla;
                //borro la mejor de las abiertas
                abiertas.RemoveAt(mejor);
                //if (actual.posicion().ToString() == "0606")
                  //  b = b;
            } while (actual != b);

            camino.Add(b);
            heuristica padre=(heuristica)cerradas[cerradas.Count-1];
           do{
                camino.Add(padre.padre);
                padre = quienEsMiPadre(padre,cerradas);
           }while(padre.casilla!=a);
            camino.Reverse();
            return camino;
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
        private heuristica quienEsMiPadre(heuristica hijo, ArrayList lista) {
            foreach (heuristica i in lista) {
                if (hijo.padre == i.casilla)
                    return i;
            }
            return hijo;
        }
        public int costoMovimiento() 
        {    
                for (int i = 0; i < _length; i++){

                    _movimientos = _camino[i].costoMovimiento();

                    if (i < _length && _camino[i].nivel() < _camino[i + 1].nivel())
                    {

                        _movimientos += _camino[i].costoMovimiento(_camino[i + 1]);

                        if (_movimientos < 0)//si hay intransitables en el camino
                            return _movimientos;
                    }
                }

                return _movimientos;
        }

        public Casilla casilla(int i) {
            return _camino[i];
        }

        public int movimentos() { return _movimientos; }

        public void print()
        { 
            string str="El camino es: ";
            foreach (Casilla i in _camino)
                str += i.posicion().ToString() + "->";
            str += "FIN";
            Console.WriteLine(str);
        }

#endregion
        #region Privado
        private int _length;
        private Casilla[] _camino;
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
                if (max < 0){
                    if (((heuristica)abiertas[i]).padre == padre)
                        max = i;
                }
                else
                {
                    if (/*((heuristica)abiertas[i]).g<20 && */((heuristica)abiertas[i]).casilla == destino && ((heuristica)abiertas[i]).padre == padre)
                        return i;
                    if (((heuristica)abiertas[max]).f > ((heuristica)abiertas[i]).f /*&& ((heuristica)abiertas[i]).padre == padre*/)
                        max = i;
                }
            }
            return max;
        }

        private int estaEn(ArrayList lista, Casilla  elem) {
            int n = 0;
            foreach (heuristica i in lista) {
                if (i.casilla == elem)
                    return n;
                n++;
            }
            return -1;
        }

        private Boolean esta(ArrayList lista, Casilla elem) {
            foreach(heuristica i in lista){
                if (i.casilla == elem)
                    return true;
            }
            return false;
        }
        #endregion

    }
}
