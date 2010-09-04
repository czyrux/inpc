
using System;
using System.IO;

namespace ico
{
    /// <summary>
    /// Clase que contiene la informacion de un componente del mech
    /// </summary>
	public class Componente
    {
        #region atributos
        /// <summary>
        /// Codigo del componente
        /// </summary>
        private int _codigo;
        /// <summary>
        /// Nombre del compononente
        /// </summary>
		private string _nombre;
        /// <summary>
        /// Clase de componente: "NADA","ARMA","MUNICION","EQUIPO","ACTUADOR","ARMADURA","ARMAFISICA"
        /// </summary>
		private string _clase;
        /// <summary>
        /// Booleano que indica si esta en la parte trasera
        /// </summary>
		private Boolean _parteTrasera;
        /// <summary>
        /// Localizacion del componente (0=BI,1=TI,2=PI,3=PD,4=TD,5=BD,6=TC,7=CAB,8=TIa,9=TDa,10=TCa) 
        /// </summary>
		private int _localizacion;
        /// <summary>
        /// Localizacion secundaria, en caso de tenerla
        /// </summary>
		private int _localizacionSecundaria;
        /// <summary>
        /// Tipo de arma: (Nada, EnergÌa, BalÌstica, Misiles)
        /// </summary>
		private string _tipoArma;
        /// <summary>
        /// Calor que genera
        /// </summary>
		private int _calor;
        /// <summary>
        /// Daño que realiza
        /// </summary>
		private int _danio;
        /// <summary>
        /// Disparos por turno
        /// </summary>
		private int _disparosTurno;
        /// <summary>
        /// Distancia minima de tiro
        /// </summary>
		private int _distanciaMinima;
        /// <summary>
        /// Distancia corta de tiro
        /// </summary>
		private int _distanciaCorta;
        /// <summary>
        /// Distancia media de tiro
        /// </summary>
		private int _distanciaMedia;
        /// <summary>
        /// Distancia larga de tiro
        /// </summary>
		private int _distanciaLarga;
        /// <summary>
        /// Indica si esta operativo
        /// </summary>
		private Boolean _operativo;
        /// <summary>
        /// Codigo del arma para el que sirve la municion (en caso de ser municion)
        /// </summary>
		private int _municionPara;
        /// <summary>
        /// Cantidad de municion (en caso de ser municion)
        /// </summary>
		private int _cantidadMunicion;
        /// <summary>
        /// Si es municion especial (en caso de ser municion)
        /// </summary>
		private Boolean _municionEspecial;
        /// <summary>
        /// Modificar al disparo
        /// </summary>
		private int _modificadorDisparo;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor con parametros
        /// </summary>
        /// <param name="f">Descriptor del fichero ya abierto</param>
		public Componente ( StreamReader f) {
			_codigo=Convert.ToInt32(f.ReadLine());
			_nombre=f.ReadLine();
			_clase=f.ReadLine(); //"NADA","ARMA","MUNICION","EQUIPO","ACTUADOR","ARMADURA","ARMAFISICA"
			_parteTrasera=Convert.ToBoolean(f.ReadLine());
			_localizacion=Convert.ToInt32(f.ReadLine()); //(0=BI,1=TI,2=PI,3=PD,4=TD,5=BD,6=TC,7=CAB,8=TIa,9=TDa,10=TCa) 
			_localizacionSecundaria=Convert.ToInt32(f.ReadLine());
			_tipoArma=f.ReadLine();// (Nada, EnergÌa, BalÌstica, Misiles)
			_calor=Convert.ToInt32(f.ReadLine());
			_danio=Convert.ToInt32(f.ReadLine());
			_disparosTurno=Convert.ToInt32(f.ReadLine());
			_distanciaMinima=Convert.ToInt32(f.ReadLine());
			_distanciaCorta=Convert.ToInt32(f.ReadLine());
			_distanciaMedia=Convert.ToInt32(f.ReadLine());
			_distanciaLarga=Convert.ToInt32(f.ReadLine());
			_operativo=Convert.ToBoolean(f.ReadLine());
			_municionPara=Convert.ToInt32(f.ReadLine());
			_cantidadMunicion=Convert.ToInt32(f.ReadLine());
			string especial=f.ReadLine();
			if ( especial=="SI")
				_municionEspecial=true;
			else
				_municionEspecial=false;

			_modificadorDisparo=Convert.ToInt32(f.ReadLine());
		}
        #endregion

        #region metodosGet
        /// <summary>
        /// Indica el codigo del componente
        /// </summary>
        /// <returns>Entero</returns>
		public int codigo() { return _codigo; }
        /// <summary>
        /// Indica el nombre del componente
        /// </summary>
        /// <returns>String</returns>
		public string nombre() { return _nombre; }
        /// <summary>
        /// Indica la clase del componente
        /// </summary>
        /// <returns>String</returns>
		public string clase() { return _clase; }
        /// <summary>
        /// Indica si esta situado en parte trasera
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean parteTrasera() { return _parteTrasera; }
        /// <summary>
        /// Indica el entero de la localizacion
        /// </summary>
        /// <returns>Entero</returns>
		public int localizacionINT() { return _localizacion; }
        /// <summary>
        /// Indica la localizacion secundaria
        /// </summary>
        /// <returns>Entero</returns>
		public int localizacionSecundaria() { return _localizacionSecundaria; }
        /// <summary>
        /// Indica el tipo de arma
        /// </summary>
        /// <returns>String</returns>
		public string tipoArma() { return _tipoArma; }
        /// <summary>
        /// Indica el calor que genera
        /// </summary>
        /// <returns>Entero</returns>
		public int calor() { return _calor; }
        /// <summary>
        /// Indica el daño que realiza
        /// </summary>
        /// <returns>Entero</returns>
		public int danio() { return _danio*_disparosTurno; }
        /// <summary>
        /// Indica los disparos por turno
        /// </summary>
        /// <returns>Entero</returns>
		public int disparosTurno() { return _disparosTurno; }
        /// <summary>
        /// Indica la distancia minima
        /// </summary>
        /// <returns>Entero</returns>
		public int distanciaMinima() { return _distanciaMinima; }
        /// <summary>
        /// Indica la distancia de tiro corta
        /// </summary>
        /// <returns>Entero</returns>
		public int distanciaCorta() { return _distanciaCorta; }
        /// <summary>
        /// Indica la distancia de tiro media
        /// </summary>
        /// <returns>Entero</returns>
		public int distanciaMedia() { return _distanciaMedia; }
        /// <summary>
        /// Indica la distancia de tiro larga
        /// </summary>
        /// <returns>Entero</returns>
		public int distanciaLarga() { return _distanciaLarga; }
        /// <summary>
        /// Indica si esta operativo
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
		public Boolean operativo() { return _operativo; }
        /// <summary>
        /// Indica el codigo del arma para el que vale la municion
        /// </summary>
        /// <returns>Entero</returns>
		public int municionPara() { return _municionPara; }
        /// <summary>
        /// Indica la cantidad de municion disponible
        /// </summary>
        /// <returns>Entero</returns>
		public int cantidadMunicion() { return _cantidadMunicion; }
        /// <summary>
        /// Indica si es municion especial
        /// </summary>
        /// <returns></returns>
		public Boolean municionEspecial() { return _municionEspecial; }
        /// <summary>
        /// Indica el modificador de disparo
        /// </summary>
        /// <returns></returns>
		public int modificadorDisparo() { return _modificadorDisparo; }

        /// <summary>
        /// Indica la localizacion del string
        /// </summary>
        /// <returns>String</returns>
        public String localizacionSTRING() {
            String ubicacion;
            //(0=BI,1=TI,2=PI,3=PD,4=TD,5=BD,6=TC,7=CAB,8=TIa,9=TDa,10=TCa) 
            switch (_localizacion) { 
                case 0:
                    ubicacion="BI";
                    break;
                case 1:
                    ubicacion="TI";
                    break;
                case 2:
                    ubicacion="PI";
                    break;
                case 3:
                    ubicacion="PD";
                    break;
                case 4:
                    ubicacion="TD";
                    break;
                case 5:
                    ubicacion="BD";
                    break;
                case 6:
                    ubicacion="TC";
                    break;
                case 7:
                    ubicacion="CAB";
                    break;
                case 8:
                    ubicacion="TIa";
                    break;
                case 9:
                    ubicacion="TDa";
                    break;
                case 10:
                    ubicacion="TCa";
                    break;
                default:
                    ubicacion=null;
                    break;
            }

            return ubicacion;
        }

        /// <summary>
        /// Indica si el tipo de arma es de energia
        /// </summary>
        /// <returns>True en caso afirmativo</returns>
        public Boolean energia() {

            if (_tipoArma.Contains("Energ") && _clase == "ARMA")
            {
                return true;
            }
            else
                return false;
        }

        #endregion
    }
}
