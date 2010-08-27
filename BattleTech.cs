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
            else if (fase == "Reaccion")
            {
                faseReaccion();
            }
            else if (fase == "AtaqueArmas")
            {
                faseAtaqueArmas();
            }
            else if (fase == "AtaqueFisico")
            {
                faseAtaquesFisico();
            }
            else
                faseFinalTurno();

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
        private void determinarEstrategia() {
            Console.WriteLine(_mechs[_myJugador].notaEstado());
            if (_mechs[_myJugador].notaEstado() >= 7.3)
            {
                _estrategia = Estrategia.Ofensiva;
            }
            else
                _estrategia = Estrategia.Defensiva;
        }

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
            Console.WriteLine("Fase Reaccion");
            Console.WriteLine();
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

            if (_mechs[_myJugador].operativo() && ((MechJugador)_mechs[_myJugador]).consciente())
            {
                determinarEstrategia();

                List<Mech> objetivos = new List<Mech>();
                //Escogemos a los mech si estan dentro del alcance de tiro largo y no estan en nuestra espalda
                for (int i = 0; i < _mechs.Length; i++)
                    if (i != _myJugador && _mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) < _mechs[_myJugador].distanciaTiroLarga() &&
                        !_mechs[_myJugador].conoTrasero(_mechs[i].posicion(), _mechs[_myJugador].ladoEncaramientoTorso()))
                        objetivos.Add(_mechs[i]);

                Console.WriteLine("Al principio tenemos:");
                for (int i = 0; i < objetivos.Count; i++)
                    Console.WriteLine(i + ": " + objetivos[i].nombre());
                Console.WriteLine();

                //Dejamos solo con los que tengamos linea de vision
                List<Camino> ldv = new List<Camino>();
                objetivosLdV(objetivos, ldv);

                Console.WriteLine();
                Console.WriteLine("En LdV tenemos:" + objetivos.Count);
                for (int i = 0; i < objetivos.Count; i++)
                {
                    Console.WriteLine(i + ": " + objetivos[i].nombre());
                    //Console.WriteLine("Distancia: " + _mechs[_myJugador].posicion().distancia(objetivos[i].posicion()));
                    //Console.WriteLine("Nota: " + objetivos[i].notaEstado());
                }

                //Escogemos al mas debil
                List<Componente> armasADisparar = new List<Componente>();
                Console.WriteLine();
                objetivoMasDebil(objetivos, ldv, armasADisparar);

                Console.WriteLine();
                Console.WriteLine("El objetivo es:" + armasADisparar.Count );
                for (int i = 0; i < objetivos.Count; i++)
                    Console.WriteLine(i + ": " + objetivos[i].nombre());
                Console.WriteLine();


                //Vemos las armas a dispararle
                seleccionArmasDisparar(objetivos, armasADisparar);

                for (int i = 0; i < armasADisparar.Count; i++)
                    Console.WriteLine(armasADisparar[i].nombre());

                //Escribimos las ordenes
                escribirOrdenesArmas(objetivos, armasADisparar);

                Console.ReadLine();
            }
        }

        private void objetivoMasDebil (List<Mech> objetivos , List<Camino> ldv , List<Componente> armas) 
        {
            if (objetivos.Count > 1)
            {
                //La nota de cada mech para saber a cual disparamos sera la siguiente: nota propia=40%, danio=30% , distancia=30%
                float NOTA = 0.4f, DANIO = 0.3f, DISTANCIA = 0.3f;
                float[] notasParciales = new float[objetivos.Count];
                List<List<Componente>> armamento = new List<List<Componente>>();
                int danio , index;
                float notaAux ;

                //Calculamos el array de notas para cada mech
                for (int i = 0; i < objetivos.Count; i++) {
                    Console.WriteLine("Mech: "+objetivos[i].nombre());
                    //Añadimos las notas
                    notasParciales[i] = objetivos[i].notaEstado() * NOTA;
                    Console.WriteLine("Nota mech: " + objetivos[i].notaEstado());

                    //Añadimos la nota del danio y la seleccion de armas para ese mech
                    List<Componente> arm = new List<Componente>();
                    danio = armasPermitidas(objetivos[i], arm);
                    armamento.Add(arm);
                    notaAux = 10 - (danio * 10.0f / _mechs[_myJugador].danioMaximo());
                    Console.WriteLine("Nota danio: " + notaAux);
                    notasParciales[i] += (notaAux * DANIO);

                    //Añadimos la nota de la distancia
                    notaAux = (_mechs[_myJugador].posicion().distancia(objetivos[i].posicion()) * 10.0f) / _mechs[_myJugador].distanciaTiroLarga();
                    notasParciales[i] += notaAux * DISTANCIA;
                    Console.WriteLine("Nota distancia: " + notaAux);

                    //Si estamos a su espalda, tenemos un bonus
                    if (objetivos[i].conoTrasero(_mechs[_myJugador].posicion(), objetivos[i].ladoEncaramiento()))
                        notasParciales[i] -= 1;

                    //Si esta atascado o en el suelo (y nos encontramos en una casilla colindante), tenemos un bonus
                    if (objetivos[i].atascado() || (objetivos[i].enSuelo() && _mechs[_myJugador].posicion().distancia(objetivos[i].posicion()) == 1))
                        notasParciales[i] -= 0.5f;

                    Console.WriteLine("Nota parcial: " + notasParciales[i]);
                    Console.WriteLine();
                }

                //Escogemos como objetivo le que tenga una nota menor
                notaAux = notasParciales[0];
                index = 0 ;
                for (int i = 1; i < notasParciales.Length; i++) {
                    if (notasParciales[i] < notaAux) {
                        index = i;
                        notaAux = notasParciales[i];
                    }
                }

                //Copiamos las armas
                for (int i=0 ; i<armamento[index].Count ;i++)
                    armas.Add(armamento[index][i]);

                //Borramos el resto de objetivos
                for (int i = 0; i < objetivos.Count; i++)
                    if (i != index) {
                        objetivos.RemoveAt(i);
                        ldv.RemoveAt(i);
                        index--;
                        i--;
                    }
            }
            else if (objetivos.Count == 1)
            {
                armasPermitidas(objetivos[0], armas);
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

        private int armasPermitidas( Mech objetivo, List<Componente> seleccionArmas) 
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
                    danio += ((Componente)armas[i]).danio();
                    seleccionArmas.Add((Componente)armas[i]);
                }
                //Console.WriteLine();
            }
            

            /*Console.WriteLine("Las armas que podrian dispararse son:"+seleccionArmas.Count);
            for (int i = 0; i < seleccionArmas.Count; i++) {
                Console.WriteLine(i + ": " + seleccionArmas[i].nombre() + " localizacion: " + seleccionArmas[i].localizacion());
            }*/
            return danio;
        }

        private void seleccionArmasDisparar(List<Mech> objetivos, List<Componente> seleccionArmas) 
        {
            int calorOfensivo = 16, calorDefensivo = 9;//calorOfensivo = 21, calorDefensivo = 14;
            int calorMovimiento;
            int limiteCalor; 

            if (objetivos.Count > 0)
            {
                //Vemos el calor por el movimiento consumido
                if (_config.movimiento(_myJugador) == "Inmovil") {
                    calorMovimiento = 0;
                }
                else if (_config.movimiento(_myJugador) == "Andar") {
                    calorMovimiento = 1;
                }
                else if (_config.movimiento(_myJugador) == "Correr") {
                    calorMovimiento = 2;
                }
                else {
                    calorMovimiento = 3;
                }

                //Establecemos le limite hasta el que podemos llegar
                if (_estrategia == Estrategia.Ofensiva)
                {
                    limiteCalor = calorOfensivo + _mechs[_myJugador].numeroRadiadores() - _mechs[_myJugador].nivelTemp() - calorMovimiento;
                }else
                    limiteCalor = calorDefensivo + _mechs[_myJugador].numeroRadiadores() - _mechs[_myJugador].nivelTemp() - calorMovimiento;

                //Calculamos la relacion de las armas daño/calor
                float[] potencia = new float[seleccionArmas.Count];
                int[] orden = new int[seleccionArmas.Count];
                for (int i = 0; i < seleccionArmas.Count; i++)
                {
                    orden[i] = i;
                    if (seleccionArmas[i].calor() == 0)
                    {
                        potencia[i] = seleccionArmas[i].danio();
                    }
                    else
                        potencia[i] = seleccionArmas[i].danio() * 1.0f / seleccionArmas[i].calor();
                }

                //Ordenamos las armas de mayor a menor (metodo Burbuja O(n^2))
                float tmp2;
                int tmp1;
                for (int i = 1; i < potencia.Length; i++ ){
                    for (int j = potencia.Length-1; j >= i; j--) {
                        if (potencia[j] > potencia[j-1]) {
                            tmp2 = potencia[j-1];
                            potencia[j-1] = potencia[j];
                            potencia[j] = tmp2;

                            tmp1 = orden[j-1];
                            orden[j-1] = orden[j];
                            orden[j] = tmp1;
                        }
                    } 
                }

                //Seleccionamos las armas mientras no se pasen del limite de calor
                List<Componente> conjuntoFinal = new List<Componente>();
                int calor=0 , itr=0;
                Boolean salir = false;
                while (!salir) {
                    if ( itr<seleccionArmas.Count && calor + seleccionArmas[orden[itr]].calor() < limiteCalor )
                    {
                        calor += seleccionArmas[orden[itr]].calor();
                        conjuntoFinal.Add(seleccionArmas[orden[itr]]);
                        itr++;
                    }
                    else
                        salir = true;
                }

                //Dejamos las armas seleccionadas en la seleccion de armas
                seleccionArmas.Clear();
                for (int i = 0; i < conjuntoFinal.Count; i++)
                    seleccionArmas.Add(conjuntoFinal[i]);
            }
            else
                seleccionArmas = null;
        }

        private void escribirOrdenesArmas(List<Mech> objetivo, List<Componente> seleccionArmas) { 
        
        }

        #endregion

        private void faseAtaquesFisico() {
            Console.WriteLine("Fase Ataque Fisico");
            Console.WriteLine();
        }

        private void faseFinalTurno() {
            Console.WriteLine("Fase Final de Turno");
            Console.WriteLine();
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
