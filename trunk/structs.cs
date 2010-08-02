
using System;

namespace ico
{

	public struct municion {
		public string _localizacion;
		public int _slotMunicion;
	};
	
    public struct heuristica
    {
        public int g;
        public int h;
        public int f;
    };

    /*
     * Enumerado que define los posibles situaciones en que puede encontrarse un mech 
     */
    public enum situacion 
    { 
        Activo ,
        Incapacitado ,
        Tumbado ,
        Atascado ,
        Desconectado
    };

    /*
     * Enumerado que define los posibles estados del blindaje de una zona del mech 
     */
    public enum estadoBlindaje
    {
        Excelente, //100%-80%
        Bueno, //80%-50%
        Medio, //50%-20%
        Malo, //20%-0%
        Nulo//0%
    };


    /*
     * Estructura usada para conocer el estado de las distintas partes de un mech
     */
    public struct vitalidad 
    {
        public estadoBlindaje brazoIzquierdo;
        public estadoBlindaje torsoIzquierdo;
        public estadoBlindaje piernaIzquierda;
        public estadoBlindaje piernaDerecha;
        public estadoBlindaje torsoDerecho;
		public estado brazoDerecho;
		public estado torsoCentral;
		public estado cabeza;
		public estado atrasTorsoIzquierdo;
		public estado atrasTorsoDerecho;
		public estado atrasTorsoCentral;
    };


    /**
     * Enumerado utilizado para clasificar al tipo de mech segun su armadura 
     */
    public enum tipoMech { 
        Ligero ,
        Medio ,
        Pesado ,
    };

	/**
	 * Enumerado que define los posibles encaramientos del mech
	 */
    public enum Encaramiento
    {
        Arriba = 1,
        SuperiorDerecha = 2,
        InferiorDerecho = 3,
        Abajo = 4,
        InferiorIzquierda = 5,
        SuperiorIzquierda = 6,
    };
}
