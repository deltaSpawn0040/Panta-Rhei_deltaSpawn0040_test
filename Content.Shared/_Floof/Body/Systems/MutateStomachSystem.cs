using Content.Server.Floofstation.Traits.Components;
using Content.Server._Floof.Vampire;
using Content.Shared.Body;
using Content.Shared.Body.Components;
using Content.Shared.Body.Systems;
using Content.Shared.Metabolism;
using Content.Shared.Chemistry.Components;
using Content.Shared.Whitelist;




namespace Content.Server.Floofstation.Traits;

/// <summary>
/// Makes sure entities with <see cref="VampirismComponent"/> have BloodSuckerComponent and
/// sets their stomach's <see cref="StomachComponent"/> and <see cref="MetabolizerComponent"/>
/// SpecialDigestible and MetabolizerTypes fields (respectively) according to the fields of
/// <see cref="VampirismComponent"/>. Note: This implementation is not ideal.
/// </summary>
public sealed class VampirismSystem : EntitySystem
{
    [Dependency] private readonly BodySystem _bodySystem = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<VampirismComponent, MapInitEvent>(OnInitVampire);
    }

    public void SwapStomachs(EntityUid originalStomach, EntityUid mutatedStomach)
    {
        (ent);
    }

    public void BuildMutatedStomachOrgan(Entity<VampirismComponent> ent, ref MapInitEvent args)
    {
        //Make sure the entity has the BloodSuckerComponent
        (ent);
    }

    public StomachComponent MutateStomachComponent(StomachComponent stoComp, EntityWhitelist? specialDigestible = null, bool? isSpecialDigestibleExclusive = null, Entity<SolutionComponent>? solution = null)
    {
        var newCompSpecialDigestible = stoComp.SpecialDigestible;
        var newCompIsSpecialDigestible = stoComp.IsSpecialDigestibleExclusive;
        var newCompSolution = stoComp.Solution;

        if (specialDigestible is { })
            newCompSpecialDigestible = specialDigestible;
        if (isSpecialDigestibleExclusive is { })
            newCompIsSpecialDigestible = isSpecialDigestibleExclusive.Value;
        if (solution is { })
            newCompSolution = solution;

        return new StomachComponent
        {
            SpecialDigestible = newCompSpecialDigestible,

            IsSpecialDigestibleExclusive = newCompIsSpecialDigestible,

            Solution = newCompSolution
        };


    }

    public MetabolizerComponent MutateMetabolizerComponent(MetabolizerComponent metComp)
    {

    }
}
