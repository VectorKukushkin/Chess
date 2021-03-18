using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Chess
{
    class SquareAction
    {
        public SquareAction(XmlNode xmlNode)
        {
            Chessman = -1;
            Chessmen = new int[0];
            if (xmlNode.Name == "this")
            {
                Relative = true;
            }
            else
            {
                if (xmlNode.Attributes.GetNamedItem("relative") != null)
                {
                    Relative = Convert.ToBoolean(xmlNode.Attributes.GetNamedItem("relative").Value);
                }
                else
                {
                    Relative = true;
                }
                if (xmlNode.Attributes.GetNamedItem("offsetX") != null)
                {
                    OffsetX = Convert.ToInt32(xmlNode.Attributes.GetNamedItem("offsetX").Value);
                }
                if (xmlNode.Attributes.GetNamedItem("offsetY") != null)
                {
                    OffsetY = Convert.ToInt32(xmlNode.Attributes.GetNamedItem("offsetY").Value);
                }
            }
            int i = 0;
            while (i < xmlNode.ChildNodes.Count)
            {
                switch (xmlNode.ChildNodes[i].Name)
                {
                    case "empty":
                        IsEmpty = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "ownTeam":
                        IsOwnTeam = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "chessman":
                        if (Chessman == -1)
                        {
                            Chessman = Storage.GetChessman(xmlNode.ChildNodes[i].InnerText);
                        }
                        else
                        {
                            Array.Resize(ref Chessmen, Chessmen.Length + 1);
                            Chessmen[Chessmen.Length - 1] = Storage.GetChessman(xmlNode.ChildNodes[i].InnerText);
                        }
                        break;
                    default:

                        break;
                }
                i++;
            }
        }

        public SquareAction(SquareAction squareAction, int a)
        {
            Relative = squareAction.Relative;
            OffsetX = squareAction.OffsetX;
            OffsetY = squareAction.OffsetY;
            IsEmpty = squareAction.IsEmpty;
            IsOwnTeam = squareAction.IsOwnTeam;
            Chessmen = new int[squareAction.Chessmen.Length];
            int i = 0;
            while (i < Chessmen.Length)
            {
                Chessmen[i] = squareAction.Chessmen[i];
                i++;
            }
            if (a == 0)
            {
                Chessman = squareAction.Chessman;
            }
            else
            {
                Chessman = Chessmen[a - 1];
            }
        }

        public SquareAction(SquareAction squareAction, bool x, bool y)
        {
            int a;
            int b;
            if (x)
            {
                a = -1;
            }
            else
            {
                a = 1;
            }
            if (y)
            {
                b = -1;
            }
            else
            {
                b = 1;
            }
            Relative = squareAction.Relative;
            if (Relative)
            {
                OffsetX = a * squareAction.OffsetX;
                OffsetY = b * squareAction.OffsetY;
            }
            else
            {
                OffsetX = squareAction.OffsetX;
                OffsetY = squareAction.OffsetY;
            }
            IsEmpty = squareAction.IsEmpty;
            IsOwnTeam = squareAction.IsOwnTeam;
            Chessman = squareAction.Chessman;
            Chessmen = new int[squareAction.Chessmen.Length];
            int i = 0;
            while (i < Chessmen.Length)
            {
                Chessmen[i] = squareAction.Chessmen[i];
                i++;
            }
        }

        public bool Relative;
        public int OffsetX;
        public int OffsetY;
        public bool IsEmpty;
        public bool IsOwnTeam;
        public int Chessman;
        public int[] Chessmen;
    }
}
