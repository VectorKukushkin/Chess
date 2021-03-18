using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Chess
{
    class Storage
    {
        public static Board[] Boards = new Board[0];
        public static Position[] Positions = new Position[0];
        public static SetOfMoves[] Moves = new SetOfMoves[0];
        public static SpecialMove[] SpecialMoves = new SpecialMove[0];
        public static Chessman[] Chessmen = new Chessman[0];

        public static SpecialAction[] SpecialActions = new SpecialAction[1] { new SpecialAction() };
        public static string[] Attributes = new string[0];
        public static int[] ImportantChessmen = new int[0];
        public static int[] PromotionChessmen = new int[0];
        public static int[][] Promotions;
        public static Position[][] PreviousPositions = new Position[0][];

        public static void AddBoard(XmlNode xmlNode)
        {
            if (GetBoard(xmlNode.Attributes.GetNamedItem("name").Value) == -1)
            {
                Array.Resize(ref Boards, Boards.Length + 1);
                Boards[Boards.Length - 1] = new Board(xmlNode);
            }
        }

        public static void AddPosition(XmlNode xmlNode)
        {
            if (GetPosition(xmlNode.Attributes.GetNamedItem("name").Value) == -1)
            {
                Array.Resize(ref Positions, Positions.Length + 1);
                Positions[Positions.Length - 1] = new Position(xmlNode);
            }
        }

        public static void AddMove(XmlNode xmlNode)
        {
            if (GetSetOfMoves(xmlNode.Attributes.GetNamedItem("name").Value) == -1)
            {
                Array.Resize(ref Moves, Moves.Length + 1);
                Moves[Moves.Length - 1] = new SetOfMoves(xmlNode);
            }
        }

        public static void AddSpecialMove(XmlNode xmlNode)
        {
            if (GetSpecialMove(xmlNode.Attributes.GetNamedItem("name").Value).Length == 0)
            {
                Array.Resize(ref SpecialMoves, SpecialMoves.Length + 1);
                SpecialMoves[SpecialMoves.Length - 1] = new SpecialMove(xmlNode.Attributes.GetNamedItem("name").Value);
                SpecialMove specialMove = new SpecialMove(xmlNode);
                if (specialMove.SymmetryX || specialMove.SymmetryY)
                {
                    if (specialMove.SymmetryX && specialMove.SymmetryY)
                    {
                        Array.Resize(ref SpecialMoves, SpecialMoves.Length + 3);
                        SpecialMoves[SpecialMoves.Length - 1] = new SpecialMove(xmlNode.Attributes.GetNamedItem("name").Value);
                        SpecialMoves[SpecialMoves.Length - 2] = new SpecialMove(xmlNode.Attributes.GetNamedItem("name").Value);
                        SpecialMoves[SpecialMoves.Length - 3] = new SpecialMove(xmlNode.Attributes.GetNamedItem("name").Value);
                    }
                    else
                    {
                        if (specialMove.SymmetryX)
                        {
                            Array.Resize(ref SpecialMoves, SpecialMoves.Length + 1);
                            SpecialMoves[SpecialMoves.Length - 1] = new SpecialMove(xmlNode.Attributes.GetNamedItem("name").Value);
                        }
                        else
                        {
                            Array.Resize(ref SpecialMoves, SpecialMoves.Length + 1);
                            SpecialMoves[SpecialMoves.Length - 1] = new SpecialMove(xmlNode.Attributes.GetNamedItem("name").Value);
                        }
                    }
                }
            }
        }

        public static void AddChessman(XmlNode xmlNode)
        {
            if (GetSetOfMoves(xmlNode.Attributes.GetNamedItem("name").Value) == -1)
            {
                Array.Resize(ref Chessmen, Chessmen.Length + 1);
                Chessmen[Chessmen.Length - 1] = new Chessman(xmlNode);
                if (Chessmen[Chessmen.Length - 1].Checkmate)
                {
                    Array.Resize(ref ImportantChessmen, ImportantChessmen.Length + 1);
                    ImportantChessmen[ImportantChessmen.Length - 1] = Chessmen.Length - 1;
                }
            }
        }

        public static void AddSpecialAction(XmlNode xmlNode)
        {
            Array.Resize(ref SpecialActions, SpecialActions.Length + 1);
            SpecialActions[SpecialActions.Length - 1] = new SpecialAction(xmlNode);
        }

        public static void AddSpecialAction(SpecialAction specialAction)
        {
            Array.Resize(ref SpecialActions, SpecialActions.Length + 1);
            SpecialActions[SpecialActions.Length - 1] = specialAction;
        }

        public static void AddSpecialAction(SpecialAction specialAction, int[] b)
        {
            Array.Resize(ref SpecialActions, SpecialActions.Length + 1);
            SpecialActions[SpecialActions.Length - 1] = new SpecialAction(specialAction, b);
        }

        public static void AddAttribute(string attribute)
        {
            if (GetAttribute(attribute) == -1)
            {
                Array.Resize(ref Attributes, Attributes.Length + 1);
                Attributes[Attributes.Length - 1] = attribute;
            }
        }

        public static void LoadSpecialMoves(XmlNode[] xmlNodes)
        {
            int i = 0;
            int j = 0;
            while (i < SpecialMoves.Length && j < xmlNodes.Length)
            {
                SpecialMoves[i] = new SpecialMove(xmlNodes[j]);
                if (SpecialMoves[i].SymmetryX || SpecialMoves[i].SymmetryY)
                {
                    if (SpecialMoves[i].SymmetryX && SpecialMoves[i].SymmetryY)
                    {
                        SpecialMoves[i + 1] = new SpecialMove(SpecialMoves[i], true, false);
                        SpecialMoves[i + 2] = new SpecialMove(SpecialMoves[i], false, true);
                        SpecialMoves[i + 3] = new SpecialMove(SpecialMoves[i], true, true);
                        i = i + 3;
                    }
                    else
                    {
                        if (SpecialMoves[i].SymmetryX)
                        {
                            SpecialMoves[i + 1] = new SpecialMove(SpecialMoves[i], true, false);
                            i++;
                        }
                        else
                        {
                            SpecialMoves[i + 1] = new SpecialMove(SpecialMoves[i], false, true);
                            i++;
                        }
                    }
                }
                i++;
                j++;
            }
        }

        public static int GetBoard(string name)
        {
            bool f = false;
            int i = 0;
            while (!f && i < Boards.Length)
            {
                if (Boards[i] != null)
                {
                    if (Boards[i].Name == name)
                    {
                        f = true;
                        i--;
                    }
                }
                i++;
            }
            if (f)
            {
                return i;
            }
            else
            {
                return -1;
            }
        }

        public static int GetPosition(string name)
        {
            bool f = false;
            int i = 0;
            while (!f && i < Positions.Length)
            {
                if (Positions[i] != null)
                {
                    if (Positions[i].PositionName == name)
                    {
                        f = true;
                        i--;
                    }
                }
                i++;
            }
            if (f)
            {
                return i;
            }
            else
            {
                return -1;
            }
        }

        public static int GetSetOfMoves(string name)
        {
            bool f = false;
            int i = 0;
            while (!f && i < Moves.Length)
            {
                if (Moves[i] != null)
                {
                    if (Moves[i].Name == name)
                    {
                        f = true;
                        i--;
                    }
                }
                i++;
            }
            if (f)
            {
                return i;
            }
            else
            {
                return -1;
            }
        }

        public static int[] GetSpecialMove(string name)
        {
            int[] s = new int[0];
            int i = 0;
            while (i < SpecialMoves.Length)
            {
                if (SpecialMoves[i] != null)
                {
                    if (SpecialMoves[i].Name == name)
                    {
                        Array.Resize(ref s, s.Length + 1);
                        s[s.Length - 1] = i;
                    }
                }
                i++;
            }
            return s;
        }

        public static int GetChessman(string name)
        {
            bool f = false;
            int i = 0;
            while (!f && i < Chessmen.Length)
            {
                if (Chessmen[i] != null)
                {
                    if (Chessmen[i].Name == name)
                    {
                        f = true;
                        i--;
                    }
                }
                i++;
            }
            if (f)
            {
                return i;
            }
            else
            {
                return -1;
            }
        }

        public static int GetAttribute(string name)
        {
            bool f = false;
            int i = 0;
            while (!f && i < Attributes.Length)
            {
                if (Attributes[i] != null)
                {
                    if (Attributes[i] == name)
                    {
                        f = true;
                        i--;
                    }
                }
                i++;
            }
            if (f)
            {
                return i;
            }
            else
            {
                return -1;
            }
        }
    }
}
