using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRegistration
{
    class Database
    {
        private static string projectPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("bin\\Debug"));
        public static string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + projectPath + @"DataBase\Employee.mdb;Persist Security Info=True";

        public static DataTable Retrieve(string sql)
        {
            DataTable dt = new DataTable();
            OleDbConnection con = new OleDbConnection(Database.conString);
            con.Open();

            OleDbCommand command = new OleDbCommand(sql, con);
            OleDbDataAdapter adt = new OleDbDataAdapter(command);

            adt.Fill(dt);

            con.Close();

            return dt;
        }

        public static void ExecuteNonQuery(string sql)
        {
            OleDbConnection con = new OleDbConnection(Database.conString);
            con.Open();

            OleDbCommand command = new OleDbCommand(sql, con);
            command.ExecuteNonQuery();

            con.Close();
        }

        public static void ExecuteNonQuery(string sql, Image img)
        {
            OleDbConnection con = new OleDbConnection(Database.conString);
            con.Open();

            OleDbCommand command = new OleDbCommand(sql, con);

            // Change image to byte[]
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            byte[] photo_aray = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(photo_aray, 0, photo_aray.Length);

            // Add image parameter
            command.Parameters.AddWithValue("@photo", photo_aray);

            command.ExecuteNonQuery();

            con.Close();
        }
    }
}
