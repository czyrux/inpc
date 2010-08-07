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
            ArrayList cerrado = new ArrayList();
            ArrayList abierto = new ArrayList();
            int n = 0;
            Casilla actual=a;

            do {
                cerrado.Add(actual);
                for (int i = 1; i < 7; i++) {
                    abierto.Add(Tablero.colindante(actual, (Encaramiento)i));
                }

            } while (true);
            return new Camino(new ArrayList());


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

        private int costoMovimiento(ArrayList camino)
        {
            int costo = 0;

            for (int i = 0; i < camino.Count; i++)
            {
                costo = ((Casilla)camino[i]).costoMovimiento();

                if (i < camino.Count && ((Casilla)camino[i]).nivel() < ((Casilla)camino[i + 1]).nivel())
                {

                    switch ((((Casilla)camino[i + 1]).nivel() - ((Casilla)camino[i]).nivel()))
                    {
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

        private int costoMovimiento(Casilla de, Casilla a) {
            int costo = 0;

          
                costo =a.costoMovimiento();

                if ( de.nivel() < a.nivel())
                {

                    switch ((a.nivel() - de.nivel()))
                    {
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
            
            return costo;

        }
        
        #endregion

    }
}
