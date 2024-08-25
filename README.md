# HULLBREAKER COMPANY - Ambigous fork

All Credits go to Venterok! This is the original mod [**HullBreaker Company**](https://thunderstore.io/c/lethal-company/p/Venterok/HullBreaker_Company/) and [**HullBreaker Company on GitHub**](https://github.com/Venterok/HullBreakerCompany). Check it out!

Please **do not** report issues with this fork to Venterok. Report them to me on [GitHub](https://github.com/YoBii/HullBreakerCompany/issues)!

I forked this mod for personal use / tweaking to personal preference. Initially I just changed some strings and made a few small modificiations to the mod's logic.
Since then more significant changes have been made.

## Major changes

### Ambiguous event messages
All event long and short messages in game chat are more ambiguous i.e. less obvious or rather 'more mysterious'. This is intended to reduce spoiling specific monster types and such for newcomers.

Basically I tried to fit the whole "Notes about this moon"-thing without giving away the exact event.
For example all events that increase monster spawn rates can print the same message in game chat: telling you something about a dominant species.
So even if you know all the events by heart you don't necessarily know whether it's spiders, lizards, slimes or bees you have to expect.

### More events
Events, events, events. Can one have enough events?
* More scrap events like 'Jar of pickles'-event. Some make your working shift harder, some make it easier.
* HordeModeEvent increases spawn rate of inside enemies.
* Time dilation events. Make your day shorter or longer.

You can find a full list of all events below.

### Reworked event execution
Rewritten event execution logic allows to dynamically replace events that fail by exectuting another random event instead.
Events fail on certain moons where the enemies or scrap they are trying to modify can't naturally spawn.

### More impactful enemy events
The goal for all events is to make them feel unique but still balanced and manageable.
So instad of increasing only the rarity of a certain enemy, additionally their max count can be increased and their power level decreased.
For example where before the spider event would only guarantee a spider to spawn, it now makes spiders more likely to spawn (early), allows encountering multiple spiders in the same round and doesn't displace other enemy spawns.
That means other enemies *can* still spawn additionally but they will spawn in later. Not all at once because we still spawn naturally.

### Integrated mod events
Depending on whether you have other mods installed certain events are enabled automatically. No user interaction or configuration required.
Introducing events for Shy guy, Herobrine, FacilityMeltdown and more. Check the changelog.

### Custom Events
* added support for modded enemies
	* make sure you use the enemy names as they're printed in logs (case insensitive)
	* only works for mods that don't use custom spawn logic
* added support for daytime enemies
* added support for setting max count and power levels for all enemy types
* added support for changing the level's power levels (inside, outside, daytime)
* added support for overriding the level's spawn curve to a constant value  (inside, outside, daytime)

#### IMPORTANT
With v2.2.0 the format of custom event config files has changed. Please update your configs. Refer to the template below.

#### Custom Event Template

The template file is located in your Hullbreaker plugins folder `profilename\BepInEx\plugins\explodingMods-HullBreaker_Company_Ambigous_Fork\HullEvents`
<details>
	<summary>Show Custom Event Template</summary>

```
# CustomEvent Template
# I recommend you create a copy of this template and edit that.
# The filename doesn't matter. You can name it whatever you like.

# Change the line below to [ENABLED] or delete it to enable this custom event. 
[DISABLED]

# EVENT ID - REQUIRED
# The internal event name
EventID = CustomEventName

# EVENT WEIGHT - REQUIRED
# Default weighted rarity of the event
EventWeight = 0

# MESSAGES - REQUIRED
# The long form message printed to in-game chat when the event is active
# You can add multiple messages separated by semicolon (first message; second message; ..)
InGameMessage = My first custom event chat message; My second custom event chat message

# SHORT MESSAGES - REQUIRED
# The short form message printed to in-game chat when the event is active
# You can add multiple messages separated by semicolon (FIRST; SECOND; ..)
InGameShortMessage = FIRST; SECOND; THIRD

# ENEMY LISTS
# Enemies followed by their modifiers, seperated by comma
# Event will be skipped when ONE or more of these are missing from the level's enemy list
# Format: enemyName:rarity:maxcount:power, ..
# enemyName: the name of the enemy. Must be identical to what's printed in hullbreaker logs/console
# rarity: the rarity to set (in % of the total enemy rarity i.e. 100 makes it 50:50), -1 to not change
# maxcount: spawn up to this amount, -1 to not change, can be omitted
# power: override the enemy's power level, -1 to not change, can be omitted

# INSIDE ENEMIES - OPTIONAL
# EXAMPLE: Centipede:100:10:-1, SandSpider:100:8:1
SpawnableEnemies = enemyName:rarity:maxcount:power, enemyName:rarity:maxcount:power

# OUTSIDE ENEMIES - OPTIONAL
# EXAMPLE: MouthDog:100:20:1, SandWorm:100:5:0
SpawnableOutsideEnemies = enemyName:rarity:maxcount:power, enemyName:rarity:maxcount:power

# DAYTIME ENEMIES - OPTIONAL
# EXAMPLE: RedLocustBees:-1:10:0
SpawnableDaytimeEnemies = enemyName:rarity

# SCRAP LIST - OPTIONAL
# List of scrap items and their rarity, seperated by comma
# Event will be skipped when ALL of these are missing from the level's loot table
# scrapName = the name of the scrap item. Must be identical to what's printed in hullbreaker logs/console
# rarity = the rarity to set (in % of the total scrap rarity i.e. 100 makes it 50:50) 
# EXAMPLE SpawnableScrap = Big Bolt:10, Cookie mold pan:20, Teeth:50
SpawnableScrap = scrapName:rarity

# MAX ENEMY POWER - OPTIONAL
# Increase the level's inside monster power cap by this number. Set to 0 to disable
GlobalPowerIncrease = 0

# MAX OUTSIDE ENEMY POWER - OPTIONAL
# Increase the level's outside monster power cap by this number. Set to 0 to disable
GlobalOutsidePowerIncrease = 0

# MAX DAYTIME ENEMY POWER - OPTIONAL
# Increase the level's daytime monster power cap by this number. Set to 0 to disable
GlobalDaytimePowerIncrease = 0

# ENEMY SPAWN RATE OVERRIDE - OPTIONAL
# Override the global spawn rate to a constant value. Think number of enemies to spawn per wave. 256 will spawn everything instantly. Set to 0 to disable
GlobalInsideSpawnRateOverride = 0 

# OUTSIDE ENEMY SPAWN RATE OVERRIDE - OPTIONAL
# Override the global outside spawn rate to a constant value. 256 will front load the outside enemy spawning. Set to 0 to disable
GlobalOutsideSpawnRateOverride = 0

# DAYTIME ENEMY SPAWN RATE OVERRIDE - OPTIONAL
# Override the global daytime spawn rate to a constant value. Think number of enemies to spawn per wave. 256 will spawn everything instantly. Set to 0 to disable
GlobalDaytimeSpawnRateOverride = 0
```
</details>

### Compatibility
Fully compatible with LethalQuantities and AdvancedCompany overrides.
<details>

Using these mods you can allow events that are otherwise not available on certain moons by setting the respective monster or scrap rarity to at least `1`.
For example with vanilla moon configuration you can't get Jester event on Experimentation because Jesters don't spawn there.
Using the tool of your choice set the Jester's rarity to `1` or any larger number. This will enable the Jester event on that moon.
</details>

### Config file
Reorganized and restructured - allowing for more customization. Hopefully you'll find some settings more intuitive.
Mainly putting this here so you remember to check your config :)


## Events

### Enemy events
<details>
	<summary>Click to expand</summary>

All enemy events increase the respective enemy's spawn chance. Some additionally increase their max count and decrease their power level to varying degrees.

They are designed to be impactful and create unique situations but still be manageable overall. Check your logs for what is changed exactly. 

Event | Details
------ | ------
Arachnophobia   | Bunker spiders are more likely to spawn. There can be two or even more. Recommended mod: Arachnophilia
Bee   | Bee hives are more likely to spawn and you can find a lot of them. This also increases daytime enemy spawns overall (take note LQ users).
Butler   | Butler spawns more likely and there's potentially more of them. They don't count to overall enemy cap.
Crawler  | Crawler spawns more frequently and has reduced power level. Max count unchanged (vanilla is 4)
DevochkaPizdec  | Ghost girls spawn more frequently and there can be more than one. Don't loose your head out there.
FlowerMan   | Brackens spawn more frequently and in larger quantities. They often move together but can separate and give you a really bad time.
Hell   | Jesters. Yes, multiple. Good luck.
HoarderBug   | Hoarding Bugs spawn a lot more frequently and in much larger quantities. It's a race for scrap - who will capture the most?
Lizards   | Puffers / Spore Lizards are more likely to spawn and there's more of them. They don't count to overall enemy cap.
Masked   | Masked enemy are more likely to spawn and there's more of them. They don't count to overall enemy cap.
Nutcracker   | Nutcrackers are more likely to spawn and there's more of them. They don't count to overall enemy cap.
Slime   | Hygrodere / Blobs spawn more frequently and in much larger quantities. Recommended mod: RandomSlimeColor, RandomEnemiesSize.
SpringMan   | Coil-heads are more likely to spawn and there can be a lot of them. Keep you eyes peeled.
</details>

### Scrap events
<details>
	<summary>Click to expand</summary>

Scrap events generally change the weights of certain scrap in the loot table to make them more common.

Most of these have been made with ImmersiveScrap in mind. They work perfectly fine without but balance might be off.

Event | Details
------ | ------
Armday   | Incerases spawn chance of heavy loot
BabkinPogreb   | Spawns a lot of pickle jars
ChristmasEve   | Spawns a lot of presents
Clownshow (Girl)   | Spawns a lot of scrap that can make noise like horns
DayDrinking   | Spawns a lot of alcoholic beverages
Easter   | Spawns a lot of Easter Eggs
LuckyDay   | Increases chance for very valuable loot to spawn
SelfDefense   | Spawns a lot of weapon-like scrap items
</details>


### Miscellaneous events
<details>
	<summary>Click to expand</summary>

Traps events, time events and even some gameplay changing events.

You might need to change your strategy so keep your eyes open for these!

Event | Details
------ | ------
EnemyBounty   | The company pays money for killing monsters. Per enemy reward + final reward (configurable, read below)
HackedTurrets   | All turrets are permanently disabled
HordeMode   | Spawns enemies early and in large amounts
HullBreak   | Take a break. The company sends a bonus payment
LandMine   | Spawns a lot of landmines
NothingEvent | This is used to balance the chance of any event occurring. This event = no event
OnAPowderKeg  | Spawns more landmines. Landmines will randomly explode throughout the day.
OneForAll  | When the first crew member dies, the ship's autopilot is immediately instructed to leave (like voting, you have two hours)
OpenTheNoor  | All big security doors start in closed state
OutSideEnemyDay  | Frontloads outside enemy spawns to occur early in the day. Like eclipsed but only outside.
SpikeTrap  | Spawns a lot of spike traps (might have no effect in some custom interiors)
TimeAnomaly  | Time passes faster
TimeDilation  | Time passes slower
TurretEvent  | Spawns a lot of turrets

</details>

### Integrated events
<details>
	<summary>Click to expand</summary>

List of events integrated with others mods (from all event categories). 

Event | Required Mod | Details
------ | ------ | ------
Boomba   | LethalThings | Increases boomba spawn chance and max count significantly.
Herobrine   | HerobrineMod | Increases Herobrine spawn chance. Can spawn  up to 2. Doesn't count towards power level.
ShyGuyEvent  | Scopophobia | Increases ShyGuy spawn chance and allows more to spawn - drastically increasing chance of actually finding one.
Meltdown   | FacilityMeltdown | Will trigger the meltdown event sometime during the day (random between early noon and midnight)
AC_Bunny  | AdvancedCompany | Increases spawn chance of AdvancedCompany unique item: Bunny ears
AC_Controller  | AdvancedCompany | Increases spawn chance of AdvancedCompany unique item: Controller (Pietsmiet)
AC_RGBShoes  | AdvancedCompany | Increases spawn chance of AdvancedCompany unique item: Light Shoes
BruceAlmighty  | Surfaced | Guarantees Bruce spawn outside. Spawns up to three. 
SeaMine  | Surfaced | Spawns a lot of old sea mines. Uses LandmineScale config.
Urchin | Surfaced | Spawns up to ten Urchin early in the round. 
InvisibleGuest  | SCP966 | Increases SCP966 spawn chance, spawns up to three.
SCP106  | SCP106 | Increases SCP106 spawn chance, spawns up to three.
Eggman  | SCP3199 | Increases SCP3199 outside spawn chance, spawns up to four.
SlimyFriend  | SCP999 | Increases SCP999 spawn chance, spawns up to three and less other monsters spawn.
</details>

## Other changes

### Events

Changes to existing events from the original Hullbreaker mod.
<details>
	<summary>Click to expand</summary>

#### EnemyBountyEvent
* The amount of credits rewarded for each kill is now random. Similar to an above average scrap item
	* Min/Max is configurable
* The amount rewarded will show in game chat
* New config: Limit the amount of rewards. The final reward will pay 150% of Max
 
#### HullBreakEvent
* The amount of bonus credits you receive is now random
	* Min/Max is configurable
* The amount will show in game chat

#### NothingEvent
* When the mod randomly selects _NothingEvent_ (no event) the event message is omitted from chat. With event counts this makes it so events seem more random especially when combined with the option that increments the number of events every day. With high `EventCount` it simply prevents flooding the chat with empty lines from NothingEvent to a point where things become unreadable. This depends on your configuration (event count and event weights) of course
* When all events on a given day are NothingEvent the entire NOTES ABOUT MOON section is omitted from game chat

#### OnAPowderKegEvent
* Now explodes mines periodically. Respects custom day lengths

#### Readded events
* TurretsEvent, OneForAllEvent, MaskedEvent
</details>

### Other
#### HullBreakerLevelSettings
* only changes enemy count and spawn rate
* no longer touches scrap amount, scrap value or quota