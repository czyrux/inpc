using System;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace ico
{
    /// <summary>
    /// Clase usada para llamar al programa LDVyC.exe, y obtener los datos que de su ejecucion
    /// derivan
    /// </summary>
    public class LdV
    {     
        #region Constructores
        /// <summary>
        /// Constructor con parametros. Se encarga de llamar al programa LDVyC.exe, y de leer 
        /// los resultados de su ejecucion
        /// </summary>
        /// <param name="p1">Mech manejado por el jugador</param>
        /// <param name="p2">Mech objetivo</param>
        /// <param name="tablero">Tablero de la partida</param>
        public LdV(Mech p1, Mech p2,Tablero tablero) {
            Process proc = new Process();
            //_movimientos = _nldv = 0;
            String[] nodos;
            proc.StartInfo.WorkingDirectory = @".";
            proc.StartInfo.FileName = "LDVyC.exe";
            string str= PanelControl.Path+"mapaJ"+p1.numeroJ().ToString()+".sbt " + p1.posicion().ToString() + " ";
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

            if (nodos[0] != "")
            {
                _camino[0] = tablero.Casilla(p1.posicion());
                for (int i = 1; i <= nodos.Length; i++)
                {
                    _camino[i] = tablero.Casilla(new Posicion(nodos[i - 1]));
                }
                _camino[nodos.Length + 1] = tablero.Casilla(p2.posicion());
                _length = _camino.Length;
            }
            else
                _length = 0;
            
        }


        /// <summary>
        /// Constructor con parametros. Se encarga de llamar al programa LDVyC.exe, y de leer 
        /// los resultados de su ejecucion
        /// </summary>
        /// <param name="p1">Posicion donde estaremos situados</param>
        /// <param name="numeroJ">Numero del jugador</param>
        /// <param name="p2">Mech enemigo</param>
        /// <param name="tablero">Mapa del juego</param>
        public LdV(Posicion p1, int numeroJ , Mech p2, Tablero tablero)
        {
            Process proc = new Process();
            String[] nodos;
            proc.StartInfo.WorkingDirectory = @".";
            proc.StartInfo.FileName = "LDVyC.exe";
            string str = PanelControl.Path+"mapaJ" + numeroJ.ToString() + ".sbt " + p1.ToString() + " ";
            //suponemos que no esta en el suelo
            str += "1 ";
            str += p2.posicion().ToString() + " ";
            if (p2.enSuelo())
            {
                str += "0";
            }
            else
                str += "1";

            proc.StartInfo.Arguments = str;
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

            if (nodos[0] != "")
            {
                _camino[0] = tablero.Casilla(p1);
                for (int i = 1; i <= nodos.Length; i++)
                {
                    _camino[i] = tablero.Casilla(new Posicion(nodos[i - 1]));
                }
                _camino[nodos.Length + 1] = tablero.Casilla(p2.posicion());
                _length = _camino.Length;
            }
            else
                _length = 0;
        }


        /// <summary>
        /// Constructor con parametros
        /// </summary>
        /// <param name="LdV">ArrayList con una LdV</param>
        public LdV(ArrayList LdV)
        {
            _length = LdV.Count;
            _camino = new Casilla[_length];
            for (int i = 0; i < _length; i++)
            {
                _camino[i] = (Casilla)LdV[i];
            }
            _ldv =_cobertura= false;
            //_nldv = _movimientos = 0;
        }
        #endregion

        #region Propiedades

        /// <summary>
        /// Devuelve la longitud de la LdV
        /// </summary>
        /// <returns>Entero</returns>
        public int longitud() {
            return _length;
        }

        /// <summary>
        /// Devuelve el array de casillas que componen la LdV
        /// </summary>
        /// <returns>Array Casilla</returns>
        public Casilla[] getLdV() {
            return _camino;
        }

        /// <summary>
        /// Indica si hay linea de vision con el objetivo
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
        public Boolean ldv() {
            return _ldv;
        }

        /// <summary>
        /// Indica si el objetivo tiene cobertura parcial
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
        public Boolean cobertura() {
            return _cobertura;
        }
        #endregion

        #region Metodos
     
        /// <summary>
        /// Devuelve la casilla del indice
        /// </summary>
        /// <param name="i">Indice</param>
        /// <returns>Casilla</returns>
        public Casilla casilla(int i) {
            return _camino[i];
        }

        //public int movimentos() { return _movimientos; }

        /// <summary>
        /// Imprime en la consola el camino
        /// </summary>
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
        /// <summary>
        /// Longitud del camino
        /// </summary>
        private int _length;

        /// <summary>
        /// Array de casillas que componen el camino
        /// </summary>
        private Casilla[] _camino;

        /// <summary>
        /// Booleano para indicar si hay linea de vision
        /// </summary>
        private Boolean _ldv;

        /// <summary>
        /// Booleano para indicar si hay cobertura
        /// </summary>
        private Boolean _cobertura;

        //private int _nldv;
        //private int _movimientos;
        #endregion

    }
    
}
