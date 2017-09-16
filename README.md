# Roguelike

This is a turn-based roguelike written in Unity I'm working on to experiment with various systems involved in roguelikes and RPGs. This includes procedural generation of levels, enemies and items, handling field of view (FOV), enemy AI, leveling, inventory management, turn management, etc. Many systems are modelled after corresponding systems in Diablo 2 and Path of Exile, though the game plays more like a traditional roguelike (Rogue, Nethack, Castle of the Winds, etc.) In addition to the systems themselves, I'm also exploring the use of tools (postprocessors, custom inspectors, property drawers, even full blown level editors) to enhance workflow. 

The game is playable (from the editor), but low on content and not terribly fun at the moment. Nonetheless, it may serve as a reference for those seeking to implement certain roguelike features. 

Last tested with Unity 2017.1.1f1.

![Roguelike Screenshot](https://imgur.com/vcWSwt6.png)

Item and creature sprites were taken from [here](http://pousse.rapiere.free.fr/tome/), shared under the Creative Commons License, and drawn by David E. Gervais. 

## Setup

If you wish to download and set up this project for yourself, then you'll need to do two things. First, copy all the folders in Assets into the project window of a new Unity project. Next, copy the assets in ProjectSettings into your project's ProjectSettings folder, located at the top level of your Unity project's directory (note that this is not accessible from the Unity editor itself), overriding the existing versions of those assets. These settings will copy over the tags, layers, custom input axes, etc. 

If sprites are appearing with a pink background, try re-importing the Sprites folder (right-click the folder, and select Reimport). Unity probably imported the sprites before my custom postprocessor which (among other things) converts the backgrounds to transparency. 

## Systems

This section briefly documents at a high level how various in-game systems are implemented. 

### GameBehaviour

GameBehaviour is a subclass of Unity's MonoBehaviour. Subclassing GameBehaviour instead of MonoBehaviour provides additional hooks for important events, such as when a new map is created, or when the player moves. It handles subscribing and unsubscribing to events so that this logic exists only in one place, reducing opportunity for error and making changes to the event system much easier. 

Event driven architecture is fairly error prone, but when used sparingly and carefully goes a long way in decoupling classes. 

### Time

Rather than having simple turns, the game runs on a custom time system. When dynamic entites such as enemies are instantiated, they automatically subscribe to certain game events, such as time passing. This results in a flow of logic along these lines:

1. Player takes an action (moves, attacks, etc.)
2. Player tells GameTime to increase time based on how long that action took.
3. GameTime broadcasts an event indicating that time has passed.
4. Subscribed entity checks if the current time exceeds the time for its next action.
4a. If so, entity acts, and increments its personal "time to next action" variable appropriately.
4b. If not, entity does nothing.

The purpose is to easily accommodate a rich notion of speed: fast monsters may move or attack several times each time a player moves, while slower ones may allow the player several actions before acting. It also allows different actions to take different amounts of time. 

Implementation notes:

GameTime is an instance class, but provides public static readonly access to the current time. This means any class can check the current time at any time, but only a class with a reference to the instance can change the time, and only the player has such a reference. 

Entities don't subscribe directly to GameTime's event. Instead, they implement the GameBehaviour class and override an appropriate method. 

### Tiles

The map consists of individual tiles stitched together. The obvious approach is to have a GameObject for every tile. This has a lot of advantages, but is a heavyweight approach since gameobjects carry a lot of overhead. Instead, I opted to create a simple mesh which has the tiles "painted" onto it as a texture. This is highly performant, but makes it much harder to manipulate individual tiles, creating some challenges (see FOV in particular).

Note: Unity will soon release its TileMap feature, which may be a far superior way to handle tiles in a roguelike.

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

This consists of four rooms, with the dashes representing connections from one room to another. 

For more information see my Atlas project, which generalizes this system.

In the future, I intend to implement a variety of different strategies for map generation, including more conventional ones. 

### Enemy spawning

At the moment, enemies are spawned in packs in a more or less uniformly random way, with some guaranteed space between them. Some proportion of packs are promoted to champions or elites, concepts taken from Diablo 2. Champions come in several varieties (Berserker, Fanatic, Champion, Ghostly, etc.) with different stat boosts. Elites are currently similar to champions, but I intend to model them after Diablo 2's elites (one special monster with randomly chosen affixes representing special abilities, with correspondingly buffed minions)

### Enemy AI

At the moment the AI is very simplistic. They have a range relative to the player where they do nothing. If the player is in range, they'll give chase until they become adjacent, at which point they try to attack. The algorithm to find a path is dijkstra's, which will probably be eventually replaced with A* (a more efficient algorithm, which is essentially dijkstra's plus a heuristic). 

It is worth noting that when computing a path, only the non-walkable areas of the map are used in the calculation. But when attempting to walk the path, a raycast is done to check if the position is unoccupied. If blocked by another enemy, it will also try the two adjacent spots to the blocked tile (e.g. if northwest is blocked, it will also try north and west). If those are blocked too, it will idle for a short time. The upshot is that an enemy will perform at most 3 raycasts per turn. 

If the player moves, or if an enemy has to take an alternate step, the previously calculated path is invalidated and has to be recalculated. 

### Enemy-Player interaction

This is an area where I see a lot of spaghetti code in online examples - player components reference enemy components which reference player components, and before you know it, everything has hooks into everything else. A few simple interfaces help avoid this problem.

When an enemy tries to walk into a player or vice versa, a raycast is done, and the object struck by that raycast is searched for an "IAttackable" component (a component implementing that interface). If found, an appropriate attack method is called on that component. 

The layer of indirection provided by an interface allows for the same piece of logic to handle quite a few different cases without explicitly coding them (player attacks enemy, enemy attacks player, enemy attacks other enemy, player attacks destructable environment, etc.). As long as the target has a component with an appropriate implementation of IAttackable, things will simply work. 

Nonetheless it may prove useful to have some more abstractions at work. Eventually I may change to the following system:

Some kind of "AttackResolver" object is given on instantiation to objects that can attack. AttackResolver takes an "Attack" object and a "Defense" object. When attacking, the attacker produces an Attack object, and gets a Defense object from the IAttackable, and passes them to the resolver to be handled. 

Enemy-Player interaction is a system that will need ongoing tweaks, improvements, additions, etc. This heavier-handed approach helps isolate changes to the Attack/Defense/AttackResolver classes rather than their consumers. It also helps cut down on code duplication: in the current system, every new implementation of IAttackable is responsible for resolving an attack. 

### Logging

Important actions (attacks, level ups, damage taken, etc.) are logged to the screen, in typical roguelike fashion. At the moment, Debug.Log is used for convenience, though it's wrapped behind an abstraction to allow a different implementation (one that will actually work in a real build) later. 

### Items

Items are defined by two hierarchies: templates and instances. Each subtype of ItemTemplate is a ScriptableObject specifying items that apply to a given inventory slot. e.g. WeaponTemplate, ShieldTemplate, ArmorTemplate, etc. Using scriptable objects for this allows many instances to be defined and maintained as separate assets in the editor. e.g. separate instances of WeaponTemplate for Scimitar, Longsword, Battle Axe, etc. This is a far more convenient and scalable approach than creating a new subclass for each of these instances. 

The other hiararchy represents the actual instantiated in-game items. Each ItemTemplate has an associated Item type associated with it. e.g. WeaponTemplate and Weapon, ShieldTemplate and Shield. The Item instance maintains a (read-only) reference to its template, implementing the flyweight design pattern (there may be many Scimitar instances, but only one Scimitar template shared by all Scimitars). Having a separate class for the instance as opposed to just using the template also allows us to put extra features on the instance specific to that instance, namely affixes. 

An affix system has not yet been implemented for items, but will be designed with Diablo 2's random item system in mind, with a randomly chosen prefix and suffix.

### Item drops

Enemies have a component that drops items on death, with a reference to an item class. An item class is a scriptable object with a list of items or other item classes, each with a weight. When it dies, a weighted random roll determines what (if anything) drops. 
