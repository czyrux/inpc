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
        public Camino(Mech p1, Mech p2,Tablero tablero,int jugador) {
            Process proc = new Process();
            _movimientos = _nldv = 0;
            String[] nodos;
            proc.StartInfo.WorkingDirectory = @".";
            proc.StartInfo.FileName = "LDVyC.exe";
            string str= "mapaJ"+jugador.ToString()+".sbt " + p1.posicion().ToString() + " ";
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

            //rellenamos heuristica?¿
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

        public Camino(Casilla de, Casilla a, Tablero tablero) {

           ArrayList camino = pathFinder(de, a, tablero);
           int j = 0;
          _length = camino.Count;
          _camino = new Casilla[_length];
          foreach (Casilla i in camino) {
              _camino[j] = i;
              j++;
          }
          _ldv = _cobertura = false;
          _nldv = _movimientos = 0;
        }
#endregion

#region Propiedades

        public int longitud() {
            return _camino.Length;
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
        public  ArrayList  pathFinder(Casilla a, Casilla b, Tablero Tablero)
        {
            ArrayList cerradas = new ArrayList();
            ArrayList abiertas = new ArrayList();
            ArrayList camino = new ArrayList();
            heuristica elemento;
            int aux = 0, iaux=0, mejor=-1;
            Casilla actual = a;
            Boolean nueva = false;
            elemento.casilla = a;
            elemento.g = 0;
            elemento.h = a.posicion().distancia(b.posicion());
            elemento.f = elemento.h;
            elemento.padre = null;

            cerradas.Add(elemento);
            do {
                for (int i = 1; i < 7; i++) {
                    try
                    {
                        elemento.casilla = Tablero.colindante(actual, (Encaramiento)i);//<-- hay que revisar en caso de que salga del tablero, aunque con el try funciona.
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

                    // verifico si es intrancitable
                    if (/*elemento.casilla.costoMovimiento() >= 0 ||*/  aux >= 0){

                        // precalculo el costo de movimiento total a esa casilla desde la actual
                        elemento.g = elemento.casilla.costoMovimiento() + aux;

                        // verifico si la casilla actual ya esta entre las abiertas
                        if ((iaux = estaEn(abiertas, elemento.casilla)) >= 0){

                            // compruebo si desde la aterior era mejor que esta
                            if (elemento.g > ((heuristica)abiertas[iaux]).g)
                                // si, si continuo sin hacer cambios
                                continue;
                            //si no modifico la casilla antigua
                            else {
                                nueva = true;
                                elemento.h = ((heuristica)abiertas[iaux]).h;
                                elemento.f = elemento.g + elemento.h;
                                elemento.padre = actual;
                                abiertas[iaux] = (elemento);
                            }

                        }
                        else {//si no estaba entre las abiertas inserto la nueva casilla
                            nueva = true;
                            elemento.h = heuristica(elemento.casilla,b);
                            elemento.f = elemento.g + elemento.h;
                            elemento.padre = actual;
                            abiertas.Add(elemento);
                        }
                        
                    }
                 
                    nueva = false;
              
                    
                }

                //buesca la mejor casilla entre las abiertas
                mejor = mejorCasillaAbierta(abiertas, actual);
                //agrego la mejor casilla
                cerradas.Add(abiertas[mejor]);
                //pongo la mejor como la siguiente actual
                actual = ((heuristica)abiertas[mejor]).casilla;
                //borro la mejor de las abiertas
                abiertas.RemoveAt(mejor);

            } while (actual != b);

            camino.Add(b);
            for (int i = cerradas.Count - 1; i > 0; i--)
            {
                camino.Add(((heuristica)cerradas[i]).padre);
            }
            camino.Reverse();
            return camino;
        }

        
        public int costoMovimiento() {
            
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

        public void print() { 
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
        private float heuristica(Casilla a, Casilla b)
        {
            float h = 0;

           //h = DistanciaAB(a.posicion(), b.posicion());// Calculo de la distancia aproximada al objetivo.
            h = a.posicion().distancia(b.posicion());

            return h;
        }
        
        private int mejorCasillaAbierta(ArrayList abiertas,Casilla padre) {
            int max = 0;
            for (int i = 1; i < abiertas.Count; i++ )
            {
                if(((heuristica)abiertas[i-1]).f>((heuristica)abiertas[i]).f && ((heuristica)abiertas[i]).padre==padre){
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
