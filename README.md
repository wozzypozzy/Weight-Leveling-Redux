# Weight Leveling Redux

A client-side SPT mod that makes your gear lighter as you level up.

`Weight Leveling Redux` is my fork of [Weight Leveling by SomeTransGirl](https://github.com/SomeTransGirl/WeightLeveling). It keeps the original idea but is now configurable and adds a couple robustness and performance improvements.

## How it works

Every item's own weight is multiplied by a factor based on your PMC profile level:

```
steps  = floor (level / Levels Per Step)
reduction = min (steps x Reduction Per Step (%), Max Reduction (%))
weight  = original weight x (1 - reduction)
```

## Settings

You can configure the mod via the BepInEx menu in-game (F12)

`Reduction Per Step (%)`
 - 0 - 100 || Percent of item weight removed for each step.

`Levels Per Step`
 - 1 - 100 || Number of levels that make up one step; a step being a block of levels. With `Levels Per Step = 5`, the weight reduction modifier is being applied every `5` levels.

`Max Reduction (%)`
 - 0 - 100 || Upper limit on the total reduction. 50 means items never weigh less than 50% of their normal weight.

## Installation

1. Copy the `BepInEx` folder into your SPT install folder, so you end up with:
   ```
   <SPT folder>/BepInEx/plugins/WeightLevelingRedux/WeightLevelingRedux.dll
   ```

## Behavior notes

- The reduction is based on your **PMC** level, including when you play a **Scav**.
- It applies to the weight of every item and is applied once per item. Container and stack totals are built from the reduced item weights, so a loaded backpack is not reduced twice.
- Grenade launcher attachments use their own weight calculation in the game and are not reduced.

## Changes from the original

- **Null safety:** the weight patch does nothing if the game's global configuration, client app, session or profile are not available yet, instead of throwing an exception from inside the weight getter.
- **Performance:** the weight multiplier is cached per experience value and only recalculated when your experience or a setting changes. The original mod recalculated your level on every weight lookup.
- something-something uhh **configurable**
- **Customizability**: added `Reduction Per Step (%)`, `Levels Per Step`, and `Max Reduction (%)` allowing the player to decide the strength of the modifier, how often that modifier is applied, and the maximum amount of weight that can be shaved off of items.

## Credits

- `SomeTransGirl`: author of the original Weight Leveling mod, which I forked from. The original is MIT licensed and its copyright notice is preserved