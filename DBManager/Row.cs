using DbManager.Parser;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DbManager
{
    public class Row
    {
        private List<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        public List<string> Values { get; set; }

        public Row(List<ColumnDefinition> columnDefinitions, List<string> values)
        {
            //TODO DEADLINE 1.A: Initialize member variables
            ColumnDefinitions = columnDefinitions;
            Values = new List<string>();

            for (int i = 0; i < values.Count; i++)
            {
                Values.Add(Encode(values[i]));
            }

        }

        public void SetValue(string columnName, string value)
        {
            //TODO DEADLINE 1.A: Given a column name and value, change the value in that column
            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions.Count != Values.Count)
                {
                    Values.Add(null);
                }

                if (ColumnDefinitions[i].Name == columnName)
                {
                    
                    Values[i] = Encode(value);
                }
            }

        }

        public string GetValue(string columnName)
        {
            //TODO DEADLINE 1.A: Given a column name, return the value in that column
            int i = 0;
            foreach (var item in ColumnDefinitions)
            {
                if (item.Name == columnName)
                {
                    return Decode(Values[i]);
                }
                i++;
            }

            return null;

        }

        public bool IsTrue(Condition condition)
        {

            string nombreCol = condition.ColumnName;
            string a = GetValue(nombreCol);

            if (a == null)
                return false;

            ColumnDefinition.DataType type = ColumnDefinition.DataType.String;

            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions[i].Name == nombreCol)
                {
                    type = ColumnDefinitions[i].Type;
                    break;
                }
            }

            //TODO DEADLINE 1.A: Given a condition (column name, operator and literal value, return whether it is true or not
            //for this row. Check Condition.IsTrue method

            return condition.IsTrue(a, type);

        }

        private const string Delimiter = ":";
        private const string DelimiterEncoded = "[SEPARATOR]";

        private static string Encode(string value)
        {
            //TODO DEADLINE 1.C: Encode the delimiter in value
            if (value == null)
            {
                return null;
            }

            return value.Replace(Delimiter, DelimiterEncoded);

        }

        private static string Decode(string value)
        {
            //TODO DEADLINE 1.C: Decode the value doing the opposite of Encode()

            if (value == null)
            {
                return null;
            }

            return value.Replace(DelimiterEncoded, Delimiter);

        }

        public string AsText()
        {
            //TODO DEADLINE 1.C: Return the row as string with all values separated by the delimiter

            if (Values == null || Values.Count == 0)
                return "";

            string row = "";
            for (int i = 0; i < Values.Count; i++)
            {
                row += Values[i];
                if (i < Values.Count - 1)
                {
                    row += Delimiter;
                }
            }
            return row;

        }

        public static Row Parse(List<ColumnDefinition> columns, string value)
        {
            //TODO DEADLINE 1.C: Parse a rowReturn the row as string with all values separated by the delimiter

            string[] ret = new string[columns.Count];

            ret = value.Split(Delimiter);

            List<String> rows = new List<String>();
            for (int i = 0; i < ret.Length && i < columns.Count; i++)
            {
                rows.Add(ret[i]);
            }
            return new Row(columns, rows);

        }
    }
}
