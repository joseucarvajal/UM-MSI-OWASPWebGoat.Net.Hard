using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebSite
{
    public partial class BlogCreate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var connection = new SqlConnection();
            try
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                connection.Open();
                var title = txtTitle.Text;
                var contents = txtContents.Text;
                var insertSql = @"
            insert into BlogEntries (Title, Contents, Author, PostedDate) 
            values(@Title, @Contents, @Author, @PostedDate); 
            select top 1 * from blogentries ORDER BY ID DESC; ";
                var insertCommand = new SqlCommand(insertSql, connection);
                var p1 = insertCommand.Parameters.Add("@Title", SqlDbType.NVarChar, 50);
                p1.Value = title;
                var p2 = insertCommand.Parameters.Add("@Contents", SqlDbType.NVarChar,
                contents.Length);
                p2.Value = contents;
                var p3 = insertCommand.Parameters.Add("@Author", SqlDbType.NVarChar, 50);
                p3.Value = User.Identity.Name;
                var p4 = insertCommand.Parameters.Add("@PostedDate", SqlDbType.DateTime);
                p4.Value = DateTime.Now;


                var dataReader = insertCommand.ExecuteReader();
                lblFeedback.Text = "New blog entry was posted.<br />";
                lblReview.Text = "Here is what your blog entry will look like: <br /><br />";
                while (dataReader.Read())
                    lblReview.Text += string.Format("Id:<br />{0}<br />Title:<br />{1}<br />Contents:<br />{2}<br />Author:<br />{3}<br />Posted date:<br />{4}<br />",
                        dataReader[0],
                        dataReader[1],
                        dataReader[2],
                        dataReader[3],
                        dataReader[4]);
                txtContents.Text = "";
                txtTitle.Text = "";
            }
            catch (Exception ex)
            {
                lblFeedback.Text = string.Format("A problem has occured.  Please try again. Error={0}", ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}