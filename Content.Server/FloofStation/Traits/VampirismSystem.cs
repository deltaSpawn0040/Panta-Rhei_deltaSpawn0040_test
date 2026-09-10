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

        var remEv = new BodySystem.WantOrganRemovedEvent(ent, regularStomach);
        var addEv = new BodySystem.WantOrganAddedEvent(ent, vampStomach);

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
