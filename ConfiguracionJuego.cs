
using System;
using System.IO;

namespace ico {

	public class ConfiguracionJuego {
		
        #region atributos
		private int _numeroJugador;
		private Boolean _incendios;
	    private Boolean _viento;
	    private int  _direccionViento;
	    private Boolean _ataquesFisicos;
	    private Boolean _faseCalor;
	    private Boolean _devastarBosques;
	    private Boolean _derrumbarEdificios;
	    private Boolean _chequeoPilotaje;
	    private Boolean _chequeoDanio;
	    private Boolean _chequeoDesconexion;
	    private Boolean _impactosCriticos;
	    private Boolean _explosionMunicion;
	    private Boolean _apagarRadiadores;
		private int[] _iniciativa;
        private String[] _movimientos;
        #endregion

        #region constructor
        public ConfiguracionJuego ( int numeroJugador) {
			_numeroJugador = numeroJugador;
			leerConfiguracion();
			leerIniciativa();
            leerMovimiento();
		}
        #endregion

        #region lectura ficheros
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
		public int numeroJugador() { return _numeroJugador; }
		public Boolean incendios() { return _incendios; }
	    public Boolean viento() { return _viento; }
	    public int  direccionViento() { return _direccionViento; }
	    public Boolean ataquesFisicos() { return _ataquesFisicos; }
	    public Boolean faseCalor() { return _faseCalor; }
	    public Boolean devastarBosques() { return _devastarBosques; }
	    public Boolean derrumbarEdificios() { return _derrumbarEdificios; }
	    public Boolean chequeoPilotaje() { return _chequeoPilotaje; }
	    public Boolean chequeoDanio() { return _chequeoDanio; }
	    public Boolean chequeoDesconexion() { return _chequeoDesconexion; }
	    public Boolean impactosCriticos() { return _impactosCriticos; }
	    public Boolean explosionMunicion() { return _explosionMunicion; }
	    public Boolean apagarRadiadores() { return _apagarRadiadores; }
		public int[] iniciativa() { return _iniciativa; }
        public String[] movimientos() { return _movimientos; }
        public String movimiento(int indice) {
            if (indice < _movimientos.Length) {
                return _movimientos[indice];
            }

            return null;
        }
        #endregion
		
	}
}
