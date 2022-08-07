using UnityEngine;
using Verse;

namespace ImmersiveResearch
{
    [StaticConstructorOnStartup]
    public static class ImmersiveResearchTextures
    {
        public static readonly Texture2D ResearchSizeSmallIcon = ContentFinder<Texture2D>.Get("UI/Icons/small");
        public static readonly Texture2D ResearchSizeMediumIcon = ContentFinder<Texture2D>.Get("UI/Icons/medium");
        public static readonly Texture2D ResearchSizeLargeIcon = ContentFinder<Texture2D>.Get("UI/Icons/large");
        public static readonly Texture2D ResearchSizeEssentialIcon = ContentFinder<Texture2D>.Get("UI/Icons/essential");
        public static readonly Texture2D ResearchSizeModIcon = ContentFinder<Texture2D>.Get("UI/Icons/mod");

        public static readonly Texture2D MechanicalIcon = ContentFinder<Texture2D>.Get("UI/Icons/mechanical_v2");
        public static readonly Texture2D BiologicalIcon = ContentFinder<Texture2D>.Get("UI/Icons/biological_v2");
        public static readonly Texture2D TextilesIcon = ContentFinder<Texture2D>.Get("UI/Icons/textiles_v2");
        public static readonly Texture2D CulturalIcon = ContentFinder<Texture2D>.Get("UI/Icons/Cultural_v2");
        public static readonly Texture2D ConstructionIcon = ContentFinder<Texture2D>.Get("UI/Icons/Construction_v2");
        public static readonly Texture2D MetallurgyIcon = ContentFinder<Texture2D>.Get("UI/Icons/metallurgy_v2");
        public static readonly Texture2D WeaponryIcon = ContentFinder<Texture2D>.Get("UI/Icons/weaponry_v2");
        public static readonly Texture2D ElectricalIcon = ContentFinder<Texture2D>.Get("UI/Icons/electrical_v2");
        public static readonly Texture2D MedicalIcon = ContentFinder<Texture2D>.Get("UI/Icons/medical_v2");
        public static readonly Texture2D AdvancedIcon = ContentFinder<Texture2D>.Get("UI/Icons/advanced_v2");
        public static readonly Texture2D SpacerIcon = ContentFinder<Texture2D>.Get("UI/Icons/spacer_v2");
        public static readonly Texture2D UltratechIcon = ContentFinder<Texture2D>.Get("UI/Icons/ultratech_v2");
        public static readonly Texture2D SpacecraftIcon = ContentFinder<Texture2D>.Get("UI/Icons/spacecraft_v2");
        public static readonly Texture2D ModIcon = ContentFinder<Texture2D>.Get("UI/Icons/mod");
        public static readonly Texture2D UncategorizedIcon = ContentFinder<Texture2D>.Get("UI/Icons/Uncategorized");
    }
}