using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace ico
{
    public class Cara
    {
        public Cara() {
            Boolean _rio = _carretera = false;
        }
        public Cara(Boolean rio, Boolean carretera)
        {
            _rio = false;
            _carretera = false;
        }
        public Boolean  rio() {
            return _rio;
        }
        public void rio(Boolean rio) {
            _rio = rio;
        }
        public Boolean carretera()
        {
            return _carretera;
        }
        public void carretera(Boolean carretera)
        {
            _carretera = carretera;
        }
        private Boolean _rio;
        private Boolean _carretera;
    }
}
