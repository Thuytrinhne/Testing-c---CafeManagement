using DAL;
using DTO;
using Moq;
using NUnit.Framework.Constraints;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace QuanLyQuanCafe.DALTest
{

    public class TableDALTest
   {
        private Mock<DataProvider> _mockDataProvider;

        [SetUp]
        public void Init() {
            _mockDataProvider= new Mock<DataProvider>();
            // mock Singleton
            mockSingleTon();
           
        }

        private void mockSingleTon()
        {
            var instanceField = typeof(DataProvider).GetField("instance", BindingFlags.Static | BindingFlags.NonPublic);
            instanceField.SetValue(null, _mockDataProvider.Object);

        }
        #region themRow
        [Test]
        [TestCase("Bàn 207", "Trống")]        
        public void TableDAL_themRow_ShouldReturnTrueForAddSuccess(string name, string status)
        {

            // setup method
            TableDTO tableDTO = new TableDTO() { Name = name, Status = status };
            string q = "insert into table_food values ( @name, @status)";
            _mockDataProvider.Setup(m => m.executeInsertQuery_Table(q,tableDTO )).Returns(true);

            // call action
            bool actual = TableDAL.Instance.themRow(tableDTO);
            //compare
            Assert.AreEqual(true, actual);

        }
        [Test]
        [TestCase("Bàn 0", "Trống")]
        public void TableDAL_themRow_ShouldReturnFalseForAddFail(string name, string status)
        {

            // setup method
            TableDTO tableDTO = new TableDTO() { Name = name, Status = status };
            string q = "insert into table_food values ( @name, @status)";
            _mockDataProvider.Setup(m => m.executeInsertQuery_Table(q, tableDTO)).Returns(false);
            
            // call action
            bool actual = TableDAL.Instance.themRow(tableDTO);
            //compare
            Assert.AreEqual(false, actual);

        }
        #endregion
        #region danhsachbanan
        [Test]
        public void TableDAL_danhSachBanAn()
        {
             // setup method 
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("id", typeof(int));
            dataTable.Columns.Add("name", typeof(string));
            dataTable.Columns.Add("status", typeof(string));

            // Thêm dữ liệu vào DataTable
            DataRow row1 = dataTable.NewRow();
            row1["id"] = 1;
            row1["name"] = "Bàn 0";
            row1["status"] = "Trống";
            dataTable.Rows.Add(row1);
            DataRow row2 = dataTable.NewRow();
            row2["id"] = 2;
            row2["name"] = "Bàn 1";
            row2["status"] = "Trống";
            dataTable.Rows.Add(row2);

            string q = "select * from table_food";
            _mockDataProvider.Setup(m=>m.excecuteQuerry(q)).Returns(dataTable);

            // call action
            List<string> listTable = TableDAL.Instance.danhSachBanAn();
            //compare
            Assert.AreEqual(2, listTable.Count);
        }
        #endregion
        #region danhSachIDTable
        [Test]
        public void TableDAL_danhSachIDTable()
        {
            // setup method 
            // setup method 
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("id", typeof(int));

            // Thêm dữ liệu vào DataTable
            DataRow row1 = dataTable.NewRow();
            row1["id"] = 1;
            
            dataTable.Rows.Add(row1);
            string q = "select id from table_food";
            _mockDataProvider.Setup(m => m.excecuteQuerry(q)).Returns(dataTable);

            // call action
            List<int> kq = TableDAL.Instance.danhSachIDTable();
            //compare
            Assert.IsNotNull(kq);
            Assert.AreEqual(1,kq.Count);
        }

        #endregion
    }
}
