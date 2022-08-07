using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using ImmersiveResearch;
using Verse;

namespace ImmersiveIshResearch;

[StaticConstructorOnStartup]
public class ResearchPalPatch
{
    static ResearchPalPatch()
    {
        var harmony = new Harmony("Mlie.ImmersiveIshResearch.ResearchPal");
        harmony.PatchAll(Assembly.GetExecutingAssembly());


        if (IsAssemblyLoaded())
        {
            LoreComputerHarmonyPatches.DiscoveredResearchEvent += (sender, args) =>
            {
                IsGameStarted = true;
                ResearchPal.Tree.Initializing = false;
                ResearchPal.Tree.ResetLayout();
            };

            LoreComputerHarmonyPatches.FinishPopulatingImmersiveResearchProjectsEvent += (sender, args) =>
            {
                IsGameStarted = true;
                ResearchPal.Tree.Initializing = false;
                ResearchPal.Tree.ResetLayout();
            };
        }

        Log.Message("[ImmersiveishResearch]: Patched for ResearchPal");
    }


    public static bool IsGameStarted { get; private set; } = false;

    public static bool IsAssemblyLoaded()
    {
        return AppDomain.CurrentDomain.GetAssemblies().Any(assembly => assembly.GetName().Name == "ResearchTree");
    }
}