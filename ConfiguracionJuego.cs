
using System;
using System.IO;

namespace ico {

    /// <summary>
    /// Clase que contiene los parametros de configuracion de la partida
    /// </summary>
	public class ConfiguracionJuego {
		
        #region atributos
        /// <summary>
        /// Numero del MechJugador
        /// </summary>
		private int _numeroJugador;
        /// <summary>
        /// Atributo booleano que indica si esta permitido los incendios
        /// </summary>
		private Boolean _incendios;
        /// <summary>
        /// Atributo booleano que indica si esta permitido el viento
        /// </summary>
	    private Boolean _viento;
        /// <summary>
        /// Indica la direccion del viento
        /// </summary>
	    private int  _direccionViento;
        /// <summary>
        /// Atributo booleano que indica si esta permitido los ataques fisicos
        /// </summary>
	    private Boolean _ataquesFisicos;
        /// <summary>
        /// Atributo booleano que indica si se comtemplan los efectos del calor
        /// </summary>
	    private Boolean _faseCalor;
        /// <summary>
        /// Atributo booleano que indica si esta permitido los devastar bosques
        /// </summary>
	    private Boolean _devastarBosques;
        /// <summary>
        /// Atributo booleano que indica si esta permitido derrumbar edificios
        /// </summary>
	    private Boolean _derrumbarEdificios;
        /// <summary>
        /// Atributo booleano que indica si realizaran chequeos de pilotaje
        /// </summary>
	    private Boolean _chequeoPilotaje;
        /// <summary>
        /// Atributo booleano que indica si realizaran chequeos de daño
        /// </summary>
	    private Boolean _chequeoDanio;
        /// <summary>
        /// Atributo booleano que indica si realizaran chequeos de desconexion
        /// </summary>
	    private Boolean _chequeoDesconexion;
        /// <summary>
        /// Atributo booleano que indica si esta permitido los impactos criticos
        /// </summary>
	    private Boolean _impactosCriticos;
        /// <summary>
        /// Atributo booleano que indica si esta permitido la explosion de municion
        /// </summary>
	    private Boolean _explosionMunicion;
        /// <summary>
        /// Atributo booleano que indica si esta permitido apagar radiadores
        /// </summary>
	    private Boolean _apagarRadiadores;

        /// <summary>
        /// Array de iniciativa de los mech
        /// </summary>
		private int[] _iniciativa;

        /// <summary>
        /// Array que indica el movimiento del mech en la fase de movimiento anterior
        /// </summary>
        private String[] _movimientos;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor con parametros
        /// </summary>
        /// <param name="numeroJugador">Numero del MechJugador</param>
        public ConfiguracionJuego ( int numeroJugador) {
			_numeroJugador = numeroJugador;
			leerConfiguracion();
			leerIniciativa();
            leerMovimiento();
		}
        #endregion

        #region lectura ficheros
        /// <summary>
        /// Metodo que lee el fichero de configuracion
        /// </summary>
        private void leerConfiguracion( ){
			string path="C:/ficheros/";
			StreamReader f = new StreamReader(path+"configJ"+_numeroJugador.ToString()+".sbt" );
			f.ReadLine();
			_incendios=Convert.ToBoolean(f.ReadLine());
		    _viento=Convert.ToBoolean(f.ReadLine());
		    _direccionViento=Convert.ToInt32(f.ReadLine());
		    _ataquesFisicos=Convert.ToBoolean(f.ReadLine());
		    _faseCalor=Convert.ToBoolean(f.ReadLine());
		    _devastarBosques=Convert.ToBoolean(f.ReadLine());
		    _derrumbarEdificios=Convert.ToBoolean(f.ReadLine());
		    _chequeoPilotaje=Convert.ToBoolean(f.ReadLine());
		    _chequeoDanio=Convert.ToBoolean(f.ReadLine());
		    _chequeoDesconexion=Convert.ToBoolean(f.ReadLine());
		    _impactosCriticos=Convert.ToBoolean(f.ReadLine());
		    _explosionMunicion=Convert.ToBoolean(f.ReadLine());
		    _apagarRadiadores=Convert.ToBoolean(f.ReadLine());
			f.Close();
		}
		
        /// <summary>
        /// Metodo que lee el fichero de iniciativa
        /// </summary>
		private void leerIniciativa() {
            string path = "C:/ficheros/";
			StreamReader f = new StreamReader(path+"iniciativaJ"+_numeroJugador.ToString()+".sbt" );
			
			int n = Convert.ToInt32(f.ReadLine());
			_iniciativa = new int[n];
			for ( int i=0 ; i<n ; i++ ){
				_iniciativa[i]=Convert.ToInt32(f.ReadLine());
			}
			f.Close();
		}

        /// <summary>
        /// Metodo que lee el fichero de movimiento
        /// </summary>
        private void leerMovimiento() {
            string path = "C:/ficheros/";
            StreamReader f = new StreamReader(path + "mov.sbt");
            f.ReadLine(); //nombre del fichero movSBT

            int cantidad;
            cantidad = Convert.ToInt32(f.ReadLine());
            _movimientos = new String[cantidad];

            for (int i = 0; i < cantidad; i++)
                _movimientos[i] = f.ReadLine();
            
            f.Close();
        }

#endregion
		
        #region metodosGet
        /// <summary>
        /// Indica el numero del MechJugador
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public int numeroJugador() { return _numeroJugador; }
        /// <summary>
        /// Indica si estan permitidos incendios
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean incendios() { return _incendios; }
        /// <summary>
        /// Indica si estan permitidos el viento
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
	    public Boolean viento() { return _viento; }
        /// <summary>
        /// Indica la direccion del viento
        /// </summary>
        /// <returns>Entero entre 1-6 con la direccion del viento</returns>
	    public int  direccionViento() { return _direccionViento; }
        /// <summary>
        /// Indica si estan permitidos los ataques fisicos
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
	    public Boolean ataquesFisicos() { return _ataquesFisicos; }
        /// <summary>
        /// Indica si estan permitidos los efectos de la fase calor
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
	    public Boolean faseCalor() { return _faseCalor; }
        /// <summary>
        /// Indica si estan permitidos devastar bosques
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
	    public Boolean devastarBosques() { return _devastarBosques; }
        /// <summary>
        /// Indica si estan permitidos derrumbar edificios
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
	    public Boolean derrumbarEdificios() { return _derrumbarEdificios; }
        /// <summary>
        /// Indica si estan permitidos los chequeos de pilotaje
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
	    public Boolean chequeoPilotaje() { return _chequeoPilotaje; }
        /// <summary>
        /// Indica si estan permitidos los chequeos de daño
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
	    public Boolean chequeoDanio() { return _chequeoDanio; }
        /// <summary>
        /// Indica si estan permitidos los chequeos de desconexion
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
	    public Boolean chequeoDesconexion() { return _chequeoDesconexion; }
        /// <summary>
        /// Indica si estan permitidos los impactos criticos
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
	    public Boolean impactosCriticos() { return _impactosCriticos; }
        /// <summary>
        /// Indica si estan permitidos la explosion de municion
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
	    public Boolean explosionMunicion() { return _explosionMunicion; }
        /// <summary>
        /// Indica si estan permitidos apagar los radiadores
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
	    public Boolean apagarRadiadores() { return _apagarRadiadores; }

        /// <summary>
        /// Devuelve el array de iniciativas
        /// </summary>
        /// <returns>Array enteros</returns>
		public int[] iniciativa() { return _iniciativa; }

        /// <summary>
        /// Devuelve el array de movimientos
        /// </summary>
        /// <returns>Array String</returns>
        public String[] movimientos() { return _movimientos; }
        /// <summary>
        /// Devuelve le movimiento del mech indicado
        /// </summary>
        /// <param name="indice">Indice del mech</param>
        /// <returns>Cadena de texto que indica el movimiento</returns>
        public String movimiento(int indice) {
            if (indice < _movimientos.Length) {
                return _movimientos[indice];
            }
            return null;
        }
        #endregion
		
	}
}
