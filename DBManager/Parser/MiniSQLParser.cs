using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2
            //SELECT columnas FROM tabla patrón
            const string selectPattern = @"^SELECT\s+([a-zA-Z0-9\*,]+)\s+FROM\s+([a-zA-Z0-9]+)$";

            //SELECT WHERE
            const string selectWherePattern = @"^SELECT\s+([a-zA-Z0-9\*,]+)\s+FROM\s+([a-zA-Z0-9]+)\s+WHERE\s+([a-zA-Z0-9]+)\s*(<|>|=)\s*(.+)";

            //INSERT INTO tabla VALUES columnas patrón
            const string insertPattern = @"^INSERT\s+INTO\s+(\w+)\s+VALUES\s*\(\s*(('[-]?\d+(\.\d+)?'|'[^']+')(?:\s*,\s*('[-]?\d+(\.\d+)?'|'[^']+'))*)\)$";

            //DROP TABLE tabla patrón
            const string dropTablePattern = @"^DROP\s+TABLE\s+([a-zA-Z0-9]+)$";

            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = @"^CREATE\s+TABLE\s+(\w+)\s+\(\s*(\w+\s+(?:INT|DOUBLE|TEXT)(?:\s*,\s*\w+\s+(?:INT|DOUBLE|TEXT))*)\s*\)$";

            const string updateTablePattern = @"^UPDATE\s+(\w+)\s+SET\s+(\w+=('[-]?\d+(\.\d+)?'|'[^']+')(?:,(\w+=('[-]?\d+(\.\d+)?'|'[^']+'))*)?)\s+WHERE\s+(\w+)(=|<|>)('[-]?\d+(\.\d+)?'|'[^']+')$";

            const string deletePattern = @"^DELETE\s+FROM\s+(\w+)\s+WHERE\s+(\w+)(=|<|>)('-?\d+(\.\d+)?'|'[^']+')$";

            //TODO DEADLINE 4
            const string createSecurityProfilePattern = @"^CREATE\s+SECURITY\s+PROFILE\s+([a-zA-Z0-9]+)$";

            const string dropSecurityProfilePattern = @"^DROP\s+SECURITY\s+PROFILE\s+([a-zA-Z0-9]+)$";

            //Captura la tabla, el privilegio y el perfil  
            const string grantPattern = @"^GRANT\s+(SELECT|INSERT|UPDATE|DELETE)\s+ON\s+([a-zA-Z0-9]+)\s+TO\s+([a-zA-Z0-9]+)$";

            const string revokePattern = @"^REVOKE\s+(SELECT|INSERT|UPDATE|DELETE)\s+ON\s+([a-zA-Z0-9]+)\s+TO\s+([a-zA-Z0-9]+)$";

            //Patrón para obtener ADD USER (usuario)(contraseña)(perfil) con espacio entre add y user y los paréntesis de luego
            const string addUserPattern = @"^ADD\s+USER\s+\(([a-zA-Z0-9]+),\s*([a-zA-Z0-9]+),\s*([a-zA-Z0-9]+)\)$";

            const string deleteUserPattern = @"^DELETE\s+USER\s+([a-zA-Z0-9]+)$";

            //TODO DEADLINE 2
            //Parse query using the regular expressions above one by one. If there is a match, create an instance of the query with the parsed parameters
            //For example, if the query is a "SELECT ...", there should be a match with selectPattern. We would create and return an instance of Select
            //initialized with the table name, the columns, and (possibly) an instance of Condition.
            //If there is no match, it means there is a syntax error. We will return null.

            Match matchSelect = Regex.Match(miniSQLQuery, selectPattern);

            if (matchSelect.Success)
            {
                //Los group corresponden al grupo de paréntesis de la expresión regular de arriba
                return new Select(matchSelect.Groups[2].Value, CommaSeparatedNames(matchSelect.Groups[1].Value));
            }

            Match matchSelectWhere = Regex.Match(miniSQLQuery, selectWherePattern);

            if (matchSelectWhere.Success)
            {
                string tableName = matchSelectWhere.Groups[2].Value;
                List<string> columns = CommaSeparatedNames(matchSelectWhere.Groups[1].Value);
                string operador = matchSelectWhere.Groups[4].Value;
                string col = matchSelectWhere.Groups[3].Value;
                string valor = matchSelectWhere.Groups[5].Value;

                if (valor.Contains(" ") && !valor.StartsWith("'"))
                {
                    return null;

                }
                string valorLimpio = valor.Trim('\'');

                Condition condicion = new Condition(col, operador, valorLimpio);
                return new Select(tableName, columns, condicion);


            }

            Match matchInsert = Regex.Match(miniSQLQuery, insertPattern);

            if (matchInsert.Success)
            {

                string tableName = matchInsert.Groups[1].Value.Trim();
                string valores = matchInsert.Groups[2].Value;

                List<string> valoresSucio = CommaSeparatedNames(valores);
                List<string> valoresLimpio = new List<string>();


                foreach (string v in valoresSucio)
                {
                    string val = v.Trim();
                    if (val.Contains(" ") && !val.StartsWith("'"))
                    {
                        return null;
                    }

                    if (val.StartsWith("'") && !val.EndsWith("'") || (!val.StartsWith("'") && val.EndsWith("'")))
                    {
                        return null;
                    }
                    valoresLimpio.Add(val.Trim('\''));
                }
                return new Insert(tableName, valoresLimpio);
            }

            Match matchCreateTable = Regex.Match(miniSQLQuery, createTablePattern);

            if (matchCreateTable.Success)
            {
                string tableName = matchCreateTable.Groups[1].Value;

                string stringColumns = matchCreateTable.Groups[2].Value;

                List<string> columnParts = CommaSeparatedNames(stringColumns);

                List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();



                foreach (string s in columnParts)
                {
                    string[] parts = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    //Solo nombre y tipo
                    if (parts.Length == 2)
                    {

                        string name = parts[0];
                        string typeInt = "INT";
                        string typeDouble = "DOUBLE";
                        string typeTxt = "TEXT";

                        string type = parts[1];
                        if (type == typeInt)
                        {
                            type = "Int";
                            ColumnDefinition.DataType Rtype = (ColumnDefinition.DataType)Enum.Parse(typeof(ColumnDefinition.DataType), type, false);
                            columnDefinitions.Add(new ColumnDefinition(Rtype, name));
                        }
                        else if (type == typeDouble)
                        {
                            type = "Double";
                            ColumnDefinition.DataType Rtype = (ColumnDefinition.DataType)Enum.Parse(typeof(ColumnDefinition.DataType), type, false);
                            columnDefinitions.Add(new ColumnDefinition(Rtype, name));
                        }
                        else if (type == typeTxt)
                        {
                            type = "String";
                            ColumnDefinition.DataType Rtype = (ColumnDefinition.DataType)Enum.Parse(typeof(ColumnDefinition.DataType), type, false);
                            columnDefinitions.Add(new ColumnDefinition(Rtype, name));

                        }
                        else
                        {
                            return null;
                        }
                        //Aquí faltaría una parte pero habría algo, así que es error
                    }
                    else if (parts.Length != 0)
                    {
                        return null;
                    }
                }

                return new CreateTable(tableName, columnDefinitions);
            }

            //A ver si esto arregla el IncorrectCapitalization
            Match matchDrop = Regex.Match(miniSQLQuery, dropTablePattern);

            if (matchDrop.Success)
            {
                return new DropTable(matchDrop.Groups[1].Value);
            }

            Match matchUpdate = Regex.Match(miniSQLQuery, updateTablePattern);

            if (matchUpdate.Success)
            {
                string tableName = matchUpdate.Groups[1].Value;
                string set = matchUpdate.Groups[2].Value;
                List<SetValue> setValues = new List<SetValue>();
                string[] parts = set.Split(',');
                foreach (string p in parts)
                {
                    string[] v = p.Split('=');

                    if (v.Length != 2)
                    {
                        return null;
                    }

                    string columnName = v[0].Trim();
                    string valores = v[1].Trim();

                    bool startsWith = valores.StartsWith("'");
                    bool endsWith = valores.EndsWith("'");

                    if (startsWith != endsWith)
                    {
                        return null;
                    }
                    if (!startsWith && valores.Contains(" "))
                    {
                        return null;
                    }

                    string valor = valores.Trim('\'');
                    setValues.Add(new SetValue(columnName, valor));
                }

                string column = matchUpdate.Groups[8].Value;
                string operador = matchUpdate.Groups[9].Value;
                string value = matchUpdate.Groups[10].Value.Trim('\'');
                Condition condicion = new Condition(column, operador, value);
                return new Update(tableName, setValues, condicion);

            }

            Match matchDelete = Regex.Match(miniSQLQuery, deletePattern);

            if (matchDelete.Success)
            {
                //Mejor simplificar en cuatro grupos los operandos para no tener que hacer los if de =<>
                string tableName = matchDelete.Groups[1].Value.Trim();
                string column = matchDelete.Groups[2].Value.Trim();
                string operador = matchDelete.Groups[3].Value.Trim();
                string valor = matchDelete.Groups[4].Value.Trim();

                if (valor.Contains(" ") && !valor.StartsWith("'"))
                {
                    return null;

                }
                string valorLimpio = valor.Trim('\'');

                Condition condicion = new Condition(column, operador, valorLimpio);
                return new Delete(tableName, condicion);

            }



            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)
            Match matchCreateSecurityProfile = Regex.Match(miniSQLQuery, createSecurityProfilePattern);


            if (matchCreateSecurityProfile.Success)
            {
                return new CreateSecurityProfile(matchCreateSecurityProfile.Groups[1].Value);
            }


            Match matchDropSecurityProfile = Regex.Match(miniSQLQuery, dropSecurityProfilePattern);


            if (matchDropSecurityProfile.Success)
            {
                return new DropSecurityProfile(matchDropSecurityProfile.Groups[1].Value);
            }


            Match matchGrant = Regex.Match(miniSQLQuery, grantPattern);


            if (matchGrant.Success)
            {
                return new Grant(matchGrant.Groups[1].Value, matchGrant.Groups[2].Value, matchGrant.Groups[3].Value);
            }


            Match matchRevoke = Regex.Match(miniSQLQuery, revokePattern);


            if (matchRevoke.Success)
            {
                return new Revoke(matchRevoke.Groups[1].Value, matchRevoke.Groups[2].Value, matchRevoke.Groups[3].Value);
            }


            Match matchAddUser = Regex.Match(miniSQLQuery, addUserPattern);


            if (matchAddUser.Success)
            {
                return new AddUser(matchAddUser.Groups[1].Value, matchAddUser.Groups[2].Value, matchAddUser.Groups[3].Value);
            }

            Match matchDeleteUser = Regex.Match(miniSQLQuery, deleteUserPattern);

            if (matchDeleteUser.Success)
            {
                return new DeleteUser(matchDeleteUser.Groups[1].Value);
            }


            return null;
        }

        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();
            for (int i = 0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i].Trim());
            }
            return commaSeparator;
        }

    }
}