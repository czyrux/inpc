
using System;
using System.IO;
using System.Collections;

namespace ico
{
    /// <summary>
    /// Clase que hereda de Mech, e incluye los parametros especificos del mech Jugador
    /// </summary>
	public class MechJugador: Mech {
		
        #region atributos
	    /// <summary>
	    /// Indica los puntos para andar
	    /// </summary>
		private int _puntosAndar;
        /// <summary>
        /// Indica los puntos para correr
        /// </summary>
		private int _puntosCorrer;
        /// <summary>
        /// Indica los puntos para saltar
        /// </summary>
		private int _puntosSaltar;
		
        /// <summary>
        /// Numero de radiadores encendidos
        /// </summary>
		private int _radiadoresEncendidos;
        /// <summary>
        /// Numero de radiadores apagados
        /// </summary>
		private int _radiadoresApagados;
		
        /// <summary>
        /// Numero de heridas del MechWarrior
        /// </summary>
		private int _heridas;
        /// <summary>
        /// Indica la consciencia del mech
        /// </summary>
		private Boolean _consciente;
		
        /// <summary>
        /// Nº de slots impactados en BI
        /// </summary>
		private Boolean[] _slotsImpactadosBrazoIzquierdo = new Boolean[12];
        /// <summary>
        /// Nº de slots impactados en TI
        /// </summary>
		private Boolean[] _slotsImpactadosTorsoIzquierdo = new Boolean[12];
        /// <summary>
        /// Nº de slots impactados en PI
        /// </summary>
		private Boolean[] _slotsImpactadosPiernaIzquierda = new Boolean[6];
        /// <summary>
        /// Nº de slots impactados en PD
        /// </summary>
		private Boolean[] _slotsImpactadosPiernaDerecha = new Boolean[6];
        /// <summary>
        /// Nº de slots impactados en 
        /// </summary>
		private Boolean[] _slotsImpactadosTorsoDerecho = new Boolean[12];
        /// <summary>
        /// Nº de slots impactados en BD
        /// </summary>
		private Boolean[] _slotsImpactadosBrazoDerecho = new Boolean[12];
        /// <summary>
        /// Nº de slots impactados en TC
        /// </summary>
		private Boolean[] _slotsImpactadosTorsoCentral = new Boolean[12];
        /// <summary>
        /// Nº de slots impactados en CAB
        /// </summary>
		private Boolean[] _slotsImpactadosCabeza = new Boolean[6];
		
		/// <summary>
		/// Atributo booleano que indica si se ha disparado un arma desde BI
		/// </summary>
		private Boolean _disparoBrazoIzquierdo;
        /// <summary>
        /// Atributo booleano que indica si se ha disparado un arma desde TI
        /// </summary>
		private Boolean _disparoTorsoIzquierdo;
        /// <summary>
        /// Atributo booleano que indica si se ha disparado un arma desde PI
        /// </summary>
		private Boolean _disparoPiernaIzquierda;
        /// <summary>
        /// Atributo booleano que indica si se ha disparado un arma desde PD
        /// </summary>
		private Boolean _disparoPiernaDerecha;
        /// <summary>
        /// Atributo booleano que indica si se ha disparado un arma desde TD
        /// </summary>
		private Boolean _disparoTorsoDerecha;
        /// <summary>
        /// Atributo booleano que indica si se ha disparado un arma desde BD
        /// </summary>
		private Boolean _disparoBrazoDerecha;
        /// <summary>
        /// Atributo booleano que indica si se ha disparado un arma desde TC
        /// </summary>
		private Boolean _disparoTorsoCentral;
        /// <summary>
        /// Atributo booleano que indica si se ha disparado un arma desde CAB
        /// </summary>
		private Boolean _disparoCabeza;
		
        /// <summary>
        /// Cantidad de municion a expulsar
        /// </summary>
		private int _municionesExpulsarCantidad;
        /// <summary>
        /// Array con la municion a expulsar
        /// </summary>
		private municion[] _municionesExpulsar;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor con parametros
        /// </summary>
        /// <param name="estado">Descriptor fichero estado</param>
        /// <param name="definicion">Descriptor fichero definicion</param>
        /// <param name="nJugadores">Numero de jugadores en la partida</param>
		public MechJugador ( StreamReader estado, StreamReader definicion, int nJugadores) {
			numeroJugadores=nJugadores;
			
			//leemos los datos de estado
			fichero_estado(estado);
			//leemos los datos de definicion
			fichero_definicion(definicion);
			
		}
        #endregion

        #region lecturafichero
        /// <summary>
        /// Metodo que lee los datos del fichero de estado del mech
        /// </summary>
        /// <param name="f">Descriptor del fichero ya abierto</param>
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

            //_BlindTotal = _BlindBrazoIzquierdo + _BlindTorsoIzquierdo + _BlindPiernaIzquierda + _BlindPiernaDerecha + _BlindTorsoDerecho + _BlindBrazoDerecho + _BlindTorsoCentral + _BlindCabeza + _BlindAtrasTorsoIzquierdo + _BlindAtrasTorsoDerecho + _BlindAtrasTorsoCentral;

			//datos esctructura interna
			_EstrucBrazoIzquierdo=Convert.ToInt32(f.ReadLine());
			_EstrucTorsoIzquierdo=Convert.ToInt32(f.ReadLine());
			_EstrucPiernaIzquierda=Convert.ToInt32(f.ReadLine());
			_EstrucPiernaDerecha=Convert.ToInt32(f.ReadLine());
			_EstrucTorsoDerecho=Convert.ToInt32(f.ReadLine());
			_EstrucBrazoDerecho=Convert.ToInt32(f.ReadLine());
			_EstrucTorsoCentral=Convert.ToInt32(f.ReadLine());
			_EstrucCabeza=Convert.ToInt32(f.ReadLine());
			
			//puntos movimiento
			_puntosAndar=Convert.ToInt32(f.ReadLine());
			_puntosCorrer=Convert.ToInt32(f.ReadLine());
			_puntosSaltar=Convert.ToInt32(f.ReadLine());
			
			//radiadores
			_radiadoresEncendidos=Convert.ToInt32(f.ReadLine());
			_radiadoresApagados=Convert.ToInt32(f.ReadLine());
			
			//heridas
			_heridas=Convert.ToInt32(f.ReadLine());
			_consciente=Convert.ToBoolean(f.ReadLine());
			
			for ( int i=0 ; i<12 ; i++){
				_slotsImpactadosBrazoIzquierdo[i]=Convert.ToBoolean(f.ReadLine()); }
			for ( int i=0 ; i<12 ; i++){
				_slotsImpactadosTorsoIzquierdo[i]=Convert.ToBoolean(f.ReadLine()); }
			for ( int i=0 ; i<6 ; i++){
				_slotsImpactadosPiernaIzquierda[i]=Convert.ToBoolean(f.ReadLine()); }
			for ( int i=0 ; i<6 ; i++){
				_slotsImpactadosPiernaDerecha[i]=Convert.ToBoolean(f.ReadLine()); }
			for ( int i=0 ; i<12 ; i++){
				_slotsImpactadosTorsoDerecho[i]=Convert.ToBoolean(f.ReadLine()); }
			for ( int i=0 ; i<12 ; i++){
				_slotsImpactadosBrazoDerecho[i]=Convert.ToBoolean(f.ReadLine()); }
			for ( int i=0 ; i<12 ; i++){
				_slotsImpactadosTorsoCentral[i]=Convert.ToBoolean(f.ReadLine()); }
			for ( int i=0 ; i<6 ; i++){
				_slotsImpactadosCabeza[i]=Convert.ToBoolean(f.ReadLine()); }
			
			//localizaciones desde donde se ha disparado un arma
			_disparoBrazoIzquierdo=Convert.ToBoolean(f.ReadLine());
			_disparoTorsoIzquierdo=Convert.ToBoolean(f.ReadLine());
			_disparoPiernaIzquierda=Convert.ToBoolean(f.ReadLine());
			_disparoPiernaDerecha=Convert.ToBoolean(f.ReadLine());
			_disparoTorsoDerecha=Convert.ToBoolean(f.ReadLine());
			_disparoBrazoDerecha=Convert.ToBoolean(f.ReadLine());
			_disparoTorsoCentral=Convert.ToBoolean(f.ReadLine());
			_disparoCabeza=Convert.ToBoolean(f.ReadLine());
			
			_municionesExpulsarCantidad=Convert.ToInt32(f.ReadLine());
			if ( _municionesExpulsarCantidad>0) {
				municion mun;
				_municionesExpulsar = new municion[_municionesExpulsarCantidad];
				for ( int i=0 ; i<_municionesExpulsarCantidad ; i++){
					mun._localizacion=f.ReadLine();
					mun._slotMunicion=Convert.ToInt32(f.ReadLine());
					_municionesExpulsar[i]=mun;
				}
			}else
				_municionesExpulsar=null;
			
			//NARC  e iNARC. No guardamos los valores
			for ( int i=0 ; i<numeroJugadores*2 ; i++) {
				Convert.ToBoolean(f.ReadLine());
			}

            _notaMech = -1.0f;
		}
        #endregion
		
        #region metodosGet
		/// <summary>
		/// Metodo que indica los puntos para andar
		/// </summary>
		/// <returns>Entero</returns>
		public int puntosAndar() { return _puntosAndar; }
        /// <summary>
        /// Metodo que indica los puntos para correr
        /// </summary>
        /// <returns>Entero</returns>
		public int puntosCorrer() { return _puntosCorrer; }
        /// <summary>
        /// Metodo que indica los puntos para saltar
        /// </summary>
        /// <returns>Entero</returns>
		public int puntosSaltar() { return _puntosSaltar; }
		
        /// <summary>
        /// Metodo que indica el nº de radiadores encendidos
        /// </summary>
        /// <returns>Entero</returns>
		public int radiadoresEncendidos() { return _radiadoresEncendidos; }
        /// <summary>
        /// Metodo que indica el nº de radiadores apagados
        /// </summary>
        /// <returns>Entero</returns>
		public int radiadoresApagados() { return _radiadoresApagados; }


        /// <summary>
        /// Metodo que indica el nº de heridas del MechWarrior
        /// </summary>
        /// <returns>Entero</returns>
		public int heridas() { return _heridas; }
        /// <summary>
        /// Metodo que indica si el MechWarrior esta consciente
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean consciente() { return _consciente; }
		
		/*public Boolean[] slotsImpactadosBrazoIzquierdo() { return _slotsImpactadosBrazoIzquierdo; }
		public Boolean[] slotsImpactadosTorsoIzquierdo() { return _slotsImpactadosTorsoIzquierdo; } 
		public Boolean[] slotsImpactadosPiernaIzquierda() { return _slotsImpactadosPiernaIzquierda; } 
		public Boolean[] slotsImpactadosPiernaDerecha() { return _slotsImpactadosPiernaDerecha; } 
		public Boolean[] slotsImpactadosTorsoDerecho() { return _slotsImpactadosTorsoDerecho; }
		public Boolean[] slotsImpactadosBrazoDerecho() { return _slotsImpactadosBrazoDerecho; }
		public Boolean[] slotsImpactadosTorsoCentral() { return _slotsImpactadosTorsoCentral; } 
		public Boolean[] slotsImpactadosCabeza() { return _slotsImpactadosCabeza; } */

        /// <summary>
        /// Metodo que indica si se ha producido un disparo con BI
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean disparoBrazoIzquierdo(){ return _disparoBrazoIzquierdo; }
        /// <summary>
        /// Metodo que indica si se ha producido un disparo con TI
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean disparoTorsoIzquierdo(){ return _disparoTorsoIzquierdo; }
        /// <summary>
        /// Metodo que indica si se ha producido un disparo con PI
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean disparoPiernaIzquierda(){ return _disparoPiernaIzquierda; }
        /// <summary>
        /// Metodo que indica si se ha producido un disparo con PD
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean disparoPiernaDerecha(){ return _disparoPiernaDerecha; }
        /// <summary>
        /// Metodo que indica si se ha producido un disparo con TD
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean disparoTorsoDerecha(){ return _disparoTorsoDerecha; }
        /// <summary>
        /// Metodo que indica si se ha producido un disparo con BD
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean disparoBrazoDerecha(){ return _disparoBrazoDerecha; }
        /// <summary>
        /// Metodo que indica si se ha producido un disparo con TC
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean disparoTorsoCentral(){ return _disparoTorsoCentral; }
        /// <summary>
        /// Metodo que indica si se ha producido un disparo con CAB
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean disparoCabeza(){ return _disparoCabeza; }


        /// <summary>
        /// Metodo que indica la cantidad de municion a expulsar
        /// </summary>
        /// <returns>Entero</returns>
		public int municionesExpulsarCantidad() { return _municionesExpulsarCantidad; }
        /// <summary>
        /// Metodo que indica el array con la municion
        /// </summary>
        /// <returns>Array de municion</returns>
		public municion[] municionesExpulsar() { return _municionesExpulsar; }
        #endregion
	}	

}
