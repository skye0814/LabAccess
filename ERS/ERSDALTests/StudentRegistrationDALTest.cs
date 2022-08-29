using ERSEntity;
using ERSDAL;
using ERSUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ERSDAL.DB;
using System.Data.SqlClient;
using System.Data;

namespace ERSDALTests
{
    [TestClass]
    public class StudentRegistrationDALTest
    {
        [Ignore]
        [TestMethod]
        public void StudentRegistrationDAL_Insert_ReturnsTrue()
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                StudentRegistrationDAL DAL = new StudentRegistrationDAL();
                var expected = new StudentEntity()
                {
                    FirstName = "John",
                    MiddleName = "John",
                    LastName = "John",
                    StudentNumber = "2020-54515-MN-0",
                    CourseID = 1,
                    SectionID = 1,
                    YearID = 1,
                    EmailAddress = "test@gmail.com",
                    UserName = "test",
                    Password = ERSUtil.DefaultValuesUTIL.defaultPasssword
                };
                var result = DAL.Insert(expected);

                Assert.AreEqual(expected, result);
            }
        }
    }
}
