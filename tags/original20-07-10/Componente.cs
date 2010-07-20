
using System;
using System.IO;

namespace ico
{
		
	public class Componente {
		
		private int _codigo;
		private string _nombre;
		private string _clase; //"NADA","ARMA","MUNICION","EQUIPO","ACTUADOR","ARMADURA","ARMAFISICA"
		private Boolean _parteTrasera;
		private int _localizacion; //(0=BI,1=TI,2=PI,3=PD,4=TD,5=BD,6=TC,7=CAB,8=TIa,9=TDa,10=TCa) 
		private int _localizacionSecundaria;
		private string _tipoArma;// (Nada, EnergÌa, BalÌstica, Misiles)
		private int _calor;
		private int _danio;
		private int _disparosTurno;
		private int _distanciaMinima;
		private int _distanciaCorta;
		private int _distanciaMedia;
		private int _distanciaLarga;
		private Boolean _operativo;
		private int _municionPara;
		private int _cantidadMunicion;
		private Boolean _municionEspecial;
		private int _modificadorDisparo;

		//constructor
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
		
		//METODOS GET
		public int codigo() { return _codigo; }
		public string nombre() { return _nombre; }
		public string clase() { return _clase; }
		public Boolean parteTrasera() { return _parteTrasera; }
		public int localizacion() { return _localizacion; }
		public int localizacionSecundaria() { return _localizacionSecundaria; }
		public string tipoArma() { return _tipoArma; }
		public int calor() { return _calor; }
		public int danio() { return _danio; }
		public int disparosTurno() { return _disparosTurno; }
		public int distanciaMinima() { return _distanciaMinima; }
		public int distanciaCorta() { return _distanciaCorta; }
		public int distanciaMedia() { return _distanciaMedia; }
		public int distanciaLarga() { return _distanciaLarga; }
		public Boolean operativo() { return _operativo; }
		public int municionPara() { return _municionPara; }
		public int cantidadMunicion() { return _cantidadMunicion; }
		public Boolean municionEspecial() { return _municionEspecial; }
		public int modificadorDisparo() { return _modificadorDisparo; }

	}
}
