using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    class Square
    {
        public Square()
        {
            Types = new bool[0];
        }

        public Square(Square square)
        {
            Name = square.Name;
            Chessman = square.Chessman;
            Team = square.Team;
            Number = square.Number;
            Types = new bool[square.Types.Length];
            int i = 0;
            while (i < square.Types.Length)
            {
                Types[i] = square.Types[i];
                i++;
            }
        }

        public string Name;
        public int Chessman;
        public int Team;
        public int Number;
        public bool[] Types;
    }
}
