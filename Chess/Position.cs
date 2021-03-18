using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Chess
{
    class Position : Board
    {
        public Position(Board board) : base(board)
        {

        }

        public Position(XmlNode xmlNode) : base()
        {
            PositionName = xmlNode.Attributes.GetNamedItem("name").Value;
            XmlNode[] pieces = new XmlNode[0];
            RelativeValue = 0;
            Pieces = new Piece[2][][];
            int i = 0;
            int j;
            int k;
            int m;
            string t;
            int d;
            int x;
            int y;
            while (i < 2)
            {
                Pieces[i] = new Piece[Storage.Chessmen.Length][];
                j = 0;
                while (j < Storage.Chessmen.Length)
                {
                    Pieces[i][j] = new Piece[0];
                    j++;
                }
                i++;
            }
            i = 0;
            while (i < xmlNode.ChildNodes.Count)
            {
                switch (xmlNode.ChildNodes[i].Name)
                {
                    case "board":
                        m = Storage.GetBoard(xmlNode.ChildNodes[i].InnerText);
                        Squares = new Square[Storage.Boards[m].Squares.Length][];
                        j = 0;
                        while (j < Storage.Boards[m].Squares.Length)
                        {
                            Squares[j] = new Square[Storage.Boards[m].Squares[j].Length];
                            k = 0;
                            while (k < Storage.Boards[m].Squares[j].Length)
                            {
                                Squares[j][k] = new Square(Storage.Boards[m].Squares[j][k]);
                                k++;
                            }
                            j++;
                        }
                        AttributesX = new int[Storage.Boards[m].AttributesX.Length][];
                        AttributesY = new int[AttributesX.Length][];
                        k = 0;
                        while (k < AttributesX.Length)
                        {
                            AttributesX[k] = new int[Storage.Boards[m].AttributesX[k].Length];
                            AttributesY[k] = new int[AttributesX[k].Length];
                            j = 0;
                            while (j < AttributesX[k].Length)
                            {
                                AttributesX[k][j] = Storage.Boards[m].AttributesX[k][j];
                                AttributesY[k][j] = Storage.Boards[m].AttributesY[k][j];
                                j++;
                            }
                            k++;
                        }
                        break;
                    case "move":
                        if (Convert.ToInt32(xmlNode.ChildNodes[i].InnerText) == 1)
                        {
                            Team = true;
                        }
                        else
                        {
                            Team = false;
                        }
                        break;
                    case "pieces":
                        Array.Resize(ref pieces, pieces.Length + 1);
                        pieces[pieces.Length - 1] = xmlNode.ChildNodes[i];
                        break;
                    default:

                        break;
                }
                i++;
            }
            i = 0;
            while (i < pieces.Length)
            {
                j = 0;
                while (j < pieces[i].ChildNodes.Count)
                {
                    if (pieces[i].ChildNodes[j].Name == "piece")
                    {
                        m = -1;
                        d = 0;
                        x = -1;
                        y = -1;
                        t = "";
                        k = 0;
                        while (k < pieces[i].ChildNodes[j].ChildNodes.Count)
                        {
                            switch (pieces[i].ChildNodes[j].ChildNodes[k].Name)
                            {
                                case "team":
                                    m = Convert.ToInt32(pieces[i].ChildNodes[j].ChildNodes[k].InnerText);
                                    break;
                                case "type":
                                    t = pieces[i].ChildNodes[j].ChildNodes[k].InnerText;
                                    break;
                                case "direction":
                                    d = Convert.ToInt32(pieces[i].ChildNodes[j].ChildNodes[k].InnerText);
                                    break;
                                case "coordinateX":
                                    x = Convert.ToInt32(pieces[i].ChildNodes[j].ChildNodes[k].InnerText);
                                    break;
                                case "coordinateY":
                                    y = Convert.ToInt32(pieces[i].ChildNodes[j].ChildNodes[k].InnerText);
                                    break;
                                default:

                                    break;
                            }
                            k++;
                        }
                        if (m >= 0 && x >= 0 && y >= 0 && Storage.GetChessman(t) >= 0)
                        {
                            if (IsEmptySquare(x, y))
                            {
                                Squares[x][y].Team = m;
                                Squares[x][y].Chessman = Storage.GetChessman(t);
                                Squares[x][y].Number = Pieces[m - 1][Squares[x][y].Chessman].Length;
                                if (m == 1)
                                {
                                    RelativeValue = RelativeValue + Storage.Chessmen[Storage.GetChessman(t)].RelativeValue;
                                }
                                if (m == 2)
                                {
                                    RelativeValue = RelativeValue - Storage.Chessmen[Storage.GetChessman(t)].RelativeValue;
                                }
                                Array.Resize(ref Pieces[m - 1][Squares[x][y].Chessman], Pieces[m - 1][Squares[x][y].Chessman].Length + 1);
                                Pieces[m - 1][Squares[x][y].Chessman][Pieces[m - 1][Squares[x][y].Chessman].Length - 1] = new Piece(x, y, d);
                            }
                        }
                    }
                    j++;
                }
                i++;
            }
        }

        public Position(Position position) : base(position)
        {
            Number = position.Number;
            Count = position.Count;
            Maximum = position.Maximum;
            Inaccuracy = position.Inaccuracy;
            PositionName = position.PositionName;
            Team = position.Team;
            RelativeValue = position.RelativeValue;
            Pieces = new Piece[position.Pieces.Length][][];
            int i = 0;
            int j;
            int k;
            while (i < position.Pieces.Length)
            {
                Pieces[i] = new Piece[position.Pieces[i].Length][];
                j = 0;
                while (j < position.Pieces[i].Length)
                {
                    Pieces[i][j] = new Piece[position.Pieces[i][j].Length];
                    k = 0;
                    while (k < position.Pieces[i][j].Length)
                    {
                        Pieces[i][j][k] = new Piece(position.Pieces[i][j][k]);
                        k++;
                    }
                    j++;
                }
                i++;
            }
        }

        public Position(Position position, Move move) : base(position)
        {
            Number = position.Number;
            Count = position.Count;
            Maximum = position.Maximum;
            Inaccuracy = position.Inaccuracy;
            PositionName = "";
            Team = !position.Team;
            RelativeValue = position.RelativeValue;
            LastMove = new Move(move);
            Pieces = new Piece[position.Pieces.Length][][];
            int i = 0;
            int j;
            int k;
            while (i < position.Pieces.Length)
            {
                Pieces[i] = new Piece[position.Pieces[i].Length][];
                j = 0;
                while (j < position.Pieces[i].Length)
                {
                    Pieces[i][j] = new Piece[position.Pieces[i][j].Length];
                    k = 0;
                    while (k < position.Pieces[i][j].Length)
                    {
                        Pieces[i][j][k] = new Piece(position.Pieces[i][j][k]);
                        k++;
                    }
                    j++;
                }
                i++;
            }
            j = move.X1;
            k = move.Y1;
            MakeMove(move);
            if (move.Special >= 0)
            {
                i = 0;
                while (i < Storage.SpecialActions[move.Special].Moves.Length)
                {
                    if (Storage.SpecialActions[move.Special].IsMoveRelative[i])
                    {
                        MakeMove(new Move(Storage.SpecialActions[move.Special].Moves[i], move.Direction, j, k));
                    }
                    else
                    {
                        MakeMove(Storage.SpecialActions[move.Special].Moves[i]);
                    }
                    i++;
                }
                i = 0;
                while (i < Storage.SpecialActions[move.Special].SquareActions.Length)
                {
                    if (Storage.SpecialActions[move.Special].SquareActions[i].Relative)
                    {
                        if (Storage.SpecialActions[move.Special].SquareActions[i].IsEmpty)
                        {
                            if (DoesSquareExist(j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY), k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)))
                            {
                                if (Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team != 0)
                                {
                                    if (Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team == 1)
                                    {
                                        RelativeValue = RelativeValue - Storage.Chessmen[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman].RelativeValue;
                                    }
                                    else
                                    {
                                        RelativeValue = RelativeValue + Storage.Chessmen[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman].RelativeValue;
                                    }
                                }
                                if (Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team > 0 && Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Number >= 0)
                                {
                                    Pieces[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team - 1][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Number].CoordinateX = -1;
                                    Pieces[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team - 1][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Number].CoordinateY = -1;
                                    Pieces[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team - 1][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Number].Moved = true;
                                }
                                Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team = 0;
                                Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman = 0;
                                Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Number = -1;
                            }
                        }
                        if (Storage.SpecialActions[move.Special].SquareActions[i].IsOwnTeam)
                        {
                            /*if (IsAvailableSquare(t, x + GetOffsetX(d, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY), y + GetOffsetY(d, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)))
                            {

                            }*/
                        }
                        if (Storage.SpecialActions[move.Special].SquareActions[i].Chessman >= 0)
                        {
                            if (DoesSquareExist(j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY), k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)))
                            {
                                if (Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team != 0)
                                {
                                    if (Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team == 1)
                                    {
                                        RelativeValue = RelativeValue - Storage.Chessmen[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman].RelativeValue + Storage.Chessmen[Storage.SpecialActions[move.Special].SquareActions[i].Chessman].RelativeValue;
                                    }
                                    else
                                    {
                                        RelativeValue = RelativeValue + Storage.Chessmen[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman].RelativeValue - Storage.Chessmen[Storage.SpecialActions[move.Special].SquareActions[i].Chessman].RelativeValue;
                                    }
                                }
                                Array.Resize(ref Pieces[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team - 1][Storage.SpecialActions[move.Special].SquareActions[i].Chessman], Pieces[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team - 1][Storage.SpecialActions[move.Special].SquareActions[i].Chessman].Length + 1);
                                Pieces[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team - 1][Storage.SpecialActions[move.Special].SquareActions[i].Chessman][Pieces[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team - 1][Storage.SpecialActions[move.Special].SquareActions[i].Chessman].Length - 1] = new Piece(j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY), k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY), Pieces[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team - 1][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Number].Direction)
                                {
                                    Moved = true
                                };
                                if (Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team > 0 && Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Number >= 0)
                                {
                                    Pieces[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team - 1][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Number].CoordinateX = -1;
                                    Pieces[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team - 1][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Number].CoordinateY = -1;
                                    Pieces[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team - 1][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman][Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Number].Moved = true;
                                }
                                Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Chessman = Storage.SpecialActions[move.Special].SquareActions[i].Chessman;
                                Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Number = Pieces[Squares[j + GetOffsetX(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)][k + GetOffsetY(move.Direction, Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY)].Team - 1][Storage.SpecialActions[move.Special].SquareActions[i].Chessman].Length - 1;
                            }
                        }
                    }
                    else
                    {
                        if (Storage.SpecialActions[move.Special].SquareActions[i].IsEmpty)
                        {
                            if (DoesSquareExist(Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY))
                            {
                                if (Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team != 0)
                                {
                                    if (Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team == 1)
                                    {
                                        RelativeValue = RelativeValue - Storage.Chessmen[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman].RelativeValue;
                                    }
                                    else
                                    {
                                        RelativeValue = RelativeValue + Storage.Chessmen[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman].RelativeValue;
                                    }
                                }
                                if (Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team > 0 && Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Number >= 0)
                                {
                                    Pieces[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team - 1][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Number].CoordinateX = -1;
                                    Pieces[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team - 1][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Number].CoordinateY = -1;
                                    Pieces[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team - 1][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Number].Moved = true;
                                }
                                Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team = 0;
                                Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman = 0;
                                Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Number = -1;
                            }
                        }
                        if (Storage.SpecialActions[move.Special].SquareActions[i].IsOwnTeam)
                        {
                            /*if (DoesSquareExist(Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY))
                            {
                                
                            }*/
                        }
                        if (Storage.SpecialActions[move.Special].SquareActions[i].Chessman >= 0)
                        {
                            if (DoesSquareExist(Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY))
                            {
                                if (Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team != 0)
                                {
                                    if (Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team == 1)
                                    {
                                        RelativeValue = RelativeValue - Storage.Chessmen[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman].RelativeValue + Storage.Chessmen[Storage.SpecialActions[move.Special].SquareActions[i].Chessman].RelativeValue;
                                    }
                                    else
                                    {
                                        RelativeValue = RelativeValue + Storage.Chessmen[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman].RelativeValue - Storage.Chessmen[Storage.SpecialActions[move.Special].SquareActions[i].Chessman].RelativeValue;
                                    }
                                }
                                Array.Resize(ref Pieces[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team - 1][Storage.SpecialActions[move.Special].SquareActions[i].Chessman], Pieces[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team - 1][Storage.SpecialActions[move.Special].SquareActions[i].Chessman].Length + 1);
                                Pieces[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team - 1][Storage.SpecialActions[move.Special].SquareActions[i].Chessman][Pieces[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team - 1][Storage.SpecialActions[move.Special].SquareActions[i].Chessman].Length - 1] = new Piece(Storage.SpecialActions[move.Special].SquareActions[i].OffsetX, Storage.SpecialActions[move.Special].SquareActions[i].OffsetY, Pieces[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team - 1][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Number].Direction)
                                {
                                    Moved = true
                                };
                                if (Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team > 0 && Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Number >= 0)
                                {
                                    Pieces[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team - 1][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Number].CoordinateX = -1;
                                    Pieces[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team - 1][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Number].CoordinateY = -1;
                                    Pieces[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team - 1][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman][Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Number].Moved = true;
                                }
                                Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Chessman = Storage.SpecialActions[move.Special].SquareActions[i].Chessman;
                                Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Number = Pieces[Squares[Storage.SpecialActions[move.Special].SquareActions[i].OffsetX][Storage.SpecialActions[move.Special].SquareActions[i].OffsetY].Team - 1][Storage.SpecialActions[move.Special].SquareActions[i].Chessman].Length - 1;
                            }
                        }
                    }
                    i++;
                }
            }
        }

        public string PositionName;
        public bool Team;
        public int RelativeValue;
        public Move LastMove = new Move(0, 0, 0, 0);
        public Piece[][][] Pieces;
        public bool Main;
        public int Number;
        public int Count;
        public int Maximum;
        public int Inaccuracy;

        private void MakeMove(Move move)
        {
            if (Squares[move.X2][move.Y2].Team != 0)
            {
                if (Squares[move.X2][move.Y2].Team == 1)
                {
                    RelativeValue = RelativeValue - Storage.Chessmen[Squares[move.X2][move.Y2].Chessman].RelativeValue;
                }
                else
                {
                    RelativeValue = RelativeValue + Storage.Chessmen[Squares[move.X2][move.Y2].Chessman].RelativeValue;
                }
            }
            Pieces[Squares[move.X1][move.Y1].Team - 1][Squares[move.X1][move.Y1].Chessman][Squares[move.X1][move.Y1].Number].CoordinateX = move.X2;
            Pieces[Squares[move.X1][move.Y1].Team - 1][Squares[move.X1][move.Y1].Chessman][Squares[move.X1][move.Y1].Number].CoordinateY = move.Y2;
            Pieces[Squares[move.X1][move.Y1].Team - 1][Squares[move.X1][move.Y1].Chessman][Squares[move.X1][move.Y1].Number].Moved = true;
            if (Squares[move.X2][move.Y2].Team > 0 && Squares[move.X2][move.Y2].Number >= 0)
            {
                Pieces[Squares[move.X2][move.Y2].Team - 1][Squares[move.X2][move.Y2].Chessman][Squares[move.X2][move.Y2].Number].CoordinateX = -1;
                Pieces[Squares[move.X2][move.Y2].Team - 1][Squares[move.X2][move.Y2].Chessman][Squares[move.X2][move.Y2].Number].CoordinateY = -1;
                Pieces[Squares[move.X2][move.Y2].Team - 1][Squares[move.X2][move.Y2].Chessman][Squares[move.X2][move.Y2].Number].Moved = true;
            }
            Squares[move.X2][move.Y2].Chessman = Squares[move.X1][move.Y1].Chessman;
            Squares[move.X2][move.Y2].Team = Squares[move.X1][move.Y1].Team;
            Squares[move.X2][move.Y2].Number = Squares[move.X1][move.Y1].Number;
            Squares[move.X1][move.Y1].Chessman = 0;
            Squares[move.X1][move.Y1].Team = 0;
            Squares[move.X1][move.Y1].Number = -1;
        }

        private bool DoesSquareExist(int x, int y)
        {
            if (x >= 0 && x < Squares.Length)
            {
                if (y >= 0 && y < Squares[x].Length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsAvailableSquare(int team, int x, int y)
        {
            if (DoesSquareExist(x, y))
            {
                if (team != Squares[x][y].Team)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsEmptySquare(int x, int y)
        {
            if (DoesSquareExist(x, y))
            {
                if (Squares[x][y].Team == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsForeignSquare(int team, int x, int y)
        {
            if (DoesSquareExist(x, y))
            {
                if (team != Squares[x][y].Team && Squares[x][y].Team != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsMovedPiece(int x, int y)
        {
            if (DoesSquareExist(x, y))
            {
                if (Squares[x][y].Team > 0)
                {
                    if (Pieces[Squares[x][y].Team - 1][Squares[x][y].Chessman][Squares[x][y].Number].Moved)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsStoodPiece(int x, int y)
        {
            if (DoesSquareExist(x, y))
            {
                if (Squares[x][y].Team > 0)
                {
                    if (!Pieces[Squares[x][y].Team - 1][Squares[x][y].Chessman][Squares[x][y].Number].Moved)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsNecessaryPiece(int chessman, int x, int y)
        {
            if (DoesSquareExist(x, y))
            {
                if (chessman == Squares[x][y].Chessman)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private int GetOffsetX(int dir, int x, int y)
        {
            switch (dir)
            {
                case 0:
                    return x;
                case 1:
                    return y;
                case 2:
                    return -x;
                case 3:
                    return -y;
                default:
                    return 0;

            }
        }

        private int GetOffsetY(int dir, int x, int y)
        {
            switch (dir)
            {
                case 0:
                    return y;
                case 1:
                    return -x;
                case 2:
                    return -y;
                case 3:
                    return x;
                default:
                    return 0;

            }
        }

        private bool AreConditionsMet(NecessaryCondition condition, int t, int d, int x, int y, bool s)
        {
            bool b = true;
            int t1 = 0;
            if (t == 1)
            {
                t1 = 2;
            }
            else
            {
                t1 = 1;
            }
            int i;
            if (condition.Relative)
            {
                if (condition.IsEmpty || condition.IsOccupied)
                {
                    if (condition.IsEmpty)
                    {
                        if (!IsEmptySquare(x + GetOffsetX(d, condition.OffsetX, condition.OffsetY), y + GetOffsetY(d, condition.OffsetX, condition.OffsetY)))
                        {
                            b = false;
                        }
                    }
                    else
                    {
                        if (IsEmptySquare(x + GetOffsetX(d, condition.OffsetX, condition.OffsetY), y + GetOffsetY(d, condition.OffsetX, condition.OffsetY)))
                        {
                            b = false;
                        }
                    }
                }
                if (condition.IsMoved || condition.IsStood)
                {
                    if (condition.IsMoved)
                    {
                        if (!IsMovedPiece(x + GetOffsetX(d, condition.OffsetX, condition.OffsetY), y + GetOffsetY(d, condition.OffsetX, condition.OffsetY)))
                        {
                            b = false;
                        }
                    }
                    else
                    {
                        if (!IsStoodPiece(x + GetOffsetX(d, condition.OffsetX, condition.OffsetY), y + GetOffsetY(d, condition.OffsetX, condition.OffsetY)))
                        {
                            b = false;
                        }
                    }
                }
                if (s && b)
                {
                    if (condition.IsChecked || condition.IsUnchecked)
                    {
                        if (condition.IsChecked)
                        {
                            if (!IsChecked(t1, x + GetOffsetX(d, condition.OffsetX, condition.OffsetY), y + GetOffsetY(d, condition.OffsetX, condition.OffsetY)))
                            {
                                b = false;
                            }
                        }
                        else
                        {
                            if (IsChecked(t1, x + GetOffsetX(d, condition.OffsetX, condition.OffsetY), y + GetOffsetY(d, condition.OffsetX, condition.OffsetY)))
                            {
                                b = false;
                            }
                        }
                    }
                }
                if (condition.IsOwnTeam || condition.IsOtherTeam)
                {
                    if (condition.IsOwnTeam)
                    {
                        if (IsAvailableSquare(t, x + GetOffsetX(d, condition.OffsetX, condition.OffsetY), y + GetOffsetY(d, condition.OffsetX, condition.OffsetY)))
                        {
                            b = false;
                        }
                    }
                    else
                    {
                        if (!IsAvailableSquare(t, x + GetOffsetX(d, condition.OffsetX, condition.OffsetY), y + GetOffsetY(d, condition.OffsetX, condition.OffsetY)))
                        {
                            b = false;
                        }
                    }
                }
                if (condition.Chessman >= 0)
                {
                    if (!IsNecessaryPiece(condition.Chessman, x + GetOffsetX(d, condition.OffsetX, condition.OffsetY), y + GetOffsetY(d, condition.OffsetX, condition.OffsetY)))
                    {
                        b = false;
                    }
                }
                if (DoesSquareExist(x + GetOffsetX(d, condition.OffsetX, condition.OffsetY), y + GetOffsetY(d, condition.OffsetX, condition.OffsetY)))
                {
                    i = 0;
                    while (i < condition.Attributes.Length)
                    {
                        if (condition.Attributes[i] >= 0 && condition.Attributes[i] < Squares[x + GetOffsetX(d, condition.OffsetX, condition.OffsetY)][y + GetOffsetY(d, condition.OffsetX, condition.OffsetY)].Types.Length)
                        {
                            if (!Squares[x + GetOffsetX(d, condition.OffsetX, condition.OffsetY)][y + GetOffsetY(d, condition.OffsetX, condition.OffsetY)].Types[condition.Attributes[i]])
                            {
                                b = false;
                            }
                        }
                        else
                        {
                            b = false;
                        }
                        i++;
                    }
                }
                else
                {
                    b = false;
                }
            }
            else
            {
                if (condition.IsEmpty || condition.IsOccupied)
                {
                    if (condition.IsEmpty)
                    {
                        if (!IsEmptySquare(condition.OffsetX, condition.OffsetY))
                        {
                            b = false;
                        }
                    }
                    else
                    {
                        if (IsEmptySquare(condition.OffsetX, condition.OffsetY))
                        {
                            b = false;
                        }
                    }
                }
                if (condition.IsMoved || condition.IsStood)
                {
                    if (condition.IsMoved)
                    {
                        if (!IsMovedPiece(condition.OffsetX, condition.OffsetY))
                        {
                            b = false;
                        }
                    }
                    else
                    {
                        if (!IsStoodPiece(condition.OffsetX, condition.OffsetY))
                        {
                            b = false;
                        }
                    }
                }
                if (s && b)
                {
                    if (condition.IsChecked || condition.IsUnchecked)
                    {
                        if (condition.IsChecked)
                        {
                            if (!IsChecked(t1, condition.OffsetX, condition.OffsetY))
                            {
                                b = false;
                            }
                        }
                        else
                        {
                            if (IsChecked(t1, condition.OffsetX, condition.OffsetY))
                            {
                                b = false;
                            }
                        }
                    }
                }
                if (condition.IsOwnTeam || condition.IsOtherTeam)
                {
                    if (condition.IsOwnTeam)
                    {
                        if (IsAvailableSquare(t, condition.OffsetX, condition.OffsetY))
                        {
                            b = false;
                        }
                    }
                    else
                    {
                        if (!IsAvailableSquare(t, condition.OffsetX, condition.OffsetY))
                        {
                            b = false;
                        }
                    }
                }
                if (condition.Chessman >= 0)
                {
                    if (!IsNecessaryPiece(condition.Chessman, condition.OffsetX, condition.OffsetY))
                    {
                        b = false;
                    }
                }
                if (DoesSquareExist(condition.OffsetX, condition.OffsetY))
                {
                    i = 0;
                    while (i < condition.Attributes.Length)
                    {
                        if (condition.Attributes[i] >= 0 && condition.Attributes[i] < Squares[condition.OffsetX][condition.OffsetY].Types.Length)
                        {
                            if (!Squares[condition.OffsetX][condition.OffsetY].Types[condition.Attributes[i]])
                            {
                                b = false;
                            }
                        }
                        else
                        {
                            b = false;
                        }
                        i++;
                    }
                }
                else
                {
                    b = false;
                }
            }
            return b;
        }

        public bool IsChecked(int team)
        {
            bool check = false;
            int i = 1;
            while (i <= 2)
            {
                if (i != team)
                {
                    if (IsChecked(i, team))
                    {
                        check = true;
                        return true;
                    }
                }
                i++;
            }
            return check;
        }

        public bool IsChecked(int teamA, int teamD)
        {
            bool check = false;
            int i = 0;
            int j;
            while (i < Storage.ImportantChessmen.Length)
            {
                j = 0;
                while (j < Pieces[teamD - 1][Storage.ImportantChessmen[i]].Length)
                {
                    if (IsChecked(teamA, Pieces[teamD - 1][Storage.ImportantChessmen[i]][j].CoordinateX, Pieces[teamD - 1][Storage.ImportantChessmen[i]][j].CoordinateY))
                    {
                        check = true;
                        return true;
                    }
                    j++;
                }
                i++;
            }
            return check;
        }

        public bool IsChecked(int team, int X, int Y)
        {
            bool check = false;
            int i = 0;
            int j;
            while (i < Pieces[team - 1].Length)
            {
                j = 0;
                while (j < Pieces[team - 1][i].Length)
                {
                    if (IsChecked(Pieces[team - 1][i][j].CoordinateX, Pieces[team - 1][i][j].CoordinateY, X, Y, false))
                    {
                        check = true;
                        return true;
                    }
                    j++;
                }
                i++;
            }
            return check;
        }

        public bool IsChecked(int X1, int Y1, int X2, int Y2, bool s)
        {
            if (X1 >= 0 && X2 >= 0 && X1 < Squares.Length && X2 < Squares.Length)
            {
                if (Y1 >= 0 && Y2 >= 0 && Y1 < Squares[X1].Length && Y2 < Squares[X1].Length)
                {
                    if (Squares[X1][Y1].Team > 0)
                    {
                        if (s || Squares[X1][Y1].Team != Squares[X2][Y2].Team)
                        {
                            bool c = false;
                            bool a;
                            bool o;
                            int i;
                            int j;
                            int d;
                            int n;
                            int m;
                            if (Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Direction)
                            {
                                i = 0;
                                d = Pieces[Squares[X1][Y1].Team - 1][Squares[X1][Y1].Chessman][Squares[X1][Y1].Number].Direction;
                                while (i < Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves.Length)
                                {
                                    a = false;
                                    if (GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) != 0 && GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) != 0)
                                    {
                                        if ((X1 - X2 + 0.0) / GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) == (Y1 - Y2 + 0.0) / GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY))
                                        {
                                            a = true;
                                        }
                                    }
                                    else
                                    {
                                        if (GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) != 0)
                                        {
                                            if ((X1 - X2) % GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) == 0 && Y1 == Y2)
                                            {
                                                a = true;
                                            }
                                        }
                                        else
                                        {
                                            if (GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) != 0)
                                            {
                                                if ((Y1 - Y2) % GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) == 0 && X1 == X2)
                                                {
                                                    a = true;
                                                }
                                            }
                                        }
                                    }
                                    if (a)
                                    {
                                        n = 0;
                                        if (GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) != 0)
                                        {
                                            n = (X2 - X1) / GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY);
                                        }
                                        if (GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) != 0)
                                        {
                                            n = (Y2 - Y1) / GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY);
                                        }
                                        if ((Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].Repetition == 0 || n <= Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].Repetition) && n > 0)
                                        {
                                            j = 1;
                                            o = false;
                                            while (!o && j < n)
                                            {
                                                if (Squares[X1 + j * GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY)][Y1 + j * GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY)].Team > 0)
                                                {
                                                    if (!IsChecked(X1 + j * GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY), Y1 + j * GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY), X2, Y2, s))
                                                    {
                                                        o = true;
                                                    }
                                                    else
                                                    {
                                                        return true;
                                                    }
                                                }
                                                j++;
                                            }
                                            if (!o)
                                            {
                                                c = true;
                                            }
                                        }
                                    }
                                    i++;
                                }
                            }
                            else
                            {
                                d = 0;
                                while (d < 4)
                                {
                                    i = 0;
                                    while (i < Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves.Length)
                                    {
                                        a = false;
                                        if (GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) != 0 && GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) != 0)
                                        {
                                            if ((X1 - X2 + 0.0) / GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) == (Y1 - Y2 + 0.0) / GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY))
                                            {
                                                a = true;
                                            }
                                        }
                                        else
                                        {
                                            if (GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) != 0)
                                            {
                                                if ((X1 - X2) % GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) == 0 && Y1 == Y2)
                                                {
                                                    a = true;
                                                }
                                            }
                                            else
                                            {
                                                if (GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) != 0)
                                                {
                                                    if ((Y1 - Y2) % GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) == 0 && X1 == X2)
                                                    {
                                                        a = true;
                                                    }
                                                }
                                            }
                                        }
                                        if (a)
                                        {
                                            n = 0;
                                            if (GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) != 0)
                                            {
                                                n = (X2 - X1) / GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY);
                                            }
                                            if (GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY) != 0)
                                            {
                                                n = (Y2 - Y1) / GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY);
                                            }
                                            if ((Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].Repetition == 0 || n <= Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].Repetition) && n > 0)
                                            {
                                                j = 1;
                                                o = false;
                                                while (!o && j < n)
                                                {
                                                    if (Squares[X1 + j * GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY)][Y1 + j * GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY)].Team > 0)
                                                    {
                                                        if (!IsChecked(X1 + j * GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY), Y1 + j * GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[X1][Y1].Chessman].Capturing].Moves[i].OffsetY), X2, Y2, s))
                                                        {
                                                            o = true;
                                                        }
                                                        else
                                                        {
                                                            return true;
                                                        }
                                                    }
                                                    j++;
                                                }
                                                if (!o)
                                                {
                                                    c = true;
                                                }
                                            }
                                        }
                                        i++;
                                    }
                                    d++;
                                }
                            }
                            if (!c)
                            {
                                m = 0;
                                while (!c && m < Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves.Length)
                                {
                                    if (Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].Capturing)
                                    {
                                        n = 0;
                                        i = 0;
                                        while (i < Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].Conditions.Length && n == i)
                                        {
                                            if (AreConditionsMet(Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].Conditions[i], Squares[X1][Y1].Team, Pieces[Squares[X1][Y1].Team - 1][Squares[X1][Y1].Chessman][Squares[X1][Y1].Number].Direction, X1, Y1, false))
                                            {
                                                n++;
                                            }
                                            i++;
                                        }
                                        if (i == n)
                                        {
                                            if (Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].Direction)
                                            {
                                                d = Pieces[Squares[X1][Y1].Team - 1][Squares[X1][Y1].Chessman][Squares[X1][Y1].Number].Direction;
                                                a = false;
                                                if (GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) != 0 && GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) != 0)
                                                {
                                                    if ((X1 - X2 + 0.0) / GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) == (Y1 - Y2 + 0.0) / GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY))
                                                    {
                                                        a = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) != 0)
                                                    {
                                                        if ((X1 - X2) % GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) == 0 && Y1 == Y2)
                                                        {
                                                            a = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) != 0)
                                                        {
                                                            if ((Y1 - Y2) % GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) == 0 && X1 == X2)
                                                            {
                                                                a = true;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (a)
                                                {
                                                    n = 0;
                                                    if (GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) != 0)
                                                    {
                                                        n = (X2 - X1) / GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY);
                                                    }
                                                    if (GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) != 0)
                                                    {
                                                        n = (Y2 - Y1) / GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY);
                                                    }
                                                    if ((Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].Repetition == 0 || n <= Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].Repetition) && n > 0)
                                                    {
                                                        j = 1;
                                                        o = false;
                                                        while (!o && j < n)
                                                        {
                                                            if (Squares[X1 + j * GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY)][Y1 + j * GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY)].Team > 0)
                                                            {
                                                                if (!IsChecked(X1 + j * GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY), Y1 + j * GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY), X2, Y2, s))
                                                                {
                                                                    o = true;
                                                                }
                                                                else
                                                                {
                                                                    return true;
                                                                }
                                                            }
                                                            j++;
                                                        }
                                                        if (!o)
                                                        {
                                                            c = true;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                d = 0;
                                                while (d < 4)
                                                {
                                                    a = false;
                                                    if (GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) != 0 && GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) != 0)
                                                    {
                                                        if ((X1 - X2 + 0.0) / GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) == (Y1 - Y2 + 0.0) / GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY))
                                                        {
                                                            a = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) != 0)
                                                        {
                                                            if ((X1 - X2) % GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) == 0 && Y1 == Y2)
                                                            {
                                                                a = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) != 0)
                                                            {
                                                                if ((Y1 - Y2) % GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) == 0 && X1 == X2)
                                                                {
                                                                    a = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (a)
                                                    {
                                                        n = 0;
                                                        if (GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) != 0)
                                                        {
                                                            n = (X2 - X1) / GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY);
                                                        }
                                                        if (GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY) != 0)
                                                        {
                                                            n = (Y2 - Y1) / GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY);
                                                        }
                                                        if ((Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].Repetition == 0 || n <= Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].Repetition) && n > 0)
                                                        {
                                                            j = 1;
                                                            o = false;
                                                            while (!o && j < n)
                                                            {
                                                                if (Squares[X1 + j * GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY)][Y1 + j * GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY)].Team > 0)
                                                                {
                                                                    if (!IsChecked(X1 + j * GetOffsetX(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY), Y1 + j * GetOffsetY(d, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetX, Storage.SpecialMoves[Storage.Chessmen[Squares[X1][Y1].Chessman].SpecialMoves[m]].OffsetY), X2, Y2, s))
                                                                    {
                                                                        o = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        return true;
                                                                    }
                                                                }
                                                                j++;
                                                            }
                                                            if (!o)
                                                            {
                                                                c = true;
                                                            }
                                                        }
                                                    }
                                                    d++;
                                                }
                                            }
                                        }
                                    }
                                    m++;
                                }
                            }
                            return c;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private int GetDistance(int x0, int y0, int x, int y)
        {
            int m = Squares.Length + Squares[0].Length;
            int i;
            if (Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Direction)
            {
                int d = Pieces[Squares[x0][y0].Team - 1][Squares[x0][y0].Chessman][Squares[x0][y0].Number].Direction;
                i = 0;
                while (i < Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves.Length)
                {
                    if (Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX != 0)
                    {
                        if (Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY != 0)
                        {
                            if ((y - y0) / (y - y0 + 0.0) / GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY) == (x - x0 + 0.0) / GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY) && (y - y0) % GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY) == 0)
                            {
                                if (GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY) > 0)
                                {
                                    m = (y - y0) / GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY);
                                }
                            }
                        }
                        else
                        {
                            if (y0 == y && (x - x0) % GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY) == 0)
                            {
                                if ((x - x0) / GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY) > 0)
                                {
                                    m = (x - x0) / GetOffsetX(d, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (x0 == x && (y - y0) % GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY) == 0)
                        {
                            if ((y - y0) / GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY) > 0)
                            {
                                m = (y - y0) / GetOffsetY(d, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX, Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY);
                            }
                        }
                    }
                    i++;
                }
            }
            else
            {
                i = 0;
                while (i < Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves.Length)
                {
                    if (Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX != 0)
                    {
                        if (Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY != 0)
                        {
                            if (Math.Abs((y - y0 + 0.0) / Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY) == Math.Abs((x - x0 + 0.0) / Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX) && (y - y0) % Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY == 0)
                            {
                                m = Math.Abs((y - y0) / Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY);
                            }
                        }
                        else
                        {
                            if (y0 == y && (x - x0) % Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX == 0)
                            {
                                m = Math.Abs((x - x0) / Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetX);
                            }
                        }
                    }
                    else
                    {
                        if (x0 == x && (y - y0) % Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY == 0)
                        {
                            m = Math.Abs((y - y0) / Storage.Moves[Storage.Chessmen[Squares[x0][y0].Chessman].Movement].Moves[i].OffsetY);
                        }
                    }
                    i++;
                }
            }
            if (m != 0)
            {
                return m;
            }
            else
            {
                return Squares.Length + Squares[0].Length;
            }
        }

        public int GetEvaluation()
        {
            Move[] moves1 = GetAllPossibleMoves(true);
            Move[] moves2 = GetAllPossibleMoves(false);
            if (moves1.Length == 0 || moves2.Length == 0)
            {
                if (moves1.Length == 0)
                {
                    if (IsChecked(1))
                    {
                        return -300000;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    if (IsChecked(2))
                    {
                        return 300000;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else
            {
                int i = 0;
                int j;
                int n;
                int m;
                int k;
                int d;
                int r = 0;
                int p = 0;
                while (i < Pieces[0].Length)
                {
                    j = 0;
                    while (j < Pieces[0][i].Length)
                    {
                        d = 0;
                        n = 0;
                        while (n < Pieces.Length)
                        {
                            m = 0;
                            while (m < Pieces[n].Length)
                            {
                                k = 0;
                                while (k < Pieces[n][m].Length)
                                {
                                    if (IsChecked(Pieces[n][m][k].CoordinateX, Pieces[n][m][k].CoordinateY, Pieces[0][i][j].CoordinateX, Pieces[0][i][j].CoordinateY, true))
                                    {
                                        if (n == 1)
                                        {
                                            d++;
                                            if (Storage.Chessmen[i].RelativeValue >= Storage.Chessmen[m].RelativeValue)
                                            {
                                                if (Team)
                                                {
                                                    r = r - Storage.Chessmen[i].RelativeValue + Storage.Chessmen[m].RelativeValue;
                                                }
                                                else
                                                {
                                                    if (!new Position(this, new Move(Pieces[n][m][k].CoordinateX, Pieces[n][m][k].CoordinateY, Pieces[0][i][j].CoordinateX, Pieces[0][i][j].CoordinateY)).IsChecked(2))
                                                    {
                                                        r = r - 20 * Storage.Chessmen[i].RelativeValue + Storage.Chessmen[m].RelativeValue;
                                                    }
                                                    else
                                                    {
                                                        r = r - 2 * Storage.Chessmen[i].RelativeValue + Storage.Chessmen[m].RelativeValue;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            d--;
                                        }
                                    }
                                    k++;
                                }
                                m++;
                            }
                            n++;
                        }
                        j++;
                        if (d > 0)
                        {
                            if (Team)
                            {
                                r = r - 3 * Storage.Chessmen[i].RelativeValue;
                            }
                            else
                            {
                                r = r - 30 * Storage.Chessmen[i].RelativeValue;
                            }
                        }
                    }
                    i++;
                }
                i = 0;
                while (i < Pieces[1].Length)
                {
                    j = 0;
                    while (j < Pieces[1][i].Length)
                    {
                        d = 0;
                        n = 0;
                        while (n < Pieces.Length)
                        {
                            m = 0;
                            while (m < Pieces[n].Length)
                            {
                                k = 0;
                                while (k < Pieces[n][m].Length)
                                {
                                    if (IsChecked(Pieces[n][m][k].CoordinateX, Pieces[n][m][k].CoordinateY, Pieces[1][i][j].CoordinateX, Pieces[1][i][j].CoordinateY, true))
                                    {
                                        if (n == 0)
                                        {
                                            d++;
                                            if (Storage.Chessmen[i].RelativeValue >= Storage.Chessmen[m].RelativeValue)
                                            {

                                                if (!Team)
                                                {
                                                    r = r + Storage.Chessmen[i].RelativeValue - Storage.Chessmen[m].RelativeValue;
                                                }
                                                else
                                                {
                                                    if (!new Position(this, new Move(Pieces[n][m][k].CoordinateX, Pieces[n][m][k].CoordinateY, Pieces[1][i][j].CoordinateX, Pieces[1][i][j].CoordinateY)).IsChecked(1))
                                                    {
                                                        r = r + 20 * Storage.Chessmen[i].RelativeValue - Storage.Chessmen[m].RelativeValue;
                                                    }
                                                    else
                                                    {
                                                        r = r + 2 * Storage.Chessmen[i].RelativeValue - Storage.Chessmen[m].RelativeValue;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            d--;
                                        }
                                    }
                                    k++;
                                }
                                m++;
                            }
                            n++;
                        }
                        j++;
                        if (d > 0)
                        {
                            if (!Team)
                            {
                                r = r + 3 * Storage.Chessmen[i].RelativeValue;
                            }
                            else
                            {
                                r = r + 30 * Storage.Chessmen[i].RelativeValue;
                            }
                        }
                    }
                    i++;
                }
                i = 0;
                while (i < Storage.PromotionChessmen.Length )
                {
                    j = 0;
                    while (j < AttributesX.Length)
                    {
                        if (Storage.Promotions[Storage.PromotionChessmen[i]][j] > Storage.Chessmen[Storage.PromotionChessmen[i]].RelativeValue)
                        {
                            n = 0;
                            while (n < Pieces[0][Storage.PromotionChessmen[i]].Length)
                            {
                                if (Pieces[0][Storage.PromotionChessmen[i]][n].CoordinateX >= 0 && Pieces[0][Storage.PromotionChessmen[i]][n].CoordinateY >= 0)
                                {
                                    m = Squares.Length + Squares[0].Length;
                                    k = 0;
                                    while (k < AttributesX[j].Length)
                                    {
                                        if (GetDistance(Pieces[0][Storage.PromotionChessmen[i]][n].CoordinateX, Pieces[0][Storage.PromotionChessmen[i]][n].CoordinateY, AttributesX[j][k], AttributesY[j][k]) < m)
                                        {
                                            m = GetDistance(Pieces[0][Storage.PromotionChessmen[i]][n].CoordinateX, Pieces[0][Storage.PromotionChessmen[i]][n].CoordinateY, AttributesX[j][k], AttributesY[j][k]);
                                        }
                                        k++;
                                    }
                                    if (m > 0)
                                    {
                                        p = p + (Storage.Promotions[Storage.PromotionChessmen[i]][j] - Storage.Chessmen[Storage.PromotionChessmen[i]].RelativeValue) / m;
                                    }
                                }
                                n++;
                            }
                            n = 0;
                            while (n < Pieces[1][Storage.PromotionChessmen[i]].Length)
                            {
                                if (Pieces[1][Storage.PromotionChessmen[i]][n].CoordinateX >= 0 && Pieces[0][Storage.PromotionChessmen[i]][n].CoordinateY >= 0)
                                {
                                    m = Squares.Length + Squares[0].Length;
                                    k = 0;
                                    while (k < AttributesX[j].Length)
                                    {
                                        if (GetDistance(Pieces[1][Storage.PromotionChessmen[i]][n].CoordinateX, Pieces[1][Storage.PromotionChessmen[i]][n].CoordinateY, AttributesX[j][k], AttributesY[j][k]) < m)
                                        {
                                            m = GetDistance(Pieces[1][Storage.PromotionChessmen[i]][n].CoordinateX, Pieces[1][Storage.PromotionChessmen[i]][n].CoordinateY, AttributesX[j][k], AttributesY[j][k]);
                                        }
                                        k++;
                                    }
                                    if (m > 0)
                                    {
                                        p = p - (Storage.Promotions[Storage.PromotionChessmen[i]][j] - Storage.Chessmen[Storage.PromotionChessmen[i]].RelativeValue) / m;
                                    }
                                }
                                n++;
                            }
                        }
                        j++;
                    }
                    i++;
                }
                return moves1.Length - moves2.Length + r + 5 * p + 30 * RelativeValue;
            }
        }

        public Move[] GetAllMoves(int team, bool special)
        {
            Move[] moves = new Move[0];
            bool[][] s;
            int i = 0;
            int j;
            int x;
            int y;
            int z;
            int d;
            int k;
            int n;
            int t;
            while (i < Pieces[team - 1].Length)
            {
                j = 0;
                while (j < Pieces[team - 1][i].Length)
                {
                    x = Pieces[team - 1][i][j].CoordinateX;
                    y = Pieces[team - 1][i][j].CoordinateY;
                    if (x >= 0 && y >= 0)
                    {
                        s = null;
                        k = 0;
                        while (k < Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves.Length)
                        {
                            if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Direction)
                            {
                                d = Pieces[team - 1][i][j].Direction;
                                n = 0;
                                t = 0;
                                while (n < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Conditions.Length)
                                {
                                    if (AreConditionsMet(Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Conditions[n], Squares[x][y].Team, Pieces[Squares[x][y].Team - 1][Squares[x][y].Chessman][Squares[x][y].Number].Direction, x, y, special))
                                    {
                                        t++;
                                    }
                                    n++;
                                }
                                t--;
                                if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].LastMove == null)
                                {
                                    t++;
                                }
                                else
                                {
                                    if (LastMove.IsMovesSame(new Move(Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].LastMove, d, x, y)))
                                    {
                                        t++;
                                    }
                                }
                                if (t == n)
                                {
                                    if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Movement && Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Capturing)
                                    {
                                        if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition == 0)
                                        {
                                            n = 1;
                                            while (IsEmptySquare(x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                            {
                                                z = 0;
                                                while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                {
                                                    Array.Resize(ref moves, moves.Length + 1);
                                                    moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                    z++;
                                                }
                                                if (s != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2] != null)
                                                    {
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s = new bool[Squares.Length][];
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                                n++;
                                            }
                                            if (IsAvailableSquare(team, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                            {
                                                z = 0;
                                                while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                {
                                                    Array.Resize(ref moves, moves.Length + 1);
                                                    moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                    z++;
                                                }
                                                if (s != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2] != null)
                                                    {
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s = new bool[Squares.Length][];
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            n = 1;
                                            while (n <= Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition && IsEmptySquare(x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                            {

                                                z = 0;
                                                while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                {
                                                    Array.Resize(ref moves, moves.Length + 1);
                                                    moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                    z++;
                                                }
                                                if (s != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2] != null)
                                                    {
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s = new bool[Squares.Length][];
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                                n++;
                                            }
                                            if (n <= Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition && IsAvailableSquare(team, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                            {

                                                z = 0;
                                                while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                {
                                                    Array.Resize(ref moves, moves.Length + 1);
                                                    moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                    z++;
                                                }
                                                if (s != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2] != null)
                                                    {
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s = new bool[Squares.Length][];
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Movement)
                                        {
                                            if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition == 0)
                                            {
                                                n = 1;
                                                while (IsEmptySquare(x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                {

                                                    z = 0;
                                                    while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                    {
                                                        Array.Resize(ref moves, moves.Length + 1);
                                                        moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                        z++;
                                                    }
                                                    if (s != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2] != null)
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s = new bool[Squares.Length][];
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                    n++;
                                                }
                                            }
                                            else
                                            {
                                                n = 1;
                                                while (n <= Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition && IsEmptySquare(x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                {

                                                    z = 0;
                                                    while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                    {
                                                        Array.Resize(ref moves, moves.Length + 1);
                                                        moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                        z++;
                                                    }
                                                    if (s != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2] != null)
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s = new bool[Squares.Length][];
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                    n++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition == 0)
                                            {
                                                n = 1;
                                                while (IsEmptySquare(x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                {
                                                    n++;
                                                }
                                                if (IsForeignSquare(team, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                {

                                                    z = 0;
                                                    while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                    {
                                                        Array.Resize(ref moves, moves.Length + 1);
                                                        moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                        z++;
                                                    }
                                                    if (s != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2] != null)
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s = new bool[Squares.Length][];
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                    n++;
                                                }
                                            }
                                            else
                                            {
                                                n = 1;
                                                while (n <= Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition && IsEmptySquare(x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                {
                                                    n++;
                                                }
                                                if (n <= Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition && IsForeignSquare(team, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                {

                                                    z = 0;
                                                    while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                    {
                                                        Array.Resize(ref moves, moves.Length + 1);
                                                        moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                        z++;
                                                    }
                                                    if (s != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2] != null)
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s = new bool[Squares.Length][];
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                    n++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                d = 0;
                                while (d < 4)
                                {
                                    n = 0;
                                    t = 0;
                                    while (n < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Conditions.Length)
                                    {
                                        if (AreConditionsMet(Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Conditions[n], team, d, x, y, special))
                                        {
                                            t++;
                                        }
                                        n++;
                                    }
                                    t--;
                                    if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].LastMove == null)
                                    {
                                        t++;
                                    }
                                    else
                                    {
                                        if (LastMove.IsMovesSame(new Move(Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].LastMove, d, x, y)))
                                        {
                                            t++;
                                        }
                                    }
                                    if (t == n)
                                    {
                                        if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Movement && Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Capturing)
                                        {
                                            if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition == 0)
                                            {
                                                n = 1;
                                                while (IsEmptySquare(x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                {

                                                    z = 0;
                                                    while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                    {
                                                        Array.Resize(ref moves, moves.Length + 1);
                                                        moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                        z++;
                                                    }
                                                    if (s != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2] != null)
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s = new bool[Squares.Length][];
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                    n++;
                                                }
                                                if (IsAvailableSquare(team, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                {

                                                    z = 0;
                                                    while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                    {
                                                        Array.Resize(ref moves, moves.Length + 1);
                                                        moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                        z++;
                                                    }
                                                    if (s != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2] != null)
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s = new bool[Squares.Length][];
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                n = 1;
                                                while (n <= Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition && IsEmptySquare(x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                {

                                                    z = 0;
                                                    while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                    {
                                                        Array.Resize(ref moves, moves.Length + 1);
                                                        moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                        z++;
                                                    }
                                                    if (s != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2] != null)
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s = new bool[Squares.Length][];
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                    n++;
                                                }
                                                if (n <= Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition && IsAvailableSquare(team, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                {

                                                    z = 0;
                                                    while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                    {
                                                        Array.Resize(ref moves, moves.Length + 1);
                                                        moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                        z++;
                                                    }
                                                    if (s != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2] != null)
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s = new bool[Squares.Length][];
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Movement)
                                            {
                                                if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition == 0)
                                                {
                                                    n = 1;
                                                    while (IsEmptySquare(x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                    {

                                                        z = 0;
                                                        while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                        {
                                                            Array.Resize(ref moves, moves.Length + 1);
                                                            moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                            z++;
                                                        }
                                                        if (s != null)
                                                        {
                                                            if (s[moves[moves.Length - 1].X2] != null)
                                                            {
                                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                            }
                                                            else
                                                            {
                                                                s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            s = new bool[Squares.Length][];
                                                            s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                        n++;
                                                    }
                                                }
                                                else
                                                {
                                                    n = 1;
                                                    while (n <= Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition && IsEmptySquare(x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                    {

                                                        z = 0;
                                                        while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                        {
                                                            Array.Resize(ref moves, moves.Length + 1);
                                                            moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                            z++;
                                                        }
                                                        if (s != null)
                                                        {
                                                            if (s[moves[moves.Length - 1].X2] != null)
                                                            {
                                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                            }
                                                            else
                                                            {
                                                                s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            s = new bool[Squares.Length][];
                                                            s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                        n++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].Repetition == 0)
                                                {
                                                    n = 1;
                                                    while (IsForeignSquare(team, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                    {

                                                        z = 0;
                                                        while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                        {
                                                            Array.Resize(ref moves, moves.Length + 1);
                                                            moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                            z++;
                                                        }
                                                        if (s != null)
                                                        {
                                                            if (s[moves[moves.Length - 1].X2] != null)
                                                            {
                                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                            }
                                                            else
                                                            {
                                                                s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            s = new bool[Squares.Length][];
                                                            s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                        n++;
                                                    }
                                                }
                                                else
                                                {
                                                    n = 1;
                                                    while (n <= Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].Repetition && IsForeignSquare(team, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY)))
                                                    {

                                                        z = 0;
                                                        while (z < Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions.Length)
                                                        {
                                                            Array.Resize(ref moves, moves.Length + 1);
                                                            moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), y + GetOffsetY(d, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetX, n * Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].OffsetY), Storage.SpecialMoves[Storage.Chessmen[Squares[x][y].Chessman].SpecialMoves[k]].SpecialActions[z], d);
                                                            z++;
                                                        }
                                                        if (s != null)
                                                        {
                                                            if (s[moves[moves.Length - 1].X2] != null)
                                                            {
                                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                            }
                                                            else
                                                            {
                                                                s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            s = new bool[Squares.Length][];
                                                            s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                        n++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    d++;
                                }
                                d = 0;
                            }
                            k++;
                        }
                        d = Pieces[team - 1][i][j].Direction;
                        if (Storage.Chessmen[Squares[x][y].Chessman].IsSame)
                        {
                            if (Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Direction)
                            {
                                k = 0;
                                while (k < Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves.Length)
                                {
                                    if (Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].Repetition == 0)
                                    {
                                        n = 1;
                                        while (IsEmptySquare(x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY)))
                                        {
                                            Array.Resize(ref moves, moves.Length + 1);
                                            moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY));
                                            if (s != null)
                                            {
                                                if (s[moves[moves.Length - 1].X2] != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                    {
                                                        Array.Resize(ref moves, moves.Length - 1);
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                            }
                                            else
                                            {
                                                s = new bool[Squares.Length][];
                                                s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                            }
                                            n++;
                                        }
                                        if (IsAvailableSquare(team, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY)))
                                        {
                                            Array.Resize(ref moves, moves.Length + 1);
                                            moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY));
                                            if (s != null)
                                            {
                                                if (s[moves[moves.Length - 1].X2] != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                    {
                                                        Array.Resize(ref moves, moves.Length - 1);
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                            }
                                            else
                                            {
                                                s = new bool[Squares.Length][];
                                                s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        n = 1;
                                        while (n <= Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].Repetition && IsEmptySquare(x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY)))
                                        {
                                            Array.Resize(ref moves, moves.Length + 1);
                                            moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY));
                                            if (s != null)
                                            {
                                                if (s[moves[moves.Length - 1].X2] != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                    {
                                                        Array.Resize(ref moves, moves.Length - 1);
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                            }
                                            else
                                            {
                                                s = new bool[Squares.Length][];
                                                s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                            }
                                            n++;
                                        }
                                        if (n <= Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].Repetition && IsAvailableSquare(team, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY)))
                                        {
                                            Array.Resize(ref moves, moves.Length + 1);
                                            moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY));
                                            if (s != null)
                                            {
                                                if (s[moves[moves.Length - 1].X2] != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                    {
                                                        Array.Resize(ref moves, moves.Length - 1);
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                            }
                                            else
                                            {
                                                s = new bool[Squares.Length][];
                                                s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                            }
                                        }
                                    }
                                    k++;
                                }
                            }
                            else
                            {
                                d = 0;
                                while (d < 4)
                                {
                                    k = 0;
                                    while (k < Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves.Length)
                                    {
                                        if (Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].Repetition == 0)
                                        {
                                            n = 1;
                                            while (IsEmptySquare(x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY)))
                                            {
                                                Array.Resize(ref moves, moves.Length + 1);
                                                moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY));
                                                if (s != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2] != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                        {
                                                            Array.Resize(ref moves, moves.Length - 1);
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s = new bool[Squares.Length][];
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                                n++;
                                            }
                                            if (IsAvailableSquare(team, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY)))
                                            {
                                                Array.Resize(ref moves, moves.Length + 1);
                                                moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY));
                                                if (s != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2] != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                        {
                                                            Array.Resize(ref moves, moves.Length - 1);
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s = new bool[Squares.Length][];
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            n = 1;
                                            while (n <= Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].Repetition && IsEmptySquare(x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY)))
                                            {
                                                Array.Resize(ref moves, moves.Length + 1);
                                                moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY));
                                                if (s != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2] != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                        {
                                                            Array.Resize(ref moves, moves.Length - 1);
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s = new bool[Squares.Length][];
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                                n++;
                                            }
                                            if (n <= Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].Repetition && IsAvailableSquare(team, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY)))
                                            {
                                                Array.Resize(ref moves, moves.Length + 1);
                                                moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY));
                                                if (s != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2] != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                        {
                                                            Array.Resize(ref moves, moves.Length - 1);
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s = new bool[Squares.Length][];
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                            }
                                        }
                                        k++;
                                    }
                                    d++;
                                }
                                d = 0;
                            }
                        }
                        else
                        {
                            if (Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Direction)
                            {
                                k = 0;
                                while (k < Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves.Length)
                                {
                                    if (Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].Repetition == 0)
                                    {
                                        n = 1;
                                        while (IsEmptySquare(x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY)))
                                        {
                                            Array.Resize(ref moves, moves.Length + 1);
                                            moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY));
                                            if (s != null)
                                            {
                                                if (s[moves[moves.Length - 1].X2] != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                    {
                                                        Array.Resize(ref moves, moves.Length - 1);
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                            }
                                            else
                                            {
                                                s = new bool[Squares.Length][];
                                                s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                            }
                                            n++;
                                        }
                                    }
                                    else
                                    {
                                        n = 1;
                                        while (n <= Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].Repetition && IsEmptySquare(x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY)))
                                        {
                                            Array.Resize(ref moves, moves.Length + 1);
                                            moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY));
                                            if (s != null)
                                            {
                                                if (s[moves[moves.Length - 1].X2] != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                    {
                                                        Array.Resize(ref moves, moves.Length - 1);
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                            }
                                            else
                                            {
                                                s = new bool[Squares.Length][];
                                                s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                            }
                                            n++;
                                        }
                                    }
                                    k++;
                                }
                            }
                            else
                            {
                                d = 0;
                                while (d < 4)
                                {
                                    k = 0;
                                    while (k < Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves.Length)
                                    {
                                        if (Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].Repetition == 0)
                                        {
                                            n = 1;
                                            while (IsEmptySquare(x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY)))
                                            {
                                                Array.Resize(ref moves, moves.Length + 1);
                                                moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY));
                                                if (s != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2] != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                        {
                                                            Array.Resize(ref moves, moves.Length - 1);
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s = new bool[Squares.Length][];
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                                n++;
                                            }
                                        }
                                        else
                                        {
                                            n = 1;
                                            while (n <= Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].Repetition && IsEmptySquare(x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY)))
                                            {
                                                Array.Resize(ref moves, moves.Length + 1);
                                                moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].OffsetY));
                                                if (s != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2] != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                        {
                                                            Array.Resize(ref moves, moves.Length - 1);
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s = new bool[Squares.Length][];
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                                n++;
                                            }
                                        }
                                        k++;
                                    }
                                    d++;
                                }
                                d = 0;
                            }
                            if (Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Direction)
                            {
                                k = 0;
                                while (k < Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves.Length)
                                {
                                    if (Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].Repetition == 0)
                                    {
                                        n = 1;
                                        while (IsEmptySquare(x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY)))
                                        {
                                            n++;
                                        }
                                        if (IsForeignSquare(team, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY)))
                                        {
                                            Array.Resize(ref moves, moves.Length + 1);
                                            moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY));
                                            if (s != null)
                                            {
                                                if (s[moves[moves.Length - 1].X2] != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                    {
                                                        Array.Resize(ref moves, moves.Length - 1);
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                            }
                                            else
                                            {
                                                s = new bool[Squares.Length][];
                                                s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                            }
                                            n++;
                                        }
                                    }
                                    else
                                    {
                                        n = 1;
                                        while (n <= Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].Repetition && IsEmptySquare(x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY)))
                                        {
                                            n++;
                                        }
                                        if (n <= Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].Repetition && IsForeignSquare(team, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY)))
                                        {
                                            Array.Resize(ref moves, moves.Length + 1);
                                            moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY));
                                            if (s != null)
                                            {
                                                if (s[moves[moves.Length - 1].X2] != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                    {
                                                        Array.Resize(ref moves, moves.Length - 1);
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                            }
                                            else
                                            {
                                                s = new bool[Squares.Length][];
                                                s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                            }
                                            n++;
                                        }
                                    }
                                    k++;
                                }
                            }
                            else
                            {
                                d = 0;
                                while (d < 4)
                                {
                                    k = 0;
                                    while (k < Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves.Length)
                                    {
                                        if (Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].Repetition == 0)
                                        {
                                            n = 1;
                                            while (IsForeignSquare(team, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY)))
                                            {
                                                Array.Resize(ref moves, moves.Length + 1);
                                                moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY));
                                                if (s != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2] != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                        {
                                                            Array.Resize(ref moves, moves.Length - 1);
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s = new bool[Squares.Length][];
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                                n++;
                                            }
                                        }
                                        else
                                        {
                                            n = 1;
                                            while (n <= Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Movement].Moves[k].Repetition && IsForeignSquare(team, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY)))
                                            {
                                                Array.Resize(ref moves, moves.Length + 1);
                                                moves[moves.Length - 1] = new Move(x, y, x + GetOffsetX(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY), y + GetOffsetY(d, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetX, n * Storage.Moves[Storage.Chessmen[Squares[x][y].Chessman].Capturing].Moves[k].OffsetY));
                                                if (s != null)
                                                {
                                                    if (s[moves[moves.Length - 1].X2] != null)
                                                    {
                                                        if (s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2])
                                                        {
                                                            Array.Resize(ref moves, moves.Length - 1);
                                                        }
                                                        else
                                                        {
                                                            s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                        s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                    }
                                                }
                                                else
                                                {
                                                    s = new bool[Squares.Length][];
                                                    s[moves[moves.Length - 1].X2] = new bool[Squares[moves[moves.Length - 1].X2].Length];
                                                    s[moves[moves.Length - 1].X2][moves[moves.Length - 1].Y2] = true;
                                                }
                                                n++;
                                            }
                                        }
                                        k++;
                                    }
                                    d++;
                                }
                                d = 0;
                            }
                        }
                    }
                    j++;
                }
                i++;
            }
            return moves;
        }

        public Move[] GetAllPossibleMoves()
        {
            return GetAllPossibleMoves(Team);
        }

        public Move[] GetAllPossibleMoves(bool Team)
        {
            int team = 0;
            if (Team)
            {
                team = 1;
            }
            else
            {
                team = 2;
            }
            Move[] moves = GetAllMoves(team, true);
            Move[] possibleMoves = new Move[0];
            int i = 0;
            while (i < moves.Length)
            {
                if (!new Position(this, moves[i]).IsChecked(team))
                {
                    Array.Resize(ref possibleMoves, possibleMoves.Length + 1);
                    possibleMoves[possibleMoves.Length - 1] = new Move(moves[i]);
                }
                i++;
            }
            if (Maximum >= 2)
            {
                bool a;
                int f = -1;
                i = 0;
                int j;
                int k;
                while (i < Storage.PreviousPositions[Number].Length && f < 0)
                {
                    if (RelativeValue == Storage.PreviousPositions[Number][i].RelativeValue)
                    {
                        a = true;
                        j = 0;
                        while (j < Squares.Length && a)
                        {
                            k = 0;
                            while (k < Squares[j].Length && a)
                            {
                                if (Squares[j][k].Team != Storage.PreviousPositions[Number][i].Squares[j][k].Team || Squares[j][k].Chessman != Storage.PreviousPositions[Number][i].Squares[j][k].Chessman)
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
                    if (Main)
                    {
                        if (Storage.PreviousPositions[Number][f].Count >= Maximum)
                        {
                            return new Move[0];
                        }
                        else
                        {
                            return possibleMoves;
                        }
                    }
                    else
                    {
                        if (Storage.PreviousPositions[Number][f].Count >= Maximum - 1)
                        {
                            return new Move[0];
                        }
                        else
                        {
                            return possibleMoves;
                        }
                    }
                }
                else
                {
                    return possibleMoves;
                }
            }
            else
            {
                return possibleMoves;
            }
        }

        public ResultMove GetTheBestMove()
        {
            Move[] moves = GetAllPossibleMoves();
            Random randomizer = new Random();
            int random = 0;
            if (Inaccuracy > 0)
            {
                random = randomizer.Next(2 * Inaccuracy) - Inaccuracy;
            }
            int m = 0;
            int r = new Position(this, moves[0]).GetEvaluation() + random;
            int i = 0;
            if (Team)
            {
                while (i < moves.Length)
                {
                    if (Inaccuracy > 0)
                    {
                        random = randomizer.Next(2 * Inaccuracy) - Inaccuracy;
                    }
                    if (r < new Position(this, moves[i]).GetEvaluation() + random)
                    {
                        m = i;
                        r = new Position(this, moves[m]).GetEvaluation() + random;
                    }
                    i++;
                }
            }
            else
            {
                while (i < moves.Length)
                {
                    if (Inaccuracy > 0)
                    {
                        random = randomizer.Next(2 * Inaccuracy) - Inaccuracy;
                    }
                    if (r > new Position(this, moves[i]).GetEvaluation() + random)
                    {
                        m = i;
                        r = new Position(this, moves[m]).GetEvaluation() + random;
                    }
                    i++;
                }
            }
            if (Inaccuracy > 0)
            {
                random = randomizer.Next(2 * Inaccuracy) - Inaccuracy;
            }
            return new ResultMove(moves[m], new Position(this, moves[m]).GetEvaluation() + random);
        }

        public ResultMove[] GetTheBestMoves(int n)
        {
            Move[] moves = GetAllPossibleMoves();
            ResultMove[] resultMoves = new ResultMove[1];
            ResultMove resultMove;
            resultMoves[0] = new ResultMove(moves[0], new Position(this, moves[0]).GetEvaluation());
            int i = 0;
            int j;
            int k;
            bool b;
            if (Team)
            {
                while (i < moves.Length)
                {
                    if (resultMoves.Length >= n)
                    {
                        if (resultMoves[n - 1].Result < new Position(this, moves[i]).GetEvaluation())
                        {
                            resultMoves[n - 1] = new ResultMove(moves[i], new Position(this, moves[i]).GetEvaluation());
                            j = resultMoves.Length - 1;
                            k = resultMoves.Length - 2;
                            b = true;
                            while (k >= 0 && b)
                            {
                                if (resultMoves[j].Result > resultMoves[k].Result)
                                {
                                    resultMove = new ResultMove(resultMoves[j]);
                                    resultMoves[j] = new ResultMove(resultMoves[k]);
                                    resultMoves[k] = new ResultMove(resultMove);
                                    j--;
                                }
                                else
                                {
                                    b = false;
                                }
                                k--;
                            }
                        }
                    }
                    else
                    {
                        Array.Resize(ref resultMoves, resultMoves.Length + 1);
                        resultMoves[resultMoves.Length - 1] = new ResultMove(moves[i], new Position(this, moves[i]).GetEvaluation());
                        j = resultMoves.Length - 1;
                        k = resultMoves.Length - 2;
                        b = true;
                        while (k >= 0 && b)
                        {
                            if (resultMoves[j].Result > resultMoves[k].Result)
                            {
                                resultMove = new ResultMove(resultMoves[j]);
                                resultMoves[j] = new ResultMove(resultMoves[k]);
                                resultMoves[k] = new ResultMove(resultMove);
                                j--;
                            }
                            else
                            {
                                b = false;
                            }
                            k--;
                        }
                    }
                    i++;
                }
            }
            else
            {
                while (i < moves.Length)
                {
                    if (resultMoves.Length >= n)
                    {
                        if (resultMoves[n - 1].Result > new Position(this, moves[i]).GetEvaluation())
                        {
                            resultMoves[n - 1] = new ResultMove(moves[i], new Position(this, moves[i]).GetEvaluation());
                            j = resultMoves.Length - 1;
                            k = resultMoves.Length - 2;
                            b = true;
                            while (k >= 0 && b)
                            {
                                if (resultMoves[j].Result < resultMoves[k].Result)
                                {
                                    resultMove = new ResultMove(resultMoves[j]);
                                    resultMoves[j] = new ResultMove(resultMoves[k]);
                                    resultMoves[k] = new ResultMove(resultMove);
                                    j--;
                                }
                                else
                                {
                                    b = false;
                                }
                                k--;
                            }
                        }
                    }
                    else
                    {
                        Array.Resize(ref resultMoves, resultMoves.Length + 1);
                        resultMoves[resultMoves.Length - 1] = new ResultMove(moves[i], new Position(this, moves[i]).GetEvaluation());
                        j = resultMoves.Length - 1;
                        k = resultMoves.Length - 2;
                        b = true;
                        while (k >= 0 && b)
                        {
                            if (resultMoves[j].Result < resultMoves[k].Result)
                            {
                                resultMove = new ResultMove(resultMoves[j]);
                                resultMoves[j] = new ResultMove(resultMoves[k]);
                                resultMoves[k] = new ResultMove(resultMove);
                                j--;
                            }
                            else
                            {
                                b = false;
                            }
                            k--;
                        }
                    }
                    i++;
                }
            }
            return resultMoves;
        }

        public ResultMove GetTheBestMove(int n)
        {
            Move[] moves = GetAllPossibleMoves();
            if (n > 0 && moves.Length > 0)
            {
                if (n > 1)
                {
                    int m = 0;
                    int r = new Position(this, moves[0]).GetTheBestMove().Result;
                    int i = 0;
                    if (Team)
                    {
                        while (i < moves.Length)
                        {
                            if (r < new Position(this, moves[i]).GetTheBestMove(n - 1).Result)
                            {
                                m = i;
                                r = new Position(this, moves[m]).GetTheBestMove(n - 1).Result;
                            }
                            i++;
                        }
                        return new ResultMove(moves[m], new Position(this, moves[m]).GetTheBestMove(n - 1).Result);
                    }
                    else
                    {
                        while (i < moves.Length)
                        {
                            if (r > new Position(this, moves[i]).GetTheBestMove(n - 1).Result)
                            {
                                m = i;
                                r = new Position(this, moves[m]).GetTheBestMove(n - 1).Result;
                            }
                            i++;
                        }
                        return new ResultMove(moves[m], new Position(this, moves[m]).GetTheBestMove(n - 1).Result);
                    }
                }
                else
                {
                    return GetTheBestMove();
                }
            }
            else
            {
                return null;
            }
        }
    }
}
