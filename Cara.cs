using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace ico
{
    /// <summary>
    /// Clase que contiene las propiedades de una cara de una casilla.
    /// </summary>
    public class Cara
    {
        /// <summary>
        /// constructor por defecto 
        /// </summary>
        public Cara() {
            Boolean _rio = _carretera = false;
        }
        
        /// <summary>
        /// constructor con parametros
        /// </summary>
        /// <param name="rio">si hay rio o no; tipo <seBoolean</param>
        /// <param name="carretera">si hay carretera o no; tipo Boolean</param>
        public Cara(Boolean rio, Boolean carretera)
        {
            _rio = false;
            _carretera = false;
        }
        

        /// <summary>
        /// propiedad que devuelve se hay rio
        /// </summary>
        /// <returns>rio, tipo Boolean</returns>
        public Boolean  rio() {
            return _rio;
        }
        /// <summary>
        /// propiedad establece si hay rio o no
        /// </summary>
        /// <param name="rio"></param>
        public void rio(Boolean rio) {
            _rio = rio;
        }

        /// <summary>
        /// propiedad que devuelve se hay carretera
        /// </summary>
        /// <returns>carretera, tipo Boolean</returns>
        public Boolean carretera()
        {
            return _carretera;
        }
        /// <summary>
        /// propiedad establece si hay rio o no
        /// </summary>
        /// <param name="rio"></param>
        public void carretera(Boolean carretera)
        {
            _carretera = carretera;
        }

        /// <summary>
        /// contine si esta cara apunta a un rio
        /// </summary>
        private Boolean _rio;
        /// <summary>
        /// contine si esta cara que apunta a una carretera
        /// </summary>
        private Boolean _carretera;
    }
}
