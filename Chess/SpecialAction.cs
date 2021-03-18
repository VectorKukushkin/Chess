using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Chess
{
    class SpecialAction
    {
        public SpecialAction()
        {
            IsMoveRelative = new bool[0];
            Moves = new Move[0];
            SquareActions = new SquareAction[0];
        }

        public SpecialAction(XmlNode xmlNode)
        {
            int i = 0;
            int j;
            while (i < xmlNode.ChildNodes.Count)
            {
                switch (xmlNode.ChildNodes[i].Name)
                {
                    case "move":
                        Array.Resize(ref Moves, Moves.Length + 1);
                        Array.Resize(ref IsMoveRelative, Moves.Length);
                        Moves[Moves.Length - 1] = new Move(0, 0, 0, 0);
                        IsMoveRelative[Moves.Length - 1] = true;
                        j = 0;
                        while (j < xmlNode.ChildNodes[i].ChildNodes.Count)
                        {
                            switch (xmlNode.ChildNodes[i].ChildNodes[j].Name)
                            {
                                case "relative":
                                    IsMoveRelative[Moves.Length - 1] = Convert.ToBoolean(xmlNode.ChildNodes[i].ChildNodes[j].InnerText);
                                    break;
                                case "x1":
                                    Moves[Moves.Length - 1].X1 = Convert.ToInt32(xmlNode.ChildNodes[i].ChildNodes[j].InnerText);
                                    break;
                                case "y1":
                                    Moves[Moves.Length - 1].Y1 = Convert.ToInt32(xmlNode.ChildNodes[i].ChildNodes[j].InnerText);
                                    break;
                                case "x2":
                                    Moves[Moves.Length - 1].X2 = Convert.ToInt32(xmlNode.ChildNodes[i].ChildNodes[j].InnerText);
                                    break;
                                case "y2":
                                    Moves[Moves.Length - 1].Y2 = Convert.ToInt32(xmlNode.ChildNodes[i].ChildNodes[j].InnerText);
                                    break;
                                default:

                                    break;
                            }
                            j++;
                        }
                        break;
                    case "square":
                        Array.Resize(ref SquareActions, SquareActions.Length + 1);
                        SquareActions[SquareActions.Length - 1] = new SquareAction(xmlNode.ChildNodes[i]);
                        break;
                    default:

                        break;
                }
                i++;
            }
        }

        public SpecialAction(SpecialAction specialAction, int[] b)
        {
            int i = 0;
            IsMoveRelative = new bool[specialAction.IsMoveRelative.Length];
            Moves = new Move[specialAction.IsMoveRelative.Length];
            while (i < specialAction.IsMoveRelative.Length)
            {
                IsMoveRelative[i] = specialAction.IsMoveRelative[i];
                Moves[i] = new Move(specialAction.Moves[i]);
                i++;
            }
            i = 0;
            SquareActions = new SquareAction[specialAction.SquareActions.Length];
            while (i < SquareActions.Length)
            {
                SquareActions[i] = new SquareAction(specialAction.SquareActions[i], b[i]);
                i++;
            }
        }

        public SpecialAction(SpecialAction specialAction, bool x, bool y)
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
            int i = 0;
            IsMoveRelative = new bool[specialAction.IsMoveRelative.Length];
            Moves = new Move[specialAction.IsMoveRelative.Length];
            while (i < specialAction.IsMoveRelative.Length)
            {
                IsMoveRelative[i] = specialAction.IsMoveRelative[i];
                if (IsMoveRelative[i])
                {
                    Moves[i] = new Move(specialAction.Moves[i])
                    {
                        X1 = a * specialAction.Moves[i].X1,
                        X2 = a * specialAction.Moves[i].X2,
                        Y1 = b * specialAction.Moves[i].Y1,
                        Y2 = b * specialAction.Moves[i].Y2,
                    };
                }
                else
                {
                    Moves[i] = new Move(specialAction.Moves[i]);
                }
                i++;
            }
            i = 0;
            SquareActions = new SquareAction[specialAction.SquareActions.Length];
            while (i < SquareActions.Length)
            {
                SquareActions[i] = new SquareAction(specialAction.SquareActions[i], x, y);
                i++;
            }
        }

        public bool[] IsMoveRelative = new bool[0];
        public Move[] Moves = new Move[0];
        public SquareAction[] SquareActions = new SquareAction[0];
    }
}
