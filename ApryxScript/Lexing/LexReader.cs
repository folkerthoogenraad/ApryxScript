using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ApryxScript.Lexing
{
    public class LexReader : StreamReader
    {
        public int Index = 0;
        public int Line = 1;
        private char _Current = '\0';

        public LexReader(Stream stream) : base(stream)
        {
        }

        public char Current
        {
            get
            {
                return _Current;
            }
        }

        public bool HasNext()
        {
            return !EndOfStream;
        }

        public char Next()
        {
            int result = Read();
            Index++;

            if (result < 0) _Current = '\0';
            else _Current = (char) result;

            if (_Current == '\n') Line++;

            return _Current;
        }
    }
}
