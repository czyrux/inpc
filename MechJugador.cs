
using System;
using System.IO;
using System.Collections;

namespace ico
{

	public class MechJugador: Mech {
		
#region atributos
		//ATRIBUTOS		
		private int _puntosAndar;
		private int _puntosCorrer;
		private int _puntosSaltar;
		
		private int _radiadoresEncendidos;
		private int _radiadoresApagados;
		
		private int _heridas;
		private Boolean _consciente;
		
		private Boolean[] _slotsImpactadosBrazoIzquierdo = new Boolean[12];
		private Boolean[] _slotsImpactadosTorsoIzquierdo = new Boolean[12];
		private Boolean[] _slotsImpactadosPiernaIzquierda = new Boolean[6];
		private Boolean[] _slotsImpactadosPiernaDerecha = new Boolean[6];
		private Boolean[] _slotsImpactadosTorsoDerecho = new Boolean[12];
		private Boolean[] _slotsImpactadosBrazoDerecho = new Boolean[12];
		private Boolean[] _slotsImpactadosTorsoCentral = new Boolean[12];
		private Boolean[] _slotsImpactadosCabeza = new Boolean[6];
		
		//localizaciones desde donde se ha disparado un arma
		private Boolean _disparoBrazoIzquierdo;
		private Boolean _disparoTorsoIzquierdo;
		private Boolean _disparoPiernaIzquierda;
		private Boolean _disparoPiernaDerecha;
		private Boolean _disparoTorsoDerecha;
		private Boolean _disparoBrazoDerecha;
		private Boolean _disparoTorsoCentral;
		private Boolean _disparoCabeza;
		
		private int _municionesExpulsarCantidad;
		private municion[] _municionesExpulsar;
#endregion
		
		//CONSTRUCTOR
		public MechJugador ( StreamReader estado, StreamReader definicion, int nJugadores) {
			numeroJugadores=nJugadores;
			
			//leemos los datos de estado
			fichero_estado(estado);
			//leemos los datos de definicion
			fichero_definicion(definicion);
			
		}

#region lecturafichero		
		//METODOS
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

		}
#endregion
		
#region metodosGet
		//Metodos get
		public int puntosAndar() { return _puntosAndar; }
		public int puntosCorrer() { return _puntosCorrer; }
		public int puntosSaltar() { return _puntosSaltar; }
		
		public int radiadoresEncendidos() { return _radiadoresEncendidos; }
		public int radiadoresApagados() { return _radiadoresApagados; }
		
		public int heridas() { return _heridas; }
		public Boolean consciente() { return _consciente; }
		
		/*public Boolean[] slotsImpactadosBrazoIzquierdo() { return _slotsImpactadosBrazoIzquierdo; }
		public Boolean[] slotsImpactadosTorsoIzquierdo() { return _slotsImpactadosTorsoIzquierdo; } 
		public Boolean[] slotsImpactadosPiernaIzquierda() { return _slotsImpactadosPiernaIzquierda; } 
		public Boolean[] slotsImpactadosPiernaDerecha() { return _slotsImpactadosPiernaDerecha; } 
		public Boolean[] slotsImpactadosTorsoDerecho() { return _slotsImpactadosTorsoDerecho; }
		public Boolean[] slotsImpactadosBrazoDerecho() { return _slotsImpactadosBrazoDerecho; }
		public Boolean[] slotsImpactadosTorsoCentral() { return _slotsImpactadosTorsoCentral; } 
		public Boolean[] slotsImpactadosCabeza() { return _slotsImpactadosCabeza; } */
		
		public Boolean disparoBrazoIzquierdo(){ return _disparoBrazoIzquierdo; }		
		public Boolean disparoTorsoIzquierdo(){ return _disparoTorsoIzquierdo; }		
		public Boolean disparoPiernaIzquierda(){ return _disparoPiernaIzquierda; }		
		public Boolean disparoPiernaDerecha(){ return _disparoPiernaDerecha; }		
		public Boolean disparoTorsoDerecha(){ return _disparoTorsoDerecha; }		
		public Boolean disparoBrazoDerecha(){ return _disparoBrazoDerecha; }		
		public Boolean disparoTorsoCentral(){ return _disparoTorsoCentral; }		
		public Boolean disparoCabeza(){ return _disparoCabeza; }
		
		public int municionesExpulsarCantidad() { return _municionesExpulsarCantidad; }
		public municion[] municionesExpulsar() { return _municionesExpulsar; }
#endregion

#region metodos
		//Excelente , Bueno , Herido , Malherido , Critico , Incapacitado , Incosciente , Muerto
		public string estado() {
			return "";
			
		}
#endregion
	}	

}
