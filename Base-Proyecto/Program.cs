using System;
using System.Collections.Generic;

namespace RegistroEmpresa
{
    class Program
    {
        static List<int> idsEmpleados = new List<int>();
        static Dictionary<int, string> nombresEmpleados = new Dictionary<int, string>();

        static List<int> idsVisitas = new List<int>();
        static Dictionary<int, string> visitantes = new Dictionary<int, string>();
        static Dictionary<int, int> empleadoDeVisita = new Dictionary<int, int>();
        static Dictionary<int, DateTime> entradas = new Dictionary<int, DateTime>();
        static Dictionary<int, DateTime> salidas = new Dictionary<int, DateTime>();

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
                            wantToContinue = false;
                            Console.WriteLine("Cerrando el sistema...");
                            break;
                        default:
                            Console.WriteLine("Opcion no valida. Intente de nuevo.");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nError: Debe ingresar un numero entero valido para el menu.");
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
            Console.WriteLine("1. Registrar Entrada ");
            Console.WriteLine("2. Buscar Visitas ");
            Console.WriteLine("3. Modificar Informacion");
            Console.WriteLine("4. Registrar Salida (Eliminar)");
            Console.WriteLine("5. Listar Todas las Visitas");
            Console.WriteLine("6. Salir");
            Console.Write("Seleccione una opción: ");
        }

        static void RegistrarEntrada()
        {
            Console.Clear();
            Console.WriteLine("--- AGREGAR NUEVA ENTRADA ---");

            try
            {
                int nuevoIdVisita = idsVisitas.Count + 1;

                Console.Write("Nombre del visitante: ");
                string nombreVis = Console.ReadLine();

                Console.Write("ID Numérico del Empleado que lo recibe: ");
                int idEmp = Convert.ToInt32(Console.ReadLine());

                if (nombresEmpleados.ContainsKey(idEmp) == false)
                {
                    Console.WriteLine("Empleado nuevo detectado. Ingrese su nombre para registrarlo:");
                    Console.Write("Nombre del Empleado: ");
                    string nombreEmp = Console.ReadLine();

                    idsEmpleados.Add(idEmp);
                    nombresEmpleados.Add(idEmp, nombreEmp);
                }

                idsVisitas.Add(nuevoIdVisita);
                visitantes.Add(nuevoIdVisita, nombreVis);
                empleadoDeVisita.Add(nuevoIdVisita, idEmp);
                entradas.Add(nuevoIdVisita, DateTime.Now);

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
            Console.Write("Seleccione  (1-2): ");
            string criterio = Console.ReadLine();

            if (criterio == "1")
            {
                try
                {
                    Console.Write("Ingrese el numero de ticket a buscar: ");
                    int idBuscado = Convert.ToInt32(Console.ReadLine());

                    if (visitantes.ContainsKey(idBuscado) == true)
                    {
                        int idEmp = empleadoDeVisita[idBuscado];
                        string nombreEmp = nombresEmpleados[idEmp];

                        string horaSalidaTxt = "--:--";
                        string estado = "En el edificio";

                        if (salidas.ContainsKey(idBuscado) == true)
                        {
                            horaSalidaTxt = salidas[idBuscado].ToString("hh:mm tt");
                            estado = "Fuera";
                        }

                        Console.WriteLine("\n[Resultado Encontrado]");
                        Console.WriteLine($"Ticket ID: {idBuscado}");
                        Console.WriteLine($"Visitante: {visitantes[idBuscado]}");
                        Console.WriteLine($"Atendido por: [{idEmp}] {nombreEmp}");
                        Console.WriteLine($"Entrada: {entradas[idBuscado].ToString("hh:mm tt")} | Salida: {horaSalidaTxt}");
                        Console.WriteLine($"Estado: {estado}");
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

                for (int i = 0; i < idsVisitas.Count; i++)
                {
                    int id = idsVisitas[i];

                    if (visitantes[id].ToLower().Contains(nombreBuscado) == true)
                    {
                        encontrado = true;
                        int idEmp = empleadoDeVisita[id];

                        string estado = "En el edificio";
                        if (salidas.ContainsKey(id) == true)
                        {
                            estado = "Fuera";
                        }

                        Console.WriteLine($"Ticket: {id} | Visitante: {visitantes[id]} | Empleado: {nombresEmpleados[idEmp]} - Estado: {estado}");
                    }
                }

                if (encontrado == false)
                {
                    Console.WriteLine("No se encontraron registros.");
                }
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

                if (visitantes.ContainsKey(idBuscado) == true)
                {
                    Console.Write($"Nuevo Nombre Visitante (actual: {visitantes[idBuscado]}): ");
                    string nuevoNombre = Console.ReadLine();
                    if (nuevoNombre != "")
                    {
                        visitantes[idBuscado] = nuevoNombre;
                    }

                    int idEmpActual = empleadoDeVisita[idBuscado];
                    Console.Write($"Nuevo Nombre del Empleado (actual: {nombresEmpleados[idEmpActual]}): ");
                    string nuevoNombreEmp = Console.ReadLine();
                    if (nuevoNombreEmp != "")
                    {
                        nombresEmpleados[idEmpActual] = nuevoNombreEmp;
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
            Console.WriteLine("--- REGISTRAR SALIDA (ELIMINAR LOGICO) ---");

            try
            {
                Console.Write("Ingrese el Ticket de la Visita para marcar salida: ");
                int idBuscado = Convert.ToInt32(Console.ReadLine());

                if (visitantes.ContainsKey(idBuscado) == true)
                {
                    if (salidas.ContainsKey(idBuscado) == false)
                    {
                        salidas[idBuscado] = DateTime.Now;
                        Console.WriteLine($"Salida marcada de forma correcta a las {salidas[idBuscado].ToString("hh:mm tt")}");
                    }
                    else
                    {
                        Console.WriteLine($"Este ticket ya registro su salida a las: {salidas[idBuscado].ToString("hh:mm tt")}");
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

            if (idsVisitas.Count == 0)
            {
                Console.WriteLine("No hay visitas registradas.");
                return;
            }

            for (int i = 0; i < idsVisitas.Count; i++)
            {
                int id = idsVisitas[i];
                int idEmp = empleadoDeVisita[id];

                string horaSalidaTxt = "--:--";
                string estado = "En el edificio";

                if (salidas.ContainsKey(id) == true)
                {
                    horaSalidaTxt = salidas[id].ToString("hh:mm tt");
                    estado = "Fuera";
                }

                Console.WriteLine($"Ticket: {id} | Visitante: {visitantes[id]} | Empleado: {nombresEmpleados[idEmp]} | Entrada: {entradas[id].ToString("hh:mm tt")} | Salida: {horaSalidaTxt} | Estado: {estado}");
            }
        }
    }
}
