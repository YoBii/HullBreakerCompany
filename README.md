# About this fork

All Credits go to Venterok! This is the original mod [**HullBreaker Company**](https://thunderstore.io/c/lethal-company/p/Venterok/HullBreaker_Company/) and [**HullBreaker Company on GitHub**](https://github.com/Venterok/HullBreakerCompany). Check it out!

I forked this mod for personal use / tweaking to personal preference. I might or might not keep this up to date merging changes from upstream repo.

## This fork changes

### Ambigous event messages
* All event long and short messages in game chat are more ambigous 
* For example all events that increase monster spawn rate have the same message. Telling you something about a large number of likely hostile life forms. So even if you know all the events you don't know whether it's spiders, lizards or bees you have to expect on a given round. Basically I tried to fit the whole "Notes about this moon"-thing without giving away the exact event
### EnemyBountyEvent
* The amount of credits rewarded for each kill is now random. Ranging from 50-80 - similar to an above average scrap item
* The amount rewarded will show in game chat
### HullBreakEvent
* The event where you receive bonus credits now gives a random amount of credits (50-200)
* The amount will show in game chat
### NothingEvent
* When the mod randomly selects _NothingEvent_ (no event) the event message is omitted from chat. With low `EventCount` such as `1` this makes it so events seem to be more random especially when combined with the option that increments the number of events every day. With high `EventCount` it simply prevents flooding the chat with empty lines from NothingEvent to a point where things become unreadable. This depends on your configuration (event count and event weights) of course
* When all events on a given day are NothingEvent the entire NOTES ABOUT MOON section is omitted from game chat

# HULLBREAKER COMPANY

#### Making it more challenging to work for the company 💀

# INSTRUCTION 

Download on [Thunderstore](https://thunderstore.io/c/lethal-company/p/Venterok/HullBreaker_Company/)

This mod can only be installed on the host client.

Unpack the `venterok-hullbreaker_company-X.X.X.zip` archive into your game folder. Make sure to install [BepInEx](https://github.com/BepInEx/BepInEx) before. 

# FEATURES
Increased enemy spawn rate and difficulty.

Increasing quota per completed profit.

Upon landing on a moon, a few random events are selected and printed in the chat. This is referred to as `"Notes about the moon"📜`.

The notes contain information about the challenge, inhabitants of the planet, and cautionary messages.

Changeable HullBreaker values in config

Different event modes: Increase event amount per day / Const events for moon

Creating custom events!

# CREATING CUSTOM EVENT 

#### After v1.3.0 events may not work, you need to add `SpawnableOutsideEnemies = off` to your cfg

For now, custom events support only mode when you increase enemy spawn rate, but in the next update i will add more modes & functions. 

How can I create custom event?

1. Create new folder in `\Lethal Company\BepInEx\HullEvents` 
2. In folder create `.cfg` file with name of event, like `CrawlerEvent.cfg`
3. In `{NameEvent}.cfg` file write like this:
```cfg
[CustomEvent]

EventID = Crawler

EventWeight = 10

InGameMessage = Crawlers coming

InGameShortMessage = CRAWLER

SpawnableEnemies = crawler,hoarderbug

EnemyRarity = 128
```

`EventID` - ID of event, must be unique

`EventWeight` - Weight of event, its default value, you can change later in HullBreaker.cfg after event creation

`InGameMessage` - Message that will be printed in chat when event starts

`InGameShortMessage` - Short message, when short mode is on

`SpawnableEnemies` - Enemies that will be spawned, on your event, enemies must be in list down below, without quotes, separated by commas
```c#
SpawnableEnemies
    
{ "flowerman",}, // https://lethal.miraheze.org/wiki/Bracken
{ "hoarderbug",}, // https://lethal.miraheze.org/wiki/Hoarding_Bug
{ "springman",}, // https://lethal.miraheze.org/wiki/Coil-Head
{ "crawler",}, // https://lethal.miraheze.org/wiki/Thumper
{ "sandspider",}, // https://lethal.miraheze.org/wiki/Bunker_Spider
{ "jester",}, // https://lethal.miraheze.org/wiki/Jester
{ "centipede",}, // https://lethal.miraheze.org/wiki/Snare_Flea
{ "blobai",}, // https://lethal.miraheze.org/wiki/Hygrodere
{ "dressgirl",}, // https://lethal.miraheze.org/wiki/Ghost_Girl
{ "pufferenemy",}, // https://lethal.miraheze.org/wiki/Spore_Lizard
{ "nutcrackerenemy" }
{ "maskedplayerenemy" } //Not tested

SpawnableOutsideEnemies 

{ "eyelessdogs"}, //https://lethal.miraheze.org/wiki/Eyeless_Dog
{ "forestgiant"}, //https://lethal.miraheze.org/wiki/Forest_Keeper
{ "sandworm",}, //https://lethal.miraheze.org/wiki/Earth_Leviathan
{ "baboonbird",} //https://lethal.miraheze.org/wiki/Baboon_Hawk
```

`EnemyRarity` - Rarity of enemies, increase this value to increase enemies spawn chance

[Text Color for message](https://docs.unity3d.com/Packages/com.unity.textmeshpro@4.0/manual/RichTextColor.html)

### Examples of events 

CrawlerEvent.cfg
```cfg
[CustomEvent]

EventID = Crawler

EventWeight = 10

InGameMessage = Crawlers coming

InGameShortMessage = CRAWLER

SpawnableEnemies = crawler,hoarderbug

SpawnableOutsideEnemies = off

EnemyRarity = 128
```
BaboonEvent.cfg

```cfg
[CustomEvent]

EventID = Baboon

EventWeight = 10

InGameMessage = Baboon bird coming

InGameShortMessage = BIGBIRDS

SpawnableEnemies = off

SpawnableOutsideEnemies = baboonbird

EnemyRarity = 128
```

OffEyeLessDogEvent.cfg

```cfg
[CustomEvent]

EventID = OffEyeLessDog

EventWeight = 10

InGameMessage = No barking

InGameShortMessage = No barking

SpawnableEnemies = off

SpawnableOutsideEnemies = eyelessdogs

EnemyRarity = 0
```

EyeLessDogEvent.cfg

```cfg
[CustomEvent]

EventID = EyeLessDogs

EventWeight = 10

InGameMessage = Barking

InGameShortMessage = Barking

SpawnableEnemies = off

SpawnableOutsideEnemies = eyelessdogs

EnemyRarity = 1285
```
