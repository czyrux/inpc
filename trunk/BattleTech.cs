using System;
using System.Collections;
using System.Collections.Generic;
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

            //PRuebas de ÑIKO
            Console.WriteLine("eres ñiko y/n ");
            string str = Console.ReadLine();
            if (str == "yes" || str == "y" || str == "Y" || str == "Yes" || str == "si" || str == "s" || str == "Si" || str == "S")
            {
                _myJugador = 3;
                fase = "AtaqueArmas";
            }

			//Leemos los mech
            readMechs();

			//Leemos el tablero de juego
            _tablero = new Tablero(path+"mapaJ"+_myJugador.ToString()+".sbt");
			
			//Leemos el fichero de configuracion
			_config = new ConfiguracionJuego( _myJugador );


            //pruebas();
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
			/*Console.WriteLine("Numero de jugadores: " + _numeroJugadores);
			Console.WriteLine();

			for (int i = 0; i < _mechs.Length; i++) {
                if (i == _myJugador)
                    ((MechJugador)_mechs[i]).datos();
				else
                    ((Mech)_mechs[i]).datos();
			}*/

            ArrayList aux = _mechs[_myJugador].armas();
            for ( int i=0 ; i<aux.Count ;i++ ){
                _mechs[_myJugador].tieneMunicion((Componente)aux[i]);
            }

			/*string c1 ;
			Posicion p1 ;
			Boolean fin=false;
			
			while (!fin) {
                Console.WriteLine();
				Console.WriteLine("Elija la casilla que desea averiguar la distancia a la casilla del mech");
				c1=Console.ReadLine();
				if (c1!="q"){
					p1 = new Posicion(c1);
                    //Casilla aux = _tablero.Casilla(p1);
                    Console.WriteLine("La distancia es:"+_mechs[0].posicion().distancia(p1));
                    if (_mechs[0].conoDerecho(p1, _mechs[0].ladoEncaramiento()))
                    {
                        Console.WriteLine("cono drcha");
                    }
                    else if (_mechs[0].conoIzquierdo(p1, _mechs[0].ladoEncaramiento()))
                        Console.WriteLine("cono izq");
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
            //prueba de pathfinder el 9/8 - Angel
            string str;

            Console.WriteLine("escribe la columnafila de ");
            str = Console.ReadLine();
            Posicion de = new Posicion(Convert.ToInt16(str.Substring(2, 2)), Convert.ToInt16(str.Substring(0, 2)));
            Console.WriteLine("escribe la columnafila a ");
            str = Console.ReadLine();
            Posicion a = new Posicion(Convert.ToInt16(str.Substring(2, 2)), Convert.ToInt16(str.Substring(0, 2)));
            Camino Camino = new Camino(_tablero.Casilla(de), _tablero.Casilla(a), _tablero);

            Camino.print();

            Console.ReadKey();
        }
        #endregion

        private void faseReaccion() { 
        
        }

        #region faseAtaqueArmas
        private void faseAtaqueArmas() {
            /*
             * 1º Eleccion de rivales dentro de radio accion (rango alcance: distancia tiro larga media) y que no esten en el cono trasero
             *    Si no hay ninguno metemos los que esten a distancia de tiro de alcance maximo?¿
             * 2º Ver si hay linea de vision con ellos
             * 3º a. De los restantes escoger si hay algunos en un radio de 3 casillas, y de ellos al que menos puntuacion tenga
             *    b. En caso opuesto escoger al mas debil (nota mas baja)
             * 4º Ver las armas a dispararle
             * 5º Escribir el fichero
             */
            Console.WriteLine("Fase Ataque con Armas");
            Console.WriteLine();

            //Console.WriteLine("Alcance de tiro maximo: " + _mechs[_myJugador].maxAlcanceTiro());
            //Console.WriteLine("Alcance de tiro largo medio: " + _mechs[_myJugador].distanciaTiroLarga());

            List<Mech> objetivos = new List<Mech>();
            for (int i = 0; i < _mechs.Length; i++)
                //Si estan dentro del alcance de tiro largo y no estan en la espalda
                if (i != _myJugador && _mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) < _mechs[_myJugador].distanciaTiroLarga() &&
                    !_mechs[_myJugador].conoTrasero(_mechs[i].posicion(),_mechs[_myJugador].ladoEncaramientoTorso()) )
                    objetivos.Add(_mechs[i]);
 
            Console.WriteLine("Al principio tenemos:");
            for (int i = 0; i < objetivos.Count; i++)
                Console.WriteLine(i + ": " + objetivos[i].nombre());
            Console.WriteLine();

            //Dejamos solo con los que tengamos linea de vision
            List<Camino> ldv = new List<Camino>();
            objetivosLdV(objetivos,ldv);

            Console.WriteLine();
            Console.WriteLine("En LdV tenemos:" + objetivos.Count);
            for (int i = 0; i < objetivos.Count; i++)
            {
                Console.WriteLine(i + ": " + objetivos[i].nombre());
                Console.WriteLine("Distancia: " + _mechs[_myJugador].posicion().distancia(objetivos[i].posicion()));
                Console.WriteLine("Nota: " + objetivos[i].nota());
                Console.WriteLine();
            }

            //Escogemos si hay alguno a una distancia de 3 o menos casillas nuestro
            /*List<Mech> objetivosCerca = new List<Mech>();
            List<Camino> ldvAux = new List<Camino>();
            for (int i = 0; i < objetivos.Count; i++)
            {
                if (_mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) <= 3)
                {
                    objetivosCerca.Add(_mechs[i]);
                    ldvAux.Add(ldv[i]);
                }
            }


            if (objetivosCerca.Count > 0)
            {
                Console.WriteLine("Hay alguno en el rango cercano");
                ldv.Clear();
                objetivos.Clear();
                ldv = ldvAux;
                objetivos = objetivosCerca;

            }
            else {
                Console.WriteLine("No hay ninguno en rango");
            }*/

            //Escogemos al mas debil
            List<Componente> armasADisparar = new List<Componente>();
            objetivoMasDebil(objetivos, ldv, armasADisparar);

            Console.WriteLine("El objetivo es:"+objetivos.Count);
            for (int i = 0; i < objetivos.Count; i++)
                Console.WriteLine(i + ": " + objetivos[i].nombre());
            Console.WriteLine();



            //Vemos las armas a dispararle
            /*if (objetivos.Count>0)
            {
                seleccionArmas(objetivos[0], armasDisparo);
            }*/

            //Escribimos las ordenes

            Console.ReadLine();
        }

        private void objetivoMasDebil (List<Mech> objetivos , List<Camino> ldv , List<Componente> armas) 
        {
            if (objetivos.Count > 1)
            {
                //La nota de cada mech para saber a cual disparamos sera la siguiente: nota propia=40%, danio=30% , distancia=30%
                float NOTA = 0.4f, DANIO = 0.3f, DISTANCIA = 0.3f;
                float[] notasParciales = new float[objetivos.Count];
                List<List<Componente>> armamento = new List<List<Componente>>();
                int danio;
                float notaDanio;

                //Calculamos el array de notas para cada mech
                for (int i = 0; i < objetivos.Count; i++) {
                    Console.WriteLine("Mech: "+objetivos[i].nombre());
                    //Añadimos las notas
                    notasParciales[i] = objetivos[i].nota() * NOTA;
                    Console.WriteLine("Nota mech: " + objetivos[i].nota());

                    //Añadimos la nota del danio y la seleccion de armas para ese mech
                    List<Componente> arm = new List<Componente>();
                    danio = seleccionArmas(objetivos[i], arm);
                    armamento.Add(arm);
                    notaDanio = 10 -(danio * 10.0f / _mechs[_myJugador].danioMaximo());
                    Console.WriteLine("danio: " + danio);
                    Console.WriteLine("danio total: " + _mechs[_myJugador].danioMaximo());
                    Console.WriteLine("Nota danio: " + notaDanio);
                    notasParciales[i] += ( notaDanio * DANIO);

                    //Añadimos la nota de la distancia

                    Console.WriteLine("Nota parcial: " + notasParciales[i]);
                    Console.WriteLine();
                }

                
                /*float valor = int.MaxValue;
                List<Mech> objetivosDebil = new List<Mech>();
                List<Camino> ldvAux = new List<Camino>(); ;

                for (int i = 0; i < objetivos.Count; i++)
                    if (objetivos[i].nota() < valor)
                    {
                        objetivosDebil.Insert(0, objetivos[i]);
                        ldvAux.Insert(0, ldv[i]);
                    }
                objetivos.Clear();
                ldv.Clear();
                objetivos.Add(objetivosDebil[0]);
                ldv.Add(ldvAux[0]);*/
            }
            else if (objetivos.Count == 1)
            {
                seleccionArmas(objetivos[0], armas);
            }
            else
                armas = null;
        }

        private void objetivosLdV(List<Mech> objetivos , List<Camino> ldv )
        {
            Camino c;
            if (objetivos.Count > 0)
            {
                for (int i = 0; i < objetivos.Count; i++)
                {
                    c = new Camino(_mechs[_myJugador], objetivos[i], _tablero);
                    if (!c.ldv())
                    {
                        objetivos.RemoveAt(i);
                        i--;
                    }
                    else
                        ldv.Add(c);
                }
            }
        }

        private int seleccionArmas( Mech objetivo, List<Componente> seleccionArmas) 
        {        
            string situacion;
            int encTorso = _mechs[_myJugador].ladoEncaramientoTorso();
            int distancia = _mechs[_myJugador].posicion().distancia(objetivo.posicion());
            int danio = 0;

            //Vemos la localizacion del objetivo respecto a nuestro mech
            if (_mechs[_myJugador].conoDerecho(objetivo.posicion(), encTorso)) {
                situacion = "DRCHA";
            }
            else if (_mechs[_myJugador].conoIzquierdo(objetivo.posicion(), encTorso)) {
                situacion = "IZQ";
            }
            else
                situacion = "DNTE";

            //(0=BI,1=TI,2=PI,3=PD,4=TD,5=BD,6=TC,7=CAB,8=TIa,9=TDa,10=TCa) 
            //Vemos las armas que podria disparar
            ArrayList armas = _mechs[_myJugador].armas();
            int localizacion;
            for (int i = 0; i < armas.Count; i++) 
            {
                //Console.WriteLine("Arma: " + ((Componente)armas[i]).nombre());
                //Console.WriteLine("Operativo: " + ((Componente)armas[i]).operativo());
                //Console.WriteLine("Municion: " + _mechs[_myJugador].tieneMunicion((Componente)armas[i]));
                //Console.WriteLine("Distancia arma: " + ((Componente)armas[i]).distanciaLarga());
                //Console.WriteLine("Distancia: " + distancia);
                localizacion = ((Componente)armas[i]).localizacion();
                //Console.WriteLine("Localizacion:" + localizacion);
                if (_mechs[_myJugador].tieneMunicion((Componente)armas[i]) && ((Componente)armas[i]).operativo() && ((Componente)armas[i]).distanciaLarga() >= distancia &&
                    ((Componente)armas[i]).distanciaMinima() < distancia && 
                   ( ((localizacion==0 || localizacion==1 || localizacion==2) && (situacion=="IZQ" || situacion=="DNTE"))
                   || ((localizacion==3 || localizacion==4 || localizacion==5) && (situacion=="DRCHA" || situacion=="DNTE"))
                   || ((localizacion != 8 || localizacion != 9 || localizacion != 10) && situacion == "DNTE" )
                    ) )
                {
                    Console.WriteLine("Añadida: " + ((Componente)armas[i]).nombre() + " " + ((Componente)armas[i]).danio());
                    danio += ((Componente)armas[i]).danio();
                    seleccionArmas.Add((Componente)armas[i]);
                }
                //Console.WriteLine();
            }
            

            /*Console.WriteLine("Las armas que podrian dispararse son:"+seleccionArmas.Count);
            for (int i = 0; i < seleccionArmas.Count; i++) {
                Console.WriteLine(i + ": " + seleccionArmas[i].nombre() + " localizacion: " + seleccionArmas[i].localizacion());
            }*/
            Console.WriteLine(danio);
            return danio;
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
