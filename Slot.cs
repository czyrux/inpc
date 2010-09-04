
using System;
using System.IO;
using System.Collections;

namespace ico
{
    /// <summary>
    /// Clase que contiene la informacion de un slot de un Mech
    /// </summary>
	public class Slot
    {
        #region atributos
        /// <summary>
        /// Clase de componente: "NADA","ARMA","MUNICION","EQUIPO","ACTUADOR","ARMADURA","ARMAFISICA"
        /// </summary>
		private string _clase;
        /// <summary>
        /// Cantidad (solo para municiones)
        /// </summary>
		private int _cantidad;
        /// <summary>
        /// Codigo del componente
        /// </summary>
		private int _codigo;
        /// <summary>
        /// Nombre del componente
        /// </summary>
		private string _nombre;
        /// <summary>
        /// En que posicion aparece en la lista de componentes
        /// </summary>
		private int _indiceComponente;
        /// <summary>
        /// En que posicion aparece en la lista de actuadores
        /// </summary>
		private int _indiceActuador;
        /// <summary>
        /// Daño de la municion en caso de critico (solo para municion)
        /// </summary>
		private int _danioCritico;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor con parametros
        /// </summary>
        /// <param name="f">Descriptor del fichero ya abierto</param>
		public Slot ( StreamReader f) {
			_clase=f.ReadLine();
			_cantidad=Convert.ToInt32(f.ReadLine());
			_codigo=Convert.ToInt32(f.ReadLine());
			_nombre=f.ReadLine();
			_indiceComponente=Convert.ToInt32(f.ReadLine());
			_indiceActuador=Convert.ToInt32(f.ReadLine());
			_danioCritico=Convert.ToInt32(f.ReadLine());
		}
        #endregion

        #region metodosGet
        /// <summary>
        /// Indica la clase del componente
        /// </summary>
        /// <returns>Texto</returns>
		public string clase() { return _clase;}

        /// <summary>
        /// Indica la cantidad de municion
        /// </summary>
        /// <returns>Entero</returns>
		public int cantidad() { return _cantidad;}

        /// <summary>
        /// Indica el codigo del componente
        /// </summary>
        /// <returns>Entero</returns>
		public int codigo() { return _codigo;}

        /// <summary>
        /// Indica el nombre del componente
        /// </summary>
        /// <returns>Texto</returns>
		public string nombre() { return _nombre;}

        /// <summary>
        /// Indica el indice del componente dentro de la lista de componentes
        /// </summary>
        /// <returns>Entero</returns>
		public int indiceComponente() { return _indiceComponente;}

        /// <summary>
        /// Indica el indice del actuador dentro de la lista de actuadores
        /// </summary>
        /// <returns>Entero</returns>
		public int indiceActuador() { return _indiceActuador;}

        /// <summary>
        /// Indica el daño que realiza la municion en caso de critico
        /// </summary>
        /// <returns>Entero</returns>
		public int danioCritico() { return _danioCritico; }
        #endregion
    }
}
