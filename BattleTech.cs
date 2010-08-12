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

//<<<<<<< ANTES DE QUE TE DE LA HISTERIA, E COPIADO ESTE TROZO DE CODIGO DENTRO DE LA FUNCION DE ESTA CLASE QUE SE LLAMA, faseMovimiento(). HAZ LAS PRUEBAS DESDE AHI
//<<<<<<< .mine 
            //prueba de pathfinder el 9/8 - Angel
            //string str;

            //Console.WriteLine("estcibe la columnafila de \n");
          //  str = Console.ReadLine();
        //    Posicion de = new Posicion(Convert.ToInt16(str.Substring(2, 2)), Convert.ToInt16(str.Substring(0, 2)));
      //      Console.WriteLine("estcibe la columnafila a \n");
    //        str = Console.ReadLine();
  //          Posicion a = new Posicion(Convert.ToInt16(str.Substring(2, 2)), Convert.ToInt16(str.Substring(0, 2)));
  //          Camino Camino= new Camino(_tablero.Casilla(de),_tablero.Casilla(a),_tablero);


//=======
            //PRuebas de �IKO
            pruebas(); 

            //Elegimos la accion a realizar
            if (fase == "Movimiento")
            {
                faseMovimiento();
            }
            else if (fase=="AtaqueArmas")
                faseAtaqueArmas();

//>>>>>>> .r116
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
			
			/*string c1 ;
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
			}*/
		}

        //Para la fase de movimiento del juego
        #region faseMovimiento
        private void faseMovimiento() 
        {
            Console.WriteLine("Fase movimiento");
            Console.WriteLine();
            //<<<<<<< .mine
            //prueba de pathfinder el 9/8 - Angel
            string str;

            Console.WriteLine("estcibe la columnafila de \n");
            str = Console.ReadLine();
            Posicion de = new Posicion(Convert.ToInt16(str.Substring(2, 2)), Convert.ToInt16(str.Substring(0, 2)));
            Console.WriteLine("estcibe la columnafila a \n");
            str = Console.ReadLine();
            Posicion a = new Posicion(Convert.ToInt16(str.Substring(2, 2)), Convert.ToInt16(str.Substring(0, 2)));
            Camino Camino = new Camino(_tablero.Casilla(de), _tablero.Casilla(a), _tablero);


            //=======
        }
        #endregion

        private void faseReaccion() { 
        
        }

        #region faseAtaqueArmas
        private void faseAtaqueArmas() {
            Console.WriteLine("Fase Ataque con Armas");
            Console.WriteLine();

            
            /*
             * 1� Eleccion de rivales dentro de cono vision
             * 2� Ver si hay linea de vision con ellos
             * 3� De los restantes escoger al mas debil
             * 4� Ver las armas a dispararle
             * 5� Escribir el fichero
             */
            ArrayList objetivos = new ArrayList();
            for (int i = 0; i < _mechs.Length; i++)
                if (i != _myJugador)
                    objetivos.Add(_mechs[i]);

            Console.WriteLine("Al principio tenemos:");
            for (int i = 0; i < objetivos.Count; i++)
                Console.WriteLine(i + ": " + ((Mech)objetivos[i]).nombre());
            Console.WriteLine();

            //Dejamos solo con los que tengamos linea de vision
            objetivosLdV(objetivos);

            Console.WriteLine("En LdV tenemos:");
            for (int i = 0; i < objetivos.Count; i++)
                Console.WriteLine(i + ": " + ((Mech)objetivos[i]).nombre());
            Console.WriteLine();


            //Dejamos solo los que esten en el cono de vision
            objetivosConoVision(objetivos);

            Console.WriteLine("En Cono vision tenemos:");
            for (int i = 0; i < objetivos.Count; i++)
                Console.WriteLine(i + ": " + ((Mech)objetivos[i]).nombre());
            Console.WriteLine();
        }

        private void objetivosConoVision (ArrayList objetivos) 
        {
            for (int i = 0; i < objetivos.Count; i++)
                if (!_mechs[_myJugador].conoVision(((Mech)objetivos[i]).posicion(), ((Mech)objetivos[i]).ladoEncaramientoTorso()))
                {
                    objetivos.RemoveAt(i);
                    i--;
                }
        }

        private void objetivosLdV(ArrayList objetivos)
        {
            Camino c;

            for (int i = 0; i < objetivos.Count; i++) {
                c = new Camino(_mechs[_myJugador].posicion(), ((Mech)objetivos[i]).posicion(),_tablero,_myJugador);
                if (c.)
                {
                    objetivos.RemoveAt(i);
                    i--;
                }
            }
        }

        #endregion

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
        private Estrategia _estrategia;
#endregion

    }

}
