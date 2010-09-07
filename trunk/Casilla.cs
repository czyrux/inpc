using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace ico
{
    public class Casilla
    {
        #region Constructores
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Casilla() {
            _nivel = _fceEdificio = _tipoTerreno = _objetoTerreno = _nGarrotes = /*_heuristica.g = */0;
            //_heuristica.f = _heuristica.h = 0;
            _edificioDerrumbado = _fuego = _humo = false;
            _caras = new Cara[6];
            _posicion = new Posicion();
            _puntosMovimientos = 0;
            
        }
        /*public Casilla(int nivel, int tipoTerreno, int objetoTerreno, int fceEdificio, Boolean edificioDerrumbado,
        Boolean fuego, Boolean humo, int nGarrotes, Cara[] caras)
        {
            _nivel = nivel;
            _tipoTerreno = tipoTerreno;
            _objetoTerreno = objetoTerreno;
            _fceEdificio = fceEdificio;
            _edificioDerrumbado = edificioDerrumbado;
            _fuego = fuego;
            _humo = humo;
            _nGarrotes = nGarrotes;
            _caras = caras;
            _puntosMovimientos = 0;
        }
        public Casilla(int nivel, int tipoTerreno, int objetoTerreno, int fceEdificio, Boolean edificioDerrumbado, Boolean fuego, Boolean humo, int nGarrotes)
        {
            _nivel = nivel;
            _tipoTerreno = tipoTerreno;
            _objetoTerreno = objetoTerreno;
            _fceEdificio = fceEdificio;
            _edificioDerrumbado = edificioDerrumbado;
            _fuego = fuego;
            _humo = humo;
            _nGarrotes = nGarrotes;
            _puntosMovimientos = 0;
        }*/
#endregion

        #region "publicas"
        
        /// <summary>
        /// costo intrinceco en puntos de movimentos de una casilla
        /// </summary>
        /// <returns>costo en puntos de mvimientos, tipo Int</returns>
        public int  costoMovimiento(){
            if (_puntosMovimientos == 0)
            {
                _puntosMovimientos = 0;
                switch (_tipoTerreno)
                {
                    case 0://despejado
                        _puntosMovimientos++;
                        break;
                    case 1://pavimentado
                        _puntosMovimientos++;
                        break;
                    case 2://agua
                        switch (_nivel)
                        { // ver comentarios de el agua en el log.
                            case 0:
                                _puntosMovimientos++;
                                break;
                            case -1:
                                _puntosMovimientos += 2;
                                break;

                            default:
                                _puntosMovimientos += 4;
                                break;
                        }
                        break;

                    case 3://pantanoso
                        _puntosMovimientos+=2;
                        break;
                }
                switch (_objetoTerreno)
                {

                    case 0://escombros
                        _puntosMovimientos += 2;
                        break;
                    case 1://bosque disperso
                        _puntosMovimientos += 2;
                        break;
                    case 2://bosque denso
                        _puntosMovimientos += 3;
                        break;
                    case 3: //edificio ligero
                        _puntosMovimientos += 2;
                        break;
                    case 4://edificio medio
                        _puntosMovimientos += 3;
                        break;
                    case 5: //edificio grande o pesado
                        _puntosMovimientos += 4;
                        break;
                    case 6: //edificio reforzado
                        _puntosMovimientos += 5;
                        break;
                }
            }
            return _puntosMovimientos;
        }

        /// <summary>
        /// costo asociado en puntos de movimentos de una casilla
        /// </summary>
        /// <param name="a">casilla al cual se le calculara el costo asociado</param>
        /// <returns>costo en puntos de mvimientos; tipo Int</returns>
        public int costoMovimiento( Casilla a)
        {
            int costo = 0;
            if (a.tipoTerreno() == 2)
                costo = PanelControl.penalizadorAgua;
            if (Math.Abs(_posicion.columna() - a.columna()) < 2 || Math.Abs(_posicion.fila() - a.fila()) < 2)
            {
                switch (a.nivel() - _nivel)
                {
                    case 0:
                        ;
                        break;
                    case 1:
                        costo++;
                        break;
                    case 2:
                        if (_tipoTerreno == 2)
                            costo = 1000;
                        else
                            costo += 2;
                        break;
                    case -1:
                    case -2:
                        ;
                        break;
                    default:
                            return 1000;//<--- Es inaccesible devuelve el maximo valor posible
                        
                        break;
                }
            }
            else return 1000;

            return costo;
        }

        /// <summary>
        /// Transfora la casilla en cadena, siendo esta su posicion.
        /// </summary>
        /// <returns>cadena de casilla; tipo String</returns>
        public override string ToString()
        {
            return _posicion.ToString();
        }
 
        #endregion

        #region Propiedades
        /// <summary>
        /// Propiedad que devuelve el nivel de la casilla
        /// </summary>
        /// <returns>nivel; tipo Int</returns>
        public int nivel() {
            return _nivel;
        }
        /// <summary>
        /// Establece el nivel de la casilla
        /// </summary>
        /// <param name="nivel">nivel; tipo Int</param>
        public void nivel(int nivel) {
            _nivel = nivel;
        }

        /// <summary>
        /// Propiedad que devuelve el tipo de terreno de la casilla
        /// </summary>
        /// <returns>tipo de nivel; tipo Int</returns>
        public int tipoTerreno()
        {
            return _tipoTerreno;
        }
        /// <summary>
        /// Establece el tipo de terreno de la casilla
        /// </summary>
        /// <param name="tipoTerreno">tipo de terren; tipo Int</param>
        public void tipoTerreno(int tipoTerreno)
        {
            _tipoTerreno = tipoTerreno;
        }
        
        /// <summary>
        /// Devuelve si hay y que objeto hay en el terreno
        /// </summary>
        /// <returns>tipo de objeto o si no hay; tipo Int</returns>
        public int objetoTerreno()
        {
            return _objetoTerreno;
        }
        /// <summary>
        /// Establece el objeto en el terreno de la casilla
        /// </summary>
        /// <param name="objetoTerreno">tipo de objeto o si no hay 255; tipo Int</param>
        public void objetoTerreno(int objetoTerreno)
        {
            _objetoTerreno = objetoTerreno;
        }

        /// <summary>
        /// el fce del edeificio
        /// </summary>
        /// <returns>fcde; tipo Int</returns>
        public int fceEdificio()
        {
            return _fceEdificio;
        }
        /// <summary>
        /// Establece el fce del Edificio de la casilla
        /// </summary>
        /// <param name="fceEdificio">establece fce; tipo Int</param>
        public void fceEdificio(int  fceEdificio)
        {
            _fceEdificio = fceEdificio;
        }

        /// <summary>
        /// indica si hay un edificio derrumbado
        /// </summary>
        /// <returns>edificio derrumbado; tipo Boolean</returns>
        public Boolean edificioDerrumbado()
        {
            return _edificioDerrumbado;
        }
        /// <summary>
        /// Establece si hay edificio derrumbado en la casilla
        /// </summary>
        /// <param name="edificioDerrumbado">hay edificio derrumbado; tipo Boolean</param>
        public void edificioDerrumbado(Boolean edificioDerrumbado)
        {
            _edificioDerrumbado = edificioDerrumbado;
        }

        /// <summary>
        /// devuelve si hay fuego
        /// </summary>
        /// <returns>hay fuego?; tipo Boolean</returns>
        public Boolean fuego()
        {
            return _fuego;
        }
        /// <summary>
        /// Establece si hay fuega en la casilla
        /// </summary>
        /// <param name="fuego">fueg; tipo Boolean</param>
        public void fuego(Boolean fuego)
        {
            _fuego = fuego;
        }

        /// <summary>
        /// devuelve si hay humo
        /// </summary>
        /// <returns>humo; tipo boolean</returns>
        public Boolean humo()
        {
            return _humo;
        }
        /// <summary>
        /// Establece si hay huma en la una casilla
        /// </summary>
        /// <param name="humo">humo; tipo Boolean</param>
        public void humo(Boolean humo)
        {
            _humo = humo;
        }
        
        /// <summary>
        /// devuelve numero entero de garrotes
        /// </summary>
        /// <returns>numero de garrotes; tipo Int</returns>
        public int nGarrotes()
        {
            return _nGarrotes;
        }
        /// <summary>
        /// Establece el numero de garrotes en la casilla
        /// </summary>
        /// <param name="nGarrotes">numero de garrotes; tipo Int</param>
        public void nGarrotes(int nGarrotes)
        {
            _nGarrotes = nGarrotes;
        }


        /// <summary>
        /// devuelve el vector de 6 caras de la casilla
        /// </summary>
        /// <returns></returns>
        public Cara[] caras() {
            return _caras;
        }
        /// <summary>
        /// Establece las propiedades de las caras de la casilla
        /// </summary>
        /// <param name="caras">establece el vector de 6 caras</param>
        public void caras(Cara[] caras) {
            if (_caras.Length!=0)
                _caras = caras;
        }

        /// <summary>
        /// Establece en la cara i si hay rio y carretera
        /// </summary>
        /// <param name="rio">establece si hay rio; tipo Boolean</param>
        /// <param name="carretera">establece si ahy carretera; tipo Boolean</param>
        /// <param name="i">cara que va establecer sus propiedades</param>
        public void caras(Boolean rio, Boolean carretera, int i)
        {
            try
            {
                _caras[i].rio(rio);
                _caras[i].carretera(carretera);
            }
            catch { 
                _caras[i]=new Cara();
            }
            finally{
                try
                {
                    _caras[i].rio(rio);
                    _caras[i].carretera(carretera);
                }
                catch {
                    Console.Beep(20000, 1000);
                }
            
            }
           
        }

        /// <summary>
        /// devuelve la posicion de la casilla
        /// </summary>
        /// <returns>posicion; tipo Posicion</returns>
        public Posicion posicion(){
            return _posicion;
        }
        /// <summary>
        /// establece la posicion de una casilla
        /// </summary>
        /// <param name="poscion">posicion; tipo Posicion</param>
        public void posicion(Posicion poscion) {
            _posicion = poscion;
        }

        /// <summary>
        /// devuleve la fila de la posicion de la casilla
        /// </summary>
        /// <returns>fila; tipo Int</returns>
        public int fila() {
            return _posicion.fila();
        }

        /// <summary>
        /// devuelve la columna de la posicion de la casilla
        /// </summary>
        /// <returns>columna; tipo int</returns>
        public int columna() {
            return _posicion.columna();
        }
        

        #endregion

        #region Privados
        /// <summary>
        /// posicion de la casilla
        /// </summary>
        private Posicion _posicion;
        /// <summary>
        /// nivel o altura de la casilla
        /// </summary>
        private int _nivel;
        /// <summary>
        /// el tipo de terreno en la casilla
        /// </summary>
        private int _tipoTerreno;
        /// <summary>
        /// si hay edificios en la casilla
        /// </summary>
        private int _objetoTerreno;
        /// <summary>
        /// el fce de edificio en la casilla
        /// </summary>
        private int _fceEdificio;
        /// <summary>
        /// si hay edificios derrumbados en la casilla
        /// </summary>
        private Boolean _edificioDerrumbado;
        /// <summary>
        /// si hay fuefo en la casilla
        /// </summary>
        private Boolean _fuego;
        /// <summary>
        /// si hay humo en la casilla
        /// </summary>
        private Boolean _humo;
        /// <summary>
        /// cuantos o si ahy garrotes en la casilla
        /// </summary>
        private int _nGarrotes;
        /// <summary>
        /// un vector de las 6 caras de la casilla
        /// </summary>
        private Cara[] _caras;
        /// <summary>
        /// costo intrinceco para moverce a esta casilla
        /// </summary>
        private int _puntosMovimientos;
        #endregion
    }
}
