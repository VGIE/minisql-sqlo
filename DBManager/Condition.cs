using DbManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

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
            this.ColumnName = column;
            this.Operator = op;
            this.LiteralValue = literalValue;
            
        }


        public bool IsTrue(string value, ColumnDefinition.DataType type)
        {
            //TODO DEADLINE 1A: return true if the condition is true for this value
            //Depending on the type of the column, the comparison should be different:
            //"ab" < "cd"
            //"9" > "10"
            //9 < 10
            //Convert first the strings to the appropriate type and then compare (depending on the operator of the condition)
            if (value == null || type == null) { return false; }
            if (type is String)
            {
                switch (Operator)
                {
                    case "<": if (value.CompareTo(LiteralValue) < 0) { return true; } return false;
                    case "=": if (value.CompareTo(LiteralValue)==0) { return true; } return false; ;
                    case ">": if (value.CompareTo(LiteralValue) > 0) { return true; } return false; ;

                }
            }
            else if (type is int)
            {
                int valor = Int32.Parse(value);
                int valor2 = Int32.Parse(value);
                switch (Operator)
                {
                    case "<": if (valor < valor2) { return true; } return false; ;
                    case "=": if (valor == valor2) { return true; } return false; ;
                    case ">": if (valor > valor2) { return true; } return false; ;

                }
            }
            else if (type is double)
            {
                double valor = Double.Parse(value);
                double valor2 = Double.Parse(LiteralValue);
                switch (Operator) {
                    case "<": if (valor<valor2) { return true; } return false; ;
                    case "=": if (valor == valor2) { return true; } return false; ;
                    case ">": if (valor > valor2) { return true; } return false; ;

                }

            }
            
            return false;
            
        }
    }
}