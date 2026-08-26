using Content.Server.Floofstation.Traits.Components;
using Content.Server._Floof.Vampire;
using Content.Shared.Body;
using Content.Shared.Body.Components;
using Content.Shared.Body.Systems;
using Content.Shared.Metabolism;


namespace Content.Server.Floofstation.Traits;

public sealed class VampirismSystem : EntitySystem
{
    [Dependency] private readonly BodySystem _bodySystem = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<VampirismComponent, MapInitEvent>(OnInitVampire);
    }

    private void OnInitVampire(Entity<VampirismComponent> ent, ref MapInitEvent args)
    {
        EnsureBloodSucker(ent);

        if (!TryComp<BodyComponent>(ent, out var body)
		    || !_bodySystem.TryGetOrgansWithComponent<MetabolizerComponent>((ent, body), out var metabolizingOrgans))
            //This method is only used because it's essentiall a drop-in replacement, if I didn't want this
            //in particular to just get done really quickly I wouldn't use it. I will be coming back to this.
            return;

        foreach (var metabolizingOrgan in metabolizingOrgans)
        {
            if (!TryComp<StomachComponent>(metabolizingOrgan, out var stomach))
                continue;
            if (!TryComp<MetabolizerComponent>(metabolizingOrgan, out var metabolizer))
                continue;

            metabolizer.MetabolizerTypes = ent.Comp.MetabolizerPrototypes;

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
