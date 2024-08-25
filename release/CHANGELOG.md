# v2.2.4
#### Added 
* EasterEvent: Spawns a lot of Easter Eggs (vanilla scrap).
* Integrated Events (SCP)
	* Eggman Event: Increases SCP3199 outside spawn chance, spawns up to four.
	* InvisibleGuest Event: Increases SCP966 spawn chance, spawns up to three.
	* RottingMan Event: Increases SCP106 spawn chance, spawns up to three.
	* SlimyFriend Event: Increases SCP999 spawn chance, spawns up to three and less other monsters spawn.
* Integrated Events (Surfaced)
	* BruceAlmighty Event: Guarantees Bruce spawn outside. Spawns up to three. 
	* Urchin Event: Spawns up to ten Urchin early in the round.
	* SeaMine Event: Spawns a lot of old sea mines. Uses LandmineScale config.
* New config option for coloured event messages in chat.

#### Changed
* Adjusted Bee event spawn curve
* Updated README

#### Fixed
* Mod not loading correctly in LAN mode

# v2.2.3
* **(NEW)** Crawler Event: Spawns thumpers more frequently and reduces their power level.
* HackedTurrets Event now also spawns additional turrets according to your TurretScale setting
	* makes more sense thematically to actually see turrets when the event reads all of them have been disabled
	* you can also troll your friends. turrets during the event are indefinitely deactivated at round start but their state can still change. Use the terminal :)
* improved code to modify and restore trap spawns
	* fixed a bug where trap's spawn curves weren't correctly restored after finishing the day
	* allowing for more possibilities in the future (more integrated mod events)
* updated event messages
* updated config descriptions
* updated README

# v2.2.2
* added support for multiple long and short messages for custom events
	* messages are separated by semicolon (;)
	* updated `CustomEventTemplate.cfg`
* improved custom event config parsing
* improved event execution by adding a method to simulate all event modifications before queueing them up for application
	* fixes a bug where custom events with no or undefined scrap definitions would apply changes to enemies without being considered an 'active event' i.e. no message printed to chat, etc.
* improved random event selection code
	* previously under normal circumstances you could only get one NothingEvent per day i.e. effectively you only had a chance to reduce the total number of events per day by one
	* now NothingEvent can roll every time an event is selected
	* if you previously increased the NothingEvent weight to increase randomness / decrease frequency of events you might want to consider tuning it back down
* ignore case for enemy and scrap names (in custom events)
* updated event messages
* updated a few logs
# v2.2.1
readme changes only
* Added warning about breaking custom event changes
	* Once more: You have to update your custom event configs. The notation for enemies has changed.
* included custom event template
* fixed table formatting

# v2.2.0
* updated for v56 
	* last version was probably already compatible
	* this version is also still compatible with v50
* **(NEW)** SpikeTrapEvent: Spawn more spike traps. Amount configurable. 
* **(NEW)** Added integrated events for AdvancedCompany
	* Every unique AC item now has its own event (Light shoes, Bunny ears, Controller)
* **(NEW)** Updated custom event system. You can do almost anything with custom events now. Please see the README and custom event template for more information
	* **IMPORTANT** You have to update your custom event configs. The notation for enemies has changed.
	* Added support for multiple custom event folders. It will search for custom event cfg files in any folder called `HullEvents` inside your profile's BepInEx folder (including subfolders)
* added a % chance column to enemy and scrap log tables
* slightly modified how Hullbreaker events modify enemies and the level's power level
* added a few checks for events that spawn or modify traps
	* should fix OnAPowderKeg spamming errors when a round ends early
* updated event messages
* added logging when scrap rarities are recalculated due to unspawnable items
* other logging changes / typos fixed
* fixed an issue where modifications to a level's max power levels weren't correctly applied
* fixed an issue where the event weights section was duplicated in BepInEx config when the config file was newly generated / deleted
* removed FloodedLevelsFixed dependency. If you encounter any issues with flooded levels during time events please let me know.
* updated README

# v2.1.0 (based on HullBreaker_Company_v1.3.11)
* v50 compatibility
* **(NEW)** ButlerEvent
* **(NEW)** TimeAnomalyEvent: Time passes quicker (150-200% of default rate)
* **(NEW)** TimeDilationEvent: Makes time pass slower (50-75% of default rate)
* changed the way rarities are applied. Enemy and scrap rarities will now scale proportionally to the total rarities of the current level
	* This applies to **custom events**!
	* e.g. event A adds HoarderBug at 100 rarity. This will add HoarderBug at a rarity equal to the sum of all other monsters rarities resulting in every other enemy being a HoarderBug (on average)
	* Scrap events will automatically compensate for scrap that can't be spawned (missing from level's loot table)
		* The rarity of scrap that *can* spawn will increase proportionally to match the same overall event:normal scrap ratio
		* For example Event A spawns gold bar and cash register as 50% of total rarity each. If gold bar can't spawn, cash register will be added as 100% of total rarity.
* refactored event execution code
	* NothingEvent is no longer removed from pool of events after being rolled
* improved logging
* changed order of event weights in config so custom events show at the very bottom
* improved event descriptions in config section '3 - Event weights'
* fixed bunker spider event failing even if SandSpider is spawnable on the moon

# v2.0.2 (based on HullBreaker_Company_v1.3.11)
* added support for modded enemies to custom events
	* make sure you use the enemy names exactly as they're printed in logs (case sensitive)
	* only works for mods that don't use custom spawn logic
* changed `DaysPassed` to actually store the number of days that have passed in the current quota
	* avoids potentially confusing logs when `IncreaseEventCountPerDay = true`
	* doesn't affect anything other than logs

# v2.0.0 (based on HullBreaker_Company_v1.3.11)

### Core
* reworked the event execution logic
	* when an event can't be executed on the given moon another event will be randomly selected and executed
	* events can't execute when a moon doesn't naturally allow the enemy or scrap to spawn
		* e.g you can't get Jester event on (unaltered) Experimentation but you can get Girl and Nutcracker event
* reworked the chat messaging logic to be more flexible
* added a variety of event messages (long and short)
	* each event now has multiple possible 'Notes'. One of which is randomly selected on each execution. Some more, some less ambiguous.
* fancy enemy and loot logging (Host only)
	* this can be especially helpful when you're balancing custom moons or changing things with LQ or AC

#### Compatibility
* fully compatible with LethalQuantities and AdvancedCompany
	* if you set custom enemy or scrap rarities for a moon using these mods it will allow relevant events
		* i.e. when you set Jester rarity to 1 on Experimentation you *can* get the Jester event where otherwise you could not
* Implicit compatibility with ImmersiveScrap, wesley's moons and other mods that add scrap
	* If you have those mods their scrap will be utilized for some events

### Integrated mod events
Reworked event execution also allows for dynamic modded enemy events and other integrated events:

* **(NEW)** MeltdownEvent (only available with [FacilityMeltdown](https://thunderstore.io/c/lethal-company/p/loaforc/FacilityMeltdown/) installed)
* **(NEW)** BoombaEvent (only available with [LethalThings](https://thunderstore.io/c/lethal-company/p/Evaisa/LethalThings/) installed)
* **(NEW)** HerobrineEvent (only available with [Herobrine](https://thunderstore.io/c/lethal-company/p/Kittenji/Herobrine/) installed)
* **(NEW)** ShyGuyEvent (only available with [Scopophobia](https://thunderstore.io/c/lethal-company/p/jaspercreations/Scopophobia/) installed)
	* **IMPORTANT** You currently **must** use this mod ([ShyGuyPatcherPatcher](https://thunderstore.io/c/lethal-company/p/DBJ/ShyGuyPatcherPatcher/)) to prevent Scopophobia from overwriting the spawn logic. The event will be disabled when you don't have this installed.

It's easy to add more modded enemy events **unless** they use custom spawning logic like Scopophobia. Feel free to request more.

### More events
I probably forgot some..
* **(NEW)** HordeModeEvent: spawns more inside enemies, earlier.
* **(NEW)** ArmdayEvent: spawns a lot of heavy loot
* **(NEW)** ChristmasEveEvent: spawns a lot of gifts
* **(NEW)** ClownshowEvent: spawns a lot of scrap that can be used to make noise
* **(NEW)** DayDrinkingEvent: spawns a lot bottles, alcohol flask, canteen and similar items
* **(NEW)** LuckyDayEvent: increases chance for high value loot (cash register, gold bar, ..)
* **(NEW)** SelfDefenseEvent: spawns a lot of scrap wepons (stop, yield, toy hammer, )

### Enemy events
* made enemy events more impactful by not only increasing rarity but also (where applicable) increasing the enemy's max count, decreasing its power level and increasing the level's overall max power to compensate
	* e.g. during a bunker spider event you might encounter up to 4 spiders on the map

### Config
* restructured config file. Changed defaults. **I RECOMMEND YOU CHECK YOUR CONFIG AFTER 1ST START OR DELETE IT ENTIRELY BEFORE**
* made the range of credits rewarded for BountyEvent configurable
	* added a configurable limit to the amount of rewards until the bounty is considered *complete*
* made the range of credits rewarded by HullBreakEvent configurable
* new level settings selection: `vanilla`, `hullbreaker` or `custom`
	* level settings no longer change scrap amount and value or quota settings
* changed behvior of having `IncreaseEventCountPerDay` enabled to increase number of events on top of the amount set in `EventCount`

### Other event changes
* readded and fixed OneForAllEvent (auto vote ship leave after 1st death)
* readded MaskedEvent
* reworked OnAPowderKegEvent
	* After a random delay mines will explode one by one (with random delays inbetween) instead of all expldoding at the same time
	* this will properly scale with custom day lengths set by other mods
* fixed BeeEvent
* fixed a bug with EnemyBountyEvent where killing an enemy resulted in multiple rewards (for good now)
	* Despawned enemies will no longer trigger a reward
	* Bounty rewards now print more diverse and better chat messages
* fixed a bug with BabkinPogreb event where pickles would keep spawning the next rounds until the ship was routed and landed on a different moon

### Other changes
* fixed `IncreaseEventCountPerDay` (config where nr. of events increases each day) being incorrect when loading a save or getting fired resulting in more or less events than expected on a given day

# v1.1.1 (based on HullBreaker_Company_v1.3.11)
* fixed a bug with EnemyBountyEvent in MP lobbies where killing an enemy resulted in multiple rewards
    * fixed a typo in the EnemeyBountyEvent reward chat message


# v1.1.0 (based on HullBreaker_Company_v1.3.11)
* Re-enabled the turrent event that is currently disabled in original HullBreaker
	* spawning and despawning of turrets
	* works fine according to my testing 
		* turrets can spawn in weird positions but that's the vanilla game - spawning more just makes it more obvious
* OnAPowderKeg will spawn some additional mines (66% of LandmineScale in config) - so it can explode more mines :D
	* changed the random explosion interval to 10-300s
* HullBreakerLevelSettings config option will only affect enemies from now on
	* no changes to scrap value or amount
	* no changes to quota
* removed error message in console when no custom event folder is found
* fixed an error in ArachnophobiaEvent (checked spawnability of wrong enemy type)
* merged changes from upstream repo


# v1.0.1 (based on HullBreaker_Company_v1.4.0-alpha)
* fixed typo in README.md :')


# v1.0.0 (based on HullBreaker_Company_v1.4.0-alpha)
* initial fork release
* fork changes (check README for more information)
	* short and long event messages reveal less information about the event (more ambigous)
	* BountyEvent: random reward; print reward to chat
	* HullBreakEvent: random amount of credits; print amount to chat
	* NothingEvent: omit message from game chat; if all events are NothingEvent omit everything from game chat including _'NOTES ABOUT MOON'_