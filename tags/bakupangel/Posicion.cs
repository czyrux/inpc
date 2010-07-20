
using System;

namespace ico
{

	public class Posicion {
		
		private int _fila;
		private int _columna;
		
		public Posicion( string numero) {
			string n1 ,n2;
			n1=numero.Substring(0,2);
			n2=numero.Substring(2,2);
			
			_columna= Convert.ToInt32(n1);
			_fila=Convert.ToInt32(n2);
		}
		
        public Posicion() { 
			_fila=_columna=0;
		}
		
        public Posicion(int fila, int columna) {
            _fila = fila;
            _columna = columna;
        }

        public void fila(int fila) { _fila = fila; }
        public void columna(int columna) { _columna = columna; }
        
        public void filaOfSet(int ofSet){
            _fila += ofSet;
        }
        public void columnaOfSet(int ofSet)
        {
            _columna += ofSet;
        }
		public int fila(){ return _fila; }
		public int columna() { return _columna; }

        public override String ToString() {
            String posicion;

            if (_columna < 10)
                posicion = "0" + _columna.ToString();
            else
                posicion = _columna.ToString();

            if (_fila < 10)
                posicion += "0" + _fila.ToString();
            else
                posicion += _fila.ToString();

            return posicion;
        }
	}
}
