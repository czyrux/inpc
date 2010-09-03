using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace ico
{
    public class Casilla
    {
        #region Constructores
        public Casilla() {
            _nivel = _fceEdificio = _tipoTerreno = _objetoTerreno = _nGarrotes = /*_heuristica.g = */0;
            //_heuristica.f = _heuristica.h = 0;
            _edificioDerrumbado = _fuego = _humo = false;
            _caras = new Cara[6];
            _posicion = new Posicion();
            _puntosMovimientos = 0;
            
        }
        public Casilla(int nivel, int tipoTerreno, int objetoTerreno, int fceEdificio, Boolean edificioDerrumbado,
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
        public Casilla(int nivel, int tipoTerreno, int objetoTerreno, int fceEdificio, Boolean edificioDerrumbado,
   Boolean fuego, Boolean humo, int nGarrotes)
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
        }
#endregion

        #region "publicas"
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

       /* public static int costoMovimiento(Casilla de, Casilla a)
        {
            int costo = 0;
            if (Math.Abs(de.columna() - a.columna()) < 2 || Math.Abs(de.fila() - a.fila()) < 2)
            {
                switch (Math.Abs(a.nivel() - de.nivel()))
                {
                    case 0:
                        ;
                    break;
                    case 1:
                        costo++;
                        break;
                    case 2:
                        if (de.tipoTerreno() == 2)
                            costo = 1000;
                        else
                            costo += 2;
                        break;
                    default:
                        return 1000;//<--- Es inaccesible devuelve el maximo valor posible

                }
            }
            else return int.MaxValue;

            return costo;
        }*/
        const int agua=10;
        public int costoMovimiento( Casilla a)
        {
            int costo = 0;
            if (a.tipoTerreno() == 2)
                costo = agua;
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

        #endregion

        #region Propiedades
        public int nivel() {
            return _nivel;
        }
        public void nivel(int nivel) {
            _nivel = nivel;
        }

        public int tipoTerreno()
        {
            return _tipoTerreno;
        }
        public void tipoTerreno(int tipoTerreno)
        {
            _tipoTerreno = tipoTerreno;
        }

        public int objetoTerreno()
        {
            return _objetoTerreno;
        }
        public void objetoTerreno(int objetoTerreno)
        {
            _objetoTerreno = objetoTerreno;
        }

        public int fceEdificio()
        {
            return _fceEdificio;
        }
        public void fceEdificio(int  fceEdificio)
        {
            _fceEdificio = fceEdificio;
        }

        public Boolean edificioDerrumbado()
        {
            return _edificioDerrumbado;
        }
        public void edificioDerrumbado(Boolean edificioDerrumbado)
        {
            _edificioDerrumbado = edificioDerrumbado;
        }

        public Boolean fuego()
        {
            return _fuego;
        }
        public void fuego(Boolean fuego)
        {
            _fuego = fuego;
        }

        public Boolean humo()
        {
            return _humo;
        }
        public void humo(Boolean humo)
        {
            _humo = humo;
        }

        public int nGarrotes()
        {
            return _nGarrotes;
        }
        public void nGarrotes(int nGarrotes)
        {
            _nGarrotes = nGarrotes;
        }

        public Cara[] caras() {
            return _caras;
        }
        public void caras(Cara[] caras) {
            if (_caras.Length!=0)
                _caras = caras;
        }

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

        public Posicion posicion(){
            return _posicion;
        }

        public void posicion(Posicion poscion) {
            _posicion = poscion;
        }

        public int fila() {
            return _posicion.fila();
        }

        public int columna() {
            return _posicion.columna();
        }
        public override string ToString()
        {
            return _posicion.ToString();
        }
       /* public heuristica heuristica() {
            return _heuristica;
        }

        public void heuristica(heuristica value) {
            _heuristica = value;
        }*/

        #endregion

        #region Privados
        private Posicion _posicion;
        private int _nivel;
        private int _tipoTerreno;
        private int _objetoTerreno;
        private int _fceEdificio;
        private Boolean _edificioDerrumbado;
        private Boolean _fuego;
        private Boolean _humo;
        private int _nGarrotes;
        private Cara[] _caras;
        private int _puntosMovimientos;
        //private heuristica _heuristica;
        #endregion
    }
}
