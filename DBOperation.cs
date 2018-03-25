using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
namespace SoftwareSalseAndServiceDesk
{
    public class DBOperation
    {
        public SqlConnection dbcon;
        public SqlCommand dbcmd;
        public SqlDataReader dbdatareader;
        public string str = "Data Source=PATH;Initial Catalog=DATABASENAME;Integrated Security=SSPI";
        public static string Msg;
        public static string CurOperation;
        public static string opName;

        public void dbconnection()
        {
            dbcon = new SqlConnection(str);
            dbcmd = new SqlCommand();

            if (dbcon.State == ConnectionState.Open)
            {
                dbcon.Close();
            }
            try
            {
                dbcon.Open();
                if (dbcon.State == ConnectionState.Open)
                {
                    dbcmd.Connection = dbcon; //Binding Connection With Command.
                }
            }
            catch (Exception e)
            {
                Msg = e.Message.ToString();
            }
        }
        public bool checkConnectionState()
        {
            if (dbcon.State == ConnectionState.Open)
                return true;
            else
                return false;
        }
        public void closeConnection()
        {
            if (dbcon.State == ConnectionState.Open)
            {
                dbcon.Close();
            }
        }
        public void insertData(string InsQuery)
        {
            dbconnection();
            if (checkConnectionState() == true)
            {
                dbcmd.CommandText = InsQuery;
                try
                {
                    if (dbcmd.ExecuteNonQuery() > 0)
                    {
                        DBOperation.Msg = "Data Saved.....";
                    }
                }
                catch (Exception ex)
                {
                    DBOperation.Msg = "Exception During Insert Operation " + ex.Message.ToString();
                }
            }
        }
        public void deleteData(string delQuery)
        {
            dbconnection();
            if (checkConnectionState() == true)
            {
                dbcmd.CommandText = delQuery;
                try
                {
                    if (dbcmd.ExecuteNonQuery() > 0)
                    {
                        DBOperation.Msg = "Data Deleted......";
                    }
                }
                catch (Exception ex)
                {
                    DBOperation.Msg = "Exception During Delete Operation " + ex.Message.ToString();
                }
            }
        }
        public void updateData(string updQuery)
        {
            dbconnection();
            if (checkConnectionState() == true)
            {
                dbcmd.CommandText = updQuery;
                try
                {
                    if (dbcmd.ExecuteNonQuery() > 0)
                    {
                        DBOperation.Msg = "Data Updated.....";
                    }
                }
                catch (Exception ex)
                {
                    DBOperation.Msg = "Exception During Update Operation " + ex.Message.ToString();
                }
            }
        }
        public void searchData(string serQuery)
        {
            dbconnection();
            if (checkConnectionState() == true)
            {
                dbcmd.CommandText = serQuery;
                try
                {
                    if (dbcmd.ExecuteNonQuery() > 0)
                    {
                        DBOperation.Msg = "Data Searched.....";
                    }
                }
                catch (Exception ex)
                {
                    DBOperation.Msg = "Exception During Search Operation...." + ex.Message.ToString();
                }
            }
        }
        public void selectRecord(string selQuery)
        {
            dbconnection();
            if (checkConnectionState() == true)
            {
                dbcmd.CommandText = selQuery;
                try
                {
                    dbdatareader = dbcmd.ExecuteReader();
                }
                catch (Exception ex)
                {
                    DBOperation.Msg = "Exception During Selction Operation : " + ex.Message.ToString();
                }
            }
        }
        public void LoadCombo(DropDownList ddl, string tblName, string colName)
        {
            selectRecord(" select distinct " + colName + " from " + tblName);
            ddl.Items.Clear();
            while (dbdatareader.Read())
            {
                ddl.Items.Add(dbdatareader.GetValue(0).ToString());
            }

        }
        public string ConvertDate(string DateToConvert)
        {
            string ConvertedDate = "";

            string[] DateArray = DateToConvert.Split('-');

            ConvertedDate = DateArray[2] + "-" + DateArray[1] + "-" + DateArray[0];

            return ConvertedDate;
        }
        public long getNextSrNo(string tblName, string colName)
        {
            selectRecord(" select " + colName + " from " + tblName + " order by " + colName + " desc ");
            if (dbdatareader.HasRows == true)
            {
                if (dbdatareader.Read())
                {
                    return Int64.Parse(dbdatareader.GetValue(0).ToString()) + 1;
                }
            }
            return 1;
            dbdatareader.Close();
            closeConnection();
        }
        public DataTable fillDataTable(string sqlQuery)
        {
            DataTable dT = new DataTable();
            SqlDataAdapter dA = new SqlDataAdapter(sqlQuery,str);
            dA.Fill(dT);
            return dT;
        }
    }
}
