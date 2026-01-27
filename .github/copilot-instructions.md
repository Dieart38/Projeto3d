# Copilot Instructions for ProjetoZelda

## Project Overview
ProjetoZelda is a Zelda-inspired game project built with **Unity 6000.0.61f1** in C#. It implements core action-adventure mechanics: player movement/combat, enemy AI, interactive environmental objects, and dynamic camera systems. The architecture is MonoBehaviour-driven with physics-based interactions.

## Architecture & Core Patterns

### Communication System: SendMessage
The codebase uses **Unity's `SendMessage()` for cross-component communication**:
- `PlayerController` attacks via `Physics.OverlapSphere()` to find colliders in range, then sends `GetHit(damageAmount)` message
- Receiving objects (`Grass`, `SlimeA`) implement `void GetHit(int amount)` to handle damage
- **Pattern**: If adding new damageable objects, implement `GetHit(int amount)` method

Example (from `PlayerController.cs` lines 56-60):
```csharp
hitInfo = Physics.OverlapSphere(hitBox.position, hitRange, hitMask);
foreach (Collider c in hitInfo)
{
    c.gameObject.SendMessage("GetHit", amountDamage, SendMessageOptions.DontRequireReceiver);
}
```

### Component Structure
- **PlayerController**: Handles input (WASD movement, mouse click attack), character rotation toward movement direction, animation states
- **Grass**: Environmental interactive object with sway animation, hit response with particle effects, respawn coroutine
- **SlimeA**: Enemy with HP system, hit animation, blood particle effect, death trigger
- **DynamicCam**: Camera switching via trigger zones (activated by "CamTrigger" tags)

### Animation System
Objects use Animator components with triggers/bools:
- `isWalk` (bool): Movement animation state
- `Attack` (trigger): Attack animation
- `GetHit` (trigger): Damage reaction animation
- `Die` (trigger): Death animation

## Project Conventions

### File Organization
- **Scripts/**: All gameplay logic (player, enemies, interactive objects, camera)
- **Prefabs/**: Reusable components (`Grass.prefab`, `GrassGroup.prefab`, `CamTrigger.prefab`)
- **Scenes/**: Game scenes (main scene is `Estudos.unity`)
- **Settings/**: Project-specific configurations
- **BloodDecalsAndEffects/**: Visual effect assets

### Naming & Header Organization
- Use `[Header("Category")]` to group editor properties (e.g., `[Header("Attack Config")]`)
- Private fields are managed internally; public fields are exposed for tweaking in Inspector
- Coroutines use descriptive names: `RegrowGrass()`, `yield return new WaitForSeconds()`

### Physics Setup
- Uses **CharacterController** for player movement (not Rigidbody-based)
- Uses **Physics.OverlapSphere()** for melee attack detection
- **LayerMask** determines what can be hit by attacks (`hitMask` field)

## Key Workflows

### Adding Damage-Receiving Objects
1. Create script inheriting `MonoBehaviour`
2. Implement `void GetHit(int amount)` method
3. Add to layer matching `PlayerController.hitMask`
4. Handle damage logic internally (health deduction, animations, effects)

### Particle Effects Pattern
Objects store public `ParticleSystem` references and call:
```csharp
if (fxHit != null) fxHit.Emit(particleCount);
```
Counts vary: Grass uses 20, SlimeA uses 40 for blood effect.

### Coroutine Patterns
**Respawn/Recovery** (from `Grass.cs`):
- Disable object temporarily (scale reduction)
- Wait for respawn time
- Smoothly interpolate back using `Vector3.Lerp()` in coroutine loop
- Reset flags and restore state

### Animator Triggering
Always check state before triggering to prevent conflicts:
```csharp
if (!isAttacking)
{
    Attack();  // Sets isAttacking = true and triggers animation
}
```

## Critical Integration Points

### Input System
- Currently uses legacy `Input` class (`GetAxis()`, `GetButtonDown()`)
- Note: Project has `InputSystem_Actions.inputactions` asset (New Input System) but not actively used
- If refactoring: Migrate to New Input System for consistency

### Camera System
- Uses trigger-based camera switching via `DynamicCam` script
- Objects tagged "CamTrigger" activate alternate cameras (`camB`)
- Useful for cinematic room transitions

### Enemy Pattern
`SlimeA` exemplifies enemy structure:
- HP field for health tracking
- Animation states for behaviors (hit, die)
- Particle effects for feedback
- Extend this pattern for new enemy types

## Common Tasks

- **Add new enemy**: Inherit pattern from `SlimeA` with `GetHit()` and death handling
- **Add environmental object**: Inherit from `Grass` pattern with `GetHit()` response
- **Adjust damage/balance**: Modify `PlayerController.amountDamage` and enemy `HP` values
- **Tweak animations**: Adjust trigger parameters in Animator and respond in scripts
- **Camera transitions**: Add "CamTrigger" tag and reference new camera in `DynamicCam`

## Notes for AI Agents
- Always check for null references when accessing components (especially ParticleSystem)
- Use `SendMessageOptions.DontRequireReceiver` to safely send messages to potentially non-receiving objects
- Coroutines are preferred for time-based logic; avoid updating state flags in Update() loops
- Maintain consistency with header organization and public/private field conventions for editor usability
