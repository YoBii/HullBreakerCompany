# v2.0.0 (based on HullBreaker_Company_v1.3.11)

## Core
* reworked the event execution logic
	* when an event can't be executed on the given moon another event will be randomly selected and executed
	* events can't execute when a moon doesn't naturally allow the enemy or scrap to spawn
		* e.g you can't get Jester event on (unaltered) Experimentation but you can get Girl and Nutcracker event
* reworked the chat messaging logic to be more flexible
* added a variety of event messages (long and short)
	* each event now has multiple possible 'Notes'. One of which is randomly selected on each execution. Some more, some less ambiguous.

* fancy enemy and loot logging (Host only)
	* this can be especially helpful when you're balancing custom moons or changing things with LQ or AC

### Compatibility
* fully compatible with LethalQuantities and AdvancedCompany
	* if you set custom enemy or scrap rarities for a moon using these mods it will allow relevant events
		* i.e. when you set Jester rarity to 1 on Experimentation you *can* get the Jester event where otherwise you could not

## Integrated mod events
Reworked event execution also allows for dynamic modded enemy events and other integrated events:

* **(NEW)** MeltdownEvent (only available with [FacilityMetldown](https://thunderstore.io/c/lethal-company/p/loaforc/FacilityMeltdown/) installed)
* **(NEW)** BoombaEvent (only available with [LethalThings](https://thunderstore.io/c/lethal-company/p/Evaisa/LethalThings/) installed)
* **(NEW)** HerobrineEvent (only available with [Herobrine](https://thunderstore.io/c/lethal-company/p/Kittenji/Herobrine/) installed)
* **(NEW)** ShyGuyEvent (only available with [Scopophobia](https://thunderstore.io/c/lethal-company/p/jaspercreations/Scopophobia/) installed)
	* **IMPORTANT** You currently **must** use this mod ([ShyGuyPatcherPatcher](https://thunderstore.io/c/lethal-company/p/DBJ/ShyGuyPatcherPatcher/)) to prevent Scopophobia from overwriting the spawn logic. The event will be disabled when you don't have this installed.

It's easy to add more modded enemy events **unless** they use custom spawning logic like Scopophobia. Feel free to request more.

## Config
* restructured config file. Changed defaults. **I RECOMMEND YOU CHECK YOUR CONFIG AFTER 1ST START OR DELETE IT ENTIRELY BEFORE**
	* made the range of credits rewarded for BountyEvent configurable
		* added a configurable limit to the amount of rewards until the bounty is considered *complete*
	* made the range of credits rewarded by HullBreakEvent configurable
* new level settings selection: `vanilla`, `hullbreaker` or `custom`
	* level settings no longer change scrap amount and value or quota settings
* changed behvior of having `IncreaseEventCountPerDay` enabled to increase number of events on top of the amount set in `EventCount`


## Other event changes
* readded and fixed OneForAllEvent (auto vote ship leave after 1st death)
* readded MaskedEvent
* reworked OnAPowderKegEvent
	* After a random delay mines will explode one by one (with random delays inbetween) instead of all expldoding at the same time
	* this will properly scale with custom day lengths set by other mods
* fixed BeeEvent
* fixed `IncreaseEventCountPerDay` (config where nr. of events increases each day) being incorrect when loading a save or getting fired resulting in more or less events than expected on a given day
* fixed a bug with EnemyBountyEvent where killing an enemy resulted in multiple rewards (for good now)
	* Despawned enemies will no longer trigger a reward
	* Bounty rewards now print more diverse and better chat messages
* fixed a bug with BabkinPogreb event where pickles would keep spawning the next rounds until the ship was routed and landed on a different moon


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