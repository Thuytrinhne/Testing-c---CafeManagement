using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.PeerToPeer;
using System.Text;
using System.Threading.Tasks;

namespace DAL 
{
    public class DataProvider
    {
        protected DataProvider()
        {
            // Khởi tạo logic
        }
        private static DataProvider instance = null;
        public static DataProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataProvider();
                }
                return instance;
            }
            private set
            { DataProvider.instance = value; }
        }
        public virtual DataTable excecuteQuerry(string q)
        {
            try
            {
                Sql_Connection.Instance.connect();
                Sql_Connection.Instance.openCon();

                SqlCommand sqlCommand = new SqlCommand(q, Sql_Connection.Instance.sqlCon);
                DataTable data = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                adapter.Fill(data);

                return data;
            }
            catch
            {
                throw new Exception();
            }

        }
        public  bool executeInsertQuery(string q, SqlParameter[] parameters)
        {
           
                Sql_Connection.Instance.connect();
                Sql_Connection.Instance.openCon();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = q;

                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);

                }


                cmd.Connection = Sql_Connection.Instance.sqlCon;


                if (cmd.ExecuteNonQuery() < 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        public virtual bool executeInsertQuery_Table(string q, TableDTO tableDTO)
        {
            
                SqlParameter[] parameters = new SqlParameter[]
                       {
                    new SqlParameter("@name", SqlDbType.NVarChar),
                    new SqlParameter("@status", SqlDbType.NVarChar)
                       };
                parameters[0].Value = tableDTO.Name;
                parameters[1].Value = tableDTO.Status;
                return this.executeInsertQuery(q, parameters);
          
        }
    }
    
}

