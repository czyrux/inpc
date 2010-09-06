using System;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
namespace ico
{
    public class Camino
    {
        #region Constructores
        
        /// <summary>
        /// Constructor que crea un camino a traves del pathfinder
        /// </summary>
        /// <param name="de">origen del camino a construir; tipo Casilla</param>
        /// <param name="ich">mi mech; tipo Mech</param>
        /// <param name="a">destino del camino a construir; tipo Casilla</param>
        /// <param name="tablero">tablero del juego; tipo Tablero</param>
        /// <param name="estrategia">estrategia conforme a la cual se hara el combate; tipo Estrategia</param>
        public Camino(Casilla de,Mech ich, Casilla a, Tablero tablero, Estrategia estrategia) {
           _estrategia=estrategia;
           ArrayList camino = pathFinder(de, ich, a, tablero);
           int j = 0;

           _camino = new List<Nodo>();
          foreach (Nodo i in camino) {
              _camino.Add(i);
              j++;
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
        /// <returns>costoen puntos de movimientos; tipo Int</returns>
        public int costoMovimiento() 
        {
            return limpiarAgua(_camino)[_camino.Count - 2].g() + limpiarAgua(_camino)[_camino.Count - 2].casilla().posicion().distancia(limpiarAgua(_camino)[_camino.Count-1].casilla().posicion());
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
            string str="El camino es: ";
            foreach (Nodo i in _camino)
                str += "("+i.casilla().posicion().ToString()+", "+ i.direccion().ToString() +")"+ "->";
            str += "FIN";
            Console.WriteLine(str);
        }

        /// <summary>
        /// Transforma el camino en cadena con formato para el archivo de fase de movimiento
        /// </summary>
        /// <returns>la cadena resultante; tipo String</returns>
        public override string ToString()
        {
            string str,str2="";
            int i = 0, pasos=0;

            if (_estrategia == Estrategia.Agresiva)
                str = "Andar\n";
            else
                str = "Correr\n";

            str += _camino[_camino.Count - 1].casilla().ToString()+"\n";

            str += ((int)_final).ToString()+"\n";

            str += "False\n";

            if (_seLevanto) {
                str2 += "Levantarse\n";
                str2 += ((int)_original).ToString();
                pasos++;
            }
            int tmp;
            if (_camino[i].direccion() != _original) {
                str2 += izqOdrch(_camino[i].direccion(), _original).ToString();
                tmp = costoEncaramiento(_original, _camino[i].direccion());
                str2 += tmp.ToString()+"\n";
                pasos++;
            }

            i++;
            while (i != _camino.Count)
            {

                if (_camino[i-1].direccion() != _camino[i].direccion())
                {
                    tmp = costoEncaramiento(_camino[i - 1].direccion(), _camino[i].direccion());
                    str2 += izqOdrch(_camino[i-1].direccion(), _camino[i].direccion()) + "\n";
                    str2 += tmp.ToString() + "\n";
                    pasos++;
                }
                str2 += "Adelante\n";
                str2 += "1\n";
                pasos++;
                i++;
                
            }

            if (_camino[i-1].direccion() != _final)
            {
                str2 += izqOdrch(_camino[i-1].direccion(),_final) + "\n";
                str2 += costoEncaramiento(_camino[i-1].direccion(), _final).ToString() + "\n";
                pasos++;
            }
            
            str += pasos.ToString()+"\n";

            str += str2;

            return str;
        }

        /// <summary>
        /// Guarda el camino en un archivo con nombre PanelControl.movimientoArchivo
        /// </summary>
        public void ToFile(){
            StreamWriter fich = new StreamWriter(PanelControl.movimientoArchivo);
            fich.Write(this.ToString());
        }

        /// <summary>
        /// Devuelve la ultima casilla del destino
        /// </summary>
        /// <returns>Casilla final del camino</returns>
        public Casilla casillaFinal() {
            return _camino[_camino.Count - 1].casilla();
        }
#endregion
       
        #region Privado

        #region Variables
        /// <summary>
        /// Lista de nodos que representan el camino
        /// </summary>
        private List<Nodo> _camino;
        /// <summary>
        /// el encaramiento original al inicio del movimiento
        /// </summary>
        private Encaramiento _original;
        /// <summary>
        /// el encaramiento final al final del movimiento
        /// </summary>
        private Encaramiento _final;
        /// <summary>
        /// Tipo de estrategia que se uso o usara en el camino
        /// </summary>
        private Estrategia _estrategia;
        /// <summary>
        /// si al inicio el mech estaba en el suelo
        /// </summary>
        private Boolean _seLevanto;
        #endregion

        #region Funciones
        /// <summary>
        /// funcion que calula el camino de un punto a a un punto intermedio hacia el punto b
        /// </summary>
        /// <param name="a">origen del camino a construir; tipo Casilla</param>
        /// <param name="ich">mi mech; tipo Mech</param>
        /// <param name="b">destino del camino a construir; tipo Casilla</param>
        /// <param name="Tablero">tablero del juego; tipo Tablero</param>
        /// <returns>devuelve el camino del punto a al punto untermedio; tipo Camino</returns>
        private ArrayList pathFinder(Casilla a, Mech ich, Casilla b, Tablero Tablero)
        {
            ArrayList cerradas = new ArrayList();
            ArrayList abiertas = new ArrayList();
            ArrayList camino = new ArrayList();
            Nodo elemento = new Nodo();
            int aux = 0, iaux = 0, mejor = -1, gAcumulada = 0;
            Boolean nueva = false;
            _original = (Encaramiento)ich.ladoEncaramiento();

            elemento.casilla(a);
            elemento.g(0);
            elemento.h(a.posicion().distancia(b.posicion()));
            elemento.f(elemento.h());
            elemento.direccion((Encaramiento)ich.ladoEncaramiento());
            elemento.padre(elemento);

            Nodo actual = elemento;

            cerradas.Add(elemento);
            do
            {
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
                    //aux = actual.posicion().distancia(elemento.casilla.posicion());
                    if (aux > 100)
                        continue;
                    // verifico si es intrancitable
                    if (/*elemento.casilla.costoMovimiento() >= 0 ||*/  aux >= 0)
                    {

                        // precalculo el costo de movimiento total a esa casilla desde la actual
                        elemento.g(elemento.casilla().costoMovimiento() + aux + gAcumulada);

                        // verifico si la casilla actual ya esta entre las abiertas
                        if ((iaux = estaEn(abiertas, elemento.casilla())) >= 0)
                        {

                            // compruebo si desde la aterior era mejor que esta
                            if (elemento.g() >= ((Nodo)abiertas[iaux]).g())
                            {
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

                    nueva = false;


                }

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

            } while (actual.casilla() != b);

            elemento = new Nodo();

            elemento.casilla(b);
            elemento.g(0);
            elemento.h(0);
            elemento.f(0);
            elemento.direccion((Encaramiento)ich.ladoEncaramiento());
            elemento.padre((Nodo)cerradas[cerradas.Count - 1]);
            camino.Add(elemento);

            Nodo padre = (Nodo)cerradas[cerradas.Count - 1];
            do
            {
                camino.Add(padre.padre());
                padre = padre.padre();
            } while (padre.casilla() != a);




            if ((aux = caminoReal(limpiarAgua(camino), b, ich, Tablero)) == -1)
                camino = camino.GetRange(camino.Count-1,1);
            else
                camino = camino.GetRange(aux, camino.Count - aux);

            camino.Reverse();


            return camino;
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
                            if((int)d>4)
                                return "Derecha";
                            else
                                return "Izquierda";
                   
                    break;
                case Encaramiento.Arriba:

                    if ((int)d < 4)
                        return "Derecha";
                    else
                        return "Izquierda";
                    
                    break;
                case Encaramiento.InferiorDerecho:

                    if ((int)d <3 || d==Encaramiento.SuperiorIzquierda)
                        return "Izquierda";
                    else
                        return "Derecha";
                    
                    break;
                case Encaramiento.InferiorIzquierda:

                    if ((int)d > 4 || d == Encaramiento.Arriba)
                        return "Izquierda";
                    else
                        return "Derecha";

                    break;
                case Encaramiento.SuperiorDerecha:
                    if (d == Encaramiento.SuperiorIzquierda || d == Encaramiento.Arriba)
                        return "Izquierda";
                    else
                        return "Derecha";

                    break;
                case Encaramiento.SuperiorIzquierda:
                    if (d == Encaramiento.SuperiorDerecha || d == Encaramiento.Arriba)
                        return "Izquierda";
                    else
                        return "Derecha";
                    break;
            
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
            for (int i = camino.Count - 2; i > 1; i--)
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
        /// <returns>indice sobre el camino, el cual es alcanzable por el mech ich</returns>
        private int caminoReal(ArrayList camino, Casilla destino, Mech ich, Tablero t)
        {
            int puntosM, puntosMR, suelo = 0;

            //devo comprobar si ay giroscopios
            if (ich.enSuelo()){
                suelo = 2;
                _original = _camino[0].direccion();
                _seLevanto = true;
            }
            if (_estrategia == Estrategia.Defensiva)
            {
                puntosMR = ich.puntosCorrer();
                puntosM = puntosMR / 2;
            }
            else {
                /*
                 * aqui hay que comprobar si la distancia de el enemigo y yo es muy grande entonces corro enlugar de caminar
                 */
                puntosMR=ich.puntosAndar()- suelo;
                puntosM = puntosMR / 2;
            }
            int j = 0, tmpC = 0, tmpJ = 0;
            Boolean flag = false, flagj = false;
            List<int> l;

            for (int i = camino.Count - 2; i != 0; i++)
            {

                if (((Nodo)camino[ i]).g() /*+ 4*/ < puntosMR)
                {/*
                    Nodo elemento = new Nodo();
                    for (int k = 1; k < 7; k++)
                    {
                        elemento = new Nodo();
                        try
                        {
                            elemento.casilla(t.colindante(((Nodo)camino[puntosM + i]).casilla().posicion(), (Encaramiento)k));//<-- hay que revisar en caso de que salga del tablero, aunque con el try funciona.
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                        /*
                         * hay que hacer una funcion que puntue las distintas casillas y de esta selecion la mejor para disparar y no ser disparado ademas de que no me aleje del destino final
                         */
                    //}

                    l = posiblesEncaramientos((Nodo)camino[i], destino, t);

                    for (int c = 1; c < l.Count; c++)
                    {
                        tmpC = costoEncaramiento(((Nodo)camino[i]).direccion(), (Encaramiento)l[c]);
                        tmpJ = costoEncaramiento(((Nodo)camino[i]).direccion(), (Encaramiento)l[j]);

<<<<<<< .mine
                        if (((Nodo)camino[ i]).g() + tmpC  < ich.puntosAndar())
=======
                        if (((Nodo)camino[puntosM + i]).g() + tmpC  < puntosMR)
>>>>>>> .r317
                        {
                            flag = true;

                            if (tmpJ > tmpC)
                            {
                                //flagj = true;
                                j = c;
                            }
                        }
                    }


                    if (l.Count == 1) {
                            j = 0;

                        tmpJ = costoEncaramiento(((Nodo)camino[i]).direccion(), (Encaramiento)l[j]);

                        if (((Nodo)camino[i]).g() + tmpC < ich.puntosAndar())
                            flag = true;                            

                    }

                    if (!flag)
                        flag = false;
                    else
                    {
                        _final = (Encaramiento)l[j];
                        ((Nodo)camino[i]).direccion((Encaramiento)l[j]);
                        return  i;
                    }

                    j = 0;
                }
            }



            return -1;
        }
  
        /// <summary>
        /// Calcula los posibles encaramientos que esten mirando hacia la casilla destino. Usada en caminoReal.
        /// </summary>
        /// <param name="o">el nodo en el que se localiza; tipo Nodo</param>
        /// <param name="destino">casilla hacia el que queremos mirar; tipo Casilla</param>
        /// <param name="t">tablero del juego; tipo Tablero</param>
        /// <returns>devuelve los encaramientos que miran hacia el destino; tipo Int(Encaramiento)</returns>
        private List<int> posiblesEncaramientos(Nodo o, Casilla destino, Tablero t)
        {
            int min = t.colindante(o.casilla().posicion(), (Encaramiento)6).posicion().distancia(destino.posicion()), tmp = 0;
            List<int> l = new List<int>();

            for (int i = 1; i < 7; i++)
            {
                try { 
                tmp = t.colindante(o.casilla().posicion(), (Encaramiento)i).posicion().distancia(destino.posicion());

                if (tmp <= min)
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
