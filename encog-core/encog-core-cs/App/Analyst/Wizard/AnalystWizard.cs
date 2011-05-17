using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Encog.App.Analyst.Script;
using Encog.App.Analyst.Script.Normalize;
using Encog.App.Analyst.Script.Prop;
using Encog.App.Analyst.Script.Segregate;
using Encog.App.Analyst.Script.Task;
using Encog.ML.Factory;
using Encog.Util.Arrayutil;
using Encog.Util.File;

namespace Encog.App.Analyst.Wizard
{
    /// <summary>
    /// The Encog Analyst Wizard can be used to create Encog Analyst script files
    /// from a CSV file. This class is typically used by the Encog Workbench, but it
    /// can easily be used from any program to create a starting point for an Encog
    /// Analyst Script.
    /// Several items must be provided to the wizard.
    /// Desired Machine Learning Method: This is the machine learning method that you
    /// would like the wizard to use. This might be a neural network, SVM or other
    /// supported method.
    /// Normalization Range: This is the range that the data should be normalized
    /// into. Some machine learning methods perform better with different ranges. The
    /// two ranges supported by the wizard are -1 to 1 and 0 to 1.
    /// Goal: What are we trying to accomplish. Is this a classification, regression
    /// or autoassociation problem.
    /// </summary>
    ///
    public class AnalystWizard
    {
        /// <summary>
        /// The default training percent.
        /// </summary>
        ///
        public const int DEFAULT_TRAIN_PERCENT = 75;

        /// <summary>
        /// The default evaluation percent.
        /// </summary>
        ///
        public const int DEFAULT_EVAL_PERCENT = 25;

        /// <summary>
        /// The default training error.
        /// </summary>
        ///
        public const double DEFAULT_TRAIN_ERROR = 0.01d;

        /// <summary>
        /// The raw file.
        /// </summary>
        ///
        public const String FILE_RAW = "FILE_RAW";

        /// <summary>
        /// The normalized file.
        /// </summary>
        ///
        public const String FILE_NORMALIZE = "FILE_NORMALIZE";

        /// <summary>
        /// The randomized file.
        /// </summary>
        ///
        public const String FILE_RANDOM = "FILE_RANDOMIZE";

        /// <summary>
        /// The training file.
        /// </summary>
        ///
        public const String FILE_TRAIN = "FILE_TRAIN";

        /// <summary>
        /// The evaluation file.
        /// </summary>
        ///
        public const String FILE_EVAL = "FILE_EVAL";

        /// <summary>
        /// The eval file normalization file.
        /// </summary>
        ///
        public const String FILE_EVAL_NORM = "FILE_EVAL_NORM";

        /// <summary>
        /// The training set.
        /// </summary>
        ///
        public const String FILE_TRAINSET = "FILE_TRAINSET";

        /// <summary>
        /// The machine learning file.
        /// </summary>
        ///
        public const String FILE_ML = "FILE_ML";

        /// <summary>
        /// The output file.
        /// </summary>
        ///
        public const String FILE_OUTPUT = "FILE_OUTPUT";

        /// <summary>
        /// The balanced file.
        /// </summary>
        ///
        public const String FILE_BALANCE = "FILE_BALANCE";

        /// <summary>
        /// The clustered file.
        /// </summary>
        ///
        public const String FILE_CLUSTER = "FILE_CLUSTER";

        /// <summary>
        /// The analyst.
        /// </summary>
        ///
        private readonly EncogAnalyst analyst;

        /// <summary>
        /// The analyst script.
        /// </summary>
        ///
        private readonly AnalystScript script;

        /// <summary>
        /// Are we using single-field(direct) classification.
        /// </summary>
        ///
        private bool directClassification;

        /// <summary>
        /// The balance filename.
        /// </summary>
        ///
        private String filenameBalance;

        /// <summary>
        /// The cluster filename.
        /// </summary>
        ///
        private String filenameCluster;

        /// <summary>
        /// The evaluation filename.
        /// </summary>
        ///
        private String filenameEval;

        /// <summary>
        /// The normalization eval file name.
        /// </summary>
        ///
        private String filenameEvalNorm;

        /// <summary>
        /// The machine learning file name.
        /// </summary>
        ///
        private String filenameML;

        /// <summary>
        /// The normalized filename.
        /// </summary>
        ///
        private String filenameNorm;

        /// <summary>
        /// The output filename.
        /// </summary>
        ///
        private String filenameOutput;

        /// <summary>
        /// The random file name.
        /// </summary>
        ///
        private String filenameRandom;

        /// <summary>
        /// The raw filename.
        /// </summary>
        ///
        private String filenameRaw;

        /// <summary>
        /// The training filename.
        /// </summary>
        ///
        private String filenameTrain;

        /// <summary>
        /// The training set filename.
        /// </summary>
        ///
        private String filenameTrainSet;

        /// <summary>
        /// The analyst goal.
        /// </summary>
        ///
        private AnalystGoal goal;

        /// <summary>
        /// Should the target field be included int he input, if we are doing 
        /// time-series.
        /// </summary>
        ///
        private bool includeTargetField;

        /// <summary>
        /// The size of the lag window, if we are doing time-series.
        /// </summary>
        ///
        private int lagWindowSize;

        /// <summary>
        /// The size of the lead window, if we are doing time-series.
        /// </summary>
        ///
        private int leadWindowSize;

        /// <summary>
        /// The machine learning method that we will be using.
        /// </summary>
        ///
        private WizardMethodType methodType;

        /// <summary>
        /// The normalization range.
        /// </summary>
        ///
        private NormalizeRange range;

        /// <summary>
        /// The target field, or "" to detect.
        /// </summary>
        ///
        private String targetField;

        /// <summary>
        /// True if the balance command should be generated.
        /// </summary>
        ///
        private bool taskBalance;

        /// <summary>
        /// True if the cluster command should be generated.
        /// </summary>
        ///
        private bool taskCluster;

        /// <summary>
        /// True if the normalize command should be generated.
        /// </summary>
        ///
        private bool taskNormalize;

        /// <summary>
        /// True if the randomize command should be generated.
        /// </summary>
        ///
        private bool taskRandomize;

        /// <summary>
        /// True if the segregate command should be generated.
        /// </summary>
        ///
        private bool taskSegregate;

        /// <summary>
        /// True if we are doing time-series.
        /// </summary>
        ///
        private bool timeSeries;

        /// <summary>
        /// Construct the analyst wizard.
        /// </summary>
        ///
        /// <param name="theAnalyst">The analyst to use.</param>
        public AnalystWizard(EncogAnalyst theAnalyst)
        {
            directClassification = false;
            taskSegregate = true;
            taskRandomize = true;
            taskNormalize = true;
            taskBalance = false;
            taskCluster = true;
            range = NormalizeRange.NegOne2One;
            analyst = theAnalyst;
            script = analyst.Script;
            methodType = WizardMethodType.FeedForward;
            targetField = "";
            goal = AnalystGoal.Classification;
            leadWindowSize = 0;
            lagWindowSize = 0;
            includeTargetField = false;
        }

        /// <summary>
        /// Set the goal.
        /// </summary>
        ///
        /// <value>The goal.</value>
        public AnalystGoal Goal
        {
            /// <returns>The analyst goal.</returns>
            get { return goal; }
            /// <summary>
            /// Set the goal.
            /// </summary>
            ///
            /// <param name="theGoal">The goal.</param>
            set { goal = value; }
        }


        /// <value>the lagWindowSize to set</value>
        public int LagWindowSize
        {
            /// <returns>the lagWindowSize</returns>
            get { return lagWindowSize; }
            /// <param name="theLagWindowSize">the lagWindowSize to set</param>
            set { lagWindowSize = value; }
        }


        /// <value>the leadWindowSize to set</value>
        public int LeadWindowSize
        {
            /// <returns>the leadWindowSize</returns>
            get { return leadWindowSize; }
            /// <param name="theLeadWindowSize">the leadWindowSize to set</param>
            set { leadWindowSize = value; }
        }


        /// <value>the methodType to set</value>
        public WizardMethodType MethodType
        {
            /// <returns>the methodType</returns>
            get { return methodType; }
            /// <param name="theMethodType">the methodType to set</param>
            set { methodType = value; }
        }


        /// <value>the range to set</value>
        public NormalizeRange Range
        {
            /// <returns>the range</returns>
            get { return range; }
            /// <param name="theRange">the range to set</param>
            set { range = value; }
        }


        /// <summary>
        /// Set the target field.
        /// </summary>
        ///
        /// <value>The target field.</value>
        public String TargetField
        {
            /// <returns>Get the target field.</returns>
            get { return targetField; }
            /// <summary>
            /// Set the target field.
            /// </summary>
            ///
            /// <param name="theTargetField">The target field.</param>
            set { targetField = value; }
        }


        /// <value>the includeTargetField to set</value>
        public bool IncludeTargetField
        {
            /// <returns>the includeTargetField</returns>
            get { return includeTargetField; }
            /// <param name="theIncludeTargetField">the includeTargetField to set</param>
            set { includeTargetField = value; }
        }


        /// <value>the taskBalance to set</value>
        public bool TaskBalance
        {
            /// <returns>the taskBalance</returns>
            get { return taskBalance; }
            /// <param name="theTaskBalance">the taskBalance to set</param>
            set { taskBalance = value; }
        }


        /// <value>the taskCluster to set</value>
        public bool TaskCluster
        {
            /// <returns>the taskCluster</returns>
            get { return taskCluster; }
            /// <param name="theTaskCluster">the taskCluster to set</param>
            set { taskCluster = value; }
        }


        /// <value>the taskNormalize to set</value>
        public bool TaskNormalize
        {
            /// <returns>the taskNormalize</returns>
            get { return taskNormalize; }
            /// <param name="theTaskNormalize">the taskNormalize to set</param>
            set { taskNormalize = value; }
        }


        /// <value>the taskRandomize to set</value>
        public bool TaskRandomize
        {
            /// <returns>the taskRandomize</returns>
            get { return taskRandomize; }
            /// <param name="theTaskRandomize">the taskRandomize to set</param>
            set { taskRandomize = value; }
        }


        /// <value>the taskSegregate to set</value>
        public bool TaskSegregate
        {
            /// <returns>the taskSegregate</returns>
            get { return taskSegregate; }
            /// <param name="theTaskSegregate">the taskSegregate to set</param>
            set { taskSegregate = value; }
        }

        /// <summary>
        /// Create a "set" command to add to a task.
        /// </summary>
        ///
        /// <param name="setTarget">The target.</param>
        /// <param name="setSource">The source.</param>
        /// <returns>The "set" command.</returns>
        private String CreateSet(String setTarget, String setSource)
        {
            var result = new StringBuilder();
            result.Append("set ");
            result.Append(ScriptProperties.ToDots(setTarget));
            result.Append("=\"");
            result.Append(setSource);
            result.Append("\"");
            return result.ToString();
        }

        /// <summary>
        /// Determine the type of classification used.
        /// </summary>
        ///
        private void DetermineClassification()
        {
            directClassification = false;

            if ((methodType == WizardMethodType.SVM)
                || (methodType == WizardMethodType.SOM))
            {
                directClassification = true;
            }
        }

        /// <summary>
        /// Determine the target field.
        /// </summary>
        ///
        private void DetermineTargetField()
        {
            IList<AnalystField> fields = script.Normalize.NormalizedFields;

            if (targetField.Trim().Length == 0)
            {
                bool success = false;

                if (goal == AnalystGoal.Classification)
                {
                    // first try to the last classify field
                    foreach (AnalystField field  in  fields)
                    {
                        DataField df = script.FindDataField(field.Name);
                        if (field.Action.IsClassify() && df.Class)
                        {
                            targetField = field.Name;
                            success = true;
                        }
                    }
                }
                else
                {
                    // otherwise, just return the last regression field
                    foreach (AnalystField field_0  in  fields)
                    {
                        DataField df_1 = script.FindDataField(field_0.Name);
                        if (!df_1.Class && (df_1.Real || df_1.Integer))
                        {
                            targetField = field_0.Name;
                            success = true;
                        }
                    }
                }

                if (!success)
                {
                    throw new AnalystError(
                        "Can't determine target field automatically, "
                        + "please specify one.\nThis can also happen if you "
                        + "specified the wrong file format.");
                }
            }
            else
            {
                if (script.FindDataField(targetField) == null)
                {
                    throw new AnalystError("Invalid target field: "
                                           + targetField);
                }
            }

            script.Properties.SetProperty(
                ScriptProperties.DATA_CONFIG_GOAL, goal);

            if (!timeSeries && taskBalance)
            {
                script.Properties.SetProperty(
                    ScriptProperties.BALANCE_CONFIG_BALANCE_FIELD,
                    targetField);
                DataField field_2 = analyst.Script.FindDataField(
                    targetField);
                if ((field_2 != null) && field_2.Class)
                {
                    int countPer = field_2.MinClassCount;
                    script.Properties.SetProperty(
                        ScriptProperties.BALANCE_CONFIG_COUNT_PER, countPer);
                }
            }

            // now that the target field has been determined, set the analyst fields
            AnalystField af = null;

            foreach (AnalystField field_3  in  analyst.Script.Normalize.NormalizedFields)
            {
                if ((field_3.Action != NormalizationAction.Ignore)
                    && field_3.Name.Equals(targetField, StringComparison.InvariantCultureIgnoreCase))
                {
                    if ((af == null) || (af.TimeSlice < af.TimeSlice))
                    {
                        af = field_3;
                    }
                }
            }

            if (af != null)
            {
                af.Output = true;
            }

            // set the clusters count
            if (taskCluster)
            {
                if ((targetField.Length == 0)
                    || (goal != AnalystGoal.Classification))
                {
                    script.Properties.SetProperty(
                        ScriptProperties.CLUSTER_CONFIG_CLUSTERS, 2);
                }
                else
                {
                    DataField tf = script
                        .FindDataField(targetField);
                    script.Properties.SetProperty(
                        ScriptProperties.CLUSTER_CONFIG_CLUSTERS,
                        tf.ClassMembers.Count);
                }
            }
        }

        /// <summary>
        /// Expand the time-series fields.
        /// </summary>
        ///
        private void ExpandTimeSlices()
        {
            IList<AnalystField> oldList = script.Normalize.NormalizedFields;
            IList<AnalystField> newList = new List<AnalystField>();


            // generate the inputs foreach the new list
            foreach (AnalystField field  in  oldList)
            {
                if (!field.Ignored)
                {
                    if (includeTargetField || field.Input)
                    {
                        for (int i = 0; i < lagWindowSize; i++)
                        {
                            var newField = new AnalystField(field);
                            newField.TimeSlice = -i;
                            newField.Output = false;
                            newList.Add(newField);
                        }
                    }
                }
                else
                {
                    newList.Add(field);
                }
            }


            // generate the outputs foreach the new list
            foreach (AnalystField field_0  in  oldList)
            {
                if (!field_0.Ignored)
                {
                    if (field_0.Output)
                    {
                        for (int i_1 = 1; i_1 <= leadWindowSize; i_1++)
                        {
                            var newField_2 = new AnalystField(field_0);
                            newField_2.TimeSlice = i_1;
                            newList.Add(newField_2);
                        }
                    }
                }
            }


            // generate the ignores foreach the new list
            foreach (AnalystField field_3  in  oldList)
            {
                if (field_3.Ignored)
                {
                    newList.Add(field_3);
                }
            }

            // swap back in
            oldList.Clear();
            foreach (AnalystField item in oldList)
            {
                oldList.Add(item);
            }
        }

        /// <summary>
        /// Generate a feed forward machine learning method.
        /// </summary>
        ///
        /// <param name="inputColumns">The input column count.</param>
        /// <param name="outputColumns">The output column count.</param>
        private void GenerateFeedForward(int inputColumns,
                                         int outputColumns)
        {
            var hidden = (int) ((inputColumns)*1.5d);
            script.Properties.SetProperty(
                ScriptProperties.ML_CONFIG_TYPE,
                MLMethodFactory.TYPE_FEEDFORWARD);

            if (range == NormalizeRange.NegOne2One)
            {
                script.Properties.SetProperty(
                    ScriptProperties.ML_CONFIG_ARCHITECTURE,
                    "?:B->TANH->" + hidden + ":B->TANH->?");
            }
            else
            {
                script.Properties.SetProperty(
                    ScriptProperties.ML_CONFIG_ARCHITECTURE,
                    "?:B->SIGMOID->" + hidden + ":B->SIGMOID->?");
            }

            script.Properties.SetProperty(ScriptProperties.ML_TRAIN_TYPE,
                                          "rprop");
            script.Properties.SetProperty(
                ScriptProperties.ML_TRAIN_TARGET_ERROR, DEFAULT_TRAIN_ERROR);
        }

        /// <summary>
        /// Generate filenames.
        /// </summary>
        ///
        /// <param name="rawFile">The raw filename.</param>
        private void GenerateFilenames(FileInfo rawFile)
        {
            filenameRaw = rawFile.Name;
            filenameNorm = FileUtil.AddFilenameBase(rawFile, "_norm").Name;
            filenameRandom = FileUtil.AddFilenameBase(rawFile, "_random").Name;
            filenameTrain = FileUtil.AddFilenameBase(rawFile, "_train").Name;
            filenameEval = FileUtil.AddFilenameBase(rawFile, "_eval").Name;
            filenameEvalNorm = FileUtil.AddFilenameBase(rawFile, "_eval_norm").Name;
            filenameTrainSet = FileUtil.ForceExtension(filenameTrain,
                                                       "egb");
            filenameML = FileUtil.ForceExtension(filenameTrain, "eg");
            filenameOutput = FileUtil.AddFilenameBase(rawFile, "_output").Name;
            filenameBalance = FileUtil.AddFilenameBase(rawFile, "_balance").Name;
            filenameCluster = FileUtil.AddFilenameBase(rawFile, "_cluster").Name;

            ScriptProperties p = script.Properties;

            p.SetFilename(FILE_RAW, filenameRaw);
            if (taskNormalize)
            {
                p.SetFilename(FILE_NORMALIZE, filenameNorm);
            }

            if (taskRandomize)
            {
                p.SetFilename(FILE_RANDOM, filenameRandom);
            }

            if (taskCluster)
            {
                p.SetFilename(FILE_CLUSTER, filenameCluster);
            }

            if (taskSegregate)
            {
                p.SetFilename(FILE_TRAIN, filenameTrain);
                p.SetFilename(FILE_EVAL, filenameEval);
                p.SetFilename(FILE_EVAL_NORM, filenameEvalNorm);
            }

            if (taskBalance)
            {
                p.SetFilename(FILE_BALANCE, filenameBalance);
            }

            p.SetFilename(FILE_TRAINSET, filenameTrainSet);
            p.SetFilename(FILE_ML, filenameML);
            p.SetFilename(FILE_OUTPUT, filenameOutput);
        }

        /// <summary>
        /// Generate the generate task.
        /// </summary>
        ///
        private void GenerateGenerate()
        {
            DetermineTargetField();

            if (targetField == null)
            {
                throw new AnalystError(
                    "Failed to find normalized version of target field: "
                    + targetField);
            }

            int inputColumns = script.Normalize
                .CalculateInputColumns();
            int idealColumns = script.Normalize
                .CalculateOutputColumns();

            switch (methodType)
            {
                case WizardMethodType.FeedForward:
                    GenerateFeedForward(inputColumns, idealColumns);
                    break;
                case WizardMethodType.SVM:
                    GenerateSVM(inputColumns, idealColumns);
                    break;
                case WizardMethodType.RBF:
                    GenerateRBF(inputColumns, idealColumns);
                    break;
                case WizardMethodType.SOM:
                    GenerateSOM(inputColumns);
                    break;
                default:
                    throw new AnalystError("Unknown method type");
            }
        }

        /// <summary>
        /// Generate the normalized fields.
        /// </summary>
        ///
        private void GenerateNormalizedFields()
        {
            IList<AnalystField> norm = script.Normalize.NormalizedFields;
            norm.Clear();
            DataField[] dataFields = script.Fields;

            for (int i = 0; i < script.Fields.Length; i++)
            {
                DataField f = dataFields[i];

                NormalizationAction action;
                bool isLast = i == script.Fields.Length - 1;

                if ((f.Integer || f.Real) && !f.Class)
                {
                    action = NormalizationAction.Normalize;
                    AnalystField af;
                    if (range == NormalizeRange.NegOne2One)
                    {
                        af = new AnalystField(f.Name, action, 1, -1);
                    }
                    else
                    {
                        af = new AnalystField(f.Name, action, 1, 0);
                    }
                    norm.Add(af);
                    af.ActualHigh = f.Max;
                    af.ActualLow = f.Min;
                }
                else if (f.Class)
                {
                    if (isLast && directClassification)
                    {
                        action = NormalizationAction.SingleField;
                    }
                    else if (f.ClassMembers.Count > 2)
                    {
                        action = NormalizationAction.Equilateral;
                    }
                    else
                    {
                        action = NormalizationAction.OneOf;
                    }

                    if (range == NormalizeRange.NegOne2One)
                    {
                        norm.Add(new AnalystField(f.Name, action, 1, -1));
                    }
                    else
                    {
                        norm.Add(new AnalystField(f.Name, action, 1, 0));
                    }
                }
                else
                {
                    action = NormalizationAction.Ignore;
                    norm.Add(new AnalystField(action, f.Name));
                }
            }

            script.Normalize.Init(script);
        }

        /// <summary>
        /// Generate a RBF machine learning method.
        /// </summary>
        ///
        /// <param name="inputColumns">The number of input columns.</param>
        /// <param name="outputColumns">The number of output columns.</param>
        private void GenerateRBF(int inputColumns, int outputColumns)
        {
            var hidden = (int) ((inputColumns)*1.5d);
            script.Properties.SetProperty(
                ScriptProperties.ML_CONFIG_TYPE,
                MLMethodFactory.TYPE_RBFNETWORK);
            script.Properties.SetProperty(
                ScriptProperties.ML_CONFIG_ARCHITECTURE,
                "?->GAUSSIAN(c=" + hidden + ")->?");

            if (outputColumns > 1)
            {
                script.Properties.SetProperty(
                    ScriptProperties.ML_TRAIN_TYPE, "rprop");
            }
            else
            {
                script.Properties.SetProperty(
                    ScriptProperties.ML_TRAIN_TYPE, "svd");
            }

            script.Properties.SetProperty(ScriptProperties.ML_TRAIN_TYPE,
                                          DEFAULT_TRAIN_ERROR);
        }

        /// <summary>
        /// Generate the segregate task.
        /// </summary>
        ///
        private void GenerateSegregate()
        {
            if (taskSegregate)
            {
                var array = new AnalystSegregateTarget[2];
                array[0] = new AnalystSegregateTarget(FILE_TRAIN,
                                                      DEFAULT_TRAIN_PERCENT);
                array[1] = new AnalystSegregateTarget(FILE_EVAL,
                                                      DEFAULT_EVAL_PERCENT);
                script.Segregate.SegregateTargets = array;
            }
            else
            {
                var array_0 = new AnalystSegregateTarget[0];
                script.Segregate.SegregateTargets = array_0;
            }
        }

        /// <summary>
        /// Generate the settings.
        /// </summary>
        ///
        private void GenerateSettings()
        {
            String target;
            String evalSource;

            // starting point
            target = FILE_RAW;
            script.Properties.SetProperty(
                ScriptProperties.HEADER_DATASOURCE_RAW_FILE, target);

            // randomize
            if (!timeSeries && taskRandomize)
            {
                script.Properties.SetProperty(
                    ScriptProperties.RANDOMIZE_CONFIG_SOURCE_FILE,
                    FILE_RAW);
                target = FILE_RANDOM;
                script.Properties.SetProperty(
                    ScriptProperties.RANDOMIZE_CONFIG_TARGET_FILE, target);
            }

            // balance
            if (!timeSeries && taskBalance)
            {
                script.Properties.SetProperty(
                    ScriptProperties.BALANCE_CONFIG_SOURCE_FILE, target);
                target = FILE_BALANCE;
                script.Properties.SetProperty(
                    ScriptProperties.BALANCE_CONFIG_TARGET_FILE, target);
            }

            // segregate
            if (taskSegregate)
            {
                script.Properties.SetProperty(
                    ScriptProperties.SEGREGATE_CONFIG_SOURCE_FILE, target);
                target = FILE_TRAIN;
            }

            // normalize
            if (taskNormalize)
            {
                script.Properties.SetProperty(
                    ScriptProperties.NORMALIZE_CONFIG_SOURCE_FILE, target);
                target = FILE_NORMALIZE;
                script.Properties.SetProperty(
                    ScriptProperties.NORMALIZE_CONFIG_TARGET_FILE, target);
            }

            if (taskSegregate)
            {
                evalSource = FILE_EVAL;
            }
            else
            {
                evalSource = target;
            }

            // cluster
            if (taskCluster)
            {
                script.Properties.SetProperty(
                    ScriptProperties.CLUSTER_CONFIG_SOURCE_FILE, evalSource);
                script.Properties.SetProperty(
                    ScriptProperties.CLUSTER_CONFIG_TARGET_FILE,
                    FILE_CLUSTER);
                script.Properties.SetProperty(
                    ScriptProperties.CLUSTER_CONFIG_TYPE, "kmeans");
            }

            // generate
            script.Properties.SetProperty(
                ScriptProperties.GENERATE_CONFIG_SOURCE_FILE, target);
            script.Properties.SetProperty(
                ScriptProperties.GENERATE_CONFIG_TARGET_FILE,
                FILE_TRAINSET);

            // ML
            script.Properties.SetProperty(
                ScriptProperties.ML_CONFIG_TRAINING_FILE,
                FILE_TRAINSET);
            script.Properties.SetProperty(
                ScriptProperties.ML_CONFIG_MACHINE_LEARNING_FILE,
                FILE_ML);
            script.Properties.SetProperty(
                ScriptProperties.ML_CONFIG_OUTPUT_FILE,
                FILE_OUTPUT);

            script.Properties.SetProperty(
                ScriptProperties.ML_CONFIG_EVAL_FILE, evalSource);

            // other
            script.Properties.SetProperty(
                ScriptProperties.SETUP_CONFIG_CSV_FORMAT,
                AnalystFileFormat.DECPNT_COMMA);
        }

        /// <summary>
        /// Generate a SOM machine learning method.
        /// </summary>
        ///
        /// <param name="inputColumns">The number of input columns.</param>
        private void GenerateSOM(int inputColumns)
        {
            script.Properties.SetProperty(
                ScriptProperties.ML_CONFIG_TYPE, MLMethodFactory.TYPE_SOM);
            script.Properties.SetProperty(
                ScriptProperties.ML_CONFIG_ARCHITECTURE, "?->?");

            script.Properties.SetProperty(ScriptProperties.ML_TRAIN_TYPE,
                                          MLTrainFactory.TYPE_SOM_NEIGHBORHOOD);
            script.Properties.SetProperty(
                ScriptProperties.ML_TRAIN_ARGUMENTS,
                "ITERATIONS=1000,NEIGHBORHOOD=rbf1d,RBF_TYPE=gaussian");

            // ScriptProperties.ML_TRAIN_arguments
            script.Properties.SetProperty(
                ScriptProperties.ML_TRAIN_TARGET_ERROR, DEFAULT_TRAIN_ERROR);
        }

        /// <summary>
        /// Generate a SVM machine learning method.
        /// </summary>
        ///
        /// <param name="inputColumns">The number of input columns.</param>
        /// <param name="outputColumns">The number of ideal columns.</param>
        private void GenerateSVM(int inputColumns, int outputColumns)
        {
            var arch = new StringBuilder();
            arch.Append("?->");
            if (goal == AnalystGoal.Classification)
            {
                arch.Append("C");
            }
            else
            {
                arch.Append("R");
            }
            arch.Append("(type=new,kernel=rbf)->?");

            script.Properties.SetProperty(
                ScriptProperties.ML_CONFIG_TYPE, MLMethodFactory.TYPE_SVM);
            script.Properties.SetProperty(
                ScriptProperties.ML_CONFIG_ARCHITECTURE, arch.ToString());

            script.Properties.SetProperty(ScriptProperties.ML_TRAIN_TYPE,
                                          MLTrainFactory.TYPE_SVM_SEARCH);
            script.Properties.SetProperty(
                ScriptProperties.ML_TRAIN_TARGET_ERROR, DEFAULT_TRAIN_ERROR);
        }

        /// <summary>
        /// Generate the tasks.
        /// </summary>
        ///
        private void GenerateTasks()
        {
            var task1 = new AnalystTask(EncogAnalyst.TASK_FULL);
            if (!timeSeries && taskRandomize)
            {
                task1.Lines.Add("randomize");
            }

            if (!timeSeries && taskBalance)
            {
                task1.Lines.Add("balance");
            }

            if (taskSegregate)
            {
                task1.Lines.Add("segregate");
            }

            if (taskNormalize)
            {
                task1.Lines.Add("normalize");
            }

            task1.Lines.Add("generate");
            task1.Lines.Add("create");
            task1.Lines.Add("train");
            task1.Lines.Add("evaluate");

            var task2 = new AnalystTask("task-generate");
            if (!timeSeries && taskRandomize)
            {
                task2.Lines.Add("randomize");
            }

            if (taskSegregate)
            {
                task2.Lines.Add("segregate");
            }
            if (taskNormalize)
            {
                task2.Lines.Add("normalize");
            }
            task2.Lines.Add("generate");

            var task3 = new AnalystTask("task-evaluate-raw");
            task3.Lines.Add(CreateSet(ScriptProperties.ML_CONFIG_EVAL_FILE,
                                      FILE_EVAL_NORM));
            task3.Lines.Add(CreateSet(ScriptProperties.NORMALIZE_CONFIG_SOURCE_FILE,
                                      FILE_EVAL));
            task3.Lines.Add(CreateSet(ScriptProperties.NORMALIZE_CONFIG_TARGET_FILE,
                                      FILE_EVAL_NORM));
            task3.Lines.Add("normalize");
            task3.Lines.Add("evaluate-raw");

            var task4 = new AnalystTask("task-create");
            task4.Lines.Add("create");

            var task5 = new AnalystTask("task-train");
            task5.Lines.Add("train");

            var task6 = new AnalystTask("task-evaluate");
            task6.Lines.Add("evaluate");

            var task7 = new AnalystTask("task-cluster");
            task7.Lines.Add("cluster");

            script.AddTask(task1);
            script.AddTask(task2);
            script.AddTask(task3);
            script.AddTask(task4);
            script.AddTask(task5);
            script.AddTask(task6);
            script.AddTask(task7);
        }


        /// <summary>
        /// Reanalyze column ranges.
        /// </summary>
        ///
        public void Reanalyze()
        {
            String rawID = script.Properties.GetPropertyFile(
                ScriptProperties.HEADER_DATASOURCE_RAW_FILE);

            FileInfo rawFilename = analyst.Script
                .ResolveFilename(rawID);

            analyst.Analyze(
                rawFilename,
                script.Properties.GetPropertyBoolean(
                    ScriptProperties.SETUP_CONFIG_INPUT_HEADERS),
                script.Properties.GetPropertyFormat(
                    ScriptProperties.SETUP_CONFIG_CSV_FORMAT));
        }

        /// <summary>
        /// Analyze a file.
        /// </summary>
        ///
        /// <param name="analyzeFile">The file to analyze.</param>
        /// <param name="b">True if there are headers.</param>
        /// <param name="format">The file format.</param>
        public void Wizard(FileInfo analyzeFile, bool b,
                           AnalystFileFormat format)
        {
            script.Properties.SetProperty(
                ScriptProperties.HEADER_DATASOURCE_SOURCE_FORMAT, format);
            script.Properties.SetProperty(
                ScriptProperties.HEADER_DATASOURCE_SOURCE_HEADERS, b);
            script.Properties.SetProperty(
                ScriptProperties.HEADER_DATASOURCE_RAW_FILE, analyzeFile);

            timeSeries = ((lagWindowSize > 0) || (leadWindowSize > 0));

            DetermineClassification();
            GenerateFilenames(analyzeFile);
            GenerateSettings();
            analyst.Analyze(analyzeFile, b, format);
            GenerateNormalizedFields();
            GenerateSegregate();

            GenerateGenerate();

            GenerateTasks();
            if (timeSeries && (lagWindowSize > 0)
                && (leadWindowSize > 0))
            {
                ExpandTimeSlices();
            }
        }

        /// <summary>
        /// Analyze a file at the specified URL.
        /// </summary>
        ///
        /// <param name="url">The URL to analyze.</param>
        /// <param name="saveFile">The save file.</param>
        /// <param name="analyzeFile">The Encog analyst file.</param>
        /// <param name="b">True if there are headers.</param>
        /// <param name="format">The file format.</param>
        public void Wizard(Uri url, FileInfo saveFile,
                           FileInfo analyzeFile, bool b,
                           AnalystFileFormat format)
        {
            script.BasePath = saveFile.DirectoryName;

            script.Properties.SetProperty(
                ScriptProperties.HEADER_DATASOURCE_SOURCE_FILE, url);
            script.Properties.SetProperty(
                ScriptProperties.HEADER_DATASOURCE_SOURCE_FORMAT, format);
            script.Properties.SetProperty(
                ScriptProperties.HEADER_DATASOURCE_SOURCE_HEADERS, b);
            script.Properties.SetProperty(
                ScriptProperties.HEADER_DATASOURCE_RAW_FILE, analyzeFile);

            GenerateFilenames(analyzeFile);
            GenerateSettings();
            analyst.Download();

            Wizard(analyzeFile, b, format);
        }
    }
}