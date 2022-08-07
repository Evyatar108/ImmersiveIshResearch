﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ImmersiveResearch
{
    public class GameComponent_ImmersiveResearch : GameComponent
    {
        private HashSet<string> _colonyExperimentAuthorsForSaving = new HashSet<string>();
        private HashSet<string> _colonyExperimentDefNamesForSaving = new HashSet<string>();


        public GameComponent_ImmersiveResearch(Game game)
        {
            MainResearchDict = new ResearchDict();
        }

        public ResearchDict MainResearchDict { get; }

        public Dictionary<string, HashSet<string>> ColonyResearcherExperimentDict { get; } =
            new Dictionary<string, HashSet<string>>();

        public bool CheckResearcherHasPublishedExperiments(string pawn)
        {
            return ColonyResearcherExperimentDict.ContainsKey(pawn);
        }

        private bool CheckResearcherAuthoredSpecificExperiment(string pawn, string def)
        {
            return ColonyResearcherExperimentDict[pawn].Contains(def);
        }

        public void AddColonyExperimentToPawn(string researcher, string researchDef)
        {
            if (CheckResearcherHasPublishedExperiments(researcher))
            {
                if (!CheckResearcherAuthoredSpecificExperiment(researcher, researchDef))
                {
                    ColonyResearcherExperimentDict[researcher].Add(researchDef);
                }
            }
            else
            {
                ColonyResearcherExperimentDict.Add(researcher, new HashSet<string> { researchDef });
            }
        }

        public void ColonyResearcherDeath(string pawn)
        {
            /*foreach(var temp in _colonyResearcherExperimentDict)
            {
                Log.Error(temp.Key);
                foreach(var temp2 in temp.Value)
                {
                    Log.Error(temp2);
                }
            }*/

            var currentProjectsInColony = ColonyResearcherExperimentDict[pawn];
            var researchDefList = new List<Tuple<ResearchProjectDef, int>>();

            foreach (var def in currentProjectsInColony)
            {
                var projDef = LoreComputerHarmonyPatches.FullConcreteResearchList.Where(x => x.defName == def).First();
                var projectDefAndAuthorsCount = new Tuple<ResearchProjectDef, int>(projDef,
                    CheckNumOfAuthorsOnExperiment(projDef.defName));
                researchDefList.Add(projectDefAndAuthorsCount);
            }

            foreach (var projAndAuthorsCount in researchDefList)
            {
                var proj = projAndAuthorsCount.Item1;
                var numOfAuthors = projAndAuthorsCount.Item2;

                if (numOfAuthors > 1)
                {
                    // TODO: move to seperate func
                    if (!MainResearchDict.MainResearchDict[proj.defName].IsDiscovered)
                    {
                        continue;
                    }

                    if (!proj.IsFinished)
                    {
                        continue;
                    }

                    var curProj = Find.ResearchManager.currentProj;

                    Find.ResearchManager.currentProj = proj;

                    var amount = GenerateProgressLossPerAuthor(numOfAuthors,
                        Find.ResearchManager.currentProj.CostApparent);
                    var finalAmount = amount * Find.ResearchManager.currentProj.CostApparent / 0.00825f;
                    Find.ResearchManager.ResearchPerformed(-finalAmount, null);

                    Find.ResearchManager.currentProj = curProj;
                }
                else
                {
                    if (!MainResearchDict.MainResearchDict[proj.defName].IsDiscovered)
                    {
                        continue;
                    }

                    if (proj.ProgressReal == 0.0f)
                    {
                        Find.LetterStack.ReceiveLetter("Colony Researcher Death - Sole Author",
                            proj.defName + " had one author. Unfortunately, the project has been lost.",
                            LetterDefOf.NegativeEvent);

                        ImmersiveResearchProject immersiveProj = MainResearchDict.MainResearchDict[proj.defName];

                        immersiveProj.IsDiscovered = false;

                        LoreComputerHarmonyPatches.TriggerDiscoveredResearchChangedEvent(immersiveProj);
                    }

                    if (!proj.IsFinished && proj.ProgressReal == 0.0f)
                    {
                        continue;
                    }

                    var curProj = Find.ResearchManager.currentProj;

                    Find.ResearchManager.currentProj = proj;

                    Find.ResearchManager.ResearchPerformed(
                        Find.ResearchManager.currentProj.ProgressReal / -0.00825f, null);

                    Find.ResearchManager.currentProj = curProj;
                }
            }

            RemoveResearcherFromDict(pawn);
        }

        private int CheckNumOfAuthorsOnExperiment(string rDef)
        {
            var result = 0;

            foreach (var authorAndExperiments in ColonyResearcherExperimentDict)
            {
                HashSet<string> experimentsDefs = authorAndExperiments.Value;
                if (experimentsDefs.Contains(rDef))
                {
                    result++;
                }
            }

            return result;
        }

        private float GenerateProgressLossPerAuthor(int authorCount, float progressLoss)
        {
            float result = 0;

            // more authors per project, less progress lost
            var totalProgressLost = 1f;

            for (var i = 1; i < authorCount; i++)
            {
                totalProgressLost -= progressLoss;
            }

            result += totalProgressLost;

            return result;
        }

        private void RemoveResearcherFromDict(string key)
        {
            ColonyResearcherExperimentDict.Remove(key);
        }

        public string GetPawnUniqueName(Pawn pawn)
        {
            if (pawn.Name is not NameTriple)
            {
                return null;
            }

            var tempName = pawn.Name.ToString();

            return tempName;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            MainResearchDict.ExposeData();

            if (Scribe.mode == LoadSaveMode.Saving)
            {
                _colonyExperimentAuthorsForSaving = ColonyResearcherExperimentDict.Keys.ToHashSet();

                foreach (var t in ColonyResearcherExperimentDict)
                {
                    var currentExpAuthor = t.Key;
                    if (t.Value.Count <= 0)
                    {
                        continue;
                    }

                    foreach (var s in t.Value)
                    {
                        var temp = currentExpAuthor + "_" + s;
                        _colonyExperimentDefNamesForSaving.Add(temp);
                    }
                }

                Scribe_Collections.Look(ref _colonyExperimentAuthorsForSaving, "ColonyCompletedExperimentsAuthors",
                    LookMode.Value);
                Scribe_Collections.Look(ref _colonyExperimentDefNamesForSaving, "ColonyCompletedExperimentsDefNames",
                    LookMode.Value);
            }

            //Scribe_Collections.Look(ref _colonyResearcherExperimentDict, "ColonyCompletedExperimentsDict", LookMode.Value, LookMode.Value);

            if (Scribe.mode != LoadSaveMode.LoadingVars)
            {
                return;
            }

            Scribe_Collections.Look(ref _colonyExperimentAuthorsForSaving, "ColonyCompletedExperimentsAuthors",
                LookMode.Value);
            Scribe_Collections.Look(ref _colonyExperimentDefNamesForSaving, "ColonyCompletedExperimentsDefNames",
                LookMode.Value);

            if (_colonyExperimentAuthorsForSaving == null || _colonyExperimentDefNamesForSaving == null)
            {
                return;
            }

            foreach (var t in _colonyExperimentAuthorsForSaving)
            {
                // TODO: change this so it doesnt use chars that could be used in a pawn name
                //       Or to something else entirely
                var experiments = new HashSet<string>();
                ColonyResearcherExperimentDict.Add(t, experiments);
                foreach (var j in _colonyExperimentDefNamesForSaving)
                {
                    var newDef = j.Substring(j.LastIndexOf("_", StringComparison.Ordinal) + 1);
                    //  Log.Error(newDef);
                    experiments.Add(newDef);
                }
            }
        }
    }
}