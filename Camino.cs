using System;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.IO;
namespace ico
{
    public class Camino
    {
#region Constructores
        public Camino(Posicion p1, Posicion p2,Tablero tablero) {
            Process proc = new Process();
            _moviminetos = _nldv = 0;
            String[] nodos;
            proc.StartInfo.WorkingDirectory = @".";
            proc.StartInfo.FileName = "LDVyC.exe";
            proc.StartInfo.Arguments = "mapaJ1.sbt " + p1.ToString() + " " + tablero.Casilla(p1).nivel().ToString() + " " + p2.ToString() + " " + tablero.Casilla(p2).nivel().ToString() + "";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();
            proc.WaitForExit();
            proc.Close();
            StreamReader fich = new StreamReader("LDV.sbt");
            nodos = fich.ReadLine().Split(' '); 
            _ldv = Convert.ToBoolean(fich.ReadLine());
            _cobertura = Convert.ToBoolean(fich.ReadLine());
            fich.Close();

            _camino = new Casilla[nodos.Length + 2];


            _camino[0] = tablero.Casilla(p1);
            for (int i = 1; i <= nodos.Length; i++) {
                _camino[i] = tablero.Casilla(new Posicion(nodos[i - 1]));
            }
            _camino[nodos.Length+1] = tablero.Casilla(p2);

            //rellenamos heuristica?¿
            for (int i = 0; i < _length; i++) {
                if (_camino[i].tipoTerreno() == 2) {

                }
            }
        }
        public Camino(ArrayList camino)
        {
            _length = camino.Count;
            _camino = new Casilla[_length];
            _ldv =_cobertura= false;
            _nldv = _moviminetos = 0;
        }
#endregion

#region Propiedades

        int longitud() {
            return _length;
        }
        Casilla[] getCamino() {
            return _camino;
        }
        Boolean ldv() {
            return _ldv;
        }
        Boolean cobertura() {
            return _cobertura;
        }
#endregion
#region "Funciones"
        public Camino pathFinder(Casilla a, Casilla b, Casilla[] Tablero)
        {
            ArrayList cerrado = new ArrayList();
            ArrayList abierto = new ArrayList();
            int n = 0;
            //adios antonio
            //hola angel
            return new Camino(new ArrayList());


        }

        int costoMovimiento() {
            int costo = 0;

            for (int i = 0; i < _length; i++)
            {
                switch (_camino[i].tipoTerreno()) { 
                    case 0://despejado
                        costo++;
                    break;
                    case 1://pavimentado
                        costo++;
                    break;
                    case 2://agua
                    switch (_camino[i].nivel()) { // ver comentarios de el agua en el log.
                        case 0:
                            costo++;
                        break;
                        case -1:
                            costo += 2;
                        break;

                        default:
                            costo += 4;
                        break;
                    }
                    break;

                    case 3://pantanoso
                        costo++;
                    break;
                    switch (_camino[i].objetoTerreno()) { 

                        case 0://escombros
                            costo += 2;
                            break;
                        case 1://bosque disperso
                            costo += 2;
                            break;
                        case 2://bosque denso
                            costo += 3;
                            break;
                        case 3: //edificio ligero
                            costo += 2;
                            break;
                        case 4://edificio medio
                            costo += 3;
                            break;
                        case 5: //edificio grande o pesado
                            costo += 4;
                            break;
                        case 6: //edificio reforzado
                            costo += 5;
                            break;
                    }
                }
            }
            return costo;
        }
#endregion
        #region Privado
        private int _length;
        private Casilla[] _camino;
        private Boolean _ldv;
        private Boolean _cobertura;
        private int _nldv;
        private int _moviminetos;

        private Camino pathfinder(Casilla a, Casilla b, Tablero tablero,ArrayList ceradas, ArrayList abiertas) {

            if (ceradas[ceradas.Count - 1] == b)
            {
            }
            else {
                calculaAbiertas(a,  ref abiertas, tablero);
                foreach (Casilla i in abiertas)
                {

                }
            }
            return new Camino(abiertas);//compile
        }
		
        private void calculaAbiertas(Casilla a, ref ArrayList abierto, Tablero tablero) {

            if (Math.Abs(tablero.colindante(a, Encaramiento.Arriba).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.Arriba));
            }
            if (Math.Abs(tablero.colindante(a, Encaramiento.SuperiorDerecha).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.SuperiorDerecha));
            } if (Math.Abs(tablero.colindante(a, Encaramiento.InferiorDerecho).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.InferiorDerecho));
            } if (Math.Abs(tablero.colindante(a, Encaramiento.Abajo).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.InferiorDerecho));
            } if (Math.Abs(tablero.colindante(a, Encaramiento.Abajo).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.Abajo));
            } if (Math.Abs(tablero.colindante(a, Encaramiento.InferiorIzquierda).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.InferiorIzquierda));
            } if (Math.Abs(tablero.colindante(a, Encaramiento.SuperiorIzquierda).nivel() - a.nivel()) < 2 && tablero.colindante(a, Encaramiento.Arriba).nivel() != -1)
            {
                abierto.Add(tablero.colindante(a, Encaramiento.SuperiorIzquierda));
            }
        } 
#endregion

    }
}
