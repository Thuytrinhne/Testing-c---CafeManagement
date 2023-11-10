using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LoginDAL
    {
        private LoginDAL() { }
        private static LoginDAL instance = null;
        public static LoginDAL Instance
        {


            get { if (instance == null)
                    instance = new LoginDAL();
                return instance; }
            private set { instance = value; }
        }
        public static  string maHoaMatKhau(string password)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] hashData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";
            foreach (byte b in hashData)
            {
                hasPass += b;
            }
            return hasPass;
        }
      
    public static  bool Login(string username,  string password, ref Account a)
        {
            #region ma hoa mat khau 
            string hasPass=maHoaMatKhau(password);

            #endregion
            DataTable t = new DataTable();
            Sql_Connection.Instance.connect();
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from account  where userName = @user and passWord = @pass";

            SqlParameter paruser = new SqlParameter("@user", SqlDbType.VarChar);
            SqlParameter parpassword = new SqlParameter("@pass", SqlDbType.VarChar);
            paruser.Value = username;
            parpassword.Value = hasPass;
            //
            password = hasPass;

            cmd.Parameters.Add(paruser);
            cmd.Parameters.Add(parpassword);

            cmd.Connection = Sql_Connection.Instance.sqlCon;


            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(t);


            if (t.Rows.Count == 0) {

                return false;
                

            }
            DataRow dr = t.Rows[0];
            if (dr["userName"].ToString().Trim() == username && dr["passWord"].ToString().Trim() == password) {
                a.ID = (int)dr["id"];
                a.UserName = dr["userName"].ToString();
                a.DisplayName = dr["displayName"].ToString();
                a.TypeAccount = (int)dr["typeAccount"];


                return true;
            }
            return false;


        }
        public static bool ktCurPass(string username, string password)
        {
            string hasPass = maHoaMatKhau(password);

            DataTable t = new DataTable();
            Sql_Connection.Instance.connect();
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from account  where userName = @user and passWord = @pass";

            SqlParameter paruser = new SqlParameter("@user", SqlDbType.VarChar);
            SqlParameter parpassword = new SqlParameter("@pass", SqlDbType.VarChar);
            paruser.Value = username;
            parpassword.Value = hasPass;
            //
            password = hasPass;

            cmd.Parameters.Add(paruser);
            cmd.Parameters.Add(parpassword);

            cmd.Connection = Sql_Connection.Instance.sqlCon;


            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(t);


            if (t.Rows.Count == 0)
            {

                return false;


            }
            return true;

        }
        public DataTable hienThiDanhSachTaiKhoan()
        {
            DataTable dt = new DataTable();
            string q = "select id [Mã tài khoản], displayName [Tên hiển thị], userName [Username] ,typeAccount [Loại tài khoản] from account";
            dt = DataProvider.Instance.excecuteQuerry(q);
            return dt;


        }
        public static List <string> danhSachUserName()
        {
          string q = "select * from account";
        DataTable ds = DataProvider.Instance.excecuteQuerry(q);

        List<string> l = new List<string>();



            foreach (DataRow dr in ds.Rows)
            {
                l.Add(dr["username"].ToString().Trim());
            }



            return l;

        }

        public static bool themRow(Account d)
        {
            try
            {
               string pass= maHoaMatKhau("0");
                Sql_Connection.Instance.openCon();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into account values (@user,@display,@pass,@type)";

                SqlParameter paruser = new SqlParameter("@user", SqlDbType.VarChar);
                SqlParameter pardisplay= new SqlParameter("@display", SqlDbType.NVarChar);
                SqlParameter parpass = new SqlParameter("@pass", SqlDbType.VarChar);
                SqlParameter partype = new SqlParameter("@type", SqlDbType.Int);

                paruser.Value = d.UserName;
                pardisplay.Value = d.DisplayName;
                parpass.Value = pass;
                partype.Value = d.TypeAccount;

                cmd.Parameters.Add(paruser);
                cmd.Parameters.Add(pardisplay);
                cmd.Parameters.Add(parpass);
                cmd.Parameters.Add(partype);





                cmd.Connection = Sql_Connection.Instance.sqlCon;


                if ((int)cmd.ExecuteNonQuery() < 0)
                {


                    return false;
                }
                {


                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                return false;
            }



        }
        public static bool xoaRow(int ma)
        {
            try
            {
                Sql_Connection.Instance.connect();
                Sql_Connection.Instance.openCon();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from account where id = " + ma;
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
        public static  string timUserNameByIDAccount(int id)
        {
            DataTable dt = new DataTable();
            dt = DataProvider.Instance.excecuteQuerry("select * from account where id = " + id);
            DataRow dataRow = dt.Rows[0];
            return dataRow["username"].ToString().Trim();

        }
        public static bool chinhSuaRow(Account d)
        {
            try
            {
                Sql_Connection.Instance.openCon();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update account \r\nset \r\n\tusername = @name,\r\n\tdisplayname = @dis ,\r\n\ttypeAccount = @type \r\nwhere id = @id";
                cmd.Connection = Sql_Connection.Instance.sqlCon;


                SqlParameter parid = new SqlParameter("@id", SqlDbType.Int);
                SqlParameter parUsername = new SqlParameter("@name", SqlDbType.VarChar);
                SqlParameter parDisplay = new SqlParameter("@dis", SqlDbType.NVarChar);
                SqlParameter parType = new SqlParameter("@type", SqlDbType.Int);

                parid.Value = d.ID;
                parUsername.Value = d.UserName;
                parDisplay.Value = d.DisplayName;
                parType.Value = d.TypeAccount;

                cmd.Parameters.Add(parid);
                cmd.Parameters.Add(parUsername);
                cmd.Parameters.Add(parDisplay);
                cmd.Parameters.Add(parType);



                if (cmd.ExecuteNonQuery() < 0)
                {


                    return false;
                }
                {
                    return true;
                }
            }
            catch { throw new Exception(); }



        }
        public  static DataTable timKiemTaiKhoan(string displayName)
        {
            DataTable dataTable = new DataTable();
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select id [Mã tài khoản], displayName [Tên hiển thị], userName [Username] ,typeAccount [Loại tài khoản] from account where displayName like '%" + displayName + "%'";

            //SqlParameter p = new SqlParameter("@n", SqlDbType.NVarChar);
            //p.Value = n;
            //cmd.Parameters.Add(p);

            cmd.Connection = Sql_Connection.Instance.sqlCon;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);

            return dataTable;

        }
        public static bool DoiPassword(string username, string password)
        {
            string hasPass = maHoaMatKhau(password);
            try
            {
                Sql_Connection.Instance.openCon();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update account \r\nset \r\n\tpassword = @pass\r\nwhere username = @username";
                cmd.Connection = Sql_Connection.Instance.sqlCon;


                SqlParameter parpass = new SqlParameter("@pass", SqlDbType.VarChar);
                SqlParameter parUsername = new SqlParameter("@username", SqlDbType.VarChar);


                parpass.Value = hasPass;
                parUsername.Value = username;

                cmd.Parameters.Add(parUsername);
                cmd.Parameters.Add(parpass);



                if (cmd.ExecuteNonQuery() < 0)
                {


                    return false;
                }
                {
                    return true;
                }
            }
            catch { throw new Exception(); }





        }

    }
}
