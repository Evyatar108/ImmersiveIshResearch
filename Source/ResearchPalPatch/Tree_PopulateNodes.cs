using HarmonyLib;
using ImmersiveResearch;
using ResearchPal;
using System;
using System.Collections.Generic;

namespace ImmersiveIshResearch;

[HarmonyPatch(typeof(Tree), "PopulateNodes")]
public static class Tree_PopulateNodes
{
    public static void Postfix(ref List<ResearchNode> __result)
    {
        if (ResearchPalPatch.IsGameStarted)
        {
            __result.RemoveAll(researchNode =>
            {
                try
                {
                    return !WasResearchOfNodeDiscovered(researchNode);
                }
                catch (Exception exc)
                {
                    return true;
                }
            });
        }
    }

    private static bool WasResearchOfNodeDiscovered(ResearchNode researchNode)
	{
        return LoreComputerHarmonyPatches.UndiscoveredResearchList.MainResearchDict.TryGetValue(researchNode.Research.defName, out var immersiveResearch)
                && immersiveResearch.IsDiscovered;
    }
}