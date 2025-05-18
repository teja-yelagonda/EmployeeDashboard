using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ADO.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=(LocalDB)\\TejaYelagonda;Database=ADO.NET;Trusted_Connection=True;";
            //new Program().CreateTable();
            //new Program().InsertIntoTable();
            //new Program().DeleteFromTable();
            //new Program().UpdateTable();
            //new Program().SelectFromTable();
            //new Program().ExecuteSP();
            //new Program().ExecuteParameterSP();
            new Program().FetchUsingDA(connectionString);
        }
        public void CreateTable()
        {
            try
            {
                string connectionString = "Server=(LocalDB)\\TejaYelagonda;Database=ADO.NET;Trusted_Connection=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Create table Students(Id int not null,Name varchar(100))", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Table Created Successfully");
                }

            }
            catch
            {
                Console.WriteLine("something went wrong");
            }
            finally
            {
                Console.ReadLine();
            }
        }
        public void InsertIntoTable()
        {
            try
            {
                string connectionString = "Server=(LocalDB)\\TejaYelagonda;Database=ADO.NET;Trusted_Connection=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Insert into Students(Id, Name) values(1,'Teja'),(2,'Yelagonda')", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Inserted Values");
                }
            }
            catch
            {
                Console.WriteLine("something went wrong");
            }
            finally
            {
                Console.ReadLine();
            }
        }
        public void DeleteFromTable()
        {
            try
            {
                string connectionString = "Server=(LocalDB)\\TejaYelagonda;Database=ADO.NET;Trusted_Connection=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("delete from Students where id=1", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Deleted Records Successfully");
                }

            }
            catch
            {
                Console.WriteLine("something went wrong");
            }
            finally
            {
                Console.ReadLine();
            }
        }
        public void UpdateTable()
        {
            try
            {
                string connectionString = "Server=(LocalDB)\\TejaYelagonda;Database=ADO.NET;Trusted_Connection=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("update Students set Name = 'Teja' where Id = 2", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Table updated");
                }

            }
            catch
            {
                Console.WriteLine("something went wrong");
            }
            finally
            {
                Console.ReadLine();
            }
        }
        public void SelectFromTable()
        {
            try
            {
                string connectionString = "Server=(LocalDB)\\TejaYelagonda;Database=ADO.NET;Trusted_Connection=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Select * from Students", conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Id={reader["Id"]},  Name={reader["Name"]}");
                    }

                    Console.WriteLine("Records retrived");
                }

            }
            catch
            {
                Console.WriteLine("something went wrong");
            }
            finally
            {
                Console.ReadLine();
            }
        }
        public void ExecuteSP()
        {
            try
            {
                string connectionString = "Server=(LocalDB)\\TejaYelagonda;Database=ADO.NET;Trusted_Connection=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "GetStudents";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Id={reader["Id"]},  Name={reader["Name"]}");
                    }

                    Console.WriteLine("Executed StoredProcedure");
                }

            }
            catch
            {
                Console.WriteLine("something went wrong");
            }
            finally
            {
                Console.ReadLine();
            }
        }
        public void ExecuteParameterSP()
        {
            try
            {
                string connectionString = "Server=(LocalDB)\\TejaYelagonda;Database=ADO.NET;Trusted_Connection=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "GetStudentById";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    SqlParameter parameter = new SqlParameter("@Id", SqlDbType.Int);
                    parameter.Value = 1;
                    cmd.Parameters.Add(parameter);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Id={reader["Id"]},  Name={reader["Name"]}");
                    }

                    Console.WriteLine("Executed StoredProcedure");
                }

            }
            catch
            {
                Console.WriteLine("something went wrong");
            }
            finally
            {
                Console.ReadLine();

            }
        }
        public void FetchUsingDA(string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "Select * from Students";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    Console.WriteLine("data from DataTable");
                    foreach(DataRow row in dt.Rows)
                    {
                        Console.WriteLine($"id={row[0]},  Name={row[1]}");
                    }
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);
                    {
                    foreach (DataRow row in dt.Rows)
                        Console.WriteLine($"id={row[0]},  Name={row[1]}");
                    }

                    ///////////////////////////////////////////////////////////////////////////////////////

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    ds.Tables.Add(dt2);
                    Console.WriteLine("data from DataSet");
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        Console.WriteLine($"id={row[0]}, Name={row[1]}");
                    }
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        Console.WriteLine($"id={row[0]}, Name={row[1]}");
                    }
                    Console.WriteLine("Executed query using DataAdapter");
                }

            }
            catch
            {
                Console.WriteLine("something went wrong");
            }
            finally
            {
                Console.ReadLine();
            }
        }

    }
}
