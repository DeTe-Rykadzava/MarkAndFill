using System;
using System.Collections.Generic;
using System.Text;

namespace MarkAndFill.Model
{
    public class Mark
    {
        public string TagName { get; set; }
        public string? TagValue { get; set; }

        public Mark(string tagName, string tagValue)
        {
            TagName = tagName;
            TagValue = tagValue;
        }

        public Mark(string tagName) 
        {
            TagName = tagName;
        }

    }
}
