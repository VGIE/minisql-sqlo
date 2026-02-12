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
            Values = values;


        }
        public ColumnDefinition GetColumnByName(string name)
        {
            foreach (ColumnDefinition cd in ColumnDefinitions)
            {
                if (cd.Name.Equals(name))
                {
                    return cd;
                    break;
                }
            }
            return null;
        }
        public void SetValue(string columnName, string value)
        {
            //TODO DEADLINE 1.A: Given a column name and value, change the value in that column
            int posi = 0;
            foreach (ColumnDefinition cd in ColumnDefinitions)
            {
                if (cd.Name.Equals(columnName))
                {
                    
                    Values[posi] = value;
                    break;
                }
                posi++;
            }
            
            

        }

        public string GetValue(string columnName)
        {
            //TODO DEADLINE 1.A: Given a column name, return the value in that column
            int posi = 0;
            foreach (ColumnDefinition cd in ColumnDefinitions)
            {
                if (cd.Name.Equals(columnName))
                {
                    return Values[posi];
                    break;
                }
                posi++;
            }
            return null;
            
        }

        public bool IsTrue(Condition condition)
        {
            //TODO DEADLINE 1.A: Given a condition (column name, operator and literal value, return whether it is true or not
            //for this row. Check Condition.IsTrue method


            return condition.IsTrue(GetValue(condition.ColumnName), GetColumnByName(condition.ColumnName).Type);

        }

        private const string Delimiter = ":";
        private const string DelimiterEncoded = "[SEPARATOR]";


        private static string Encode(string value)
        {
            //TODO DEADLINE 1.C: Encode the delimiter in value

            return value.Replace(Delimiter, DelimiterEncoded);

        }

        private static string Decode(string value)
        {
            //TODO DEADLINE 1.C: Decode the value doing the opposite of Encode()

            return value.Replace(DelimiterEncoded, Delimiter); ;

        }

        public string AsText()
        {
            //TODO DEADLINE 1.C: Return the row as string with all values separated by the delimiter
            string stringSum = "";
            foreach (string v in Values)
            {
                if (Values[Values.Count-1].Equals(v))
                {
                    return stringSum = stringSum + v;
                }
                    stringSum = stringSum + v + DelimiterEncoded;

            }
            return string.Empty;

        }

        public static Row Parse(List<ColumnDefinition> columns, string value)
        {
            //TODO DEADLINE 1.C: Parse a rowReturn the row as string with all values separated by the delimiter
            string[] valuesToArray = Decode(value).Split(Delimiter);
            List<string> val = new List<string>();

            for(int i=0;i<valuesToArray.Length;i++)
            {
                    val.Add(valuesToArray[i]);
                
            }
            
            return new Row(columns, val);

        }
    }
}

