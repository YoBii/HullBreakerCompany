# HULLBREAKER COMPANY - Ambigous fork

All Credits go to Venterok! This is the original mod [**HullBreaker Company**](https://thunderstore.io/c/lethal-company/p/Venterok/HullBreaker_Company/) and [**HullBreaker Company on GitHub**](https://github.com/Venterok/HullBreakerCompany). Check it out!

Please **do not** report issues with this fork to Venterok. Report them to me on [GitHub](https://github.com/YoBii/HullBreakerCompany/issues)!

I forked this mod for personal use / tweaking to personal preference. Initially I just changed some strings and made a few small modificiations to the mod's logic.
Since then more significant changes have been made.

## Major changes

### Reworked event execution
Rewritten event execution logic allows to dynamically replace events that fail to execute by another random event.
Events fail to execute on certain moons where mobs or scrap can't naturally spawn.

#### Integrated mod events
Depending on whether you have other mods installed certain events are enabled automatically. No user interaction or configuration required.
Introducing events for Shy guy, Herobrine, FacilityMeltdown and more. Check the changelog.

### More impactful enemy events
You will definitely notice enemy events more.
No longer do events just bump up the rarity of let's say bunker spiders. They also increase their max count and decrease their power level accordingly.

### More events
Events, events, events. Can one have enough events?
* More scrap events like 'Jar of pickles'-event. Some make your working shift harder, some make it easier.
* HordeModeEvent increases spawn rate of inside enemies.
* **(PLANNED)** Time dilation events. Make your day shorter or longer.

### Compatibility
Fully compatible with LethalQuantities and AdvancedCompany overrides.
Using these mods you can allow events that would usually fail on certain moons by setting their rarity.
For example with vanilla moon configuration you can't get Jester event on Experimentation because Jesters don't spawn there.
Set Jester rarity to `1` or any larger number to allow the Jester event to occur.

### Ambiguous event messages
All event long and short messages in game chat are more ambigous i.e. less obvious.
Basically I tried to fit the whole "Notes about this moon"-thing without giving away the exact event.
For example all events that increase monster spawn rates have the same message. Telling you something about a large number of likely hostile life forms. So even if you know all the events you don't know whether it's spiders, lizards, slimes or bees you have to expect.

### Config file
Reorganized and restructured - allowing for more customization. Hopefully you'll find some settings more intuitive.
Mainly putting this here so you remember to check your config :)

## Smaller changes

### Events

#### EnemyBountyEvent
* The amount of credits rewarded for each kill is now random. Similar to an above average scrap item
* The amount rewarded will show in game chat
* There's a configurable limit to the amount of rewards
 
#### HullBreakEvent
* The event where you receive bonus credits now gives a random amount of credits (50-200)
* The amount will show in game chat

#### NothingEvent
* When the mod randomly selects _NothingEvent_ (no event) the event message is omitted from chat. With low `EventCount` such as `1` this makes it so events seem to be more random especially when combined with the option that increments the number of events every day. With high `EventCount` it simply prevents flooding the chat with empty lines from NothingEvent to a point where things become unreadable. This depends on your configuration (event count and event weights) of course
* When all events on a given day are NothingEvent the entire NOTES ABOUT MOON section is omitted from game chat

#### OnAPowderKegEvent
* Now explodes mines periodically. Respects custom day lengths.

#### Readded events
* TurretsEvent, OneForAllEvent, MaskedEvent

### Other
#### HullBreakerLevelSettings
* only change enemy count and spawn rate
* no more changes to scrap amount and value or quota

Check changelog for more