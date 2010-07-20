using System;
using System.Collections;
//using System.Linq;
using System.Text;
using System.IO;

namespace ico {
	
    class Inpc {
			
        static void Main(string[] args) {
			
			int numeroJugador=2;
			string fase="Movimiento";
			BattleTech juego = new BattleTech(numeroJugador,fase);
			
			juego.pruebas();
        }
		
		
    }
}
