using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ico
{
    public class Tablero
    {
        #region Constructores
        public Tablero(uint filas, uint columas) {
            _casillas = new Casilla[filas, columas];
        }
		
		
        public Tablero() { 
            _casillas=new Casilla[12,30];
        }
		
		
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
        }
		
		
        public Casilla Casilla(Posicion p) {
            if (p.fila() - 1 < 0 || p.columna() - 1 < 0) {
                return null;
            }else
                return _casillas[p.fila() - 1, p.columna() - 1];
        }

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


        public void casillasEnRadio( Posicion actual, List<Posicion> casillas , int radio ) {
            for (int i = 0; i < _filas; i++)
                for (int j = 0; j < _columnas; j++) {
                    if (this._casillas[i, j].posicion().distancia(actual) <= radio && actual != this._casillas[i,j].posicion() )
                        casillas.Add(_casillas[i, j].posicion());
                }
        }

        public void casillasEnMov(Mech ich, List<Posicion> casillas, int puntMov) 
        {
            int dist;
            for (int i = 0; i < _filas; i++)
                for (int j = 0; j < _columnas; j++)
                {
                    dist = this._casillas[i, j].posicion().distancia(ich.posicion());
                    if ( dist < puntMov) {
                        //si esta en cono derecho o izquierdo
                        if ( (ich.conoDerecho(this._casillas[i, j].posicion(), ich.ladoEncaramiento()) || ich.conoIzquierdo(this._casillas[i, j].posicion(), ich.ladoEncaramiento())) && dist<puntMov-2)
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

        public int filas() { return _filas; }
        public int columnas() { return _columnas; }

        #endregion

        #region Privados
        private Casilla[,] _casillas;
        int _filas; 
        int _columnas;
        #region Funciones privadas		
		
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
		
		
        private void rellenaHeuristica( ArrayList casillas) { 
        }
        #endregion
        #endregion
    }
   
}
