using Robust.Shared.Containers;
using Robust.Shared.Map;


namespace Content.Shared.Body;

public sealed partial class BodySystem
{

    public void InitializeOrganManipulation()
    {
        SubscribeLocalEvent<WantOrganAddedEvent>(OnOrganAddedRequest);
        SubscribeLocalEvent<WantOrganRemovedEvent>(OnOrganRemovedRequest);
    }

    private void OnOrganAddedRequest(ref WantOrganAddedEvent args)
    {
        if (!TryGetBodyOrganContainer(args.bodyHaver, out var bodyOrgans))
            if (bodyOrgans != null)
                if (!_container.Insert(args.organ, bodyOrgans))
                    Log.Warning($"Failed to insert organ {ToPrettyString(args.organ)} into {ToPrettyString(args.bodyHaver)}.");
    }

    private void OnOrganRemovedRequest(ref WantOrganRemovedEvent args)
    {
        if (!TryGetBodyOrganContainer(args.bodyHaver, out var bodyOrgans))
            if (bodyOrgans != null)
                if (!_container.Remove(args.organ, bodyOrgans, args.reparent, args.force, args.destination, args.localRotation))
                    Log.Warning($"Failed to remove organ {ToPrettyString(args.organ)} from {ToPrettyString(args.bodyHaver)}.");
    }


    public bool TryGetBodyOrganContainer(EntityUid ent, out Container? organs)
    {
        organs = null;

        if(!TryComp<BodyComponent>(ent, out var bodyComp))
           return false;

        organs = bodyComp.Organs;

        return true;
    }

    /// <summary>
    /// Event that calls for an organ to be added to an entity with <see cref="BodyComponent"/>
    /// </summary>
    [ByRefEvent]
    public record struct WantOrganAddedEvent(EntityUid BodyHaver, EntityUid Organ)
    {
        /// <summary>
        /// Entity we're trying to add the organ to
        /// </summary>
        public readonly EntityUid bodyHaver = BodyHaver;

        /// <summary>
        /// Organ that we want to add.
        /// </summary>
        public readonly EntityUid organ = Organ;

        /// <summary>
        /// Did we succeed in adding the organ?
        /// </summary>
        public bool succeeded = false;

    }

    /// <summary>
    /// Event that calls for an organ to be removed from an entity with <see cref="BodyComponent"/>
    /// </summary>
    [ByRefEvent]
    public record struct WantOrganRemovedEvent(EntityUid BodyHaver, EntityUid Organ, bool reparent = true, bool force = false, EntityCoordinates? destination = null, Angle? localRotation = null)
    {
        /// <summary>
        /// Entity we're trying to remove the organ from
        /// </summary>
        public readonly EntityUid bodyHaver = BodyHaver;

        /// <summary>
        /// Organ that we want to remove.
        /// </summary>
        public readonly EntityUid organ = Organ;

        /// <summary>
        /// Did we succeed removing the organ?
        /// </summary>
        public bool succeeded = false;

        // Begin function arguments for Remove
        //---------------------------------------------------------
        /// <summary>
        /// Argument for <see cref="SharedContainerSystem.Remove"/>
        /// </summary>
        public bool reparent = reparent;

        /// <summary>
        /// Argument for <see cref="SharedContainerSystem.Remove"/>
        /// </summary>
        public bool force = force;

        /// <summary>
        /// Argument for <see cref="SharedContainerSystem.Remove"/>
        /// </summary>
        public EntityCoordinates? destination = destination;

        /// <summary>
        /// Argument for <see cref="SharedContainerSystem.Remove"/>
        /// </summary>
        public Angle? localRotation = localRotation;
        //---------------------------------------------------------
        // End function arguments for Remove
    }

}
