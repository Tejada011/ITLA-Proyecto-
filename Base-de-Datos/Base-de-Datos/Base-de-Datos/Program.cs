using System;
using System.Data.SqlClient;

namespace Base_de_Datos
{
    class Program
    {
        static string connectionString = @"Server=DESKTOP-Q1OJ2CN\SQLEXPRESS;Database=Registroempresa;Trusted_Connection=True;TrustServerCertificate=True;";

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
                catch (SqlException ex)
                {
                    Console.WriteLine($"\nError de base de datos: {ex.Message}");
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

        static string ObtenerEstado(bool activa)
        {
            if (activa == true)
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

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string sqlCheckVis = "SELECT COUNT(*) FROM Visitantes WHERE Id = @Id";
                    SqlCommand cmdCheckVis = new SqlCommand(sqlCheckVis, con);
                    cmdCheckVis.Parameters.AddWithValue("@Id", idVis);
                    int existeVis = (int)cmdCheckVis.ExecuteScalar();

                    if (existeVis == 0)
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

                        Visitante nuevoV = new Visitante(idVis, nombreVis, apellidoVis, empresaVis);

                        string sqlInsertVis = "INSERT INTO Visitantes (Id, Nombre, Apellido, Empresa) VALUES (@Id, @Nombre, @Apellido, @Empresa)";
                        SqlCommand cmdInsertVis = new SqlCommand(sqlInsertVis, con);
                        cmdInsertVis.Parameters.AddWithValue("@Id", nuevoV.Id);
                        cmdInsertVis.Parameters.AddWithValue("@Nombre", nuevoV.Nombre);
                        cmdInsertVis.Parameters.AddWithValue("@Apellido", nuevoV.Apellido);
                        cmdInsertVis.Parameters.AddWithValue("@Empresa", nuevoV.Empresa);
                        cmdInsertVis.ExecuteNonQuery();
                    }

                    Console.Write("ID Numérico del Empleado que lo recibe: ");
                    int idEmpNumero = Convert.ToInt32(Console.ReadLine());

                    if (idEmpNumero <= 0)
                    {
                        Console.WriteLine("El ID del empleado debe ser mayor que cero.");
                        return;
                    }

                    string idEmp = idEmpNumero.ToString();

                    string sqlCheckEmp = "SELECT COUNT(*) FROM Empleados WHERE Id = @Id";
                    SqlCommand cmdCheckEmp = new SqlCommand(sqlCheckEmp, con);
                    cmdCheckEmp.Parameters.AddWithValue("@Id", idEmp);
                    int existeEmp = (int)cmdCheckEmp.ExecuteScalar();

                    if (existeEmp == 0)
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

                        Empleado nuevoE = new Empleado(idEmp, nombreEmp, apellidoEmp, departamentoEmp);

                        string sqlInsertEmp = "INSERT INTO Empleados (Id, Nombre, Apellido, Departamento) VALUES (@Id, @Nombre, @Apellido, @Departamento)";
                        SqlCommand cmdInsertEmp = new SqlCommand(sqlInsertEmp, con);
                        cmdInsertEmp.Parameters.AddWithValue("@Id", nuevoE.Id);
                        cmdInsertEmp.Parameters.AddWithValue("@Nombre", nuevoE.Nombre);
                        cmdInsertEmp.Parameters.AddWithValue("@Apellido", nuevoE.Apellido);
                        cmdInsertEmp.Parameters.AddWithValue("@Departamento", nuevoE.Departamento);
                        cmdInsertEmp.ExecuteNonQuery();
                    }

                    string horaEntrada = DateTime.Now.ToString("hh:mm tt");
                    string horaSalida = "--:--";
                    bool activa = true;

                    string sqlInsertVisita = @"
                        INSERT INTO Visitas (IdVisitante, IdEmpleado, HoraEntrada, HoraSalida, Activa) 
                        VALUES (@IdVis, @IdEmp, @Entrada, @Salida, @Activa);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    SqlCommand cmdVisita = new SqlCommand(sqlInsertVisita, con);
                    cmdVisita.Parameters.AddWithValue("@IdVis", idVis);
                    cmdVisita.Parameters.AddWithValue("@IdEmp", idEmp);
                    cmdVisita.Parameters.AddWithValue("@Entrada", horaEntrada);
                    cmdVisita.Parameters.AddWithValue("@Salida", horaSalida);
                    cmdVisita.Parameters.AddWithValue("@Activa", activa);

                    int nuevoIdVisita = (int)cmdVisita.ExecuteScalar();

                    Console.WriteLine($"\n¡Visita registrada! Ticket de visita asignado: {nuevoIdVisita}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error real: {ex.Message}");
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                if (criterio == "1")
                {
                    try
                    {
                        Console.Write("Ingrese el numero de ticket a buscar: ");
                        int idBuscado = Convert.ToInt32(Console.ReadLine());
                        string sql = @"
SELECT V.IdVisita, V.HoraEntrada, V.HoraSalida, V.Activa,
Vis.Nombre AS VisNombre, Vis.Apellido AS VisApellido, Vis.Empresa,
Emp.Id AS EmpId, Emp.Nombre AS EmpNombre, Emp.Apellido AS EmpApellido, Emp.Departamento
FROM Visitas V
INNER JOIN Visitantes Vis ON V.IdVisitante = Vis.Id
INNER JOIN Empleados Emp ON V.IdEmpleado = Emp.Id
WHERE V.IdVisita = @Id";
                        SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@Id", idBuscado);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            bool activa = (bool)reader["Activa"];
                            Console.WriteLine("\n[Resultado Encontrado]");
                            Console.WriteLine($"Ticket ID: {reader["IdVisita"]}");
                            Console.WriteLine($"Visitante: {reader["VisNombre"]} {reader["VisApellido"]} ({reader["Empresa"]})");
                            Console.WriteLine($"Atendido por: [{reader["EmpId"]}] {reader["EmpNombre"]} {reader["EmpApellido"]} - Depto: {reader["Departamento"]}");
                            Console.WriteLine($"Entrada: {reader["HoraEntrada"]} | Salida: {reader["HoraSalida"]}");
                            Console.WriteLine($"Estado: {ObtenerEstado(activa)}");
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
                    string sql = @"
SELECT V.IdVisita, V.Activa,
Vis.Nombre AS VisNombre, Vis.Apellido AS VisApellido,
Emp.Nombre AS EmpNombre
FROM Visitas V
INNER JOIN Visitantes Vis ON V.IdVisitante = Vis.Id
INNER JOIN Empleados Emp ON V.IdEmpleado = Emp.Id
WHERE LOWER(Vis.Nombre) LIKE @Nombre OR LOWER(Vis.Apellido) LIKE @Nombre";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@Nombre", "%" + nombreBuscado + "%");
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        encontrado = true;
                        bool activa = (bool)reader["Activa"];
                        Console.WriteLine($"Ticket: {reader["IdVisita"]} | Visitante: {reader["VisNombre"]} {reader["VisApellido"]} | Empleado: {reader["EmpNombre"]} - Estado: {ObtenerEstado(activa)}");
                    }
                    if (encontrado == false)
                    {
                        Console.WriteLine("No se encontraron registros.");
                    }
                }
                else if (criterio == "3")
                {
                    bool encontrado = false;
                    string sql = @"
SELECT V.IdVisita, V.HoraEntrada,
Vis.Nombre AS VisNombre, Vis.Apellido AS VisApellido,
Emp.Nombre AS EmpNombre
FROM Visitas V
INNER JOIN Visitantes Vis ON V.IdVisitante = Vis.Id
INNER JOIN Empleados Emp ON V.IdEmpleado = Emp.Id
WHERE V.Activa = 1";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        encontrado = true;
                        Console.WriteLine($"Ticket: {reader["IdVisita"]} | Visitante: {reader["VisNombre"]} {reader["VisApellido"]} | Empleado: {reader["EmpNombre"]} | Entrada: {reader["HoraEntrada"]}");
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
        }
        static void ModificarVisita()
        {
            Console.Clear();
            Console.WriteLine("--- MODIFICAR DATOS ---");
            try
            {
                Console.Write("Ingrese el Ticket de la visita a modificar: ");
                int idBuscado = Convert.ToInt32(Console.ReadLine());
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlSelect = @"
SELECT V.IdVisitante, V.IdEmpleado, Vis.Nombre AS VisNombre, Emp.Nombre AS EmpNombre
FROM Visitas V
INNER JOIN Visitantes Vis ON V.IdVisitante = Vis.Id
INNER JOIN Empleados Emp ON V.IdEmpleado = Emp.Id
WHERE V.IdVisita = @Id";
                    SqlCommand cmdSelect = new SqlCommand(sqlSelect, con);
                    cmdSelect.Parameters.AddWithValue("@Id", idBuscado);
                    SqlDataReader reader = cmdSelect.ExecuteReader();
                    if (reader.Read())
                    {
                        string idVis = reader["IdVisitante"].ToString();
                        string idEmp = reader["IdEmpleado"].ToString();
                        string actualVis = reader["VisNombre"].ToString();
                        string actualEmp = reader["EmpNombre"].ToString();
                        reader.Close();
                        Console.Write($"Nuevo Nombre Visitante (actual: {actualVis}): ");
                        string nuevoNombre = Console.ReadLine();
                        if (nuevoNombre.Trim() != "")
                        {
                            string sqlUpdateVis = "UPDATE Visitantes SET Nombre = @Nombre WHERE Id = @Id";
                            SqlCommand cmdUpdVis = new SqlCommand(sqlUpdateVis, con);
                            cmdUpdVis.Parameters.AddWithValue("@Nombre", nuevoNombre.Trim());
                            cmdUpdVis.Parameters.AddWithValue("@Id", idVis);
                            cmdUpdVis.ExecuteNonQuery();
                        }
                        Console.Write($"Nuevo Nombre del Empleado (actual: {actualEmp}): ");
                        string nuevoNombreEmp = Console.ReadLine();
                        if (nuevoNombreEmp.Trim() != "")
                        {
                            string sqlUpdateEmp = "UPDATE Empleados SET Nombre = @Nombre WHERE Id = @Id";
                            SqlCommand cmdUpdEmp = new SqlCommand(sqlUpdateEmp, con);
                            cmdUpdEmp.Parameters.AddWithValue("@Nombre", nuevoNombreEmp.Trim());
                            cmdUpdEmp.Parameters.AddWithValue("@Id", idEmp);
                            cmdUpdEmp.ExecuteNonQuery();
                        }
                        Console.WriteLine("¡Informacion actualizada exitosamente!");
                    }
                    else
                    {
                        Console.WriteLine("El ticket ingresado no existe.");
                    }
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
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlCheck = "SELECT Activa, HoraSalida FROM Visitas WHERE IdVisita = @Id";
                    SqlCommand cmdCheck = new SqlCommand(sqlCheck, con);
                    cmdCheck.Parameters.AddWithValue("@Id", idBuscado);
                    SqlDataReader reader = cmdCheck.ExecuteReader();
                    if (reader.Read())
                    {
                        bool activa = (bool)reader["Activa"];
                        string horaSalidaAnterior = reader["HoraSalida"].ToString();
                        reader.Close();
                        if (activa == true)
                        {
                            string horaSalidaActual = DateTime.Now.ToString("hh:mm tt");
                            string sqlUpdate = "UPDATE Visitas SET HoraSalida = @Salida, Activa = 0 WHERE IdVisita = @Id";
                            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, con);
                            cmdUpdate.Parameters.AddWithValue("@Salida", horaSalidaActual);
                            cmdUpdate.Parameters.AddWithValue("@Id", idBuscado);
                            cmdUpdate.ExecuteNonQuery();
                            Console.WriteLine($"Salida marcada de forma correcta a las {horaSalidaActual}");
                        }
                        else
                        {
                            Console.WriteLine($"Este ticket ya registro su salida a las: {horaSalidaAnterior}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ticket no encontrado.");
                    }
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sql = @"
SELECT V.IdVisita, V.HoraEntrada, V.HoraSalida, V.Activa,
Vis.Nombre AS VisNombre, Vis.Apellido AS VisApellido,
Emp.Nombre AS EmpNombre
FROM Visitas V
INNER JOIN Visitantes Vis ON V.IdVisitante = Vis.Id
INNER JOIN Empleados Emp ON V.IdEmpleado = Emp.Id";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                bool hayRegistros = false;
                while (reader.Read())
                {
                    hayRegistros = true;
                    bool activa = (bool)reader["Activa"];
                    Console.WriteLine($"Ticket: {reader["IdVisita"]} | Visitante: {reader["VisNombre"]} {reader["VisApellido"]} | Empleado: {reader["EmpNombre"]} | Entrada: {reader["HoraEntrada"]} | Salida: {reader["HoraSalida"]} | Estado: {ObtenerEstado(activa)}");
                }
                if (hayRegistros == false)
                {
                    Console.WriteLine("No hay visitas registradas.");
                }
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
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlDelete = "DELETE FROM Visitas WHERE IdVisita = @Id";
                    SqlCommand cmdDelete = new SqlCommand(sqlDelete, con);
                    cmdDelete.Parameters.AddWithValue("@Id", idBuscado);
                    int filasAfectadas = cmdDelete.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        Console.WriteLine("Visita eliminada correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Ticket no encontrado.");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error: El ticket debe ser numerico.");
            }
        }
    }
}