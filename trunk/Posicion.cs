
using System;

namespace ico
{
    /// <summary>
    /// Clase que almacena datos de un posicion y proporciona funcionalidades sobre ella con respecto a tras posiciones.
    /// </summary>
	public class Posicion {
		
		/// <summary>
		/// constructor que recive una posicion en forma "CCFF" y crea una Posicion
		/// </summary>
		/// <param name="numero">cadena que representa la posicion, en formato FFCC; tipo String</param>
		public Posicion( string numero) {
			string n1 ,n2;
			n1=numero.Substring(0,2);
			n2=numero.Substring(2,2);
			
			_columna= Convert.ToInt32(n1);
			_fila=Convert.ToInt32(n2);
		}
		
        /// <summary>
        /// constructor por defecto
        /// </summary>
        public Posicion() { 
			_fila=_columna=0;
		}
		
        /// <summary>
        /// constructor que recive una fila y una columna
        /// </summary>
        /// <param name="fila">fila; tipo Int</param>
        /// <param name="columna">columna; tipo Int</param>
        public Posicion(int fila, int columna) {
            _fila = fila;
            _columna = columna;
        }

        /// <summary>
        /// establece la propidad fila
        /// </summary>
        /// <param name="fila">fila; tipo Int</param>
        public void fila(int fila) { _fila = fila; }
        /// <summary>
        /// establece la propidad columna
        /// </summary>
        /// <param name="columna">columna; tipo int</param>
        public void columna(int columna) { _columna = columna; }
        
        /*
        public void filaOfSet(int ofSet){
            _fila += ofSet;
        }
        public void columnaOfSet(int ofSet)
        {
            _columna += ofSet;
        }*/
		/// <summary>
		///  devulve el valor de la fila
		/// </summary>
		/// <returns>valor de la propidad fila; tipo Int</returns>
        public int fila() { return _fila; }
        /// <summary>
        ///  devulve el valor de la columna
        /// </summary>
        /// <returns>valor de la propidad columna; tipo Int</returns>
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

        /// <summary>
        /// funcion que calcula el costo heuristico de una camino "recto" entre esta posicion(this) y otra b. 
        /// Dicha distancia se calcula contando el numero minimo de casillas que separa esta a b, sin considerar si es posible pasar por dichas posiciones.
        /// </summary>
        /// <param name="b">posicion a la cual se quiere calcular su distancia</param>
        /// <returns>distancia en numero de casillas; tipo Int</returns>
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

        /// <summary>
        /// funcion que calcula de una posicion a a otra b la distancia heuristica. Siendo esta aproximada y no entera.
        /// Esta funcion esta obsoleta con respecto a distancia(Posicion b)
        /// </summary>
        /// <param name="a">posicion de origen</param>
        /// <param name="b">posicion de destino</param>
        /// <returns>distancia aproximada entre a y ; tipo float</returns>
        public static float distancia(Posicion a, Posicion b)
        {
            //dx <----La distancia entre la x de a y la x de b. Idem para la dy
            int dx = Math.Abs(a.columna() - b.columna()) + 1, dy = Math.Abs(b.fila() - a.fila()) + 1;

            // (dx^2+dy^2)^1/2<-----La parte entera
            return (float)Math.Pow((Math.Pow(dx, 2) + Math.Pow(dy, 2)), 0.5);

        }

        /// <summary>
        /// fila de ubicacion con respecto al tablero del juego no de la matriz de tablero
        /// </summary>
        private int _fila;
        /// <summary>
        /// columna de ubicacion con respecto al tablero del juego no de la matriz de tablero
        /// </summary>
        private int _columna;


        #region conosVision
        /// <summary>
        /// Metodo que indica si una posicion esta dentro del cono delantero de vision de un mech
        /// </summary>
        /// <param name="casilla">Posicion a observar</param>
        /// <param name="encaramiento">Encaramiento hacia donde estamos mirando</param>
        /// <returns>True en caso de estar en el cono delantero, false en caso opuesto</returns>
        public Boolean conoDelantero(Posicion casilla, int encaramiento)
        {
            Boolean enCono = false;
            int jIzq, jDrcha;

            //Calculamos las diagonales del cono
            jIzq = _columna - Math.Abs(casilla.fila() - _fila) * 2;
            jDrcha = _columna + Math.Abs(casilla.fila() - _fila) * 2;
            if (jIzq <= 0) jIzq = 1;


            //Si tiene el encaramiento 1
            if (encaramiento == 1)
            {
                if (casilla.fila() > _fila)
                {//Si esta mas abajo que la casilla donde me encuentro
                    enCono = false;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_columna % 2 == 0)
                    {//posicion pares
                        if ((casilla.columna() >= jIzq || casilla.columna() >= jIzq - 1) && (casilla.columna() <= jDrcha || casilla.columna() <= jDrcha + 1))
                            enCono = true;
                    }
                    else
                    {//posicion impar
                        if ((casilla.columna() >= jIzq || casilla.columna() >= jIzq + 1) && (casilla.columna() <= jDrcha || casilla.columna() <= jDrcha - 1))
                            enCono = true;
                    }
                }


            }//Si tiene el encaramiento 2
            else if (encaramiento == 2)
            {
                if (casilla.columna() < _columna)
                {//Si esta mas a la izquierda que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() <= _fila)
                {
                    //Vemos esta en la zona superior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_columna % 2 != 0)
                    {//posicion impares
                        if (casilla.columna() >= jDrcha || casilla.columna() >= jDrcha + 1)
                            enCono = true;
                    }
                    else
                    {//posicion pares
                        if (casilla.columna() >= jDrcha || casilla.columna() >= jDrcha - 1)
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 3
            else if (encaramiento == 3)
            {
                if (casilla.columna() < _columna)
                {//Si esta mas a la izquierda que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() >= _fila)
                {
                    //Vemos esta en la zona inferior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_columna % 2 == 0)
                    {//posicion pares
                        if (casilla.columna() >= jDrcha || casilla.columna() >= jDrcha + 1)
                            enCono = true;
                    }
                    else
                    {//posicion impar
                        if (casilla.columna() >= jDrcha || casilla.columna() >= jDrcha - 1)
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 4
            else if (encaramiento == 4)
            {
                if (casilla.fila() < _fila)
                {
                    //Si esta mas arriba que la casilla donde me encuentro
                    enCono = false;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_columna % 2 != 0)
                    {//posicion impares
                        if ((casilla.columna() >= jIzq || casilla.columna() >= jIzq - 1) && (casilla.columna() <= jDrcha || casilla.columna() <= jDrcha + 1))
                            enCono = true;
                    }
                    else
                    {//posicion pares
                        if ((casilla.columna() >= jIzq || casilla.columna() >= jIzq + 1) && (casilla.columna() <= jDrcha || casilla.columna() <= jDrcha - 1))
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 5
            else if (encaramiento == 5)
            {
                if (casilla.columna() > _columna)
                {//Si esta mas a la derecha que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() >= _fila)
                {
                    //Vemos esta en la zona inferior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_columna % 2 == 0)
                    {//posicion pares
                        if ((casilla.columna() <= jIzq || casilla.columna() <= jIzq - 1))
                            enCono = true;
                    }
                    else
                    {//posicion impar
                        if ((casilla.columna() <= jIzq || casilla.columna() <= jIzq + 1))
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 6
            else if (encaramiento == 6)
            {
                if (casilla.columna() > _columna)
                {//Si esta mas a la derecha que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() <= _fila)
                {
                    //Vemos esta en la zona superior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_columna % 2 != 0)
                    {//posicion impares
                        if (casilla.columna() <= jIzq || casilla.columna() <= jIzq - 1)
                            enCono = true;
                    }
                    else
                    {//posicion pares
                        if (casilla.columna() <= jIzq || casilla.columna() <= jIzq + 1)
                            enCono = true;
                    }
                }

            }

            return enCono;
        }

        /// <summary>
        /// Metodo que indica si una posicion esta dentro del cono trasero de vision de un mech
        /// </summary>
        /// <param name="casilla">Posicion a observar</param>
        /// <param name="encaramiento">Encaramiento hacia donde estamos mirando</param>
        /// <returns>True en caso de estar en el cono delantero, false en caso opuesto</returns>
        public Boolean conoTrasero(Posicion casilla, int encaramiento)
        {
            Boolean enCono = false;
            int jIzq, jDrcha;
            //Console.WriteLine(casilla.columna() + " "+casilla.fila());
            //Console.WriteLine(encaramiento);
            //Calculamos las diagonales del cono
            jIzq = _columna - Math.Abs(casilla.fila() - _fila) * 2;
            jDrcha = _columna + Math.Abs(casilla.fila() - _fila) * 2;
            if (jIzq <= 0) jIzq = 1;


            //Si tiene el encaramiento 1
            if (encaramiento == 1)
            {
                if (casilla.fila() < _fila)
                {
                    //Si esta mas arriba que la casilla donde me encuentro
                    enCono = false;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_columna % 2 != 0)
                    {//posicion impares
                        if (casilla.columna() > jIzq && casilla.columna() < jDrcha)
                            enCono = true;
                    }
                    else
                    {//posicion pares
                        if (casilla.columna() > jIzq + 1 && casilla.columna() < jDrcha - 1)
                            enCono = true;
                    }
                }


            }//Si tiene el encaramiento 2
            else if (encaramiento == 2)
            {
                if (casilla.columna() >= _columna)
                {//Si esta mas a la derecha que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() > _fila)
                {
                    //Vemos esta en la zona inferior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_columna % 2 == 0)
                    {//posicion pares
                        if (casilla.columna() < jIzq - 1)
                            enCono = true;
                    }
                    else
                    {//posicion impar
                        if (casilla.columna() < jIzq)
                            enCono = true;
                    }
                }
            }//Si tiene el encaramiento 3
            else if (encaramiento == 3)
            {
                if (casilla.columna() >= _columna)
                {//Si esta mas a la derecha que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() < _fila)
                {
                    //Vemos esta en la zona superior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_columna % 2 != 0)
                    {//posicion impares
                        if (casilla.columna() < jIzq - 1)
                            enCono = true;
                    }
                    else
                    {//posicion pares
                        if (casilla.columna() < jIzq)
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 4
            else if (encaramiento == 4)
            {
                if (casilla.fila() > _fila)
                {//Si esta mas abajo que la casilla donde me encuentro
                    enCono = false;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_columna % 2 == 0)
                    {//posicion pares
                        if (casilla.columna() > jIzq && casilla.columna() < jDrcha)
                            enCono = true;
                    }
                    else
                    {//posicion impar
                        if (casilla.columna() > jIzq + 1 && casilla.columna() < jDrcha - 1)
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 5
            else if (encaramiento == 5)
            {
                if (casilla.columna() <= _columna)
                {//Si esta mas a la izquierda que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() < _fila)
                {
                    //Vemos esta en la zona superior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_columna % 2 != 0)
                    {//posicion impares
                        if (casilla.columna() > jDrcha + 1)
                            enCono = true;
                    }
                    else
                    {//posicion pares
                        if (casilla.columna() > jDrcha)
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 6
            else if (encaramiento == 6)
            {
                if (casilla.columna() <= _columna)
                {//Si esta mas a la izquierda que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() > _fila)
                {
                    //Vemos esta en la zona inferior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_columna % 2 == 0)
                    {//posicion pares
                        if (casilla.columna() > jDrcha + 1)
                            enCono = true;
                    }
                    else
                    {//posicion impar
                        if (casilla.columna() > jDrcha)
                            enCono = true;
                    }
                }
            }

            return enCono;
        }

        /// <summary>
        /// Metodo que indica si una posicion esta dentro del cono derecho de vision de un mech
        /// </summary>
        /// <param name="casilla">Posicion a observar</param>
        /// <param name="encaramiento">Encaramiento hacia donde estamos mirando</param>
        /// <returns>True en caso de estar en el cono delantero, false en caso opuesto</returns>
        public Boolean conoDerecho(Posicion casilla, int encaramiento)
        {
            Boolean enCono = false;

            if (!conoDelantero(casilla, encaramiento) && !conoTrasero(casilla, encaramiento))
            {
                if (encaramiento == 1)
                {
                    if (_columna < casilla.columna())
                        enCono = true;
                }
                else if (encaramiento == 2)
                {
                    if (_fila < casilla.fila())
                        enCono = true;
                }
                else if (encaramiento == 3)
                {
                    if (_fila < casilla.fila())
                        enCono = true;
                }
                else if (encaramiento == 4)
                {
                    if (_columna > casilla.columna())
                        enCono = true;
                }
                else if (encaramiento == 5)
                {
                    if (_fila > casilla.fila())
                        enCono = true;
                }
                else
                {
                    if (_fila >= casilla.fila())
                        enCono = true;
                }

            }

            return enCono;
        }

        /// <summary>
        /// Metodo que indica si una posicion esta dentro del cono izquierdo de vision de un mech
        /// </summary>
        /// <param name="casilla">Posicion a observar</param>
        /// <param name="encaramiento">Encaramiento hacia donde estamos mirando</param>
        /// <returns>True en caso de estar en el cono delantero, false en caso opuesto</returns>
        public Boolean conoIzquierdo(Posicion casilla, int encaramiento)
        {
            Boolean enCono = false;

            if (!conoDelantero(casilla, encaramiento) && !conoTrasero(casilla, encaramiento) && !conoDerecho(casilla, encaramiento))
            {
                enCono = true;
            }

            return enCono;
        }

        #endregion
    }
}
