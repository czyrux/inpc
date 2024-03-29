using System;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
namespace ico
{
    /// <summary>
    /// Clase resopnsable de formar caminos desde la posicion de un mech hasta un posicion deseada, 
    /// devolviendo un camino real(alcanzable) y encarada al enemigo seleccionado
    /// </summary>
    public class Camino
    {
        #region Constructores

        /// <summary>
        /// Constructor de caminos en caso de salto saltos
        /// </summary>
        /// <param name="myJugador">el indice del jugador que hara el movimiento sobre el vector de <paramref name="mechs"/>; tipo int</param>
        /// <param name="destino">posicion destino al que se quiere saltar; tipo posicion</param>
        /// <param name="t"> tablero del juego; tipo Tablero </param>
        /// <param name="objetivo">el indice del mech rival al cual quiera escapar o atacar sobre sobre el vector de <paramref name="mechs"/>; tipo int</param>
        /// <param name="mechs">mechs en el juego; tipo Mech[]</param>
        public Camino(int myJugador, Posicion destino, Tablero t, int objetivo, Mech[] mechs)
        {
            _salta = true;
            _camino = new List<Nodo>();
            Nodo orig = new Nodo();
            orig.casilla(t.Casilla(mechs[myJugador].posicion()));
            orig.direccion((Encaramiento)mechs[myJugador].ladoEncaramiento());
            _final = mejorEncaramiento(mechs[myJugador], mechs[objetivo],destino);
            _camino.Add(orig);

            orig.casilla(t.Casilla(destino));
            _camino.Add(orig);

            //Ordenes del fichero log
            _debug = "\tEl Mech Jugador salta hacia la casilla: " + destino.ToString() + ", con encaramiento: " + _final.ToString();
            _debug += ". Encarandose al mech J-" + mechs[objetivo].numeroJ() + ": " + mechs[objetivo].nombre() + ".\n";
            _debug += "\t\t\t<<< ----------------- >>>\n";
            _debug += "Escribimos en el archivo de accion:\n" + this.ToString();
        }

        /// <summary>
        /// Constructor que crea un camino a traves del pathfinder
        /// </summary>
        /// <param name="myJugador">el indice del jugador que hara el movimiento sobre el vector de <paramref name="mechs"/>; tipo int</param>
        /// <param name="destino">destino del camino a construir; tipo Casilla</param>
        /// <param name="tablero"> tablero del juego; tipo Tablero</param>
        /// <param name="estrategia">estrategia de forma que se hara el camino; tipo estrategia</param>
        /// <param name="objetivo">el indice del mech rival al cual quiera escapar o atacar sobre sobre el vector de <paramref name="mechs"/>; tipo int</param>
        /// <param name="mechs">mechs en el juego; tipo Mech[]</param>
        /// <param name="reversa">indica si se va ir en reversa(esta opcion aun esta en desarrollo)</param>
        public Camino(int myJugador, Casilla destino, Tablero tablero, Estrategia estrategia, int objetivo, Mech[] mechs, Boolean reversa=false ) {
            _ich = mechs[myJugador];
            _estrategia=estrategia;
            _salta = _seLevanto = false;
            _reversa = reversa;
            _camino = new List<Nodo>();
            string tmpstring;


            ArrayList camino = null, tmp;
            if (((MechJugador)mechs[myJugador]).andar() != 0)
            {
                if (estrategia == Estrategia.Defensiva)
                {
                    camino = pathFinder(myJugador, destino, tablero, objetivo, mechs);
                    if ( camino==null)
                    {
                        _debug = "";
                        _estrategia = Estrategia.Agresiva;
                        camino = pathFinder(myJugador, destino, tablero, objetivo, mechs);
                    }
                }
                else
                {
                    tmp = pathFinder(myJugador, destino, tablero, objetivo, mechs);
                    if (mechs[myJugador].posicion() != destino.posicion() && tmp.Count == 1)
                    {
                        tmpstring = _debug;
                        _debug = "";

                        _estrategia = Estrategia.Defensiva;
                        if ((camino = pathFinder(myJugador, destino, tablero, objetivo, mechs)) == null)
                        {
                            _debug = tmpstring;
                            camino = tmp;
                        }
                    }
                    else
                        camino = tmp;
                }

                foreach (Nodo i in camino)
                {
                    _camino.Add(i);
                }

                _debug += "\t\t\t<<< ----------------- >>>\n";

                _debug += "Escribimos en el archivo de accion:\n" + this.ToString();
            }
            else {
                Nodo e = new Nodo();
                e.casilla(tablero.Casilla(mechs[myJugador].posicion()));
                e.direccion((Encaramiento)mechs[myJugador].ladoEncaramiento());
                //_original = e.direccion();
                _final = e.direccion();
                _seLevanto = false;
                _camino.Add(e);
            }
        }

#endregion

        #region Propiedades
        /// <summary>
        /// Devuelve el numero de casillas en el camino
        /// </summary>
        /// <returns>numero de casillas; tipo Int</returns>
        public int longitud() {
            return _camino.Count;
        }
        
        /// <summary>
        /// Devuelve el una lista de Nodos
        /// </summary>
        /// <returns>la lista; tipo Lista de Nodos</returns>
        public List<Nodo> getCamino() {
            return _camino;
        }

#endregion

        #region Funciones publicas
        /// <summary>
        /// Devuelve el costo del movimiento en puntos de movimientos
        /// </summary>
        /// <returns>costo en puntos de movimientos; tipo Int</returns>
        public int costoMovimiento() 
        {
            return limpiarAgua(_camino)[_camino.Count - 1].g() ;
        }

        /// <summary>
        /// Devuelve el nodo i
        /// </summary>
        /// <param name="i">el indice del nodo seleccionado; tipo Nodo</param>
        /// <returns>devuelve el nodo indicado; tipo Nodo</returns>
        public Nodo nodo(int i) {
            return _camino[i];
        }

        /// <summary>
        /// Funcion que imprime el camino
        /// </summary>
        public void print()
        {
            /*string str = "El camino hecho por " + _ich.nombre() + ", con " + ((_estrategia==Estrategia.Agresiva)?((MechJugador)_ich).andar().ToString():((MechJugador)_ich).correr().ToString()) + "PM con costo de camino "+costoMovimiento().ToString()+" es: \n";
            foreach (Nodo i in _camino)
                str += "("+i.casilla().posicion().ToString()+", "+ i.direccion().ToString() +")"+ "->";
            str += _final.ToString();*/
            Console.WriteLine(_debug);
        }

        /// <summary>
        /// Transforma el camino en cadena con formato para el archivo de fase de movimiento
        /// </summary>
        /// <returns>la cadena resultante; tipo String</returns>
        public override string ToString()
        {
            string str,str2="";
            int i = 0, pasos=0;

            if (!_salta)
            {

                if (_estrategia == Estrategia.Agresiva || _seLevanto )
                    str = "Andar\n";
                else
                    str = "Correr\n";

                str += _camino[_camino.Count - 1].casilla().ToString() + "\n";


                if (_final == 0)
                {
                    _final = _camino[_camino.Count - 1].direccion();
                }
                str += ((int)_final).ToString() + "\n";

                str += "False\n";

                if (_seLevanto )
                {
                    str2 += "Levantarse\n";

                    str2 += ((int)((Nodo)_camino[0]).direccion()).ToString() + "\n";//((int)_final).ToString() + "\n";

                    pasos++;
                    if (_camino.Count == 1) {
                        str += pasos.ToString()+"\n";
                        str += str2;
                        return str;
                    }
                }
                int tmp;

                i++;
                while (i != _camino.Count)
                {

                    if (_camino[i - 1].direccion() != _camino[i].direccion())
                    {
                        tmp = costoEncaramiento(_camino[i - 1].direccion(), _camino[i].direccion());
                        str2 += izqOdrch(_camino[i - 1].direccion(), _camino[i].direccion()) + "\n";
                        str2 += tmp.ToString() + "\n";
                        pasos++;
                    }
                    str2 += "Adelante\n";
                    str2 += "1\n";
                    pasos++;
                    i++;

                }

                if (_camino[i - 1].direccion() != _final)
                {
                    str2 += izqOdrch(_camino[i - 1].direccion(), _final) + "\n";
                    str2 += costoEncaramiento(_camino[i - 1].direccion(), _final).ToString() + "\n";
                    pasos++;
                }

                if (pasos == 0)
                    return "Inmovil\n";


                str += pasos.ToString() + "\n";

                str += str2;

            }
            else {//Si saltamos
                str = "Saltar\n";
                str += _camino[_camino.Count - 1].casilla().ToString() + "\n";
                str += ((int)_final).ToString();
            }

            return str;
        }

        /// <summary>
        /// Guarda el camino en un archivo con nombre PanelControl.movimientoArchivo
        /// </summary>
        public void ToFile(int jugador){
            StreamWriter fich = new StreamWriter(PanelControl.archivoAcciones(jugador), false);
            fich.Write(this.ToString());
            fich.Close();
        }
       
        /// <summary>
        /// Devuelve la ultima casilla del destino
        /// </summary>
        /// <returns>Casilla final del camino</returns>
        public Casilla casillaFinal() {
            return _camino[_camino.Count - 1].casilla();
        }

        /// <summary>
        /// Devuelve una cadena con informacion de depuracion
        /// </summary>
        /// <returns>la cadena de este camino; tipo string</returns>
        public string ToDebug() {
            return _debug;
        }

#endregion
       
        #region Privado

        #region Variables
        /// <summary>
        /// Lista de nodos que representan el camino
        /// </summary>
        private List<Nodo> _camino;
        /*/// <summary>
        /// el encaramiento original al inicio del movimiento
        /// </summary>
        private Encaramiento _original;*/
        /// <summary>
        /// el encaramiento final al final del movimiento
        /// </summary>
        private Encaramiento _final;
        /// <summary>
        /// Tipo de estrategia que se uso o usara en el camino
        /// </summary>
        private Estrategia _estrategia;
        /// <summary>
        /// indica que se levanto
        /// </summary>
        private Boolean _seLevanto;
        /// <summary>
        /// indica si salto el mech
        /// </summary>
        private Boolean _salta;
        /// <summary>
        /// indica el mech que hace el camino
        /// </summary>
        private Mech _ich;
        /// <summary>
        /// informacion de depuracion del camino
        /// </summary>
        private string _debug;
        /// <summary>
        /// indica si va en reversa (en desarrollo)
        /// </summary>
        private Boolean _reversa;
        #endregion

        #region Funciones

 
        /// <summary>
        /// funcion que calula el camino de donde esta el mech a un punto intermedio hacia el punto b
        /// </summary>
        /// <param name="myJugador">indice de el mech que hara el camino, sobre el vector <paramref name="mechs"/>; tipo int</param>
        /// <param name="b">destion de el camino; tipo Casilla</param>
        /// <param name="Tablero">tablero del juego; tipo Tablero</param>
        /// <param name="objetivo">indice de el mech al cual queremos a tacar o defendernos, sobre el vector <paramref name="mechs"/>; tipo int</param>
        /// <param name="mechs">vector de mechs en el juego; tipo mechs[]</param>
        /// <returns>una ArrayList de Nodos que representan un camino; tipo ArrayList</returns>
        private ArrayList pathFinder(int myJugador, Casilla b, Tablero Tablero, int objetivo, Mech[] mechs)
        {
            ArrayList cerradas = new ArrayList();
            ArrayList abiertas = new ArrayList();
            ArrayList camino = new ArrayList();
            Nodo elemento = new Nodo();
            int aux = 0, iaux = 0, mejor = -1, gAcumulada = 0;
            Boolean nueva = false;
            //_original = (Encaramiento)mechs[myJugador].ladoEncaramiento();



            elemento.casilla(Tablero.Casilla(mechs[myJugador].posicion()));
            elemento.g(0);
            elemento.h(mechs[myJugador].posicion().distancia(b.posicion()));
            elemento.f(elemento.h());
            elemento.direccion((Encaramiento)mechs[myJugador].ladoEncaramiento());
            elemento.padre(elemento);

            //en caso de que este en el suelo y no tenga piernas no hago nada o giroscopios o puntos de movimiento
            if ((!mechs[myJugador].giroscopioOperativo() || (!mechs[myJugador].conPiernaDerecha() && !mechs[myJugador].conPiernaIzquierda()) 
                || ((MechJugador)mechs[myJugador]).andar() == 0 || ((MechJugador)mechs[myJugador]).andar() == 1)
                && mechs[myJugador].enSuelo() )
            {
                camino.Add(elemento);
                _final = mejorEncaramiento(mechs[myJugador], mechs[objetivo], mechs[myJugador].posicion());
                if (costoEncaramiento(elemento.direccion(), _final) > ((MechJugador)mechs[myJugador]).andar())
                    _final = elemento.direccion();
                return camino;
            }
            // en caso que este en el suelo y me levanto encarado al objetivo y no hago nada
            if (mechs[myJugador].enSuelo() && ((MechJugador)mechs[myJugador]).andar()>1 )
            {
                _seLevanto = true;
                _final = mejorEncaramiento(elemento, mechs[objetivo].posicion(), Tablero);
                //_final = mejorEncaramiento(mechs[myJugador], mechs[objetivo], mechs[myJugador].posicion());
                elemento.direccion(mejorEncaramiento(elemento, mechs[objetivo].posicion(), Tablero));
                //elemento.direccion(mejorEncaramiento(mechs[myJugador], mechs[objetivo], mechs[myJugador].posicion()));
                camino.Add(elemento);
                return camino;
            }

            Nodo actual = elemento;

            cerradas.Add(elemento);
            if (elemento.casilla() != b)
            {
                do
                {
                    nueva = false;

                    for (int i = 1; i < 7; i++)
                    {
                        elemento = new Nodo();
                        try
                        {
                            elemento.casilla(Tablero.colindante(actual.casilla().posicion(), (Encaramiento)i));//<-- hay que revisar en caso de que salga del tablero, aunque con el try funciona.
                        }
                        catch (Exception e)
                        {
                            continue;
                        }

                        // Ignoro las casillas que ya estan en la lista de cerradas.
                        if (esta(cerradas, elemento.casilla()))
                            continue;

                        // Precalculo el costo de movimiento relacional, para no hacerlo varias veces
                        aux = actual.casilla().costoMovimiento(elemento.casilla()) + costoEncaramiento(actual.casilla(), (Encaramiento)i, elemento.casilla(), Tablero);

                        // verifico si es intrancitable
                        if (aux > 100 || Tablero.casillaOcupada(elemento.casilla().posicion(), mechs, myJugador) )
                            continue;
                        // verifico si es intrancitable
                        if (/*elemento.casilla.costoMovimiento() >= 0 ||*/  aux >= 0)
                        {
                            //si es agua menor a 0 y estmos en defensiva
                            if (elemento.casilla().nivel() < 0 && elemento.casilla().tipoTerreno() == 2 && _estrategia == Estrategia.Defensiva)
                            {
                                continue;//<--- no la agrego por ser intransitable la casilla
                            }


                            // precalculo el costo de movimiento total a esa casilla desde la actual
                            elemento.g(elemento.casilla().costoMovimiento() + aux + gAcumulada);

                            // verifico si la casilla actual ya esta entre las abiertas
                            if ((iaux = estaEn(abiertas, elemento.casilla())) >= 0)
                            {

                                // compruebo si desde la aterior era mejor que esta
                                if (elemento.g() >= ((Nodo)abiertas[iaux]).g())
                                {
                                    nueva = true;
                                    // si, si continuo sin hacer cambios
                                    continue;
                                }
                                //sino modifico la casilla antigua
                                else
                                {
                                    nueva = true;
                                    elemento.h(((Nodo)abiertas[iaux]).h());
                                    elemento.f(elemento.g() + elemento.h());
                                    elemento.padre(actual);
                                    elemento.direccion((Encaramiento)i);
                                    abiertas[iaux] = elemento;
                                }

                            }
                            else
                            {//si no estaba entre las abiertas inserto la nueva casilla
                                nueva = true;
                                elemento.h(heuristica(elemento.casilla(), b));
                                elemento.f(elemento.g() + elemento.h());
                                elemento.padre(actual);
                                elemento.direccion((Encaramiento)i);
                                abiertas.Add(elemento);
                            }

                        }


                    }

                    if (abiertas.Count != 0)
                    {
                        //buesca la mejor casilla entre las abiertas
                        mejor = mejorCasillaAbierta(abiertas, actual.casilla(), b);
                        //actualizo la acomulacion de la g
                        gAcumulada = ((Nodo)abiertas[mejor]).g();
                        //agrego la mejor casilla
                        cerradas.Add(abiertas[mejor]);
                        //pongo la mejor como la siguiente actual
                        actual = (Nodo)abiertas[mejor];
                        //borro la mejor de las abiertas
                        abiertas.RemoveAt(mejor);
                        nueva = true;
                    }

                } while (actual.casilla() != b && nueva);
            }
            if (actual.casilla() == b)
            {
                elemento = new Nodo();

                elemento.casilla(b);
                elemento.h(0);
                elemento.f(0);
                elemento.padre((Nodo)cerradas[cerradas.Count - 1]);
                elemento.direccion(elemento.direccion());
                elemento.g(elemento.padre().g() + elemento.casilla().costoMovimiento() + elemento.padre().casilla().posicion().distancia(elemento.casilla().posicion()));
                camino.Add((Nodo)cerradas[cerradas.Count - 1]);

                Nodo padre = (Nodo)cerradas[cerradas.Count - 1];
                do
                {
                    camino.Add(padre.padre());
                    padre = padre.padre();
                } while (padre.casilla() != Tablero.Casilla(mechs[myJugador].posicion()));

                limpiarAgua(camino);

                debugString(camino, myJugador, objetivo, mechs);

                if ((aux = caminoReal(camino, b, mechs[myJugador], Tablero, mechs[objetivo])) == -1)
                    camino = camino.GetRange(camino.Count - 1, 1);
                else
                    camino = camino.GetRange(aux, camino.Count-aux);
                
                camino.Reverse();
                if(camino.Count>1)
                    //Para caminos donde el destino e inicio coinciden
                    if(camino[0]==camino[1])
                        camino.RemoveAt(1);
                
            }
            else
                camino.Add(cerradas[0]);

            if (!sePuedeHacerCorriendo(camino))
                return null;

            debugString(camino, myJugador, objetivo, mechs, false);
            return camino;
        }

        /// <summary>
        /// Funcion que forma la la informacion de depuracion y archivo bitacora; la cual es establecida en la variable _debug
        /// </summary>
        /// <param name="camino">el camino ideal o completo</param>
        /// <param name="my">indice de el mech que hara el camino, sobre el vector <paramref name="mechs"/>; tipo int</param>
        /// <param name="objetivo">indice de el mech al cual queremos a tacar o defendernos, sobre el vector <paramref name="mechs"/>; tipo int</param>
        /// <param name="mechs">vector de mechs en el juego; tipo mechs[]</param>
        /// <param name="ideal">indica si la llamada es de el camino ideal o del el real</param>
        private void debugString(ArrayList camino, int my, int objetivo, Mech[] mechs, Boolean ideal=true) {
            int j = 0;
            
            if (ideal) {
                _debug += "\tIntenta ir a el destino: " +((Nodo)camino[camino.Count-1]).casilla().ToString()+" con " + 
                    (_estrategia == Estrategia.Defensiva ? ((MechJugador)mechs[my]).correr().ToString() : ((MechJugador)mechs[my]).andar().ToString())
                    + "PM de " + (_estrategia == Estrategia.Defensiva ? "correr" : "andar") + ". Encarandose al mech J-" + mechs[objetivo].numeroJ().ToString() +": "+ mechs[objetivo].nombre() + "\n";

                /*for (int i = camino.Count - 1; i > -1; i--) {
                    _debug += "\t(" + ((Nodo)camino[i]).casilla().ToString() + ", " + ((Nodo)camino[i]).direccion().ToString() + ", " + ((Nodo)camino[i]).g().ToString() + "g)\n";
                    _debug += "\t\t\t|\n";
                }*/
            }
            else
            {
                _debug += "\tEl Mech Jugador hace " + (_estrategia == Estrategia.Defensiva ? "corriendo" : "caminando") + " el camino hasta la casilla: ";
                _debug += ((Nodo)camino[camino.Count - 1]).casilla().ToString() + ", con encaramiento: " + _final.ToString() + ".\n";
                _debug += "\tEl camino realizado ha sido:\n";

                foreach (Nodo i in camino)
                {
                    _debug += "\t\t(" + i.casilla().ToString() + ", " + i.direccion().ToString() + ", " + ((Nodo)camino[j]).g().ToString() + "PM )\n";
                    _debug += "\t\t\t\t\t|\n";
                    j++;
                }
            }
            if (_final == 0)
            {
                _final = ((Nodo)camino[camino.Count - 1]).direccion();
            }
            _debug += (ideal ? "" : "\t\tEncarandose hacia: "+_final.ToString()) + "\n";
        }

       /* Boolean hayAlgunMech(Posicion deseada,Mech[] mechs) {
            foreach (Mech i in mechs) 
                if (i.posicion().ToString() == deseada.ToString())
                    return true;
            
            return false;
        }*/
        /// <summary>
        /// comprueba si el camino es recorrible corriendo
        /// </summary>
        /// <param name="camino">el camino ideal; tipo ArrayList</param>
        /// <returns>devuele true si es recorrible corriendo y false por el contrario; tipo bool</returns>
        private bool sePuedeHacerCorriendo(ArrayList camino) {
            foreach (Nodo i in camino)
            {
                if ((i.casilla().tipoTerreno() == 2 && i.casilla().nivel() < 0) && _estrategia==Estrategia.Defensiva)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Indica si hay que girar a la derecha o a la izquierda para ponerse en el encaramiento deseado
        /// </summary>
        /// <param name="o">encaramiento de origen; tipo Encaramiento</param>
        /// <param name="d">encaramiento de destino; tipo Encaramiento</param>
        /// <returns>devuelve si "Izquierda" o "Derecha"; tipo String</returns>
        private string izqOdrch(Encaramiento o, Encaramiento d) {
            switch (o) { 
                case Encaramiento.Abajo:
                    if (d == Encaramiento.InferiorIzquierda || d == Encaramiento.SuperiorIzquierda)
                        return "Derecha";
                    else
                        return "Izquierda";

                case Encaramiento.Arriba:
                    if (d==Encaramiento.InferiorDerecho || d==Encaramiento.SuperiorDerecha)
                        return "Derecha";
                    else
                        return "Izquierda";

                case Encaramiento.InferiorDerecho:
                    if (d==Encaramiento.SuperiorDerecha || d==Encaramiento.Arriba)
                        return "Izquierda";
                    else
                        return "Derecha";

                case Encaramiento.InferiorIzquierda:

                    if (d==Encaramiento.InferiorDerecho || d == Encaramiento.Abajo)
                        return "Izquierda";
                    else
                        return "Derecha";

                case Encaramiento.SuperiorDerecha:
                    if (d == Encaramiento.SuperiorIzquierda || d == Encaramiento.Arriba)
                        return "Izquierda";
                    else
                        return "Derecha";

                case Encaramiento.SuperiorIzquierda:
                    if (d == Encaramiento.SuperiorDerecha || d == Encaramiento.Arriba)
                        return "Derecha";
                    else
                        return "Izquierda";
            }
            return "";

        }

        /// <summary>
        /// Limpia el penalizador de agua de las g de los nodos
        /// </summary>
        /// <param name="camino">el camino de nodos a limpiar; tipo Lista de Nodos(by ref)</param>
        /// <returns>devueleve el mismo camino modificado; tipo Lista de Nodos</returns>
        private List<Nodo> limpiarAgua(List<Nodo> camino)
        {
            int veces = 0;
            for (int i = camino.Count - 2; i > 1; i--)
            {
                ((Nodo)camino[i]).g(((Nodo)camino[i]).g() - PanelControl.penalizadorAgua * veces);
                if (((Nodo)camino[i]).casilla().tipoTerreno() == 2)
                    veces++;
            }
            return camino;
        }

        /// <summary>
        /// Limpia el penalizador de agua de las g de los nodos
        /// </summary>
        /// <param name="camino">el camino de nodos a limpiar; tipo ArrayLists(by ref)</param>
        /// <returns>devueleve el mismo camino modificado; tipo ArrayLists</returns>
        private ArrayList limpiarAgua(ArrayList camino)
        {
            int veces = 0;
            for (int i = camino.Count - 2; i > -1; i--)
            {
                if (((Nodo)camino[i]).casilla().tipoTerreno() == 2)
                    veces++;
                ((Nodo)camino[i]).g(((Nodo)camino[i]).g() - PanelControl.penalizadorAgua * veces);
            }
            return camino;
        }

        /// <summary>
        /// Calcula de un camino entero(a->b) una posicion c, siendo c un indice del camino que representa un nodo intermedio entre a y b, y siendo este c alcanzable con los puntos de movimientos del mech (incluyendo el encaramiento final).
        /// Es decir calcula un indice de nodo alacanzable sobre el camino deseado. Se usa para el pathfinder
        /// </summary>
        /// <param name="camino">el camino deseado; tipo ArrayList</param>
        /// <param name="destino">casilla destino; tipo Casilla</param>
        /// <param name="ich">mech que hara el camino; tipo Mech</param>
        /// <param name="t">tablero del juego; tipo Tablero</param>
        /// <param name="objetivo">indice de el mech al cual queremos a tacar o defendernos; tipo Mech</param>
        /// <returns>indice sobre el camino, el cual es alcanzable por el mech ich</returns>
        private int caminoReal(ArrayList camino, Casilla destino, Mech ich, Tablero t, Mech objetivo)
        {
            int puntosMR, suelo = 0;
            Encaramiento tmpE=Encaramiento.Arriba;

            costoEncaramientoAcumulado(camino);

            //Determinamos los puntos de movimiento de los que disponemos
            if (_estrategia == Estrategia.Defensiva)
            {
                puntosMR = ((MechJugador)ich).correr() - suelo;
            }
            else {
                puntosMR= ((MechJugador)ich).andar() - suelo;
            }

            //copruebo que se pueda levantar si no no hace nada
            if (puntosMR < 0)
            {
                _seLevanto = false;
                //_original = tmpE;//_camino[0].direccion();
                ((Nodo)camino[0]).direccion(tmpE);
            }
            else
            {// o no se a levantado o tine puntos negativos de movimientos

                //Vemos hasta que punto del camino podemos movernos
                for (int i = 0; i < camino.Count; i++)
                {
                    if (((Nodo)camino[i]).g() <= puntosMR)
                    {
                        //tmpE = mejorEncaramiento(((Nodo)camino[i]), objetivo.posicion(), t);
                        if(i!=0){
                            tmpE = mejorEncaramiento(((Nodo)camino[i]), objetivo.posicion(), t, (int)((Nodo)camino[i-1]).direccion());
                        }else
                            tmpE = mejorEncaramiento(((Nodo)camino[i]), objetivo.posicion(), t);
                        
                        if (((Nodo)camino[i]).g() + costoEncaramiento(((Nodo)camino[i]).direccion(), tmpE) <= puntosMR)
                        {
                            _final = tmpE;
                            return i;
                        }


                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// limpia el costo del penalizador por agua
        /// </summary>
        /// <param name="camino">el camino al que va limpiar; tipo ArrayList</param>
        private void costoEncaramientoAcumulado(ArrayList camino) {
            int acumulado=0;
            for (int i = camino.Count - 2; i != -1; i--) { 
                if(((Nodo)camino[i+1]).direccion()!=((Nodo)camino[i]).direccion()){
                    acumulado += costoEncaramiento(((Nodo)camino[i + 1]).direccion(), ((Nodo)camino[i]).direccion());
                }
                ((Nodo)camino[i]).g(acumulado + ((Nodo)camino[i]).g());
            }
        }

       
        /// <summary>
        /// Devuelve cual es el mejor encaramiento de la posicion <paramref name="p"/> para estar encarado con el enemigo <paramref name="enemigo"/>
        /// <paramref name="enemigo"/>
        /// </summary>
        /// <param name="yo">Mech Jugador</param>
        /// <param name="enemigo">Mech oponente</param>
        /// <param name="p">Posicion desde la que observamos</param>
        /// <returns>el mejor encaramiento</returns>
        private Encaramiento mejorEncaramiento(Mech yo, Mech enemigo, Posicion p)
        {
            int /*min = 1000,*/ imin = 0;
            for (int i = 1; i < 7; i++)
            {
                if (p.conoDelantero(enemigo.posicion(), i))
                {
                    return (Encaramiento)i;
                    /*if (costoEncaramiento((Encaramiento)yo.ladoEncaramiento(), (Encaramiento)i) < min)
                    {
                        min = costoEncaramiento((Encaramiento)yo.ladoEncaramiento(), (Encaramiento)i);
                        imin = i;
                    }*/
                }
            }
            return (Encaramiento)imin;
        }

        /// <summary>
        /// Devuelve cual es el mejor encaramiento de la posicion <paramref name="p"/> para estar encarado con el enemigo <paramref name="enemigo"/>
        /// <paramref name="enemigo"/>
        /// </summary>
        /// <param name="yo">Mech Jugador</param>
        /// <param name="enemigo">Mech oponente</param>
        /// <param name="p">Posicion que queremos observar</param>
        /// <param name="siguiente">el encaramiento siguente a encarar</param>
        /// <returns>el mejor encaramiento</returns>
        private Encaramiento mejorEncaramiento(Mech yo, Mech enemigo , Posicion p, Encaramiento siguiente) { 
            int min=1000, imin=0;
            for (int i = 1; i < 7;i++)
            {
                if (p.conoDelantero(enemigo.posicion(), i)) {
                    if (((Encaramiento)i) == siguiente) {
                        imin = i;
                        break;
                    }
                    if ( costoEncaramiento((Encaramiento)yo.ladoEncaramiento(), (Encaramiento)i) < min)
                    {
                        min = costoEncaramiento((Encaramiento)yo.ladoEncaramiento(), (Encaramiento)i);
                        imin = i;
                    }
                }
            }
            return (Encaramiento)imin;
        }

 
        /*Encaramiento mejorEncaramiento(Nodo origen, Posicion objetivo, Tablero t)
        {
            List<int> l= new List<int>();
            int min, tmp, tmpi=0;

            l = posiblesEncaramientos(origen, objetivo, t);
            min = costoEncaramiento(origen.direccion(), (Encaramiento)l[0]);

            for (int c = 1; c < l.Count; c++)
            {
                tmp = costoEncaramiento(origen.direccion(), (Encaramiento)l[c]);

                if ( tmp < min)
                {
                    tmpi = c;
                    min = tmp;
                }
            }


            return (Encaramiento)l[tmpi];

        }*/
        //--------------------------------------------------------------------
        /// <summary>
        /// Devuelve cual es el mejor encaramiento del nodo <paramref name="origen"/> para estar encarado con la posicion <paramref name="objetivo"/>
        /// </summary>
        /// <param name="origen">nodo de origen; tipo Nodo</param>
        /// <param name="objetivo">la posiscion del enemigo; tipo Posicion</param>
        /// <param name="t">tablero del juego; tipo tablero</param>
        /// <param name="siguienteEncaramiento">indica cual es el encaramiento para movernos a la siguiente casilla; tipo int</param>
        /// <returns>el mejor encaramiento; tipo encaramiento</returns>
        Encaramiento mejorEncaramiento(Nodo origen, Posicion objetivo, Tablero t, int siguienteEncaramiento=0)
        {
            List<int> l = new List<int>();
            int min, tmp, tmpi = 0;

            l = posiblesEncaramientos(origen, objetivo, t);
            min = costoEncaramiento(origen.direccion(), (Encaramiento)l[0]);

            for (int c = 1; c < l.Count; c++)
            {
                tmp = costoEncaramiento(origen.direccion(), (Encaramiento)l[c]);
                //--------------------------------------------------------------
                if (c == siguienteEncaramiento)
                {
                    return (Encaramiento)c;
                }
                //--------------------------------------------------------------
                if (tmp < min)
                {
                    tmpi = c;
                    min = tmp;
                }
            }

            return (Encaramiento)l[tmpi];

        }

        /*private Encaramiento queEncaramientoVengo(Casilla  origen, Casilla destino, Tablero t) {
            Encaramiento oEncaramiento=Encaramiento.Arriba;
            Casilla tmp;
            for (int i = 1; i < 7; i++) {
                tmp = t.colindante(origen.posicion(), (Encaramiento)i);
                if(tmp==destino){
                    oEncaramiento=(Encaramiento)i;
                    break;
                }
            }
            switch ((Encaramiento)oEncaramiento)
            {
                case Encaramiento.Abajo:
                    return Encaramiento.Arriba;

                case Encaramiento.Arriba:
                    return Encaramiento.Abajo;

                case Encaramiento.InferiorDerecho:
                    return Encaramiento.SuperiorIzquierda;

                case Encaramiento.InferiorIzquierda:
                    return Encaramiento.SuperiorDerecha;

                case Encaramiento.SuperiorDerecha:
                    return Encaramiento.InferiorIzquierda;

                case Encaramiento.SuperiorIzquierda:
                    return Encaramiento.InferiorDerecho;
            }
            return Encaramiento.Arriba;
        }*/
        /// <summary>
        /// Calcula los posibles encaramientos que esten mirando hacia la casilla destino. Usada en caminoReal.
        /// </summary>
        /// <param name="o">el nodo en el que se localiza; tipo Nodo</param>
        /// <param name="destino">casilla hacia el que queremos mirar; tipo Casilla</param>
        /// <param name="t">tablero del juego; tipo Tablero</param>
        /// <returns>devuelve los encaramientos que miran hacia el destino; tipo Int(Encaramiento)</returns>
        private List<int> posiblesEncaramientos(Nodo o, Posicion destino, Tablero t)
        {
            int min =10000, tmp = 0;
            Casilla tmpCasilla;
            List<int> l = new List<int>();
            //cal
            for (int i = 1; i < 7; i++)
            {
                try
                {
                    tmpCasilla = t.colindante(o.casilla().posicion(), (Encaramiento)i);

                    if (tmpCasilla.posicion().ToString() == destino.ToString())
                    {
                        l.Add(i);
                        return l;
                    }

                    tmp = tmpCasilla.posicion().distancia(destino);
                    if (min > tmp)
                    {
                        min = tmp;
                    }
                }
                catch (Exception e) { }
            }


            for (int i = 1; i < 7; i++)
            {
                try { 
                    tmp = t.colindante(o.casilla().posicion(), (Encaramiento)i).posicion().distancia(destino);

                    if (tmp == min)
                    {
                        min = tmp;
                        l.Add(i);
                    }
                }
                catch(Exception e) { }

            }

            return l;
        }

        /// <summary>
        /// calcula el costo de asociado de moverce de la Origen(casilla, direccion)->Destino(casilla, direccion)
        /// </summary>
        /// <param name="origenCasilla">casilla desde donde quiere moverce; tipo Casilla</param>
        /// <param name="direccion">encaramiento desde cual queremos movernos; tipo Encaramiento</param>
        /// <param name="destinoCasilla">casilla donde queremos acabar; tipo Casilla</param>
        /// <param name="t">tablero del juego; tipo Tablero</param>
        /// <returns>devuelve el costo asociado en puntos de movimientos; tipo Int</returns>
        private int costoEncaramiento(Casilla origenCasilla, Encaramiento direccion, Casilla destinoCasilla, Tablero t)
        {
            if (t.colindante(origenCasilla.posicion(), direccion) == destinoCasilla)
                return 0;
            else if (t.colindante(origenCasilla.posicion(), direccion + 1) == destinoCasilla || t.colindante(origenCasilla.posicion(), direccion + 5) == destinoCasilla)
                return 1;
            else if (t.colindante(origenCasilla.posicion(), direccion + 2) == destinoCasilla || t.colindante(origenCasilla.posicion(), direccion + 4) == destinoCasilla)
                return 2;
            else
                return 3;
        }
      
        /// <summary>
        /// Calcula el costo de moverce (solo el giro), de un encaramiento a otro.
        /// </summary>
        /// <param name="o">encaramiento donde empezamos; tipo Encaramiento</param>
        /// <param name="des">encaramiento donde queremos acabar; tipo Encaramiento</param>
        /// <returns>costo del giro indicado en puntos de movimientos; tipo Int</returns>
        private int costoEncaramiento( Encaramiento o, Encaramiento des)
        {
            switch (Math.Abs(o - des))
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 3;
                case 4:
                    return 2;
                case 5:
                    return 1;
            }
            return -1;
        }
        
        /// <summary>
        /// funcion que calcula los valores heuristicos siendo menor el mejor.
        /// </summary>
        /// <param name="a">casilla de origen</param>
        /// <param name="b">casilla de destino</param>
        /// <returns>el valor heuristicos; tipo Int</returns>
        private int heuristica(Casilla a, Casilla b)
        {
            int h = 0;

            //Distancia aproximada
            //h = Posicion.distancia(a.posicion(), b.posicion());// Calculo de la distancia aproximada al objetivo.
            //Distancia real
            h = a.posicion().distancia(b.posicion());

            return h;
        }
        
        /// <summary>
        /// Elige la mejor casilla de las casillas posibles(abiertas); usado en el pathfinder.
        /// </summary>
        /// <param name="abiertas">array de casillas posibles(abiertas); tipo ArrayList</param>
        /// <param name="padre">padre de las casilla elegibles para el final; tipo Casilla</param>
        /// <param name="destino">destio final deseado del camino; tipo Casilla</param>
        /// <returns>el indice (referencia a abiertas) de la mejor casilla; tipo Int</returns>
        private int mejorCasillaAbierta(ArrayList abiertas,Casilla padre, Casilla destino) {
            int max = 0;
            for (int i = 1; i < abiertas.Count; i++ )
            {
                if (((Nodo)abiertas[i]).casilla() == destino && ((Nodo)abiertas[i]).padre().casilla() == padre)
                        return i;
                if (((Nodo)abiertas[max]).f() > ((Nodo)abiertas[i]).f() )
                        max = i;
            }
            return max;
        }

        /// <summary>
        /// verifica si un elemento esta en una lista: si esta devuelve el indice de donde esta; si no devuelve -1
        /// </summary>
        /// <param name="lista">lista donde se buscara el elemento, tipo ArrayList</param>
        /// <param name="elem">elemento a buscar, tipo Casilla</param>
        /// <returns>indice en donde ha encontrado el elemento, -1 por el contrario; tipo Int</returns>
        private int estaEn(ArrayList lista, Casilla  elem) {
            int n = 0;
            foreach (Nodo i in lista)
            {
                if (i.casilla() == elem)
                    return n;
                n++;
            }
            return -1;
        }

        /// <summary>
        ///  verifica si un elemento esta en una lista, devolviendo True si esta; false por el contrario
        /// </summary>
        /// <param name="lista">lista donde se buscara el elemento, tipo ArrayList</param>
        /// <param name="elem">elemento a buscar, tipo Casilla</param>
        /// <returns>devuelve true si lo encuetra false si no; tipo Boolean</returns>
        private Boolean esta(ArrayList lista, Casilla elem) {
            foreach (Nodo i in lista)
            {
                if (i.casilla() == elem)
                    return true;
            }
            return false;
        }
        #endregion
        #endregion

    }
}
