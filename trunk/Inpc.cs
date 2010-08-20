using System;
using System.Collections;
//using System.Linq;
using System.Text;
using System.IO;

namespace ico {
	
    class Inpc {
			
        static void Main(string[] args) {

            int numeroJugador = 3;//=1;
            string fase = "AtaqueArmas";//"Movimiento";
            
            //En caso de tener argumentos
            if (args.Length > 0) {
                numeroJugador = Convert.ToInt32(args[0]);
                fase = args[1];
                Console.WriteLine(numeroJugador);
                Console.WriteLine(fase);
            }
			
            
            //QUE CARAJO ES ESTO?¿
            int i = int.MaxValue;
            i++;
            //
			BattleTech juego = new BattleTech(numeroJugador,fase);
        }
		
		
    }
}
