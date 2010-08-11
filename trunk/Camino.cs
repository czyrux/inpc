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
        public Camino(Posicion p1, Posicion p2,Tablero tablero) {
            Process proc = new Process();
            _movimientos = _nldv = 0;
            String[] nodos;
            proc.StartInfo.WorkingDirectory = @".";
            proc.StartInfo.FileName = "LDVyC.exe";
            proc.StartInfo.Arguments = "mapaJ1.sbt " + p1.ToString() + " " + tablero.Casilla(p1).nivel().ToString() + " " + p2.ToString() + " " + tablero.Casilla(p2).nivel().ToString() + "";
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


            _camino[0] = tablero.Casilla(p1);
            for (int i = 1; i <= nodos.Length; i++) {
                _camino[i] = tablero.Casilla(new Posicion(nodos[i - 1]));
            }
            _camino[nodos.Length+1] = tablero.Casilla(p2);

            //rellenamos heuristica?�
            for (int i = 0; i < _length; i++) {
                if (_camino[i].tipoTerreno() == 2) {

                }
            }
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
            pathFinder(de, a, tablero);
        }
#endregion

#region Propiedades

        int longitud() {
            return _length;
        }
        Casilla[] getCamino() {
            return _camino;
        }
        Boolean ldv() {
            return _ldv;
        }
        Boolean cobertura() {
            return _cobertura;
        }
#endregion
#region "Funciones"
        public  Camino pathFinder(Casilla a, Casilla b, Tablero Tablero)
        {
            ArrayList cerradas = new ArrayList();
            ArrayList abiertas = new ArrayList();
            ArrayList camino = new ArrayList();
            heuristica elemento;
            int aux = 0, iaux=0;
            Casilla actual = a;
            Boolean nueva = false;
            elemento.casilla = a;
            elemento.g = 0;
            elemento.h = DistanciaAB(a.posicion(), b.posicion());
            elemento.f = elemento.h;
            elemento.padre = null;

            cerradas.Add(elemento);
            do {
                for (int i = 1; i < 7; i++) {
                    try
                    {
                        elemento.casilla = Tablero.colindante(actual, (Encaramiento)i);//<-- hay que revisar en caso de que salga del tablero
                    }
                    catch (Exception e) {
                        continue;
                    }
                    // Ignoro las casillas que ya estan en la lista de cerradas.
                    if (esta(cerradas,elemento.casilla))
                        continue;

                    // Precalculo el costo de movimiento relaciona, para no hacerlo varias veces
                    aux=Casilla.costoMovimientoAB(actual, elemento.casilla);

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
                            elemento.h = DistanciaAB(elemento.casilla.posicion(), b.posicion());
                            elemento.f = elemento.g + elemento.h;
                            elemento.padre = actual;
                            abiertas.Add(elemento);
                        }
                        
                    }
                   // if (nueva){//si hay nuevas en la lista (o revisadas)

                     
                   //}
                    nueva = false;
                }

                //buesca la mejor casilla entre las abiertas
                aux = mejorCasillaAbierta(abiertas, actual);
                //agrego la mejor casilla
                cerradas.Add(abiertas[aux]);
                //pongo la mejor como la siguiente actual
                actual = ((heuristica)abiertas[aux]).casilla;
                //borro la mejor de las abiertas
                abiertas.RemoveAt(aux);

            } while (actual != b);

            camino.Add(b);
            for (int i = abiertas.Count - 1; i > 0; i--) {
                camino.Add(((heuristica)cerradas[i]).padre);
            }
            camino.Reverse();
            return new Camino(camino);//<-!!!!! lo hace mal necesito revisar paso a paso
        }

        //Funcion que devuelve la distancia aproximada entre el punto a y el punto b. (revisada)
        public int DistanciaAB(Posicion a, Posicion b)
        {
            //dx <----La distancia entre la x de a y la x de b. Idem para la dy
            int dx = Math.Abs(a.columna() - b.columna())+1, dy = Math.Abs(b.fila() - a.fila())+1;

            // (dx^2+dy^2)^1/2<-----La parte entera
            return (int)Math.Truncate(Math.Pow((Math.Pow(dx, 2) + Math.Pow(dx, 2)), 0.5));

        }

        public int costoMovimiento() {
            
                for (int i = 0; i < _length; i++){

                    _movimientos = _camino[i].costoMovimiento();

                    if (i < _length && _camino[i].nivel() < _camino[i + 1].nivel())
                    {

                        _movimientos += Casilla.costoMovimientoAB(_camino[i], _camino[i + 1]);

                        if (_movimientos < 0)//si hay intransitables en el camino
                            return _movimientos;
                    }
                }

                return _movimientos;
        }
#endregion
        #region Privado
        private int _length;
        private Casilla[] _camino;
        private Boolean _ldv;
        private Boolean _cobertura;
        private int _nldv;
        private int _movimientos;
        /*
        private Camino pathfinder(Casilla a, Casilla b, Tablero tablero,ArrayList ceradas, ArrayList abiertas) {

            if (ceradas[ceradas.Count - 1] == b)
            {
            }
            else {
                calculaAbiertas(a,  ref abiertas, tablero);
                foreach (Casilla i in abiertas)
                {

                }
            }
            return new Camino(abiertas);//compile
        }
		
        private void calculaAbiertas(Casilla a, ref ArrayList abierto, Tablero tablero) {

            if (Math.Abs(tablero.colindante(a, Encaramiento.Arriba).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.Arriba));
            }
            if (Math.Abs(tablero.colindante(a, Encaramiento.SuperiorDerecha).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.SuperiorDerecha));
            } if (Math.Abs(tablero.colindante(a, Encaramiento.InferiorDerecho).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.InferiorDerecho));
            } if (Math.Abs(tablero.colindante(a, Encaramiento.Abajo).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.InferiorDerecho));
            } if (Math.Abs(tablero.colindante(a, Encaramiento.Abajo).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.Abajo));
            } if (Math.Abs(tablero.colindante(a, Encaramiento.InferiorIzquierda).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.InferiorIzquierda));
            } if (Math.Abs(tablero.colindante(a, Encaramiento.SuperiorIzquierda).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.SuperiorIzquierda));
            }
        }
        //puede que la borre
        private int costoMovimiento(ArrayList camino)
        {
            int costo = 0;

            for (int i = 0; i < camino.Count; i++)
            {
                costo = ((Casilla)camino[i]).costoMovimiento();

                if (i < camino.Count && ((Casilla)camino[i]).nivel() < ((Casilla)camino[i + 1]).nivel())
                {

                    costo = Casilla.costoMovimientoAB((Casilla)camino[i], (Casilla)camino[i + 1]);
                }
            }
            return costo;
        }
        //puede que la borre
        private heuristica costoMovimiento(Casilla de, Casilla a) {
            int costo = 0;

          
                costo =a.costoMovimiento();

                if ( de.nivel() < a.nivel())
                {
                    costo = Casilla.costoMovimientoAB(de, a);
                }
            
            return costo;

        }*/
        private int mejorCasillaAbierta(ArrayList abiertas,Casilla padre) {
            int max = 0;
            for (int i = 1; i < abiertas.Count; i++ )
            {
                if(((heuristica)abiertas[i-1]).f<((heuristica)abiertas[i]).f && ((heuristica)abiertas[i]).padre==padre){
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