namespace Capital.Value;

/// <summary>
/// The development of the value form (Capital I, ch. 1 §3) as a closed hierarchy of four arms.
/// Consumption goes through switch expressions with an arm per form; the wildcard arm throws, so
/// a new form surfaces loudly at every consumption site (C# does not yet enforce exhaustiveness
/// over class hierarchies, so the throwing default is the guardrail).
/// </summary>
/// <remarks>
/// §4 (commodity fetishism) introduces no new primitive: it is Marx's commentary on exactly these
/// forms. With the value form, the social character of the producers' private labours appears as
/// the social — indeed natural — character of the things themselves. Its content lives here in the
/// forms' social validity: the equal-labour invariants and the socially fixed equivalent.
/// </remarks>
public abstract record ValueForm
{
    /// <summary>The lots whose values are being expressed (relative form side).</summary>
    public abstract IReadOnlyList<CommodityLot> Relatives { get; }

    /// <summary>The lots serving as equivalents.</summary>
    public abstract IReadOnlyList<CommodityLot> Equivalents { get; }

    /// <summary>The general equivalent where the form has one (general and money forms); otherwise null.</summary>
    public abstract CommodityLot? GeneralEquivalent { get; }

    private static void RequireEqualMagnitude(CommodityLot left, CommodityLot right)
    {
        if (left.Value != right.Value)
            throw new ArgumentException(
                $"Value equations require equal labour: {left} embodies {left.Value}, {right} embodies {right.Value}.");
    }

    private static void RequireDistinctKinds(CommodityLot[] lots)
    {
        for (var i = 0; i < lots.Length; i++)
        for (var j = i + 1; j < lots.Length; j++)
            if (lots[i].Commodity == lots[j].Commodity)
                throw new ArgumentException(
                    $"A value expression cannot repeat: {lots[i]} and {lots[j]} are the same commodity.");
    }

    /// <summary>
    /// §3a — the simple or accidental form: one lot's value expressed in a single equivalent
    /// ("20 yards of linen = 1 coat"). The equation holds only between lots embodying equal
    /// magnitudes of labour.
    /// </summary>
    public sealed record Simple : ValueForm
    {
        public CommodityLot Relative { get; }
        public CommodityLot Equivalent { get; }
        private readonly CommodityLot[] _relatives;
        private readonly CommodityLot[] _equivalents;

        public Simple(CommodityLot relative, CommodityLot equivalent)
        {
            ArgumentNullException.ThrowIfNull(relative);
            ArgumentNullException.ThrowIfNull(equivalent);
            RequireEqualMagnitude(relative, equivalent);
            RequireDistinctKinds(new[] { relative, equivalent });
            Relative = relative;
            Equivalent = equivalent;
            _relatives = new[] { relative };
            _equivalents = new[] { equivalent };
        }

        public static Simple Create(CommodityLot relative, CommodityLot equivalent) => new(relative, equivalent);

        public override IReadOnlyList<CommodityLot> Relatives => _relatives;
        public override IReadOnlyList<CommodityLot> Equivalents => _equivalents;
        public override CommodityLot? GeneralEquivalent => null;

        public override string ToString() => $"{Relative} = {Equivalent}";
    }

    /// <summary>
    /// §3b — the total or expanded form: the relative lot's value expressed in a series of
    /// particular equivalents (modeled as at least two — a single equivalent is the simple form).
    /// Marx's defects: a mosaic of unrelated expressions, never complete (the series is endless),
    /// each equivalent excluding the others.
    /// </summary>
    public sealed record Expanded : ValueForm
    {
        public CommodityLot Relative { get; }
        private readonly CommodityLot[] _relatives;
        private readonly CommodityLot[] _equivalents;

        public Expanded(CommodityLot relative, IReadOnlyList<CommodityLot> equivalents)
        {
            ArgumentNullException.ThrowIfNull(relative);
            ArgumentNullException.ThrowIfNull(equivalents);
            if (equivalents.Count < 2)
                throw new ArgumentException(
                    "The expanded form is a series of at least two simple expressions; one equivalent is the simple form.");
            var all = new CommodityLot[equivalents.Count + 1];
            all[0] = relative;
            for (var i = 0; i < equivalents.Count; i++)
            {
                var equivalent = equivalents[i] ?? throw new ArgumentException("Equivalents cannot be null.", nameof(equivalents));
                RequireEqualMagnitude(relative, equivalent);
                all[i + 1] = equivalent;
            }
            RequireDistinctKinds(all);
            Relative = relative;
            _equivalents = all[1..];
            _relatives = new[] { relative };
        }

        public static Expanded Create(CommodityLot relative, IReadOnlyList<CommodityLot> equivalents) =>
            new(relative, equivalents);

        public override IReadOnlyList<CommodityLot> Relatives => _relatives;
        public override IReadOnlyList<CommodityLot> Equivalents => _equivalents;
        public override CommodityLot? GeneralEquivalent => null;

        public override string ToString() => $"{Relative} = " + string.Join(" or = ", _equivalents);
    }

    /// <summary>
    /// §3c — the general form: every commodity expresses its value in one single commodity that is
    /// excluded from the world of relative commodities; the only socially valid form at this stage.
    /// </summary>
    public sealed record General : ValueForm
    {
        private readonly CommodityLot[] _relatives;
        private readonly CommodityLot[] _equivalents;

        public General(IReadOnlyList<CommodityLot> relatives, CommodityLot generalEquivalent)
        {
            ArgumentNullException.ThrowIfNull(relatives);
            ArgumentNullException.ThrowIfNull(generalEquivalent);
            if (relatives.Count < 2)
                throw new ArgumentException(
                    "The general form expresses the values of at least two commodities in one equivalent; fewer is the simple form.");
            var relativeArray = new CommodityLot[relatives.Count];
            for (var i = 0; i < relatives.Count; i++)
            {
                var relative = relatives[i] ?? throw new ArgumentException("Relative commodities cannot be null.", nameof(relatives));
                RequireEqualMagnitude(relative, generalEquivalent);
                relativeArray[i] = relative;
            }
            RequireDistinctKinds(relativeArray);
            foreach (var relative in relativeArray)
                if (relative.Commodity == generalEquivalent.Commodity)
                    throw new ArgumentException(
                        $"The general equivalent {generalEquivalent} is excluded from the world of relative commodities.");
            _relatives = relativeArray;
            GeneralEquivalent = generalEquivalent;
            _equivalents = new[] { generalEquivalent };
        }

        public static General Create(IReadOnlyList<CommodityLot> relatives, CommodityLot generalEquivalent) =>
            new(relatives, generalEquivalent);

        public override IReadOnlyList<CommodityLot> Relatives => _relatives;
        public override IReadOnlyList<CommodityLot> Equivalents => _equivalents;
        public override CommodityLot? GeneralEquivalent { get; }

        public override string ToString() =>
            string.Join("\n", _relatives.Select(relative => $"{relative} = {GeneralEquivalent}"));
    }

    /// <summary>
    /// §3d — the money form: the general equivalent fixed by social custom to a particular
    /// commodity (historically gold). Structurally the general form; a distinct arm so the social
    /// fixity is carried by the type and the §3c → §3d transition is explicit.
    /// </summary>
    public sealed record Money : ValueForm
    {
        private readonly CommodityLot[] _relatives;
        private readonly CommodityLot[] _equivalents;

        private Money(CommodityLot generalEquivalent, CommodityLot[] relatives)
        {
            _relatives = relatives;
            GeneralEquivalent = generalEquivalent;
            _equivalents = new[] { generalEquivalent };
        }

        /// <summary>
        /// §3c → §3d: whatever commodity the general equivalent has crystallized into becomes the
        /// money commodity by social custom.
        /// </summary>
        public static Money FromGeneral(General general)
        {
            ArgumentNullException.ThrowIfNull(general);
            return new Money(general.GeneralEquivalent!, general.Relatives.ToArray());
        }

        public override IReadOnlyList<CommodityLot> Relatives => _relatives;
        public override IReadOnlyList<CommodityLot> Equivalents => _equivalents;
        public override CommodityLot? GeneralEquivalent { get; }

        public override string ToString() =>
            string.Join("\n", _relatives.Select(relative => $"{relative} = {GeneralEquivalent}"));
    }
}
