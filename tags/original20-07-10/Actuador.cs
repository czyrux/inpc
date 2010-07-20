
using System;
using System.IO;

namespace ico
{

	public class Actuador {

		private int _codigo;
		private string _nombre;
		private int _localizacion;//(0=BI,1=TI,2=PI,3=PD,4=TD,5=BD,6=TC,7=CAB,8=TIa, 9=TDa,10=TCa) 
		private Boolean _operativo;
		private int _numeroImpactos;
		
		//Constructor
		public Actuador ( StreamReader f) {
			_codigo=Convert.ToInt32(f.ReadLine());
			_nombre=f.ReadLine();
			_localizacion=Convert.ToInt32(f.ReadLine());
			_operativo=Convert.ToBoolean(f.ReadLine());
			_numeroImpactos=Convert.ToInt32(f.ReadLine());
		}
		
		//Metodos GEt
		public int codigo() { return _codigo;}
		public string nombre() { return _nombre;}
		public int localizacion() { return _localizacion;}
		public Boolean operativo() { return _operativo;}
		public int numeroImpactos() { return _numeroImpactos;}

	}
}
