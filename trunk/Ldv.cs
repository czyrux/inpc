using System;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace ico
{
    public class LdV
    {     
        #region Constructores
        public LdV(Mech p1, Mech p2,Tablero tablero) {
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
        }

        public LdV(ArrayList camino)
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
     
        public Casilla casilla(int i) {
            return _camino[i];
        }

        public int movimentos() { return _movimientos; }

        public void print()
        { 
            string str="El camino es: ";
            foreach (Casilla i in _camino)
                str += "("+i.posicion().ToString()+", "+ "dir" +")"+ "->";
            str += "FIN";
            Console.WriteLine(str);
        }

        #endregion

        #region Atributos
        private int _length;
        private Casilla[] _camino;
        private Boolean _ldv;
        private Boolean _cobertura;
        private int _nldv;
        private int _movimientos;
        #endregion

    }
    
}
