using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using Microsoft.Win32;

namespace HSPI_JJLATITUDE
{
  public static class Db
  {
    private static OleDbConnection connection = null;

    public static string DbPath;

    static Db()
    {
      DbPath = Registry.LocalMachine.OpenSubKey("Software\\HomeSeer Technologies\\HomeSeer 2").GetValue("Installdir").ToString();
      DbPath += "Data" + "\\" + App.PLUGIN_NAME + "\\" + App.PLUGIN_NAME + ".mdb";
    }

    private static void Init()
    {
      string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0; " +
                                "Data Source=" + DbPath;
      connection = new OleDbConnection(connectionString);

      if (connection == null)
        throw new Exception("Error initializing database connection");
    }

    private static void CheckDBConn()
    {
      if (connection == null)
        Init();

      if (connection.State != ConnectionState.Open)
        connection.Open();
    }

    public static void PersistToken(string name, string email, string token, string secret)
    {
      CheckDBConn();

      string exists = null;
      using (OleDbCommand command = connection.CreateCommand())
      {
        command.CommandText = "SELECT id FROM People WHERE email=@email";
        command.CommandType = CommandType.Text;
        command.Parameters.Add(new OleDbParameter("@email", email));

        try
        {
          var result = command.ExecuteScalar();
          exists = result != null ? result.ToString() : null;
        }
        catch (Exception) { }
        finally
        {
          command.Dispose();
        }
      }

      string sql = null;
      if (exists != null && exists.Length > 0)
      {
        sql = "UPDATE [People] SET name=?, token=?, secret=?, insert_time=NOW() WHERE id=?";
      }
      else
      {
        sql = "INSERT INTO People (name, token, secret, email) VALUES (?,?,?,?)";
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
        catch (Exception) { }
        finally
        {
          command.Dispose();
          connection.Close();
        }
      }

    }

    public static List<Dictionary<string, string>> GetAccessTokens()
    {
      if (connection == null)
        Init();

      if (connection.State != ConnectionState.Open)
        connection.Open();

      List<Dictionary<string, string>> tokens = new List<Dictionary<string, string>>();

      using (OleDbCommand command = connection.CreateCommand())
      {
        command.CommandText = "SELECT id,name,email,token,secret FROM People";
        command.CommandType = CommandType.Text;

        try
        {
          var result = command.ExecuteReader();

          while (result.Read())
          {
            Dictionary<string, string> tmpDict = new Dictionary<string, string>();
            tmpDict.Add("id", result[0].ToString());
            tmpDict.Add("name", result[1].ToString());
            tmpDict.Add("email", result[2].ToString());
            tmpDict.Add("token", result[3].ToString());
            tmpDict.Add("secret", result[4].ToString());
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
    /*
    public static void PersistPlace(string name, string lat, string lon)
    {
      CheckDBConn();

      string sql = "INSERT INTO Places (name, lat, lon) VALUES (?,?,?)";

      using (OleDbCommand command = connection.CreateCommand())
      {
        command.CommandText = sql;
        command.CommandType = CommandType.Text;

        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@lat", lat);
        command.Parameters.AddWithValue("@lon", lon);

        try
        {
          command.ExecuteNonQuery();
        }
        catch (Exception) { }
        finally
        {
          command.Dispose();
          connection.Close();
        }
      }

    }
    */
  }
}