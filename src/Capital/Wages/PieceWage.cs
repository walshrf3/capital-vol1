using Capital.Value;

namespace Capital.Wages;

/// <summary>
/// The piece-wage (Capital I, ch. 20–21): the converted form of the time-wage — the piece price is
/// nothing but the average daily wage divided by the average number of pieces turned out per day.
/// Under this form the wage appears to be paid for the product itself, and the unpaid part of the
/// day disappears from view; it also smuggles in quality gatekeeping and the sweating system of
/// subcontracting. Marx's verdict: the piece-wage is the form of wages most in harmony with the
/// capitalist's need to squeeze labour.
/// </summary>
public sealed record PieceWage
{
    public MoneyQuantity DailyWage { get; }
    public decimal AveragePiecesPerDay { get; }

    public PieceWage(MoneyQuantity dailyWage, decimal averagePiecesPerDay)
    {
        if (averagePiecesPerDay <= 0m)
            throw new ArgumentOutOfRangeException(
                nameof(averagePiecesPerDay), averagePiecesPerDay, "A piece wage needs a positive average output.");
        DailyWage = dailyWage;
        AveragePiecesPerDay = averagePiecesPerDay;
    }

    /// <summary>The piece price — the converted time-wage (ch. 21 §2).</summary>
    public MoneyQuantity PricePerPiece => DailyWage / AveragePiecesPerDay;

    /// <summary>Total wage for a definite day's output; more pieces, more wage, and the surplus stays hidden.</summary>
    public MoneyQuantity ForPieces(decimal pieces) =>
        pieces <= 0m
            ? throw new ArgumentOutOfRangeException(nameof(pieces), pieces, "Pieces produced must be positive.")
            : PricePerPiece * pieces;

    /// <summary>Ch. 20–21: the piece-wage is derived from the time-wage, never the reverse.</summary>
    public static PieceWage ConvertedFrom(TimeWage timeWage, decimal averagePiecesPerDay) =>
        new(timeWage.DailyWage, averagePiecesPerDay);

    public override string ToString() => $"piece price {PricePerPiece} ({AveragePiecesPerDay:0.###} pieces/day)";
}
