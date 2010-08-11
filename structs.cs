
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
        public Casilla casilla;
        public Casilla padre;
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
		Nulo=1, //0%
		Malo, //30%-0%
		Medio, //60%-30%
		Bueno, //100%-60%
	};

    /*
     * Estructura usada para conocer el estado de las distintas partes de un mech
     */
    /*public struct vitalidad 
    {
        public estadoBlindaje brazoIzquierdo;
        public estadoBlindaje torsoIzquierdo;
        public estadoBlindaje piernaIzquierda;
        public estadoBlindaje piernaDerecha;
        public estadoBlindaje torsoDerecho;
        public estadoBlindaje brazoDerecho;
        public estadoBlindaje torsoCentral;
        public estadoBlindaje cabeza;
        public estadoBlindaje atrasTorsoIzquierdo;
        public estadoBlindaje atrasTorsoDerecho;
        public estadoBlindaje atrasTorsoCentral;
    };*/


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
