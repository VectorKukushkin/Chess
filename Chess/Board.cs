using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Chess
{
    class Board
    {
        public Board()
        {
            
        }

        public Board(XmlNode xmlNode)
        {
            Name = xmlNode.Attributes.GetNamedItem("name").Value;
            XmlNode[] Attributes = new XmlNode[0];
            Squares = new Square[0][];
            bool v = false;
            int x0;
            int y0;
            int x;
            int y;
            int w;
            int h = 0;
            int i = 0;
            int j;
            while (i < xmlNode.ChildNodes.Count)
            {
                switch (xmlNode.ChildNodes[i].Name)
                {
                    case "width":
                        Squares = new Square[Convert.ToInt32(xmlNode.ChildNodes[i].InnerText)][];
                        break;
                    case "height":
                        h = Convert.ToInt32(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "special":
                        j = 0;
                        while (j < xmlNode.ChildNodes[i].ChildNodes.Count)
                        {
                            Storage.AddAttribute(xmlNode.ChildNodes[i].ChildNodes[j].Attributes.GetNamedItem("name").Value);
                            Array.Resize(ref Attributes, Attributes.Length + 1);
                            Attributes[Attributes.Length - 1] = xmlNode.ChildNodes[i].ChildNodes[j];
                            j++;
                        }
                        break;
                    default:

                        break;
                }
                i++;
            }
            i = 0;
            while (i < Squares.Length)
            {
                Squares[i] = new Square[h];
                j = 0;
                while (j < h)
                {
                    Squares[i][j] = new Square()
                    {
                        Name = Convert.ToChar(97 + i) + Convert.ToString(j + 1),
                        Types = new bool[Storage.Attributes.Length]
                    };
                    j++;
                }
                i++;
            }
            AttributesX = new int[Storage.Attributes.Length][];
            AttributesY = new int[AttributesX.Length][];
            i = 0;
            while (i < AttributesX.Length)
            {
                AttributesX[i] = new int[0];
                AttributesY[i] = new int[0];
                i++;
            }
            i = 0;
            while (i < Attributes.Length)
            {
                v = false;
                x0 = 0;
                y0 = 0;
                w = 0;
                h = 0;
                j = 0;
                while (j < Attributes[i].ChildNodes.Count)
                {
                    switch (Attributes[i].ChildNodes[j].Name)
                    {
                        case "value":
                            v = Convert.ToBoolean(Attributes[i].ChildNodes[j].InnerText);
                            break;
                        case "x1":
                            x0 = Convert.ToInt32(Attributes[i].ChildNodes[j].InnerText);
                            break;
                        case "y1":
                            y0 = Convert.ToInt32(Attributes[i].ChildNodes[j].InnerText);
                            break;
                        case "x2":
                            w = Convert.ToInt32(Attributes[i].ChildNodes[j].InnerText);
                            break;
                        case "y2":
                            h = Convert.ToInt32(Attributes[i].ChildNodes[j].InnerText);
                            break;
                        default:

                            break;
                    }
                    j++;
                }
                j = Storage.GetAttribute(Attributes[i].Attributes.GetNamedItem("name").Value);
                x = Math.Min(x0, w);
                w = Math.Max(x0, w);
                x0 = x;
                y = Math.Min(y0, h);
                h = Math.Max(y0, h);
                y0 = y;
                while (x <= w)
                {
                    y = y0;
                    while (y <= h)
                    {
                        Squares[x][y].Types[j] = v;
                        Array.Resize(ref AttributesX[j], AttributesX[j].Length + 1);
                        Array.Resize(ref AttributesY[j], AttributesX[j].Length);
                        AttributesX[j][AttributesX[j].Length - 1] = x;
                        AttributesY[j][AttributesX[j].Length - 1] = y;

                        y++;
                    }
                    x++;
                }
                i++;
            }
        }

        public Board(Board board)
        {
            int i = 0;
            int j;
            Squares = new Square[board.Squares.Length][];
            while (i < board.Squares.Length)
            {
                Squares[i] = new Square[board.Squares[i].Length];
                j = 0;
                while (j < board.Squares[i].Length)
                {
                    Squares[i][j] = new Square(board.Squares[i][j]);
                    j++;
                }
                i++;
            }
            AttributesX = new int[board.AttributesX.Length][];
            AttributesY = new int[AttributesX.Length][];
            i = 0;
            while (i < AttributesX.Length)
            {
                AttributesX[i] = new int[board.AttributesX[i].Length];
                AttributesY[i] = new int[AttributesX[i].Length];
                j = 0;
                while (j < AttributesX[i].Length)
                {
                    AttributesX[i][j] = board.AttributesX[i][j];
                    AttributesY[i][j] = board.AttributesY[i][j];
                    j++;
                }
                i++;
            }
        }

        public string Name;
        public Square[][] Squares;
        public int[][] AttributesX;
        public int[][] AttributesY;
    }
}
