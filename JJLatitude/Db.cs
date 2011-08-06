using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;

namespace HSPI_JJLATITUDE
{
  public static class Db
  {
    private static OleDbConnection connection;

    public static void Init(string dbPath)
    {
      string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0; " +
                                "Data Source=" + dbPath;
      connection = new OleDbConnection(connectionString);
    }

    public static void PersistToken(string name, string email, string token, string secret)
    {
      if (connection.State != ConnectionState.Open)
        connection.Open();

      string exists = null;
      using (OleDbCommand command = connection.CreateCommand())
      {
        command.CommandText = "SELECT id FROM Tokens WHERE email=@email";
        command.CommandType = CommandType.Text;
        command.Parameters.Add(new OleDbParameter("@email", email));

        try
        {
          var result = command.ExecuteScalar();
          exists = result != null ? result.ToString() : null;
        }
        finally
        {
          command.Dispose();
        }
      }

      string sql = null;
      if (exists != null && exists.Length > 0)
      {
        sql = "UPDATE [Tokens] SET name=?, token=?, secret=?, insert_time=NOW() WHERE id=?";
      }
      else
      {
        sql = "INSERT INTO Tokens (name, token, secret, email) VALUES (?,?,?,?)";
      }

      using (OleDbCommand command = connection.CreateCommand())
      {
        command.CommandText = sql;
        command.CommandType = CommandType.Text;

        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@token", token);
        command.Parameters.AddWithValue("@secret", secret);

        if (exists != null && exists.Length > 0)
          command.Parameters.AddWithValue("@id", exists);
        else
          command.Parameters.AddWithValue("@email", email);

        try
        {
          command.ExecuteNonQuery();
        }
        finally
        {
          command.Dispose();
          connection.Close();
        }
      }

    }

    public static List<Dictionary<string, string>> GetAccessTokens()
    {
      if (connection.State != ConnectionState.Open)
        connection.Open();

      List<Dictionary<string, string>> tokens = new List<Dictionary<string, string>>();

      using (OleDbCommand command = connection.CreateCommand())
      {
        command.CommandText = "SELECT name,email,token,secret FROM Tokens";
        command.CommandType = CommandType.Text;

        try
        {
          var result = command.ExecuteReader();

          while (result.Read())
          {
            Dictionary<string, string> tmpDict = new Dictionary<string, string>();
            tmpDict.Add("name", result[0].ToString());
            tmpDict.Add("email", result[1].ToString());
            tmpDict.Add("token", result[2].ToString());
            tmpDict.Add("secret", result[3].ToString());
            tokens.Add(tmpDict);
          }
          result.Close();

        }
        finally
        {
          command.Dispose();
        }
      }
      return tokens;
    }

  }
}