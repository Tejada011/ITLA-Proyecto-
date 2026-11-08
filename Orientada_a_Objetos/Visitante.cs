using System;
using System.Collections.Generic;
using System.Text;

namespace Orientada_a_Objetos
{
    public class Visitante
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Empresa { get; set; }

        public Visitante(string id, string nombre, string apellido, string empresa)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Empresa = empresa;
        }
    }
}
