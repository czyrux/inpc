
using System;
using System.IO;
using System.Collections;

namespace ico {
	
    /// <summary>
    /// Clase que tendra todos los parametros relacionados con el mech
    /// </summary>
	public class Mech {

        #region atributos

		//ATRIBUTOS	ESTADO
		protected int numeroJugadores;
		
		protected int _numeroJ;
		protected Boolean _operativo; //***
        protected Boolean _desconectado; //***
        protected Boolean _atascado; //***
		protected Boolean _enSuelo; //***
		protected Posicion _posicion; // clase posicion
        protected int _ladoEncaramiento; //***
        protected int _ladoEncaramientoTorso; //***
        protected int _nivelTemp; //***
		protected Boolean _ardiendo;
		protected Boolean _garrote;
		protected int _tipoGarrote;
		
		//puntos blindaje
		protected int _BlindBrazoIzquierdo;
		protected int _BlindTorsoIzquierdo;
		protected int _BlindPiernaIzquierda;
		protected int _BlindPiernaDerecha;
		protected int _BlindTorsoDerecho;
		protected int _BlindBrazoDerecho;
		protected int _BlindTorsoCentral;
		protected int _BlindCabeza;
		protected int _BlindAtrasTorsoIzquierdo;
		protected int _BlindAtrasTorsoDerecho;
		protected int _BlindAtrasTorsoCentral;

        //protected int _BlindTotal;
		
		//puntos estructura interna
		protected int _EstrucBrazoIzquierdo;
		protected int _EstrucTorsoIzquierdo;
		protected int _EstrucPiernaIzquierda;
		protected int _EstrucPiernaDerecha;
		protected int _EstrucTorsoDerecho;
		protected int _EstrucBrazoDerecho;
		protected int _EstrucTorsoCentral;
		protected int _EstrucCabeza;
		

        //ATRIBUTOS DE FICHERO DEFINICION
		protected string _nombre;
		protected string _modelo;
		protected int _toneladas;
		protected int _potencia;
		protected int _numeroRadiadoresInternos;
		protected int _numeroRadiadores;
		protected Boolean _masc;
		protected Boolean _dacmtd;
		protected Boolean _dacmti;
		protected Boolean _dacmtc;
		protected int _maximoCalorGenerado;
        protected Boolean _conBrazos; //***
        protected Boolean _conHombroIzquierdo; //***
        protected Boolean _conBrazoIzquierdo; //***
        protected Boolean _conAntebrazoIzquierdo; //***
        protected Boolean _conManoIzquierda; //***
        protected Boolean _conHombroDerecho; //***
        protected Boolean _conBrazoDerecho; //***
        protected Boolean _conAntebrazoDerecho; //***
        protected Boolean _conManoDerecha; //***
		
		protected int _numeroComponentes;
		protected Componente[] _componentes;
		
		protected int _numeroArmas;
		protected ArrayList _armas;
		protected int _numeroActuadores;
		protected Actuador[] _actuadores;

        protected LocalizacionMech[] _slots;
		/*protected LocalizacionMech _slotsBrazoIzquierdo;
		protected LocalizacionMech _slotsTorsoIzquierdo;
		protected LocalizacionMech _slotsPiernaIzquierda;
		protected LocalizacionMech _slotsPiernaDerecha;
		protected LocalizacionMech _slotsTorsoDerecho;
		protected LocalizacionMech _slotsBrazoDerecho;
		protected LocalizacionMech _slotsTorsoCentral;
		protected LocalizacionMech _slotsCabeza;*/

        protected int _andarDefinicion;
        protected int _correrDefinicion;
        protected int _saltarDefinicion;
        protected int _tipoRadiador;

        protected int _distanciaTiroCorta; //***
        protected int _distanciaTiroMedia; //***
        protected int _distanciaTiroLarga; //***
        protected int _maxAlcanceDisparo; //***
        protected int _danioMaximo; //***

        //DATOS DE ARMADURA INICIALES DEL MECH
        protected int _BlindBrazoIzquierdoInicial;
        protected int _BlindTorsoIzquierdoInicial;
        protected int _BlindPiernaIzquierdaInicial;
        protected int _BlindPiernaDerechaInicial;
        protected int _BlindTorsoDerechoInicial;
        protected int _BlindBrazoDerechoInicial;
        protected int _BlindTorsoCentralInicial;
        protected int _BlindCabezaInicial;
        protected int _BlindAtrasTorsoIzquierdoInicial;
        protected int _BlindAtrasTorsoDerechoInicial;
        protected int _BlindAtrasTorsoCentralInicial;

        protected int _EstrucBrazoIzquierdoInicial;
        protected int _EstrucTorsoIzquierdoInicial;
        protected int _EstrucPiernaIzquierdaInicial;
        protected int _EstrucPiernaDerechaInicial;
        protected int _EstrucTorsoDerechoInicial;
        protected int _EstrucBrazoDerechoInicial;
        protected int _EstrucTorsoCentralInicial;
        protected int _EstrucCabezaInicial;

        protected float _notaMech;

        #endregion

        #region constructores
        //CONSTRUCTORES
		public Mech () {}
		
		public Mech ( StreamReader f1 , StreamReader f2 ,  int n) {
			numeroJugadores=n;
			fichero_estado(f1);
			fichero_definicion(f2);
		}
        #endregion

        #region Privados ficheros
        //Metodos de lecturas de datos

		private void fichero_estado( StreamReader f ) {
			_numeroJ=Convert.ToInt32(f.ReadLine());
			
			//datos posicion y estado
			_operativo=Convert.ToBoolean(f.ReadLine());
			_desconectado=Convert.ToBoolean(f.ReadLine());
			_atascado=Convert.ToBoolean(f.ReadLine());
			_enSuelo=Convert.ToBoolean(f.ReadLine());
			_posicion = new Posicion( f.ReadLine());
			_ladoEncaramiento=Convert.ToInt32(f.ReadLine());
			_ladoEncaramientoTorso=Convert.ToInt32(f.ReadLine());
			_nivelTemp=Convert.ToInt32(f.ReadLine());
			_ardiendo=Convert.ToBoolean(f.ReadLine());
			_garrote=Convert.ToBoolean(f.ReadLine());
			_tipoGarrote=Convert.ToInt32(f.ReadLine());
			
			//datos blindaje
			_BlindBrazoIzquierdo=Convert.ToInt32(f.ReadLine());
            _BlindTorsoIzquierdo = Convert.ToInt32(f.ReadLine());
			_BlindPiernaIzquierda=Convert.ToInt32(f.ReadLine());
			_BlindPiernaDerecha=Convert.ToInt32(f.ReadLine());
			_BlindTorsoDerecho=Convert.ToInt32(f.ReadLine());
			_BlindBrazoDerecho=Convert.ToInt32(f.ReadLine());
			_BlindTorsoCentral=Convert.ToInt32(f.ReadLine());
			_BlindCabeza=Convert.ToInt32(f.ReadLine());
			_BlindAtrasTorsoIzquierdo=Convert.ToInt32(f.ReadLine());
			_BlindAtrasTorsoDerecho=Convert.ToInt32(f.ReadLine());
            _BlindAtrasTorsoCentral = Convert.ToInt32(f.ReadLine());

            //_BlindTotal = _BlindBrazoIzquierdo + _BlindTorsoIzquierdo + _BlindPiernaIzquierda + _BlindPiernaDerecha + _BlindTorsoDerecho + _BlindBrazoDerecho + _BlindTorsoCentral + _BlindCabeza + _BlindAtrasTorsoIzquierdo + _BlindAtrasTorsoDerecho + _BlindAtrasTorsoCentral;

			//datos esctructura interna
			_EstrucBrazoIzquierdo=Convert.ToInt32(f.ReadLine());
			_EstrucTorsoIzquierdo=Convert.ToInt32(f.ReadLine());
			_EstrucPiernaIzquierda=Convert.ToInt32(f.ReadLine());
			_EstrucPiernaDerecha=Convert.ToInt32(f.ReadLine());
			_EstrucTorsoDerecho=Convert.ToInt32(f.ReadLine());
			_EstrucBrazoDerecho=Convert.ToInt32(f.ReadLine());
			_EstrucTorsoCentral=Convert.ToInt32(f.ReadLine());
            _EstrucCabeza = Convert.ToInt32(f.ReadLine());
			
			//NARC  e iNARC No guardamos los valores
			for ( int i=0 ; i<numeroJugadores*2 ; i++) {
				Convert.ToBoolean(f.ReadLine());
			}

            //Damos un valor por defecto a la nota
            _notaMech = -1;
		}
		
		protected void fichero_definicion( StreamReader f){
			
			f.ReadLine(); //nombre fichero
			
			_nombre=f.ReadLine();
			_modelo=f.ReadLine();
			_toneladas=Convert.ToInt32(f.ReadLine());
			_potencia=Convert.ToInt32(f.ReadLine());
			_numeroRadiadoresInternos=Convert.ToInt32(f.ReadLine());
			_numeroRadiadores=Convert.ToInt32(f.ReadLine());
			_masc=Convert.ToBoolean(f.ReadLine());
			_dacmtd=Convert.ToBoolean(f.ReadLine());
			_dacmti=Convert.ToBoolean(f.ReadLine());
			_dacmtc=Convert.ToBoolean(f.ReadLine());
			_maximoCalorGenerado=Convert.ToInt32(f.ReadLine());
			_conBrazos=Convert.ToBoolean(f.ReadLine());
			_conHombroIzquierdo=Convert.ToBoolean(f.ReadLine());
			_conBrazoIzquierdo=Convert.ToBoolean(f.ReadLine());
			_conAntebrazoIzquierdo=Convert.ToBoolean(f.ReadLine());
			_conManoIzquierda=Convert.ToBoolean(f.ReadLine());
			_conHombroDerecho=Convert.ToBoolean(f.ReadLine());
			_conBrazoDerecho=Convert.ToBoolean(f.ReadLine());
			_conAntebrazoDerecho=Convert.ToBoolean(f.ReadLine());
			_conManoDerecha=Convert.ToBoolean(f.ReadLine());

            //readline correspondientes a los datos de blindaje y estructura interna del fichero de definicion, que son repetidos
            for (int i = 0; i < 19; i++) f.ReadLine();
			
			_numeroComponentes=Convert.ToInt32(f.ReadLine());
			_componentes = new Componente[_numeroComponentes];
			_armas = new ArrayList();
			Componente aux1;
            _danioMaximo = 0;
			for ( int i=0 ; i<_numeroComponentes ; i++){
				aux1 = new Componente(f);
                if (aux1.clase() == "ARMA") { 
                    _armas.Add(aux1);
                    _danioMaximo += aux1.danio();
                }
				_componentes[i]=aux1;
			}

            _numeroArmas = Convert.ToInt32(f.ReadLine());
			_numeroActuadores=Convert.ToInt32(f.ReadLine());
			_actuadores = new Actuador[_numeroActuadores];
			Actuador aux2;
			for (int i=0 ; i<_numeroActuadores ; i++ ){
				aux2 = new Actuador(f);
				_actuadores[i]=aux2;
			}

            _slots = new LocalizacionMech[8];
            _slots[0] = new LocalizacionMech(f);
            _slots[1] = new LocalizacionMech(f);
            _slots[2] = new LocalizacionMech(f);
            _slots[3] = new LocalizacionMech(f);
            _slots[4] = new LocalizacionMech(f);
            _slots[5] = new LocalizacionMech(f);
            _slots[6] = new LocalizacionMech(f);
            _slots[7] = new LocalizacionMech(f);
			/*_slotsBrazoIzquierdo = new LocalizacionMech(f);
			_slotsTorsoIzquierdo = new LocalizacionMech(f);
			_slotsPiernaIzquierda = new LocalizacionMech(f);
			_slotsPiernaDerecha = new LocalizacionMech(f);
			_slotsTorsoDerecho = new LocalizacionMech(f);
			_slotsBrazoDerecho = new LocalizacionMech(f);
			_slotsTorsoCentral = new LocalizacionMech(f);
			_slotsCabeza = new LocalizacionMech(f);*/
			
			_andarDefinicion=Convert.ToInt32(f.ReadLine());
			_correrDefinicion=Convert.ToInt32(f.ReadLine());
			_saltarDefinicion=Convert.ToInt32(f.ReadLine());
			_tipoRadiador=Convert.ToInt32(f.ReadLine());

            //Rellenamos los atributos de la armadura que tenia al inicio de partida el mech
            datos_armadura_inicial();

            //Calculamos la media de las armas
            calculoDistanciaTiro();
		}

        protected void datos_armadura_inicial() {
            bool nuevo = true;
            string path = "C:/ficheros/";
            //si existe el fichero con los datos de armadura iniciales del mech, lo leemos
            if (System.IO.File.Exists(path+"armaduraInicialJ" + _numeroJ.ToString() + ".sbt")) {
                StreamReader f = new StreamReader(path+"armaduraInicialJ" + _numeroJ.ToString() + ".sbt");
                string nombre=f.ReadLine();
                //si no es el mismo modelo de mech es el fichero de otro mech de una partida anterior
                if (_nombre == nombre) nuevo = false;

                if (!nuevo)
                {
                    _BlindBrazoIzquierdoInicial = Convert.ToInt32(f.ReadLine());
                    _BlindTorsoIzquierdoInicial = Convert.ToInt32(f.ReadLine());
                    _BlindPiernaIzquierdaInicial = Convert.ToInt32(f.ReadLine());
                    _BlindPiernaDerechaInicial = Convert.ToInt32(f.ReadLine());
                    _BlindTorsoDerechoInicial = Convert.ToInt32(f.ReadLine());
                    _BlindBrazoDerechoInicial = Convert.ToInt32(f.ReadLine());
                    _BlindTorsoCentralInicial = Convert.ToInt32(f.ReadLine());
                    _BlindCabezaInicial = Convert.ToInt32(f.ReadLine());
                    _BlindAtrasTorsoIzquierdoInicial = Convert.ToInt32(f.ReadLine());
                    _BlindAtrasTorsoDerechoInicial = Convert.ToInt32(f.ReadLine());
                    _BlindAtrasTorsoCentralInicial = Convert.ToInt32(f.ReadLine());

                    _EstrucBrazoIzquierdoInicial = Convert.ToInt32(f.ReadLine());
                    _EstrucTorsoIzquierdoInicial = Convert.ToInt32(f.ReadLine());
                    _EstrucPiernaIzquierdaInicial = Convert.ToInt32(f.ReadLine());
                    _EstrucPiernaDerechaInicial = Convert.ToInt32(f.ReadLine());
                    _EstrucTorsoDerechoInicial = Convert.ToInt32(f.ReadLine());
                    _EstrucBrazoDerechoInicial = Convert.ToInt32(f.ReadLine());
                    _EstrucTorsoCentralInicial = Convert.ToInt32(f.ReadLine());
                    _EstrucCabezaInicial = Convert.ToInt32(f.ReadLine());
                }

                //Cerramos el fichero
                f.Close();
            }
            
            if (nuevo) { //en caso opuesto lo creamos

                StreamWriter f = new StreamWriter(path+"armaduraInicialJ" + _numeroJ.ToString() + ".sbt");
                //escribimos los atributos
                //escribimos el modelo de mech
                f.WriteLine(_nombre);
                //puntos blindaje
                f.WriteLine(_BlindBrazoIzquierdo);
                f.WriteLine(_BlindTorsoIzquierdo);
                f.WriteLine(_BlindPiernaIzquierda);
                f.WriteLine(_BlindPiernaDerecha);
                f.WriteLine(_BlindTorsoDerecho);
                f.WriteLine(_BlindBrazoDerecho);
                f.WriteLine(_BlindTorsoCentral);
                f.WriteLine(_BlindCabeza);
                f.WriteLine(_BlindAtrasTorsoIzquierdo);
                f.WriteLine(_BlindAtrasTorsoDerecho);
                f.WriteLine(_BlindAtrasTorsoCentral);

                //puntos estructura interna
                f.WriteLine(_EstrucBrazoIzquierdo);
                f.WriteLine(_EstrucTorsoIzquierdo);
                f.WriteLine(_EstrucPiernaIzquierda);
                f.WriteLine(_EstrucPiernaDerecha);
                f.WriteLine(_EstrucTorsoDerecho);
                f.WriteLine(_EstrucBrazoDerecho);
                f.WriteLine(_EstrucTorsoCentral);
                f.WriteLine(_EstrucCabeza);

                f.Close();

                //actualizamos los datos de blindaje y estructura interna iniciales para este turno
                _BlindBrazoIzquierdoInicial = _BlindBrazoIzquierdo;
                _BlindTorsoIzquierdoInicial = _BlindTorsoIzquierdo;
                _BlindPiernaIzquierdaInicial = _BlindPiernaIzquierda;
                _BlindPiernaDerechaInicial = _BlindPiernaDerecha;
                _BlindTorsoDerechoInicial = _BlindTorsoDerecho;
                _BlindBrazoDerechoInicial = _BlindBrazoDerecho;
                _BlindTorsoCentralInicial = _BlindTorsoCentral;
                _BlindCabezaInicial = _BlindCabeza;
                _BlindAtrasTorsoIzquierdoInicial = _BlindAtrasTorsoIzquierdo;
                _BlindAtrasTorsoDerechoInicial = _BlindAtrasTorsoDerecho;
                _BlindAtrasTorsoCentralInicial = _BlindAtrasTorsoCentral;

                _EstrucBrazoIzquierdoInicial = _EstrucBrazoIzquierdo;
                _EstrucTorsoIzquierdoInicial = _EstrucTorsoIzquierdo;
                _EstrucPiernaIzquierdaInicial = _EstrucPiernaIzquierda;
                _EstrucPiernaDerechaInicial = _EstrucPiernaDerecha;
                _EstrucTorsoDerechoInicial = _EstrucTorsoDerecho;
                _EstrucBrazoDerechoInicial = _EstrucBrazoDerecho;
                _EstrucTorsoCentralInicial = _EstrucTorsoCentral;
                _EstrucCabezaInicial = _EstrucCabeza;
            }
        }


        #endregion		
			
        #region metodosGet
				
        /// <summary>
        /// Indica el numero de jugador
        /// </summary>
        /// <returns>Entero</returns>
		public int numeroJ() { return _numeroJ; }

        /// <summary>
        /// Indica si el mech esta operativo
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean operativo() { return _operativo; }

        /// <summary>
        /// Indica si el mech esta desconectado
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean desconectado(){return _desconectado; }

        /// <summary>
        /// Indica si el mech esta atascado
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean atascado(){ return _atascado; }

        /// <summary>
        /// Indica si el mech esta en el suelo
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean enSuelo(){ return _enSuelo; }

        /// <summary>
        /// Posicion que ocupa el mech en el tablero
        /// </summary>
        /// <returns>Variable de clase Posicion</returns>
		public Posicion posicion() { return _posicion; }

        /// <summary>
        /// Indica la fila donde se encuentra el mech
        /// </summary>
        /// <returns>Entero con el valor de la fila</returns>
		public int posicionFila() { return _posicion.fila();}

        /// <summary>
        /// Indica la columna donde se encuentra el mech
        /// </summary>
        /// <returns>Entero con el valor de la columna</returns>
		public int posicionColumna() { return _posicion.columna();}

        /// <summary>
        /// Indica el lado de encaramiento del mech
        /// </summary>
        /// <returns>Entero del 1-6</returns>
		public int ladoEncaramiento() { return _ladoEncaramiento; }

        /// <summary>
        /// Indica el lado de encaramiento del torso del mech
        /// </summary>
        /// <returns>Entero 1-6</returns>
		public int ladoEncaramientoTorso() { return _ladoEncaramientoTorso; }

        /// <summary>
        /// Indica el nivel de temperatura acumulada del mech
        /// </summary>
        /// <returns>Entero que indica el nivel de temp.</returns>
		public int nivelTemp() { return _nivelTemp; }

        /// <summary>
        /// Indica si el mech esta ardiendo
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean ardiendo() { return _ardiendo; }

        /// <summary>
        /// Indica si el mech tiene garrote
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean garrote() { return _garrote; }

        /// <summary>
        /// Indica el tipo de garrote en caso de poseerlo
        /// </summary>
        /// <returns>Entero que indica el tipo de garrote</returns>
		public int tipoGarrote() { return _tipoGarrote; }
		
		//puntos blindaje
		/*public int BlindBrazoIzquierdo() { return _BlindBrazoIzquierdo; }		
		public int BlindTorsoIzquierdo() { return _BlindTorsoIzquierdo; }		
		public int BlindPiernaIzquierda() { return _BlindPiernaIzquierda; }		
		public int BlindPiernaDerecha() { return _BlindPiernaDerecha; }		
		public int BlindTorsoDerecho() { return _BlindTorsoDerecho; }		
		public int BlindBrazoDerecho() { return _BlindBrazoDerecho; }		
		public int BlindTorsoCentral() { return _BlindTorsoCentral; }		
		public int BlindCabeza() { return _BlindCabeza; }		
		public int BlindAtrasTorsoIzquierdo() { return _BlindAtrasTorsoIzquierdo; }		
		public int BlindAtrasTorsoDerecho() { return _BlindAtrasTorsoDerecho; }		
		public int BlindAtrasTorsoCentral() { return _BlindAtrasTorsoCentral; }
		
		//puntos estructura interna
		public int EstrucBrazoIzquierdo() { return _EstrucBrazoIzquierdo; }
		public int EstrucTorsoIzquierdo() { return _EstrucTorsoIzquierdo; }
		public int EstrucPiernaIzquierda() { return _EstrucPiernaIzquierda; }
		public int EstrucPiernaDerecha() { return _EstrucPiernaDerecha; }
		public int EstrucTorsoDerecho() { return _EstrucTorsoDerecho; }		
		public int EstrucBrazoDerecho() { return _EstrucBrazoDerecho; }
		public int EstrucTorsoCentral() { return _EstrucTorsoCentral; }		
		public int EstrucCabeza() { return _EstrucCabeza; }*/
		
		public string nombre() { return _nombre; }
		public string modelo() { return _modelo; }
		public int tonelados() { return _toneladas; }
		public int potencia() { return _potencia; }
		public int numeroRadiadoresInternos() { return _numeroRadiadoresInternos; }

		public int numeroRadiadores() {
            if (_tipoRadiador == 1)
            {
                return _numeroRadiadores * 2;
            }
            else
                return _numeroRadiadores;
         }

		/*public Boolean masc() { return _masc; }
		public Boolean dacmtd() { return _dacmtd; }
		public Boolean dacmti() { return _dacmti; }
		public Boolean dacmtc() { return _dacmtc; }*/
		public int maximoCalorGenerado() { return _maximoCalorGenerado; }
		public Boolean conBrazos() { return _conBrazos; }
		public Boolean conHombroIzquierdo() { return _conHombroIzquierdo; }
		public Boolean conBrazoIzquierdo() { return _conBrazoIzquierdo; }
		public Boolean conAntebrazoIzquierdo() { return _conAntebrazoIzquierdo; }
		public Boolean conManoIzquierda() { return _conManoIzquierda; }
		public Boolean conHombroDerecho() { return _conHombroDerecho; }
		public Boolean conBrazoDerecho() { return _conBrazoDerecho; }
		public Boolean conAntebrazoDerecho() { return _conAntebrazoDerecho; }
		public Boolean conManoDerecha() { return _conManoDerecha; }
		
		public int numeroComponentes() { return _numeroComponentes; }
		public Componente[] componentes() { return _componentes; }
		
		public int numeroArmas() { return _numeroArmas; }
		public ArrayList armas() { return _armas;}

		public int numeroActuadores() { return _numeroActuadores; }
		public Actuador[] actuadores() { return _actuadores; }

        public LocalizacionMech slots(int i) {
            if (i < 8)
            {
                return _slots[i];
            }
            else return null;
        }

        public int puntosAndar() { return _andarDefinicion; }
		public int puntosCorrer() { return _correrDefinicion; }
		public int puntosSaltar() { return _saltarDefinicion; }
		public int tipoRadiador() { return _tipoRadiador; }
		
		public int distanciaTiroCorta() { return _distanciaTiroCorta;}
		public int distanciaTiroMedia() { return _distanciaTiroMedia;}
		public int distanciaTiroLarga() { return _distanciaTiroLarga;}
        public int maxAlcanceTiro() { return _maxAlcanceDisparo; }
        public int danioMaximo() { return _danioMaximo; }
		
        #endregion

        #region metodos
		public void datos(){
			Console.WriteLine("Numero jugador:"+ _numeroJ);
			Console.WriteLine("Nombre mech: "+_nombre);
			Console.WriteLine("Modelo mech: "+_modelo);
            Console.WriteLine("Tonelaje: " + _toneladas);
			Console.WriteLine("operativo:" + _operativo);
			Console.WriteLine("desconectado: "+_desconectado);
			Console.WriteLine("atascado terreno pantanoso: "+_atascado);
			Console.WriteLine("en el suelo: "+_enSuelo);
			//Console.WriteLine("posicion fila: "+_posicion.fila()+" columna: "+_posicion.columna());
			//Console.WriteLine("lado encaramiento: "+_ladoEncaramiento);
			//Console.WriteLine("lado encaramiento torso: "+_ladoEncaramientoTorso);
			Console.WriteLine("nivel temperatura: "+_nivelTemp);
			/*Console.WriteLine("Numero armas: "+_numeroArmas);
			for ( int i=0 ; i<_armas.Count ; i++ ){
				Console.WriteLine("Arma "+i+": "+((Componente)_armas[i]).nombre());		
				Console.WriteLine("\tTipo Arma: "+((Componente)_armas[i]).tipoArma());
				Console.WriteLine("\tDanio: "+((Componente)_armas[i]).danio());
				Console.WriteLine("\tLocalizacion: "+((Componente)_armas[i]).localizacion());
				Console.WriteLine("\tDistancia corta: "+ ((Componente)_armas[i]).distanciaCorta());
				Console.WriteLine("\tDistancia media: "+ ((Componente)_armas[i]).distanciaMedia());
				Console.WriteLine("\tDistancia larga: "+ ((Componente)_armas[i]).distanciaLarga());
				Console.WriteLine("\tDistancia minima: "+ ((Componente)_armas[i]).distanciaMinima());

			}*/
			Console.WriteLine("Distancia corta en media de tiro del mech: "+_distanciaTiroCorta);
			Console.WriteLine("Distancia media en media de tiro del mech: "+_distanciaTiroMedia);
			Console.WriteLine("Distancia larga en media de tiro del mech: "+_distanciaTiroLarga);
            Console.WriteLine("Maximo alcance: " + _maxAlcanceDisparo);
			/*Console.WriteLine("Numero radiadores: "+_numeroRadiadores);*/

            Console.WriteLine("Datos blindaje: ");
            Console.WriteLine("Blindaje brazo izq : "+_BlindBrazoIzquierdo);
            Console.WriteLine("Blindaje torso izq: "+_BlindTorsoIzquierdo);
            Console.WriteLine("Blindaje pierna izq: "+_BlindPiernaIzquierda);
            Console.WriteLine("Blindaje pierna drcha: "+_BlindPiernaDerecha);
            Console.WriteLine("Blindaje torso drcha: "+_BlindTorsoDerecho);
            Console.WriteLine("Blindaje brazo drcha: "+_BlindBrazoDerecho);
            Console.WriteLine("Blindaje torso central: "+_BlindTorsoCentral);
            Console.WriteLine("Blindaje cabeza: "+_BlindCabeza);
            Console.WriteLine("Blindaje torso atras izq: "+_BlindAtrasTorsoIzquierdo);
            Console.WriteLine("Blindaje torso atras drcha: "+_BlindAtrasTorsoDerecho);
            Console.WriteLine("Blindaje torso central atras: "+_BlindAtrasTorsoCentral);
            //Console.WriteLine("Blindaje total: " + _BlindTotal);
            estadoBlindajeMech();

            Console.WriteLine(); Console.WriteLine();
		}

        /// <summary>
        /// Metodo que calcula la media de la distancia por armas del mech
        /// </summary>
        private void calculoDistanciaTiro()
        {
            int media = 0, larga = 0, corta = 0;
            _maxAlcanceDisparo = 0;
            for (int i = 0; i < _armas.Count; i++)
            {
                corta += ((Componente)_armas[i]).distanciaCorta();
                media += ((Componente)_armas[i]).distanciaMedia();
                larga += ((Componente)_armas[i]).distanciaLarga();
                if ( ((Componente)_armas[i]).distanciaLarga() > _maxAlcanceDisparo) _maxAlcanceDisparo = ((Componente)_armas[i]).distanciaLarga();

            }

            _distanciaTiroCorta = corta / _armas.Count;
            _distanciaTiroMedia = media / _armas.Count;
            _distanciaTiroLarga = larga / _armas.Count;
        }

        #region Funciones para componentes
        /// <summary>
        /// Metodo que indica si un arma tiene municion para poder ser disparada
        /// </summary>
        /// <param name="arma">Componente del que se desea saber si tiene municion</param>
        /// <returns>True en caso de poseer municion. False en caso opuesto.</returns>
        public bool tieneMunicion(Componente arma)
        {
            bool municion = false;
            if ( arma.energia() || arma.tipoArma() == "Nada")
            {
                municion = true;
            }
            else
            {
                for (int i = 0; i < _componentes.Length && !municion; i++)
                {
                    if (_componentes[i].clase() == "MUNICION" && _componentes[i].municionPara() == arma.codigo() && _componentes[i].cantidadMunicion() > 0 && _componentes[i].operativo() ) {
                        municion = true;
                    }

                }
            }
            return municion;
        }

        /// <summary>
        /// Metodo que devuelve el slot donde esta hubicado un componente de tipo Arma
        /// </summary>
        /// <param name="c">Componente de tipo arma del que se desea saber el slot</param>
        /// <returns>Entero correspondiente al slot</returns>
        public int slotArma (Componente c) {

            LocalizacionMech l = _slots[c.localizacionINT()];
            int slot=0 ;
            Boolean salir = false;
            Slot[] slots = l.slots();

            for (int i = 0; i < slots.Length && !salir ; i++)
                if (c.codigo() == slots[i].codigo())
                {
                    slot = i;
                    salir = true;
                }
            return slot;
        }

        /// <summary>
        /// Metodo que indica el slot donde esta hubicada la municion del arma pasada como argumento
        /// </summary>
        /// <param name="arma">Componente del tipo arma a la que hay que encontrar el slot de la municion</param>
        /// <returns>Entero correspondiente al slot</returns>
        public int slotMunicion(Componente arma) {
            int codigo = 0, localizacion = 0, slot = 0;
            Boolean salir = false;

            if (arma.energia() || arma.tipoArma() == "Nada")
            {
                slot = -1;
            }
            else
            {
                //Buscamos la localizacion y el codigo de la municion
                for (int i = 0; i < _componentes.Length && !salir; i++)
                {
                    if (_componentes[i].clase() == "MUNICION" && _componentes[i].municionPara() == arma.codigo() && _componentes[i].cantidadMunicion() > 0 && _componentes[i].operativo())
                    {
                        localizacion = _componentes[i].localizacionINT();
                        codigo = _componentes[i].codigo();
                        salir = true;
                    }
                }

                //Buscamos el slot
                LocalizacionMech l = _slots[localizacion];
                Slot[] slots = l.slots();

                salir = false;
                //Console.WriteLine("Localizacion "+localizacion+" codigo:"+codigo);
                for (int i = 0; i < slots.Length && !salir; i++)
                {
                    //Console.WriteLine("Slot "+i+": "+slots[i].codigo());
                    if (codigo == slots[i].codigo())
                    {
                        slot = i;
                        salir = true;
                    }
                }
            }

            return slot;
        }

        /// <summary>
        /// Metodo que nos indica la localizacion donde esta hubicada la municion del arma pasada como argumento
        /// </summary>
        /// <param name="arma">Componente del tipo arma a la que hay que encontrar la localizacion de la municion</param>
        /// <returns>Cadena que indica la localizacion de la municion</returns>
        public String localizacionMunicion(Componente arma) {
            Boolean salir = false;
            String localizacion="";
            if (arma.energia() || arma.tipoArma() == "Nada")
            {
                localizacion = "-1";
            }
            else
            {
                for (int i = 0; i < _componentes.Length && !salir; i++)
                {
                    if (_componentes[i].clase() == "MUNICION" && _componentes[i].municionPara() == arma.codigo() && _componentes[i].cantidadMunicion() > 0 && _componentes[i].operativo())
                    {
                        localizacion = _componentes[i].localizacionSTRING();
                        salir = true;
                    }
                }
            }

            return localizacion;
        }

        #endregion

        /// <summary>
        /// Metodo que devuelve un la puntuacion asociada al tipo de Mech en funcion de su tonelaje.
        /// La nota sera la siguiente:
        /// - Ligero: 3.0
        /// - Medio: 5.0
        /// - Pesado: 7.0
        /// - Asalto: 10.0
        /// </summary>
        /// <returns>Flotante correspondiente a la puntuacion</returns>
        public float tipo()
        {
            float t;
            
            if (_toneladas >= 20 && _toneladas <= 35)
            {
                t = 3.0f; //ligero
            }
            else if (_toneladas >= 40 && _toneladas <= 55)
            {
                t = 5.0f;//medio
            }
            else if (_toneladas >= 60 && _toneladas <= 75)
            {
                t = 7.0f;//pesado
            }
            else
                t = 10.0f;//asalto

            return t;
        }

        #region Metodos de los conosVision
        /// <summary>
        /// Metodo que indica si una posicion esta dentro del cono delantero de vision de un mech
        /// </summary>
        /// <param name="casilla">Posicion a observar</param>
        /// <param name="encaramiento">Encaramiento hacia donde estamos mirando</param>
        /// <returns>True en caso de estar en el cono delantero, false en caso opuesto</returns>
        public Boolean conoDelantero( Posicion casilla , int encaramiento ) 
        {
            Boolean enCono = false;
            int jIzq, jDrcha;
            //Console.WriteLine(casilla.columna() + " "+casilla.fila());
            //Console.WriteLine(encaramiento);
            //Calculamos las diagonales del cono
            jIzq = _posicion.columna() - Math.Abs(casilla.fila() - _posicion.fila()) * 2;
            jDrcha = _posicion.columna() + Math.Abs(casilla.fila() - _posicion.fila()) * 2;
            if (jIzq <= 0) jIzq = 1;


            //Si tiene el encaramiento 1
            if (encaramiento == 1)
            {
                if (casilla.fila() > _posicion.fila()) {//Si esta mas abajo que la casilla donde me encuentro
                    enCono = false;
                }
                else {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_posicion.columna() % 2 == 0) {//posicion pares
                        if ((casilla.columna() >= jIzq || casilla.columna() >= jIzq - 1) && (casilla.columna() <= jDrcha || casilla.columna() <= jDrcha + 1))
                            enCono = true;
                    }
                    else {//posicion impar
                        if ((casilla.columna() >= jIzq || casilla.columna() >= jIzq + 1) && (casilla.columna() <= jDrcha || casilla.columna() <= jDrcha - 1))
                            enCono = true;
                    }
                }


            }//Si tiene el encaramiento 2
            else if (encaramiento == 2)
            {
                if (casilla.columna() < _posicion.columna())
                {//Si esta mas a la izquierda que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() <= _posicion.fila()) {
                    //Vemos esta en la zona superior
                    enCono = true;
                }
                else {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_posicion.columna() % 2 != 0)
                    {//posicion impares
                        if (casilla.columna() >= jDrcha || casilla.columna() >= jDrcha + 1)
                            enCono = true;
                    }
                    else
                    {//posicion pares
                        if (casilla.columna() >= jDrcha || casilla.columna() >= jDrcha - 1)
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 3
            else if (encaramiento == 3)
            {
                if (casilla.columna() < _posicion.columna())
                {//Si esta mas a la izquierda que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() >= _posicion.fila())
                {
                    //Vemos esta en la zona inferior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_posicion.columna() % 2 == 0)
                    {//posicion pares
                        if (casilla.columna() >= jDrcha || casilla.columna() >= jDrcha + 1)
                            enCono = true;
                    }
                    else
                    {//posicion impar
                        if  (casilla.columna() >= jDrcha || casilla.columna() >= jDrcha - 1)
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 4
            else if (encaramiento == 4)
            {
                if (casilla.fila() < _posicion.fila()) {
                    //Si esta mas arriba que la casilla donde me encuentro
                    enCono = false;
                }
                else {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_posicion.columna() % 2 != 0)
                    {//posicion impares
                        if ((casilla.columna() >= jIzq || casilla.columna() >= jIzq - 1) && (casilla.columna() <= jDrcha || casilla.columna() <= jDrcha + 1))
                            enCono = true;
                    }
                    else
                    {//posicion pares
                        if ((casilla.columna() >= jIzq || casilla.columna() >= jIzq + 1) && (casilla.columna() <= jDrcha || casilla.columna() <= jDrcha - 1))
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 5
            else if (encaramiento == 5)
            {
                if (casilla.columna() > _posicion.columna())
                {//Si esta mas a la derecha que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() >= _posicion.fila())
                {
                    //Vemos esta en la zona inferior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_posicion.columna() % 2 == 0)
                    {//posicion pares
                        if ((casilla.columna() <= jIzq || casilla.columna() <= jIzq - 1))
                            enCono = true;
                    }
                    else
                    {//posicion impar
                        if ((casilla.columna() <= jIzq || casilla.columna() <= jIzq + 1))
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 6
            else if (encaramiento == 6)
            {
                if (casilla.columna() > _posicion.columna())
                {//Si esta mas a la derecha que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() <= _posicion.fila())
                {
                    //Vemos esta en la zona superior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_posicion.columna() % 2 != 0)
                    {//posicion impares
                        if (casilla.columna() <= jIzq || casilla.columna() <= jIzq - 1)
                            enCono = true;
                    }
                    else
                    {//posicion pares
                        if (casilla.columna() <= jIzq || casilla.columna() <= jIzq + 1)
                            enCono = true;
                    }
                }

            }

            return enCono;
        }

        /// <summary>
        /// Metodo que indica si una posicion esta dentro del cono trasero de vision de un mech
        /// </summary>
        /// <param name="casilla">Posicion a observar</param>
        /// <param name="encaramiento">Encaramiento hacia donde estamos mirando</param>
        /// <returns>True en caso de estar en el cono delantero, false en caso opuesto</returns>
        public Boolean conoTrasero(Posicion casilla, int encaramiento)
        {
            Boolean enCono = false;
            int jIzq, jDrcha;
            //Console.WriteLine(casilla.columna() + " "+casilla.fila());
            //Console.WriteLine(encaramiento);
            //Calculamos las diagonales del cono
            jIzq = _posicion.columna() - Math.Abs(casilla.fila() - _posicion.fila()) * 2;
            jDrcha = _posicion.columna() + Math.Abs(casilla.fila() - _posicion.fila()) * 2;
            if (jIzq <= 0) jIzq = 1;


            //Si tiene el encaramiento 1
            if (encaramiento == 1)
            {
                if (casilla.fila() < _posicion.fila())
                {
                    //Si esta mas arriba que la casilla donde me encuentro
                    enCono = false;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_posicion.columna() % 2 != 0)
                    {//posicion impares
                        if (casilla.columna() > jIzq && casilla.columna() < jDrcha)
                            enCono = true;
                    }
                    else
                    {//posicion pares
                        if ( casilla.columna() > jIzq + 1 && casilla.columna() < jDrcha - 1)
                            enCono = true;
                    }
                }


            }//Si tiene el encaramiento 2
            else if (encaramiento == 2)
            {
                if (casilla.columna() >= _posicion.columna())
                {//Si esta mas a la derecha que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() > _posicion.fila())
                {
                    //Vemos esta en la zona inferior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_posicion.columna() % 2 == 0)
                    {//posicion pares
                        if ( casilla.columna() < jIzq - 1)
                            enCono = true;
                    }
                    else
                    {//posicion impar
                        if (casilla.columna() < jIzq )
                            enCono = true;
                    }
                } 
            }//Si tiene el encaramiento 3
            else if (encaramiento == 3)
            {
                if (casilla.columna() >= _posicion.columna())
                {//Si esta mas a la derecha que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() < _posicion.fila())
                {
                    //Vemos esta en la zona superior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_posicion.columna() % 2 != 0)
                    {//posicion impares
                        if ( casilla.columna() < jIzq - 1)
                            enCono = true;
                    }
                    else
                    {//posicion pares
                        if (casilla.columna() < jIzq )
                            enCono = true;
                    }
                } 

            }//Si tiene el encaramiento 4
            else if (encaramiento == 4)
            {
                if (casilla.fila() > _posicion.fila())
                {//Si esta mas abajo que la casilla donde me encuentro
                    enCono = false;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_posicion.columna() % 2 == 0)
                    {//posicion pares
                        if (casilla.columna() > jIzq && casilla.columna() < jDrcha )
                            enCono = true;
                    }
                    else
                    {//posicion impar
                        if ( casilla.columna() > jIzq + 1 && casilla.columna() < jDrcha - 1)
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 5
            else if (encaramiento == 5)
            {
                if (casilla.columna() <= _posicion.columna())
                {//Si esta mas a la izquierda que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() < _posicion.fila())
                {
                    //Vemos esta en la zona superior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_posicion.columna() % 2 != 0)
                    {//posicion impares
                        if ( casilla.columna() > jDrcha + 1)
                            enCono = true;
                    }
                    else
                    {//posicion pares
                        if (casilla.columna() > jDrcha )
                            enCono = true;
                    }
                }

            }//Si tiene el encaramiento 6
            else if (encaramiento == 6)
            {
                if (casilla.columna() <= _posicion.columna())
                {//Si esta mas a la izquierda que la casilla donde me encuentro
                    enCono = false;
                }
                else if (casilla.fila() > _posicion.fila())
                {
                    //Vemos esta en la zona inferior
                    enCono = true;
                }
                else
                {
                    //Vemos si la casilla a observar esta dentro de esos limites
                    if (_posicion.columna() % 2 == 0)
                    {//posicion pares
                        if ( casilla.columna() > jDrcha + 1)
                            enCono = true;
                    }
                    else
                    {//posicion impar
                        if (casilla.columna() > jDrcha )
                            enCono = true;
                    }
                }
            }

            return enCono;
        }

        /// <summary>
        /// Metodo que indica si una posicion esta dentro del cono derecho de vision de un mech
        /// </summary>
        /// <param name="casilla">Posicion a observar</param>
        /// <param name="encaramiento">Encaramiento hacia donde estamos mirando</param>
        /// <returns>True en caso de estar en el cono delantero, false en caso opuesto</returns>
        public Boolean conoDerecho(Posicion casilla, int encaramiento) 
        {
            Boolean enCono = false;

            if (!conoDelantero(casilla, encaramiento) && !conoTrasero(casilla, encaramiento)) {
                if (encaramiento == 1) {
                    if (_posicion.columna() < casilla.columna())
                        enCono = true;
                }
                else if (encaramiento == 2) {
                    if (_posicion.fila() < casilla.fila())
                        enCono = true;
                }
                else if (encaramiento == 3) {
                    if (_posicion.fila() < casilla.fila())
                        enCono = true;
                }
                else if (encaramiento == 4) {
                    if (_posicion.columna() > casilla.columna())
                        enCono = true;
                }
                else if (encaramiento == 5) {
                    if (_posicion.fila() > casilla.fila())
                        enCono = true;
                }
                else {
                    if (_posicion.fila() > casilla.fila())
                        enCono = true;
                }  
            
            }

            return enCono;
        }

        /// <summary>
        /// Metodo que indica si una posicion esta dentro del cono izquierdo de vision de un mech
        /// </summary>
        /// <param name="casilla">Posicion a observar</param>
        /// <param name="encaramiento">Encaramiento hacia donde estamos mirando</param>
        /// <returns>True en caso de estar en el cono delantero, false en caso opuesto</returns>
        public Boolean conoIzquierdo(Posicion casilla, int encaramiento)
        {
            Boolean enCono = false;

            if (!conoDelantero(casilla, encaramiento) && !conoTrasero(casilla, encaramiento) && !conoDerecho(casilla,encaramiento) )
            {
                enCono = true;
            }

            return enCono;
        }

        #endregion


        #region Metodos del estado del Mech

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre el blindaje del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoBlindajeMech() 
        {
            //tantos por ciento
            float TC = 0.15f, CAB = 0.15f, 
                BD = 0.08f, BI = 0.08f, ATD = 0.08f, ATI = 0.08f, ATC = 0.08f, 
                TI = 0.1f, TD = 0.1f, 
                PI = 0.05f, PD = 0.05f;
            float estado = 0;

            //Vemos los datos de las diferentes partes
            estado = estadoBlindajeBI() * BI +
                estadoBlindajeTI() * TI +
                estadoBlindajePI() * PI +
                estadoBlindajePD() * PD +
                estadoBlindajeTD() * TD +
                estadoBlindajeBD() * BD +
                estadoBlindajeTC() * TC +
                estadoBlindajeCAB() * CAB +
                estadoBlindajeATI() * ATI +
                estadoBlindajeATD() * ATD +
                estadoBlindajeATC() * ATC;

            return estado;
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre el blindaje del BI del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoBlindajeBI()
        {
            return (_BlindBrazoIzquierdo * 10 / _BlindBrazoIzquierdoInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre el blindaje del TI del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoBlindajeTI() 
        {
            return (_BlindTorsoIzquierdo * 10 / _BlindTorsoIzquierdoInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre el blindaje del PI del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoBlindajePI() 
        {
            return (_BlindPiernaIzquierda * 10 / _BlindPiernaIzquierdaInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre el blindaje del PD del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoBlindajePD() 
        {
            return (_BlindPiernaDerecha * 10 / _BlindPiernaDerechaInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre el blindaje del TD del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoBlindajeTD()
        {
            return (_BlindTorsoDerecho * 10 / _BlindTorsoDerechoInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre el blindaje del BD del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoBlindajeBD()
        {
            return (_BlindBrazoDerecho * 10 / _BlindBrazoDerechoInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre el blindaje del TC del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoBlindajeTC()
        {
            return (_BlindTorsoCentral * 10 / _BlindTorsoCentralInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre el blindaje del CAB del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoBlindajeCAB()
        {
            return (_BlindCabeza * 10 / _BlindCabezaInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre el blindaje del ATI del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoBlindajeATI()
        {
            return (_BlindAtrasTorsoIzquierdo * 10 / _BlindAtrasTorsoIzquierdoInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre el blindaje del ATD del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoBlindajeATD()
        {
            return (_BlindAtrasTorsoDerecho * 10 / _BlindAtrasTorsoDerechoInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre el blindaje del ATC del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoBlindajeATC()
        {
            return (_BlindAtrasTorsoCentral * 10 / _BlindAtrasTorsoCentralInicial);
        }

        
        //ESTADO ESTRUCTURA INTERNA
        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre la estructura interna del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoEsctructuraMech()
        {
            //tantos por ciento
            float TC = 0.2f, CAB = 0.2f,
                BD = 0.1f, BI = 0.1f,
                TI = 0.15f, TD = 0.15f,
                PI = 0.05f, PD = 0.05f;
            float estado = 0;

            //Vemos los datos de las diferentes partes
            estado = estadoEsctructuraBI() * BI +
                estadoEsctructuraTI() * TI +
                estadoEsctructuraPI() * PI +
                estadoEsctructuraPD() * PD +
                estadoEsctructuraTD() * TD +
                estadoEsctructuraBD() * BD +
                estadoEsctructuraTC() * TC +
                estadoEsctructuraCAB() * CAB;

            return estado;
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre la estructura interna del BI del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoEsctructuraBI()
        {
            return (_EstrucBrazoIzquierdo * 10 / _EstrucBrazoIzquierdoInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre la estructura interna del TI del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoEsctructuraTI()
        {
            return (_EstrucTorsoIzquierdo * 10 / _EstrucTorsoIzquierdoInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre la estructura interna del PI del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoEsctructuraPI()
        {
            return (_EstrucPiernaIzquierda * 10 / _EstrucPiernaIzquierdaInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre la estructura interna del PD del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoEsctructuraPD()
        {
            return (_EstrucPiernaDerecha * 10 / _EstrucPiernaDerechaInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre la estructura interna del TD del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoEsctructuraTD()
        {
            return (_EstrucTorsoDerecho * 10 / _EstrucTorsoDerechoInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre la estructura interna del BD del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoEsctructuraBD()
        {
            return (_EstrucBrazoDerecho * 10 / _EstrucBrazoDerechoInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre la estructura interna del TC del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoEsctructuraTC()
        {
            return (_EstrucTorsoCentral * 10 / _EstrucTorsoCentralInicial);
        }

        /// <summary>
        /// Metodo que devuelve una nota entre 0-10 sobre la estructura interna del CAB del Mech
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float estadoEsctructuraCAB()
        {
            return (_EstrucCabeza * 10 / _EstrucCabezaInicial);
        }

        #endregion


        /// <summary>
        /// Metodo que indica la nota global del estado del Mech. Comprendida entre 0-10
        /// </summary>
        /// <returns>Flotante que indica la nota</returns>
        public float notaEstado()
        {
            //porcentajes
            float Blindaje = 0.4f, Estructura = 0.2f, Tipo = 0.4f;
            float nota;

            if (_notaMech == -1) //Si no ha sido calculada con anterioridad
            {
                nota = estadoBlindajeMech() * Blindaje +
                   estadoEsctructuraMech() * Estructura +
                   tipo() * Tipo;
                _notaMech = nota;
            }
            else
                nota = _notaMech;

            return nota;
        }


        /// <summary>
        /// Metodo que calcula, para el arma pasada como argumento, la nota de tirada aproximada que se necesita para impactar al objetivo
        /// </summary>
        /// <param name="arma">Componente de tipo arma que se desea disparar</param>
        /// <param name="objetivo">Mech al cual se va a disparar</param>
        /// <param name="mapa">Tablero del juego</param>
        /// <param name="movimientoPropio">Cadena que indica el tipo de movimiento realizado por el mech atacante</param>
        /// <param name="movimientoObjetivo">Cadena que indica el tipo de movimiento realizado por el mech objetivo</param>
        /// <returns>Entero correspondiente a la puntuacion calculada</returns>
        public int tiradaImpacto(Componente arma, Mech objetivo, Tablero mapa, String movimientoPropio = "Andar", String movimientoObjetivo = "Andar")
        {
            int puntuacion = 0, distancia = _posicion.distancia(objetivo.posicion());

            //Añadimos el modificador de distancia
            if (distancia < arma.distanciaCorta())
            {
                puntuacion += 4;
            }
            else if (distancia < arma.distanciaMedia())
            {
                puntuacion += 6;
            }
            else
                puntuacion += 8;

            //Console.WriteLine("Distancia: " + puntuacion);
            //El modificador de distancia minima no lo tenemos en cuenta puesto que las armas elegidas como condicion no estarna en 
            //distancia minima

            //Modificador de movimiento atacante
            switch (movimientoPropio)
            {
                case "Inmovil":
                    puntuacion += 0;
                    break;
                case "Andar":
                    puntuacion += 1;
                    break;
                case "Correr":
                    puntuacion += 2;
                    break;
                case "Saltar":
                    puntuacion += 3;
                    break;
                default:
                    break;
            }
            //Console.WriteLine("Movimiento propio: " + puntuacion);
            //Modificadores atacante en suelo
            if (_enSuelo) puntuacion += 2;
            //Console.WriteLine("En suelo: " + puntuacion);
            //Modificador de movimiento objetivo
            switch (movimientoObjetivo)
            {
                case "Inmovil":
                    puntuacion -= 4;
                    break;
                case "Andar":
                    puntuacion += 1;
                    break;
                case "Correr":
                    puntuacion += 2;
                    break;
                case "Saltar":
                    puntuacion += 3;
                    break;
                default:
                    break;
            }
            //Console.WriteLine("Movimiento objetivo: " + puntuacion);
            //Modificadores objetivo en suelo
            if (objetivo.enSuelo() && distancia == 1)
            {
                puntuacion -= 2;
            }
            else if (objetivo.enSuelo())
                puntuacion += 1;
            //Console.WriteLine("Objetivo en suelo: " + puntuacion);
            //Modificadores del terreno
            int tipoTerreno = mapa.Casilla(objetivo.posicion()).objetoTerreno();
            switch (tipoTerreno)
            {
                case 1://bosque ligero
                    puntuacion += 1;
                    break;
                case 2://bosque denso
                    puntuacion += 2;
                    break;
                default:
                    break;
            }
            //si el terreno es agua
            if (mapa.Casilla(objetivo.posicion()).tipoTerreno() == 2)
            {
                if (mapa.Casilla(objetivo.posicion()).nivel() == -1)
                {
                    puntuacion -= 1;
                }
                else if (mapa.Casilla(objetivo.posicion()).nivel() >= 2)
                    puntuacion = 100;

            }
            //Console.WriteLine("Terreno: " + puntuacion);
            //Modificadores de calor
            if (_nivelTemp >= 0 && _nivelTemp <= 7)
            {
                puntuacion += 0;
            }
            else if (_nivelTemp >= 8 && _nivelTemp <= 12)
            {
                puntuacion += 1;
            }
            else if (_nivelTemp >= 13 && _nivelTemp <= 16)
            {
                puntuacion += 2;
            }
            else if (_nivelTemp >= 17 && _nivelTemp <= 23)
            {
                puntuacion += 3;
            }
            else // >24
                puntuacion += 4;
            //Console.WriteLine("Calor: " + puntuacion);
            /** MODIFICADORES PARA DISPARO
             * modificador distancia
             * " alcance minimo
             * " movimiento atacante
             * " movimiento objetivo
             * " terreno
             * " calor y daños
             * " objetivos inmoviles
             * " en suelo
             */

            //bonus
            puntuacion += 4;
            Console.WriteLine("Arma: " + arma.nombre() + " tirada para impacto:" + puntuacion);
            return puntuacion;
        }
        #endregion
    }

}
