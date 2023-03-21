using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using System.Configuration;

namespace BulkInsertHeaderDetail
{
    internal static class ClaimsDataSet
    {
        static DataSet _claimsDataSet = new DataSet();
        static int _currentHeaderId = 0;

        /*
         * Static Class Constructor
         */
        static ClaimsDataSet()
        {
            _claimsDataSet.Tables.Add("Header");
            _claimsDataSet.Tables["Header"].Columns.Add("Id", typeof(int)).AutoIncrement = true;
            _claimsDataSet.Tables["Header"].Columns["Id"].AutoIncrementSeed = 1;
            _claimsDataSet.Tables["Header"].Columns["Id"].AutoIncrementStep = 1;
            _claimsDataSet.Tables["Header"].Columns["Id"].Unique = true;
            _claimsDataSet.Tables["Header"].Columns.Add("TreatmentDate", typeof(System.DateTime));
            _claimsDataSet.Tables["Header"].Columns.Add("Beneficiary", typeof(string));
            _claimsDataSet.Tables["Header"].Columns.Add("ServiceProvider", typeof(string));
            _claimsDataSet.Tables["Header"].Columns.Add("AmountPaid", typeof(decimal));
            _claimsDataSet.Tables["Header"].Columns.Add("AmountClaimed", typeof(decimal));

            _claimsDataSet.Tables.Add("Detail");
            _claimsDataSet.Tables["Detail"].Columns.Add("Id", typeof(int)).AutoIncrement = true;
            _claimsDataSet.Tables["Detail"].Columns["Id"].AutoIncrementSeed = 1;
            _claimsDataSet.Tables["Detail"].Columns["Id"].AutoIncrementStep = 1;
            _claimsDataSet.Tables["Detail"].Columns["Id"].Unique = true;
            _claimsDataSet.Tables["Detail"].Columns.Add("HeaderId", typeof(int));
            _claimsDataSet.Tables["Detail"].Columns.Add("TreatmentDate", typeof(System.DateTime));
            _claimsDataSet.Tables["Detail"].Columns.Add("TariffCode", typeof(string));
            _claimsDataSet.Tables["Detail"].Columns.Add("TariffDescription", typeof(string));
            _claimsDataSet.Tables["Detail"].Columns.Add("AmountPaid", typeof(decimal));
            _claimsDataSet.Tables["Detail"].Columns.Add("AmountClaimed", typeof(decimal));
            _claimsDataSet.Tables["Detail"].Columns.Add("TariffAmount", typeof(decimal));
            _claimsDataSet.Tables["Detail"].Columns.Add("AmountPaidToProvider", typeof(decimal));
            _claimsDataSet.Tables["Detail"].Columns.Add("AmountPaidToBeneficiary", typeof(decimal));

            ForeignKeyConstraint foreignKeyConstraint = new ForeignKeyConstraint("HeaderIdConstraint", _claimsDataSet.Tables["Header"].Columns["Id"], _claimsDataSet.Tables["Detail"].Columns["HeaderId"]);
            foreignKeyConstraint.UpdateRule = Rule.Cascade;
            _claimsDataSet.Tables["Detail"].Constraints.Add(foreignKeyConstraint);
        }

    internal static int AddHeaderRow(string rowContent)
        {
            string errorMessage;
            DateTime workingDate;
            decimal workingAmount;
            string[] commaSeparatedValues = ParseCSVLine(rowContent);
            Console.WriteLine("Just parsed header row from CSV file...");
            DataRow row = _claimsDataSet.Tables["Header"].NewRow();

            if (DateTime.TryParse(commaSeparatedValues[0], out workingDate))
            {
                row["TreatmentDate"] = workingDate;
            }
            else
            {
                errorMessage = String.Format("Error converting string content to date value in {0}, Column: {1}", "Treatment Header", "TreatmentDate");
                Console.WriteLine(errorMessage);
                throw new FormatException(errorMessage);
            }
            row["Beneficiary"] = commaSeparatedValues[1];
            row["ServiceProvider"] = commaSeparatedValues[2];
            if (Decimal.TryParse(commaSeparatedValues[3], NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, null, out workingAmount))
            {
                row["AmountPaid"] = workingAmount;
            }
            else
            {
                errorMessage = String.Format("Error converting string content to money value in {0}, Column: {1}", "Treatment Header", "AmountPaid");
                Console.WriteLine(errorMessage);
                throw new FormatException(errorMessage);
            }
            if (Decimal.TryParse(commaSeparatedValues[4], NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, null, out workingAmount))
            {
                row["AmountClaimed"] = workingAmount;
            }
            else
            {
                errorMessage = String.Format("Error converting string content to money value in {0}, Column: {1}", "Treatment Header", "AmountClaimed");
                Console.WriteLine(errorMessage);
                throw new FormatException(errorMessage);
            }
            _claimsDataSet.Tables["Header"].Rows.Add(row);
            Console.WriteLine("Newly added header row id: " + row["Id"].ToString());
            _currentHeaderId = Int32.Parse(row["Id"].ToString());
            return _currentHeaderId;
        }

        internal static int AddDetailRow(string rowContent)
        {
            string errorMessage = "";
            DateTime workingDate;
            Decimal workingAmount;
            string[] commaSeparatedValues = ParseCSVLine(rowContent);
            Console.WriteLine("Just parsed detail row from CSV file...");
            DataRow row = _claimsDataSet.Tables["Detail"].NewRow();
            row["HeaderId"] = _currentHeaderId;
            if (DateTime.TryParse(commaSeparatedValues[0], out workingDate))
            {
                row["TreatmentDate"] = workingDate;
            }
            else
            {
                errorMessage = String.Format("Error converting string content to date value in {0}, Column: {1}", "Treatment Detail", "TreatmentDate");
                Console.WriteLine(errorMessage);
                throw new FormatException(errorMessage);
            }
            row["TariffCode"] = commaSeparatedValues[1];
            row["TariffDescription"] = commaSeparatedValues[2];
            if (Decimal.TryParse(commaSeparatedValues[3], NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, null, out workingAmount))
            {
                row["AmountPaid"] = workingAmount;
            }
            else
            {
                errorMessage = String.Format("Error converting string content to money value in {0}, Column: {1}", "Treatment Detail", "AmountPaid");
                Console.WriteLine(errorMessage);
                throw new FormatException(errorMessage);
            }
            if (Decimal.TryParse(commaSeparatedValues[4], NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, null, out workingAmount))
            {
                row["AmountClaimed"] = workingAmount;
            }
            else
            {
                errorMessage = String.Format("Error converting string content to money value in {0}, Column: {1}", "Treatment Detail", "AmountClaimed");
                Console.WriteLine(errorMessage);
                throw new FormatException(errorMessage);
            }
            if (Decimal.TryParse(commaSeparatedValues[4], NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, null, out workingAmount))
            {
                row["TariffAmount"] = workingAmount;
            }
            else
            {
                errorMessage = String.Format("Error converting string content to money value in {0}, Column: {1}", "Treatment Detail", "TariffAmount");
                Console.WriteLine(errorMessage);
                throw new FormatException(errorMessage);
            }
            if (Decimal.TryParse(commaSeparatedValues[5], NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, null, out workingAmount))
            {
                row["AmountPaidToProvider"] = workingAmount;
            }
            else
            {
                errorMessage = String.Format("Error converting string content to money value in {0}, Column: {1}", "Treatment Detail", "AmountPaidToProvider");
                Console.WriteLine(errorMessage);
                throw new FormatException(errorMessage);
            }
            if (Decimal.TryParse(commaSeparatedValues[6], NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, null, out workingAmount))
            {
                row["AmountPaidToBeneficiary"] = workingAmount;
            }
            else
            {
                errorMessage = String.Format("Error converting string content to money value in {0}, Column: {1}", "Treatment Detail", "AmountPaidToBeneficiary");
                Console.WriteLine(errorMessage);
                throw new FormatException(errorMessage);
            }
            _claimsDataSet.Tables["Detail"].Rows.Add(row);
            return 1;
        }

        internal static void WriteToTargetDatabase()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["claimAdminConnectionString"].ConnectionString;
                using (SqlConnection destinationConnection = new SqlConnection(connectionString))
                {
                    destinationConnection.Open();
                    ConnectionState destState = destinationConnection.State;
                    Console.WriteLine("State of destination DB after calling open..." + destState.ToString());
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection);
                    bulkCopy.ColumnMappings.Add("TreatmentDate", "TreatmentDate");
                    bulkCopy.ColumnMappings.Add("Beneficiary", "Beneficiary");
                    bulkCopy.ColumnMappings.Add("ServiceProvider", "Provider");
                    bulkCopy.ColumnMappings.Add("AmountPaid", "AmountPaid");
                    bulkCopy.ColumnMappings.Add("AmountClaimed", "AmountClaimed");

                    bulkCopy.DestinationTableName = "flatfile.ClaimHeader";
                    bulkCopy.WriteToServer(_claimsDataSet.Tables["Header"]);

                    bulkCopy.ColumnMappings.Clear();
                    bulkCopy.ColumnMappings.Add("HeaderId", "HeaderId");
                    bulkCopy.ColumnMappings.Add("TreatmentDate", "ClaimDate");
                    bulkCopy.ColumnMappings.Add("TariffCode", "TariffCode");
                    bulkCopy.ColumnMappings.Add("TariffDescription", "ProcedureDescription");
                    bulkCopy.ColumnMappings.Add("AmountPaid", "AmountPaid");
                    bulkCopy.ColumnMappings.Add("AmountClaimed", "AmountClaimed");
                    bulkCopy.ColumnMappings.Add("TariffAmount", "TariffAmount");
                    bulkCopy.ColumnMappings.Add("AmountPaidToProvider", "PaidToProvider");
                    bulkCopy.ColumnMappings.Add("AmountPaidToBeneficiary", "PaidToMember");
                    bulkCopy.DestinationTableName = "flatfile.ClaimDetail";
                    bulkCopy.WriteToServer(_claimsDataSet.Tables["Detail"]);
                    destinationConnection.Close();
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Problem with bulk copy operation...");
                Console.WriteLine(ex.Message);
            }
        }

        internal static string[] ParseCSVLine(string inputString)
        {
            //takes inputString and splits it into array of strings split at commas
            //but if a section between two commas is enclosed in double quotes, the entire
            //section inside the double quotes is treated as one column that includes the commas.

            //If a string that is enclosed by double quotes contains a double quote, it is escaped by a second double quote.

            //example: "a,b,c","d,e,f","g,h,i"	//returns array of 3 strings
            // a,b,c
            // d,e,f
            // g,h,i

            //example: a,b,"c,d,e",f,g			//returns array of 5 strings
            // a
            // b
            // c,d,e
            // f
            // g

            //example: a,b,"c,""d,e",f,g        //returns array of 5 strings
            // a
            // b
            // c,"d,e
            // f
            // g

            //this method correctly reads a csv file created by saving an Excel worksheet as a csv file

            //create a list to hold the strings
            string workString = Regex.Replace(inputString, "(?<!^|,)(\\\")(?!$|,)", "\t");
            List<string> values = new List<string>();
            List<string> finalValue = new List<string>();
            // Variables to track position in line.
            int pos = 0;
            int start = 0;
            bool inQuotes = false;

            // Iterate over each character in the string.
            while (pos < workString.Length)
            {
                char c = workString[pos];

                // If character is a quotation mark, track if we're inside quotes.
                if (c == '\"')
                {
                    inQuotes = !inQuotes;
                }

                // If character is a comma and we're not inside quotes,
                // add the substring between the start and current position to the list.
                else if (c == ',' && !inQuotes)
                {
                    values.Add(workString.Substring(start, pos - start).Replace("\t", "\"\""));
                    start = pos + 1;
                }

                // Move to the next character in the string.
                pos++;
            }

            // Add the last value to the list.
            values.Add(workString.Substring(start).Replace("\t", "\"\""));

            foreach (string value in values)
            {
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    finalValue.Add(value.Substring(1, value.Length - 2));
                }
                else
                {
                    finalValue.Add(value);
                }
            };

            // Convert list to array and return.
            return finalValue.ToArray();
        }
    }
}
