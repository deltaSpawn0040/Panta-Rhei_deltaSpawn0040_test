using Content.Server.Floofstation.Traits.Components;
using Content.Server._Floof.Vampire;
using Content.Shared.Body;
using Content.Shared.Body.Components;
using Content.Shared.Body.Systems;
using Content.Shared.Metabolism;


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

    private void OnInitVampire(Entity<VampirismComponent> ent, ref MapInitEvent args)
    {
        //Make sure the entity has the BloodSuckerComponent
        EnsureBloodSucker(ent);

        /*
        This method is only used because it's essentially a drop-in replacement for how the code originally worked.
        There is very much a better way to do this and it should be done that way... eventually. When that is, I'm
        unsure. Nubody is very unstable and an implementation of this system that adheres entirely to its principles
        seems iffy to me. I think I'm stuck between a rock and a hard place here. It seems to be there can either be
        an outdated implementation using bad practices by forcefully setting the fields of two components and using
        obsolete methods, or be at the mercy of whatever changes come down to Nubody. The way I'm thinking, it will
        need yet another rewrite. In the meantime, this horrible little implementation allows us to choose when that
        will be done.
        */
        if (!TryComp<BodyComponent>(ent, out var body)
		    || !_bodySystem.TryGetOrgansWithComponent<MetabolizerComponent>((ent, body), out var metabolizingOrgans))
            return;

        foreach (var metabolizingOrgan in metabolizingOrgans)
        {
            //Try and get the StomachComponent
            if (!TryComp<StomachComponent>(metabolizingOrgan, out var stomach))
                continue;
            //Try and get the MetabolizerComponent
            if (!TryComp<MetabolizerComponent>(metabolizingOrgan, out var metabolizer))
                continue;

            //Set the MetabolizerComponent's MetabolizerTypes to VampireComponent's field instead
            metabolizer.MetabolizerTypes = ent.Comp.MetabolizerPrototypes;

            //Do much the same to the StomachComponent's SpecialDigestible field.
            if (ent.Comp.SpecialDigestible is {} whitelist)
                stomach.SpecialDigestible = whitelist;
        }
    }

    private void EnsureBloodSucker(Entity<VampirismComponent> uid)
    {
        if (HasComp<BloodSuckerComponent>(uid))
            return;

        AddComp(uid, new BloodSuckerComponent
        {
            Delay = uid.Comp.SuccDelay,
            // InjectWhenSucc = false, // The code for it is deprecated, might wanna make it inject something when (if?) it gets reworked
            UnitsToSucc = uid.Comp.UnitsToSucc,
            // WebRequired = false
        });
    }
}
