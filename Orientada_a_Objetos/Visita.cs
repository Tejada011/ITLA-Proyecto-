using System;
using System.Collections.Generic;
using System.Text;

namespace Orientada_a_Objetos
{
    public class Visita
    {
        public int IdVisita { get; set; }
        public Visitante VisitanteAsignado { get; set; }
        public Empleado EmpleadoAsignado { get; set; }
        public string HoraEntradaTxt { get; set; }
        public string HoraSalidaTxt { get; set; }
        public bool Activa { get; set; }

        public Visita(int idVisita, Visitante visitante, Empleado empleado)
        {
            IdVisita = idVisita;
            VisitanteAsignado = visitante;
            EmpleadoAsignado = empleado;
            HoraEntradaTxt = DateTime.Now.ToString("hh:mm tt");
            HoraSalidaTxt = "--:--";
            Activa = true;
        }
    }
}
