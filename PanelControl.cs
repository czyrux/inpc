using System;
using System.Collections.Generic;
using System.Text;

namespace ico
{
    /// <summary>
    /// Clase encargada de almacenar las distintas constantes del programa
    /// </summary>
    static public class PanelControl
    {
        #region constantes del Camino

        public const int penalizadorAgua = 10;

        public const string movimientoArchivo = "faseMovimiento.txt";

        #endregion

        #region constantes de Mechs

        #region blindaje
        /// <summary>
        /// Tanto por ciento correspondiente al blindaje de TC
        /// </summary>
        public const float B_TC = 0.15f;
        /// <summary>
        /// Tanto por ciento correspondiente al blindaje de CAB
        /// </summary>
        public const float B_CAB = 0.15f;
        /// <summary>
        /// Tanto por ciento correspondiente al blindaje de BD
        /// </summary>
        public const float B_BD = 0.08f;
        /// <summary>
        /// Tanto por ciento correspondiente al blindaje de BI
        /// </summary>
        public const float B_BI = 0.08f;
        /// <summary>
        /// Tanto por ciento correspondiente al blindaje de ATD
        /// </summary>
        public const float B_ATD = 0.08f;
        /// <summary>
        /// Tanto por ciento correspondiente al blindaje de ATI
        /// </summary>
        public const float B_ATI = 0.08f;
        /// <summary>
        /// Tanto por ciento correspondiente al blindaje de ATC
        /// </summary>
        public const float B_ATC = 0.08f;
        /// <summary>
        /// Tanto por ciento correspondiente al blindaje de TI
        /// </summary>
        public const float B_TI = 0.1f;
        /// <summary>
        /// Tanto por ciento correspondiente al blindaje de TD
        /// </summary>
        public const float B_TD = 0.1f;
        /// <summary>
        /// Tanto por ciento correspondiente al blindaje de PI
        /// </summary>
        public const float B_PI = 0.05f;
        /// <summary>
        /// Tanto por ciento correspondiente al blindaje de PD
        /// </summary>
        public const float B_PD = 0.05f;
        #endregion

        #region estructura interna
        /// <summary>
        /// Tanto por ciento correspondiente a la estructura interna de TC
        /// </summary>
        public const float E_TC = 0.2f;
        /// <summary>
        /// Tanto por ciento correspondiente a la estructura interna de CAB
        /// </summary>
        public const float E_CAB = 0.2f;
        /// <summary>
        /// Tanto por ciento correspondiente a la estructura interna de BD
        /// </summary>
        public const float E_BD = 0.1f;
        /// <summary>
        /// Tanto por ciento correspondiente a la estructura interna de BI
        /// </summary>
        public const float E_BI = 0.1f;
        /// <summary>
        /// Tanto por ciento correspondiente a la estructura interna de TI
        /// </summary>
        public const float E_TI = 0.15f;
        /// <summary>
        /// Tanto por ciento correspondiente a la estructura interna de TD
        /// </summary>
        public const float E_TD = 0.15f;
        /// <summary>
        /// Tanto por ciento correspondiente a la estructura interna de PI
        /// </summary>
        public const float E_PI = 0.05f;
        /// <summary>
        /// Tanto por ciento correspondiente a la estructura interna de PD
        /// </summary>
        public const float E_PD = 0.05f;
        #endregion

        /// <summary>
        /// Tanto por ciento correspondiente al blindaje del mech para la nota final de estado
        /// </summary>
        public const float Nota_Blindaje = 0.4f;

        /// <summary>
        /// Tanto por ciento correspondiente a la estructura del mech para la nota final de estado
        /// </summary>
        public const float Nota_Estructura = 0.2f;

        /// <summary>
        /// Tanto por ciento correspondiente al tipo de mech el mech para la nota final de estado
        /// </summary>
        public const float Nota_Tipo = 0.4f;

        #endregion

        #region constantes fase de Armas

        /// <summary>
        /// Tanto por ciento de la nota del mech usado para el calculo del enemigo a disparar.
        /// </summary>
        public const float NOTA_ARMAS = 0.4f;

        /// <summary>
        /// Tanto por ciento del daño de las armas usado para el calculo del enemigo a disparar.
        /// </summary>
        public const float DANIO_ARMAS = 0.3f;

        /// <summary>
        /// Tanto por ciento de la distancia al objetivo usado para el calculo del enemigo a disparar.
        /// </summary>
        public const float DISTANCIA_ARMAS = 0.3f;


        /// <summary>
        /// Limite de calor permitido cuando la estrategia es agresiva
        /// </summary>
        public const int calorOfensivo = 16;

        /// <summary>
        /// Limite de calor permitido cuando la estrategia es defensiva
        /// </summary>
        public const int calorDefensivo = 9;
        #endregion

        #region constantes fase de Movimientos

        /// <summary>
        /// Tanto por ciento de la nota del objetivo usado para el calculo del enemigo al que encararse.
        /// </summary>
        public const float NOTA_MOV = 0.4f;

        /// <summary>
        /// Tanto por ciento de la distancia al objetivo usado para el calculo del enemigo al que encararse.
        /// </summary>
        public const float DISTANCIA_MOV = 0.6f;

        /// <summary>
        /// Radio usado para ver el numero de casillas alrededor de un mech mirar para la seleccion de
        /// casillas
        /// </summary>
        public const int radio = 5;

        /// <summary>
        /// Importancia que se le da a la distancia de la casilla a nuestro mech, para la eleccion de casilla
        /// </summary>
        public const float pesoDistancia = .20f;

        #endregion
    }
}
