# TaleScrawl.UnitTests

Unit tests using NUnit with BDD-style testing conventions.

## Dependencies
- **References:** TaleScrawl.Core
- **Packages:** NUnit, LeapingGorilla.Testing (BDD-style framework), coverlet

## Conventions
- Uses BDD-style test classes with **When/Then/Given** naming patterns (via LeapingGorilla.Testing)
- Test directories mirror the project structure being tested (e.g. `Ds/` tests `TaleScrawl.Core/Ds`)
- Parser tests under `Ds/`, geometry tests under `Geometry/`, slab tests under `Slab/`
- Focuses on pure business logic — no database or network dependencies
- Test classes follow the pattern: `When<Action>Then<ExpectedOutcome>
- Each method under test has its own folder: `ReadTests`, `FetchByIdTests`
- Each method under test has its own base class: `WhenTestingRead`, `WhenTestingFetchById`
  - Each base method class inherits from the item under test's base class
### Naming
- [When] methods are named `<Action>IsCalled`
- [Then] methods are named `Should<Outcome>`.
  - `ShouldNotThrow`, `ShouldReturnFoo`, `ShouldReturnNull`

