# SQLBulkCopy Example Project

This is a very basic example put together merely to show how to use the [System.Data.SqlClient.SqlBulkCopy](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlbulkcopy?view=netframework-4.8) class to copy data from a two different [System.Data.DataTable](https://learn.microsoft.com/en-us/dotnet/api/system.data.datatable?view=netframework-4.8) objects contained in a [System.Data.DataSet](https://learn.microsoft.com/en-us/dotnet/api/system.data.dataset?view=netframework-4.8) object to SQL Server database tables while maintaining the correct header/detail relationship between the two tables.

In order to ensure that the HeaderId column on the ClaimDetail table points to the newly inserted IDENTITY value for the ClaimHeader Id column, we use a [System.Data.ForeignKeyConstraint](https://learn.microsoft.com/en-us/dotnet/api/system.data.foreignkeyconstraint?view=netframework-4.8) to describe the relationship and most importantly, we set the [UpdateRule](https://learn.microsoft.com/en-us/dotnet/api/system.data.foreignkeyconstraint.updaterule?view=netframework-4.8) property of the ForeignKeyConstraint object to [Cascade.](https://learn.microsoft.com/en-us/dotnet/api/system.data.rule?view=netframework-4.8)

## Running the Example

The example reads data from a flat file that consists of data describing claims submitted by medical service providers to the medical insurance company where the user has insurance. When downloading claims from the insurer's website, the only supported format is a Microsoft Excel Workbook that has columns like this:

![Excel Data](./Miscellaneous%20Files/Excel%20Workbook%20Downloaded.png)

We open the Excel workbook, make sure that we have the desired worksheet selected and that the columns are as follows:

1. Header
   1. Treatment Date
   2. Beneficiary
   3. Provider
   4. Paid
   5. Claimed
2. Detail
   1. Treatment Date
   2. Tariff Code
   3. Description
   4. Paid
   5. Claimed
   6. Tariff Amount
   7. Paid to Provider
   8. Paid to You
   9. Remarks

Then save that worksheet as a CSV file. 

While processing the lines in the flat file, we do a little manoeuvring to always process the line before the line we just read from the file.  This is because if the line just read contains detail column headings, we know that the line before it contains the header information.

## Structure of the Solution

The solution consists of two projects and a folder containing example data. One of the projects is a SQL Database project that will create the database and tables required for the example.  The other project is a console application that will read the data from the flat file and insert it into the database.

The `Example Data` folder contains a CSV file that can be used to test the example.  The Excel workbook used to create the CSV file is also included in the folder.
