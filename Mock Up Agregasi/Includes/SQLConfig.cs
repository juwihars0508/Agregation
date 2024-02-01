using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace Mock_Up_Agregasi.Includes
{
    class SQLConfig
    {
        //private MySqlConnection con = new MySqlConnection("server=localhost;userid=root;pwd=root;database=db_autocoding;sslMode=none");
        //private MySqlConnection con = new MySqlConnection("server=localhost;userid=root;database=db_autocoding;sslMode=none");
        private MySqlCommand cmd;
        private MySqlDataAdapter da;
        public DataTable dt;
        int result;
        usableFunction funct = new usableFunction();

        //variable Data Con String
        public string fileCon_String = @"Data\Con_String.txt";
        public string VCon_String;

        public MySqlConnection con;

        public void Init_Con()
        {
            Read_Con_String();
            con = new MySqlConnection(VCon_String);
        }

        //Read data ConnectionString
        public void Read_Con_String()
        {
            StreamReader file = new StreamReader(fileCon_String);
            string line = file.ReadLine();

            VCon_String = line;
            file.Close();
        }

        public void Execute_CUD(string sql, string msg_false, string msg_true)
        {
            Init_Con();
            try
            {
                con.Open();
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show(msg_true);
                }
                else
                {
                    MessageBox.Show(msg_false);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public void Execute_Query(string sql)
        {
            Init_Con();
            try
            {
                con.Open();
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public void Load_DTG(string sql, DataGridView dtg)
        {
            Init_Con();
            try
            {
                con.Open();
                cmd = new MySqlCommand();
                da = new MySqlDataAdapter();
                dt = new DataTable();


                cmd.Connection = con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);
                dtg.DataSource = dt;


                funct.ResponsiveDtg(dtg);
                dtg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dtg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }

        }

        public void fiil_CBO(string sql, ComboBox cbo)
        {
            Init_Con();

            try
            {
                con.Open();
                cmd = new MySqlCommand();
                da = new MySqlDataAdapter();
                dt = new DataTable();


                cmd.Connection = con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);
                cbo.DataSource = dt;
                cbo.ValueMember = dt.Columns[0].ColumnName;
                cbo.DisplayMember = dt.Columns[0].ColumnName;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }

        }

        public void singleResult(string sql)
        {
            Init_Con();
            try
            {
                con.Open();
                cmd = new MySqlCommand();
                da = new MySqlDataAdapter();
                dt = new DataTable();


                cmd.Connection = con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }
        }

        public void loadReports(string sql)
        {
            Init_Con();
            try
            {
                con.Open();
                cmd = new MySqlCommand();
                da = new MySqlDataAdapter();
                dt = new DataTable();


                cmd.Connection = con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }
        }

        public void autocomplete(string sql, TextBox txt)
        {
            Init_Con();
            try
            {
                con.Open();
                cmd = new MySqlCommand();
                da = new MySqlDataAdapter();
                dt = new DataTable();


                cmd.Connection = con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);
                txt.AutoCompleteCustomSource.Clear();
                txt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txt.AutoCompleteSource = AutoCompleteSource.CustomSource;
                foreach (DataRow r in dt.Rows)
                {
                    txt.AutoCompleteCustomSource.Add(r.Field<string>(0));
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }
        }

        public void autonumber(string sql, TextBox txt)
        {
            Init_Con();
            try
            {
                con.Open();
                cmd = new MySqlCommand();
                da = new MySqlDataAdapter();
                dt = new DataTable();


                cmd.Connection = con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);

                txt.Text = dt.Rows[0].Field<string>(0);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }
        }
        public void update_Autonumber(string id)
        {
            Execute_Query("UPDATE `tblautonumber` SET `END`=`END`+`INCREMENT` WHERE `DESCRIPTION`='" + id + "'");
        }

    }
}
