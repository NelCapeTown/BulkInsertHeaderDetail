# SQLBulkCopy Example Project

This is a very basic example put together merely to show how to use the [System.Data.SqlClient.SqlBulkCopy](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlbulkcopy?view=netframework-4.8) class to copy data from a two different [System.Data.DataTable](https://learn.microsoft.com/en-us/dotnet/api/system.data.datatable?view=netframework-4.8) objects contained in a [System.Data.DataSet](https://learn.microsoft.com/en-us/dotnet/api/system.data.dataset?view=netframework-4.8) object to SQL Server database tables while maintaining the correct header/detail relationship between the two tables.

In order to ensure that the HeaderId column on the ClaimDetail table points to the newly inserted IDENTITY value for the ClaimHeader Id column, we use a [System.Data.ForeignKeyConstraint](https://learn.microsoft.com/en-us/dotnet/api/system.data.foreignkeyconstraint?view=netframework-4.8) to describe the relationship and most importantly, we set the [UpdateRule](https://learn.microsoft.com/en-us/dotnet/api/system.data.foreignkeyconstraint.updaterule?view=netframework-4.8) property of the ForeignKeyConstraint object to [Cascade.](https://learn.microsoft.com/en-us/dotnet/api/system.data.rule?view=netframework-4.8)

## Running the Example

The example reads data from a flat file that consists of data describing claims submitted by medical service providers to the medical insurance company where the user has insurance. When downloading claims from the insurer's website, the only supported format is a Microsoft Excel Workbook that has columns like this:

![Excel Data](./Miscellaneous%20Files/Excel%20Workbook%20Downloaded.png)

We open the Excel workbook, make sure that we have the desired worksheet selected and then save that worksheet as a CSV file. 

The flat CSV file contains a header row, followed by detail column headings, followed by one or many detail lines. We do a little manoeuvring to always process the line before the line we just read from the file, because if the line just read contains detail column headings, we know that the line before it contains the header information.

## Structure of the Solution

This system is designed to manage medical insurance claims data. It includes functionality to import claims data from an Excel spreadsheet, process the data, and store it in a relational database.

## Importing Claims Data

To import claims data, navigate to the customer service website of the medical insurance company and download the claims data Excel file. Once downloaded, open the file to review the data and ensure it is formatted correctly.

## Processing Claims Data

To process the claims data, use the ClaimsDataSet class provided in this system. The ClaimsDataSet class includes two DataTable classes, "Header" and "Detail", which represent the header and detail information for each claim.

To manage the relationship between the two DataTable classes, add them to a System.Data.DataSet class. Then, add the columns for each DataTable in memory and indicate how the IDENTITY columns should be populated. Finally, specify the foreign key constraint to link the Header and Detail tables.

## Storing Claims Data

Once the claims data has been processed, it can be stored in a relational database. This system uses SQL Server as the database platform, and includes functionality to create the necessary database objects and tables.

![Database Diagram](placeholder_database_image.png)

## Conclusion

The Medical Insurance Claims Processing System provides a simple yet powerful tool for managing medical insurance claims data. By importing, processing, and storing claims data in a relational database, this system streamlines the claims processing workflow and helps ensure accurate and timely payments.