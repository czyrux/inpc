
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
        public float h;
        public float f;
        public Casilla casilla;
        public Casilla padre;

       /*  heuristica(int g=0 , float h=0,float f=0, Casilla casilla=null,Casilla padre=null) {
            this.g = 0;
            this.h = this.f = 0;
            this.casilla = this.padre = null;
        }*/
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


    /**
     * Enumerado utilizado para clasificar al tipo de mech segun su armadura 
     */
    public enum tipoMech { 
        Ligero ,
        Medio ,
        Pesado ,
        Asalto ,
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

    /*
     * Enumerado que define las posibles estrategias que puede tomar un mech
     */ 
    public enum Estrategia { 
        Ofensiva ,
        Defensiva ,
    };
}
