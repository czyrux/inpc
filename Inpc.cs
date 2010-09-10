using System;
using System.Collections;
//using System.Linq;
using System.Text;
using System.IO;

namespace ico {
	
    class Inpc {

		/// <summary>
		/// funcion main del programa
		/// </summary>
		/// <param name="args">revive en los argumentos el jugador y la fase en la que esta</param>
	
        static void Main(string[] args) {

            int numeroJugador = 1;
            string fase = "Movimiento";

            //En caso de tener argumentos
            if (args.Length > 0) {
                numeroJugador = Convert.ToInt32(args[0]);
                fase = args[1];
            }
			

			BattleTech juego = new BattleTech(numeroJugador,fase);
        }
		
		
    }
}
