using HarmonyLib;
using ResearchPal;
using System;

namespace ImmersiveIshResearch;

[HarmonyPatch(typeof(Tree), "PlaceSingletons")]
public static class Tree_PlaceSingletons
{
    public static void Prefix(ref int colNum)
    {
        colNum = Math.Max(1, colNum);
    }
}