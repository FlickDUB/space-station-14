﻿#nullable enable
using Content.Server.GameObjects.Components.Botany;
using Content.Shared.Interfaces.Chemistry;
using JetBrains.Annotations;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Interfaces.Random;
using Robust.Shared.Interfaces.Serialization;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.Shared.Random;
using Robust.Shared.Serialization;

namespace Content.Server.Chemistry.PlantMetabolism
{
    [UsedImplicitly]
    public class RobustHarvest : IPlantMetabolizable
    {
        void IExposeData.ExposeData(ObjectSerializer serializer)
        {
        }

        public void Metabolize(IEntity plantHolder, float customPlantMetabolism = 1f)
        {
            if (plantHolder.Deleted || !plantHolder.TryGetComponent(out PlantHolderComponent? plantHolderComp)
                                    || plantHolderComp.Seed == null || plantHolderComp.Dead ||
                                    plantHolderComp.Seed.Immutable)
                return;

            var random = IoCManager.Resolve<IRobustRandom>();

            var chance = MathHelper.Lerp(15f, 150f, plantHolderComp.Seed.Potency) * 3.5f * customPlantMetabolism;

            if (random.Prob(chance))
            {
                plantHolderComp.CheckForDivergence(true);
                plantHolderComp.Seed.Potency++;
            }

            chance = MathHelper.Lerp(6f, 2f, plantHolderComp.Seed.Yield) * 0.15f * customPlantMetabolism;

            if (random.Prob(chance))
            {
                plantHolderComp.CheckForDivergence(true);
                plantHolderComp.Seed.Yield--;
            }
        }
    }
}
