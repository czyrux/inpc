
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
     * Enumerado que define los posibles estados en que puede encontrarse un mech 
     */
    public enum estadoGeneral 
    { 
        Activo ,
        Incapacitado,
        Inconsciente,
        Muerto
    };

    /*
     * Enumerado que define los posibles estados de una zona del cuerpo de un mech 
     */
    public enum estado
    {
        Excelente,
        Bueno,
        Herido,
        Malherido,
        Critico,
        Inutilizado
    };


    /*
     * Estructura usada para conocer el estado de las distintas partes de un mech
     */
    public struct vitalidad 
    { 
        public estado brazoIzquierdo;
		public estado torsoIzquierdo;
		public estado piernaIzquierda;
		public estado piernaDerecha;
		public estado torsoDerecho;
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
