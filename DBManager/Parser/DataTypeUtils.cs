using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Parser
{
    public static class DataTypeUtils
    {
        public static ColumnDefinition.DataType FromMiniSQLName(string typeName)
        {
            if (typeName == "INT")
                return ColumnDefinition.DataType.Int;
            else if (typeName == "DOUBLE")
                return ColumnDefinition.DataType.Double;
            else if (typeName == "TEXT")
                return ColumnDefinition.DataType.String;
            throw new Exception($"Unknown MiniSQL data type: {typeName}");
        }

        public static ColumnDefinition.DataType FromMiniTypeName(string typeName)
        {
            if (typeName == "Int")
                return ColumnDefinition.DataType.Int;
            else if (typeName == "Double")
                return ColumnDefinition.DataType.Double;
            else if (typeName == "String")
                return ColumnDefinition.DataType.String;
            throw new Exception($"Unknown MiniSQL data type: {typeName}");
        }
    }
}
