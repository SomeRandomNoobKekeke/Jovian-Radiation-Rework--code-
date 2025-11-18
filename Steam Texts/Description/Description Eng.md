C# mod that completely changes jovian radiation

[h2] List of changes on Default settings: [/h2]
[list]
    [*] Jovian radiation is now damaging you with "Radiation sickness" affliction, once again
    [*] Radiation is now blue
    [*] Added ambient glow on levels covered by radiation, brightness is proportional to the radiation background
    [*] Radiation is moving faster and continuously and not in step of 10 min, but still only between levels
    [*] Radiation damage is growing indefinitely with radiation background
    [*] Monsted don't spawn dead and are not harmed by radiation
    [*] Radiation slightly increases monster speed, but not hp
    [*] Your submarine hull gives 50% radiation protection, outposts and beacons are 100% protected, caves and wreck gives 75% protection 
    [*] Gaps passes radiation
    [*] Outpost don't evacuate for a very long time
    [*] Added radiation protection hull upgrades that can increase your sub protection from radiaiton up to 100%
    [*] Radiation is now damaging electronics, but upgrades reduces the damage
    [*] Radiation is blocked by water and is getting weaker with depth
    [*] Husks now have 50% radiation resistance
    [*] Added jovian radiation counter that can measure jovian radiation
    [*] Radiation now moves on outposts but 4 times slower
[/list]

[h2] Common changes: [/h2]
[list]
    [*] Hazmat Suit is more expensive, available only from 2nd biome and gives 20% radiation protection instead of 60%
    [*] Dormant radiation sickness period reduced from 25 to 5 and now have the same healing speed as active period
[/list]

[h2] Max tolerable radiation levels: [/h2]
[list]
    [*] For unprotected human -> 0.2
    [*] In hazmat suit -> 0.25
    [*] In diving suit -> 0.27
    [*] In diving suit + hazmat suit -> 0.33
    [*] In PUCS -> 0.8
    [*] In PUCS + hazmat suit -> 1.0
[/list]

Also somewhere around 0.2 inside sub full time engineer begins to struggle with maintaining dugong
So hire more engineers or buy hull rad upgrades

[h3] I'll add descriptions of all settings when I'm not lazy [/h3]

[url=https://github.com/SomeRandomNoobKekeke/Jovian-Radiation-Rework--code-] Source code [/url]