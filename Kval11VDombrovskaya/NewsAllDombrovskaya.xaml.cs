using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Kval11VDombrovskaya
{
    /// <summary>
    /// Логика взаимодействия для NewsAll.xaml
    /// </summary>
    public partial class NewsAllDombrovskaya : Window
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable News;
        public NewsAllDombrovskaya()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                //подключено БД
            }
            catch (SqlException)
            {
                MessageBox.Show("Ошибка подключения БД");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * from  News;";
            SqlConnection connection = null;

            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql, connection);
            connection.Open();
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);
            adapter.UpdateCommand = myCommandBuilder.GetUpdateCommand();
            adapter.DeleteCommand = myCommandBuilder.GetDeleteCommand();

            News = new DataTable();
            adapter.Fill(News); //загрузка данных
            AllNews.ItemsSource = News.DefaultView; //привязка к DataGrid
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                adapter.Update(News);
                MessageBox.Show("Таблица отелей успешно сохранена");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.ToString());
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (AllNews.SelectedItem != null)
            {
                string idNews = News.Rows[AllNews.SelectedIndex]["IdNews"].ToString().Trim();
                string sql2 = "DELETE FROM News where IdNews='" + idNews + "';";
                SqlConnection connection = null;

                connection = new SqlConnection(connectionString);

                SqlCommand command = new SqlCommand(sql2, connection);
                connection.Open();
                int num = command.ExecuteNonQuery();
                if (num != 0)
                {
                    MessageBox.Show("Строка успешно удалена");
                }
                else MessageBox.Show("Ошибка удаления");
            }
            else MessageBox.Show("Вы не выбрали строку для удаления");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
