using Npgsql;

class Bank
{
    private readonly string _username;
    private Database database = new Database();
    public Bank (string username)
    {
        _username = username;
    }

    public void ViewOption()
    {
        Console.WriteLine("[1] View Balance\n[2] Withdraw\n[3] Deposit\n[4] Exit");
        Console.Write("Choose: ");
        int choice = int.Parse(Console.ReadLine()!);

        switch (choice)
        {
            case 1:
                Console.WriteLine($"Your Balance is {ViewBalance()}\n");
                ViewOption();
                return;
            case 2:
                Console.Write("Enter amount: ");
                decimal withdrawAmount = decimal.Parse(Console.ReadLine()!);
                Withdraw(withdrawAmount);
                ViewOption();
                return;
            case 3:
                Console.Write("Enter amount: ");
                decimal depositAmount = decimal.Parse(Console.ReadLine()!);
                Deposit(depositAmount);
                ViewOption();
                return;
            case 4:
                Console.WriteLine("Thank you for using the system\n");
                return;
        }
    }

    public decimal ViewBalance()
    {
        string query = "SELECT amount FROM balance WHERE username = @username";
        var parameters = new[]
        {
            new NpgsqlParameter("@username", _username)
        };

        decimal amount = 0;
        database.ReadData(query, parameters, reader => {
            if (reader.Read())
            {
                amount = Convert.ToDecimal(reader["amount"]);
            }
        });

        return amount;
        
    }
    public void Deposit(decimal amount)
    {
        
        string query = "UPDATE balance SET amount = amount + @amount WHERE username = @username";

        var parameters = new[] 
        {
            new NpgsqlParameter("@amount", amount),
            new NpgsqlParameter("@username", _username)
        };

        if (database.TryExecuteQuery(query, parameters))
        {
            Console.WriteLine($"Successfully deposited {amount}\n");
        }
        else
        {
            Console.WriteLine("Deposit Failed\n");
        }
    }
    public void Withdraw(decimal amount)
    {
        if (ViewBalance() < amount)
        {
            Console.WriteLine("Not Enough Balance\n");
            return;
        }

        string query = "UPDATE balance SET amount = amount - @amount WHERE username = @username";

        var parameters = new[] 
        {
            new NpgsqlParameter("@amount", amount),
            new NpgsqlParameter("@username", _username)
        };

        if (database.TryExecuteQuery(query, parameters))
        {
            Console.WriteLine($"Successfully withdrawn {amount}\n");
        }
        else
        {
            Console.WriteLine("Withdraw Failed\n");
        }
    }
}