
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
            int dx , dy , jIzq , jDrcha;
            dx = Math.Abs(columna() - b.columna()) ;
            dy = Math.Abs(b.fila() - fila()) ;
            //Console.WriteLine("dx: "+dx);
            //Console.WriteLine("dy:"+dy);
            //Console.WriteLine("Raiz: "+Math.Sqrt((Math.Pow(dx, 2) + Math.Pow(dy, 2))));


            //Calculamos la posicion de la diagonal
            jIzq = columna() - Math.Abs(fila() - b.fila()) * 2;
            jDrcha = columna() + Math.Abs(fila() - b.fila()) * 2;
            if (jIzq <= 0) jIzq = 1;
            if (columna() % 2 == 0 )
            {
                if (fila() > b.fila())
                {
                    Console.WriteLine("jIzq: " + (jIzq - 1) + " " + jIzq);
                    Console.WriteLine("jDrcha: " + jDrcha + " " + (jDrcha + 1));
                }
                else {
                    Console.WriteLine("jIzq: " + (jIzq + 1) + " " + jIzq);
                    Console.WriteLine("jDrcha: " + jDrcha + " " + (jDrcha - 1));
                }
            }
            else {
                if (fila() > b.fila())
                {
                    Console.WriteLine("jIzq: " + (jIzq + 1) + " " + jIzq);
                    Console.WriteLine("jDrcha: " + jDrcha + " " + (jDrcha - 1));
                }
                else {
                    Console.WriteLine("jIzq: " + (jIzq - 1) + " " + jIzq);
                    Console.WriteLine("jDrcha: " + jDrcha + " " + (jDrcha + 1));
                }
            }
            

            //Si esta en la misma fila o columna o en la diagonal
            if (dx == 0 || dy == 0 )/*|| (columna() % 2 == 0 && ((b.columna() == jIzq || b.columna() == jIzq - 1) || (b.columna() == jDrcha || b.columna() == jDrcha + 1)))
                || (columna() % 2 != 0 && ((b.columna() == jIzq || b.columna() == jIzq + 1) || (b.columna() == jDrcha || b.columna() == jDrcha - 1))))*/
            {
                Console.WriteLine("En lineas");
                return (int)Math.Truncate(Math.Sqrt((Math.Pow(dx, 2) + Math.Pow(dy, 2))));
            }
            else //vemos si esta en las diagonales
            {
                //diagonales superiores
                if ( fila()>b.fila() )
                Console.WriteLine("No lineas");
                return (int)(Math.Truncate(Math.Sqrt((Math.Pow(dx, 2) + Math.Pow(dy, 2)))) + 1);
            }

            
        }
	}
}
