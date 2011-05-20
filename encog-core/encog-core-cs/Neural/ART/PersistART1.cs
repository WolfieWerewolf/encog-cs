//
// Encog(tm) Core v3.0 - .Net Version
// http://www.heatonresearch.com/encog/
//
// Copyright 2008-2011 Heaton Research, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//   
// For more information on Heaton Research copyrights, licenses 
// and trademarks visit:
// http://www.heatonresearch.com/copyright
//
using System;
using System.Collections.Generic;
using System.IO;
using Encog.Persist;
using Encog.Util;

namespace Encog.Neural.ART
{
    /// <summary>
    /// Persist an ART1 network.  
    /// </summary>
    ///
    [Serializable]
    public class PersistART1 : EncogPersistor
    {
        #region EncogPersistor Members

        /// <inheritdoc/>
        public int FileVersion
        {
            get { return 1; }
        }


        /// <inheritdoc/>
        public String PersistClassString
        {
            get { return "ART1"; }
        }


        /// <inheritdoc/>
        public Object Read(Stream mask0)
        {
            var result = new ART1();
            var ins0 = new EncogReadHelper(mask0);
            EncogFileSection section;

            while ((section = ins0.ReadNextSection()) != null)
            {
                if (section.SectionName.Equals("ART1")
                    && section.SubSectionName.Equals("PARAMS"))
                {
                    IDictionary<String, String> paras = section.ParseParams();
                    EngineArray.PutAll(paras, result.Properties);
                }
                if (section.SectionName.Equals("ART1")
                    && section.SubSectionName.Equals("NETWORK"))
                {
                    IDictionary<String, String> params_0 = section.ParseParams();

                    result.A1 = EncogFileSection.ParseDouble(params_0,
                                                             BasicART.PROPERTY_A1);
                    result.B1 = EncogFileSection.ParseDouble(params_0,
                                                             BasicART.PROPERTY_B1);
                    result.C1 = EncogFileSection.ParseDouble(params_0,
                                                             BasicART.PROPERTY_C1);
                    result.D1 = EncogFileSection.ParseDouble(params_0,
                                                             BasicART.PROPERTY_D1);
                    result.F1Count = EncogFileSection.ParseInt(params_0,
                                                               PersistConst.PROPERTY_F1_COUNT);
                    result.F2Count = EncogFileSection.ParseInt(params_0,
                                                               PersistConst.PROPERTY_F2_COUNT);
                    result.NoWinner = EncogFileSection.ParseInt(params_0,
                                                                BasicART.PROPERTY_NO_WINNER);
                    result.L = EncogFileSection
                        .ParseDouble(params_0, BasicART.PROPERTY_L);
                    result.Vigilance = EncogFileSection.ParseDouble(params_0,
                                                                    BasicART.PROPERTY_VIGILANCE);
                    result.WeightsF1toF2 = EncogFileSection.ParseMatrix(params_0,
                                                                        PersistConst.PROPERTY_WEIGHTS_F1_F2);
                    result.WeightsF2toF1 = EncogFileSection.ParseMatrix(params_0,
                                                                        PersistConst.PROPERTY_WEIGHTS_F2_F1);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public void Save(Stream os, Object obj)
        {
            var xout = new EncogWriteHelper(os);
            var art1 = (ART1) obj;
            xout.AddSection("ART1");
            xout.AddSubSection("PARAMS");
            xout.AddProperties(art1.Properties);
            xout.AddSubSection("NETWORK");

            xout.WriteProperty(BasicART.PROPERTY_A1, art1.A1);
            xout.WriteProperty(BasicART.PROPERTY_B1, art1.B1);
            xout.WriteProperty(BasicART.PROPERTY_C1, art1.C1);
            xout.WriteProperty(BasicART.PROPERTY_D1, art1.D1);
            xout.WriteProperty(PersistConst.PROPERTY_F1_COUNT, art1.F1Count);
            xout.WriteProperty(PersistConst.PROPERTY_F2_COUNT, art1.F2Count);
            xout.WriteProperty(BasicART.PROPERTY_NO_WINNER, art1.NoWinner);
            xout.WriteProperty(BasicART.PROPERTY_L, art1.L);
            xout.WriteProperty(BasicART.PROPERTY_VIGILANCE, art1.Vigilance);
            xout.WriteProperty(PersistConst.PROPERTY_WEIGHTS_F1_F2,
                               art1.WeightsF1toF2);
            xout.WriteProperty(PersistConst.PROPERTY_WEIGHTS_F2_F1,
                               art1.WeightsF2toF1);

            xout.Flush();
        }

        /// <inheritdoc/>
        public Type NativeType
        {
            get { return typeof(ART1); }
        }

        #endregion
    }
}
