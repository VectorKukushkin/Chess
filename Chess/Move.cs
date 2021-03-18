using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    class Move
    {
        public Move(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Special = -1;
            Direction = 0;
        }

        public Move(int x1, int y1, int x2, int y2, int special, int direction)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Special = special;
            Direction = direction;
        }

        public Move(Move move)
        {
            X1 = move.X1;
            Y1 = move.Y1;
            X2 = move.X2;
            Y2 = move.Y2;
            Special = move.Special;
            Direction = move.Direction;
        }

        public Move(Move move, int d, int x, int y)
        {
            X1 = GetOffsetX(d, move.X1, move.Y1) + x;
            Y1 = GetOffsetY(d, move.X1, move.Y1) + y;
            X2 = GetOffsetX(d, move.X2, move.Y2) + x;
            Y2 = GetOffsetY(d, move.X2, move.Y2) + y;
            Special = move.Special;
            Direction = move.Direction;
        }

        public int X1;
        public int Y1;
        public int X2;
        public int Y2;
        public int Special;
        public int Direction;

        public bool IsMovesSame(Move move)
        {
            if (move != null)
            {
                if (X1 == move.X1 && Y1 == move.Y1 && X2 == move.X2 && Y2 == move.Y2)
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
                return true;
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
    }
}
