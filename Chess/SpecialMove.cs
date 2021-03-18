using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Chess
{
    class SpecialMove
    {
        public SpecialMove(string name)
        {
            Name = name;
        }

        public SpecialMove(XmlNode xmlNode)
        {
            Name = xmlNode.Attributes.GetNamedItem("name").Value;
            SpecialActions = new int[1] { 0 };
            Conditions = new NecessaryCondition[0];
            int i = 0;
            int j;
            int k;
            while (i < xmlNode.ChildNodes.Count)
            {
                switch (xmlNode.ChildNodes[i].Name)
                {
                    case "movement":
                        Movement = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "capturing":
                        Capturing = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "direction":
                        Direction = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "symmetryX":
                        SymmetryX = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "symmetryY":
                        SymmetryY = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "repetition":
                        Repetition = Convert.ToInt32(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "offsetX":
                        OffsetX = Convert.ToInt32(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "offsetY":
                        OffsetY = Convert.ToInt32(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "necessary":
                        j = 0;
                        while (j < xmlNode.ChildNodes[i].ChildNodes.Count)
                        {
                            if (xmlNode.ChildNodes[i].ChildNodes[j].Name != "lastMove")
                            {
                                if (xmlNode.ChildNodes[i].ChildNodes[j].Name == "this" || xmlNode.ChildNodes[i].ChildNodes[j].Name == "square")
                                {
                                    Array.Resize(ref Conditions, Conditions.Length + 1);
                                    Conditions[Conditions.Length - 1] = new NecessaryCondition(xmlNode.ChildNodes[i].ChildNodes[j]);
                                }
                            }
                            else
                            {
                                k = 0;
                                LastMove = new Move(0, 0, 0, 0);
                                IsLastMoveRelative = true;
                                while (k < xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes.Count)
                                {
                                    switch (xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name)
                                    {
                                        case "relative":
                                            IsLastMoveRelative = Convert.ToBoolean(xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                            break;
                                        case "x1":
                                            LastMove.X1 = Convert.ToInt32(xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                            break;
                                        case "y1":
                                            LastMove.Y1 = Convert.ToInt32(xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                            break;
                                        case "x2":
                                            LastMove.X2 = Convert.ToInt32(xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                            break;
                                        case "y2":
                                            LastMove.Y2 = Convert.ToInt32(xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                            break;
                                        default:

                                            break;
                                    }
                                    k++;
                                }
                            }
                            j++;
                        }
                        break;
                    case "action":
                        SpecialActions[0] = Storage.SpecialActions.Length;
                        Storage.AddSpecialAction(xmlNode.ChildNodes[i]);
                        break;
                    default:

                        break;
                }
                i++;
            }
            if (Storage.SpecialActions[SpecialActions[0]].SquareActions.Length > 0)
            {
                int[] a = new int[Storage.SpecialActions[SpecialActions[0]].SquareActions.Length];
                int[] b = new int[Storage.SpecialActions[SpecialActions[0]].SquareActions.Length];
                i = 0;
                while (i < a.Length)
                {
                    a[i] = Storage.SpecialActions[SpecialActions[0]].SquareActions[i].Chessmen.Length + 1;
                    b[i] = 0;
                    i++;
                }
                i = b.Length - 1;
                b[i]++;
                while (b[i] >= a[i] && i > 0)
                {
                    b[i] = 0;
                    i--;
                    b[i]++;
                }
                while (b[0] < a[0])
                {
                    Array.Resize(ref SpecialActions, SpecialActions.Length + 1);
                    SpecialActions[SpecialActions.Length - 1] = Storage.SpecialActions.Length;
                    Storage.AddSpecialAction(Storage.SpecialActions[SpecialActions[0]], b);
                    i = b.Length - 1;
                    b[i]++;
                    while (b[i] >= a[i] && i > 0)
                    {
                        b[i] = 0;
                        i--;
                        b[i]++;
                    }
                }
            }
        }

        public SpecialMove(SpecialMove specialMove, bool x, bool y)
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
            OffsetX = a * specialMove.OffsetX;
            OffsetY = b * specialMove.OffsetY;
            Name = specialMove.Name;
            Capturing = specialMove.Capturing;
            Movement = specialMove.Movement;
            Direction = specialMove.Direction;
            Repetition = specialMove.Repetition;
            IsLastMoveRelative = specialMove.IsLastMoveRelative;
            SpecialActions = new int[specialMove.SpecialActions.Length];
            int i = 0;
            while (i < SpecialActions.Length)
            {
                SpecialActions[i] = Storage.SpecialActions.Length;
                Storage.AddSpecialAction(new SpecialAction(Storage.SpecialActions[specialMove.SpecialActions[i]], x, y));
                i++;
            }
            if (specialMove.LastMove != null)
            {
                LastMove = new Move(specialMove.LastMove)
                {
                    X1 = a * specialMove.LastMove.X1,
                    X2 = a * specialMove.LastMove.X2,
                    Y1 = b * specialMove.LastMove.Y1,
                    Y2 = b * specialMove.LastMove.Y2,
                };
            }
            Conditions = new NecessaryCondition[specialMove.Conditions.Length];
            i = 0;
            while (i < Conditions.Length)
            {
                Conditions[i] = new NecessaryCondition(specialMove.Conditions[i], x, y);
                i++;
            }
        }

        public string Name;
        public bool Capturing;
        public bool Movement;
        public bool Direction;
        public bool SymmetryX;
        public bool SymmetryY;
        public int Repetition;
        public int OffsetX;
        public int OffsetY;
        public bool IsLastMoveRelative;
        public int[] SpecialActions;
        public Move LastMove;
        public NecessaryCondition[] Conditions;
    }
}
