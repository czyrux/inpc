using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ico
{
    /// <summary>
    /// Clase principal del programa. Sera la encargada de actuar segun las distintas fase  del juego.
    /// </summary>
    class BattleTech
    {

        #region constructores
        /// <summary>
        /// Constructor con parametros. Se encargara de leer todos los datos necesarios para la fase de juego, asi
        /// como de llamar a los correspodientes metodos que solventara dicha fase.
        /// </summary>
        /// <param name="Jugador">Numero del jugador</param>
        /// <param name="fase">Fase del juego en que nos encontramos</param>
        public BattleTech(int Jugador, String  fase) {

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
            _mechs[_myJugador].posicion();
			//Leemos el tablero de juego
            _tablero = new Tablero(PanelControl.Path+"mapaJ"+_myJugador.ToString()+".sbt");
			
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
        /// <summary>
        /// Metodo usado para leer los parametros de los mechs
        /// </summary>
        private void readMechs () {

			StreamReader f1, f2;
			Mech m;
			
			f1 = new StreamReader(PanelControl.Path+"mechsJ"+_myJugador.ToString()+".sbt");
			f1.ReadLine();//nombre magico fichero
			
			//Leemos el numero de jugadores
			_numeroJugadores=Convert.ToInt32(f1.ReadLine());
            _mechs = new Mech[_numeroJugadores];

			//Leemos los datos de los jugadores
            for (int i = 0; i < _numeroJugadores; i++) {
                f2 = new StreamReader(PanelControl.Path+"defmechJ" + _myJugador.ToString() + "-" + Convert.ToString(i) + ".sbt");//fichero definicion				
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

        /// <summary>
        /// Determina si la estrategia del mech jugador ha de ser ofensiva o defensiva
        /// </summary>
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

            Console.WriteLine("Mech: "+ _mechs[_myJugador].nombre()+" nota:"+_mechs[_myJugador].notaEstado()+ " estrategia del mech: " + _estrategia);
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


        #region faseMovimiento

        /// <summary>
        /// /funcion que engloba las acciones de la fase de movimiento
        /// </summary>
        private void faseMovimiento() 
        {
            Console.WriteLine("Fase movimiento");
            Console.WriteLine();
            Mech objetivo;
            Posicion[] destinos = new Posicion[PanelControl.numeroDestinos];
            Camino[] posiblesCaminos = new Camino[PanelControl.numeroDestinos];

            if (_mechs[_myJugador].operativo() && ((MechJugador)_mechs[_myJugador]).consciente())
            {
                //Determinamos la estrategia con la que jugaremos
                determinarEstrategia();
  
                //Seleccionamos el objetivo hacia el que vamos a dirigirnos en caso de ser una estrategia ofensiva
                objetivo = eleccionObjetivo();
                if(objetivo != null )Console.WriteLine("El objetivo elegido es: " + objetivo.nombre());

                //Seleccionamos la casilla destino
                seleccionDestino(objetivo,destinos);

                //Evaluamos la ultima posicion de cada camino y nos quedamos con el mayor
                int[] puntuacionCamino = new int[PanelControl.numeroDestinos];
                int index = 0, valor = int.MinValue;
                for (int i = 0; i < destinos.Length; i++) {
                    posiblesCaminos[i] = new Camino(_myJugador, _tablero.Casilla(destinos[i]), _tablero, _estrategia,objetivo.numeroJ(),_mechs);
                    Console.WriteLine(i + ": " + destinos[i].ToString());
                    //posiblesCaminos[i].print();
                    Console.WriteLine(posiblesCaminos[i].ToString());
                    Console.WriteLine("Destino " + i + ": " + posiblesCaminos[i].casillaFinal().posicion().ToString());
                    puntuacionCamino[i] = puntuacionCasilla(posiblesCaminos[i].casillaFinal().posicion(), objetivo);
                    Console.WriteLine("Puntuacion: " + puntuacionCamino[i]);
                    if (puntuacionCamino[i] > valor && (casillaEnLdV(posiblesCaminos[i].casillaFinal().posicion(),objetivo) || i==destinos.Length-1 ) ) {
                        index = i;
                        valor = puntuacionCamino[i];
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Elegimos " + posiblesCaminos[index].casillaFinal().posicion().ToString());

                posiblesCaminos[index].print();
                Console.WriteLine(posiblesCaminos[index].ToString());
                posiblesCaminos[index].ToFile(_myJugador);
                posiblesCaminos[index].ToFile();
                //prueba de pathfinder el 9/8 - Angel
               /* string str;
                do
                {///*
                    Console.WriteLine("escribe la columnafila de ");
                    str = Console.ReadLine();
                    Posicion de = new Posicion(Convert.ToInt16(str.Substring(2, 2)), Convert.ToInt16(str.Substring(0, 2)));
                    Console.WriteLine("escribe la columnafila a ");
                    str = Console.ReadLine();
                    Posicion a = new Posicion(Convert.ToInt16(str.Substring(2, 2)), Convert.ToInt16(str.Substring(0, 2)));
                    Camino Camino = new Camino(_tablero.Casilla(de), _mechs[_myJugador], _tablero.Casilla(a), _tablero, _estrategia);
                    //
                    //Camino Camino = new Camino(_tablero.Casilla(_mechs[_myJugador].posicion()), _mechs[_myJugador], _tablero.Casilla(destino), _tablero);

                    Camino.print();
                    Console.WriteLine(Camino.ToString());

                    Console.ReadKey();
                } while (true);*/
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Metodo que elige el objetivo al cual atacar si la estrategia es ofensiva. Si la estrategia es defensiva
        /// devolvera como objetivo al rival mas peligroso.
        /// </summary>
        /// <returns>un mech rival; tipo Mech</returns>
        private Mech eleccionObjetivo() {
            Mech objetivo=null;

            if (_numeroJugadores != 2)
            {
                float[] notasParciales = new float[_numeroJugadores];

                //Vemos cual la distancia maxima de los enemigos
                int max = 0;
                for (int i = 0; i < _mechs.Length; i++)
                {
                    if (i != _myJugador && _mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) > max)
                        max = _mechs[_myJugador].posicion().distancia(_mechs[i].posicion());
                }

                //Escogemos al enemigo
                if (_estrategia == Estrategia.Agresiva)
                {
                    //Asignamos una nota a cada mech en funcion de la distancia que nos separa y su puntuacion
                    for (int i = 0; i < _mechs.Length; i++)
                        if (i != _myJugador)
                        {
                            //Console.WriteLine("Mech: " + _mechs[i].nombre());
                            //Nota estado
                            notasParciales[i] = _mechs[i].notaEstado() * PanelControl.NOTA_MOV;
                            //Console.WriteLine("Nota estado: " + _mechs[i].notaEstado());
                            //Nota distancia
                            notasParciales[i] += ((_mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) * 10.0f) / max) * PanelControl.DISTANCIA_MOV;
                            //Nota por haberse movido
                            if (_config.mechMovido(_mechs[i].numeroJ()))
                                notasParciales[i] -= 1;
                            //Console.WriteLine("Nota distancia: " + ((_mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) * 10.0f) / max) * PanelControl.DISTANCIA_MOV);
                            //Console.WriteLine("Nota parcial:" + notasParciales[i]);
                            //Console.WriteLine();
                        }

                    //Nos quedamos con el mech que tenga la nota menor
                    float nota = float.MaxValue;
                    for (int i = 0; i < _mechs.Length; i++)
                        if (i != _myJugador && notasParciales[i] < nota)
                        {
                            nota = notasParciales[i];
                            objetivo = _mechs[i];
                        }
                }
                else //estrategia.defensiva
                {
                    for (int i = 0; i < _mechs.Length; i++)
                        if (i != _myJugador)
                        {
                            //Console.WriteLine("Mech: " + _mechs[i].nombre());
                            //Nota estado
                            notasParciales[i] = _mechs[i].notaEstado() * PanelControl.NOTA_MOV;
                            //Console.WriteLine("Nota estado: " + _mechs[i].notaEstado());
                            //Nota distancia
                            notasParciales[i] += 10 - (((_mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) * 10.0f) / max) * PanelControl.DISTANCIA_MOV);
                            //Nota por haberse movido
                            if (_config.mechMovido(_mechs[i].numeroJ()))
                                notasParciales[i] += 1;
                            //Console.WriteLine("Nota distancia: " + ((_mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) * 10.0f) / max) * PanelControl.DISTANCIA_MOV);
                            //Console.WriteLine("Nota parcial:" + notasParciales[i]);
                            //Console.WriteLine();
                        }

                    //Nos quedamos con el mech que tenga la nota mayor
                    float nota = float.MinValue;
                    for (int i = 0; i < _mechs.Length; i++)
                        if (i != _myJugador && notasParciales[i] > nota)
                        {
                            nota = notasParciales[i];
                            objetivo = _mechs[i];
                        }
                }

            }else
                objetivo = _mechs[(_myJugador + 1) % 2];

            return objetivo;
        }

        /// <summary>
        /// elige el destino el cual se desea calcular el camino
        /// </summary>
        /// <param name="objetivo">elige el mech rival que mas peso tine; tipo Mech</param>
        /// <param name="destinosElegidos">Array de tamñao fijo que sera completado con los mejores destinos. Segun su tamaño
        /// se incluiran mas o menos destinos</param>
        /// <returns>devuelve la posicion que se desea alcanzar; tipo posicion</returns>
        private void seleccionDestino(Mech objetivo , Posicion[] destinosElegidos)  
        {
            List<Posicion> posiblesDestinos = new List<Posicion>();
            int[] puntuacion;
            Boolean salir = false;
            int escogidas = 0;

            //Escogemos las casillas a evaluar
            if (_estrategia == Estrategia.Agresiva)
            {
                _tablero.casillasEnRadio(objetivo.posicion(), posiblesDestinos,PanelControl.radio);
            }
            else {
                _tablero.casillasEnMov(_mechs[_myJugador], posiblesDestinos, _mechs[_myJugador].puntosAndar()*2/3 );
            }

            //Puntuamos las casillas
            puntuacion = new int[posiblesDestinos.Count];
            for (int i = 0; i < posiblesDestinos.Count; i++)
            {
                puntuacion[i] = puntuacionCasilla(posiblesDestinos[i], objetivo);
            }

            //Escogemos las que tienen mejores puntuaciones mientras haya espacio en el array destinosElegidos
            int max = 0;
            for (int i = 0; i < puntuacion.Length; i++)
                if (puntuacion[i] > max)
                    max = puntuacion[i];

            while (!salir)
            {
                for (int i = 0; i < puntuacion.Length && !salir; i++)
                {
                    if (puntuacion[i] == max)
                    {
                        destinosElegidos[escogidas] = posiblesDestinos[i];
                        escogidas++;
                        Console.WriteLine(i + ": " + posiblesDestinos[i].ToString() + " punt:" + puntuacion[i]);
                    }
                    if (escogidas >= destinosElegidos.Length)
                        salir = true;
                }
                max--;
            }
        }

        
        /// <summary>
        /// Metodo que puntua una casilla
        /// </summary>
        /// <param name="p">Casilla a puntuar</param>
        /// <param name="objetivo">Enemigo al que nos enfrentamos. Para fase de ataque</param>
        /// <returns>Entero que corresponde a la puntuacion dada</returns>
        private int puntuacionCasilla ( Posicion p , Mech objetivo ) 
        {
            int puntuacion = 0;

            //Estrategia agresiva
            if (_estrategia == Estrategia.Agresiva)
            {
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

                //Puntuacion por distancia
                puntuacion += (PanelControl.radio - p.distancia(objetivo.posicion()) ); 
                //puntuacion -= (int)Math.Truncate(_mechs[_myJugador].posicion().distancia(p)*PanelControl.pesoDistancia);
            }
            else //Estrategia defensiva
            {
                //Puntuacion por tipo terreno
                switch (_tablero.Casilla(p).tipoTerreno())
                {
                    case 0://despejado
                        puntuacion += 1;
                        break;
                    case 1://pavimentado
                        puntuacion += 1;
                        break;
                    case 2://agua
                        puntuacion -= 2;
                        break;
                    case 3://pantanoso
                        puntuacion += 0;
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
                        puntuacion += 3;
                        break;
                    case 2://bosque denso
                        puntuacion += 4;
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
                if ( _tablero.Casilla(p).nivel() > 0)
                    puntuacion += -_tablero.Casilla(p).nivel();

                //Puntuacion por distancia
                int distancia , bonificador;
                for (int i = 0; i < _mechs.Length; i++) {
                    if (i != _myJugador) {
                        distancia = _mechs[i].posicion().distancia(p);
                        if (_mechs[i].tipo() == tipoMech.Asalto) {
                            bonificador = 2;
                        } else
                            bonificador = 1;

                        if (distancia > _mechs[i].distanciaTiroLarga())
                        {
                            puntuacion += 5 * bonificador;
                        }
                        else if (distancia > _mechs[i].distanciaTiroMedia())
                        {
                            puntuacion += 4 * bonificador;
                        }
                        else if (distancia > _mechs[i].distanciaTiroCorta())
                        {
                            puntuacion += 2 * bonificador;
                        }
                        else
                            puntuacion += 0;
                    }
                }

            }

            return puntuacion;
        }


        /// <summary>
        /// Indica si desde una posicion hay linea de vision con el objetivo. Suponemos que estaremos levantados
        /// </summary>
        /// <param name="p">Posicion en la que estaremos hubicados</param>
        /// <param name="objetivo">Objetivo al que queremos hacerle la LdV</param>
        /// <returns></returns>
        private Boolean casillaEnLdV(Posicion p, Mech objetivo) 
        {
            LdV linea = new LdV(p, _myJugador, objetivo, _tablero);

            return linea.ldv();
        }
        #endregion


        #region faseReaccion
        /// <summary>
        /// Metodo que realiza la fase de Reaccion
        /// </summary>
        private void faseReaccion() {
            Console.WriteLine("Fase Reaccion");
            Console.WriteLine();

            string giro = "Igual";

            if (_mechs[_myJugador].operativo() && ((MechJugador)_mechs[_myJugador]).consciente())
            {
                determinarEstrategia();

                List<Mech> objetivos = new List<Mech>();
                Mech objetivo;

                //Escogemos a los mech si estan dentro del alcance de tiro largo y no estan en nuestra espalda
                for (int i = 0; i < _mechs.Length; i++)
                    if (i != _myJugador && _mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) < _mechs[_myJugador].distanciaTiroLarga() &&
                        !_mechs[_myJugador].conoTrasero(_mechs[i].posicion(), _mechs[_myJugador].ladoEncaramientoTorso()) && _mechs[i].operativo())
                        objetivos.Add(_mechs[i]);

                //Dejamos solo con los que tengamos linea de vision
                List<LdV> ldv = new List<LdV>();
                objetivosLdV(objetivos, ldv);

                //Escogemos al mas debil
                List<Componente> armasADisparar = new List<Componente>();
                Console.WriteLine();
                objetivo = objetivoMasDebil(objetivos, ldv, armasADisparar);

                //Vemos si nos giramos
                if (objetivo != null)
                {
                    if (!_mechs[_myJugador].conoDelantero(objetivo.posicion(), _mechs[_myJugador].ladoEncaramiento()))
                    {
                        if (_mechs[_myJugador].conoDerecho(objetivo.posicion(), _mechs[_myJugador].ladoEncaramiento()))
                        {
                            giro = "Derecha";
                        }
                        else if (_mechs[_myJugador].conoIzquierdo(objetivo.posicion(), _mechs[_myJugador].ladoEncaramiento()))
                            giro = "Izquierda";
                    }
                }
            }

            //Escribimos las ordenes
            StreamWriter f = new StreamWriter(PanelControl.archivoAcciones(_myJugador), false);
            f.WriteLine(giro);
            f.Close();

            Console.ReadLine();
        }

        #endregion


        #region faseAtaqueArmas
        /// <summary>
        /// Metodo que realiza la fase de Ataque con Armas
        /// </summary>
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
                Mech objetivo = null;

                //Escogemos a los mech si estan dentro del alcance de tiro largo y no estan en nuestra espalda
                for (int i = 0; i < _mechs.Length; i++)
                    if (i != _myJugador && _mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) < _mechs[_myJugador].distanciaTiroLarga() &&
                        !_mechs[_myJugador].conoTrasero(_mechs[i].posicion(), _mechs[_myJugador].ladoEncaramientoTorso()) && _mechs[i].operativo() )
                        objetivos.Add(_mechs[i]);

                /*Console.WriteLine("Al principio tenemos:");
                for (int i = 0; i < objetivos.Count; i++)
                    Console.WriteLine(i + ": " + objetivos[i].nombre());
                Console.WriteLine();*/

                //Dejamos solo con los que tengamos linea de vision
                List<LdV> ldv = new List<LdV>();
                objetivosLdV(objetivos, ldv);

                /*Console.WriteLine();
                Console.WriteLine("En LdV tenemos:" + objetivos.Count);
                for (int i = 0; i < objetivos.Count; i++)
                    Console.WriteLine(i + ": " + objetivos[i].nombre());*/

                //Escogemos al mas debil
                List<Componente> armasADisparar = new List<Componente>();
                Console.WriteLine();
                objetivo = objetivoMasDebil(objetivos, ldv, armasADisparar);

                Console.WriteLine();
                if (objetivo != null) Console.WriteLine("El objetivo es:" + objetivo.nombre() +" ldv:"+ldv[0].ldv()+" cobr:"+ldv[0].cobertura());
                Console.WriteLine();


                //Vemos las armas a dispararle
                seleccionArmasDisparar(objetivo, armasADisparar , ldv);

                Console.WriteLine("Disparamos:");
                for (int i = 0; i < armasADisparar.Count; i++)
                    Console.WriteLine(armasADisparar[i].nombre());

                //Escribimos las ordenes
                escribirOrdenesArmas(objetivo, armasADisparar);

                Console.ReadLine();
            }
        }

        /// <summary>
        /// Metodo que escoge al objetivo mas debil de la lista de <paramref name="objetivos"/>, y devuelve en <paramref name="armas"/> las armas
        /// que se le pueden disparar
        /// </summary>
        /// <param name="objetivos">Lista de objetivos a evaluar</param>
        /// <param name="ldv">LdV con los objetivos</param>
        /// <param name="armas">Lista de armas. Vacia</param>
        private Mech objetivoMasDebil (List<Mech> objetivos , List<LdV> ldv , List<Componente> armas) 
        {
            Mech objetivo = null;

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
                    notasParciales[i] = objetivos[i].notaEstado() * PanelControl.NOTA_ARMAS;
                    Console.WriteLine("Nota mech: " + objetivos[i].notaEstado());

                    //Añadimos la nota del danio y la seleccion de armas para ese mech
                    List<Componente> arm = new List<Componente>();
                    danio = armasPermitidas(objetivos[i], arm);
                    armamento.Add(arm);
                    notaAux = 10 - (danio * 10.0f / _mechs[_myJugador].danioMaximo());
                    Console.WriteLine("Nota danio: " + notaAux);
                    notasParciales[i] += (notaAux * PanelControl.DANIO_ARMAS);

                    //Añadimos la nota de la distancia
                    notaAux = (_mechs[_myJugador].posicion().distancia(objetivos[i].posicion()) * 10.0f) / _mechs[_myJugador].distanciaTiroLarga();
                    notasParciales[i] += notaAux * PanelControl.DISTANCIA_ARMAS;
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
                objetivo = objetivos[0];
                objetivos.Clear();
            }
            else if (objetivos.Count == 1)
            {
                objetivo = objetivos[0];
                armasPermitidas(objetivos[0], armas);
                objetivos.Clear();
            }


            return objetivo;
        }

        /// <summary>
        /// Metodo que deja en la lista de <paramref name="objetivos"/> solo a los que tengan ldV con nuestro Mech
        /// </summary>
        /// <param name="objetivos">Lista de objetivos a evaluar</param>
        /// <param name="ldv">Lista de linea de vision para cada mech de la lista de objetivos. Vacia</param>
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

        /// <summary>
        /// Selecciona dentro del parametro <paramref name="seleccionArmas"/> todas aquellas armas que se le puedan
        /// disparar al <paramref name="objetivo"/>
        /// </summary>
        /// <param name="objetivo">Objetivo al que se quiere disparar</param>
        /// <param name="seleccionArmas">Lista de armas. Vacia</param>
        /// <returns>La cantidad de daño que hacen las armas escogidas</returns>
        private int armasPermitidas( Mech objetivo, List<Componente> seleccionArmas) 
        {        
            string situacion;
            MechJugador my = (MechJugador)_mechs[_myJugador];
            int encTorso = my.ladoEncaramientoTorso();
            int enc = my.ladoEncaramiento();
            int distancia = my.posicion().distancia(objetivo.posicion());
            int danio = 0;

            //Vemos la localizacion del objetivo respecto a nuestro mech
            if (my.conoDerecho(objetivo.posicion(), encTorso)) {
                situacion = "DRCHA";
            }
            else if (my.conoIzquierdo(objetivo.posicion(), encTorso)) {
                situacion = "IZQ";
            }
            else
                situacion = "DNTE";

            //Vemos las armas que podria disparar
            ArrayList armas = my.armas();
            String localizacion;
            for (int i = 0; i < armas.Count; i++) 
            {
                localizacion = ((Componente)armas[i]).localizacionSTRING();
                if (my.tieneMunicion((Componente)armas[i]) && ((Componente)armas[i]).operativo() && ((Componente)armas[i]).distanciaLarga() >= distancia &&
                    ((Componente)armas[i]).distanciaMinima() < distancia && 
                   ( (localizacion == "BI" && (situacion == "IZQ" || situacion == "DNTE"))
                   || (localizacion == "BD" && (situacion == "DRCHA" || situacion == "DNTE"))
                   || ((localizacion == "TC" || localizacion == "TD" || localizacion == "TI" || localizacion == "CAB") && situacion == "DNTE" )
                   || (localizacion == "PD" && (my.conoDerecho(objetivo.posicion(), enc) || my.conoDelantero(objetivo.posicion(), enc)))
                   || (localizacion == "PI" && (my.conoIzquierdo(objetivo.posicion(), enc) || my.conoDelantero(objetivo.posicion(), enc)))
                    ) )
                {
                    danio += ((Componente)armas[i]).danio();
                    seleccionArmas.Add((Componente)armas[i]);
                }
            }
            
            return danio;
        }

        /// <summary>
        /// Metodo que selecciona de la lista de armas a disparar, aquellas que finalmente dispararemos al Mech
        /// <paramref name="objetivo"/>
        /// </summary>
        /// <param name="objetivos">Lista de objetivos. Un unico elemento</param>
        /// <param name="seleccionArmas">List de componentes tipo arma</param>
        /// <param name="ldV">Lista de LdV. Contendra un elemento, la linea de vision al objetivo</param>
        private void seleccionArmasDisparar( Mech objetivo, List<Componente> seleccionArmas , List<LdV> ldV ) 
        {

            int calorMovimiento;
            int limiteCalor; 

            if (objetivo != null)
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
                    limiteCalor = PanelControl.calorOfensivo + _mechs[_myJugador].numeroRadiadores() - _mechs[_myJugador].nivelTemp() - calorMovimiento;
                }else
                    limiteCalor = PanelControl.calorDefensivo + _mechs[_myJugador].numeroRadiadores() - _mechs[_myJugador].nivelTemp() - calorMovimiento;
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
                        && _mechs[_myJugador].tiradaImpacto(seleccionArmas[orden[itr]],objetivo,_tablero,ldV[0].cobertura(),_config.movimiento(_myJugador),_config.movimiento(objetivo.numeroJ())) <= PanelControl.limiteTiradaPermitido )
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

        /// <summary>
        /// Metodo que escribe la tarea que va a realizar nuestro mech en esta fase del juego
        /// </summary>
        /// <param name="objetivo">El objetivo a disparar</param>
        /// <param name="seleccionArmas">List de armas a disparar</param>
        private void escribirOrdenesArmas( Mech objetivo, List<Componente> seleccionArmas) {
            StreamWriter f = new StreamWriter(PanelControl.archivoAcciones(_myJugador), false);

            f.WriteLine("False");//coger garrote
            if ( objetivo != null) {
                f.WriteLine(objetivo.posicion().ToString()); //Posicion del objetivo primario
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
                    f.WriteLine(objetivo.posicion().ToString());
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


        #region faseAtaqueFisico
        /// <summary>
        /// Metodo que realiza la fase de Ataques Fisicos
        /// </summary>
        private void faseAtaquesFisico() 
        {
            Console.WriteLine("Fase Ataque Fisico");
            Console.WriteLine();

            Mech objetivo = null;
            MechJugador my = (MechJugador)_mechs[_myJugador];
            String ordenes="";
            int numeroArmas = 0, diferenciaNivel = 0;
            bool ataque = false;

            if (my.operativo() && my.consciente())
            {
                String situacion ;
                int encTorso = my.ladoEncaramientoTorso();
                int enc = my.ladoEncaramiento();

                //Escogemos al objetivo
                for (int i = 0; i < _mechs.Length; i++) 
                {
                    if (i != _myJugador && my.posicion().distancia(_mechs[i].posicion()) == 1 && !my.conoTrasero(_mechs[i].posicion(), encTorso))
                        objetivo = _mechs[i];
                }

                //Si tenemos objetivo
                if (objetivo != null)
                {
                    Console.WriteLine("Hay objetivo");
                    //Vemos la localizacion del objetivo respecto a nuestro mech
                    if (my.conoDerecho(objetivo.posicion(), encTorso))
                    {
                        situacion = "DRCHA";
                    }
                    else if (my.conoIzquierdo(objetivo.posicion(), encTorso))
                    {
                        situacion = "IZQ";
                    }
                    else
                        situacion = "DNTE";

                    //Vemos la diferencia de nivel entre las casillas
                    diferenciaNivel = _tablero.Casilla(my.posicion()).nivel() - _tablero.Casilla(objetivo.posicion()).nivel();

                    //Vemos las armas fisicas con las que darle
                    if (my.conBrazoDerecho() && my.conAntebrazoDerecho() && (situacion == "DNTE" || situacion == "DRCHA") && !my.disparoBrazoDerecha()
                        && ((diferenciaNivel == 0 && !objetivo.enSuelo()) || diferenciaNivel == -1) )
                    {
                        numeroArmas++;
                        ordenes += "BD\n";
                        ordenes += "1000\n";
                        ordenes += objetivo.posicion() + "\n";
                        ordenes += "Mech\n";
                    }
                    
                    if (my.conBrazoIzquierdo() && my.conAntebrazoIzquierdo() && (situacion == "DNTE" || situacion == "IZQ") && !my.disparoBrazoIzquierdo()
                        && ((diferenciaNivel == 0 && !objetivo.enSuelo()) || diferenciaNivel == -1))
                    {
                        numeroArmas++;
                        ordenes += "BI\n";
                        ordenes += "1000\n";
                        ordenes += objetivo.posicion() + "\n";
                        ordenes += "Mech\n";
                    }
                    
                    if (my.conPiernaIzquierda() && (my.conoIzquierdo(objetivo.posicion(), enc) || my.conoDelantero(objetivo.posicion(), enc)) && !my.disparoPiernaIzquierda()
                        && (diferenciaNivel == 0 || (diferenciaNivel == 1 && !objetivo.enSuelo()) ))
                    {
                        numeroArmas++;
                        ordenes += "PI\n";
                        ordenes += "2000\n";
                        ordenes += objetivo.posicion() + "\n";
                        ordenes += "Mech\n";
                        ataque = true;
                    }
                    
                    if (my.conPiernaDerecha() && (my.conoDerecho(objetivo.posicion(), enc) || my.conoDelantero(objetivo.posicion(), enc)) && !my.disparoPiernaDerecha()
                        && (diferenciaNivel == 0 || (diferenciaNivel == 1 && !objetivo.enSuelo())) && !ataque)
                    {
                        numeroArmas++;
                        ordenes += "PD\n";
                        ordenes += "2000\n";
                        ordenes += objetivo.posicion() + "\n";
                        ordenes += "Mech\n";
                        ataque = true;
                    }
                }
            }

            ordenes = numeroArmas.ToString() + "\n" + ordenes;

            //Escribimos las ordenes
            StreamWriter f = new StreamWriter(PanelControl.archivoAcciones(_myJugador), false);
            Console.WriteLine(ordenes);
            f.WriteLine(ordenes);
            f.Close();

            Console.ReadLine();
        }
        #endregion

        /// <summary>
        /// Metodo que realiza la fase final del turno
        /// </summary>
        private void faseFinalTurno() {
            Console.WriteLine("Fase Final de Turno");
            Console.WriteLine();

            StreamWriter f = new StreamWriter(PanelControl.archivoAcciones(_myJugador), false);
            f.WriteLine(0); //numero radiadores a apagar
            f.WriteLine(0);//numero de radiadores a encender que estuvieran apagados
            f.WriteLine(((MechJugador)_mechs[_myJugador]).garrote()); //si tiene garrote y quiere soltarlo
            f.WriteLine(0); //numero de municiones a expulsar
            //quitamos municiones?¿
            f.Close();

            Console.ReadLine();
        }

        #endregion

        #region atributos

        /// <summary>
        /// Numero de nuestro jugador dentro del array de mech
        /// </summary>
        private int _myJugador;

        /// <summary>
        /// Fase de juego donde nos encontramos
        /// </summary>
        private String _faseJuego;

        /// <summary>
        /// Numero de jugadores que tiene la partida
        /// </summary>
        private int _numeroJugadores;

        /// <summary>
        /// Tablero de la partida
        /// </summary>
        private Tablero _tablero;

        /// <summary>
        /// Array con los Mechs
        /// </summary>
        private Mech[] _mechs;

        /// <summary>
        /// Variable con la configuracion del juego
        /// </summary>
		private ConfiguracionJuego _config;

        /// <summary>
        /// Variable que determina la estrategia a seguir por nuestro mech
        /// </summary>
        private Estrategia _estrategia;

        #endregion

    }

}
