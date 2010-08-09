using System;
using System.Collections;
using System.Text;
using System.IO;

namespace ico
{
    class BattleTech
    {

        #region constructores
        public BattleTech(int Jugador, String  fase) {
            string path = "C:/ficheros/";
            _myJugador = Jugador;
            _faseJuego = fase;
          
			//Leemos los mech
            readMechs();

			//Leemos el tablero de juego
            _tablero = new Tablero(path+"mapaJ"+_myJugador.ToString()+".sbt");
			
			//Leemos el fichero de configuracion
			_config = new ConfiguracionJuego( _myJugador );


            //Elegimos la accion a realizar
            if (fase == "Movimiento")
            {
                faseMovimiento();
            }
            else
                faseReaccion();

        }
        #endregion

        #region lectura parametros juego
        private void readMechs () {

			StreamReader f1, f2;
			Mech m;
			//variable path
            string path = "C:/ficheros/";
			
			f1 = new StreamReader(path+"mechsJ"+_myJugador.ToString()+".sbt");
			f1.ReadLine();//nombre magico fichero
			
			//Leemos el numero de jugadores
			_numeroJugadores=Convert.ToInt32(f1.ReadLine());
            _mechs = new Mech[_numeroJugadores];

			//Leemos los datos de los jugadores
            for (int i = 0; i < _numeroJugadores; i++) {
                f2 = new StreamReader(path+"defmechJ" + _myJugador.ToString() + "-" + Convert.ToString(i) + ".sbt");//fichero definicion				
                if (i == _myJugador) {
                    m = new MechJugador(f1, f2, _numeroJugadores);	
				} else {
                    m = new Mech(f1, f2, _numeroJugadores);
				}
				_mechs[i]=m;
				f2.Close();
			}
			//Cerramos el fichero
			f1.Close();

        }
		
        #endregion

        #region metodos
        public void pruebas () 
        {
			Console.WriteLine("Numero de jugadores: " + _numeroJugadores);
			Console.WriteLine();

			for (int i = 0; i < _mechs.Length; i++) {
                if (i == _myJugador)
                    ((MechJugador)_mechs[i]).datos();
				else
                    ((Mech)_mechs[i]).datos();
			}
			
			string c1 ;
			Posicion p1 ;
			Camino c;
			Boolean fin=false;
			
			while (!fin) {
				Console.WriteLine("Elija la casilla que desea averiguar si nuestro mech la ve");
				c1=Console.ReadLine();
				if (c1!="q"){
					p1 = new Posicion(c1);
                    Casilla aux = _tablero.Casilla(p1);
                    if (_mechs[0].conoVision(aux))
                    {
                        Console.WriteLine("Esta dentro");
                    }else
                        Console.WriteLine("No esta dentro");
					//_tablero.casillaInfo(p1.fila(),p1.columna());
				}else
					fin=true;
			}
		}

        //Para la fase de movimiento del juego
        #region faseMovimiento
        private void faseMovimiento() 
        {
        
        }
        #endregion

        private void faseReaccion() { 
        
        }

        private void faseAtaqueArmas() { 
        
        }

        private void faseAtaquesFisico() { 
        
        }

        private void faseFinalTurno() { 
        
        }

        #endregion

        #region atributos
        private int _myJugador;
        private String _faseJuego;
        private int _numeroJugadores;
        private Tablero _tablero;
        private Mech[] _mechs;
		private ConfiguracionJuego _config;
#endregion

    }

}
