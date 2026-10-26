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
| `src/Capital/Circulation` | Exchange, money functions, and circuits (ch. 2–4) |
| `src/Capital/Production` | Labour-power and the production of surplus value (ch. 6–11) |
| `src/Capital/Wages` | The transformed wage forms (ch. 16–21) |
| `tests/Capital.Tests` | xUnit tests proving the invariants |

## Roadmap

| *Capital* Vol. I | Module | Status |
| --- | --- | --- |
| Part 1, ch. 1–3 — commodities, value, money | `Capital.Value` | covered: the commodity and its value (`LaborTime`, `UseValue`, `Commodity`, `Quantity`, `CommodityLot`); all four forms of value (`ValueForm`, §3a–d; §4 fetishism documented on the type); money (ch. 2–3): `MoneyQuantity`, `Price`, `Sale`/`Purchase`/`CommodityCircuit`, `MediumOfCirculation`, `Hoard`, `Debt`/`Payment`, `WorldMoney`. Coin/token money not separately modeled |
| Part 2, ch. 4–6 — general formula of capital, labour-power | `Capital.Circulation` | started: `MoneyCircuit` (M–C–M′, ch. 4) composed from the same `Sale`/`Purchase`; the formula's contradiction documented on the type. Next: labour-power as commodity (ch. 6), the contradiction resolved in production |
| Part 3, ch. 7–11 — production of absolute surplus value | `Capital.Production` | covered: labour-power and subsistence (ch. 6) resolving the contradiction of M–C–M′ via `CapitalistCircuit` (M–C…P…C′–M′); the valorization process with c, v, s and product value = c + new value (ch. 7–8); rate s′ = s/v (ch. 9); mass S = v × s′ (ch. 11). The working-day minimum appears as an invariant; the length struggle (ch. 10) is not separately modeled |
| Part 4, ch. 12–15 — production of relative surplus value | `Capital.Production` | covered: `SurplusProfit` (individual vs social value — the motive of innovation, transient by construction) and `RelativeSurplusValue` (cheapened labour-power, fixed working day, s′ rises). Co-operation, division of labour/manufacture, and machinery (ch. 13–15) documented as the carriers of the productivity rise; fixed-capital depreciation not separately modeled |
| Part 5, ch. 16–21 — wages | `Capital.Wages` | covered: `TimeWage` (the apparent "price of an hour of labour"; market divergence from the value of labour-power representable — the engine of the illusion, ch. 17) and `PieceWage` (the converted form of the time-wage, `ConvertedFrom`, with its concealments documented). Deliberately no "value of labour" type: labour is the substance of value, not a commodity (ch. 16) |
| Part 6–7, ch. 21–25 — accumulation | `Capital.Accumulation` | not started: simple and expanded reproduction, organic composition of capital, the general law of capitalist accumulation |
| Part 8, ch. 26–33 — primitive accumulation | `Capital.Accumulation` | not started |

## Modeling conventions

- **Value is abstract labour time.** Quantities of value are `LaborTime` (hours of socially
  necessary labour time): exact `decimal` arithmetic, never negative.
- **Money is never conflated with value.** A price is the money-expression of a value and will be
  its own unit type (Part 1, ch. 3) — not an alias for hours.
- **Qualitative categories are sealed records** (`UseValue`, `Commodity`); value equality means
  all components equal.
- **Alternative forms are one sealed hierarchy** (`ValueForm` and its successors) **consumed only
  through `switch` expressions with an arm per form; the wildcard arm throws.** C# does not yet
  enforce exhaustiveness over class hierarchies, so the throwing default is the guardrail: a new
  form surfaces loudly at every consumption site instead of being silently dropped.
- **Derived quantities are computed, not stored** (e.g. s′ = s/v will be a function of surplus
  value and variable capital).

## Building

Requires the .NET SDK pinned in `global.json` (.NET 10).

```sh
dotnet build
dotnet test
```
