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
            _moviminetos = _nldv = 0;
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

            //rellenamos heuristica?¿
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
            _nldv = _moviminetos = 0;
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
        public Camino pathFinder(Casilla a, Casilla b, Tablero Tablero)
        {
            ArrayList cerradas = new ArrayList();
            ArrayList abiertas = new ArrayList();
            heuristica elemento;
            int aux = 0, aux2=0;
            Casilla actual=a;

            do {
                cerradas.Add(actual);
                for (int i = 1; i < 7; i++) {
                    elemento.casilla = Tablero.colindante(actual, (Encaramiento)i);

                    if (esta(cerradas,elemento.casilla))// Ignoro las casillas que ya estan en la lista de cerradas.
                        continue;

                    aux=Casilla.costoMovimientoAB(actual, elemento.casilla);// Precalculo la el costo de movimiento relaciona, para no hacer varias veces
                    if (elemento.casilla.costoMovimiento() != int.MaxValue ||  aux!= int.MaxValue)// verifico si es intrancitable
                    {
                        elemento.g = elemento.casilla.costoMovimiento() + aux;// precalculo el costo de movimiento total a esa casilla desde la actual
                        if ((aux2=estaEn(abiertas, elemento.casilla))>=0)// verifico si la casilla acual ya esta entre las abiertas
                        {
                            if (elemento.g > ((heuristica)abiertas[aux2]).g)// compruebo si desde la aterior era mejor que esta
                            {
                                continue;// si, si continuo sin hacer cambios
                            }
                            else {//si no modifico la casilla antigua
                                elemento.h = ((heuristica)abiertas[aux2]).h;
                                elemento.f = elemento.g + elemento.h;
                                abiertas[aux2]=(elemento);
                            }

                        }
                        else//si no estaba entre las abiertas incerto la nueva casilla
                        {
                            elemento.h = DistanciaAB(elemento.casilla.posicion(), b.posicion());
                            elemento.f = elemento.g + elemento.h;
                            abiertas.Add(elemento);
                        }
                        
                    }
                    aux = mejorCasillaAbierta(abiertas);//buesca la mejor casilla entre las abiertas
                    cerradas.Add(abiertas[aux]);//agrego la mejor casilla
                    actual = (Casilla)abiertas[aux];//pongo la mejor como la siguiente actual
                    abiertas.RemoveAt(aux);//borro la mejor de las abiertas
                }

            } while (true);
            return new Camino(new ArrayList());


        }
        //Funcion que devuelve la distancia aproximada entre el punto a y el punto b.
        public int DistanciaAB(Posicion a, Posicion b)
        {
            //dx <----La distancia entre la x de a y la x de b. Idem para la dy
            int dx = Math.Abs(a.columna() + b.columna()), dy = Math.Abs(b.columna() + a.columna());

            // (dx^2+dy^2)^1/2<-----La parte entera
            return (int)Math.Truncate(Math.Pow((Math.Pow(dx, 2) + Math.Pow(dx, 2)), 0.5));

        }

        public int costoMovimiento() {
            int costo = 0;

            for (int i = 0; i < _length; i++)
            {
                costo = _camino[i].costoMovimiento();

                if(i<_length && _camino[i].nivel() < _camino[i+1].nivel()){

                    switch((_camino[i + 1].nivel() - _camino[i].nivel())) {
                        case 1:
                            costo++;
                        break;
                        case 2:
                            costo += 1;
                        break;
                        default:
                            return int.MaxValue;//<--- Es inaccesible devuelve el maximo valor posible

                    }
                }
            }
            return costo;
        }
#endregion
        #region Privado
        private int _length;
        private Casilla[] _camino;
        private Boolean _ldv;
        private Boolean _cobertura;
        private int _nldv;
        private int _moviminetos;
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
        private int mejorCasillaAbierta(ArrayList abiertas) {
            int max = 0;
            for (int i = 1; i < abiertas.Count; i++ )
            {
                if(((heuristica)abiertas[i-1]).f<((heuristica)abiertas[i]).f){
                    max = i;
                }
            }
            return max;
        }

        private int estaEn(ArrayList lista, Casilla  elem) {
            int n = 0;
            foreach (heuristica i in lista) {
                if (i.casilla.posicion().ToString() == elem.posicion().ToString())
                    return n;
                n++;
            }
            return -1;
        }

        private Boolean esta(ArrayList lista, Casilla elem) {
            foreach(heuristica i in lista){
                if (i.casilla.ToString() == elem.ToString())
                    return true;
            }
            return false;
        }
        #endregion

    }
}
