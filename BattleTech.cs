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
                //Vemos si quien es el jugador con la nota mas alta
                int max=-1;
                float nota = 0;
                for (int i = 0; i < _mechs.Length; i++)
                    if (_mechs[i].operativo() && nota < _mechs[i].notaEstado() ) {
                        nota = _mechs[i].notaEstado();
                        max = i;
                    }

                //Si somos el jugador con la nota mas alta o la diferencia en es pequeña, nos ponemos en agresiva
                if (max == _myJugador || Math.Abs(_mechs[_myJugador].notaEstado() - _mechs[max].notaEstado()) <= 0.3)
                {
                    _estrategia = Estrategia.Agresiva;
                }
                else
                    _estrategia = Estrategia.Defensiva;
            }
        }

        /*
        /// <summary>
        /// Funcion para debuguear
        /// </summary>
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
            }

			string c1 ;
			Posicion p1 ;
			Boolean fin=false;
			
			while (!fin) {
                //Console.WriteLine();
				//Console.WriteLine("Elija la casilla que desea averiguar la distancia a la casilla del mech");
				c1=Console.ReadLine();
				if (c1!="q"){
					p1 = new Posicion(c1);
                    //Casilla aux = _tablero.Casilla(p1);
                    //Console.WriteLine("La distancia es:"+_mechs[0].posicion().distancia(p1));
                   /* if (_mechs[0].conoDerecho(p1, _mechs[0].ladoEncaramiento()))
                    {
                        Console.WriteLine("cono drcha");
                    }
                    else if (_mechs[0].conoIzquierdo(p1, _mechs[0].ladoEncaramiento()))
                        Console.WriteLine("cono izq");
					//_tablero.casillaInfo(p1.fila(),p1.columna());
				/*}else
					fin=true;
			}
		}
        */

        #region faseMovimiento

        /// <summary>
        /// /funcion que engloba las acciones de la fase de movimiento
        /// </summary>
        private void faseMovimiento() 
        {
            //Console.WriteLine("Fase movimiento");
            //Console.WriteLine();
            Mech objetivo;
            bool salto = false;
            int destino = 0;
            List<Posicion> destinos;
            Camino[] posiblesCaminos;

            if (_mechs[_myJugador].operativo() && ((MechJugador)_mechs[_myJugador]).consciente())
            {
                //Determinamos la estrategia con la que jugaremos
                determinarEstrategia();
  
                //Seleccionamos el objetivo hacia el que vamos a dirigirnos en caso de ser una estrategia ofensiva
                objetivo = eleccionObjetivo();
                //if(objetivo != null )Console.WriteLine("El objetivo elegido es: " + objetivo.nombre());

                //Seleccionamos la casilla destino
                destinos = seleccionDestino(objetivo);

                //Vemos si hay condicion de salto
                for (int i = 0; i < destinos.Count && !salto; i++)
                    if (deboSaltar(destinos[i], objetivo))
                    {
                        salto = true;
                        destino = i;
                    }

                //Si no podemos saltar directamente a ninguna casilla
                if (!salto)
                {
                    if (destinos.Count > 0)
                    {
                        //Console.WriteLine("Tenemos destinos:" + destinos.Count);
                        //Evaluamos la ultima posicion de cada camino y nos quedamos con el mayor
                        posiblesCaminos = new Camino[destinos.Count];
                        int[] puntuacionCamino = new int[destinos.Count];
                        int valor = int.MinValue;
                        for (int i = 0; i < destinos.Count; i++)
                        {
                            posiblesCaminos[i] = new Camino(_myJugador, _tablero.Casilla(destinos[i]), _tablero, _estrategia, objetivo.numeroJ(), _mechs);
                            //Console.WriteLine("Destino preferido:" + i + ": " + destinos[i].ToString());
                            //posiblesCaminos[i].print();
                            //Console.WriteLine(posiblesCaminos[i].ToString());
                            //Console.WriteLine("Llegamos hasta: " + posiblesCaminos[i].casillaFinal().posicion().ToString());
                            puntuacionCamino[i] = puntuacionCasilla(posiblesCaminos[i].casillaFinal().posicion(), objetivo);
                            //Console.WriteLine("Puntuacion: " + puntuacionCamino[i]);
                            if (puntuacionCamino[i] > valor && (casillaEnLdV(posiblesCaminos[i].casillaFinal().posicion(), objetivo) || i == destinos.Count - 1))
                            {
                                destino = i;
                                valor = puntuacionCamino[i];
                            }
                            //Console.WriteLine();
                        }

                        //Console.WriteLine();
                        //Console.WriteLine("Elegimos " + posiblesCaminos[destino].casillaFinal().posicion().ToString());

                        //posiblesCaminos[destino].print();
                        //Console.WriteLine(posiblesCaminos[destino].ToString());
                        posiblesCaminos[destino].ToFile(_myJugador);
                        //Console.ReadLine();
                        escribirLog(posiblesCaminos[destino].ToDebug());
                    }
                    else //Caso base de no tener destinos porque no tenemos puntos de movimiento
                    {
                        //Hacemos camino con nuestro casilla como destino
                        /*Camino c = new Camino(_myJugador, _tablero.Casilla(_mechs[_myJugador].posicion()), _tablero, _estrategia, objetivo.numeroJ(), _mechs);
                        c.print();
                        c.ToFile(_myJugador);*/
                        //Console.ReadLine();
                    }
                }
                else //Caso de poder llegar al destino saltando directamente
                {
                    Camino c = new Camino(_myJugador, destinos[destino], _tablero, objetivo.numeroJ(), _mechs);
                    //c.print();
                    c.ToFile(_myJugador);
                    //Console.ReadLine();
                    escribirLog(c.ToDebug());
                }   
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
                    if (i != _myJugador && _mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) > max && _mechs[i].operativo())
                        max = _mechs[_myJugador].posicion().distancia(_mechs[i].posicion());
                }

                //Escogemos al enemigo
                if (_estrategia == Estrategia.Agresiva)
                {
                    //Asignamos una nota a cada mech en funcion de la distancia que nos separa y su puntuacion
                    for (int i = 0; i < _mechs.Length; i++)
                        if (i != _myJugador && _mechs[i].operativo())
                        {
                            //Nota estado
                            notasParciales[i] = _mechs[i].notaEstado() * PanelControl.NOTA_MOV;
                            //Nota distancia
                            notasParciales[i] += ((_mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) * 10.0f) / max) * PanelControl.DISTANCIA_MOV;
                            //Nota por haberse movido
                            if (_config.mechMovido(_mechs[i].numeroJ()))
                                notasParciales[i] -= 1;
                        }

                    //Nos quedamos con el mech que tenga la nota menor
                    float nota = float.MaxValue;
                    for (int i = 0; i < _mechs.Length; i++)
                        if (i != _myJugador && notasParciales[i] < nota && _mechs[i].operativo())
                        {
                            nota = notasParciales[i];
                            objetivo = _mechs[i];
                        }
                }
                else //estrategia.defensiva
                {
                    for (int i = 0; i < _mechs.Length; i++)
                        if (i != _myJugador && _mechs[i].operativo())
                        {
                            //Nota estado
                            notasParciales[i] = _mechs[i].notaEstado() * PanelControl.NOTA_MOV;
                            //Nota distancia
                            notasParciales[i] += 10 - (((_mechs[_myJugador].posicion().distancia(_mechs[i].posicion()) * 10.0f) / max) * PanelControl.DISTANCIA_MOV);
                            //Nota por haberse movido
                            if (_config.mechMovido(_mechs[i].numeroJ()))
                                notasParciales[i] += 1;
                        }

                    //Nos quedamos con el mech que tenga la nota mayor
                    float nota = float.MinValue;
                    for (int i = 0; i < _mechs.Length; i++)
                        if (i != _myJugador && notasParciales[i] > nota && _mechs[i].operativo())
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
        private List<Posicion> seleccionDestino(Mech objetivo )  
        {
            List<Posicion> destinosElegidos = new List<Posicion>();
            List<Posicion> posiblesDestinos ;
            int[] puntuacion;
            Boolean salir = false;
            int escogidas = 0;

            //Escogemos las casillas a evaluar
            if (_estrategia == Estrategia.Agresiva)
            {
                posiblesDestinos = _tablero.casillasEnRadio(objetivo.posicion(),PanelControl.radio,_mechs,_myJugador);
            }
            else {
                posiblesDestinos = _tablero.casillasEnMov(_mechs[_myJugador], ((MechJugador)_mechs[_myJugador]).andar(), _mechs);
            }

            //Puntuamos las casillas
            puntuacion = new int[posiblesDestinos.Count];
            for (int i = 0; i < posiblesDestinos.Count; i++)
            {
                puntuacion[i] = puntuacionCasilla(posiblesDestinos[i], objetivo);
            }

            //Vemos cual es la maxima puntuacion
            int max = 0;
            for (int i = 0; i < puntuacion.Length; i++)
                if (puntuacion[i] > max)
                    max = puntuacion[i];

            //Escogemos las que tienen mejores puntuaciones mientras haya espacio en el array destinosElegidos
            while (!salir)
            {
                //La recorremos de forma ascendente
                if ((_estrategia == Estrategia.Agresiva && _mechs[_myJugador].posicion().fila() >= objetivo.posicion().fila())
                    || (_estrategia == Estrategia.Defensiva && _mechs[_myJugador].posicion().fila() <= objetivo.posicion().fila()))
                {
                    for (int i = 0; i < puntuacion.Length && !salir; i++)
                    {
                        if (puntuacion[i] == max)
                        {
                            destinosElegidos.Add(posiblesDestinos[i]);
                            escogidas++;
                        }
                        if (escogidas >= PanelControl.numeroDestinos)
                            salir = true;
                    }
                }
                else //La recorremos de forma descendente
                {
                    for (int i = puntuacion.Length-1 ; i >= 0 && !salir; i--)
                    {
                        if (puntuacion[i] == max)
                        {
                            destinosElegidos.Add(posiblesDestinos[i]);
                            escogidas++;
                        }
                        if (escogidas >= PanelControl.numeroDestinos)
                            salir = true;
                    }
                }
                max--;
                if (max < -20) salir = true;
            }

            return destinosElegidos;
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

                //Bono por estar pegado
                if (objetivo.posicion().distancia(p) == 1)
                    puntuacion += 3;

                //Bono por cercania
                if (objetivo.posicion().distancia(p) <= PanelControl.radio)
                    puntuacion += (PanelControl.radio - p.distancia(objetivo.posicion()) ) + 4;

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
                        puntuacion += 2;
                        break;
                    case 2://bosque denso
                        puntuacion += 3;
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

                //Bonus por estar situado a espalda cuando el objetivo ya se ha movido 
                if (_config.mechMovido(objetivo.numeroJ()) && objetivo.conoTrasero(p, objetivo.ladoEncaramiento())
                    && (_mechs[_myJugador].posicion().distancia(objetivo.posicion()) == 1 || _mechs[_myJugador].posicion().distancia(objetivo.posicion()) == 2))
                    puntuacion += 5;

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


        /// <summary>
        /// Indica si un mech puede saltar a la casilla indica como parametro <paramref name="p"/>
        /// </summary>
        /// <param name="p">Casilla a la que desea saltar</param>
        /// <param name="objetivo">Mech al que nos estamos enfrentando</param>
        /// <returns>True en caso afirmativo</returns>
        private Boolean deboSaltar(Posicion p , Mech objetivo) {
            bool salto = false;
            int punt = ((MechJugador)_mechs[_myJugador]).saltar();

            if (_estrategia == Estrategia.Defensiva && _mechs[_myJugador].posicion().distancia(p) <= punt && _tablero.Casilla(p).tipoTerreno() != 2
                && punt != 0 && _config.mechMovido(objetivo.numeroJ()))
            {
                salto = true;
            }
            else if (_estrategia == Estrategia.Agresiva && _mechs[_myJugador].posicion().distancia(p) <= punt && _tablero.Casilla(p).tipoTerreno() != 2
                && objetivo.conoTrasero(p, objetivo.ladoEncaramiento()) && punt != 0 && p.ToString() != _mechs[_myJugador].posicion().ToString() && _config.mechMovido(objetivo.numeroJ())
                && _mechs[_myJugador].posicion().distancia(objetivo.posicion()) > 1)
                salto = true;

            return salto;
        }
        #endregion


        #region faseReaccion
        /// <summary>
        /// Metodo que realiza la fase de Reaccion
        /// </summary>
        private void faseReaccion() {
            String log = "";
            //Console.WriteLine("Fase Reaccion");
            //Console.WriteLine();

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
                List<LdV> ldv;
                ldv = objetivosLdV(objetivos);

                //Escogemos al mas debil
                List<Componente> armasADisparar = new List<Componente>();
                objetivo = objetivoMasDebil(objetivos, ldv, armasADisparar);

                //Vemos si nos giramos
                if (objetivo != null)
                {
                    if (!_mechs[_myJugador].conoDelantero(objetivo.posicion(), _mechs[_myJugador].ladoEncaramiento()))
                    {
                        if (_mechs[_myJugador].conoDerecho(objetivo.posicion(), _mechs[_myJugador].ladoEncaramiento()))
                        {
                            giro = "Derecha";
                            log += "\tNos giramos a derecha hacia el Mech J-" + objetivo.numeroJ() + ": " + objetivo.nombre() 
                                + ". Hubicacion: " + objetivo.posicion().ToString() + "\n";
                        }
                        else if (_mechs[_myJugador].conoIzquierdo(objetivo.posicion(), _mechs[_myJugador].ladoEncaramiento()))
                        {
                            giro = "Izquierda";
                            log += "\tNos giramos a izquierda hacia el Mech J-" + objetivo.numeroJ() + ": " + objetivo.nombre()
                                + ". Hubicacion: " + objetivo.posicion().ToString() + "\n";
                        }
                    }
                }
            }

            if (giro == "Igual") log += "\tNos quedamos igual\n";

            //Escribimos las ordenes
            StreamWriter f = new StreamWriter(PanelControl.archivoAcciones(_myJugador), false);
            f.WriteLine(giro);
            f.Close();

            escribirLog(log);
            //Console.ReadLine();
        }

        #endregion


        #region faseAtaqueArmas
        /// <summary>
        /// Metodo que realiza la fase de Ataque con Armas
        /// </summary>
        private void faseAtaqueArmas() {
            /*
             * 1º Eleccion de rivales dentro de radio accion (rango alcance: distancia tiro larga media) y que no esten en el cono trasero
             * 2º Ver si hay linea de vision con ellos
             * 3º a. De los restantes escoger al mas debil (nota mas baja)
             * 4º Ver las armas a dispararle
             * 5º Escribir el fichero
             */
            String log = "";
            //Console.WriteLine("Fase Ataque con Armas");
            //Console.WriteLine();
            List<Mech> objetivos = new List<Mech>();
            List<Componente> armasADisparar = new List<Componente>();
            List<LdV> ldv;
            Mech objetivo = null;

            if (_mechs[_myJugador].operativo() && ((MechJugador)_mechs[_myJugador]).consciente())
            {
                determinarEstrategia();
                log += "\tEstrategia del Mech Jugador: " + _estrategia + "\n";
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
                ldv = objetivosLdV(objetivos);

                /*Console.WriteLine();
                Console.WriteLine("En LdV tenemos:" + objetivos.Count);
                for (int i = 0; i < objetivos.Count; i++)
                    Console.WriteLine(i + ": " + objetivos[i].nombre());*/

                //Escogemos al mas debil
                objetivo = objetivoMasDebil(objetivos, ldv, armasADisparar);

                if (objetivo != null) 
                {
                    log += "\tEscogemos como objetivo al Mech J-" + objetivo.numeroJ() + ": " + objetivo.nombre() + ". Hubicacion: " + objetivo.posicion().ToString() + "\n";
                    log += "\tLinea de vision con objetivo: " + ldv[0].ldv() + ". Cobertura del objetivo: " + ldv[0].cobertura() + "\n";
                    log += "\tPodriamos disparar:\n";
                    foreach (Componente c in armasADisparar) 
                    {
                        log += "\t\t" + c.nombre() + " \n";
                    }
                }
                //Console.WriteLine("El objetivo es:" + objetivo.nombre() +" ldv:"+ldv[0].ldv()+" cobr:"+ldv[0].cobertura());
                //Console.WriteLine();


                //Vemos las armas a dispararle
                seleccionArmasDisparar(objetivo, armasADisparar , ldv);

                if (objetivo != null)
                {
                    if (armasADisparar.Count > 0)
                    {
                        log += "\tDisparamos:\n";
                        foreach (Componente c in armasADisparar)
                        {
                            log += "\t\t" + c.nombre() + " \n";
                        }
                    }
                    else
                        log += "\tNo disparemos ningun arma\n";
                    //Console.WriteLine("Disparamos:");
                    //for (int i = 0; i < armasADisparar.Count; i++)
                        //Console.WriteLine(armasADisparar[i].nombre());
                }else
                    log += "\tNo hay ningun objetivo a nuestro alcance o bien no hay linea de vision con ellos\n";

                //Escribimos las ordenes
                escribirOrdenesArmas(objetivo, armasADisparar);
            }

            escribirLog(log);
            //Console.ReadLine();
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
                    //Console.WriteLine("Mech: "+objetivos[i].nombre());
                    //Añadimos las notas
                    notasParciales[i] = objetivos[i].notaEstado() * PanelControl.NOTA_ARMAS;
                    //Console.WriteLine("Nota mech: " + objetivos[i].notaEstado());

                    //Añadimos la nota del danio y la seleccion de armas para ese mech
                    List<Componente> arm = new List<Componente>();
                    danio = armasPermitidas(objetivos[i], arm);
                    armamento.Add(arm);
                    notaAux = 10 - (danio * 10.0f / _mechs[_myJugador].danioMaximo());
                    //Console.WriteLine("Nota danio: " + notaAux);
                    notasParciales[i] += (notaAux * PanelControl.DANIO_ARMAS);

                    //Añadimos la nota de la distancia
                    notaAux = (_mechs[_myJugador].posicion().distancia(objetivos[i].posicion()) * 10.0f) / _mechs[_myJugador].distanciaTiroLarga();
                    notasParciales[i] += notaAux * PanelControl.DISTANCIA_ARMAS;
                    //Console.WriteLine("Nota distancia: " + notaAux);

                    //Si estamos a su espalda, tenemos un bonus
                    if (objetivos[i].conoTrasero(_mechs[_myJugador].posicion(), objetivos[i].ladoEncaramiento()))
                        notasParciales[i] -= 1;

                    //Si esta atascado o en el suelo (y nos encontramos en una casilla colindante), tenemos un bonus
                    if (objetivos[i].atascado() || (objetivos[i].enSuelo() && _mechs[_myJugador].posicion().distancia(objetivos[i].posicion()) == 1))
                        notasParciales[i] -= 0.5f;

                    //Console.WriteLine("Nota parcial: " + notasParciales[i]);
                    //Console.WriteLine();
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
        /// <returns>Devuelve una lista con correspondencia de cada posicion con la lista de objetivos</returns>
        private List<LdV> objetivosLdV(List<Mech> objetivos )
        {
            LdV c;
            List<LdV> ldv = new List<LdV>();
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

            return ldv;
        }

        /// <summary>
        /// Selecciona dentro del parametro <paramref name="seleccionArmas"/> todas aquellas armas que se le puedan
        /// disparar al <paramref name="objetivo"/>
        /// </summary>
        /// <param name="objetivo">Objetivo al que se quiere disparar</param>
        /// <param name="seleccionArmas">Lista de armas. Variable usada para devolver la lista de armas</param>
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
            bool conBrazos = (my.conBrazoIzquierdo() || my.conBrazoDerecho()) ? true : false;
            bool ambosBrazos = (my.conBrazoIzquierdo() && my.conBrazoDerecho()) ? true : false;
            String localizacion;
            for (int i = 0; i < armas.Count; i++) 
            {
                localizacion = ((Componente)armas[i]).localizacionSTRING();
                if (my.tieneMunicion((Componente)armas[i]) && ((Componente)armas[i]).operativo() && ((Componente)armas[i]).distanciaLarga() >= distancia &&
                    ((Componente)armas[i]).distanciaMinima() < distancia && 
                   ( (localizacion == "BI" && (situacion == "IZQ" || situacion == "DNTE") && !my.enSuelo())
                   || (localizacion == "BD" && (situacion == "DRCHA" || situacion == "DNTE") && (!my.enSuelo() || (my.enSuelo() && ambosBrazos)) )
                   || ((localizacion == "TC" || localizacion == "TD" || localizacion == "TI" || localizacion == "CAB") && situacion == "DNTE" && ((my.enSuelo() && conBrazos) || !my.enSuelo() ))
                   || (localizacion == "PD" && (my.conoDerecho(objetivo.posicion(), enc) || my.conoDelantero(objetivo.posicion(), enc)) && !my.enSuelo())
                   || (localizacion == "PI" && (my.conoIzquierdo(objetivo.posicion(), enc) || my.conoDelantero(objetivo.posicion(), enc)) && !my.enSuelo())
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
        /// <param name="objetivo">Mech rival al que nos enfrentamos</param>
        /// <param name="seleccionArmas">List de componentes tipo arma</param>
        /// <param name="ldV">Lista de LdV. Contendra un elemento, la linea de vision al objetivo, o ninguno
        /// en caso de ser un objetivo nulo</param>
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

                if (limiteCalor > 29) limiteCalor = 29;

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
            String log = "";
            //Console.WriteLine("Fase Ataque Fisico");
            //Console.WriteLine();

            Mech objetivo = null;
            MechJugador my = (MechJugador)_mechs[_myJugador];
            String ordenes="";
            int numeroArmas = 0, diferenciaNivel = 0;
            bool ataque = false;

            if (my.operativo() && my.consciente())
            {
                String situacion;
                int encTorso = my.ladoEncaramientoTorso();
                int enc = my.ladoEncaramiento();

                //Escogemos al objetivo
                for (int i = 0; i < _mechs.Length; i++)
                {
                    if (i != _myJugador && my.posicion().distancia(_mechs[i].posicion()) == 1 && !my.conoTrasero(_mechs[i].posicion(), encTorso)
                        && _mechs[i].operativo())
                        objetivo = _mechs[i];
                }

                //Si tenemos objetivo
                if (objetivo != null)
                {
                    log += "\tEscogemos como objetivo al Mech J-" + objetivo.numeroJ() + ": " + objetivo.nombre() + ". Hubicacion: " + objetivo.posicion().ToString() + "\n";
                    //Console.WriteLine("Hay objetivo");
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
                        && ((diferenciaNivel == 0 && !objetivo.enSuelo()) || diferenciaNivel == -1) && !my.enSuelo())
                    {
                        numeroArmas++;
                        ordenes += "BD\n";
                        ordenes += "1000\n";
                        ordenes += objetivo.posicion() + "\n";
                        ordenes += "Mech\n";
                        log += "\t\tGolpeamos con: BD\n";
                    }

                    if (my.conBrazoIzquierdo() && my.conAntebrazoIzquierdo() && (situacion == "DNTE" || situacion == "IZQ") && !my.disparoBrazoIzquierdo()
                        && ((diferenciaNivel == 0 && !objetivo.enSuelo()) || diferenciaNivel == -1) && !my.enSuelo())
                    {
                        numeroArmas++;
                        ordenes += "BI\n";
                        ordenes += "1000\n";
                        ordenes += objetivo.posicion() + "\n";
                        ordenes += "Mech\n";
                        log += "\t\tGolpeamos con: BI\n";
                    }

                    if (my.conPiernaIzquierda() && (my.conoIzquierdo(objetivo.posicion(), enc) || my.conoDelantero(objetivo.posicion(), enc)) && !my.disparoPiernaIzquierda()
                        && (diferenciaNivel == 0 || (diferenciaNivel == 1 && !objetivo.enSuelo())) && !my.enSuelo())
                    {
                        numeroArmas++;
                        ordenes += "PI\n";
                        ordenes += "2000\n";
                        ordenes += objetivo.posicion() + "\n";
                        ordenes += "Mech\n";
                        ataque = true;
                        log += "\t\tGolpeamos con: PI\n";
                    }

                    if (my.conPiernaDerecha() && (my.conoDerecho(objetivo.posicion(), enc) || my.conoDelantero(objetivo.posicion(), enc)) && !my.disparoPiernaDerecha()
                        && (diferenciaNivel == 0 || (diferenciaNivel == 1 && !objetivo.enSuelo())) && !ataque && !my.enSuelo())
                    {
                        numeroArmas++;
                        ordenes += "PD\n";
                        ordenes += "2000\n";
                        ordenes += objetivo.posicion() + "\n";
                        ordenes += "Mech\n";
                        ataque = true;
                        log += "\t\tGolpeamos con: PD\n";
                    }
                }
            }
            else
                log += "\t\tNo podemos atacar a ningun Mech";

            if (numeroArmas == 0) log += "\t\tNo podemos golpearle\n";
            ordenes = numeroArmas.ToString() + "\n" + ordenes;

            //Escribimos las ordenes
            StreamWriter f = new StreamWriter(PanelControl.archivoAcciones(_myJugador), false);
            //Console.WriteLine(ordenes);
            escribirLog(log);
            f.WriteLine(ordenes);
            f.Close();

            //Console.ReadLine();
        }
        #endregion


        #region faseFinalTurno
        /// <summary>
        /// Metodo que realiza la fase final del turno
        /// </summary>
        private void faseFinalTurno() {
            String log = "";
            //Console.WriteLine("Fase Final de Turno");
            //Console.WriteLine();

            StreamWriter f = new StreamWriter(PanelControl.archivoAcciones(_myJugador), false);
            f.WriteLine(0); //numero radiadores a apagar
            log += "\tNº radiadores a apagar: 0\n";
            f.WriteLine(0);//numero de radiadores a encender que estuvieran apagados
            log += "\tNº radiadores apagados a encender: 0\n";
            f.WriteLine(((MechJugador)_mechs[_myJugador]).garrote()); //si tiene garrote y quiere soltarlo
            log += "\tSoltamos garrote: "+((MechJugador)_mechs[_myJugador]).garrote()+"\n";
            f.WriteLine(0); //numero de municiones a expulsar
            log += "\tNº municiones a expulsar (funcionalidad no implementada):0\n";
            //quitamos municiones?¿
            f.Close();
            escribirLog(log);

            //Console.ReadLine();
        }
        #endregion


        /// <summary>
        /// Funcion que escribe en el fichero log el resultado de las distintas fases del juego
        /// </summary>
        /// <param name="text">Cadena de texto con la informacion a escribir en el log</param>
        private void escribirLog( String text ) 
        {
            StreamWriter f = new StreamWriter(PanelControl.fichLog + _myJugador.ToString() + ".log", true);
            f.WriteLine("===============================================================================");
            f.WriteLine("Dia: " + System.DateTime.Now.Day + "/" + System.DateTime.Now.Month + "/" + System.DateTime.Now.Year + " Hora: " + System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second);
            f.WriteLine("Fase de " + _faseJuego + ". Jugador: " + _mechs[_myJugador].nombre() + ". Nº: " + _myJugador.ToString() + ". Hubicacion: " + _mechs[_myJugador].posicion().ToString() + ". Encaramiento: " + _mechs[_myJugador].ladoEncaramiento().ToString());
            f.WriteLine(text);
            f.WriteLine("===============================================================================\n");
            f.Close();
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
