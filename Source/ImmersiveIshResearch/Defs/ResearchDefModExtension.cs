using System.Collections.Generic;
using Verse;

namespace ImmersiveResearch
{
    public class ResearchDefModExtension : DefModExtension
    {
        public bool ExperimentHasBeenMade;

        public ResearchProjectDef ResearchDefAttachedToExperiment;

        public ResearchSizes ResearchSize;
        public List<ResearchTypes> researchTypes = new List<ResearchTypes>();
    }
}