using DbManager.Parser;
using System;
using System.Collections.Generic;
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
           
           //INSERT INTO tabla VALUES columnas patrón
            const string insertPattern = @"^\s*INSERT\s+INTO\s+([a-zA-Z0-9]+)\s+VALUES\s*\((.+?)\)\s*$";
            
           //DROP TABLE tabla patrón
            const string dropTablePattern = @"^DROP\s+TABLE\s+([a-zA-Z0-9]+)$";
            
            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = @"^CREATE\s+TABLE\s+([a-zA-Z0-9]+)\s+\(([a-zA-Z0-9,\s]*)\)$";

            const string updateTablePattern = @"^UPDATE\s+([a-zA-Z0-9]+)\s+SET\s+([a-zA-Z0-9\s=,\.']+)\s+WHERE\s+(.+)$";

            const string deletePattern = @"^\s*DELETE\s+FROM\s+([a-zA-Z0-9]+)\s+WHERE\s+([a-zA-Z0-9]+)\s*(<|>|=)\s*(.+?)\s*$";

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

           Match matchSelect= Regex.Match(miniSQLQuery, selectPattern);

           if (matchSelect.Success)
           {
               //Los group corresponden al grupo de paréntesis de la expresión regular de arriba
               return new Select(matchSelect.Groups[2].Value, CommaSeparatedNames(matchSelect.Groups[1].Value)); 
           }

           Match matchInsert= Regex.Match(miniSQLQuery, insertPattern, RegexOptions.IgnoreCase);

           if (matchInsert.Success)
           {

            string tableName= matchInsert.Groups[1].Value.Trim();
            string valores= matchInsert.Groups[2].Value; 

            List<string> valoresSucio= CommaSeparatedNames(valores);
            List<string> valoresLimpio= new List<string>();


                foreach (string v in valoresSucio)
                {
                    if (v.Contains(" ") && !v.StartsWith("'"))
                    {
                    return null;
                    }
                    
                    if(v.StartsWith("'") && !v.EndsWith("'") || (!v.StartsWith("'") && v.EndsWith("'")))
                    {
                        return null;   
                    }
                    valoresLimpio.Add(v.Trim('\''));
                }
               return new Insert(tableName, valoresLimpio);
           }

           Match matchCreateTable= Regex.Match(miniSQLQuery, createTablePattern);

           if (matchCreateTable.Success)
           {
               string tableName= matchCreateTable.Groups[1].Value;
               List<string> stringColumns= CommaSeparatedNames(matchCreateTable.Groups[2].Value);

               List<ColumnDefinition> columnDefinitions= new List<ColumnDefinition>();

               foreach(string s in stringColumns)
               {
                   //El trim es para evitar los errores si hay espacios extra al haber escrito las columnas en el query
                   //Split separa cada columna para quedarse con una cada vez
                   string[] parts= s.Trim().Split(' ');

                   //El >=2 es para que al menos esté un tipo y su columna
                   if (parts.Length>=2)
                   {
                       //Aquí usa parse para convertir el texto en el valor real del tipo que sea. True hace que INT e int sean válidos igual
                       ColumnDefinition.DataType type= (ColumnDefinition.DataType)Enum.Parse(typeof(ColumnDefinition.DataType), parts[1], true);
                       columnDefinitions.Add(new ColumnDefinition(type, parts[0]));
                   }
                  
               }
               return  new CreateTable(tableName, columnDefinitions);
           }
            
            //A ver si esto arregla el IncorrectCapitalization
            Match matchDrop= Regex.Match(miniSQLQuery, dropTablePattern, RegexOptions.IgnoreCase);

           if (matchDrop.Success)
           {
               //Los group corresponden al grupo de paréntesis de la expresión regular de arriba
               return new DropTable(matchDrop.Groups[1].Value);  
           }

            Match matchUpdate = Regex.Match(miniSQLQuery, updateTablePattern);

            if (matchUpdate.Success)
            {
                //Los group corresponden al grupo de paréntesis de la expresión regular de arriba
                string tableName = matchUpdate.Groups[1].Value;
                string value = matchUpdate.Groups[2].Value;
                string condicionTxt = matchUpdate.Groups[3].Value;

                List<SetValue> setValues = new List<SetValue>();

                string[] partes = value.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (string part in partes)
                {
                    string[] v = part.Split('=', StringSplitOptions.RemoveEmptyEntries);

                    if (v.Length != 2)
                    {
                        return null;
                    }

                    string columnName = v[0].Trim();
                    string val = v[1].Trim();

                    if (val.StartsWith("'") && !val.EndsWith("'"))
                    {
                        return null;
                    }

                    setValues.Add(new SetValue(columnName, val));
                }

                char[] opreadores = new char[] { '=', '>', '<' };
                string[] parts = condicionTxt.Split(opreadores, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 2)
                {
                    string column = parts[0].Trim();
                    string value1 = parts[1].Trim();

                    if (value1.StartsWith("'") && !value1.EndsWith("'"))
                    {
                        return null;
                    }

                    //Por defecto, que normalmente suele ser un =
                    string operadorElegido = "=";

                    if (condicionTxt.Contains(">"))
                    {
                        operadorElegido = ">";

                    }
                    else if (condicionTxt.Contains("<"))
                    {
                        operadorElegido = "<";
                    }

                    Condition condicion = new Condition(column, operadorElegido, value1);
                    return new Update(tableName, setValues, condicion);

                }
            }

            Match matchDelete = Regex.Match(miniSQLQuery, deletePattern);

           if (matchDelete.Success)
           {
            //Mejor simplificar en cuatro grupos los operandos para no tener que hacer los if de =<>
            string tableName= matchDelete.Groups[1].Value.Trim();
            string column= matchDelete.Groups[2].Value.Trim();
            string operador= matchDelete.Groups[3].Value.Trim();
            string valor= matchDelete.Groups[4].Value.Trim();

            if (valor.Contains(" ") && !valor.StartsWith("'"))
                {
                    return null;
                    
                }
                string valorLimpio= valor.Trim('\'');

                Condition condicion= new Condition(column, operador, valorLimpio);
                return new Delete(tableName, condicion);
                      
         }
        

            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)
            Match matchCreateSecurityProfile= Regex.Match(miniSQLQuery, createSecurityProfilePattern);


          if (matchCreateSecurityProfile.Success)
          {
              return new CreateSecurityProfile(matchCreateSecurityProfile.Groups[1].Value);
          }


          Match matchDropSecurityProfile= Regex.Match(miniSQLQuery, dropSecurityProfilePattern);


          if (matchDropSecurityProfile.Success)
          {
              return new DropSecurityProfile(matchDropSecurityProfile.Groups[1].Value);
          }


          Match matchGrant= Regex.Match(miniSQLQuery, grantPattern);


          if (matchGrant.Success)
          {
              return new Grant(matchGrant.Groups[1].Value, matchGrant.Groups[2].Value, matchGrant.Groups[3].Value);
          }


          Match matchRevoke= Regex.Match(miniSQLQuery, revokePattern);


          if (matchRevoke.Success)
          {
              return new Revoke(matchRevoke.Groups[1].Value, matchRevoke.Groups[2].Value, matchRevoke.Groups[3].Value);
          }


          Match matchAddUser= Regex.Match(miniSQLQuery, addUserPattern);


          if (matchAddUser.Success)
          {
              return new AddUser(matchAddUser.Groups[1].Value, matchAddUser.Groups[2].Value, matchAddUser.Groups[3].Value);
          }

          Match matchDeleteUser= Regex.Match(miniSQLQuery, deleteUserPattern);

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
            for(int i=0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i].Trim());
            }
            return commaSeparator;
        }
        
    }
}