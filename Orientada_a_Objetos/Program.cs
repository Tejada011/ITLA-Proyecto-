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
            bool wantToContinue = true;

            while (wantToContinue)
            {
                try
                {
                    DisplayMenu();
                    int chosenOption = Convert.ToInt32(Console.ReadLine());

                    switch (chosenOption)
                    {
                        case 1: RegistrarEntrada(); break;
                        case 2: BuscarVisitas(); break;
                        case 3: ModificarVisita(); break;
                        case 4: RegistrarSalida(); break;
                        case 5: ListarVisitas(); break;
                        case 6: EliminarVisita(); break;
                        case 7:
                            wantToContinue = false;
                            Console.WriteLine("Cerrando el sistema...");
                            break;
                        default:
                            Console.WriteLine("Opción no válida. Intente de nuevo.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nError: Debe ingresar un número entero válido para el menú.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError inesperado: {ex.Message}");
                }

                if (wantToContinue)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("=== CONTROL DE ACCESO ===");
            Console.WriteLine("Sistema de gestión de visitas (POO)");
            Console.WriteLine("\n=== MENÚ PRINCIPAL ===");
            Console.WriteLine("1. Registrar Entrada");
            Console.WriteLine("2. Buscar Visitas");
            Console.WriteLine("3. Modificar Información");
            Console.WriteLine("4. Registrar Salida");
            Console.WriteLine("5. Listar Todas las Visitas");
            Console.WriteLine("6. Eliminar Visita");
            Console.WriteLine("7. Salir");
            Console.Write("Seleccione una opción: ");
        }

        static void RegistrarEntrada()
        {
            Console.Clear();
            Console.WriteLine("--- AGREGAR NUEVA ENTRADA ---");

            try
            {
                Console.Write("Cédula/ID del Visitante: ");
                string idVis = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(idVis))
                {
                    Console.WriteLine("El ID del visitante no puede estar vacío.");
                    return;
                }

                if (!visitantes.ContainsKey(idVis))
                {
                    Console.WriteLine("Visitante nuevo detectado. Ingrese sus datos:");
                    Console.Write("Nombre: "); string nomVis = Console.ReadLine();
                    Console.Write("Apellido: "); string apeVis = Console.ReadLine();
                    Console.Write("Empresa: "); string empVis = Console.ReadLine();

                    visitantes.Add(idVis, new Visitante(idVis, nomVis, apeVis, empVis));
                }

                Console.Write("ID/Código del Empleado que lo recibe: ");
                string idEmp = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(idEmp))
                {
                    Console.WriteLine("El ID del empleado no puede estar vacío.");
                    return;
                }

                if (!empleados.ContainsKey(idEmp))
                {
                    Console.WriteLine("Empleado nuevo detectado. Ingrese sus datos:");
                    Console.Write("Nombre: "); string nomEmp = Console.ReadLine();
                    Console.Write("Apellido: "); string apeEmp = Console.ReadLine();
                    Console.Write("Departamento: "); string depEmp = Console.ReadLine();

                    empleados.Add(idEmp, new Empleado(idEmp, nomEmp, apeEmp, depEmp));
                }

                visitas.Add(new Visita(siguienteIdVisita, visitantes[idVis], empleados[idEmp]));
                Console.WriteLine($"\n¡Visita registrada! Ticket asignado: {siguienteIdVisita}");
                siguienteIdVisita++;
            }
            catch (Exception)
            {
                Console.WriteLine("Error: Entrada de datos inválida.");
            }
        }

        static void BuscarVisitas()
        {
            Console.Clear();
            Console.WriteLine("--- BUSCAR VISITAS (CRITERIOS) ---");
            Console.WriteLine("1. Buscar por Ticket de Visita");
            Console.WriteLine("2. Buscar por Nombre del Visitante");
            Console.WriteLine("3. Ver visitas activas en el edificio");
            Console.Write("Seleccione (1-3): ");
            string criterio = Console.ReadLine();

            if (criterio == "1")
            {
                try
                {
                    Console.Write("Ingrese el número de ticket a buscar: ");
                    int idBuscado = Convert.ToInt32(Console.ReadLine());
                    Visita v = visitas.Find(x => x.IdVisita == idBuscado);

                    if (v != null)
                    {
                        Console.WriteLine("\n[Resultado Encontrado]");
                        Console.WriteLine($"Ticket ID: {v.IdVisita}");
                        Console.WriteLine($"Visitante: {v.VisitanteAsignado.Nombre} {v.VisitanteAsignado.Apellido} ({v.VisitanteAsignado.Empresa})");
                        Console.WriteLine($"Atendido por: {v.EmpleadoAsignado.Nombre} {v.EmpleadoAsignado.Apellido} - Depto: {v.EmpleadoAsignado.Departamento}");
                        Console.WriteLine($"Entrada: {v.HoraEntradaTxt} | Salida: {v.HoraSalidaTxt}");
                        Console.WriteLine($"Estado: {(v.Activa ? "En el edificio" : "Fuera")}");
                    }
                    else Console.WriteLine("No existe ninguna visita con ese código.");
                }
                catch { Console.WriteLine("Error: El ticket debe ser numérico."); }
            }
            else if (criterio == "2")
            {
                Console.Write("Ingrese el nombre del visitante a buscar: ");
                string nombreBuscado = Console.ReadLine().ToLower();
                bool encontrado = false;

                foreach (var v in visitas)
                {
                    if (v.VisitanteAsignado.Nombre.ToLower().Contains(nombreBuscado) || v.VisitanteAsignado.Apellido.ToLower().Contains(nombreBuscado))
                    {
                        encontrado = true;
                        Console.WriteLine($"Ticket: {v.IdVisita} | Visitante: {v.VisitanteAsignado.Nombre} {v.VisitanteAsignado.Apellido} | Estado: {(v.Activa ? "En el edificio" : "Fuera")}");
                    }
                }
                if (!encontrado) Console.WriteLine("No se encontraron registros.");
            }
            else if (criterio == "3")
            {
                bool encontrado = false;
                foreach (var v in visitas)
                {
                    if (v.Activa)
                    {
                        encontrado = true;
                        Console.WriteLine($"Ticket: {v.IdVisita} | Visitante: {v.VisitanteAsignado.Nombre} {v.VisitanteAsignado.Apellido} | Entrada: {v.HoraEntradaTxt}");
                    }
                }
                if (!encontrado) Console.WriteLine("No hay visitas dentro del edificio.");
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
                Visita v = visitas.Find(x => x.IdVisita == idBuscado);

                if (v != null)
                {
                    Console.Write($"Nuevo Nombre Visitante (actual: {v.VisitanteAsignado.Nombre}): ");
                    string nuevoNombre = Console.ReadLine();
                    if (!string.IsNullOrEmpty(nuevoNombre)) v.VisitanteAsignado.Nombre = nuevoNombre;

                    Console.Write($"Nuevo Nombre del Empleado (actual: {v.EmpleadoAsignado.Nombre}): ");
                    string nuevoNombreEmp = Console.ReadLine();
                    if (!string.IsNullOrEmpty(nuevoNombreEmp)) v.EmpleadoAsignado.Nombre = nuevoNombreEmp;

                    Console.WriteLine("¡Información actualizada exitosamente!");
                }
                else Console.WriteLine("El ticket ingresado no existe.");
            }
            catch { Console.WriteLine("Error: Ingrese un número de ticket válido."); }
        }

        static void RegistrarSalida()
        {
            Console.Clear();
            Console.WriteLine("--- REGISTRAR SALIDA ---");

            try
            {
                Console.Write("Ingrese el Ticket de la Visita para marcar salida: ");
                int idBuscado = Convert.ToInt32(Console.ReadLine());
                Visita v = visitas.Find(x => x.IdVisita == idBuscado);

                if (v != null)
                {
                    if (v.Activa)
                    {
                        v.HoraSalidaTxt = DateTime.Now.ToString("hh:mm tt");
                        v.Activa = false;
                        Console.WriteLine($"Salida marcada de forma correcta a las {v.HoraSalidaTxt}");
                    }
                    else Console.WriteLine($"Este ticket ya registró su salida a las: {v.HoraSalidaTxt}");
                }
                else Console.WriteLine("Ticket no encontrado.");
            }
            catch { Console.WriteLine("Error: El ticket debe ser numérico."); }
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
            foreach (var v in visitas)
            {
                Console.WriteLine($"Ticket: {v.IdVisita} | Visitante: {v.VisitanteAsignado.Nombre} {v.VisitanteAsignado.Apellido} | Empleado: {v.EmpleadoAsignado.Nombre} | Entrada: {v.HoraEntradaTxt} | Salida: {v.HoraSalidaTxt} | Estado: {(v.Activa ? "En el edificio" : "Fuera")}");
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
                Visita v = visitas.Find(x => x.IdVisita == idBuscado);
                if (v != null)
                {
                    visitas.Remove(v);
                    Console.WriteLine("Visita eliminada correctamente.");
                }
                else Console.WriteLine("Ticket no encontrado.");
            }
            catch { Console.WriteLine("Error: El ticket debe ser numérico."); }
        }
    }
}