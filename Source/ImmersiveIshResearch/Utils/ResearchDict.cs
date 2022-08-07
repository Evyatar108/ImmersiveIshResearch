using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ImmersiveResearch
{
    // This class essentially behaves as a 'save file' for a mod, in that it is exposed to RWs XML functionality (See IExposable).
    public class ResearchDict : IExposable
    {
        private static Dictionary<string, ImmersiveResearchProject> UndiscoveredResearchList =
            new Dictionary<string, ImmersiveResearchProject>();

        private List<bool> ResearchDictBools = new List<bool>();

        private List<string> ResearchDictKeys = new List<string>();
        private List<float> ResearchDictWeightings = new List<float>();

        //TODO: figure out better way to scribe nested lists if possible
        private Dictionary<int, ResearchTypes> researchTypesLocal = new Dictionary<int, ResearchTypes>();

        public Dictionary<string, ImmersiveResearchProject> MainResearchDict
        {
            get => UndiscoveredResearchList;
            set => UndiscoveredResearchList = value;
        }

        public void ExposeData()
        {
            PrepareDictForScribing();
            Scribe_Collections.Look(ref ResearchDictKeys, "MainResearchDictKeys", LookMode.Value);
            Scribe_Collections.Look(ref ResearchDictBools, "MainResearchDictBools", LookMode.Value);
            Scribe_Collections.Look(ref ResearchDictWeightings, "MainResearchDictWeightings", LookMode.Value);

            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                //Log.Error("loading function running");
                LoadListsToDict();
            }
        }

        // we only save vars that can be saved by the scribing system
        private void PrepareDictForScribing()
        {
            ResearchDictKeys.Clear();
            ResearchDictBools.Clear();
            ResearchDictWeightings.Clear();

            foreach (KeyValuePair<string, ImmersiveResearchProject> kvp in UndiscoveredResearchList)
            {
                ResearchDictKeys.Add(kvp.Key);
                ResearchDictBools.Add(kvp.Value.IsDiscovered);
                ResearchDictWeightings.Add(kvp.Value.Weighting);
            }
        }

        private void LoadListsToDict()
        {
            UndiscoveredResearchList.Clear();
            for (var i = 0; i < ResearchDictKeys.Count; i++)
            {
                var projToAdd =
                    LoreComputerHarmonyPatches.FullConcreteResearchList
                    .Where(item => item.defName == ResearchDictKeys[i])
                    .FirstOrDefault();

                if (projToAdd != null)
                {
                    UndiscoveredResearchList.Add(ResearchDictKeys[i],
                        new ImmersiveResearchProject(projToAdd, ResearchDictBools[i], ResearchDictWeightings[i]));
                }
                else
                {
                    Log.Error("A research project def was null/not found.");
                }
            }
        }
    }
}