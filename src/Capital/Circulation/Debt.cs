using Capital.Value;

namespace Capital.Circulation;

/// <summary>
/// A debt (Capital I, ch. 3 §3b): a sale completed while its money-payment is still outstanding.
/// Here the commodity and the money never meet in circulation — money functions as means of
/// payment, entering circulation only after the commodity has left it.
/// </summary>
public sealed record Debt
{
    public Owner Creditor { get; }
    public Owner Debtor { get; }
    public MoneyQuantity Principal { get; }

    public Debt(Owner creditor, Owner debtor, MoneyQuantity principal)
    {
        Creditor = creditor ?? throw new ArgumentNullException(nameof(creditor));
        Debtor = debtor ?? throw new ArgumentNullException(nameof(debtor));
        Principal = principal;
    }

    public override string ToString() => $"{Debtor.Name} owes {Creditor.Name} {Principal}";
}
