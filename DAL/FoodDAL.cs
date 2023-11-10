using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DTO;

namespace DAL
{
    public  class FoodDAL
    {
        private FoodDAL() { }   
        private static FoodDAL instance= null;
        public static FoodDAL Instance
        {  get { if (instance == null) instance = new FoodDAL(); return instance; } 
           set { instance = value; } 
        }   

        public List <FoodDTO> getFoodByCateGory (int ma)
        {
            List <FoodDTO> l = new List <FoodDTO>();
            DataTable dataTable = new DataTable();
            string q = "select * from food where idCategory = " + ma;
           dataTable =  DataProvider.Instance.excecuteQuerry(q); ;
            foreach (DataRow row in dataTable.Rows)
            {
                DataRow dr = row;
                FoodDTO f = new FoodDTO(row);
                l.Add(f);

            }

            return l;

        }
        public DataTable hienThiDanhSachFood()
        {
            string q = "select f.id [Mã món ăn], f.name [Tên món ăn], f.price [Giá], c.name [Loại] from food f\r\ninner join FOOD_CATEGORY c\r\non c.id = f.idCategory";
            return DataProvider.Instance.excecuteQuerry(q);



        }
        public bool themRow(FoodDTO a)
        {
            try
            {
                Sql_Connection.Instance.connect();


                Sql_Connection.Instance.openCon();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into food values ( @name, @price, @loai)";

                SqlParameter parname = new SqlParameter("@name", SqlDbType.NVarChar);
                SqlParameter parprice = new SqlParameter("@price", SqlDbType.Float);
                SqlParameter parLoai = new SqlParameter("@loai", SqlDbType.Int);
                parname.Value = a.TenMon;
                parprice.Value =(float) a.Gia;
                parLoai.Value = a.Loai;

                cmd.Parameters.Add(parname);
                cmd.Parameters.Add(parprice);
                cmd.Parameters.Add(parLoai);



                cmd.Connection = Sql_Connection.Instance.sqlCon;


                if (cmd.ExecuteNonQuery() < 0)
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
            }




        }
       public  List <string> danhSachFood()
        {
            string q = "select * from food";
            DataTable ds = DataProvider.Instance.excecuteQuerry(q);

            List<string> l = new List<string>();



            foreach (DataRow dr in ds.Rows)
            {
                l.Add(dr["name"].ToString().Trim());
            }

            return l;


        }
        public bool chinhSuaRow(FoodDTO a)
        {
            try
            {
                // kt naem. status trc khi truyen vao 
                Sql_Connection.Instance.connect();
                Sql_Connection.Instance.openCon();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Food\r\nset \r\n\tname = @name,\r\n\tprice = @price ,\r\n\tidCategory = @idLoai \r\nwhere id = @id";
                cmd.Connection = Sql_Connection.Instance.sqlCon;


                SqlParameter parid = new SqlParameter("@id", SqlDbType.Int);
                SqlParameter parname = new SqlParameter("@name", SqlDbType.NVarChar);
                SqlParameter parprice  = new SqlParameter("@price", SqlDbType.Float);
                SqlParameter parLoai =  new SqlParameter("@idLoai", SqlDbType.Int);
                parid.Value = a.Id;
                parname.Value = a.TenMon;
                parprice.Value =(float) a.Gia;
                parLoai.Value = a.Loai;

                cmd.Parameters.Add(parid);
                cmd.Parameters.Add(parname);
                cmd.Parameters.Add(parprice);
                cmd.Parameters.Add(parLoai);



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

        public string timNameFoodByIDFood (int idFood) {
            DataTable dt = new DataTable();
             dt =  DataProvider.Instance.excecuteQuerry("select * from Food where id = "+ idFood);
            DataRow dataRow = dt.Rows[0];
            return dataRow["name"].ToString().Trim();   
        
        }
        public DataTable TimKiemMonAn(string n)
        {
            DataTable dataTable = new DataTable();
            Sql_Connection.Instance.openCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "TimKiemMonAn";

            SqlParameter p = new SqlParameter("@name", SqlDbType.NVarChar);
            p.Value = n;
            cmd.Parameters.Add(p);

            cmd.Connection = Sql_Connection.Instance.sqlCon;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);

            return dataTable;


        }
        public static bool xoaMonAn( int id)
        {
            try
            {
                Sql_Connection.Instance.openCon();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from food where id = " + id;
                cmd.Connection = Sql_Connection.Instance.sqlCon;
                if (cmd.ExecuteNonQuery() <0)
                return false;
                return true;


            }
            catch
            {
                return false;
            }




        }
    }
}
