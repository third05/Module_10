using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Newtonsoft.Json;

namespace Module_10
{
    class TelegramMessage
    {
        private MainWindow w;

        private TelegramBotClient tClient;
        public ObservableCollection<UserLog> BotMessageLog { get; set; }

        public ObservableCollection<MassageLog> MsgLog { get; set; }

        public ObservableCollection<FilesLog> FileLog { get; set; }

        public TelegramMessage(MainWindow W)
        {
            this.BotMessageLog = new ObservableCollection<UserLog>();

            this.MsgLog = new ObservableCollection<MassageLog>();

            this.w = W;

            FileLog = new ObservableCollection<FilesLog>();

            tClient = new TelegramBotClient(@"2100436570:AAF6Nn9tKvy_VFZvBEF78Hc849W_lH6PkVc");

            DirectoryInfo dInfo = new DirectoryInfo(@"DownLoad");
            if (!dInfo.Exists)                                          //Проверяем существует ли папка DonwLoad куда будут сохранятся файлы
            {
                dInfo.Create();                                         //Если не то создаем её
            }

            tClient.OnMessage += MessageListener;

            tClient.StartReceiving();
        }

        private void MessageListener(object sender, MessageEventArgs e)
        {
            Debug.WriteLine("+++---");



            var msg = e.Message;
            var messageText = e.Message.Text;
            var userPhoto = tClient.GetUserProfilePhotosAsync((msg.Chat.Id));       //Присваеваем фото пользователя в переменную
            string text = $"{DateTime.Now.ToLongTimeString()}, {msg.Chat.FirstName},{msg.Chat.Id}, {msg.Text} Type text: {msg.Type}";
            

            Debug.WriteLine($"{text} TypeMessage: {msg.Type.ToString()}");

            //if (e.Message.Text == null) return;

            #region Код из прошлого модуля
            try
            {


                if (e.Message.Text == @"/start" || msg.Text == @"/старт")               //Если прислали сообщение с командой "/start"
                {
                    var massage = $"Привет,\t{msg.Chat.FirstName}!\n💿 Ты можешь отправить мне файл и в последствии скачать его\n\n📄 Для просмотра всех файлов напиши \"/files\"\n\n⬆️ Чтобы скачать файл напиши \"/download имя_файла\"";
                    tClient.SendTextMessageAsync(msg.Chat.Id, massage);
                }


                else if (messageText != null && messageText.ToLower() == @"/files")     //Если прислали сообщение с командой "/files"
                {
                    string message = "";
                    DirectoryInfo dInfo = new DirectoryInfo(@"DownLoad");
                    foreach (var item in dInfo.GetFiles())                              //Просматриваем все файлы в директори
                    {
                        message += $"{item} объем: {item.Length / 1024} килобайт\n";      //Имя и размер каждого файла добавляем в строку
                    }
                    if (message == "")
                    {
                        tClient.SendTextMessageAsync(msg.Chat.Id, "У меня пока нет файлов");
                    }
                    else
                    {
                        tClient.SendTextMessageAsync(msg.Chat.Id, message);                             //Отправлеям сообщение со всеми файлами которые когда либо скидывали боту
                    }

                }
                else if (messageText != null && messageText.ToLower().StartsWith(@"/download"))     //Если прислали сообщение с командой "/download"
                {
                    char[] example = { ' ' };
                    string[] value = msg.Text.Split(example, StringSplitOptions.RemoveEmptyEntries);            //Разбиваем строку на массив строк

                    if (value.Length > 1)
                    {
                        if (File.Exists($"DownLoad/{value[1]}"))                                                    //Проверяем есть ли файл с название вторго элемента массива
                        {
                            UnloadDocument(tClient, msg.Chat.Id, $"{value[1]}");                                             //Если есть отправляем этот файл собеседнику
                        }
                        else
                        {
                            tClient.SendTextMessageAsync(msg.Chat.Id, "Такого файла у меня нет 🙈");                   //Иначе пишем что такого файла нет
                        }
                    }
                    else
                    {
                        tClient.SendTextMessageAsync(msg.Chat.Id, "Необходимо прислать не только команду но и имя файла 😡");
                    }

                }
                //else if (msg.Text != null)                                                                          //Если сообщение содержит текст
                //{
                //    tClient.SendTextMessageAsync(msg.Chat.Id, messageText, replyToMessageId: msg.MessageId);        //Бот отвечает таким же сообщением

                //}

                //Если прислали документ 
                if (msg.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                {
                    tClient.SendTextMessageAsync(msg.Chat.Id, "Пожалуй я это сохраню, 😉", replyToMessageId: msg.MessageId);         //Бот пишет, что сохранит этот файл
                    DownLoad(tClient, msg.Document.FileId, @"Download\" + msg.Document.FileName);                                              //Вызываем метод для сохранения файла 
                }

                //Если прислали фото
                if (msg.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
                {
                    tClient.SendTextMessageAsync(msg.Chat.Id, "Пожалуй я это сохраню, 😉", replyToMessageId: msg.MessageId);
                    DownLoad(tClient, msg.Photo.LastOrDefault().FileId, @"Download\" + msg.Photo.LastOrDefault().FileId.Substring(text.Length / 10, text.Length / 10) + ".jpg");
                }

                //Если прислали аудио
                if (msg.Type == Telegram.Bot.Types.Enums.MessageType.Audio)
                {
                    tClient.SendTextMessageAsync(msg.Chat.Id, "Пожалуй я это сохраню, 😉", replyToMessageId: msg.MessageId);
                    DownLoad(tClient, msg.Audio.FileId, @"Download\" + msg.Audio.FileName);
                }

                //Если прислали голосовое сообщение
                if (msg.Type == Telegram.Bot.Types.Enums.MessageType.Voice)
                {
                    tClient.SendTextMessageAsync(msg.Chat.Id, "Пожалуй я это сохраню, 😉", replyToMessageId: msg.MessageId);
                    DownLoad(tClient, msg.Voice.FileId, @"Download\" + DateTime.Now.ToShortTimeString() + "Voice.mp3");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
            }

            #endregion

            w.Dispatcher.Invoke(() =>
            {
                MsgLog.Add(new MassageLog(DateTime.Now.ToShortTimeString(), messageText, msg.Chat.Id));
                bool checkId = false;
                for (int i = 0; i < BotMessageLog.Count; i++)
                {
                    if(BotMessageLog[i].Id == msg.Chat.Id)
                    {
                        BotMessageLog.RemoveAt(i);
                        BotMessageLog.Insert(0,
                        new UserLog(DateTime.Now.ToShortTimeString(), messageText, e.Message.Chat.FirstName, e.Message.Chat.Id));
                        checkId = true;
                        break;
                    }
                }
                if (!checkId)
                {
                    BotMessageLog.Add(
                        new UserLog(DateTime.Now.ToShortTimeString(), messageText, e.Message.Chat.FirstName, e.Message.Chat.Id));
                }

                CheckFiles(FileLog);

               
            });

            Console.WriteLine(text);                                                    //Выводим в консоль время получения сообщения, имя и id отправителя, текст сообщения и его тип

           
        }
        /// <summary>
        /// Метод отправки введенного сообщения
        /// </summary>
        /// <param name="text"></param>
        /// <param name="Id"></param>
        public async void SendMessage(long Id, string text)
        {
            if (text == "") return;
            else
            {
                await tClient.SendTextMessageAsync(Id, text);
            }
            
        }
        /// <summary>
        /// Метод для сохранения файла
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="path"></param>
        async void DownLoad(TelegramBotClient tClient, string fileId, string path)
        {
            var file = await tClient.GetFileAsync(fileId);
            using (FileStream fstream = new FileStream(path, FileMode.Create))
            {
                await tClient.DownloadFileAsync(file.FilePath, fstream);
                
            }
        }

        /// <summary>
        /// Метод для отправки файла
        /// </summary>
        /// <param name="ChatID"></param>
        /// <param name="path"></param>
        async void UnloadDocument(TelegramBotClient tClient, long ChatID, string path)
        {
            using (FileStream stream = new FileStream($"DownLoad/" + path, FileMode.Open, FileAccess.Read))
            {
                await tClient.SendDocumentAsync(ChatID, new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream, path));
            }
        }

        /// <summary>
        /// Сохранение полученных сообщений в json файл
        /// </summary>
        /// <param name="name"></param>
        /// <param name="massages"></param>
        public void Serilized(string name, ObservableCollection<MassageLog> massages)
        {
            string json = JsonConvert.SerializeObject(massages);
            File.WriteAllText($"{name}.json", json);
        }

        /// <summary>
        /// Проверка какие файлы прислал пользователь
        /// </summary>
        /// <param name="FileLog"></param>
        public void CheckFiles(ObservableCollection<FilesLog> FileLog)
        {
            DirectoryInfo dInfo = new DirectoryInfo(@"DownLoad");
            FileLog.Clear();
            foreach (var item in dInfo.GetFiles())                              //Просматриваем все файлы в директори
            {
                FileLog.Add(new FilesLog($"{item}"));      //Имя и размер каждого файла добавляем в строку
            }
        }
        

    }
}
