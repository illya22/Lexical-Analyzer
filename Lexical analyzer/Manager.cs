using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Lexical_analyzer
{
    class Manager
    {
        List<Lexeme> lexeme_List = null;
        DataTable lexeme_Table = null;

        List<string> keywords = null;
        List<string> operators = null;
        List<char> ignor_symb = new List<char>()
        {
            ' ','\n',Environment.NewLine.ToCharArray()[0],',','\'','\"','(',')',';'
        };



        public DataTable getLexemesTable(string query)
        {
            lexeme_List = analyze(query);
            createTable();
            lexeme_Table = fillTable(lexeme_List);
            return lexeme_Table;
        }

        private List<Lexeme> analyze(string query)
        {
            readFiles();
            lexeme_List = new List<Lexeme>();
            char[] queryArr = query.ToCharArray();
            string lexeme = "";
            int lenght = 0;
            int position = 0;
            int tempCount = 0;
            for (int i = 0; i < queryArr.Length; i++)
            {
                if (ignor_symb.Contains(queryArr[i]) || i + 1 == queryArr.Length)
                {
                    if (lexeme != "")
                    {
                        if (i + 1 == queryArr.Length && !ignor_symb.Contains(queryArr[i]))
                        {
                            lexeme += queryArr[i];
                            lenght++;
                            tempCount++;
                        }
                        lexeme_List.Add(new Lexeme(findToken(lexeme), lexeme, tempCount - lenght + 1, lenght, position));
                        lexeme = "";
                        lenght = 0;
                        position++;
                    }
                }
                else
                {
                    lexeme += queryArr[i];
                    lenght++;
                    tempCount++;
                }
            }
            return lexeme_List;
        }

        private void readFiles()
        {
            keywords = new List<string>();
            using (StreamReader sr = new StreamReader("Keywords.txt"))
            {
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    keywords.Add(str);
                }
            }
            operators = new List<string>();
            using (StreamReader sr = new StreamReader("Operators.txt"))
            {
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    operators.Add(str);
                }
            }
        }

        private string findToken(string lexeme)
        {
            string token = "---";
            if (keywords.Contains(lexeme.ToUpper()))
            {
                token = "Keyword";
            }
            else
            {
                if (operators.Contains(lexeme.ToUpper()))
                {
                    token = "Operator";
                }
                else
                {
                    try
                    {
                        Convert.ToDouble(lexeme);
                        token = "Number";
                    }
                    catch (Exception)
                    {
                        token = "ID";
                    }
                }
            }
            return token;
        }
        private DataTable fillTable(List<Lexeme> lexeme_List)
        {
            for (int i = 0; i < lexeme_List.Count; i++)
            {
                DataRow row = lexeme_Table.NewRow();
                row[0] = lexeme_List[i].getToken();
                row[1] = lexeme_List[i].getLexeme();
                row[2] = lexeme_List[i].getStart();
                row[3] = lexeme_List[i].getLenght();
                row[4] = lexeme_List[i].getPosition();
                lexeme_Table.Rows.Add(row);
            }
            return lexeme_Table;
        }

        private void createTable()
        {
            lexeme_Table = new DataTable("Lexemes");
            lexeme_Table.Columns.AddRange(new DataColumn[5] {
                new DataColumn("Token", typeof(String)),
                new DataColumn("Lexeme", typeof(String)),
                new DataColumn("Start", typeof(int)),
                new DataColumn("Lenght", typeof(int)),
                new DataColumn("Position", typeof(int))
            });
        }
    }
}
