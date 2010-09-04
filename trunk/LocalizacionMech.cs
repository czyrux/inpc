
using System;
using System.IO;
using System.Collections;

namespace ico {
    /// <summary>
    /// Incluye la informacion que contiene cada una de las localizaciones de un mech
    /// </summary>
	public class LocalizacionMech
    {
        #region atributos
        /// <summary>
        /// Numero de slots ocupados en la localizacion
        /// </summary>
        private int _slotsOcupados;

        /// <summary>
        /// Array con los slots de la localizacion
        /// </summary>
		private Slot[] _slots;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="f">Descriptor del fichero ya abierto</param>
        public LocalizacionMech ( StreamReader f) {
			
			_slotsOcupados=Convert.ToInt32(f.ReadLine());
			
			_slots = new Slot[_slotsOcupados];
			Slot aux;
			
			for ( int i=0 ; i<_slotsOcupados ; i++){
				aux = new Slot (f);
				_slots[i]=aux;
			}
		}
        #endregion

        #region metodosGEt
        /// <summary>
        /// Devuelve la cantidad de slots que tiene ocupados la localizacion
        /// </summary>
        /// <returns>Entero</returns>
        public int slotsOcupados() { return _slotsOcupados; }

        /// <summary>
        /// Devuelve un array con los slots de la localizacion
        /// </summary>
        /// <returns>Array de Slot</returns>
		public Slot[] slots() { return _slots; }
        #endregion
    }
		
}
