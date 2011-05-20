using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Util.Arrayutil;

namespace Encog.App.Analyst.Script.Normalize
{
    /// <summary>
    /// This class holds information about the fields that the Encog Analyst will
    /// normalize.
    /// </summary>
    ///
    public class AnalystNormalize
    {
        /// <summary>
        /// The normalized fields.  These fields define the order and format 
        /// that data will be presented to the ML method.
        /// </summary>
        ///
        private readonly IList<AnalystField> _normalizedFields;

        /// <summary>
        /// Construct the object.
        /// </summary>
        public AnalystNormalize()
        {
            _normalizedFields = new List<AnalystField>();
        }

        /// <value>the normalizedFields</value>
        public IList<AnalystField> NormalizedFields
        {
            get { return _normalizedFields; }
        }


        /// <returns>Calculate the input columns.</returns>
        public int CalculateInputColumns()
        {
            return _normalizedFields.Where(field => field.Input).Sum(field => field.ColumnsNeeded);
        }

        /// <summary>
        /// Calculate the output columns.
        /// </summary>
        ///
        /// <returns>The output columns.</returns>
        public int CalculateOutputColumns()
        {
            return _normalizedFields.Where(field => field.Output).Sum(field => field.ColumnsNeeded);
        }


        /// <returns>Count the active fields.</returns>
        public int CountActiveFields()
        {
            int result = 0;

            foreach (AnalystField field  in  _normalizedFields)
            {
                if (field.Action != NormalizationAction.Ignore)
                {
                    result++;
                }
            }
            return result;
        }


        /// <summary>
        /// Init the normalized fields.
        /// </summary>
        ///
        /// <param name="script">The script.</param>
        public void Init(AnalystScript script)
        {
            if (_normalizedFields == null)
            {
                return;
            }


            foreach (AnalystField norm  in  _normalizedFields)
            {
                DataField f = script.FindDataField(norm.Name);

                if (f == null)
                {
                    throw new AnalystError("Normalize specifies unknown field: "
                                           + norm.Name);
                }

                if (norm.Action == NormalizationAction.Normalize)
                {
                    norm.ActualHigh = f.Max;
                    norm.ActualLow = f.Min;
                }

                if ((norm.Action == NormalizationAction.Equilateral)
                    || (norm.Action == NormalizationAction.OneOf)
                    || (norm.Action == NormalizationAction.SingleField))
                {
                    int index = 0;

                    foreach (AnalystClassItem item  in  f.ClassMembers)
                    {
                        norm.Classes.Add(new ClassItem(item.Name, index++));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        ///
        public override sealed String ToString()
        {
            var result = new StringBuilder("[");
            result.Append(GetType().Name);
            result.Append(": ");
            if (_normalizedFields != null)
            {
                result.Append(_normalizedFields.ToString());
            }
            result.Append("]");
            return result.ToString();
        }
    }
}