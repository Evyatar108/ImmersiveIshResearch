using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace ImmersiveResearch
{
    public class Thing_FinishedExperiment : ThingWithComps
    {
        private readonly List<ResearchProjectDef> _researchProjsForSelection = new List<ResearchProjectDef>();
        private ResearchSizes _thingResearchSize = 0;
        private List<ResearchTypes> _thingResearchTypes = new List<ResearchTypes>();

        public List<ResearchTypes> ThingResearchTypes
        {
            get => _thingResearchTypes;
            set => _thingResearchTypes = value;
        }

        public ResearchSizes ThingResearchSize
        {
            get => _thingResearchSize;
            set => _thingResearchSize = value;
        }

        public override void PostMake()
        {
            //TODO: move this from post make so a NEW research proj isnt discovered every time an new instance of this appears in the game world
            _thingResearchTypes = def.GetModExtension<ResearchDefModExtension>().researchTypes;
            _thingResearchSize = def.GetModExtension<ResearchDefModExtension>().ResearchSize;

            //Log.Error(_thingResearchSize.ToString());
            //Log.Error(_thingResearchTypes[0].ToString());
            if (def.GetModExtension<ResearchDefModExtension>().ExperimentHasBeenMade)
            {
                // Log.Error("experiment has been made already");
            }
            else
            {
                if (!_thingResearchTypes.NullOrEmpty())
                {
                    GetResearchProjsByType();
                }
                else
                {
                    Log.Error("Thing_FinishedExperiment failed to get research types from recipe.");
                }
            }

            base.PostMake();
        }

        public override string GetInspectString()
        {
            var sBuilder = new StringBuilder();
            var comp = GetComp<ResearchThingComp>();

            if (comp.pawnExperimentAuthorName == null)
            {
                return string.Empty;
            }

            sBuilder.Append("Most Recent Experiment Author: ");
            sBuilder.AppendLine(comp.pawnExperimentAuthorName);
            sBuilder.Append("Research Discovered: ");
            sBuilder.Append(comp.researchDef.label.CapitalizeFirst());


            return sBuilder.ToString();
        }

        private void SelectResearch()
        {
            def.GetModExtension<ResearchDefModExtension>().ResearchDefAttachedToExperiment =
                LoreComputerHarmonyPatches.SelectResearchByWeightingAndType(_researchProjsForSelection);
        }

        private void GetResearchProjsByType()
        {
            if (!def.HasModExtension<ResearchDefModExtension>())
            {
                return;
            }

            if (!def.GetModExtension<ResearchDefModExtension>().researchTypes.NullOrEmpty())
            {
                var mainResearchDict = LoreComputerHarmonyPatches.UndiscoveredResearchList.MainResearchDict;

                if (_thingResearchTypes[0] == ResearchTypes.Mod)
                {
                    var nonModImmersiveResearchProjects = mainResearchDict.Values.Where(item => item.ResearchTypes[0] != ResearchTypes.Mod);

                    foreach (var immersiveResearchProject in nonModImmersiveResearchProjects)
                    {
                        _researchProjsForSelection.Add(immersiveResearchProject.ProjectDef);
                    }

                    SelectResearch();
                }
                else
                {

                    var projects = mainResearchDict.Values
                        .Where(item => item.ResearchSize == _thingResearchSize
                                && item.ResearchTypes.Any(x => x == _thingResearchTypes[0])
                                && item.ProjectDef != null
                                && !item.IsDiscovered);

                    bool foundProjects = false;
                    foreach (var project in projects)
                    {
                        foundProjects = true;
                        _researchProjsForSelection.Add(project.ProjectDef);
                    }

                    if (foundProjects)
                    {
                        SelectResearch();
                    }
                    else
                    {
                        //Log.Error("no researches are undiscovered");
                        Find.LetterStack.ReceiveLetter("Experiment Completed",
                            "An experiment has been completed, and unfortunately nothing has been discovered.",
                            LetterDefOf.NeutralEvent);
                    }
                }
            }
            else
            {
                Log.Error("Finished Experiment type list is empty.");
            }
        }

        //TODO: expose researchthingcomp
        public override void ExposeData()
        {
            ResearchThingComp comp;
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                comp = GetComp<ResearchThingComp>();

                Scribe_Collections.Look(ref _thingResearchTypes, "ResearchTypes", LookMode.Value);
                Scribe_Values.Look(ref _thingResearchSize, "ResearchSize");
                Scribe_Values.Look(ref comp.pawnExperimentAuthorName, "ExperimentAuthor");
                //Scribe_Defs.Look(ref comp.researchDef, "ExperimentDef");
                Scribe_Values.Look(ref comp.researchDefName, "ExperimentDefName");
            }

            if (Scribe.mode != LoadSaveMode.LoadingVars)
            {
                return;
            }

            comp = GetComp<ResearchThingComp>();

            Scribe_Collections.Look(ref _thingResearchTypes, "ResearchTypes", LookMode.Value);
            Scribe_Values.Look(ref _thingResearchSize, "ResearchSize");
            Scribe_Values.Look(ref comp.pawnExperimentAuthorName, "ExperimentAuthor");
            // Scribe_Defs.Look(ref comp.researchDef, "ExperimentDef");
            Scribe_Values.Look(ref comp.researchDefName, "ExperimentDefName");
        }
    }
}