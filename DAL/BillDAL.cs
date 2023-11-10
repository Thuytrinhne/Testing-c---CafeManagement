using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public  class BillDAL
    {
        private static BillDAL instance = null;
        private BillDAL() { }

        public static BillDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BillDAL();
                }
                return instance;
            }
            private set
            { BillDAL.instance = value; }
        }


        // tìm mã bill chưa thanh toán khi biết bàn 

        public int  getUncheckBillByTable (int id)
        {
            int m = id;
            int ma;
            Sql_Connection.Instance.connect();
            Sql_Connection.Instance.openCon();

            DataTable t = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "getUncheckBillByTable";
            cmd.Connection = Sql_Connection.Instance.sqlCon;

            SqlParameter p = new SqlParameter("@ma", SqlDbType.Int);
            p.Value = id;
            cmd.Parameters.Add(p);


            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(t);


            // kiểm tra liệu có null 


            if (t.Rows.Count > 0)
            {
                DataRow dr = t.Rows[0];
                ma = (int)dr["id"];
                return ma;
            }

            return -1;
        }
       public bool  thucHienCheckOut(int maBan)
        {

            int maBill = getUncheckBillByTable(maBan);
            if (maBill ==-1) { return false; }
            Sql_Connection.Instance.connect();
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "thucHienCheckOut";

            SqlParameter par = new SqlParameter("@maBill", SqlDbType.Int);
            par.Value = maBill;
            cmd.Parameters.Add(par);

            cmd.Connection = Sql_Connection.Instance.sqlCon;

            int kq =cmd.ExecuteNonQuery();
            if (kq < 0) return false;
            return true;


        }
       public bool themBill(int maBan)
        {
            try
            {

                Sql_Connection.Instance.connect();
                Sql_Connection.Instance.openCon();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "themBill";

                SqlParameter pa = new SqlParameter("@idTable", SqlDbType.Int);
                pa.Value = maBan;
                cmd.Parameters.Add(pa);



                cmd.Connection = Sql_Connection.Instance.sqlCon;
                int kq = cmd.ExecuteNonQuery();
                if (kq < 0) return false; return true;
            } catch
            {
                return false;
            }

        }
        public bool ChuyenBan(int maBill, int maBanNew )
        {
            Sql_Connection.Instance.connect();
            Sql_Connection.Instance.openCon();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "chuyenBan";

            SqlParameter paBill = new SqlParameter("@idBill", SqlDbType.Int);
            SqlParameter paTable = new SqlParameter("@idNew", SqlDbType.Int);
            paTable.Value = maBanNew;
            paBill.Value = maBill;
            cmd.Parameters.Add(paBill);
            cmd.Parameters.Add(paTable);



            cmd.Connection = Sql_Connection.Instance.sqlCon;
            int kq = cmd.ExecuteNonQuery();
            if (kq < 0) return false; return true;


        }

        public DataTable HienThiDoanhThu(int page, DateTime dateStart, DateTime dateEnd)
        {
            DataTable dataTable = new DataTable();
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "HienThiDoanhThuPhanTrang";
            SqlParameter parD1 = new SqlParameter("@dateStart", SqlDbType.VarChar);
            SqlParameter parD2 = new SqlParameter("@dateEnd", SqlDbType.VarChar);
            SqlParameter parD3 = new SqlParameter("@page_num", SqlDbType.Int);

            parD1.Value = dateStart.ToString("yyyy-MM-dd");
            parD2.Value = dateEnd.ToString("yyyy-MM-dd")+" 23:59:59";
            parD3.Value = page;
            cmd.Parameters.Add(parD1);
            cmd.Parameters.Add(parD2);
            cmd.Parameters.Add(parD3);

            cmd.Connection = Sql_Connection.Instance.sqlCon;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);
            return dataTable;


        }

        public static int  hienThiTongDanhThu(DateTime dateStart,DateTime dateEnd)
        {
            DataTable dataTable = new DataTable();
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "HienThiTongDoanhThu";
            SqlParameter parD1 = new SqlParameter("@dateStart", SqlDbType.Date);
            SqlParameter parD2 = new SqlParameter("@dateEnd", SqlDbType.Date);

            parD1.Value = dateStart;
            parD2.Value = dateEnd;
            cmd.Parameters.Add(parD1);
            cmd.Parameters.Add(parD2);

            cmd.Connection = Sql_Connection.Instance.sqlCon;
            DataTable d = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(d);

            DataRow dr = d.Rows[0];
            if ( dr.IsNull("Tổng")  )
            {
                return 0;
            }

            return int.Parse(dr["Tổng"].ToString());



        }




        public bool capNhatDiscount(int maBill,decimal  discount)
        {
            Sql_Connection.Instance.openCon();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "capNhatDiscount";

            SqlParameter parid = new SqlParameter("@id", SqlDbType.Int);
            SqlParameter parDis = new SqlParameter("@discount", SqlDbType.Int);
            parid.Value = maBill;
            parDis.Value =(int) discount;

            cmd.Parameters.Add(parid);
            cmd.Parameters.Add(parDis);
            cmd.Connection= Sql_Connection.Instance.sqlCon;

            int kq = cmd.ExecuteNonQuery();
            if (kq<0)
            {
                return false;
            }
            return true;


        }
        public static int getSizeOfBill(DateTime dateStart, DateTime dateEnd)
        {
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            String dateStartString = dateStart.ToString("yyyy-MM-dd");
            String dateEndString = dateEnd.ToString("yyyy-MM-dd")+" 23:59:59";
            //string query = string.Format("select count(id) from bill where dateCheckIn between {0} and {1}", dateStart, dateEnd);
            cmd.CommandText = "select count(id) from bill where (dateCheckIn between '" + dateStartString  + "' and '" + dateEndString+"') and status = 1";
            
            cmd.Connection = Sql_Connection.Instance.sqlCon;

           int kq = (int)cmd.ExecuteScalar();
           return kq;

        }
        public int getDiscount (int maBill)
        {
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select discount from bill where id = " + maBill;
            cmd.Connection = Sql_Connection.Instance.sqlCon;


            int kq =int.Parse(cmd.ExecuteScalar().ToString().Trim());

            return kq;


        }
        public bool huyBill(int maBill)
        {
            // xóa bill infor
            



            if (!xoaBill_Infor(maBill))
                return false;
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from Bill where id = " + maBill;
            cmd.Connection = Sql_Connection.Instance.sqlCon;

            int kq = cmd.ExecuteNonQuery();
            if (kq < 0) { return false; }
            return true;



        }

        private bool xoaBill_Infor(int maBill)
        {
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from Bill_infor where idBill = " + maBill;
            cmd.Connection = Sql_Connection.Instance.sqlCon;

            int kq = cmd.ExecuteNonQuery();
           if (kq<0) { return false; }
            return true;


        }
        public static  DataTable HienThiDoanhThuForReport( DateTime dateStart, DateTime dateEnd)
        {
            DataTable dataTable = new DataTable();
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "HienThiDoanhThuForReport";
            SqlParameter parD1 = new SqlParameter("@dateStart", SqlDbType.Date);
            SqlParameter parD2 = new SqlParameter("@dateEnd", SqlDbType.Date);

            parD1.Value = dateStart;
            parD2.Value = dateEnd;
            cmd.Parameters.Add(parD1);
            cmd.Parameters.Add(parD2);

            cmd.Connection = Sql_Connection.Instance.sqlCon;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);
            return dataTable;


        }

    }
}
