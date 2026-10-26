# capital-vol1

Typed data structures for the economic categories of Karl Marx's *Capital, Volume I* (1867).

## What this is

The repository models Marx's categories as immutable, strongly typed C# domain objects,
following the analytical order of the book. Every structure cites the chapter it formalises;
derived relations (e.g. the rate of surplus value) are computed from primitive quantities
rather than stored twice.

## Layout

| Path | Contents |
| --- | --- |
| `src/Capital` | The library — one namespace per analytical layer |
| `tests/Capital.Tests` | xUnit tests proving the invariants |

## Roadmap

| *Capital* Vol. I | Module | Status |
| --- | --- | --- |
| Part 1, ch. 1–3 — commodities, value, money | `Capital.Value` | started: `LaborTime`, `UseValue`, `Commodity`, simple form of value. Next: expanded/general/money forms, money (measure of value, circulation, hoard, means of payment, world money), circulation |
| Part 2, ch. 4–6 — general formula of capital, labour-power | `Capital.Circulation` | not started: C–M–C vs M–C–M′, contradiction of the general formula, labour-power as commodity |
| Part 3, ch. 7–11 — production of absolute surplus value | `Capital.Production` | not started: labour process vs valorization, constant/variable capital, surplus value, rate of surplus value s′ = s/v, the working day |
| Part 4, ch. 12–15 — production of relative surplus value | `Capital.Production` | not started: relative surplus value, co-operation, division of labour and manufacture, machinery and modern industry |
| Part 5, ch. 16–20 — wages | `Capital.Wages` | not started: value of labour-power presenting itself as the price of labour |
| Part 6–7, ch. 21–25 — accumulation | `Capital.Accumulation` | not started: simple and expanded reproduction, organic composition of capital, the general law of capitalist accumulation |
| Part 8, ch. 26–33 — primitive accumulation | `Capital.Accumulation` | not started |

## Modeling conventions

- **Value is abstract labour time.** Quantities of value are `LaborTime` (hours of socially
  necessary labour time): exact `decimal` arithmetic, never negative.
- **Money is never conflated with value.** A price is the money-expression of a value and will be
  its own unit type (Part 1, ch. 3) — not an alias for hours.
- **Qualitative categories are sealed records** (`UseValue`, `Commodity`); value equality means
  all components equal.
- **Alternative forms are sealed hierarchies with exhaustive `switch`** (the forms of value, the
  forms of capital) so adding a form forces every consumption site to handle it.
- **Derived quantities are computed, not stored** (e.g. s′ = s/v will be a function of surplus
  value and variable capital).

## Building

Requires the .NET SDK pinned in `global.json` (.NET 10).

```sh
dotnet build
dotnet test
```
