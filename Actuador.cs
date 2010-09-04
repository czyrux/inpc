
using System;
using System.IO;

namespace ico
{
    /// <summary>
    /// Clase que contiene los datos de los actuadores
    /// </summary>
	public class Actuador
    {
        #region atributos
        /// <summary>
        /// Codigo del actuador
        /// </summary>
        private int _codigo;
        /// <summary>
        /// Nombre del actuador
        /// </summary>
		private string _nombre;
        /// <summary>
        /// Localizacion del actuador: (0=BI,1=TI,2=PI,3=PD,4=TD,5=BD,6=TC,7=CAB,8=TIa, 9=TDa,10=TCa) 
        /// </summary>
		private int _localizacion;
        /// <summary>
        /// Indica si esta operativo
        /// </summary>
		private Boolean _operativo;
        /// <summary>
        /// Indica el numero de impactos
        /// </summary>
		private int _numeroImpactos;
        #endregion

        #region contructores
        /// <summary>
        /// Constructor con parametros
        /// </summary>
        /// <param name="f">Descriptor del fichero</param>
		public Actuador ( StreamReader f) {
			_codigo=Convert.ToInt32(f.ReadLine());
			_nombre=f.ReadLine();
			_localizacion=Convert.ToInt32(f.ReadLine());
			_operativo=Convert.ToBoolean(f.ReadLine());
			_numeroImpactos=Convert.ToInt32(f.ReadLine());
		}
        #endregion

        #region metodosGet
        /// <summary>
        /// Indica el codigo
        /// </summary>
        /// <returns>Entero</returns>
		public int codigo() { return _codigo;}
        /// <summary>
        /// Indica el nombre
        /// </summary>
        /// <returns>String</returns>
		public string nombre() { return _nombre;}
        /// <summary>
        /// Indica la localizacion
        /// </summary>
        /// <returns>Entero</returns>
		public int localizacion() { return _localizacion;}
        /// <summary>
        /// Indica si esta operativo
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean operativo() { return _operativo;}
        /// <summary>
        /// Indica el numero de impactos del actuador
        /// </summary>
        /// <returns>Entero</returns>
		public int numeroImpactos() { return _numeroImpactos; }
        #endregion

    }
}
