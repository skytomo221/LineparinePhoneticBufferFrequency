using System;
using System.Collections.Generic;
using System.Text;

namespace LineparinePhoneticBufferFrequency
{
    public class Example
    {
        public string Word { get; set; }
        public List<string> Decomposition { get; set; }
        public int FastLetterIndex { get; set; }
        public int LastLetterIndex { get; set; }
    }
}
