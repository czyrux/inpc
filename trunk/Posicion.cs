
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

        /*
         * Devuelve la distancia que separa a una posicion de la otra
         */ 
        public int distancia(Posicion b)
        {
            int colD = Math.Abs(columna() - b.columna());
            int filOff = colD/2;

            if (((columna() % 2) == 1) && ((b.columna() % 2) == 0)) filOff += 1;

            int fmin = fila() - filOff;
            int fmax = fmin + colD;
            int fmod = 0;

            if (b.fila() < fmin) fmod = fmin - b.fila();
            if (b.fila() > fmax) fmod = b.fila() - fmax;

            return colD + fmod;
        }

        //Funcion que devuelve la distancia aproximada entre el punto a y el punto b. (revisada)
        public static float distancia(Posicion a, Posicion b)
        {
            //dx <----La distancia entre la x de a y la x de b. Idem para la dy
            int dx = Math.Abs(a.columna() - b.columna()) + 1, dy = Math.Abs(b.fila() - a.fila()) + 1;

            // (dx^2+dy^2)^1/2<-----La parte entera
            return (float)Math.Pow((Math.Pow(dx, 2) + Math.Pow(dy, 2)), 0.5);

        }
	}
}
