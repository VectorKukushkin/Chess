using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    class Piece
    {
        public Piece(int x, int y, int dir)
        {
            CoordinateX = x;
            CoordinateY = y;
            Direction = dir;
            Moved = false;
        }

        public Piece(Piece piece)
        {
            CoordinateX = piece.CoordinateX;
            CoordinateY = piece.CoordinateY;
            Direction = piece.Direction;
            Moved = piece.Moved;
        }

        public int CoordinateX;
        public int CoordinateY;
        public int Direction;
        public bool Moved;
    }
}
