using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace WeatherClientTool
{
    class DataOperation
    {

        public DataTable GetDataFromCSV(string dataFilePath)
        {


            DataTable dataTable = new DataTable("Cities");
            using (TextFieldParser textFieldParser = new TextFieldParser(dataFilePath))
            {
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.SetDelimiters(",");



                while (!textFieldParser.EndOfData)
                {
                    string[] row = textFieldParser.ReadFields();
                    foreach (var item in row)
                        dataTable.Columns.Add(item);

                    break;
                }

                int i = 0;
                while (!textFieldParser.EndOfData)
                {
                    if (i == 0) { i++; continue; };

                    string[] row = textFieldParser.ReadFields();
                    DataRow dataRow = dataTable.NewRow();
                    int productIndex = 0;
                    foreach (var header in row)
                    {
                        string newHeader = header;
                        byte[] bytes = Encoding.Default.GetBytes(newHeader);
                        newHeader = Encoding.UTF8.GetString(bytes);

                        dataRow[productIndex++] = newHeader;
                    }

                    dataTable.Rows.Add(dataRow);
                }

            }

            return dataTable;
        }
        public string removeExtra(string str)
        {
            try
            {
                var normStr = str.Normalize(NormalizationForm.FormD);
                var sB = new StringBuilder(capacity: normStr.Length);

                for (int i = 0; i < normStr.Length; i++)
                {
                    char c = normStr[i];
                    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    {
                        sB.Append(c);
                    }
                }

                return sB
                    .ToString()
                    .Normalize(NormalizationForm.FormC);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
