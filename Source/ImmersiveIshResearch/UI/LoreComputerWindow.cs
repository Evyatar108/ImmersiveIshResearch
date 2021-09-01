﻿using UnityEngine;
using Verse;

namespace ImmersiveResearch
{
    // UNUSED CLASS

    public class LoreComputerWindow : Window
    {
        public LoreComputerWindow()
        {
            forcePause = true;
            absorbInputAroundWindow = true;
            onlyOneOfTypeAllowed = false;

            InitialiseThingList();
        }

        // sets initial size of the window
        public override Vector2 InitialSize => new Vector2(950f, 760f);


        private void InitialiseThingList()
        {
            /*
            ThingList = Find.CurrentMap.mapPawns.AllPawns.Where<Pawn>((Func<Pawn, bool>)(p =>
            {
                if (p.Spawned && p.Faction == null && p.AnimalOrWildMan())
                    return !p.Position.Fogged(p.Map);
                return false;
            }));*/
        }
        // OLD MOD IDEAS
        // 
        //       window needs a list of everything kinda like the list of current stuff in storage in game
        // could use a custom thing that is used within the info card but links to other things of same name
        // if using info card route, would probs use the drawstats implementation to create a list of objects

        // OVERALL DESIGN:
        // ingame 'theme friendly' version of rimworld wiki
        // lore entries that you have to find in game to add to database
        // could also make it so you have to find lore entries to unlock further research options


        //info card (inspect button) algorithm
        /*
         user presses button
            a constructor (out of 4 seperate types) is called
                in UI update for this window:
                    create window
                    create tabs (depending on what is selected, i.e. pawn)
                    fill the currently opened tab:
                        stats == draw stats of selected object            
         */

        // custom info card functions
        /*
          // currently moused over record - statdrawentry 
          // currently selected record - statdrawentry
          // list of all selectable Things
             in UI update:
                create window
                fill the info card with our list of things

         */

        // our proposed solution algorithm
        /*
         user accesses the lore datatbase
           a constructor is called on LoreComputerWindow
               populate list of lore objects
               in UI update
                   create custom info card window (no tabs)
                   populate the window with custom drawstats method
        */

        private void CreateLoreWindow(Rect inRect)
        {
            var rect1 = new Rect(inRect).ContractedBy(18f);
            rect1.height = 74f;
            rect1.width = 100f;
            Text.Font = GameFont.Medium;
            Widgets.Label(rect1, "Lore Database Test");
            var rect2 = new Rect(inRect)
            {
                yMin = rect1.yMax
            };
            rect2.yMax -= 38f;
            var rect3 = rect2;
            rect3.yMin += 45f;

            CreateThingListWindow(rect3.ContractedBy(18f));
        }

        private void CreateThingListWindow(Rect refRect)
        {
            //LoreWindowDrawingUtility.DrawLoreFullList(refRect, ThingList);
        }

        // UI-based update
        public override void DoWindowContents(Rect inRect)
        {
            CreateLoreWindow(inRect);


            // this code just creates a window.. not sure if really needed
            /*float testYPos = inRect.y;

            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(0f, testYPos, inRect.width, 42f), "Lore Computer");

            testYPos += 42f;

            Text.Font = GameFont.Small;
            Rect outRect = new Rect(inRect.x, testYPos, inRect.width, inRect.height - 35f - 5f - testYPos);
            float width = outRect.width - 16f;

            Rect viewRect = new Rect(0f, 0f, width, width + outRect.height);

             if (Widgets.ButtonText(new Rect(350f, 0, inRect.width / 4f - 20f, 35f), "Close", true, false, true))
             {
                 this.Close(true);
             }*/
        }
    }
}