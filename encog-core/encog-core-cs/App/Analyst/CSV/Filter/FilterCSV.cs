using System;
using System.Collections.Generic;
using System.IO;
using Encog.App.Analyst.CSV.Basic;
using Encog.Util.CSV;

namespace Encog.App.Analyst.CSV.Filter
{
    /// <summary>
    /// This class can be used to remove certain rows from a CSV. You can remove rows
    /// where a specific field has a specific value
    /// </summary>
    ///
    public class FilterCSV : BasicFile
    {
        /// <summary>
        /// The excluded fields.
        /// </summary>
        ///
        private readonly IList<ExcludedField> excludedFields;

        /// <summary>
        /// A count of the filtered rows.
        /// </summary>
        ///
        private int filteredCount;

        /// <summary>
        /// Construct the object.
        /// </summary>
        public FilterCSV()
        {
            excludedFields = new List<ExcludedField>();
        }


        /// <value>A list of the fields and their values, that should be excluded.</value>
        public IList<ExcludedField> Excluded
        {
            get { return excludedFields; }
        }


        /// <value>A count of the filtered rows. This is the resulting line count
        /// for the output CSV.</value>
        public int FilteredRowCount
        {
            get { return filteredCount; }
        }

        /// <summary>
        /// Analyze the file.
        /// </summary>
        ///
        /// <param name="inputFile">The name of the input file.</param>
        /// <param name="headers">True, if headers are expected.</param>
        /// <param name="format">The format.</param>
        public void Analyze(FileInfo inputFile, bool headers,
                            CSVFormat format)
        {
            InputFilename = inputFile;
            ExpectInputHeaders = headers;
            InputFormat = format;

            Analyzed = true;

            PerformBasicCounts();
        }

        /// <summary>
        /// Exclude rows where the specified field has the specified value.
        /// </summary>
        ///
        /// <param name="fieldNumber">The field number.</param>
        /// <param name="fieldValue">The field value.</param>
        public void Exclude(int fieldNumber, String fieldValue)
        {
            excludedFields.Add(new ExcludedField(fieldNumber, fieldValue));
        }


        /// <summary>
        /// Process the input file.
        /// </summary>
        ///
        /// <param name="outputFile">The output file to write to.</param>
        public void Process(FileInfo outputFile)
        {
            var csv = new ReadCSV(InputFilename.ToString(),
                                  ExpectInputHeaders, InputFormat);

            StreamWriter tw = PrepareOutputFile(outputFile);
            filteredCount = 0;

            ResetStatus();
            while (csv.Next() && !ShouldStop())
            {
                UpdateStatus(false);
                var row = new LoadedRow(csv);
                if (ShouldProcess(row))
                {
                    WriteRow(tw, row);
                    filteredCount++;
                }
            }
            ReportDone(false);
            tw.Close();
            csv.Close();
        }

        /// <summary>
        /// Determine if the specified row should be processed, or not.
        /// </summary>
        ///
        /// <param name="row">The row.</param>
        /// <returns>True, if the row should be processed.</returns>
        private bool ShouldProcess(LoadedRow row)
        {
            foreach (ExcludedField field  in  excludedFields)
            {
                if (row.Data[field.FieldNumber].Trim().Equals(
                    field.FieldValue.Trim()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}