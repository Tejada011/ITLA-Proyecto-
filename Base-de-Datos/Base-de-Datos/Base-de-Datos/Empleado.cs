using System;
using System.Collections.Generic;
using System.Text;

namespace Base_de_Datos
{
    public class Empleado
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Departamento { get; set; }

        public Empleado(string id, string nombre, string apellido, string departamento)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Departamento = departamento;
        }
    }
}
