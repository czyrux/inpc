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
                _puntosMovimientos = 1;
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
                    default:
                        if (a.nivel() - _nivel < 0)
                            costo += 0;
                        else
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
        /// <param name="tipoTerreno"></param>
        public void tipoTerreno(int tipoTerreno)
        {
            _tipoTerreno = tipoTerreno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int objetoTerreno()
        {
            return _objetoTerreno;
        }
        /// <summary>
        /// Establece el objeto en el terreno de la casilla
        /// </summary>
        /// <param name="objetoTerreno"></param>
        public void objetoTerreno(int objetoTerreno)
        {
            _objetoTerreno = objetoTerreno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int fceEdificio()
        {
            return _fceEdificio;
        }
        /// <summary>
        /// Establece el fceEdificio de la casilla
        /// </summary>
        /// <param name="fceEdificio"></param>
        public void fceEdificio(int  fceEdificio)
        {
            _fceEdificio = fceEdificio;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean edificioDerrumbado()
        {
            return _edificioDerrumbado;
        }
        /// <summary>
        /// Establece si hay edificio derrumbado en la casilla
        /// </summary>
        /// <param name="edificioDerrumbado"></param>
        public void edificioDerrumbado(Boolean edificioDerrumbado)
        {
            _edificioDerrumbado = edificioDerrumbado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean fuego()
        {
            return _fuego;
        }
        /// <summary>
        /// Establece si hay fuega en la casilla
        /// </summary>
        /// <param name="fuego"></param>
        public void fuego(Boolean fuego)
        {
            _fuego = fuego;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean humo()
        {
            return _humo;
        }
        /// <summary>
        /// Establece si hay huma en la una casilla
        /// </summary>
        /// <param name="humo"></param>
        public void humo(Boolean humo)
        {
            _humo = humo;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int nGarrotes()
        {
            return _nGarrotes;
        }
        /// <summary>
        /// Establece el numero de garrotes en la casilla
        /// </summary>
        /// <param name="nGarrotes"></param>
        public void nGarrotes(int nGarrotes)
        {
            _nGarrotes = nGarrotes;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Cara[] caras() {
            return _caras;
        }
        /// <summary>
        /// Establece las propiedades de las caras de la casilla
        /// </summary>
        /// <param name="caras"></param>
        public void caras(Cara[] caras) {
            if (_caras.Length!=0)
                _caras = caras;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rio"></param>
        /// <param name="carretera"></param>
        /// <param name="i"></param>
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
        /// 
        /// </summary>
        /// <returns></returns>
        public Posicion posicion(){
            return _posicion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poscion"></param>
        public void posicion(Posicion poscion) {
            _posicion = poscion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int fila() {
            return _posicion.fila();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int columna() {
            return _posicion.columna();
        }
        

        #endregion

        #region Privados
        /// <summary>
        /// 
        /// </summary>
        private Posicion _posicion;
        /// <summary>
        /// 
        /// </summary>
        private int _nivel;
        /// <summary>
        /// 
        /// </summary>
        private int _tipoTerreno;
        /// <summary>
        /// 
        /// </summary>
        private int _objetoTerreno;
        /// <summary>
        /// 
        /// </summary>
        private int _fceEdificio;
        /// <summary>
        /// 
        /// </summary>
        private Boolean _edificioDerrumbado;
        /// <summary>
        /// 
        /// </summary>
        private Boolean _fuego;
        /// <summary>
        /// 
        /// </summary>
        private Boolean _humo;
        /// <summary>
        /// 
        /// </summary>
        private int _nGarrotes;
        /// <summary>
        /// 
        /// </summary>
        private Cara[] _caras;
        /// <summary>
        /// 
        /// </summary>
        private int _puntosMovimientos;
        #endregion
    }
}
