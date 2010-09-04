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
            /*Console.WriteLine("eres ñiko y/n ");
            string str = Console.ReadLine();
            if (str == "yes" || str == "y" || str == "Y" || str == "Yes" || str == "si" || str == "s" || str == "Si" || str == "S")
            {
                _myJugador = 3;
                fase = "AtaqueArmas";
            }*/

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

            if (_mechs[_myJugador].notaEstado() >= 7.3)
            {
                _estrategia = Estrategia.Agresiva;
            }
            else {
                //Vemos si somos el jugador con la nota mas alta
                int max=-1;
                float nota = 0;
                for (int i = 0; i < _mechs.Length; i++)
                    if (_mechs[i].operativo() && nota < _mechs[i].notaEstado()) {
                        nota = _mechs[i].notaEstado();
                        max = i;
                    }

                if (max == _myJugador) {
                    _estrategia = Estrategia.Agresiva;
                }else
                    _estrategia = Estrategia.Defensiva;
            }

            Console.WriteLine("Estrategia del mech: " + _estrategia);
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
			}

            ArrayList aux = _mechs[_myJugador].armas();
            for ( int i=0 ; i<aux.Count ;i++ ){
                _mechs[_myJugador].tieneMunicion((Componente)aux[i]);
            }*/

			string c1 ;
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
                   /* if (_mechs[0].conoDerecho(p1, _mechs[0].ladoEncaramiento()))
                    {
                        Console.WriteLine("cono drcha");
                    }
                    else if (_mechs[0].conoIzquierdo(p1, _mechs[0].ladoEncaramiento()))
                        Console.WriteLine("cono izq");*/
					_tablero.casillaInfo(p1.fila(),p1.columna());
				}else
					fin=true;
			}
		}

        //Para la fase de movimiento del juego
        #region faseMovimiento
        //La nota de cada mech para saber a cual disparamos sera la siguiente: nota propia=40% y distancia=60%
        const float NOTA_MOV = 0.4f, DISTANCIA_MOV = 0.6f;

        private void faseMovimiento() 
        {
            Console.WriteLine("Fase movimiento");
            Console.WriteLine();
            Mech objetivo;
            Posicion destino;

            if (_mechs[_myJugador].operativo() && ((MechJugador)_mechs[_myJugador]).consciente())
            {
                determinarEstrategia();

                //Seleccionamos el objetivo hacia el que vamos a dirigirnos en caso de ser una estrategia ofensiva
                //if (_estrategia == Estrategia.Agresiva)
                //{
                    objetivo = eleccionObjetivo();
                /*}
                else {
                    objetivo = null;
                }*/

                //Seleccionamos la casilla destino
                destino = seleccionDestino(objetivo);
                Console.WriteLine("Destino " + destino.ToString());
                //prueba de pathfinder el 9/8 - Angel
                string str;
                do
                {
                   /* Console.WriteLine("escribe la columnafila de ");
                    str = Console.ReadLine();
                    Posicion de = new Posicion(Convert.ToInt16(str.Substring(2, 2)), Convert.ToInt16(str.Substring(0, 2)));
                    Console.WriteLine("escribe la columnafila a ");
                    str = Console.ReadLine();
                    Posicion a = new Posicion(Convert.ToInt16(str.Substring(2, 2)), Convert.ToInt16(str.Substring(0, 2)));
                    Camino Camino = new Camino(_tablero.Casilla(de), _mechs[_myJugador], _tablero.Casilla(a), _tablero);*/
                    Camino Camino = new Camino(_tablero.Casilla(_mechs[_myJugador].posicion()), _mechs[_myJugador], _tablero.Casilla(destino), _tablero);

                    Camino.print();

                    Console.ReadKey();
                } while (true);
            }
        }

        private Mech eleccionObjetivo() {
            int objetivo=0;

            if (_mechs.Length > 2)
            {
                float[] notasParciales = new float[_numeroJugadores];

                //Vemos cual la distancia maxima de los enemigos
                int max = 0;
                for (int i = 0; i < _mechs.Length; i++) {
                    if (i != _myJugador && _mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) > max)
                        max = _mechs[_myJugador].posicion().distancia(_mechs[i].posicion());
                }

                //Asignamos una nota a cada mech en funcion de la distancia que nos separa y su puntuacion
                for (int i = 0; i < _mechs.Length; i++)
                {
                    if ( i != _myJugador)
                    {
                        Console.WriteLine("Mech: " + _mechs[i].nombre());
                        //Nota estado
                        notasParciales[i] = _mechs[i].notaEstado() * NOTA_MOV;
                        Console.WriteLine("Nota estado: " + _mechs[i].notaEstado());
                        //Nota distancia
                        notasParciales[i] += ((_mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) * 10.0f) / max) * DISTANCIA_MOV;
                        Console.WriteLine("Nota distancia: " + ((_mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) * 10.0f) / max) * DISTANCIA_MOV);
                        Console.WriteLine("Nota parcial:" + notasParciales[i]);
                        Console.WriteLine();
                    }
                }

                //Nos quedamos con el mech que tenga la nota menor
                float nota = float.MaxValue ;
                for (int i = 0; i < _mechs.Length; i++ )
                    if (i != _myJugador && notasParciales[i] < nota) {
                        nota = notasParciales[i];
                        objetivo = i;
                    }
            }
            else
                objetivo = (_myJugador + 1) % 2;

            Console.WriteLine("El objetivo elegido es: " + _mechs[objetivo].nombre());
            return _mechs[objetivo];
        }

        const int Radio = 5;
        private Posicion seleccionDestino(Mech objetivo) 
        {
            List<Posicion> posiblesDestinos = new List<Posicion>();
            int[] puntuacion;
            Posicion destino = null;

            //if (_estrategia == Estrategia.Agresiva)
            //{

                //Escogemos las casillas alrededor
                _tablero.casillasEnRadio(objetivo.posicion(), posiblesDestinos,Radio);

                //Puntuamos las casillas
                puntuacion = new int[posiblesDestinos.Count];
                //Console.WriteLine("Casillas escogidas: " + posiblesDestinos.Count);
                for (int i = 0; i < posiblesDestinos.Count; i++)
                {
                    puntuacion[i]=puntuacionCasilla(posiblesDestinos[i],objetivo);
                    //Console.WriteLine(i + ": " + posiblesDestinos[i].ToString()+" punt:"+puntuacion[i]);
                }

                //Escogemos las que tienen mejores puntuaciones
                int max = 0;
                for (int i = 0; i < puntuacion.Length; i++)
                    if (puntuacion[i] > max)
                        max = puntuacion[i];

                for (int i = 0; i < puntuacion.Length; i++) {
                    if (puntuacion[i] != max) {
                        posiblesDestinos[i] = null;
                    }
                }

                Console.WriteLine("Casillas finales: " + posiblesDestinos.Count);
                for (int i = 0; i < posiblesDestinos.Count; i++)
                {
                    if ( posiblesDestinos[i]!=null)
                        Console.WriteLine(i + ": " + posiblesDestinos[i].ToString() + " punt:" + puntuacion[i]);
                }

                //De las que tienen mejores puntuaciones escogemos la mas cercana al objetivo
                int min = int.MaxValue;
                int posicion=0;
                for (int i = 0; i < posiblesDestinos.Count; i++)
                {
                    if (posiblesDestinos[i] != null && objetivo.posicion().distancia(posiblesDestinos[i]) < min)
                    {
                        posicion = i;
                        destino = posiblesDestinos[i];
                        min = objetivo.posicion().distancia(posiblesDestinos[i]);
                    }
                }

                Console.WriteLine("Destino: "+destino.ToString()+" i:"+posicion);
            //}
            //else {
            //    _tablero.casillasEnMov(_mechs[_myJugador], posiblesDestinos, _mechs[_myJugador].puntosCorrer());
            //}

            return destino;
        }
        

        private int puntuacionCasilla ( Posicion p , Mech objetivo ) 
        {
            int puntuacion = 0;

            //Estrategia agresiva
            //if (_estrategia == Estrategia.Agresiva)
            //{
                //Puntuacion por tipo terreno
                switch (_tablero.Casilla(p).tipoTerreno()) { 
                    case 0://despejado
                        puntuacion += 3;
                        break;
                    case 1://pavimentado
                        puntuacion += 3;
                        break;
                    case 2://agua
                        puntuacion += 0;
                        break;
                    case 3://pantanoso
                        puntuacion += 1;
                        break;
                    default:
                        break;
                }

                //Puntuacion por objeto en el terreno
                switch (_tablero.Casilla(p).objetoTerreno()) {
                    case 0://escombros
                        puntuacion += 0;
                        break;
                    case 1://bosque ligero
                        puntuacion += 2;
                        break;
                    case 2://bosque denso
                        puntuacion += 2;
                        break;
                    case 3://edificio ligero
                        puntuacion += 1;
                        break;
                    case 4://edificio medio
                        puntuacion += 1;
                        break;
                    case 5://edificio pesado
                        puntuacion += 1;
                        break;
                    case 6://edificio reforzado
                        puntuacion += 1;
                        break;
                    case 7://bunker
                        puntuacion += 2;
                        break;
                    default:
                        break;
                }

                //Puntuacion por nivel
                puntuacion += _tablero.Casilla(p).nivel();

                //Bonus por estar a la espalda del enemigo
                if (objetivo.conoTrasero(p, objetivo.ladoEncaramiento()))
                    puntuacion += 4;

                puntuacion -= _mechs[_myJugador].posicion().distancia(p)/2;
            //}
            /*else //Estrategia defensiva
            {
                //Puntuacion por tipo terreno
                switch (_tablero.Casilla(p).tipoTerreno())
                {
                    case 0://despejado
                        puntuacion += 1;
                        break;
                    case 1://pavimentado
                        puntuacion += 3;
                        break;
                    case 2://agua
                        puntuacion += 0;
                        break;
                    case 3://pantanoso
                        puntuacion += 1;
                        break;
                    default:
                        break;
                }

                //Puntuacion por objeto en el terreno
                switch (_tablero.Casilla(p).objetoTerreno())
                {
                    case 0://escombros
                        puntuacion += 0;
                        break;
                    case 1://bosque ligero
                        puntuacion += 4;
                        break;
                    case 2://bosque denso
                        puntuacion += 5;
                        break;
                    case 3://edificio ligero
                        puntuacion += 0;
                        break;
                    case 4://edificio medio
                        puntuacion += 0;
                        break;
                    case 5://edificio pesado
                        puntuacion += 0;
                        break;
                    case 6://edificio reforzado
                        puntuacion += 0;
                        break;
                    case 7://bunker
                        puntuacion += 0;
                        break;
                    default:
                        break;
                }

                //Puntuacion por nivel
                puntuacion -= _tablero.Casilla(p).nivel();

                //Bonus por distancia que nos separa?¿

            }*/

            return puntuacion;
        }
        #endregion

        private void faseReaccion() {
            Console.WriteLine("Fase Reaccion");
            Console.WriteLine();
        }

        #region faseAtaqueArmas
        //La nota de cada mech para saber a cual disparamos sera la siguiente: nota propia=40%, danio=30% , distancia=30%
        const float NOTA_ARMAS = 0.4f, DANIO_ARMAS = 0.3f, DISTANCIA_ARMAS = 0.3f;

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
                        !_mechs[_myJugador].conoTrasero(_mechs[i].posicion(), _mechs[_myJugador].ladoEncaramientoTorso()) && _mechs[i].operativo() )
                        objetivos.Add(_mechs[i]);

                Console.WriteLine("Al principio tenemos:");
                for (int i = 0; i < objetivos.Count; i++)
                    Console.WriteLine(i + ": " + objetivos[i].nombre());
                Console.WriteLine();

                //Dejamos solo con los que tengamos linea de vision
                List<LdV> ldv = new List<LdV>();
                objetivosLdV(objetivos, ldv);

                Console.WriteLine();
                Console.WriteLine("En LdV tenemos:" + objetivos.Count);
                for (int i = 0; i < objetivos.Count; i++)
                    Console.WriteLine(i + ": " + objetivos[i].nombre());

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

        private void objetivoMasDebil (List<Mech> objetivos , List<LdV> ldv , List<Componente> armas) 
        {
            if (objetivos.Count > 1)
            {
               
                float[] notasParciales = new float[objetivos.Count];
                List<List<Componente>> armamento = new List<List<Componente>>();
                int danio , index;
                float notaAux ;

                //Calculamos el array de notas para cada mech
                for (int i = 0; i < objetivos.Count; i++) {
                    Console.WriteLine("Mech: "+objetivos[i].nombre());
                    //Añadimos las notas
                    notasParciales[i] = objetivos[i].notaEstado() * NOTA_ARMAS;
                    Console.WriteLine("Nota mech: " + objetivos[i].notaEstado());

                    //Añadimos la nota del danio y la seleccion de armas para ese mech
                    List<Componente> arm = new List<Componente>();
                    danio = armasPermitidas(objetivos[i], arm);
                    armamento.Add(arm);
                    notaAux = 10 - (danio * 10.0f / _mechs[_myJugador].danioMaximo());
                    Console.WriteLine("Nota danio: " + notaAux);
                    notasParciales[i] += (notaAux * DANIO_ARMAS);

                    //Añadimos la nota de la distancia
                    notaAux = (_mechs[_myJugador].posicion().distancia(objetivos[i].posicion()) * 10.0f) / _mechs[_myJugador].distanciaTiroLarga();
                    notasParciales[i] += notaAux * DISTANCIA_ARMAS;
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

        private void objetivosLdV(List<Mech> objetivos , List<LdV> ldv )
        {
            LdV c;
            if (objetivos.Count > 0)
            {
                for (int i = 0; i < objetivos.Count; i++)
                {
                    c = new LdV(_mechs[_myJugador], objetivos[i], _tablero);
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

            //Vemos las armas que podria disparar
            ArrayList armas = _mechs[_myJugador].armas();
            String localizacion;
            for (int i = 0; i < armas.Count; i++) 
            {
                localizacion = ((Componente)armas[i]).localizacionSTRING();
                if (_mechs[_myJugador].tieneMunicion((Componente)armas[i]) && ((Componente)armas[i]).operativo() && ((Componente)armas[i]).distanciaLarga() >= distancia &&
                    ((Componente)armas[i]).distanciaMinima() < distancia && 
                   ( ((localizacion == "BI" || localizacion == "PI") && (situacion == "IZQ" || situacion == "DNTE"))
                   || ((localizacion == "PD"|| localizacion == "BD") && (situacion == "DRCHA" || situacion == "DNTE"))
                   || ((localizacion != "TIa" || localizacion != "TDa" || localizacion != "TCa") && situacion == "DNTE" )
                    ) )
                {
                    danio += ((Componente)armas[i]).danio();
                    seleccionArmas.Add((Componente)armas[i]);
                }
            }
            
            return danio;
        }

        //Constantes para establecer el limite de calor
        const int calorOfensivo = 16, calorDefensivo = 9;//calorOfensivo = 21, calorDefensivo = 14;

        private void seleccionArmasDisparar(List<Mech> objetivos, List<Componente> seleccionArmas) 
        {

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
                if (_estrategia == Estrategia.Agresiva)
                {
                    limiteCalor = calorOfensivo + _mechs[_myJugador].numeroRadiadores() - _mechs[_myJugador].nivelTemp() - calorMovimiento;
                }else
                    limiteCalor = calorDefensivo + _mechs[_myJugador].numeroRadiadores() - _mechs[_myJugador].nivelTemp() - calorMovimiento;
                //Console.WriteLine("Limite calor= " + limiteCalor + " (calorOfensivo:" + calorOfensivo + " numero radiadores:" + _mechs[_myJugador].numeroRadiadores() + " temp:" + _mechs[_myJugador].nivelTemp() + " mov:"+calorMovimiento+" )");
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
                int calor=0 , itr=0 ;
                Boolean salir = false;
                while (!salir) {
                    if ( itr<seleccionArmas.Count && calor + seleccionArmas[orden[itr]].calor() < limiteCalor 
                        && _mechs[_myJugador].tiradaImpacto(seleccionArmas[orden[itr]],objetivos[0],_tablero,_config.movimiento(_myJugador),_config.movimiento(objetivos[0].numeroJ())) <=9 )
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
            StreamWriter f = new StreamWriter("accionJ" + _myJugador.ToString() + ".sbt", false);

            f.WriteLine("False");//coger garrote
            if ( objetivo.Count>0) {
                f.WriteLine(objetivo[0].posicion().ToString()); //Posicion del objetivo primario
                f.WriteLine(seleccionArmas.Count); //Numero de armas a disparar
                for (int i = 0; i < seleccionArmas.Count; i++) 
                {
                    //Localizacion arma
                    f.WriteLine(seleccionArmas[i].localizacionSTRING());
                    //Slot arma
                    f.WriteLine(_mechs[_myJugador].slotArma(seleccionArmas[i]));
                    //Doble cadencia (booleano)
                    f.WriteLine("False");//f.WriteLine("Doble cadencia ");
                    //Localizacion municion
                    f.WriteLine(_mechs[_myJugador].localizacionMunicion(seleccionArmas[i]));
                    //Slot municion
                    f.WriteLine(_mechs[_myJugador].slotMunicion(seleccionArmas[i]));
                    //Hexagono objtivo
                    f.WriteLine(objetivo[0].posicion().ToString());
                    //Tipo objetivo
                    f.WriteLine("Mech");
                }
            }else {
                f.WriteLine("0000");
                f.WriteLine(0);
            }
            

            f.Close();
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
