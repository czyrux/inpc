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
            _nivel = _fceEdificio = _tipoTerreno = _objetoTerreno = _nGarrotes = _heristica.f = _heristica.g = _heristica.h = 0;
            _edificioDerrumbado = _fuego = _humo = false;
            _caras = new Cara[6];
            _posicion = new Posicion();
            
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

        public heuristica heuristica() {
            return _heristica;
        }

        public void heuristica(heuristica value) {
            _heristica = value;
        }

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
        private heuristica _heristica;
        #endregion
    }
}
