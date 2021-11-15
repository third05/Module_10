using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Module_10
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TelegramMessage client;
        UserLog userLogs;
        ObservableCollection<MassageLog> messages;
        public MainWindow()
        {
            InitializeComponent();

            client = new TelegramMessage(this);
            messages = new ObservableCollection<MassageLog>();      //Массив где будут храниться сообщения конкретного диалога
            

            logList.ItemsSource = client.BotMessageLog;

            logFileList.ItemsSource = client.FileLog;               //Источник данных для листбокса где отображаются полученные файлы

            logList.SelectionChanged += lBox_SelectionChanged;      //Подписываемся на изменение диалога
        }

        private void lBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userLogs = logList.SelectedItem as UserLog;
            if (userLogs == null) return;
            else
            {
                messages.Clear();
                for (int i = 0; i < client.MsgLog.Count; i++)
                {
                    if (client.MsgLog[i].Id == userLogs.Id)
                    {
                        messages.Add(client.MsgLog[i]);             //Добавляем в массив элементы у которых чат id совпадает с чат id выбранного диалога
                    }
                }
            }
            
            logMessageList.ItemsSource = client.MsgLog;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(userLogs == null)
            {
                MessageBox.Show("Необходимо выбрать чат");
            }
            else
            {
                client.SendMessage(userLogs.Id, txtBox.Text);
                txtBox.Text = "";                                   //После отпраки сообщения очищаем тексбокс
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            btnSend.Opacity = 0.7;
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            btnSend.Opacity = 1;
        }

        private void btnSaveMsg_Click(object sender, RoutedEventArgs e)
        {
            
            if (userLogs == null)
            {
                MessageBox.Show("Необходимо выбрать чат");
            }
            else
            {
                client.Serilized($"{userLogs.FirstName}_{userLogs.Id}", messages);          //вызов метода для сохранения сообщений
            }
        }
    }
}
