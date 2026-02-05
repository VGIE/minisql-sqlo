using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DbManager;

namespace DbManager
{
    public class Condition
    {
        public string ColumnName { get; private set; }
        public string Operator { get; private set; }
        public string LiteralValue { get; private set; }

        public Condition(string column, string op, string literalValue)
        {
            //TODO DEADLINE 1A: Initialize member variables
            ColumnName=column;
            Operator=op;
            LiteralValue=literalValue;

        }


        public bool IsTrue(string value, ColumnDefinition.DataType type)
        {

            //TODO DEADLINE 1A: return true if the condition is true for this value
            //Depending on the type of the column, the comparison should be different:
            //"ab" < "cd
            //"9" > "10"
            //9 < 10
            //Convert first the strings to the appropriate type and
            //then compare (depending on the operator of the condition)
            bool esIg=false;
            bool esMa=false;
            bool esMe = false;
            switch (type) {
                case ColumnDefinition.DataType.String:
                    esIg=string.Compare(LiteralValue, value)==0;
                    esMa=string.Compare(LiteralValue,value)<0;
                    esMe= string.Compare(LiteralValue, value)>0;
                    break;
                case ColumnDefinition.DataType.Int:
                    int v = int.Parse(value);
                    int tab=int.Parse(LiteralValue);
                    esIg = v == tab;
                    esMa=v > tab;
                    esMe=v < tab;
                    break;
                case ColumnDefinition.DataType.Double:
                    double v2 = double.Parse(value);
                    double tab2 = double.Parse(LiteralValue);
                    double dif = v2 - tab2;
                    esIg=((dif < 0.0000000000000001) && (-dif < 0.0000000000000001));
                    esMa=(dif> 0.0000000000000001);
                    esMe=(-dif > 0.0000000000000001);
                    break;
            }
            if (Operator == "=") { return esIg; }
            if (Operator == ">") { return esMa; }
            if (Operator == "<") { return esMe; }

            return false;
            
        }
    }
}