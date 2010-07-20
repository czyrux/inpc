using System;
using System.Collections;
//using System.Linq;
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

        public Camino pathFinder(Casilla a, Casilla b, Casilla[] Tablero)
        {
            ArrayList cerrado = new ArrayList();
            ArrayList abierto = new ArrayList();
            int n = 0;
            
            return new Camino(new ArrayList());

        }

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
        #endregion

    }
}
