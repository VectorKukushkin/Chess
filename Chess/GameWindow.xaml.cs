using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace Chess
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow(MainWindow mainWindow, int number, int positionNumber, int maximumNumber, int inaccuracy, bool player1, bool player2)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            WindowState = MainWindow.WindowState;
            Width = MainWindow.Width;
            Height = MainWindow.Height;
            Number = number;
            position = new Position(Storage.Positions[positionNumber])
            {
                Main = true,
                Number = number,
                Maximum = maximumNumber,
                Inaccuracy = inaccuracy
            };
            endgame = false;
            Grid.Width = 50 * position.Squares.Length;
            Grid.Height = 50 * position.Squares[0].Length;
            Array.Resize(ref Storage.PreviousPositions[number], Storage.PreviousPositions[number].Length + 1);
            Storage.PreviousPositions[number][Storage.PreviousPositions[number].Length - 1] = new Position(position)
            {
                Count = 1
            };
            Window.Title = Storage.Positions[positionNumber].PositionName;
            DrawBoard(position);
            DrawPosition(position);
            Stabilize();
            Player1 = player1;
            Player2 = player2;
            if (!((Player1 && !Player2) || (Player2 && !Player1)))
            {
                DrawButton.IsEnabled = false;
                SurrenderButton.IsEnabled = false;
            }
            Timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, 100)
            };
            Timer.Tick += new EventHandler((sender, args) =>
            {
                if (position.Team)
                {
                    if (!Player1)
                    {
                        MakeMove();
                    }
                }
                else
                {
                    if (!Player2)
                    {
                        MakeMove();
                    }
                }
                Stabilize();
            });
            Timer.Start();
        }

        private MainWindow MainWindow;
        public int Number;
        public int Intelligence = 1;
        public bool Player1;
        public bool Player2;
        Position position;
        Move move;
        Move[] pmoves = new Move[0];
        bool rotation;
        bool prom;
        bool endgame;
        int[] p;
        int pr = -2;
        int n = 1;
        int x = -1;
        int y = -1;
        int z = 0;
        int c = 0;
        Rectangle[][] Rectangles;
        DispatcherTimer Timer;

        private void DrawBoard(Board board)
        {
            Board.Children.Clear();
            int i = 0;
            int j;
            Rectangles = new Rectangle[board.Squares.Length][];
            while (i < board.Squares.Length)
            {
                Rectangles[i] = new Rectangle[board.Squares[i].Length];
                j = 0;
                while (j < board.Squares[i].Length)
                {
                    int x0 = i;
                    int y0 = j;
                    Rectangles[i][j] = new Rectangle()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(i * 50, (board.Squares[i].Length - j - 1) * 50, 0, 0),
                        Width = 50,
                        Height = 50,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Fill = Brushes.White
                    };
                    if (rotation)
                    {
                        Rectangles[i][j].Margin = new Thickness((position.Squares.Length - i - 1) * 50, j * 50, 0, 0);
                    }
                    if (i % 2 == j % 2)
                    {
                        Rectangles[i][j].Fill = Brushes.Gray;
                    }
                    Rectangles[i][j].MouseDown += new MouseButtonEventHandler((sender, args) =>
                    {
                        if (x >= 0 && y >= 0)
                        {
                            MakeMove(x0, y0, false);
                        }
                    });
                    Board.Children.Add(Rectangles[i][j]);
                    j++;
                }
                i++;
            }
        }

        private void DrawPosition(Position Position)
        {
            Pieces.Children.Clear();
            int i = 0;
            int j;
            while (i < Position.Squares.Length)
            {
                j = 0;
                while (j < Position.Squares[i].Length)
                {
                    int x0 = i;
                    int y0 = j;
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
                        if (rotation)
                        {
                            image.Margin = new Thickness((position.Squares.Length - i - 1) * 50, j * 50, 0, 0);
                        }
                        if (position.Squares[i][j].Team == 1)
                        {
                            image.Source = Storage.Chessmen[position.Squares[i][j].Chessman].ImageWhite;
                        }
                        else
                        {
                            image.Source = Storage.Chessmen[position.Squares[i][j].Chessman].ImageBlack;
                        }
                        image.MouseDown += new MouseButtonEventHandler((sender, args) =>
                        {
                            if (x >= 0 && y >= 0)
                            {
                                MakeMove(x0, y0, true);
                            }
                            else
                            {
                                c = position.Squares[x0][y0].Chessman;
                                x = x0;
                                y = y0;
                                Rectangles[x][y].StrokeThickness = 5;
                                Rectangles[x][y].Stroke = Brushes.Green;
                            }
                        });
                        Pieces.Children.Add(image);
                    }
                    j++;
                }
                i++;
            }
        }

        private void MakeMove(int x0, int y0, bool o)
        {
            int k = 0;
            if (pmoves.Length == 0)
            {
                z = 0;
                Move[] moves = position.GetAllPossibleMoves();
                while (k < moves.Length)
                {
                    if (moves[k].X1 == x && moves[k].Y1 == y && moves[k].X2 == x0 && moves[k].Y2 == y0)
                    {
                        Array.Resize(ref pmoves, pmoves.Length + 1);
                        pmoves[pmoves.Length - 1] = new Move(moves[k]);
                    }
                    k++;
                }
            }
            if (pmoves.Length > 0 && !endgame)
            {
                Move[] nmoves;
                bool s;
                int[] c = new int[0];
                int i = 0;
                int m = 0;
                if (pmoves.Length > 1)
                {
                    while (i < pmoves.Length)
                    {
                        if (pmoves[i].Special >= 0)
                        {
                            if (m < Storage.SpecialActions[pmoves[i].Special].SquareActions.Length)
                            {
                                m = Storage.SpecialActions[pmoves[i].Special].SquareActions.Length;
                            }
                        }
                        i++;
                    }
                }
                if (pr >= -1)
                {
                    nmoves = new Move[pmoves.Length];
                    i = 0;
                    while (i < pmoves.Length)
                    {
                        nmoves[i] = new Move(pmoves[i]);
                        i++;
                    }
                    pmoves = new Move[0];
                    if (pr >= 0)
                    {
                        i = 0;
                        while (i < nmoves.Length)
                        {
                            if (z < Storage.SpecialActions[nmoves[i].Special].SquareActions.Length)
                            {
                                if (Storage.SpecialActions[nmoves[i].Special].SquareActions[z].Chessman == pr)
                                {
                                    Array.Resize(ref pmoves, pmoves.Length + 1);
                                    pmoves[pmoves.Length - 1] = new Move(nmoves[i]);
                                }
                            }
                            i++;
                        }
                    }
                    else
                    {
                        i = 0;
                        while (i < nmoves.Length)
                        {
                            if (z < Storage.SpecialActions[nmoves[i].Special].SquareActions.Length)
                            {
                                if (Storage.SpecialActions[nmoves[i].Special].SquareActions[z].Chessman == -1)
                                {
                                    Array.Resize(ref pmoves, pmoves.Length + 1);
                                    pmoves[pmoves.Length - 1] = new Move(nmoves[i]);
                                }
                            }
                            else
                            {
                                Array.Resize(ref pmoves, pmoves.Length + 1);
                                pmoves[pmoves.Length - 1] = new Move(nmoves[i]);
                            }
                            i++;
                        }
                    }
                    z++;
                    pr = -2;
                    PromotionGrid.Children.Clear();
                }
                if (pmoves.Length > 1 && z < m)
                {
                    prom = true;
                    Curtain.Width = Window.ActualWidth;
                    Curtain.Height = Window.ActualHeight;
                    p = new int[0];
                    i = 0;
                    while (i < pmoves.Length)
                    {
                        if (pmoves[i].Special >= 0)
                        {
                            if (z < Storage.SpecialActions[pmoves[i].Special].SquareActions.Length)
                            {
                                k = 0;
                                s = false;
                                while (k < p.Length)
                                {
                                    if (Storage.SpecialActions[pmoves[i].Special].SquareActions[z].Chessman == p[k])
                                    {
                                        s = true;
                                    }
                                    k++;
                                }
                                if (!s)
                                {
                                    Array.Resize(ref p, p.Length + 1);
                                    p[p.Length - 1] = Storage.SpecialActions[pmoves[i].Special].SquareActions[z].Chessman;
                                }
                            }
                        }
                        i++;
                    }
                    Stabilize();
                }
                else
                {
                    prom = false;
                    Curtain.Width = 0;
                    Curtain.Height = 0;
                    n++;
                    move = new Move(pmoves[0]);
                    position = new Position(position, move)
                    {
                        Main = true
                    };
                    bool a;
                    int f = -1;
                    i = 0;
                    int j;
                    while (i < Storage.PreviousPositions[Number].Length && f < 0)
                    {
                        if (position.RelativeValue == Storage.PreviousPositions[Number][i].RelativeValue)
                        {
                            a = true;
                            j = 0;
                            while (j < position.Squares.Length && a)
                            {
                                k = 0;
                                while (k < position.Squares[j].Length && a)
                                {
                                    if (position.Squares[j][k].Team != Storage.PreviousPositions[Number][i].Squares[j][k].Team || position.Squares[j][k].Chessman != Storage.PreviousPositions[Number][i].Squares[j][k].Chessman)
                                    {
                                        a = false;
                                    }
                                    k++;
                                }
                                j++;
                            }
                            if (a)
                            {
                                f = i;
                            }
                        }
                        i++;
                    }
                    if (f >= 0)
                    {
                        Storage.PreviousPositions[Number][f].Count++;
                    }
                    else
                    {
                        Array.Resize(ref Storage.PreviousPositions[Number], Storage.PreviousPositions[Number].Length + 1);
                        Storage.PreviousPositions[Number][Storage.PreviousPositions[Number].Length - 1] = new Position(position)
                        {
                            Count = 1
                        };
                    }
                    Rectangles[x][y].StrokeThickness = 1;
                    Rectangles[x][y].Stroke = Brushes.Black;
                    x = -1;
                    y = -1;
                    EndMoving();
                    z = 0;
                    pmoves = new Move[0];
                    GetMoves();
                }
            }
            else
            {
                if (o)
                {
                    Rectangles[x][y].StrokeThickness = 1;
                    Rectangles[x][y].Stroke = Brushes.Black;
                    c = position.Squares[x0][y0].Chessman;
                    x = x0;
                    y = y0;
                    Rectangles[x][y].StrokeThickness = 5;
                    Rectangles[x][y].Stroke = Brushes.Green;
                }
            }
        }

        private void MakeMove()
        {
            if (!endgame)
            {
                move = position.GetTheBestMove(Intelligence);
                if (move != null)
                {
                    prom = false;
                    Curtain.Width = 0;
                    Curtain.Height = 0;
                    n++;
                    c = position.Squares[move.X1][move.Y1].Chessman;
                    position = new Position(position, move)
                    {
                        Main = true
                    };
                    bool a;
                    int f = -1;
                    int i = 0;
                    int j;
                    int k;
                    while (i < Storage.PreviousPositions[Number].Length && f < 0)
                    {
                        if (position.RelativeValue == Storage.PreviousPositions[Number][i].RelativeValue)
                        {
                            a = true;
                            j = 0;
                            while (j < position.Squares.Length && a)
                            {
                                k = 0;
                                while (k < position.Squares[j].Length && a)
                                {
                                    if (position.Squares[j][k].Team != Storage.PreviousPositions[Number][i].Squares[j][k].Team || position.Squares[j][k].Chessman != Storage.PreviousPositions[Number][i].Squares[j][k].Chessman)
                                    {
                                        a = false;
                                    }
                                    k++;
                                }
                                j++;
                            }
                            if (a)
                            {
                                f = i;
                            }
                        }
                        i++;
                    }
                    if (f >= 0)
                    {
                        Storage.PreviousPositions[Number][i].Count++;
                    }
                    else
                    {
                        Array.Resize(ref Storage.PreviousPositions[Number], Storage.PreviousPositions[Number].Length + 1);
                        Storage.PreviousPositions[Number][Storage.PreviousPositions[Number].Length - 1] = new Position(position)
                        {
                            Count = 1
                        };
                    }
                    if (x >= 0 && y >= 0)
                    {
                        Rectangles[x][y].StrokeThickness = 1;
                        Rectangles[x][y].Stroke = Brushes.Black;
                    }
                    x = -1;
                    y = -1;
                    EndMoving();
                    z = 0;
                    pmoves = new Move[0];
                    GetMoves();
                }
            }
        }

        private void EndMoving()
        {
            int e = Math.Sign(position.GetEvaluation()) * Math.Abs(position.GetEvaluation()) / 30 + (100 * (Math.Abs(position.GetEvaluation()) % 30) / 30) / 100;
            DrawPosition(position);
            if (n % 2 == 0)
            {
                TextBlock.Text = TextBlock.Text + n / 2 + " ";
            }
            else
            {
                int i = 0;
                while (Math.Pow(10, i) <= n / 2)
                {
                    TextBlock.Text = TextBlock.Text + "  ";
                    i++;
                }
                TextBlock.Text = TextBlock.Text + " ";
            }
            if (Storage.Chessmen[c].Abbreviation != "")
            {
                TextBlock.Text = TextBlock.Text + Storage.Chessmen[c].Abbreviation + " ";
            }
            TextBlock.Text = TextBlock.Text + Convert.ToChar(97 + move.X1) + "" + Convert.ToString(move.Y1 + 1) + " - " + Convert.ToChar(97 + move.X2) + "" + Convert.ToString(move.Y2 + 1);
            if (c != position.Squares[move.X2][move.Y2].Chessman)
            {
                if (Storage.Chessmen[position.Squares[move.X2][move.Y2].Chessman].Abbreviation != "")
                {
                    TextBlock.Text = TextBlock.Text + " -> " + Storage.Chessmen[position.Squares[move.X2][move.Y2].Chessman].Abbreviation + " ";
                }
                else
                {
                    TextBlock.Text = TextBlock.Text + " -> ???";
                }
            }
            if (position.Team)
            {
                if (position.IsChecked(1))
                {
                    if (position.GetAllPossibleMoves().Length == 0)
                    {
                        TextBlock.Text = TextBlock.Text + " x";
                    }
                    else
                    {
                        TextBlock.Text = TextBlock.Text + " +";
                    }
                }
            }
            else
            {
                if (position.IsChecked(2))
                {
                    if (position.GetAllPossibleMoves().Length == 0)
                    {
                        TextBlock.Text = TextBlock.Text + " x";
                    }
                    else
                    {
                        TextBlock.Text = TextBlock.Text + " +";
                    }
                }
            }
            TextBlock.Text = TextBlock.Text + "\r\n";
            if (position.GetAllPossibleMoves().Length == 0)
            {
                DrawButton.IsEnabled = false;
                SurrenderButton.IsEnabled = false;
                endgame = true;
                prom = false;
                Curtain.Width = 0;
                Curtain.Height = 0;
                PromotionGrid.Children.Clear();
                if (position.Team == true)
                {
                    if (position.IsChecked(1))
                    {
                        TextBlock.Text = TextBlock.Text + "Чёрные поставили мат!";
                    }
                    else
                    {
                        TextBlock.Text = TextBlock.Text + "На доске ничья!";
                    }
                }
                else
                {
                    if (position.IsChecked(2))
                    {
                        TextBlock.Text = TextBlock.Text + "Белые поставили мат!";
                    }
                    else
                    {
                        TextBlock.Text = TextBlock.Text + "На доске ничья!";
                    }
                }
            }
            ScrollViewer.ScrollToEnd();
            Stabilize();
        }

        private void GetMoves()
        {
            /*Move[] moves = position.GetAllPossibleMoves();
            TextBlock.Height = 100 * moves.Length / 6;
            TextBlock.Text = "";
            int i = 0;
            while (i < moves.Length)
            {
                TextBlock.Text = TextBlock.Text + Storage.Chessmen[position.Squares[moves[i].X1][moves[i].Y1].Chessman].Name + "  " + Convert.ToChar(97 + moves[i].X1) + "" + Convert.ToString(moves[i].Y1 + 1) + " - " + Convert.ToChar(97 + moves[i].X2) + "" + Convert.ToString(moves[i].Y2 + 1);
                if (new Position(position, moves[i]).Squares[moves[i].X2][moves[i].Y2].Chessman != position.Squares[moves[i].X1][moves[i].Y1].Chessman)
                {
                    TextBlock.Text = TextBlock.Text + " " + Storage.Chessmen[new Position(position, moves[i]).Squares[moves[i].X2][moves[i].Y2].Chessman].Name;
                }
                TextBlock.Text = TextBlock.Text + "\r\n";
                i++;
            }*/
        }

        private void Stabilize()
        {
            double m = Canvas.ActualWidth / Grid.Width * 0.65;
            if (m > Canvas.ActualHeight / Grid.Height)
            {
                m = Canvas.ActualHeight / Grid.Height;
            }
            GridScale.ScaleX = m;
            GridScale.ScaleY = m;
            double w = (0.65 * Canvas.ActualWidth - m * Grid.Width) / 2;
            double h = (Canvas.ActualHeight - m * Grid.Height) / 2;
            Grid.Margin = new Thickness(w, h, 0, 0);
            ScrollViewer.Margin = new Thickness(2 * w + m * Grid.Width, 0, 0, 75 * m);
            if (m > 0)
            {
                TextBlock.FontSize = 20 * m;
            }
            //TextBlock.Height = 100 * (n + 1) * m / 3;
            TextBlock.Height = TextBlock.LineHeight;
            TextBlock.Width = 250 * m;
            ButtonPanel.Margin = new Thickness(2 * w + m * Grid.Width, Canvas.ActualHeight - 75 * m, 0, 0);
            RotateButton.Margin = new Thickness(ButtonPanel.ActualWidth / 21, ButtonPanel.ActualHeight / 8, 16 * ButtonPanel.ActualWidth / 21, ButtonPanel.ActualHeight / 8);
            DrawButton.Margin = new Thickness(6 * ButtonPanel.ActualWidth / 21, ButtonPanel.ActualHeight / 8, 11 * ButtonPanel.ActualWidth / 21, ButtonPanel.ActualHeight / 8);
            SurrenderButton.Margin = new Thickness(11 * ButtonPanel.ActualWidth / 21, ButtonPanel.ActualHeight / 8, 6 * ButtonPanel.ActualWidth / 21, ButtonPanel.ActualHeight / 8);
            CloseButton.Margin = new Thickness(16 * ButtonPanel.ActualWidth / 21, ButtonPanel.ActualHeight / 8, ButtonPanel.ActualWidth / 21, ButtonPanel.ActualHeight / 8);
            if (prom)
            {
                Curtain.Margin = new Thickness(w, h, 0, 0);
                Curtain.Width = Grid.Width * m;
                Curtain.Height = Grid.Height * m;
                PromotionGrid.Children.Clear();
                bool f = false;
                int lx = 1;
                while (!f)
                {
                    if ((lx + 1) <= p.Length && Math.Abs(Math.Ceiling(p.Length / (lx + 0.0)) / lx - Canvas.ActualHeight / (0.65 * Canvas.ActualWidth)) > Math.Abs(Math.Ceiling(p.Length / (lx + 1.0)) / (lx + 1) - Canvas.ActualHeight / (0.65 * Canvas.ActualWidth)))
                    {
                        lx++;
                    }
                    else
                    {
                        f = true;
                    }
                }
                int ly = Convert.ToInt32(Math.Ceiling(p.Length / (lx + 0.0)));
                int i = 0;
                int j;
                while (i < ly)
                {
                    j = 0;
                    while (j < lx)
                    {
                        if (i * lx + j < p.Length)
                        {
                            int u = i * lx + j;
                            Rectangle rectangle = new Rectangle()
                            {
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top,
                                Margin = new Thickness(0.325 * Canvas.ActualWidth - 25 * lx * m + j * 50 * m, 0.5 * Canvas.ActualHeight - 25 * ly * m + i * 50 * m, 0, 0),
                                Width = 50 * m,
                                Height = 50 * m,
                                Stroke = Brushes.Black,
                                StrokeThickness = 1,
                                Fill = Brushes.White
                            };
                            if (i % 2 == j % 2)
                            {
                                rectangle.Fill = Brushes.Gray;
                            }
                            rectangle.MouseDown += new MouseButtonEventHandler((sender, args) =>
                            {
                                pr = p[u];
                                p = new int[0];
                                MakeMove(0, 0, false);
                            });
                            PromotionGrid.Children.Add(rectangle);
                            Image image = new Image()
                            {
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top,
                                Margin = new Thickness(0.325 * Canvas.ActualWidth - 25 * lx * m + j * 50 * m, 0.5 * Canvas.ActualHeight - 25 * ly * m + i * 50 * m, 0, 0),
                                Width = 50 * m,
                                Height = 50 * m,
                                Stretch = Stretch.Fill
                            };
                            if (position.Team)
                            {
                                image.Source = Storage.Chessmen[p[i * lx + j]].ImageWhite;
                            }
                            else
                            {
                                image.Source = Storage.Chessmen[p[i * lx + j]].ImageBlack;
                            }
                            image.MouseDown += new MouseButtonEventHandler((sender, args) =>
                            {
                                pr = p[u];
                                p = new int[0];
                                MakeMove(0, 0, false);
                            });
                            PromotionGrid.Children.Add(image);
                            if (p.Length < lx * (i + 1) && p.Length > lx * i)
                            {
                                rectangle.Margin = new Thickness(0.325 * Canvas.ActualWidth - 25 * (p.Length - lx * i) * m + j * 50 * m, 0.5 * Canvas.ActualHeight - 25 * ly * m + i * 50 * m, 0, 0);
                                image.Margin = new Thickness(0.325 * Canvas.ActualWidth - 25 * (p.Length - lx * i) * m + j * 50 * m, 0.5 * Canvas.ActualHeight - 25 * ly * m + i * 50 * m, 0, 0);
                            }
                        }
                        j++;
                    }
                    i++;
                }
            }
            else
            {
                Curtain.Width = 0;
                Curtain.Height = 0;
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

        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            rotation = !rotation;
            DrawBoard(position);
            DrawPosition(position);
            if (x >= 0 && y >= 0)
            {
                Rectangles[x][y].StrokeThickness = 5;
                Rectangles[x][y].Stroke = Brushes.Green;
            }
            Stabilize();
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите предложить ничью?", "Ничья", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (Player1)
                {
                    if (position.GetEvaluation() >= 30)
                    {
                        DrawButton.IsEnabled = false;
                        SurrenderButton.IsEnabled = false;
                        endgame = true;
                        prom = false;
                        Curtain.Width = 0;
                        Curtain.Height = 0;
                        PromotionGrid.Children.Clear();
                        TextBlock.Text = TextBlock.Text + "Игроки согласились на ничью.";
                    }
                    else
                    {
                        MessageBox.Show("Противник не согласен на ничью!");
                    }
                }
                else
                {
                    if (position.GetEvaluation() <= -30)
                    {
                        DrawButton.IsEnabled = false;
                        SurrenderButton.IsEnabled = false;
                        endgame = true;
                        prom = false;
                        Curtain.Width = 0;
                        Curtain.Height = 0;
                        PromotionGrid.Children.Clear();
                        TextBlock.Text = TextBlock.Text + "Игроки согласились на ничью.";
                    }
                    else
                    {
                        MessageBox.Show("Противник не согласен на ничью!");
                    }
                }
            }
        }

        private void SurrenderButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сдаться?", "Сдаться", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DrawButton.IsEnabled = false;
                SurrenderButton.IsEnabled = false;
                endgame = true;
                prom = false;
                Curtain.Width = 0;
                Curtain.Height = 0;
                PromotionGrid.Children.Clear();
                if (Player1)
                {
                    TextBlock.Text = TextBlock.Text + "Белые сдались.";
                }
                else
                {
                    TextBlock.Text = TextBlock.Text + "Чёрные сдались.";
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (endgame || n == 1)
            {
                Timer.Stop();
                MainWindow.WindowState = WindowState;
                MainWindow.Width = Width;
                MainWindow.Height = Height;
                Close();
                MainWindow.Show();
            }
            else
            {
                if (Player1 || Player2)
                {
                    if (MessageBox.Show("Вы действительно хотите завершить игру?", "Выход", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        endgame = true;
                        Timer.Stop();
                        MainWindow.WindowState = WindowState;
                        MainWindow.Width = Width;
                        MainWindow.Height = Height;
                        Close();
                        MainWindow.Show();
                    }
                }
                else
                {
                    if (MessageBox.Show("Вы действительно хотите прекратить это безумие?", "Выход", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        endgame = true;
                        Timer.Stop();
                        MainWindow.WindowState = WindowState;
                        MainWindow.Width = Width;
                        MainWindow.Height = Height;
                        Close();
                        MainWindow.Show();
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (endgame || n == 1)
            {
                Timer.Stop();
                MainWindow.WindowState = WindowState;
                MainWindow.Width = Width;
                MainWindow.Height = Height;
                MainWindow.Show();
            }
            else
            {
                if (MessageBox.Show("Вы действительно хотите завершить игру?", "Выход", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Timer.Stop();
                    MainWindow.WindowState = WindowState;
                    MainWindow.Width = Width;
                    MainWindow.Height = Height;
                    MainWindow.Show();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
