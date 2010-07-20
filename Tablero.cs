using System;
using System.Collections;
//using System.Linq;
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
                int filas = Convert.ToInt32(fich.ReadLine());
                int columnas = Convert.ToInt32(fich.ReadLine());
                _casillas = new Casilla[filas, columnas];
                for (int f = 0; f < filas; f++)
                {
                    for (int c = 0; c < columnas; c++)
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
                            _casillas[f, c].caras(Convert.ToBoolean(fich.ReadLine()), Convert.ToBoolean(fich.ReadLine()), i);
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
            return _casillas[p.fila() - 1, p.columna() - 1];
        }
        /*
        public int deCasillaACasillaMinimo(int deFila, int deColumna, int aFila,int aColumna) {
            return Math.Abs(Math.Abs((deFila + deColumna) - (aFila + aColumna)) - (Math.Min(Math.Abs(deFila-aFila),Math.Abs(deColumna-aColumna))-1));
            
        }public int masProximo(Posicion de, Posicion a) {
            int pasos = 0;
            while (a.fila()!=de.fila() || a.fila() !=de.fila()){
                if (deColumna % 2)//impar
                {
                    if (a.fila()<de.fila()||a.columna()<de.columna()) {
                        if (Math.Abs(a.fila() - de.fila()) > Math.Abs(a.columna() - de.columna()))
                        {
                            de.fila(de.fila()--);
                            pasos++;
                        }
                        else
                        {
                            de.fila(de.fila()--);
                            pasos++;
                        }
                        pasos++;
                    }else if (deFila < aFila && deColumna > aColumna) {
                        deFila++;
                        deColumna--;
                        pasos++;
                    }else if(deFila< aFila && deColumna < aColumna){
                        fila++;
                        pasos++;
                    }
                    else if (deFila > aFila && deColumna > aColumna) {
                        deFila--;
                        pasos++;
                    } else if (deFila == aFila && deColumna > aColumna) {
                        deColumna--;
                        pasos++;
                    } if (deFila == aFila && deColumna < aColumna)
                    {
                        deColumna++;
                        pasos++;
                    }
                }
                else { 
                }
            }
        }*/
        public int g(Casilla a, Casilla b, Encaramiento encara) {
            return 1;
        }

        #endregion

        #region Privados
        private Casilla[,] _casillas;

        #region Funciones privadas
        public Casilla colindante(Casilla actual, Encaramiento direccion ) {
            Casilla devolver= null;
            if (direccion == Encaramiento.Arriba || direccion == Encaramiento.Abajo)
            {
                if (direccion == Encaramiento.Arriba)
                {
                    devolver = _casillas[actual.posicion().fila(), actual.posicion().columna() - 1];
                }
                else {
                    devolver = _casillas[actual.posicion().fila() - 1, actual.posicion().columna()];
                }
            }
            else if (((actual.posicion().columna() - 1) % 2) == 0)//par
            {
                switch (direccion)
                {
                    case Encaramiento.SuperiorDerecha:
                        devolver = _casillas[actual.posicion().fila(), actual.posicion().columna()];
                        break;
                    case Encaramiento.InferiorDerecho:
                        devolver = _casillas[actual.posicion().fila()-1, actual.posicion().columna() ];
                        break;
                    case Encaramiento.InferiorIzquierda:
                        devolver = _casillas[actual.posicion().fila() - 1, actual.posicion().columna()-2];
                        break;
                    case Encaramiento.SuperiorIzquierda:
                        devolver = _casillas[actual.posicion().fila() - 2, actual.posicion().columna() - 2];
                        break;
                }
            }
            else  //impar
            {
                switch (direccion)
                {
                    case Encaramiento.SuperiorDerecha:
                        devolver = _casillas[actual.posicion().fila()-1, actual.posicion().columna()];
                        break;
                    case Encaramiento.InferiorDerecho:
                        devolver = _casillas[actual.posicion().fila()-2, actual.posicion().columna() ];
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
