
using System;

namespace ico
{

    /// <summary>
    /// Estructura usada para guardar datos de municion
    /// </summary>
	public struct municion {
		public string _localizacion;
		public int _slotMunicion;
	};
    
    /// <summary>
    /// Enumerado utilizado para clasificar al tipo de mech segun su armadura 
    /// </summary>
    public enum tipoMech { 
        Ligero ,
        Medio ,
        Pesado ,
        Asalto ,
    };

	/// <summary>
    /// Enumerado que define los posibles encaramientos del mech
	/// </summary>
    public enum Encaramiento
    {
        /// <summary>
        /// Indica si el encaramiento es uno
        /// </summary>
        Arriba = 1,
        /// <summary>
        /// indica si el encaramiento es dos
        /// </summary>
        SuperiorDerecha = 2,
        /// <summary>
        /// indica si el encaramiento es 3
        /// </summary>
        InferiorDerecho = 3,
        /// <summary>
        /// indica si el encaramiento es 4
        /// </summary>
        Abajo = 4,
        /// <summary>
        /// indica si el encaramiento es 5
        /// </summary>
        InferiorIzquierda = 5,
        /// <summary>
        /// indica si el encaramiento es 6
        /// </summary>
        SuperiorIzquierda = 6,
    };

    /// <summary>
    /// Enumerado que define las posibles estrategias que puede tomar un mech
    /// </summary>
    public enum Estrategia { 
        /// <summary>
        /// indica si la estrategia es Agresiva o 0
        /// </summary>
        Agresiva ,
        /// <summary>
        /// indica si la estrategia es defensiva o 1
        /// </summary>
        Defensiva ,
    };
}
