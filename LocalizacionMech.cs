
using System;
using System.IO;
using System.Collections;

namespace ico {

	public class LocalizacionMech {
		private int _slotsOcupados;
		private Slot[] _slots;
		
		//Constructor
		public LocalizacionMech ( StreamReader f) {
			
			_slotsOcupados=Convert.ToInt32(f.ReadLine());
			
			_slots = new Slot[_slotsOcupados];
			Slot aux;
			
			for ( int i=0 ; i<_slotsOcupados ; i++){
				aux = new Slot (f);
				_slots[i]=aux;
			}
		}
		
		//Metodos get
		public int slotsOcupados() { return _slotsOcupados; }
		public Slot[] slots() { return _slots; }
	}
		
}
