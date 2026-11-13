using System;
using System.Collections.Generic;

namespace Orientada_a_Objetos
{
    class Program
    {
        static List<Visita> visitas = new List<Visita>();
        static Dictionary<string, Empleado> empleados = new Dictionary<string, Empleado>();
        static Dictionary<string, Visitante> visitantes = new Dictionary<string, Visitante>();

        static int siguienteIdVisita = 1;

       

        static void Main(string[] args)
        {
            DisplayHeader();

            bool wantToContinue = true;

            while (wantToContinue == true)
            {
                try
                {
                    DisplayMenu();
                    int chosenOption = Convert.ToInt32(Console.ReadLine());

                    switch (chosenOption)
                    {
                        case 1:
                            RegistrarEntrada();
                            break;
                        case 2:
                            BuscarVisitas();
                            break;
                        case 3:
                            ModificarVisita();
                            break;
                        case 4:
                            RegistrarSalida();
                            break;
                        case 5:
                            ListarVisitas();
                            break;
                        case 6:
                            EliminarVisita();
                            break;
                        case 7:
                            wantToContinue = false;
                            Console.WriteLine("Cerrando el sistema...");
                            break;
                        default:
                            Console.WriteLine("Opcion no valida. Intente de nuevo.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nError: Debe ingresar un numero entero valido para el menu.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError inesperado: {ex.Message}");
                }

                if (wantToContinue == true)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        static void DisplayHeader()
        {
            Console.WriteLine("=== CONTROL DE ACCESO ===");
            Console.WriteLine("Sistema de gestion de visitas");
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\n=== MENÚ PRINCIPAL ===");
            Console.WriteLine("1. Registrar Entrada");
            Console.WriteLine("2. Buscar Visitas");
            Console.WriteLine("3. Modificar Informacion");
            Console.WriteLine("4. Registrar Salida");
            Console.WriteLine("5. Listar Todas las Visitas");
            Console.WriteLine("6. Eliminar Visita");
            Console.WriteLine("7. Salir");
            Console.Write("Seleccione una opción: ");
        }

        static Visita BuscarVisitaPorTicket(int ticket)
        {
            for (int i = 0; i < visitas.Count; i++)
            {
                if (visitas[i].IdVisita == ticket)
                {
                    return visitas[i];
                }
            }
            return null;
        }

        static string ObtenerEstado(Visita visita)
        {
            if (visita.Activa == true)
            {
                return "En el edificio";
            }
            return "Fuera";
        }

        static void RegistrarEntrada()
        {
            Console.Clear();
            Console.WriteLine("--- AGREGAR NUEVA ENTRADA ---");

            try
            {
                Console.Write("Cédula/ID del visitante: ");
                string idVis = Console.ReadLine().Trim();

                if (idVis == "")
                {
                    Console.WriteLine("El ID del visitante no puede estar vacio.");
                    return;
                }

                if (visitantes.ContainsKey(idVis) == false)
                {
                    Console.WriteLine("Visitante nuevo detectado. Ingrese sus datos:");

                    Console.Write("Nombre: ");
                    string nombreVis = Console.ReadLine().Trim();
                    Console.Write("Apellido: ");
                    string apellidoVis = Console.ReadLine().Trim();
                    Console.Write("Empresa: ");
                    string empresaVis = Console.ReadLine().Trim();

                    if (nombreVis == "" || apellidoVis == "" || empresaVis == "")
                    {
                        Console.WriteLine("Los datos del visitante no pueden estar vacios.");
                        return;
                    }

                    visitantes.Add(idVis, new Visitante(idVis, nombreVis, apellidoVis, empresaVis));
                }

                Console.Write("ID Numérico del Empleado que lo recibe: ");
                int idEmpNumero = Convert.ToInt32(Console.ReadLine());

                if (idEmpNumero <= 0)
                {
                    Console.WriteLine("El ID del empleado debe ser mayor que cero.");
                    return;
                }

                string idEmp = idEmpNumero.ToString();

                if (empleados.ContainsKey(idEmp) == false)
                {
                    Console.WriteLine("Empleado nuevo detectado. Ingrese sus datos:");

                    Console.Write("Nombre: ");
                    string nombreEmp = Console.ReadLine().Trim();
                    Console.Write("Apellido: ");
                    string apellidoEmp = Console.ReadLine().Trim();
                    Console.Write("Departamento: ");
                    string departamentoEmp = Console.ReadLine().Trim();

                    if (nombreEmp == "" || apellidoEmp == "" || departamentoEmp == "")
                    {
                        Console.WriteLine("Los datos del empleado no pueden estar vacios.");
                        return;
                    }

                    empleados.Add(idEmp, new Empleado(idEmp, nombreEmp, apellidoEmp, departamentoEmp));
                }

                int nuevoIdVisita = siguienteIdVisita;

                visitas.Add(new Visita(nuevoIdVisita, visitantes[idVis], empleados[idEmp]));
                siguienteIdVisita++;

                Console.WriteLine($"\n¡Visita registrada! Ticket de visita asignado: {nuevoIdVisita}");
            }
            catch (Exception)
            {
                Console.WriteLine("Error: Entrada de datos invalida.");
            }
        }

        static void BuscarVisitas()
        {
            Console.Clear();
            Console.WriteLine("--- BUSCAR VISITAS (CRITERIOS) ---");
            Console.WriteLine("1. Buscar por Ticket de Visita");
            Console.WriteLine("2. Buscar por Nombre del Visitante");
            Console.WriteLine("3. Ver visitas que siguen en el edificio");
            Console.Write("Seleccione (1-3): ");
            string criterio = Console.ReadLine();

            if (criterio == "1")
            {
                try
                {
                    Console.Write("Ingrese el numero de ticket a buscar: ");
                    int idBuscado = Convert.ToInt32(Console.ReadLine());

                    Visita visita = BuscarVisitaPorTicket(idBuscado);

                    if (visita != null)
                    {
                        Console.WriteLine("\n[Resultado Encontrado]");
                        Console.WriteLine($"Ticket ID: {visita.IdVisita}");
                        Console.WriteLine($"Visitante: {visita.VisitanteAsignado.Nombre} {visita.VisitanteAsignado.Apellido} ({visita.VisitanteAsignado.Empresa})");
                        Console.WriteLine($"Atendido por: [{visita.EmpleadoAsignado.Id}] {visita.EmpleadoAsignado.Nombre} {visita.EmpleadoAsignado.Apellido} - Depto: {visita.EmpleadoAsignado.Departamento}");
                        Console.WriteLine($"Entrada: {visita.HoraEntradaTxt} | Salida: {visita.HoraSalidaTxt}");
                        Console.WriteLine($"Estado: {ObtenerEstado(visita)}");
                    }
                    else
                    {
                        Console.WriteLine("No existe ninguna visita con ese codigo.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error: El ticket debe ser numerico.");
                }
            }
            else if (criterio == "2")
            {
                Console.Write("Ingrese el nombre del visitante a buscar: ");
                string nombreBuscado = Console.ReadLine().ToLower();
                bool encontrado = false;

                for (int i = 0; i < visitas.Count; i++)
                {
                    Visita visita = visitas[i];

                    if (visita.VisitanteAsignado.Nombre.ToLower().Contains(nombreBuscado) == true || visita.VisitanteAsignado.Apellido.ToLower().Contains(nombreBuscado) == true)
                    {
                        encontrado = true;
                        Console.WriteLine($"Ticket: {visita.IdVisita} | Visitante: {visita.VisitanteAsignado.Nombre} {visita.VisitanteAsignado.Apellido} | Empleado: {visita.EmpleadoAsignado.Nombre} - Estado: {ObtenerEstado(visita)}");
                    }
                }

                if (encontrado == false)
                {
                    Console.WriteLine("No se encontraron registros.");
                }
            }
            else if (criterio == "3")
            {
                bool encontrado = false;

                for (int i = 0; i < visitas.Count; i++)
                {
                    Visita visita = visitas[i];

                    if (visita.Activa == true)
                    {
                        encontrado = true;
                        Console.WriteLine($"Ticket: {visita.IdVisita} | Visitante: {visita.VisitanteAsignado.Nombre} {visita.VisitanteAsignado.Apellido} | Empleado: {visita.EmpleadoAsignado.Nombre} | Entrada: {visita.HoraEntradaTxt}");
                    }
                }

                if (encontrado == false)
                {
                    Console.WriteLine("No hay visitas dentro del edificio.");
                }
            }
            else
            {
                Console.WriteLine("Criterio no valido.");
            }
        }

        static void ModificarVisita()
        {
            Console.Clear();
            Console.WriteLine("--- MODIFICAR DATOS ---");

            try
            {
                Console.Write("Ingrese el Ticket de la visita a modificar: ");
                int idBuscado = Convert.ToInt32(Console.ReadLine());

                Visita visita = BuscarVisitaPorTicket(idBuscado);

                if (visita != null)
                {
                    Console.Write($"Nuevo Nombre Visitante (actual: {visita.VisitanteAsignado.Nombre}): ");
                    string nuevoNombre = Console.ReadLine();
                    if (nuevoNombre.Trim() != "")
                    {
                        visita.VisitanteAsignado.Nombre = nuevoNombre.Trim();
                    }

                    Console.Write($"Nuevo Nombre del Empleado (actual: {visita.EmpleadoAsignado.Nombre}): ");
                    string nuevoNombreEmp = Console.ReadLine();
                    if (nuevoNombreEmp.Trim() != "")
                    {
                        visita.EmpleadoAsignado.Nombre = nuevoNombreEmp.Trim();
                    }

                    Console.WriteLine("¡Informacion actualizada exitosamente!");
                }
                else
                {
                    Console.WriteLine("El ticket ingresado no existe.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error: Ingrese un numero de ticket valido.");
            }
        }

        static void RegistrarSalida()
        {
            Console.Clear();
            Console.WriteLine("--- REGISTRAR SALIDA ---");

            try
            {
                Console.Write("Ingrese el Ticket de la Visita para marcar salida: ");
                int idBuscado = Convert.ToInt32(Console.ReadLine());

                Visita visita = BuscarVisitaPorTicket(idBuscado);

                if (visita != null)
                {
                    if (visita.Activa == true)
                    {
                        visita.HoraSalidaTxt = DateTime.Now.ToString("hh:mm tt");
                        visita.Activa = false;
                        Console.WriteLine($"Salida marcada de forma correcta a las {visita.HoraSalidaTxt}");
                    }
                    else
                    {
                        Console.WriteLine($"Este ticket ya registro su salida a las: {visita.HoraSalidaTxt}");
                    }
                }
                else
                {
                    Console.WriteLine("Ticket no encontrado.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error: El ticket debe ser numerico.");
            }
        }

        static void ListarVisitas()
        {
            Console.Clear();
            Console.WriteLine("--- LISTA DE VISITAS ---");

            if (visitas.Count == 0)
            {
                Console.WriteLine("No hay visitas registradas.");
                return;
            }

            for (int i = 0; i < visitas.Count; i++)
            {
                Visita visita = visitas[i];

                Console.WriteLine($"Ticket: {visita.IdVisita} | Visitante: {visita.VisitanteAsignado.Nombre} {visita.VisitanteAsignado.Apellido} | Empleado: {visita.EmpleadoAsignado.Nombre} | Entrada: {visita.HoraEntradaTxt} | Salida: {visita.HoraSalidaTxt} | Estado: {ObtenerEstado(visita)}");
            }
        }

        static void EliminarVisita()
        {
            Console.Clear();
            Console.WriteLine("--- ELIMINAR VISITA ---");

            try
            {
                Console.Write("Ingrese el Ticket de la visita a eliminar: ");
                int idBuscado = Convert.ToInt32(Console.ReadLine());

                Visita visita = BuscarVisitaPorTicket(idBuscado);

                if (visita != null)
                {
                    visitas.Remove(visita);
                    Console.WriteLine("Visita eliminada correctamente.");
                }
                else
                {
                    Console.WriteLine("Ticket no encontrado.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error: El ticket debe ser numerico.");
            }
        }
    }
}