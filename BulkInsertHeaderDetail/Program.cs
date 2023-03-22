using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkInsertHeaderDetail
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int currentLine = 0;
                string Line = "";
                string PreviousLine = "";
                //StreamReader sr = new StreamReader(@"D:\OneDrive\Documents\FedHealth\MediClinic Claims Feb 2023.csv");
                StreamReader sr = new StreamReader(@"D:\OneDrive\Documents\FedHealth\CJPrinslooClaimsJanFeb.csv");
                while ((Line = sr.ReadLine()) != null)
                {
                    currentLine++;
                    if (!Line.StartsWith("Treatment Date,Beneficiary,Provider,Paid,Claimed"))
                    {
                        if (Line.StartsWith("Treatment Date,Tariff Code"))
                        {
                            ClaimsDataSet.AddHeaderRow(PreviousLine);
                        }
                        else
                        {
                            if ((currentLine > 2) && (!PreviousLine.StartsWith("Treatment Date,Tariff Code")))
                            {
                                ClaimsDataSet.AddDetailRow(PreviousLine);
                            }
                        }
                    }
                    PreviousLine = Line;
                }
                string[] testArray = ClaimsDataSet.ParseCSVLine(PreviousLine);
                foreach (string s in testArray)
                {
                    Console.WriteLine(s);
                }
                if (testArray.Length == 9)
                {
                    ClaimsDataSet.AddDetailRow(PreviousLine);
                }
                sr.Close();
            }
            catch (FormatException fex)
            {
                Console.WriteLine("Source: " + fex.Source);
                Console.WriteLine("Message: " + fex.Message);
                Console.WriteLine("Stack Trace: " + fex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Source: " + ex.Source);
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }
            ClaimsDataSet.WriteToTargetDatabase();
        }
    }
}
