# Roguelike

This is a turn-based roguelike written in Unity I'm working on to experiment with various systems involved in roguelikes and RPGs. This includes procedural generation of levels, enemies and items, handling field of view (FOV), enemy AI, leveling, inventory management, turn management, etc. Many systems are modelled after corresponding systems in Diablo 2 and Path of Exile (particularly item and level generation), though the game plays more like a traditional roguelike (Rogue, Nethack, Castle of the Winds, etc.) 

The game is playable (from the editor), but low on content and not particularly interesting yet. Nonetheless, it may serve as a reference for those seeking to implement certain roguelike features. 

Targets .NET 4.6, and currently uses Unity 2017.3.0f3.

![Roguelike Screenshot](https://imgur.com/vcWSwt6.png)

Almost all item and creature sprites were taken from [here](http://pousse.rapiere.free.fr/tome/), shared under the Creative Commons License, and drawn by David E. Gervais. 

## Setup and Controls

No special setup is required: download the two folders (Assets and PlayerSettings) to a directory, open that directory in Unity, and you should be good to go. I will include an actual build in the future, when it makes to do so (the main reason not to do so at the moment is that the game's console implementation currently uses Debug.Log, which only works in the editor). 

Movement: Arrow keys or WASD (horizontal/vertical axes).
Pickup/Use item: G (Get axis).
Use stairs: Space (Exit axis).
Open map: M (Map axis).
Open inventory: I (Inventory axis).

Note: Documentation may become out of date. Up to date controls can always be determined by checking the Input Manager in the Unity Editor. 

## Systems

I document the various systems used in the game here. Those interested in implementing something similar may find it useful.

### Time

Rather than having simple turns, the game runs on a custom time system where player actions result in a certain amount of time passing. When dynamic entites such as enemies are instantiated, they automatically subscribe to changes in time. This results in a flow of logic along these lines:

1. Player takes an action (moves, attacks, etc.)
2. Player tells GameTime to increase time based on how long that action took.
3. GameTime broadcasts an event indicating that time has passed.
4. Subscribed entity checks if the current time exceeds the time for its next action.
5. If so, entity acts, and increments its personal "time to next action" variable appropriately. Otherwise idles.

The purpose is to easily accommodate a rich notion of speed: fast monsters may move or attack several times each time a player moves, while slower ones may allow the player several actions before acting. It also allows different actions to take different amounts of time. Moving is (normally) faster than attacking, and certain abilities may take longer than others.

It's also worth noting that this system would make it easy to switch to a real-time implementation, simply by having the game time automatically increment in an Update method instead of requiring the player to move. This is how Diablo 1 was switched to real-time after being designed as a turn-based game.

Implementation notes:

GameTime is an instance class, but provides public static readonly access to the current time. This means any class can check the current time at any time, but only a class with a reference to the instance can change the time, and only the player has such a reference. 

Entities don't subscribe directly to GameTime's event. Instead, they implement the GameBehaviour class and override an appropriate method. GameBehaviour handles subscribing/unsubscribing, so that logic exists in only one place.

### Tiles

The map consists of individual tiles stitched together. The obvious approach is to have a GameObject for every tile. This has a lot of advantages, but is a heavyweight approach since gameobjects carry a lot of overhead. Instead, I opted to create a simple mesh which has the tiles "painted" onto it as a texture. This is highly performant, but makes it much harder to manipulate individual tiles, creating some challenges (see FOV in particular).

### Field of View (FOV)

Field of view determines what the player can see from a given position. It's used for two things: determining visibility of enemies and other dynamic entities, and also for revealing the map as the player explores. 

The first half of this problem has a standard, efficient solution: recursive shadowcasting. Plenty of resources on this approach are available online, including visualizations and code samples. This determines which tiles near the player should be visible. 

The second half is trickier, because we're using a single mesh for the entire map, so we cannot simply toggle some sprite renderers like we would if each tile had its own game object. Instead, the approach was to build the entire mesh, cache some information about which vertices in the mesh correspond to coordinates in the map, and then clear out the triangles. Each tile consists of two triangles in the mesh, so revealing a tile is equivalent to adding two triangles, or 6 integers, to the triangles array in the mesh. Each turn we determine which new tiles we've discovered, then update the mesh accordingly. 

A fair bit of caching is used to optimize this process by avoiding unnecessary calculations whenever possible. 

Alternative approach 1: This would probably be simpler with a shader that uses its Color array to determine transparency and lighting. Instead of rebuilding the triangles array, we simply fiddle with the alpha of the appropriate values in the colors array. Not only would this support revealing new tiles, but it would also allow the darkening of previously explored tiles that are no longer visible, something my current approach does not permit.

Alternative approach 2: Although not yet available (as of this writing), the upcoming TileMap feature will likely support the necessary features to implement this type of system more easily. 

### Procedural map generation

Currently the map generation uses a maze-room approach that closely resembles the approach taken in Diablo 2. Individual level chunks are hand-painted. A maze algorithm determines the arrangement of rooms and their connections (each cell in the maze is one level chunk). For example, we might have the following maze:

```
0 - 0
    |
0 - 0
```

This consists of four rooms, with the dashes representing connections from one room to another. At the moment, I only have a few chunks, and only one type of level. This will be ramped up when I switch from working on systems to content. 

For more information see my Atlas project, which generalizes this system.

### Enemy spawning

At the moment, enemies are spawned in packs in a more or less uniformly random way, with some guaranteed space between them. Some proportion of packs are promoted to champions or elites, concepts taken from Diablo 2. Champions come in several varieties (Berserker, Fanatic, Champion, Ghostly, etc.) with different stat boosts. Elites are currently similar to champions, but I intend to model them after Diablo 2's elites (one special monster with randomly chosen affixes representing special abilities, with correspondingly buffed minions)

### Enemy AI

At the moment the AI is very simplistic. They have a range relative to the player where they do nothing. If the player is in range, they'll give chase until they become adjacent, at which point they try to attack. The algorithm to find a path is dijkstra's, which will probably be eventually replaced with A* (a more efficient algorithm, which is essentially dijkstra's plus a heuristic). 

It is worth noting that when computing a path, only the non-walkable areas of the map are used in the calculation. But when attempting to walk the path, a raycast is done to check if the position is unoccupied. If blocked by another enemy, it will also try the two adjacent spots to the blocked tile (e.g. if northwest is blocked, it will also try north and west). If those are blocked too, it will idle for a short time. The upshot is that an enemy will perform at most 3 raycasts per turn. 

If the player moves, or if an enemy has to take an alternate step, the previously calculated path is invalidated and has to be recalculated. 

### Enemy-Player interaction

This is an area where I see a lot of spaghetti code in online examples - player components reference enemy components which reference player components, and before you know it, everything has hooks into everything else.

When an enemy tries to walk into a player or vice versa, a raycast is performed, and the object struck by that raycast is searched for an "IAttackable" component (a component implementing that interface). If found, the attacker produces an "Attack" object, and the attacked produces a "Defense" object. These are POCOs (plain old CLR object) that produce all the information needed to resolve the attack (damage, health, defenses, crit chance, crit multiplier, resistances, accuracy, etc.). These objects are passed off to an "AttackResolver" which performs all the calculations for the attack, and returns an "AttackResult". This result is logged to the console and applied to the attacked target.

This is a system that will be regularly modified, so having a centralized implementation with minimal indirection will make it easy to maintain, and having all the calculations performed by AttackResolver makes it easy to add new types of attacking or attackable entities (destructible terrain, new enemies, traps, etc.). An attackable entity simply needs to be able to produce a Defense object, and an attacking entity needs to be able to produce an Attack object.

### Logging

Important actions (attacks, level ups, damage taken, etc.) are logged to the screen, in typical roguelike fashion. At the moment, Debug.Log is used for convenience, though it's wrapped behind an abstraction to allow a different implementation (one that will actually work in a real build) later. 

### Items

Items are defined by two hierarchies: templates and instances. Each subtype of ItemTemplate is a ScriptableObject specifying items that apply to a given inventory slot. e.g. WeaponTemplate, ShieldTemplate, ArmorTemplate, etc. Using scriptable objects for this allows many instances to be defined and maintained as separate assets in the editor. e.g. separate instances of WeaponTemplate for Scimitar, Longsword, Battle Axe, etc. This is a far more convenient and scalable approach than creating a new subclass for each of these instances. 

The other hiararchy represents the actual instantiated in-game items. Each ItemTemplate has an associated Item type associated with it. e.g. WeaponTemplate and Weapon, ShieldTemplate and Shield. The Item instance maintains a reference to its template, implementing the flyweight design pattern (there may be many Scimitar instances, but only one Scimitar template shared by all Scimitars). Having a separate class for the instance as opposed to just using the template also allows us to put extra features on the instance specific to that instance, namely affixes. 

### Affixes

The affix system was a tricky one to design. Affixes are randomized modifiers that can appear on items, e.g. Flaming Longsword of The Stars has the two affixes "Flaming" and "The Stars", and they provide a variety of effects. They can do anything from enhancing the item they appear on (e.g. increasing a weapon's damage by 37%), to increasing a player's attributes (e.g. +5 to strength), to casting a spell in response to an event (e.g. 7% chance to cast lightning nova when struck), to modifying all sources of damage (e.g. +10% to all fire damage), etc. Naturally, this is a system requiring frequent additions, removals and balance changes, which should be permitted as much as possible in the editor (i.e. without having to write or change code). 

Given the need to create/modify affixes in the editor, scriptable objects are an obvious choice. All affixes share a common abstract base class extending ScriptableObject, and various implementations handle affixes that behave fundamentally differently (AttributeAffix for affixes that improve some stat, SkillAffix for a skill that increases a skill, CombatAffix for an affix that triggers on acombat event, etc.). 

Assigning affixes to items is simply enough - we can simply store them in a collection on the item in question, and when an item is equipped, it iterates through the affixes can calls an "OnEquip" method which activates the enhancement provided by the affix. This is done by providing an "IEquipContext" object with methods to accept the various types of affects to the OnEquip method. When an item is unequipped, all affixes are flushed and rebuilt from the currently equipped items. 

### Player Attributes

The attribute system was another important system to design carefully, as there are a lot of attributes, and a lot of systems interacting with the attributes: item affixes, spell effects, environmental effects, stat allocations on level-up, base attributes (potentially with multiple classes), etc. 

Similar to the affixes, the attributes should be easy to add, remove, and balance. Enums are particularly easy to work with when it comes to serialization, the editor, and also in-code. They also make for very convenient dictionary keys for all kinds of tables. I settled upon using enums to represent attributes (and a number of other things) and wrote a special EnumDictionary class for the editor which automatically updates its keys so that it's always up to date with the enum: i.e. if I add an option to the enum, the dictionary will gain an entry in the editor corresponding to that key. Similarly, it will lose the corresponding entry if an enum option is removed. 

Note that since EnumDictionary is generic (works with any enum as a key) some trickery was needed to get Unity to draw it in the Editor. The trick is to have the generic version inherit from a non-generic base class, write a custom inspector for the base class, then create an empty subclass of the generic version, and use that subclass in scripts.

i.e.

```
[Serializable]
public abstract class EnumDictionary{}

[Serializable]
public abstract class EnumDictionary<T, K> : EnumDictionary
{
  // full implementation
}

[Serializable]
public sealed class IndexedAttributes : EnumDictionary<Attribute, int>{}
```

This kind of pattern appears in a few different places: a custom inspector is written for EnumDictionary, and it will be applied to all subclasses of EnumDictionary, bypassing two painful limitations with generics in Unity: can't serialize them, and can't define a custom inspector for them.

### Item drops

Enemies have a component that drops items on death, with a reference to an item class. An item class is a scriptable object with a list of items or other item classes, each with a weight. When it dies, a weighted random roll determines what (if anything) drops. Better tools are definitely needed to build/maintain item classes however, as this will quickly become a tangled mess as it stands. 
