using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ImmersiveResearch;

[HarmonyPatch(typeof(MainTabWindow_Research), "VisibleResearchProjects", MethodType.Getter)]
public static class MainTabWindow_Research_VisibleResearchProjects
{
    public static void Postfix(ref List<ResearchProjectDef> __result)
    {
        __result.RemoveAll(projDef => LoreComputerHarmonyPatches.UndiscoveredResearchList.MainResearchDict[projDef.defName]?.IsDiscovered != true);
    }
}