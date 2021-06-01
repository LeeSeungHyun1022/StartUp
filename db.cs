using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db : MonoBehaviour
{
    /*
    static db _instance = null;

    public static db Instance
    {
        get
        {
            return _instance;
        }
    }

    string constr = "Server=127.0.0.1;" +
                    "Port=3306;" +
                    "Database=UnityDB;" +
                    "User ID=root;" +
                    "Password=root;" +
                    "Pooling=true";

    MySqlConnection con = null;
    MySqlCommand cmd = null;

    private void Awake()
    {
        try
        {
            con = new MySqlConnection(constr);
            con.Open();
            Debug.Log("Connection State : " + con.State);
        }
        catch (MySqlException ex)
        {
            Debug.Log(ex.ToString());
        }

        MySqlDataReader reader = SelectDB();

        while (reader.Read())
        {
            string temp = reader["stage1"].ToString();
            temp += ";";
            temp += reader["stage2"].ToString();
            temp += ";";
            temp += reader["stage3"].ToString();
            temp += ";";
            temp += reader["stage4"].ToString();
            temp += ";";
            temp += reader["stage5"].ToString();
            temp += ";";
            temp += reader["total"].ToString();

            Debug.Log(temp);
        }
    }

    public MySqlDataReader SelectDB()
    {
        MySqlDataReader reader = null;
        cmd.CommandText = "Select * From unitydb";

        try
        {
            reader = cmd.ExecuteReader();
            reader.Read();
            con.Close();
        }
        catch
        {
            con.Close();
        }
        return reader;
    }
    */
}
