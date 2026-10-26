using Capital.Value;

namespace Capital.Circulation;

/// <summary>
/// Payment (Capital I, ch. 3 §3b): money entering circulation after the commodity has left it,
/// discharging a debt exactly. Chains of payments and obligations make the payment system
/// vulnerable as a system — money as means of payment is what turns a local failure into crisis.
/// </summary>
public sealed record Payment
{
    public Debt Debt { get; }
    public MoneyQuantity Paid { get; }

    public Payment(Debt debt, MoneyQuantity paid)
    {
        Debt = debt ?? throw new ArgumentNullException(nameof(debt));
        if (paid != debt.Principal)
            throw new ArgumentException(
                $"Payment must discharge the debt exactly: principal {debt.Principal}, paid {paid}.");
        Paid = paid;
    }

    public static Payment Settle(Debt debt, MoneyQuantity paid) => new(debt, paid);

    public override string ToString() => $"{Paid} discharges {Debt}";
}
