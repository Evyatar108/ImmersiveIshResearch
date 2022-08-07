using Verse;

namespace ImmersiveResearch
{
    internal class ResearchThingComp : ThingComp
    {
        public string pawnExperimentAuthorName;
        public string researchDefName;
        public ResearchCompProperties Properties => Properties;

        public ResearchProjectDef researchDef => LoreComputerHarmonyPatches.UndiscoveredResearchList.MainResearchDict[researchDefName].ProjectDef;

        public void AddPawnAuthor(string author)
        {
            pawnExperimentAuthorName = author;
        }

        public void AddResearch(ResearchProjectDef projDef)
        {
            researchDefName = projDef.defName;
        }
    }
}