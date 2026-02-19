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
            if (this.ColumnDefinitions == null || this.Values == null || name==null || name =="")
            {
                return null;
            }
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
            if (this.ColumnDefinitions == null || this.Values == null || value == null || columnName == "" || columnName ==null || value=="")
            {
                return;
            }
            //get Index of columnName
            int posi = 0;
            while (!ColumnDefinitions[posi].Name.Equals(columnName))
            {
                posi++;
            }
            //Lenght comparison
            if(posi>Values.Count)
            {
                for(int i=Values.Count-1; i < posi-1; i++)
                {
                    Values.Add(null);
                }
                Values.Add(value);
            }
            else if (posi==Values.Count)
            {
                Values.Add(value);
            }
            else
            {
                Values[posi] = value;
            }


        }

        public string GetValue(string columnName)
        {
            //TODO DEADLINE 1.A: Given a column name, return the value in that column
            int posi = 0;
            if(this.ColumnDefinitions == null || this.Values == null || columnName == null ||columnName == "")
            {
                return null;
            }
            if (posi < Values.Count && posi >= 0)
            {
                foreach (ColumnDefinition cd in ColumnDefinitions)
                {
                    if (cd.Name.Equals(columnName))
                    {
                        return Values[posi];
                        break;
                    }
                    posi++;
                }
                
            }
            return null;
            
        }

        public bool IsTrue(Condition condition)
        {
            //TODO DEADLINE 1.A: Given a condition (column name, operator and literal value, return whether it is true or not
            //for this row. Check Condition.IsTrue method

            if(condition==null)
            {
                return false;
            }
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
            if (this.ColumnDefinitions == null || this.Values == null)
            {
                return null;
            }
            string stringSum = "";
            foreach (string v in Values)
            {
                if(v==null)
                {
                    stringSum = stringSum + Delimiter;
                
                } else
                {
                    stringSum = stringSum + Encode(v) + Delimiter;
                }
                

            }
            return stringSum.Remove(stringSum.Length - 1);

        }

        public static Row Parse(List<ColumnDefinition> columns, string value)
        {
            //TODO DEADLINE 1.C: Parse a rowReturn the row as string with all values separated by the delimiter
            string[] valuesToArray = value.Split(Delimiter);
            List<string> val = new List<string>();

            for (int i = 0; i < valuesToArray.Length; i++)
            {
                val.Add(Decode(valuesToArray[i]));

            }

            return new Row(columns, val);

        }
    }
}

