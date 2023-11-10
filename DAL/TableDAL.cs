using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using DTO;



namespace DAL
{
    public class TableDAL 
    {

        protected TableDAL()
        {
            // Khởi tạo logic
        }
        private static TableDAL instance = null;
        public static TableDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TableDAL();
                }
                return instance;
            }
            private set
            { TableDAL.instance = value; }
        }

        public virtual List<int> danhSachIDTable()
        {
            string q = "select id from table_food";
            DataTable ds = DataProvider.Instance.excecuteQuerry(q);

            List<int> l = new List<int>();
            foreach (DataRow dr in ds.Rows)
            {
                l.Add(int.Parse(dr["id"].ToString()));
            }



            return l;

        }
        public virtual List<string> danhSachBanAn ()
        {
            string q = "select * from table_food";
            DataTable ds =DataProvider.Instance.excecuteQuerry(q);

            List<string> l = new List<string>();



            foreach (DataRow dr in ds.Rows)
            {
                l.Add(dr["name"].ToString().Trim());
            }



            return l;

        }
        public virtual bool themRow(TableDTO tableDTO)
        {
            try
            {
               
                string q = "insert into table_food values ( @name, @status)";
                bool kq = DataProvider.Instance.executeInsertQuery_Table(q, tableDTO);
                if (kq)
                    return true;
                else
                    return false;
                
            }catch(Exception ex) { 
             
                return false;
            }
           




        }
        public virtual bool xoaRow(int ma)
        {
            try
            {
                Sql_Connection.Instance.connect();
                Sql_Connection.Instance.openCon();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from Table_food where id = " + ma;
                cmd.Connection = Sql_Connection.Instance.sqlCon;





                if (cmd.ExecuteNonQuery() < 0)
                {
                    return false;
                }
                {
                    return true;
                }

            }
            catch
            {
                throw new Exception();
            } 
            

        }
        public virtual bool chinhSuaRow (TableDTO a)
        {
            try
            {
                // kt naem. status trc khi truyen vao 
                Sql_Connection.Instance.connect();
                Sql_Connection.Instance.openCon();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Table_Food\r\nset \r\n\tname = @name,\r\n\tstatus = @status\r\nwhere id = @id";
                cmd.Connection = Sql_Connection.Instance.sqlCon;


                SqlParameter parid = new SqlParameter("@id", SqlDbType.Int);
                SqlParameter parname = new SqlParameter("@name", SqlDbType.NVarChar);
                SqlParameter pars = new SqlParameter("@status", SqlDbType.NVarChar);
                parid.Value = a.Id;
                parname.Value = a.Name;
                pars.Value = a.Status;

                cmd.Parameters.Add(parid);
                cmd.Parameters.Add(parname);
                cmd.Parameters.Add(pars);



                if (cmd.ExecuteNonQuery() < 0)
                {


                    return false;
                }
                {
                    return true;
                }
            } catch { throw new Exception(); }


        }

        public virtual DataTable timKiemTable (string n)
        {
            DataTable dataTable= new DataTable();
            Sql_Connection.Instance.connect();
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType= CommandType.Text;
            cmd.CommandText = "select id as [Mã bàn ăn] , name as [Tên bàn ăn], status as [Trạng thái] from table_food where name like '%"+n+"%'";

            //SqlParameter p = new SqlParameter("@n", SqlDbType.NVarChar);
            //p.Value = n;
            //cmd.Parameters.Add(p);

            cmd.Connection = Sql_Connection.Instance.sqlCon;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);

            return dataTable;

        }

        public virtual string timNameTableByIDTable(int idTable)
        {
            DataTable dt = new DataTable();
            dt = DataProvider.Instance.excecuteQuerry("select * from Table_Food where id = " + idTable );
            DataRow dataRow = dt.Rows[0];
            return dataRow["name"].ToString().Trim();

        }
        public virtual DataTable HienThiTable()
        {
            DataTable dt = new DataTable();
            dt = DataProvider.Instance.excecuteQuerry("select id as [Mã bàn ăn] ,name as [Tên bàn ăn] ,status as [Trạng thái]from table_food");
            return dt;

        }

    }
}
