using Npgsql;

class Database 
{
    private const string ConnectionString = "";
    public void TestConnection()
    {
        try
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                Console.WriteLine("Connection Success");
            }
        }
        catch
        {
            Console.WriteLine("Connection Failed");
        }
    }
    public void ReadData(
        string query, 
        NpgsqlParameter[] parameters, 
        Action<NpgsqlDataReader> reader)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand(query, connection))
            {
                if (parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var executeReader = command.ExecuteReader())
                {
                    
                    reader(executeReader);
                    
                }
            }
        }
    }
    public bool TryExecuteQuery(string query, NpgsqlParameter[] parameters)
    {
        try
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch (System.Exception)
        {
            throw;
        }
        
    }
}