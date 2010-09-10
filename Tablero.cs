using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ico
{
    /// <summary>
    /// Clase que almacena informacino de el tablero del juego y funciones que generan informacion referente a el mismo.
    /// </summary>
    public class Tablero
    {
        #region Constructores
		/// <summary>
		/// Constructor de tablero, recive el archivo tablero a leer
		/// </summary>
		/// <param name="nombreMapa">archivo funte del tablero; tipo string</param>
        public Tablero(string nombreMapa) {
            try
            {
                StreamReader fich = new StreamReader(nombreMapa);
                fich.ReadLine();
                _filas = Convert.ToInt32(fich.ReadLine());
                _columnas = Convert.ToInt32(fich.ReadLine());
                _casillas = new Casilla[_filas, _columnas];
                for (int c = 0; c < _columnas; c++)
                {
                    for (int f = 0; f < _filas; f++)
                    {

                        _casillas[f, c] = new Casilla();
                        _casillas[f, c].posicion(new Posicion(f+1, c+1));
                        _casillas[f, c].nivel(Convert.ToInt32(fich.ReadLine()));
                        _casillas[f, c].tipoTerreno(Convert.ToInt32(fich.ReadLine()));
                        _casillas[f, c].objetoTerreno(Convert.ToInt32(fich.ReadLine()));
                        _casillas[f, c].fceEdificio(Convert.ToInt32(fich.ReadLine()));
                        _casillas[f, c].edificioDerrumbado(Convert.ToBoolean(fich.ReadLine()));
                        _casillas[f, c].fuego(Convert.ToBoolean(fich.ReadLine()));
                        _casillas[f, c].humo(Convert.ToBoolean(fich.ReadLine()));
                        _casillas[f, c].nGarrotes(Convert.ToInt32(fich.ReadLine()));
                        for (int i = 0; i < 6; i++)
                        {
                            _casillas[f, c].caras(Convert.ToBoolean(fich.ReadLine()), false, i);
                        }
                        for (int i = 0; i < 6; i++)
                        {
                            _casillas[f, c].caras(false, Convert.ToBoolean(fich.ReadLine()), i);
                        }
                    }
                }
                fich.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.Beep(10000, 5000);
            }

        }
        #endregion

        #region Funciones
        /*
        public void casillaInfo(int fila, int columna) {
            int n = 0;
            Console.WriteLine("Casilla (" + fila + "," + columna + ") ");
            fila--;
            columna--;
            Console.WriteLine("Nivel= " + _casillas[fila, columna].nivel().ToString());
            Console.WriteLine("Tipo de terreno= " + _casillas[fila, columna].tipoTerreno().ToString());
            Console.WriteLine("Objeto en el terreno= " + _casillas[fila, columna].objetoTerreno().ToString());
            Console.WriteLine("FCD Edificio= " + _casillas[fila, columna].fceEdificio().ToString());
            Console.WriteLine("Edificio derrumbado= " + _casillas[fila, columna].edificioDerrumbado().ToString());
            Console.WriteLine("Fuego= " + _casillas[fila, columna].fuego().ToString());
            Console.WriteLine("Humo= " + _casillas[fila, columna].humo().ToString());
            Console.WriteLine("Num. garrotes= " + _casillas[fila, columna].nGarrotes().ToString());
            foreach (Cara i in _casillas[fila,columna].caras()){

                Console.WriteLine("Cara" + n.ToString());
                Console.WriteLine("Carretra= " + i.carretera().ToString());
                Console.WriteLine("Rio= " + i.rio().ToString());
                n++;
            }
        }*/
		
		/// <summary>
		/// funcion que devuelve la casilla que esta en la posicion <paramref name="p"/>
		/// </summary>
		/// <param name="p">posicion</param>
		/// <returns></returns>
        public Casilla Casilla(Posicion p) {
            if (p.fila() - 1 < 0 || p.columna() - 1 < 0) {
                return null;
            }else
                return _casillas[p.fila() - 1, p.columna() - 1];
        }
        /// <summary>
        /// funcion que indicandole la posicion en la que estas y el encaramiento deseado develve la casilla que se encuentra hacia ese encaramiento
        /// </summary>
        /// <param name="actual">posicion de la casilla la cual deseas saber la colindante; tipo Posicion</param>
        /// <param name="direccion">direcion en la cual deseas buscar la colindante; tipo Encaramiento</param>
        /// <returns>Devuelve la casilla colindante a <paramref name="actual"/> en el el encaramiento <paramref name="direccion"/></returns>
        public Casilla colindante(Posicion actual, Encaramiento direccion)
        {//revisada v2
            Casilla devolver = null;
            if (direccion == Encaramiento.Arriba || direccion == Encaramiento.Abajo)
            {
                if (direccion == Encaramiento.Arriba)
                {
                    devolver = _casillas[actual.fila() -2, actual.columna() - 1];
                }
                else
                {
                    devolver = _casillas[actual.fila() , actual.columna()-1];
                }
            }
            else if (((actual.columna() ) % 2) == 0)//par
            {
                switch (direccion)
                {
                    case Encaramiento.SuperiorDerecha:
                        devolver = _casillas[actual.fila() - 1, actual.columna()];
                        break;
                    case Encaramiento.InferiorDerecho:
                        devolver = _casillas[actual.fila(), actual.columna()];
                        break;
                    case Encaramiento.InferiorIzquierda:
                        devolver = _casillas[actual.fila(), actual.columna() - 2];
                        break;
                    case Encaramiento.SuperiorIzquierda:
                        devolver = _casillas[actual.fila() - 1, actual.columna() - 2];
                        break;
                }
            }
            else  //impar
            {
                switch (direccion)
                {
                    case Encaramiento.SuperiorDerecha:
                        devolver = _casillas[actual.fila() - 2, actual.columna()];
                        break;
                    case Encaramiento.InferiorDerecho:
                        devolver = _casillas[actual.fila() - 1, actual.columna()];
                        break;
                    case Encaramiento.InferiorIzquierda:
                        devolver = _casillas[actual.fila() - 1, actual.columna() - 2];
                        break;
                    case Encaramiento.SuperiorIzquierda:
                        devolver = _casillas[actual.fila() - 2, actual.columna() - 2];
                        break;
                }

            }
            return devolver;
        }

        /// <summary>
        /// Devuelve las casillas alrededor de la posicion <paramref name="actual"/> de tamaño <paramref name="radio"/> ignorando las ocupadas por los <paramref name="mechs"/>.
        /// </summary>
        /// <param name="actual">el centro del radio a generar; tipo Posicion</param>
        /// <param name="radio">el tamaño del radio; tipo int</param>
        /// <param name="mechs">lista de mechs para saber su localizacion; tipo Mech[]</param>
        /// <param name="my">inidice sobre la el vector de <paramref name="mechs"/> que representa mi mech; tipo int</param>
        /// <returns>devuelve la lista de casillas que representan todas las del radio</returns>
        public List<Posicion> casillasEnRadio( Posicion actual  , int radio , Mech[] mechs , int my ) {
            List<Posicion> casillas = new List<Posicion>();
            for (int i = 0; i < _filas; i++)
                for (int j = 0; j < _columnas; j++) {
                    if (this._casillas[i, j].posicion().distancia(actual) <= radio && !casillaOcupada(_casillas[i, j].posicion(), mechs, my))
                        casillas.Add(_casillas[i, j].posicion());
                }

            return casillas;
        }

        /// <summary>
        /// Devuelve las casillas alrededor del mech <paramref name="ich"/> de radio <paramref name="puntMov"/> que son los puntos de movimientos. 
        /// Esta funcion ademas tiene algunas consideraciones sobre el movimiento.
        /// </summary>
        /// <param name="ich">el mech sobre el cual se calculara el radio; tipo Mech</param>
        /// <param name="puntMov">los puntos de movimientos del mech sobre la cual se calculara el radio; tipo int</param>
        /// <param name="mechs">lista de todos los mechs del juego; tipo Mech[]</param>
        /// <returns>devuelve la lista de casillas que representan todas las del radio</returns>
        public List<Posicion> casillasEnMov(Mech ich, int puntMov, Mech[] mechs ) 
        {
            List<Posicion> casillas = new List<Posicion>();
            if (puntMov != 0)
            {
                int dist;
                for (int i = 0; i < _filas; i++)
                    for (int j = 0; j < _columnas; j++)
                    {
                        dist = this._casillas[i, j].posicion().distancia(ich.posicion());
                        if (dist < puntMov && !casillaOcupada(_casillas[i, j].posicion(), mechs, ich.numeroJ()))
                        {
                            //si esta en cono derecho o izquierdo
                            if ((ich.conoDerecho(this._casillas[i, j].posicion(), ich.ladoEncaramiento()) || ich.conoIzquierdo(this._casillas[i, j].posicion(), ich.ladoEncaramiento())) && dist < puntMov - 2)
                            {
                                casillas.Add(_casillas[i, j].posicion());
                            }
                            else if (ich.conoTrasero(this._casillas[i, j].posicion(), ich.ladoEncaramiento()) && dist < puntMov - 3)
                            {
                                casillas.Add(_casillas[i, j].posicion());
                            }
                            else
                                casillas.Add(_casillas[i, j].posicion());
                        }
                    }
            }
            else
                casillas.Add(ich.posicion());

            return casillas;
        }

        /// <summary>
        /// funcion que indica si una casilla esta ocupada por alguno de los <paramref name="mechs"/> que no se el mio (<paramref name="my"/>)
        /// </summary>
        /// <param name="p">posicion que queremos saber si esta ocupada; tipo int</param>
        /// <param name="mechs">lista de mechs en el juego; tipo mech[]</param>
        /// <param name="my">indice sobre el vector de <paramref name="mechs"/> que representa mi mech, tipo int</param>
        /// <returns>devuelve true si esta ocupado por un mech que no se yo; false por el contrario</returns>
        static public bool casillaOcupada( Posicion p , Mech[] mechs , int my ) 
        {
            bool ocupada = false;

            foreach ( Mech i in mechs)
            {
                if (i.numeroJ() != my && i.posicion().ToString() == p.ToString())
                    ocupada = true;
            }

            return ocupada;
        }

        /// <summary>
        /// devuelve el numero de filas del tablero
        /// </summary>
        /// <returns>numero de filas; tipo int</returns>
        public int filas() { return _filas; }

        /// <summary>
        /// devuelve el numero de columnas del tablero
        /// </summary>
        /// <returns>numero de columnas; tipo int</returns>
        public int columnas() { return _columnas; }

        #endregion

        #region Privados
        /// <summary>
        /// la matriz de casillas
        /// </summary>
        private Casilla[,] _casillas;
        /// <summary>
        /// el numero de filas que tiene la matriz de casillas
        /// </summary>
        int _filas; 
        /// <summary>
        /// numero de columnas que tine la matriz de casillas
        /// </summary>
        int _columnas;
        #region Funciones privadas		
		/*
        public Casilla desplazamientoRecto(Casilla actual, Encaramiento direccion ,int movimiento) {
            Casilla devolver = null;
            if (direccion == Encaramiento.Arriba || direccion == Encaramiento.Abajo)
            {
                if (direccion == Encaramiento.Arriba)
                {
                    devolver = _casillas[actual.posicion().fila() + movimiento, actual.posicion().columna() - 1 + movimiento];
                }
                else
                {
                    devolver = _casillas[actual.posicion().fila() - 1 + movimiento, actual.posicion().columna() + movimiento];
                }
            }
            else if (((actual.posicion().columna() - 1) % 2) == 0)//par
            {
                switch (direccion)
                {
                    case Encaramiento.SuperiorDerecha:
                        devolver = _casillas[actual.posicion().fila() - 1 - (movimiento / 2), actual.posicion().columna() - 1 + movimiento];
                        break;
                    case Encaramiento.InferiorDerecho:
                        devolver = _casillas[actual.posicion().fila() - 1 + (movimiento / 2), actual.posicion().columna() + movimiento];
                        break;
                    case Encaramiento.InferiorIzquierda:
                        devolver = _casillas[actual.posicion().fila() - 1 + (movimiento / 2), actual.posicion().columna() - 1 -  movimiento];
                        break;
                    case Encaramiento.SuperiorIzquierda:
                        devolver = _casillas[actual.posicion().fila() - 1 - (movimiento / 2), actual.posicion().columna() - 1 - movimiento];
                        break;
                }
            }
            else  //impar
            {
                switch (direccion)
                {
                    case Encaramiento.SuperiorDerecha:
                        devolver = _casillas[actual.posicion().fila() - 1, actual.posicion().columna()-1];
                        break;
                    case Encaramiento.InferiorDerecho:
                        devolver = _casillas[actual.posicion().fila() - 2, actual.posicion().columna()];
                        break;
                    case Encaramiento.InferiorIzquierda:
                        devolver = _casillas[actual.posicion().fila() - 2, actual.posicion().columna() - 2];
                        break;
                    case Encaramiento.SuperiorIzquierda:
                        devolver = _casillas[actual.posicion().fila() - 1, actual.posicion().columna() - 2];
                        break;
                }

            }
            return devolver;
        }
		
		*/
 
        #endregion
        #endregion
    }
   
}
