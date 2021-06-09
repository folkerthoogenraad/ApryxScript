using System;
using System.Collections.Generic;
using System.Text;

namespace ApryxScript.Typing
{
    public class ApryxNamespace
    {
        public List<ApryxType> Types = new List<ApryxType>();
    }

    public class ApryxVariable
    {
        public string Name;
        public ApryxType Type;
    }

    public class ApryxFunction
    {
        public string Name;
        public List<ApryxVariable> Parameters = new List<ApryxVariable>();
    }

    public class ApryxType
    {
        public string Name;
        public List<ApryxVariable> Variables = new List<ApryxVariable>();
        public List<ApryxFunction> Functions = new List<ApryxFunction>();
    }
}
