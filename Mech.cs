
using System;
using System.IO;
using System.Collections;

namespace ico {
	
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
				
	//METODOS GET ESTADO
		public int numeroJ() { return _numeroJ; }
		public Boolean operativo() { return _operativo; }
		public Boolean desconectado(){return _desconectado; }
		public Boolean atascado(){ return _atascado; }
		public Boolean enSuelo(){ return _enSuelo; }
		public Posicion posicion() { return _posicion; }
		public int posicionFila() { return _posicion.fila();} //POSICIONES
		public int posicionColumna() { return _posicion.columna();}
		public int ladoEncaramiento() { return _ladoEncaramiento; }
		public int ladoEncaramientoTorso() { return _ladoEncaramientoTorso; }
		public int nivelTemp() { return _nivelTemp; }
		public Boolean ardiendo() { return _ardiendo; }
		public Boolean garrote() { return _garrote; }
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
		
	//METODOS GET DEFINICION
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
		/*public LocalizacionMech slotsBrazoIzquierdo() { return _slotsBrazoIzquierdo; }
		public LocalizacionMech slotsTorsoIzquierdo() { return _slotsTorsoIzquierdo; }
		public LocalizacionMech slotsPiernaIzquierda() { return _slotsPiernaIzquierda; }
		public LocalizacionMech slotsPiernaDerecha() { return _slotsPiernaDerecha; }
		public LocalizacionMech slotsTorsoDerecho() { return _slotsTorsoDerecho; }
		public LocalizacionMech slotsBrazoDerecho() { return _slotsBrazoDerecho; }
		public LocalizacionMech slotsTorsoCentral() { return _slotsTorsoCentral; }
		public LocalizacionMech slotsCabeza() { return _slotsCabeza; }*/

	/*	public int andarDefinicion() { return _andarDefinicion; }
		public int correrDefinicion() { return _correrDefinicion; }
		public int saltarDefinicion() { return _saltarDefinicion; } */
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

        /*
         * Metodo que calcula la media de la distancia por armas del mech
         */
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
        /*
         * Indica si un componente tipo arma tiene municion para ser disparada 
         */
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

        /*
         * indica el tipo mech
         */
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

        #region conosVision
        /*
         * Indica si una posicion esta dentro del cono delantero de vision de un mech
         */
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

        /*
         * Indica si una posicion esta dentro del cono trasero de vision de un mech
         */
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

        /*
         * Indica si una posicion esta dentro del cono derecho de vision de un mech
         */
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

        /*
         * Indica si una posicion esta dentro del cono izquierdo de vision de un mech
         */
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

        //METODOS PARA VER LA SITUACION DEL MECH
        /*public situacion situacionMech() {
            situacion e;

            if ( _desconectado ) {
                e = situacion.Desconectado;
            }
            else if (_EstrucBrazoIzquierdo == 0 || _EstrucBrazoDerecho == 0 || _EstrucPiernaDerecha == 0 || _EstrucPiernaIzquierda == 0) {
                //Le falta alguna extremidad
                e = situacion.Incapacitado;
            }
            else if (_atascado) {
                e = situacion.Atascado;
            }
            else if (_enSuelo) {
                e = situacion.Tumbado;
            }
            else
                e = situacion.Activo;

            return e;
        }*/


        //METODOS PARA VER EL ESTADO DEL MECH
        #region estadoMech

        //ESTADO BLINDAJE
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

            //Console.WriteLine("Estado del blindaje es: " + estado);

            return estado;
        }

        public float estadoBlindajeBI()
        {
            float heridas = _BlindBrazoIzquierdo * 10 / _BlindBrazoIzquierdoInicial;
            return heridas;
        }

        public float estadoBlindajeTI() 
        {
            float heridas = _BlindTorsoIzquierdo * 10 / _BlindTorsoIzquierdoInicial;

            //Console.WriteLine("estado torso izq " + heridas);
            return heridas;
        }

        public float estadoBlindajePI() 
        {
            float heridas = _BlindPiernaIzquierda * 10 / _BlindPiernaIzquierdaInicial;

            //Console.WriteLine("estado pierna izq " + heridas);
            return heridas;
        }

        public float estadoBlindajePD() 
        {
            float heridas = _BlindPiernaDerecha * 10 / _BlindPiernaDerechaInicial;

            //Console.WriteLine("estado pierna drcha " + heridas);
            return heridas;
        }

        public float estadoBlindajeTD()
        {
            float heridas = _BlindTorsoDerecho * 10 / _BlindTorsoDerechoInicial;

            //Console.WriteLine("estado torso drcha " + heridas);
            return heridas;
        }

        public float estadoBlindajeBD()
        {
            float heridas = _BlindBrazoDerecho * 10 / _BlindBrazoDerechoInicial;

            //Console.WriteLine("estado brazo drcha " + heridas);
            return heridas;
        }

        public float estadoBlindajeTC()
        {
            float heridas = _BlindTorsoCentral * 10 / _BlindTorsoCentralInicial;

            //Console.WriteLine("estado torso central " + heridas);
            return heridas;
        }

        public float estadoBlindajeCAB()
        {
            float heridas = _BlindCabeza * 10 / _BlindCabezaInicial;

            //Console.WriteLine("estado cabeza " + heridas);
            return heridas;
        }

        public float estadoBlindajeATI()
        {
            float heridas = _BlindAtrasTorsoIzquierdo * 10 / _BlindAtrasTorsoIzquierdoInicial;

            //Console.WriteLine("estado atras izq " + heridas);
            return heridas;
        }

        public float estadoBlindajeATD()
        {
            float heridas = _BlindAtrasTorsoDerecho * 10 / _BlindAtrasTorsoDerechoInicial;

            //Console.WriteLine("estado atras drcha " + heridas);
            return heridas;
        }

        public float estadoBlindajeATC()
        {
            int heridas = _BlindAtrasTorsoCentral * 10 / _BlindAtrasTorsoCentralInicial;

            //Console.WriteLine("estado atras central " + heridas);
            return heridas;
        }

        
        //ESTADO ESTRUCTURA INTERNA
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

            //Console.WriteLine("Estado de la estructura interna es: " + estado);

            return estado;
        }

        public float estadoEsctructuraBI()
        {
            float heridas = _EstrucBrazoIzquierdo * 10 / _EstrucBrazoIzquierdoInicial;

            //Console.WriteLine("Estado brazo izq " + heridas);
            return heridas;
        }

        public float estadoEsctructuraTI()
        {
            float heridas = _EstrucTorsoIzquierdo * 10 / _EstrucTorsoIzquierdoInicial;

            //Console.WriteLine("estado torso izq " + heridas);
            return heridas;
        }

        public float estadoEsctructuraPI()
        {
            float heridas = _EstrucPiernaIzquierda * 10 / _EstrucPiernaIzquierdaInicial;

            //Console.WriteLine("estado pierna izq " + heridas);
            return heridas;
        }

        public float estadoEsctructuraPD()
        {
            float heridas = _EstrucPiernaDerecha * 10 / _EstrucPiernaDerechaInicial;

            //Console.WriteLine("estado pierna drcha " + heridas);
            return heridas;
        }

        public float estadoEsctructuraTD()
        {
            float heridas = _EstrucTorsoDerecho * 10 / _EstrucTorsoDerechoInicial;

            //Console.WriteLine("estado torso drcha " + heridas);
            return heridas;
        }

        public float estadoEsctructuraBD()
        {
            float heridas = _EstrucBrazoDerecho * 10 / _EstrucBrazoDerechoInicial;

            //Console.WriteLine("estado brazo drcha " + heridas);
            return heridas;
        }

        public float estadoEsctructuraTC()
        {
            float heridas = _EstrucTorsoCentral * 10 / _EstrucTorsoCentralInicial;

            //Console.WriteLine("estado torso central " + heridas);
            return heridas;
        }

        public float estadoEsctructuraCAB()
        {
            float heridas = _EstrucCabeza * 10 / _EstrucCabezaInicial;

            //Console.WriteLine("estado cabeza " + heridas);
            return heridas;
        }

        #endregion

        /*
         * Indica una nota con el estado del mech 0-10
         */
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

        #endregion

        #region PENDIENTES

        /*
         * Metodo que calcula, para el arma pasada como argumento, la media de tirada que se necesita para impactar al objetivo
         */
        public int tiradaImpacto(Componente arma, Mech objetivo, Tablero mapa, String movimientoPropio = "Andar", String movimientoObjetivo = "Andar")
        {
            int puntuacion = 0 , distancia= _posicion.distancia(objetivo.posicion()) ;

            //A�adimos el modificador de distancia
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
            switch (movimientoPropio) {
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
            if (mapa.Casilla(objetivo.posicion()).tipoTerreno() == 2) {
                if (mapa.Casilla(objetivo.posicion()).nivel() == -1)
                {
                    puntuacion -= 1;
                }else if (mapa.Casilla(objetivo.posicion()).nivel() >= 2 )
                    puntuacion = 100;
            
            }
            //Console.WriteLine("Terreno: " + puntuacion);
            //Modificadores de calor
            if ( _nivelTemp >= 0 && _nivelTemp <= 7)
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
             * " calor y da�os
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
