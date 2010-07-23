
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
        protected Boolean _atascado;
		protected Boolean _enSuelo;
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
		protected int _tonelados;
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
		
		protected int _BlindBrazoIzquierdoDefinicion;
		protected int _BlindTorsoIzquierdoDefinicion;
		protected int _BlindPiernaIzquierdaDefinicion;
		protected int _BlindPiernaDerechaDefinicion;
		protected int _BlindTorsoDerechoDefinicion;
		protected int _BlindBrazoDerechoDefinicion;
		protected int _BlindTorsoCentralDefinicion;
		protected int _BlindCabezaDefinicion;
		protected int _BlindAtrasTorsoIzquierdoDefinicion;
		protected int _BlindAtrasTorsoDerechoDefinicion;
		protected int _BlindAtrasTorsoCentralDefinicion;
		
		protected int _EstrucBrazoIzquierdoDefinicion;
		protected int _EstrucTorsoIzquierdoDefinicion;
		protected int _EstrucPiernaIzquierdaDefinicion;
		protected int _EstrucPiernaDerechaDefinicion;
		protected int _EstrucTorsoDerechoDefinicion;
		protected int _EstrucBrazoDerechoDefinicion;
		protected int _EstrucTorsoCentralDefinicion;
		protected int _EstrucCabezaDefinicion;
		
		protected int _numeroComponentes;
		protected Componente[] _componentes;
		
		protected int _numeroArmas;
		protected ArrayList _armas;
		protected int _numeroActuadores;
		protected Actuador[] _actuadores;
		
		protected LocalizacionMech _slotsBrazoIzquierdo;
		protected LocalizacionMech _slotsTorsoIzquierdo;
		protected LocalizacionMech _slotsPiernaIzquierda;
		protected LocalizacionMech _slotsPiernaDerecha;
		protected LocalizacionMech _slotsTorsoDerecho;
		protected LocalizacionMech _slotsBrazoDerecho;
		protected LocalizacionMech _slotsTorsoCentral;
		protected LocalizacionMech _slotsCabeza;
        protected int _andarDefinicion; //***
        protected int _correrDefinicion; //***
        protected int _saltarDefinicion; //***
        protected int _tipoRadiador; //***

        protected int _distanciaTiroCorta; //***
        protected int _distanciaTiroMedia; //***
        protected int _distanciaTiroLarga; //***
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

        #region lecturaficheros
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
			_BlindTorsoIzquierdo=Convert.ToInt32(f.ReadLine());
			_BlindPiernaIzquierda=Convert.ToInt32(f.ReadLine());
			_BlindPiernaDerecha=Convert.ToInt32(f.ReadLine());
			_BlindTorsoDerecho=Convert.ToInt32(f.ReadLine());
			_BlindBrazoDerecho=Convert.ToInt32(f.ReadLine());
			_BlindTorsoCentral=Convert.ToInt32(f.ReadLine());
			_BlindCabeza=Convert.ToInt32(f.ReadLine());
			_BlindAtrasTorsoIzquierdo=Convert.ToInt32(f.ReadLine());
			_BlindAtrasTorsoDerecho=Convert.ToInt32(f.ReadLine());
			_BlindAtrasTorsoCentral=Convert.ToInt32(f.ReadLine());
			
			//datos esctructura interna
			_EstrucBrazoIzquierdo=Convert.ToInt32(f.ReadLine());
			_EstrucTorsoIzquierdo=Convert.ToInt32(f.ReadLine());
			_EstrucPiernaIzquierda=Convert.ToInt32(f.ReadLine());
			_EstrucPiernaDerecha=Convert.ToInt32(f.ReadLine());
			_EstrucTorsoDerecho=Convert.ToInt32(f.ReadLine());
			_EstrucBrazoDerecho=Convert.ToInt32(f.ReadLine());
			_EstrucTorsoCentral=Convert.ToInt32(f.ReadLine());
			_EstrucCabeza=Convert.ToInt32(f.ReadLine());
			
			//NARC  e iNARC No guardamos los valores
			for ( int i=0 ; i<numeroJugadores*2 ; i++) {
				Convert.ToBoolean(f.ReadLine());
			}
		}
		
		protected void fichero_definicion( StreamReader f){
			
			f.ReadLine(); //nombre fichero
			
			_nombre=f.ReadLine();
			_modelo=f.ReadLine();
			_tonelados=Convert.ToInt32(f.ReadLine());
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
			
			_BlindBrazoIzquierdoDefinicion=Convert.ToInt32(f.ReadLine());
			_BlindTorsoIzquierdoDefinicion=Convert.ToInt32(f.ReadLine());
			_BlindPiernaIzquierdaDefinicion=Convert.ToInt32(f.ReadLine());
			_BlindPiernaDerechaDefinicion=Convert.ToInt32(f.ReadLine());
			_BlindTorsoDerechoDefinicion=Convert.ToInt32(f.ReadLine());
			_BlindBrazoDerechoDefinicion=Convert.ToInt32(f.ReadLine());
			_BlindTorsoCentralDefinicion=Convert.ToInt32(f.ReadLine());
			_BlindCabezaDefinicion=Convert.ToInt32(f.ReadLine());
			_BlindAtrasTorsoIzquierdoDefinicion=Convert.ToInt32(f.ReadLine());
		 	_BlindAtrasTorsoDerechoDefinicion=Convert.ToInt32(f.ReadLine());
			_BlindAtrasTorsoCentralDefinicion=Convert.ToInt32(f.ReadLine());
			
			_EstrucBrazoIzquierdoDefinicion=Convert.ToInt32(f.ReadLine());
			_EstrucTorsoIzquierdoDefinicion=Convert.ToInt32(f.ReadLine());
			_EstrucPiernaIzquierdaDefinicion=Convert.ToInt32(f.ReadLine());
			_EstrucPiernaDerechaDefinicion=Convert.ToInt32(f.ReadLine());
			_EstrucTorsoDerechoDefinicion=Convert.ToInt32(f.ReadLine());
			_EstrucBrazoDerechoDefinicion=Convert.ToInt32(f.ReadLine());
			_EstrucTorsoCentralDefinicion=Convert.ToInt32(f.ReadLine());
			_EstrucCabezaDefinicion=Convert.ToInt32(f.ReadLine());
			
			_numeroComponentes=Convert.ToInt32(f.ReadLine());
			_componentes = new Componente[_numeroComponentes];
			_armas = new ArrayList();
			Componente aux1;
			for ( int i=0 ; i<_numeroComponentes ; i++){
				aux1 = new Componente(f);
				if ( aux1.clase()=="ARMA") _armas.Add(aux1);
				_componentes[i]=aux1;
			}
			
			_numeroArmas=Convert.ToInt32(f.ReadLine());
			_numeroActuadores=Convert.ToInt32(f.ReadLine());
			_actuadores = new Actuador[_numeroActuadores];
			Actuador aux2;
			for (int i=0 ; i<_numeroActuadores ; i++ ){
				aux2 = new Actuador(f);
				_actuadores[i]=aux2;
			}
			
			_slotsBrazoIzquierdo = new LocalizacionMech(f);
			_slotsTorsoIzquierdo = new LocalizacionMech(f);
			_slotsPiernaIzquierda = new LocalizacionMech(f);
			_slotsPiernaDerecha = new LocalizacionMech(f);
			_slotsTorsoDerecho = new LocalizacionMech(f);
			_slotsBrazoDerecho = new LocalizacionMech(f);
			_slotsTorsoCentral = new LocalizacionMech(f);
			_slotsCabeza = new LocalizacionMech(f);
			
			_andarDefinicion=Convert.ToInt32(f.ReadLine());
			_correrDefinicion=Convert.ToInt32(f.ReadLine());
			_saltarDefinicion=Convert.ToInt32(f.ReadLine());
			_tipoRadiador=Convert.ToInt32(f.ReadLine());
		
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
		public int BlindBrazoIzquierdo() { return _BlindBrazoIzquierdo; }		
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
		public int EstrucCabeza() { return _EstrucCabeza; }
		
	//METODOS GET DEFINICION
		public string nombre() { return _nombre; }
		public string modelo() { return _modelo; }
		public int tonelados() { return _tonelados; }
		public int potencia() { return _potencia; }
		public int numeroRadiadoresInternos() { return _numeroRadiadoresInternos; }
		public int numeroRadiadores() { return _numeroRadiadores; }
		public Boolean masc() { return _masc; }
		public Boolean dacmtd() { return _dacmtd; }
		public Boolean dacmti() { return _dacmti; }
		public Boolean dacmtc() { return _dacmtc; }
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
		
		/*public int BlindBrazoIzquierdoDefinicion() { return _BlindBrazoIzquierdoDefinicion; }
		public int BlindTorsoIzquierdoDefinicion() { return _BlindTorsoIzquierdoDefinicion; }
		public int BlindPiernaIzquierdaDefinicion() { return _BlindPiernaIzquierdaDefinicion; }
		public int BlindPiernaDerechaDefinicion() { return _BlindPiernaDerechaDefinicion; }
		public int BlindTorsoDerechoDefinicion() { return _BlindTorsoDerechoDefinicion; }
		public int BlindBrazoDerechoDefinicion() { return _BlindBrazoDerechoDefinicion; }
		public int BlindTorsoCentralDefinicion() { return _BlindTorsoCentralDefinicion; }
		public int BlindCabezaDefinicion() { return _BlindCabezaDefinicion; }
		public int BlindAtrasTorsoIzquierdoDefinicion() { return _BlindAtrasTorsoIzquierdoDefinicion; }
		public int BlindAtrasTorsoDerechoDefinicion() { return _BlindAtrasTorsoDerechoDefinicion; }
		public int BlindAtrasTorsoCentralDefinicion() { return _BlindAtrasTorsoCentralDefinicion; }
		
		public int EstrucBrazoIzquierdoDefinicion() { return _EstrucBrazoIzquierdoDefinicion; }
		public int EstrucTorsoIzquierdoDefinicion() { return _EstrucTorsoIzquierdoDefinicion; }
		public int EstrucPiernaIzquierdaDefinicion() { return _EstrucPiernaIzquierdaDefinicion; }
		public int EstrucPiernaDerechaDefinicion() { return _EstrucPiernaDerechaDefinicion; }
		public int EstrucTorsoDerechoDefinicion() { return _EstrucTorsoDerechoDefinicion; }
		public int EstrucBrazoDerechoDefinicion() { return _EstrucBrazoDerechoDefinicion; }
		public int EstrucTorsoCentralDefinicion() { return _EstrucTorsoCentralDefinicion; }
		public int EstrucCabezaDefinicion() { return _EstrucCabezaDefinicion; }*/
		
		public int numeroComponentes() { return _numeroComponentes; }
		public Componente[] componentes() { return _componentes; }
		
		public int numeroArmas() { return _numeroArmas; }
		public ArrayList armas() { return _armas;}

		public int numeroActuadores() { return _numeroActuadores; }
		public Actuador[] actuadores() { return _actuadores; }
		
		public LocalizacionMech slotsBrazoIzquierdo() { return _slotsBrazoIzquierdo; }
		public LocalizacionMech slotsTorsoIzquierdo() { return _slotsTorsoIzquierdo; }
		public LocalizacionMech slotsPiernaIzquierda() { return _slotsPiernaIzquierda; }
		public LocalizacionMech slotsPiernaDerecha() { return _slotsPiernaDerecha; }
		public LocalizacionMech slotsTorsoDerecho() { return _slotsTorsoDerecho; }
		public LocalizacionMech slotsBrazoDerecho() { return _slotsBrazoDerecho; }
		public LocalizacionMech slotsTorsoCentral() { return _slotsTorsoCentral; }
		public LocalizacionMech 	slotsCabeza() { return _slotsCabeza; }
	/*	public int andarDefinicion() { return _andarDefinicion; }
		public int correrDefinicion() { return _correrDefinicion; }
		public int saltarDefinicion() { return _saltarDefinicion; } */
		public int tipoRadiador() { return _tipoRadiador; }
		
		public int distanciaTiroCorta() { return _distanciaTiroCorta;}
		public int distanciaTiroMedia() { return _distanciaTiroMedia;}
		public int distanciaTiroLarga() { return _distanciaTiroLarga;}
		
        #endregion

        #region metodos
		public void datos(){
			Console.WriteLine("Numero jugador:"+ _numeroJ);
			Console.WriteLine("Nombre mech: "+_nombre);
			Console.WriteLine("Modelo mech: "+_modelo);
			Console.WriteLine("operativo:" + _operativo);
			Console.WriteLine("desconectado: "+_desconectado);
			Console.WriteLine("atascado terreno pantanoso: "+_atascado);
			Console.WriteLine("en el suelo: "+_enSuelo);
			Console.WriteLine("posicion fila: "+_posicion.fila()+" columna: "+_posicion.columna());
			Console.WriteLine("lado encaramiento: "+_ladoEncaramiento);
			Console.WriteLine("lado encaramiento torso: "+_ladoEncaramientoTorso);
			Console.WriteLine("nivel temperatura: "+_nivelTemp);
			Console.WriteLine("Numero armas: "+_numeroArmas);
			for ( int i=0 ; i<_armas.Count ; i++ ){
				Console.WriteLine("Arma "+i+": "+((Componente)_armas[i]).nombre());		
				Console.WriteLine("\tTipo Arma: "+((Componente)_armas[i]).tipoArma());
				Console.WriteLine("\tDanio: "+((Componente)_armas[i]).danio());
				Console.WriteLine("\tLocalizacion: "+((Componente)_armas[i]).localizacion());
				Console.WriteLine("\tDistancia corta: "+ ((Componente)_armas[i]).distanciaCorta());
				Console.WriteLine("\tDistancia media: "+ ((Componente)_armas[i]).distanciaMedia());
				Console.WriteLine("\tDistancia larga: "+ ((Componente)_armas[i]).distanciaLarga());
				Console.WriteLine("\tDistancia minima: "+ ((Componente)_armas[i]).distanciaMinima());

			}
			Console.WriteLine("Distancia corta de tiro del mech: "+_distanciaTiroCorta);
			Console.WriteLine("Distancia media de tiro del mech: "+_distanciaTiroMedia);
			Console.WriteLine("Distancia larga de tiro del mech: "+_distanciaTiroLarga);
			Console.WriteLine("Numero radiadores: "+_numeroRadiadores);
			
		
			Console.WriteLine();
		}
		
		
		public void calculoDistanciaTiro() {
			int media=0 , larga=0 , corta=0 ;
			for ( int i=0 ; i<_armas.Count ; i++ ) {
				corta+=((Componente)_armas[i]).distanciaCorta();
				media+=((Componente)_armas[i]).distanciaMedia();
				larga+=((Componente)_armas[i]).distanciaLarga();
			}
			
			_distanciaTiroCorta=corta/_armas.Count;
			_distanciaTiroMedia=media/_armas.Count;
			_distanciaTiroLarga=larga/_armas.Count;
		}
		
        //metodo para ver si me puedo mover
        public estadoGeneral estado() {

            return estadoGeneral.Activo;
        }

        public estado estadoBrazoIzquierdo(){
            return 0;
        }

        public estado estadoTorsoIzquierdo() {
            return 0;
        }
        public estado estadoPiernaIzquierda () {
            return 0;
        }

        public estado estadoPiernaDerecha() {
            return 0;
        }

        public estado estadoTorsoDerecho()
        {
            return 0;
        }

        public estado estadoBrazoDerecho()
        {
            return 0;
        }

        public estado estadoTorsoCentral()
        {
            return 0;
        }

        public estado estadoCabeza()
        {
            return 0;
        }

        public estado estadoAtrasTorsoIzquierdo()
        {
            return 0;
        }

        public estado estadoAtrasTorsoDerecho()
        {
            return 0;
        }

        public estado estadoAtrasTorsoCentral()
        {
            return 0;
        }

        //metodo que de una porcentaje de accion?�
		
        #endregion
		
	}

}