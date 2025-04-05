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
        Console.WriteLine("[1] View Balance\n[2] Withdraw\n[3] Deposit\n[4] Transfer Money\n[5] Delete Account\n[6] Exit");
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
                Console.Write("Enter receiver username: ");
                string receiver = Console.ReadLine()!;
                if (IsValidUser(receiver))
                {
                    Console.Write("Enter amount: ");
                    decimal transferAmount = decimal.Parse(Console.ReadLine()!);
                    TransferMoney(receiver, transferAmount);
                }
                else
                {
                    Console.WriteLine("Username does not exists!");
                }
                ViewOption();
                return;
            case 5:
                Console.WriteLine("Are you sure you want to delete your account? Your balance will be remove too \n[1] Yes \n[2] No");
                Console.Write("Choose: ");
                int deleteOption = int.Parse(Console.ReadLine()!);
                if (deleteOption == 1)
                {
                    DeleteUser();
                    break;
                }
                ViewOption();
                return;
            case 6:
                Console.WriteLine("Thank you for using the system\n");
                return;
        }
    }

    public decimal ViewBalance()
    {
        string query = "";
        var parameters = new[]
        {
            new NpgsqlParameter("", _username)
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
        
        string query = "";

        var parameters = new[] 
        {
            new NpgsqlParameter("", amount),
            new NpgsqlParameter("", _username)
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

        string query = "";

        var parameters = new[] 
        {
            new NpgsqlParameter("", amount),
            new NpgsqlParameter("", _username)
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
    public void TransferMoney(string receiver, decimal amount)
    {
        if (ViewBalance() < amount)
        {
            Console.WriteLine("Not Enough Balance\n");
            return;
        }
        string query = "";

        var parameters = new[] 
        {
            new NpgsqlParameter("", amount),
            new NpgsqlParameter("", receiver)
        };

        if (database.TryExecuteQuery(query, parameters))
        {
            Console.WriteLine($"Successfully transferred {amount} to {receiver}\n");
        }
        else
        {
            Console.WriteLine("Transferred Failed\n");
        }
    }
    public bool IsValidUser(string username)
    {
        string query = "";
        var parameters = new[]
        {
            new NpgsqlParameter("", username)
        };

        bool isValid = false;
        database.ReadData(query, parameters, reader => {
            if (reader.Read())
            {
                isValid = true;
            }
        });
        return isValid;
    }
    public void DeleteUser()
    {
        string query = "";
        var parameters = new[]
        {
            new NpgsqlParameter("", _username)
        };

        if (database.TryExecuteQuery(query, parameters))
        {
            Console.WriteLine("Succesfully deleted your account!");
        }
        else
        {
            Console.WriteLine("Account deletion failed!");
        }
    }
}