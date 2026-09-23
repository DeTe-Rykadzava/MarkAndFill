using System;
using System.Collections.Generic;
using System.Text;

namespace MarkAndFill.Model
{
    public class FileTempGroup
    {
        public static FileTempGroup BaseGroup => new FileTempGroup { GroupName = "Без группы" };

        public string GroupName { get; set; } = string.Empty;

        public FileTempGroup(string groupName)
        {
            GroupName = groupName;
        }

        private FileTempGroup()
        {
            
        }
    }
}
