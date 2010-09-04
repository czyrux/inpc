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
        

        public Camino(ArrayList camino)
        {
            _camino= new List<Nodo>();
             foreach (Nodo i in camino) {
                _camino.Add((Nodo)i);
            }
            _ldv =_cobertura= false;
            _nldv = _movimientos = 0;
        }

        public Camino(Casilla de,Mech ich, Casilla a, Tablero tablero, Estrategia estrategia) {
           _estrategia=estrategia;
           ArrayList camino = pathFinder(de, ich, a, tablero);
           int j = 0;

           _camino = new List<Nodo>();
          foreach (Nodo i in camino) {
              _camino.Add(i);
              j++;
          }

          //Rellenamos el resto de atributos privados
          _ldv = _cobertura = false;
          _nldv = _movimientos = 0;
        }

#endregion

#region Propiedades

        public int longitud() {
            return _camino.Count;
        }
        public List<Nodo> getCamino() {
            return _camino;
        }
        public Boolean ldv() {
            return _ldv;
        }
        public Boolean cobertura() {
            return _cobertura;
        }
#endregion

#region Funciones
     
        public  ArrayList  pathFinder(Casilla a, Mech ich, Casilla b, Tablero Tablero)
        {
            ArrayList cerradas = new ArrayList();
            ArrayList abiertas = new ArrayList();
            ArrayList camino = new ArrayList();
            Nodo elemento = new Nodo();
            int aux = 0, iaux=0, mejor=-1, gAcumulada=0;
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
            do {
                for (int i = 1; i < 7; i++) {
                    elemento = new Nodo();
                    try
                    {
                        elemento.casilla(Tablero.colindante(actual.casilla().posicion(), (Encaramiento)i));//<-- hay que revisar en caso de que salga del tablero, aunque con el try funciona.
                    }
                    catch (Exception e) {
                        continue;
                    }
                    // Ignoro las casillas que ya estan en la lista de cerradas.
                    if (esta(cerradas,elemento.casilla()))
                        continue;

                    // Precalculo el costo de movimiento relacional, para no hacerlo varias veces
                    aux = actual.casilla().costoMovimiento(elemento.casilla()) + costoEncaramiento(actual.casilla(), (Encaramiento)i, elemento.casilla(), Tablero);
                    //aux = actual.posicion().distancia(elemento.casilla.posicion());
                    if (aux > 100)
                        continue;
                    // verifico si es intrancitable
                    if (/*elemento.casilla.costoMovimiento() >= 0 ||*/  aux >= 0){

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
                        else {//si no estaba entre las abiertas inserto la nueva casilla
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
           do{
               camino.Add(padre.padre());
               padre = padre.padre();
           } while (padre.casilla() != a);



           aux = caminoReal(limpiarAgua(camino), b, ich, Tablero);
           camino = camino.GetRange(aux, camino.Count -aux);

            camino.Reverse();


            return camino;
        }

        public int costoMovimiento() 
        {    
                for (int i = 0; i < _camino.Count; i++){

                    _movimientos = _camino[i].casilla().costoMovimiento();

                    if (i < _camino.Count && _camino[i].casilla().nivel() < _camino[i + 1].casilla().nivel())
                    {

                        _movimientos += _camino[i].casilla().costoMovimiento(_camino[i + 1].casilla());

                        if (_movimientos < 0)//si hay intransitables en el camino
                            return _movimientos;
                    }
                }

                return _movimientos;
        }

        public Casilla casilla(int i) {
            return _camino[i].casilla();
        }

        public Nodo nodo(int i)
        {
            return _camino[i];
        }

        public int movimentos() { return _movimientos; }

        public void print()
        { 
            string str="El camino es: ";
            foreach (Nodo i in _camino)
                str += "("+i.casilla().posicion().ToString()+", "+ i.direccion().ToString() +")"+ "->";
            str += "FIN";
            Console.WriteLine(str);
        }


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
            }

            if (_camino[i].direccion() != _original) {
                str2 += izqOdrch(_camino[i].direccion(), _original) + "\n";
                str2 += costoEncaramiento(_original, _camino[i].direccion()).ToString()+"\n";
                pasos++;
            }

            i++;
            do{

                if (_camino[i-1].direccion() != _camino[i].direccion())
                {
                    int tmp = costoEncaramiento(_camino[i - 1].direccion(), _camino[i].direccion());
                    str2 += izqOdrch(_camino[i-1].direccion(), _camino[i].direccion()) + "\n";
                    str2 += tmp.ToString() + "\n";
                    pasos+=tmp;
                }
                str2 += "Adelante\n";
                str2 += "1\n";
                pasos++;
                i++;
                
            }while(i!=_camino.Count);

            if (_camino[i-1].direccion() != _final)
            {
                str2 += izqOdrch(_camino[i].direccion(),_final) + "\n";
                str2 += costoEncaramiento(_camino[i].direccion(), _final).ToString() + "\n";
                pasos++;
            }
            
            str += pasos.ToString()+"\n";

            str += str2;

            return str;
        }

        public void ToFile(){
            StreamWriter fich = new StreamWriter(PanelControl.movimientoArchivo);
            fich.Write(this.ToString());
        }
#endregion
        #region Privado
        //private int _length;
        private List<Nodo> _camino;
        private Boolean _ldv;
        private Boolean _cobertura;
        private int _nldv;
        private int _movimientos;
        private Encaramiento _original;
        private Encaramiento _final;
        private tipoMovimiento _tipoMovimiento;
        private Estrategia _estrategia;
        private Boolean _seLevanto;

        private string izqOdrch(Encaramiento o, Encaramiento d) {
            switch (o) { 
                case Encaramiento.Abajo:
                        if(d==o){
                            if((int)d>4)
                                return "Derecha";
                            else
                                return "Izquierda";
                        }
                   
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

        private ArrayList limpiarAgua(ArrayList camino)
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

        private int caminoReal(ArrayList camino, Casilla destino, Mech ich, Tablero t)
        {
            int puntosM, puntosMR, suelo = 0;
            if (ich.enSuelo()){
                suelo = 2;
                _original = _camino[0].direccion();
                _seLevanto = true;
            }
            if (_estrategia == Estrategia.Defensiva)
            {
                puntosM = ich.puntosCorrer() - suelo / 2;
                puntosMR = ich.puntosCorrer();
            }
            else {
                puntosM = ich.puntosAndar() - suelo / 2;
                puntosMR = ich.puntosAndar();
            }
            int j = 0, tmpC = 0, tmpJ = 0;
            Boolean flag = false, flagj = false;
            List<int> l;

            for (int i = 0; i < camino.Count - (1 + puntosM); i++)
            {
                if (puntosM + i >= camino.Count)
                    puntosM = camino.Count - 1;

                if (((Nodo)camino[puntosM + i]).g() < puntosMR)
                {
                    l = posiblesEncaramientos((Nodo)camino[puntosM + i], destino, t);

                    for (int c = 1; c < l.Count; c++)
                    {
                        tmpC = costoEncaramiento(((Nodo)camino[puntosM + i]).direccion(), (Encaramiento)l[c]);
                        tmpJ = costoEncaramiento(((Nodo)camino[puntosM + i]).direccion(), (Encaramiento)l[j]);

                        if (((Nodo)camino[puntosM + i]).g() + tmpC < ich.puntosAndar())
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

                        tmpJ = costoEncaramiento(((Nodo)camino[puntosM + i]).direccion(), (Encaramiento)l[j]);

                        if (((Nodo)camino[puntosM + i]).g() + tmpC < ich.puntosAndar())
                            flag = true;                            

                    }

                    if (!flag)
                        flag = false;
                    else
                    {
                        _final = (Encaramiento)l[j];
                        ((Nodo)camino[puntosM + i]).direccion((Encaramiento)l[j]);
                        return puntosM + i;
                    }

                    j = 0;
                }
            }

            return -1;
        }
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
 

        // funcion que calcula los valores euristicos siendo menos el mejor.
        private int heuristica(Casilla a, Casilla b)
        {
            int h = 0;

            //Distancia aproximada
            //h = Posicion.distancia(a.posicion(), b.posicion());// Calculo de la distancia aproximada al objetivo.
            //Distancia real
            h = a.posicion().distancia(b.posicion());

            return h;
        }
        
        private int mejorCasillaAbierta(ArrayList abiertas,Casilla padre, Casilla destino) {
            int max = 0;
            for (int i = 1; i < abiertas.Count; i++ )
            {
               /* if (max < 0){
                    if (((heuristica)abiertas[i]).padre == padre)
                        max = i;
                }
                else
                {*/
                if (/*((heuristica)abiertas[i]).g<20 && */((Nodo)abiertas[i]).casilla() == destino && ((Nodo)abiertas[i]).padre().casilla() == padre)
                        return i;
                if (((Nodo)abiertas[max]).f() > ((Nodo)abiertas[i]).f() /*&& ((heuristica)abiertas[i]).padre == padre*/)
                        max = i;
                //}
            }
            return max;
        }

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

        private Boolean esta(ArrayList lista, Casilla elem) {
            foreach (Nodo i in lista)
            {
                if (i.casilla() == elem)
                    return true;
            }
            return false;
        }
        #endregion

    }
}
