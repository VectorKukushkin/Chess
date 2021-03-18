using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace Chess
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            XmlDocument xDoc = new XmlDocument();
            XmlNode[] boards = new XmlNode[0];
            XmlNode[] positions = new XmlNode[0];
            XmlNode[] movings = new XmlNode[0];
            XmlNode[] specialMoves = new XmlNode[0];
            XmlNode[] chessmen = new XmlNode[0];
            string text = "";
            string s = "";
            using (StreamReader sr = new StreamReader("files.txt", Encoding.Default))
            {
                text = sr.ReadToEnd();
            }
            int i = 0;
            while (i < text.Length)
            {
                if (text[i] != '\n' && text[i] != '\r')
                {
                    s = s + text[i];
                }
                else
                {
                    if (s != "")
                    {
                        xDoc.Load(s);
                        switch (xDoc.DocumentElement.Name)
                        {
                            case "board":
                                Array.Resize(ref boards, boards.Length + 1);
                                boards[boards.Length - 1] = xDoc.DocumentElement;
                                break;
                            case "position":
                                Array.Resize(ref positions, positions.Length + 1);
                                positions[positions.Length - 1] = xDoc.DocumentElement;
                                break;
                            case "moving":
                                Array.Resize(ref movings, movings.Length + 1);
                                movings[movings.Length - 1] = xDoc.DocumentElement;
                                break;
                            case "specialMove":
                                Array.Resize(ref specialMoves, specialMoves.Length + 1);
                                specialMoves[specialMoves.Length - 1] = xDoc.DocumentElement;
                                break;
                            case "chessman":
                                Array.Resize(ref chessmen, chessmen.Length + 1);
                                chessmen[chessmen.Length - 1] = xDoc.DocumentElement;
                                break;
                            default:

                                break;
                        }
                        s = "";
                    }
                }
                i++;
            }
            if (s != "")
            {
                xDoc.Load(s);
                switch (xDoc.DocumentElement.Name)
                {
                    case "board":
                        Array.Resize(ref boards, boards.Length + 1);
                        boards[boards.Length - 1] = xDoc.DocumentElement;
                        break;
                    case "position":
                        Array.Resize(ref positions, positions.Length + 1);
                        positions[positions.Length - 1] = xDoc.DocumentElement;
                        break;
                    case "moving":
                        Array.Resize(ref movings, movings.Length + 1);
                        movings[movings.Length - 1] = xDoc.DocumentElement;
                        break;
                    case "specialMove":
                        Array.Resize(ref specialMoves, specialMoves.Length + 1);
                        specialMoves[specialMoves.Length - 1] = xDoc.DocumentElement;
                        break;
                    case "chessman":
                        Array.Resize(ref chessmen, chessmen.Length + 1);
                        chessmen[chessmen.Length - 1] = xDoc.DocumentElement;
                        break;
                    default:

                        break;
                }
                s = "";
            }
            i = 0;
            while (i < boards.Length)
            {
                Storage.AddBoard(boards[i]);
                i++;
            }
            i = 0;
            while (i < movings.Length)
            {
                Storage.AddMove(movings[i]);
                i++;
            }
            i = 0;
            while (i < specialMoves.Length)
            {
                Storage.AddSpecialMove(specialMoves[i]);
                i++;
            }
            i = 0;
            while (i < chessmen.Length)
            {
                Storage.AddChessman(chessmen[i]);
                i++;
            }
            i = 0;
            while (i < positions.Length)
            {
                Storage.AddPosition(positions[i]);
                i++;
            }
            Storage.LoadSpecialMoves(specialMoves);
            Storage.Promotions = new int[Storage.Chessmen.Length][];
            i = 0;
            int j;
            int k;
            int l;
            int m;
            bool f;
            while (i < Storage.Chessmen.Length)
            {
                Storage.Promotions[i] = new int[Storage.Attributes.Length];
                f = false;
                j = 0;
                while (j < Storage.Chessmen[i].SpecialMoves.Length)
                {
                    m = 0;
                    k = 0;
                    while (k < Storage.SpecialMoves[Storage.Chessmen[i].SpecialMoves[j]].SpecialActions.Length)
                    {
                        l = 0;
                        while (l < Storage.SpecialActions[Storage.SpecialMoves[Storage.Chessmen[i].SpecialMoves[j]].SpecialActions[k]].SquareActions.Length)
                        {
                            if (Storage.SpecialActions[Storage.SpecialMoves[Storage.Chessmen[i].SpecialMoves[j]].SpecialActions[k]].SquareActions[l].Chessman >= 0)
                            {
                                if (!f)
                                {
                                    Array.Resize(ref Storage.PromotionChessmen, Storage.PromotionChessmen.Length + 1);
                                    Storage.PromotionChessmen[Storage.PromotionChessmen.Length - 1] = i;
                                }
                                if (m < Storage.Chessmen[Storage.SpecialActions[Storage.SpecialMoves[Storage.Chessmen[i].SpecialMoves[j]].SpecialActions[k]].SquareActions[l].Chessman].RelativeValue)
                                {
                                    m = Storage.Chessmen[Storage.SpecialActions[Storage.SpecialMoves[Storage.Chessmen[i].SpecialMoves[j]].SpecialActions[k]].SquareActions[l].Chessman].RelativeValue;
                                }
                                f = true;
                            }
                            l++;
                        }
                        k++;
                    }
                    k = 0;
                    while (k < Storage.SpecialMoves[Storage.Chessmen[i].SpecialMoves[j]].Conditions.Length)
                    {
                        l = 0;
                        while (l < Storage.SpecialMoves[Storage.Chessmen[i].SpecialMoves[j]].Conditions[k].Attributes.Length)
                        {
                            Storage.Promotions[i][Storage.SpecialMoves[Storage.Chessmen[i].SpecialMoves[j]].Conditions[k].Attributes[l]] = m;
                            l++;
                        }
                        k++;
                    }
                    j++;
                }
                i++;
            }
            if (Storage.Positions.Length > 0)
            {
                DrawBroad(Storage.Positions[0]);
            }
            GetReport();
            Timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, 100)
            };
            Timer.Tick += new EventHandler((sender, args) =>
            {
                Stabilize();
            });
            //Timer.Start();
        }

        int n = 0;
        int selectedLabel = 0;
        int lastSelectedLabel = 0;
        int Inaccuracy = 5;
        int selectedInaccuracy = 5;
        int MaximumNumber = 3;
        int selectedMaximumNumber = 3;
        bool player1 = true;
        bool player2 = false;
        bool NewMaximumNumber = false;
        bool selectedNewMaximumNumber = false;
        bool startingGame = false;
        bool changePosition = false;
        bool settingsOpening = false;
        DispatcherTimer Timer;

        private void GetReport()
        {
            bool send = false;
            string text = "";
            try
            {
                text = text + Environment.UserName;
                if (!Directory.Exists("C:/Users/" + Environment.UserName + "/AppData/Roaming/VariableChess"))
                {
                    Directory.CreateDirectory("C:/Users/" + Environment.UserName + "/AppData/Roaming/VariableChess");
                }
                if (!File.Exists("C:/Users/" + Environment.UserName + "/AppData/Roaming/VariableChess/settings.dll"))
                {
                    using (StreamWriter sw = new StreamWriter("C:/Users/" + Environment.UserName + "/AppData/Roaming/VariableChess/settings.dll", false, Encoding.Default))
                    {
                        sw.Write("setNumber=false;\r\nmaximumNumber=3;\r\ninaccuracy=5");
                    }
                    send = true;
                }
                else
                {
                    string s = "";
                    using (StreamReader sr = new StreamReader("C:/Users/" + Environment.UserName + "/AppData/Roaming/VariableChess/settings.dll"))
                    {
                        s = sr.ReadToEnd();
                    }
                    int i = 0;
                    bool p = true;
                    string s1 = "";
                    string s2 = "";
                    while (i < s.Length)
                    {
                        if (s[i] == ';' || s[i] == '=' || s[i] == '\r' || s[i] == '\n' || s[i] == ' ')
                        {
                            if (s[i] == ';' || s[i] == '=')
                            {
                                p = !p;
                                if (s[i] == ';')
                                {
                                    switch (s1)
                                    {
                                        case "setNumber":
                                            NewMaximumNumber = Convert.ToBoolean(s2);
                                            selectedNewMaximumNumber = Convert.ToBoolean(s2);
                                            break;
                                        case "maximumNumber":
                                            MaximumNumber = Convert.ToInt32(s2);
                                            selectedMaximumNumber = Convert.ToInt32(s2);
                                            break;
                                        case "inaccuracy":
                                            Inaccuracy = Convert.ToInt32(s2);
                                            selectedInaccuracy = Convert.ToInt32(s2);
                                            break;
                                        default:

                                            break;
                                    }
                                    s1 = "";
                                    s2 = "";
                                }
                            }
                        }
                        else
                        {
                            if (p)
                            {
                                s1 = s1 + s[i];
                            }
                            else
                            {
                                s2 = s2 + s[i];
                            }
                        }
                        i++;
                    }
                    switch (s1)
                    {
                        case "setNumber":
                            NewMaximumNumber = Convert.ToBoolean(s2);
                            selectedNewMaximumNumber = Convert.ToBoolean(s2);
                            break;
                        case "maximumNumber":
                            MaximumNumber = Convert.ToInt32(s2);
                            selectedMaximumNumber = Convert.ToInt32(s2);
                            break;
                        case "inaccuracy":
                            Inaccuracy = Convert.ToInt32(s2);
                            selectedInaccuracy = Convert.ToInt32(s2);
                            break;
                        default:

                            break;
                    }
                }
            }
            catch
            {

            }
            try
            {
                MailAddress from = new MailAddress("x.denis.feofanov@yandex.ru", "Денис Феофанов");
                // кому отправляем
                string myAdress = "chess-score@yandex.com";
                //password = "chestniy";
                MailAddress to = new MailAddress(myAdress);
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to)
                {
                    // тема письма
                    Subject = "Пользователь " + Environment.UserName + " скачал Шахматы 1.0!",
                    // текст письма
                    Body = "Пользователь " + Environment.UserName + " запустил Шахматы 1.0 в первый раз! Теперь мы будем следить за его достижениями!"
                };
                // письмо представляет код html
                // m.IsBodyHtml = true;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587)
                {
                    // логин и пароль
                    Credentials = new System.Net.NetworkCredential("x.denis.feofanov@yandex.ru", "forgotthispassword"),
                    EnableSsl = true
                };
                if (send)
                {
                    smtp.Send(m);
                }
            }
            catch
            {

            }
        }

        private void DrawBroad(Position position)
        {
            Grid.Width = 50 * position.Squares.Length;
            Grid.Height = 50 * position.Squares[0].Length;
            Board.Children.Clear();
            int i = 0;
            int j;
            while (i < position.Squares.Length)
            {
                j = 0;
                while (j < position.Squares[i].Length)
                {
                    int x0 = i;
                    int y0 = j;
                    Rectangle rectangle = new Rectangle()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(i * 50, (position.Squares[i].Length - j - 1) * 50, 0, 0),
                        Width = 50,
                        Height = 50,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Fill = Brushes.White
                    };
                    if (i % 2 == j % 2)
                    {
                        rectangle.Fill = Brushes.Gray;
                    }
                    Board.Children.Add(rectangle);
                    if (position.Squares[i][j].Team > 0)
                    {
                        Image image = new Image()
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Margin = new Thickness(i * 50, (position.Squares[i].Length - j - 1) * 50, 0, 0),
                            Width = 50,
                            Height = 50,
                            Stretch = Stretch.Fill
                        };
                        if (position.Squares[i][j].Team == 1)
                        {
                            image.Source = Storage.Chessmen[position.Squares[i][j].Chessman].ImageWhite;
                        }
                        else
                        {
                            image.Source = Storage.Chessmen[position.Squares[i][j].Chessman].ImageBlack;
                        }
                        Board.Children.Add(image);
                    }
                    j++;
                }
                i++;
            }
        }

        private void Stabilize()
        {
            double m = 0;
            double k = 0;
            double w = 0;
            double h = 0;
            if (Grid.Width > 0 && Grid.Height > 0)
            {
                m = Canvas.ActualWidth / Grid.Width * 0.65;
                if (m > Canvas.ActualHeight / Grid.Height)
                {
                    m = Canvas.ActualHeight / Grid.Height;
                }
                GridScale.ScaleX = m;
                GridScale.ScaleY = m;
                w = (0.65 * Canvas.ActualWidth - m * Grid.Width) / 2;
                h = (Canvas.ActualHeight - m * Grid.Height) / 2;
                Grid.Margin = new Thickness(w, h, 0, 0);
            }
            ButtonPanel.Margin = new Thickness(2 * w + m * Grid.Width, 0, 0, 0);
            double bpw = Canvas.ActualWidth - (2 * w + m * Grid.Width);
            double bph = Canvas.ActualHeight;
            double cppw = Canvas.ActualWidth - ButtonPanel.Margin.Left;
            double cpph = Canvas.ActualHeight - 6 * bph / 21;
            m = bpw / 200;
            if (m > bph / 150)
            {
                m = bph / 150;
            }
            k = cppw / 200;
            if (k > cpph / 150)
            {
                k = cpph / 150;
            }
            if (Storage.Positions.Length > 0)
            {
                ChangePositionButton.IsEnabled = true;
            }
            else
            {
                ChangePositionButton.IsEnabled = false;
            }
            StartGameButton.Margin = new Thickness(bpw / 8, bph / 21, bpw / 8, 16 * bph / 21);
            ChangePositionButton.Margin = new Thickness(bpw / 8, 6 * bph / 21, bpw / 8, 11 * bph / 21);
            SettingsButton.Margin = new Thickness(bpw / 8, 11 * bph / 21, bpw / 8, 6 * bph / 21);
            ExitButton.Margin = new Thickness(bpw / 8, 16 * bph / 21, bpw / 8, bph / 21);
            if (m > 0)
            {
                StartGameText.FontSize = 15 * m;
                ChangePositionText.FontSize = 15 * m;
                SettingsText.FontSize = 15 * m;
                ExitText.FontSize = 15 * m;
            }
            if (k > 0)
            {
                ChangePositionOKText.FontSize = 10 * k;
                ChangePositionCancelText.FontSize = 10 * k;
            }
            if (changePosition || settingsOpening || startingGame)
            {
                ChangePositionPanel.Margin = new Thickness(ButtonPanel.Margin.Left, 0, 0, 6 * bph / 21);
                SettingsDrawText.Margin = new Thickness(0, 0, cppw, cpph);
                SettingsDrawTextEx.Margin = new Thickness(0, 0, cppw, cpph);
                SettingsDrawTextBox.Margin = new Thickness(0, 0, cppw, cpph);
                SettingsDrawCheckBox.Margin = new Thickness(0, 0, cppw, cpph);
                SettingsInaccuracyText.Margin = new Thickness(0, 0, cppw, cpph);
                SettingsInaccuracyTextBox.Margin = new Thickness(0, 0, cppw, cpph);
                Player1Text.Margin = new Thickness(0, 0, cppw, cpph);
                Player2Text.Margin = new Thickness(0, 0, cppw, cpph);
                Player1Hum.Margin = new Thickness(0, 0, cppw, cpph);
                Player1Comp.Margin = new Thickness(0, 0, cppw, cpph);
                Player2Hum.Margin = new Thickness(0, 0, cppw, cpph);
                Player2Comp.Margin = new Thickness(0, 0, cppw, cpph);
                ChangePositionOKButton.Margin = new Thickness(cppw / 8, 7 * cpph / 8, 4.25 * cppw / 8, 0);
                ChangePositionCancelButton.Margin = new Thickness(4.25 * cppw / 8, 7 * cpph / 8, cppw / 8, 0);
                ChangePositionGrid.Children.Clear();
                if (changePosition)
                {
                    ChangePositionScroll.Margin = new Thickness(cppw / 8, 0.5 * cpph / 8, cppw / 8, 1.5 * cpph / 8);
                    ChangePositionOKText.Text = "Выбрать";
                    int i = 0;
                    while (i < Storage.Positions.Length)
                    {
                        int a = i;
                        Label label = new Label()
                        {
                            Content = Storage.Positions[i].PositionName,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Margin = new Thickness(0, i * 20 * k, 0, 0),
                            Width = 200 * k,
                            Height = 20 * k
                        };
                        if (k > 0)
                        {
                            label.FontSize = 10 * k;
                        }
                        if (i == selectedLabel)
                        {
                            label.FontWeight = FontWeights.Bold;
                        }
                        label.MouseDown += new MouseButtonEventHandler((sender, args) =>
                        {
                            selectedLabel = a;
                            DrawBroad(Storage.Positions[a]);
                            Stabilize();
                        });
                        ChangePositionGrid.Children.Add(label);
                        i++;
                    }
                }
                else
                {
                    ChangePositionScroll.Margin = new Thickness(0, 0, cppw, cpph);
                    if (settingsOpening)
                    {
                        if (selectedNewMaximumNumber)
                        {
                            SettingsDrawTextBox.IsEnabled = false;
                            SettingsDrawCheckBox.IsChecked = true;
                        }
                        else
                        {
                            SettingsDrawTextBox.IsEnabled = true;
                            SettingsDrawCheckBox.IsChecked = false;
                        }
                        SettingsDrawText.Margin = new Thickness(cppw / 8, 0.5 * cpph / 8, cppw / 8, 6.8 * cpph / 8);
                        SettingsDrawTextEx.Margin = new Thickness(4 * cppw / 8, 1.7 * cpph / 8, 2 * cppw / 8, 5.7 * cpph / 8);
                        SettingsDrawTextBox.Margin = new Thickness(2.5 * cppw / 8, 1.65 * cpph / 8, 4.5 * cppw / 8, 5.65 * cpph / 8);
                        SettingsDrawCheckBox.Margin = new Thickness(1.7 * cppw / 8, 2.7 * cpph / 8, 1.5 * cppw / 8, 3.7 * cpph / 8);
                        SettingsInaccuracyText.Margin = new Thickness(cppw / 8, 4.8 * cpph / 8, cppw / 8, 2.5 * cpph / 8);
                        SettingsInaccuracyTextBox.Margin = new Thickness(3.7 * cppw / 8, 4.65 * cpph / 8, 3.3 * cppw / 8, 2.65 * cpph / 8);
                        ChangePositionOKText.Text = "Подтвердить";
                        if (k > 0)
                        {
                            SettingsDrawText.FontSize = 10 * k;
                            SettingsDrawTextEx.FontSize = 10 * k;
                            SettingsDrawTextBox.FontSize = 10 * k;
                            SettingsInaccuracyText.FontSize = 10 * k;
                            SettingsInaccuracyTextBox.FontSize = 10 * k;
                            SettingsDrawCheckBoxText.FontSize = 7.5 * k;
                        }
                        SettingsDrawTextBox.Text = selectedMaximumNumber.ToString();
                        SettingsInaccuracyTextBox.Text = selectedInaccuracy.ToString();
                        if (Convert.ToInt32(SettingsDrawTextBox.Text) > 10)
                        {
                            if (SettingsDrawTextBox.Text[SettingsDrawTextBox.Text.Length - 2] == 49)
                            {
                                SettingsDrawTextEx.Text = "раз";
                            }
                            else
                            {
                                if (SettingsDrawTextBox.Text[SettingsDrawTextBox.Text.Length - 1] >= 50 && SettingsDrawTextBox.Text[SettingsDrawTextBox.Text.Length - 1] <= 52)
                                {
                                    SettingsDrawTextEx.Text = "раза";
                                }
                                else
                                {
                                    SettingsDrawTextEx.Text = "раз";
                                }
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(SettingsDrawTextBox.Text) >= 2 && Convert.ToInt32(SettingsDrawTextBox.Text) <= 4)
                            {
                                SettingsDrawTextEx.Text = "раза";
                            }
                            else
                            {
                                SettingsDrawTextEx.Text = "раз";
                            }
                        }
                    }
                    else
                    {
                        ChangePositionOKText.Text = "Играть";
                        if (player1)
                        {
                            Player1Hum.IsChecked = true;
                            Player1Comp.IsChecked = false;
                        }
                        else
                        {
                            Player1Hum.IsChecked = false;
                            Player1Comp.IsChecked = true;
                        }
                        if (player2)
                        {
                            Player2Hum.IsChecked = true;
                            Player2Comp.IsChecked = false;
                        }
                        else
                        {
                            Player2Hum.IsChecked = false;
                            Player2Comp.IsChecked = true;
                        }
                        Player1Text.Margin = new Thickness(cppw / 8, 1 * cpph / 8, cppw / 8, 6.3 * cpph / 8);
                        Player2Text.Margin = new Thickness(cppw / 8, 2.5 * cpph / 8, cppw / 8, 4.8 * cpph / 8);
                        Player1Hum.Margin = new Thickness(1.5 * cppw / 8, 1.7 * cpph / 8, 3.5 * cppw / 8, 5.6 * cpph / 8);
                        Player1Comp.Margin = new Thickness(3.8 * cppw / 8, 1.7 * cpph / 8, 1.2 * cppw / 8, 5.6 * cpph / 8);
                        Player2Hum.Margin = new Thickness(1.5 * cppw / 8, 3.2 * cpph / 8, 3.5 * cppw / 8, 4.1 * cpph / 8);
                        Player2Comp.Margin = new Thickness(3.8 * cppw / 8, 3.2 * cpph / 8, 1.2 * cppw / 8, 4.1 * cpph / 8);
                        if (k > 0)
                        {
                            SettingsDrawText.FontSize = 10 * k;
                            SettingsDrawTextEx.FontSize = 10 * k;
                            SettingsDrawTextBox.FontSize = 10 * k;
                            Player1Text.FontSize = 10 * k;
                            Player2Text.FontSize = 10 * k;
                            Player1Hum.FontSize = 10 * k;
                            Player1Comp.FontSize = 10 * k;
                            Player2Hum.FontSize = 10 * k;
                            Player2Comp.FontSize = 10 * k;
                        }
                        if (NewMaximumNumber)
                        {
                            SettingsDrawTextBox.IsEnabled = true;
                            SettingsDrawText.Margin = new Thickness(cppw / 8, 4.5 * cpph / 8, cppw / 8, 2.8 * cpph / 8);
                            SettingsDrawTextEx.Margin = new Thickness(4 * cppw / 8, 5.7 * cpph / 8, 2 * cppw / 8, 1.7 * cpph / 8);
                            SettingsDrawTextBox.Margin = new Thickness(2.5 * cppw / 8, 5.65 * cpph / 8, 4.5 * cppw / 8, 1.65 * cpph / 8);
                            SettingsDrawTextBox.Text = selectedMaximumNumber.ToString();
                            if (Convert.ToInt32(SettingsDrawTextBox.Text) > 10)
                            {
                                if (SettingsDrawTextBox.Text[SettingsDrawTextBox.Text.Length - 2] == 49)
                                {
                                    SettingsDrawTextEx.Text = "раз";
                                }
                                else
                                {
                                    if (SettingsDrawTextBox.Text[SettingsDrawTextBox.Text.Length - 1] >= 50 && SettingsDrawTextBox.Text[SettingsDrawTextBox.Text.Length - 1] <= 52)
                                    {
                                        SettingsDrawTextEx.Text = "раза";
                                    }
                                    else
                                    {
                                        SettingsDrawTextEx.Text = "раз";
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(SettingsDrawTextBox.Text) >= 2 && Convert.ToInt32(SettingsDrawTextBox.Text) <= 4)
                                {
                                    SettingsDrawTextEx.Text = "раза";
                                }
                                else
                                {
                                    SettingsDrawTextEx.Text = "раз";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ChangePositionPanel.Margin = new Thickness(0, 0, Canvas.ActualWidth, Canvas.ActualHeight);
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            Stabilize();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Stabilize();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Stabilize();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (Storage.Positions.Length > 0)
            {
                startingGame = true;
                SettingsDrawTextBox.Text = "3";
                Stabilize();
            }
            else
            {
                MessageBox.Show("Нет доступных позиций!");
            }
        }

        private void ChangePositionButton_Click(object sender, RoutedEventArgs e)
        {
            changePosition = true;
            Stabilize();
        }

        private void ChangePositionOKButton_Click(object sender, RoutedEventArgs e)
        {
            if (changePosition)
            {
                changePosition = false;
                lastSelectedLabel = selectedLabel;
            }
            if (settingsOpening)
            {
                settingsOpening = false;
                Inaccuracy = selectedInaccuracy;
                MaximumNumber = selectedMaximumNumber;
                NewMaximumNumber = selectedNewMaximumNumber;
                if (File.Exists("C:/Users/" + Environment.UserName + "/AppData/Roaming/VariableChess/settings.dll"))
                {
                    using (StreamWriter sw = new StreamWriter("C:/Users/" + Environment.UserName + "/AppData/Roaming/VariableChess/settings.dll", false, Encoding.Default))
                    {
                        sw.Write("setNumber=" + NewMaximumNumber.ToString() + ";\r\nmaximumNumber=" + MaximumNumber.ToString() + ";\r\ninaccuracy=" + Inaccuracy.ToString() + "");
                    }
                }
            }
            if (startingGame)
            {
                n++;
                Array.Resize(ref Storage.PreviousPositions, n);
                Storage.PreviousPositions[n - 1] = new Position[0];
                GameWindow gameWindow = new GameWindow(this, n - 1, selectedLabel, selectedMaximumNumber, Inaccuracy, player1, player2);
                Hide();
                gameWindow.ShowDialog();
            }
            Stabilize();
        }

        private void ChangePositionCancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (changePosition)
            {
                changePosition = false;
                selectedLabel = lastSelectedLabel;
                DrawBroad(Storage.Positions[selectedLabel]);
            }
            if (settingsOpening)
            {
                settingsOpening = false;
                selectedInaccuracy = Inaccuracy;
                selectedMaximumNumber = MaximumNumber;
                selectedNewMaximumNumber = NewMaximumNumber;
            }
            if (startingGame)
            {
                startingGame = false;
                selectedMaximumNumber = MaximumNumber;
            }
            Stabilize();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            settingsOpening = true;
            Stabilize();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SettingsDrawTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = "";
            int i = 0;
            bool f = false;
            while (i < SettingsDrawTextBox.Text.Length)
            {
                if (!f)
                {
                    if (SettingsDrawTextBox.Text[i] == 48)
                    {
                        i++;
                    }
                    else
                    {
                        f = true;
                    }
                }
                else
                {
                    if (SettingsDrawTextBox.Text[i] >= 48 && SettingsDrawTextBox.Text[i] <= 57)
                    {
                        s = s + SettingsDrawTextBox.Text[i];
                    }
                    i++;
                }
            }
            if (s != "")
            {
                SettingsDrawTextBox.Text = s;
            }
            else
            {
                SettingsDrawTextBox.Text = "0";
            }
            if (Convert.ToInt32(SettingsDrawTextBox.Text) < 2)
            {
                SettingsDrawTextBox.Text = "0";
            }
            if (Convert.ToInt32(SettingsDrawTextBox.Text) > 999)
            {
                SettingsDrawTextBox.Text = "999";
            }
            if (Convert.ToInt32(SettingsDrawTextBox.Text) > 10)
            {
                if (SettingsDrawTextBox.Text[SettingsDrawTextBox.Text.Length - 2] == 49)
                {
                    SettingsDrawTextEx.Text = "раз";
                }
                else
                {
                    if (SettingsDrawTextBox.Text[SettingsDrawTextBox.Text.Length - 1] >= 50 && SettingsDrawTextBox.Text[SettingsDrawTextBox.Text.Length - 1] <= 52)
                    {
                        SettingsDrawTextEx.Text = "раза";
                    }
                    else
                    {
                        SettingsDrawTextEx.Text = "раз";
                    }
                }
            }
            else
            {
                if (Convert.ToInt32(SettingsDrawTextBox.Text) >= 2 && Convert.ToInt32(SettingsDrawTextBox.Text) <= 4)
                {
                    SettingsDrawTextEx.Text = "раза";
                }
                else
                {
                    SettingsDrawTextEx.Text = "раз";
                }
            }
            selectedMaximumNumber = Convert.ToInt32(SettingsDrawTextBox.Text);
        }

        private void SettingsInaccuracyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = "";
            int i = 0;
            bool f = false;
            while (i < SettingsInaccuracyTextBox.Text.Length)
            {
                if (!f)
                {
                    if (SettingsInaccuracyTextBox.Text[i] == 48)
                    {
                        i++;
                    }
                    else
                    {
                        f = true;
                    }
                }
                else
                {
                    if (SettingsInaccuracyTextBox.Text[i] >= 48 && SettingsInaccuracyTextBox.Text[i] <= 57)
                    {
                        s = s + SettingsInaccuracyTextBox.Text[i];
                    }
                    i++;
                }
            }
            if (s != "")
            {
                SettingsInaccuracyTextBox.Text = s;
            }
            else
            {
                SettingsInaccuracyTextBox.Text = "0";
            }
            if (Convert.ToInt32(SettingsInaccuracyTextBox.Text) < 0)
            {
                SettingsInaccuracyTextBox.Text = "0";
            }
            if (Convert.ToInt32(SettingsInaccuracyTextBox.Text) > 999)
            {
                SettingsInaccuracyTextBox.Text = "999";
            }
            selectedInaccuracy = Convert.ToInt32(SettingsInaccuracyTextBox.Text);
        }

        private void SettingsDrawCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SettingsDrawTextBox.IsEnabled = false;
            selectedNewMaximumNumber = true;
        }

        private void SettingsDrawCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsDrawTextBox.IsEnabled = true;
            selectedNewMaximumNumber = false;
        }

        private void Player1Hum_Checked(object sender, RoutedEventArgs e)
        {
            player1 = true;
        }

        private void Player1Comp_Checked(object sender, RoutedEventArgs e)
        {
            player1 = false;
        }

        private void Player2Hum_Checked(object sender, RoutedEventArgs e)
        {
            player2 = true;
        }

        private void Player2Comp_Checked(object sender, RoutedEventArgs e)
        {
            player2 = false;
        }
    }
}
