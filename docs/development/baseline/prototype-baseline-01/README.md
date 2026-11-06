# Prototype baseline 01 - visual evidence

Recorded: 2026-11-06 (submission date; capture timestamps were not independently established).

- Branch: `codex/m0-baseline-01`.
- Annotated baseline tag: `prototype-baseline-01`, on the commit containing this evidence.
- Gameplay source: `d2d40a9` (M0 Windows baseline and prototype audit). This evidence-only change does not modify gameplay, scenes, settings, or packages.
- Recorded Editor: Unity 6000.3.27f1; Input System 1.20.1 and URP 17.3.0 remain unchanged.

The developer supplied these screenshots as baseline evidence. They visibly include Unity's Game tab, Play-mode controls, and surrounding Editor UI. They are therefore catalogued as **Editor Game-view captures**, not screenshots of a standalone executable. The separate developer report that the Windows build looked good and basic commands worked remains in [playtest-notes.md](../../playtest-notes.md). No exact standalone build identity or per-command results can be inferred from these images.

Original bytes are preserved; only repository filenames use lowercase descriptive names to match the existing `*.png` Git LFS rule. No cropping, image editing, or generated imagery was used. SHA256SUMS.txt records hashes verified against the originals in Downloads.

| Repository file | Original filename | Visible evidence |
| --- | --- | --- |
| [editor-playground-overview.png](editor-playground-overview.png) | ghost-kurogane-capture-1.PNG | Placeholder player, ground, obstacles, and control instructions from a raised camera angle. |
| [editor-terminal-prompt.png](editor-terminal-prompt.png) | ghost-kurogane-capture-2.PNG | Player facing the terminal with the E: Access terminal prompt visible. Does not prove a completed interaction. |
| [editor-playground-platform.png](editor-playground-platform.png) | ghost-kurogane-capture-3.PNG | Player positioned above the low platform with surrounding graybox geometry. A still image does not prove a full jump/landing cycle. |

![Playground overview](editor-playground-overview.png)

![Terminal interaction prompt](editor-terminal-prompt.png)

![Player above the platform](editor-playground-platform.png)

These images complete M0's screenshots-or-gameplay-capture task. They do not close unreported standalone collision, interaction, cursor/focus, normal-exit/log checks, or the allocator diagnostic. No Unity tests or builds were rerun for this documentation-only update; prior results remain 7 EditMode and 5 PlayMode passes, with a successful Windows build and developer-reported basic playability.