## Until Clock v1 - Sprint-Only Plan Variant

This variant contains only sprint-sized execution tickets.
Constraints: 1-week sprints, 2-3 developers, ticket size 0.5-1 day, TDD per ticket (RED -> GREEN -> BLUE), >=80% unit test coverage.

## Sprint 1 (Week 1)
Goal: WinUI 3 baseline, quality gates, core model primitives.

1. TKT-S1-01 - Add WinUI 3 project shell and solution wiring
Estimate: 1 day
Depends on: none
DoD: solution builds with WPF + WinUI projects side-by-side
TDD: RED smoke expectation, GREEN startup wiring, BLUE cleanup

2. TKT-S1-02 - Add migration boundary doc for shared code ownership
Estimate: 0.5 day
Depends on: TKT-S1-01
DoD: clear mapping of reuse vs adaptation areas
TDD: checklist-based validation

3. TKT-S1-03 - Enforce >=80% coverage gate in pipeline
Estimate: 0.5 day
Depends on: none
DoD: test command fails below threshold and publishes coverage artifacts
TDD: RED threshold breach check, GREEN gate config, BLUE command cleanup

4. TKT-S1-04 - Add PRD traceability seed matrix
Estimate: 0.5 day
Depends on: none
DoD: each PRD section mapped to planned test suites
TDD: review-based acceptance

5. TKT-S1-05 - Add CountdownStartDateTime and completion semantics
Estimate: 1 day
Depends on: none
DoD: non-negative remaining time and clear complete/incomplete transitions
TDD: RED past/future boundary tests, GREEN model changes, BLUE naming cleanup

6. TKT-S1-06 - Add day/hour/minute progress helper math with clamp 0..1
Estimate: 1 day
Depends on: TKT-S1-05
DoD: deterministic helper API with edge-bound coverage
TDD: RED table-driven edge tests, GREEN math implementation, BLUE extraction

Sprint 1 exit:
1. Build + tests green
2. Coverage gate active
3. Domain primitives ready for persistence/UI binding

## Sprint 2 (Week 2)
Goal: Persistence, first-launch behavior, minute-boundary timer correctness.

1. TKT-S2-01 - Create persistence contract and JSON file implementation
Estimate: 1 day
Depends on: Sprint 1 complete
DoD: save/load TargetDateTime + CountdownStartDateTime under LocalAppData
TDD: RED roundtrip tests, GREEN implementation, BLUE path/config cleanup

2. TKT-S2-02 - Add corrupt/missing file recovery behavior
Estimate: 0.5 day
Depends on: TKT-S2-01
DoD: malformed/absent JSON falls back to first-launch state without crash
TDD: RED malformed payload tests, GREEN fallback handling, BLUE exception narrowing

3. TKT-S2-03 - Add interruption-safe/atomic save strategy
Estimate: 0.5 day
Depends on: TKT-S2-01
DoD: robust write behavior using temp + replace or equivalent
TDD: RED partial-write scenario, GREEN safe write path, BLUE IO helper cleanup

4. TKT-S2-04 - Rework timer to minute-boundary aligned ticks
Estimate: 1 day
Depends on: Sprint 1 complete
DoD: updates align to real minute boundaries, avoid second-interval drift
TDD: RED alignment tests with fake clock, GREEN scheduler, BLUE timer abstraction cleanup

5. TKT-S2-05 - Add anti-drift long-session tests
Estimate: 0.5 day
Depends on: TKT-S2-04
DoD: simulated 1+ hour run remains boundary-aligned
TDD: RED drift scenario, GREEN scheduling adjustments, BLUE fixture consolidation

6. TKT-S2-06 - Wire startup load path into MainViewModel
Estimate: 1 day
Depends on: TKT-S2-01 and TKT-S2-04
DoD: startup enters correct first-launch/resume state automatically
TDD: RED startup-state tests, GREEN orchestration, BLUE dependency cleanup

7. TKT-S2-07 - Persist on create/edit transitions
Estimate: 0.5 day
Depends on: TKT-S2-06
DoD: all target updates immediately saved
TDD: RED command persistence tests, GREEN save call wiring, BLUE flow dedupe

Sprint 2 exit:
1. Persistence resilient and restart-safe
2. Timer minute-boundary accurate
3. Startup behavior PRD-compliant

## Sprint 3 (Week 3)
Goal: Complete user interaction flow and WinUI 3 visual structure.

1. TKT-S3-01 - Implement set target confirm flow
Estimate: 1 day
Depends on: Sprint 2 complete
DoD: confirm sets target, resets countdown start, immediate UI update
TDD: RED command outcome tests, GREEN implementation, BLUE state refactor

2. TKT-S3-02 - Implement cancel path for date edit
Estimate: 0.5 day
Depends on: TKT-S3-01
DoD: cancel makes no state/persistence changes
TDD: RED cancel tests, GREEN no-op path, BLUE guard reuse

3. TKT-S3-03 - Add completion-state transition behavior
Estimate: 0.5 day
Depends on: TKT-S3-01
DoD: now >= target -> complete state, zero values, restart persistence
TDD: RED boundary tests, GREEN state transitions, BLUE state machine cleanup

4. TKT-S3-04 - Build three-unit horizontal WinUI layout
Estimate: 1 day
Depends on: Sprint 2 complete
DoD: Days/Hours/Minutes evenly spaced and stable during value width changes
TDD: logic validated in ViewModel tests + manual layout checklist

5. TKT-S3-05 - Add target datetime display + edit affordance
Estimate: 0.5 day
Depends on: TKT-S3-04 and TKT-S3-01
DoD: human-readable target text under countdown with edit trigger
TDD: RED formatting/binding tests, GREEN binding hookup, BLUE helper cleanup

6. TKT-S3-06 - Apply dark theme + blue accent (#007FFF)
Estimate: 0.5 day
Depends on: TKT-S3-04
DoD: dark background and accent styling for core countdown visuals
TDD: style checklist + unchanged logic suite remains green

7. TKT-S3-07 - Add completion-state color inversion
Estimate: 0.5 day
Depends on: TKT-S3-03 and TKT-S3-06
DoD: completion sets blue background and dark foreground/rings
TDD: RED complete-state property tests, GREEN visual state binding, BLUE style trigger cleanup

Sprint 3 exit:
1. Create/edit/cancel/complete flows working
2. WinUI layout + theming meets PRD baseline
3. Completion visuals persist across restart

## Sprint 4 (Week 4)
Goal: Circular progress polish, GUI automation, release hardening.

1. TKT-S4-01 - Bind and render minute progress ring
Estimate: 0.5 day
Depends on: Sprint 3 complete
DoD: minute ring reflects 0-60 scope and updates each minute
TDD: RED minute progress tests, GREEN binding/render wiring, BLUE utility cleanup

2. TKT-S4-02 - Bind and render hour progress ring
Estimate: 0.5 day
Depends on: TKT-S4-01
DoD: hour ring reflects 0-24 scope
TDD: RED hour progress tests, GREEN implementation, BLUE dedupe

3. TKT-S4-03 - Bind and render day progress ring
Estimate: 1 day
Depends on: TKT-S4-02
DoD: day ring uses CountdownStartDateTime baseline across total duration
TDD: RED elapsed-duration tests, GREEN implementation, BLUE precision cleanup

4. TKT-S4-04 - Validate scaling resilience (100/125/150)
Estimate: 0.5 day
Depends on: TKT-S4-03
DoD: no clipping/overlap at required scales
TDD: manual checklist + supporting ViewModel checks

5. TKT-S4-05 - Create WinAppDriver automation harness
Estimate: 1 day
Depends on: Sprint 3 complete
DoD: harness launches app and finds key controls
TDD: RED failing smoke automation, GREEN setup, BLUE selector stabilization

6. TKT-S4-06 - Automate first-launch set-target flow
Estimate: 0.5 day
Depends on: TKT-S4-05
DoD: end-to-end GUI test for initial setup path
TDD: RED flow assertion, GREEN interactions, BLUE robust waits/selectors

7. TKT-S4-07 - Automate edit + completion persistence flows
Estimate: 1 day
Depends on: TKT-S4-06
DoD: GUI tests cover edit action, completion visuals, restart persistence
TDD: RED failing scenarios, GREEN automation, BLUE helper extraction

8. TKT-S4-08 - BLUE refactor pass for service/ViewModel seams
Estimate: 1 day
Depends on: all Sprint 4 feature tickets
DoD: cleaner boundaries, no behavior changes, all tests green
TDD: existing suite only

9. TKT-S4-09 - PRD traceability closure + release checklist
Estimate: 0.5 day
Depends on: TKT-S4-08
DoD: each PRD acceptance criterion linked to automated or manual validation
TDD: documentation closure

Sprint 4 exit:
1. Circular progress complete and stable
2. Critical GUI automation passing
3. Coverage >=80% and release checklist signed off

## Global Constraints
1. Each feature ticket follows RED -> GREEN -> BLUE.
2. Coverage must remain >=80%.
3. Out-of-scope v1 items remain excluded: multi-countdown, notifications, cloud sync, advanced settings.
