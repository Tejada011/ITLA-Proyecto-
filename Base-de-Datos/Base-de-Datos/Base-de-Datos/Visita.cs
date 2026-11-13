using System;

namespace Base_de_Datos
{
    public class Visita
    {
        public int IdVisita { get; set; }
        public string IdVisitante { get; set; }
        public string IdEmpleado { get; set; }
        public string HoraEntrada { get; set; }
        public string HoraSalida { get; set; }
        public bool Activa { get; set; }

        public Visita(int idVisita, string idVisitante, string idEmpleado)
        {
            IdVisita = idVisita;
            IdVisitante = idVisitante;
            IdEmpleado = idEmpleado;
            HoraEntrada = DateTime.Now.ToString("hh:mm tt");
            HoraSalida = "--:--";
            Activa = true;
        }
    }
}