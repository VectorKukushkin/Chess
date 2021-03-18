using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    class ResultMove : Move
    {
        public ResultMove(Move move) : base(move)
        {

        }

        public ResultMove(Move move, int result) : base(move)
        {
            Result = result;
        }

        public int Result;
    }
}
