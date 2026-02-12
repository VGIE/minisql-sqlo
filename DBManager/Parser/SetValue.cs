using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace DbManager.Parser
{
    public class SetValue
    {
        public string ColumnName { get; private set; }
        public string Value { get; private set; }


        public SetValue(string column, string value)
        {
            //TODO DEADLINE 1A: Initialize member variables
            ColumnName = column;
            Value = value;
        }
    }
}
