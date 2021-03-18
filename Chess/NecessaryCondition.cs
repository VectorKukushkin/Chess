using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Chess
{
    class NecessaryCondition
    {
        public NecessaryCondition(XmlNode xmlNode)
        {
            Attributes = new int[0];
            Chessman = -1;
            int i = 0;
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
            while (i < xmlNode.ChildNodes.Count)
            {
                switch (xmlNode.ChildNodes[i].Name)
                {
                    case "empty":
                        IsEmpty = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        IsOccupied = !Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "moved":
                        IsMoved = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        IsStood = !Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "checked":
                        IsChecked = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        IsUnchecked = !Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "ownTeam":
                        IsOwnTeam = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        IsOtherTeam = !Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "chessman":
                        Chessman = Storage.GetChessman(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "attribute":
                        Array.Resize(ref Attributes, Attributes.Length + 1);
                        Attributes[Attributes.Length - 1] = Storage.GetAttribute(xmlNode.ChildNodes[i].InnerText);
                        break;
                    default:

                        break;
                }
                i++;
            }
        }

        public NecessaryCondition(NecessaryCondition necessaryCondition, bool x, bool y)
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
            Relative = necessaryCondition.Relative;
            if (Relative)
            {
                OffsetX = a * necessaryCondition.OffsetX;
                OffsetY = b * necessaryCondition.OffsetY;
            }
            else
            {
                OffsetX = necessaryCondition.OffsetX;
                OffsetY = necessaryCondition.OffsetY;
            }
            IsEmpty = necessaryCondition.IsEmpty;
            IsOccupied = necessaryCondition.IsOccupied;
            IsMoved = necessaryCondition.IsMoved;
            IsStood = necessaryCondition.IsStood;
            IsChecked = necessaryCondition.IsChecked;
            IsUnchecked = necessaryCondition.IsUnchecked;
            IsOwnTeam = necessaryCondition.IsOwnTeam;
            IsOtherTeam = necessaryCondition.IsOtherTeam;
            Chessman = necessaryCondition.Chessman;
            Attributes = new int[necessaryCondition.Attributes.Length];
            int i = 0;
            while (i < Attributes.Length)
            {
                Attributes[i] = necessaryCondition.Attributes[i];
                i++;
            }
        }

        public bool Relative;
        public int OffsetX;
        public int OffsetY;
        public bool IsEmpty;
        public bool IsOccupied;
        public bool IsMoved;
        public bool IsStood;
        public bool IsChecked;
        public bool IsUnchecked;
        public bool IsOwnTeam;
        public bool IsOtherTeam;
        public int Chessman;
        public int[] Attributes;
    }
}
