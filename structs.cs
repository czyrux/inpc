
using System;

namespace ico
{

	public struct municion {
		public string _localizacion;
		public int _slotMunicion;
	};

    /// <summary>
    /// tipo de movimientos que puede realizar un mech
    /// </summary>
    public enum tipoMovimiento
    {
        Inmovil,
        Andar,
        Correr,
        Saltar
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

	/// <summary>
    /// Enumerado que define los posibles encaramientos del mech
	/// </summary>
    public enum Encaramiento
    {
        Arriba = 1,
        SuperiorDerecha = 2,
        InferiorDerecho = 3,
        Abajo = 4,
        InferiorIzquierda = 5,
        SuperiorIzquierda = 6,
    };

    /// <summary>
    /// Enumerado que define las posibles estrategias que puede tomar un mech
    /// </summary>
    public enum Estrategia { 
        Agresiva ,
        Defensiva ,
    };
}
