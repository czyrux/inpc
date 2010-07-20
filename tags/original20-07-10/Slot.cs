
using System;
using System.IO;
using System.Collections;

namespace ico
{

	public class Slot {

		private string _clase;
		private int _cantidad;
		private int _codigo;
		private string _nombre;
		private int _indiceComponente;
		private int _indiceActuador;
		private int _danioCritico;
		
		//Constructores
		public Slot ( StreamReader f) {
			_clase=f.ReadLine();
			_cantidad=Convert.ToInt32(f.ReadLine());
			_codigo=Convert.ToInt32(f.ReadLine());
			_nombre=f.ReadLine();
			_indiceComponente=Convert.ToInt32(f.ReadLine());
			_indiceActuador=Convert.ToInt32(f.ReadLine());
			_danioCritico=Convert.ToInt32(f.ReadLine());
		}
		
		//Metodos get
		public string clase() { return _clase;}
		public int cantidad() { return _cantidad;}
		public int codigo() { return _codigo;}
		public string nombre() { return _nombre;}
		public int indiceComponente() { return _indiceComponente;}
		public int indiceActuador() { return _indiceActuador;}
		public int danioCritico() { return _danioCritico;}
	}
}
