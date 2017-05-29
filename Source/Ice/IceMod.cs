﻿using Harmony;
using HugsLib;
using HugsLib.Utils;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;
using static Ice.Patches.CheckAndPatch;

namespace Ice
{
    public class IceMod : ModBase
    {
        private IceMapComponent iceComponent;

        public override string ModIdentifier { get; } = "Ice";

        public override void Initialize()
        {
        }

        public override void DefsLoaded()
        {
            InjectDetours();
        }

        public override void MapLoaded(Map map)
        {
            InjectDesignators();

            iceComponent = HugsLib.Utils.MapComponentUtility.GetMapComponent<IceMapComponent>(map);
            if (iceComponent == null)
                iceComponent = new IceMapComponent(map);
        }

        private void InjectDesignators()
        {
            var orders = Traverse.Create(DesignationCategories.Orders).Field("resolvedDesignators").GetValue<List<Designator>>();

            if (orders.Any(item => item is Designator_DigIce))
                return;

            var index = orders.FindIndex(item => item is Designator_PlantsHarvestWood);

            var designator = new Designator_DigIce();
            orders.Insert(index + 1, designator);
        }
    }
}