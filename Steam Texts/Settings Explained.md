[h2] Into [/h2]

Mod consists of several models that change different aspects of radiation behaviour
Models have settings, Settings are stored in configs

Current config is stored in "Barotrauma\ModSettings\Jovian Radiation Rework\Settings.xml"
Pedefined configs are stored in "ModFolder\Presets"

You can edit current config with "rad" command, save and load it with rad_save and rad_load commands

Radiation amount is pixels on the global map from rad front

[h1] MainConfig [/h1]
[h2] EnabledModels [/h2]
These are all available models, they can be turned on and off independently, when you turn one off its aspect implementations are replaced with implementations from model with lower priprity, vanilla model has the lowest priority and can't be turned off

[h2] Vanilla [/h2]
This is a proxy to vanilla RadiationParams
Those values are copied to vanilla params on round start

if OverrideVanillaSettings is false then they won't
Modded models don't depend on vanilla params, but if you want to play with vanilla radiaton it could be useful

[h2] Visuals [/h2]
[h3] AmbientLight [/h3]
This model changes ambient light on a level as a function of amount of radiation on a camera

brightness = (Radiation Amount + RadiationAmountOffset) * RadiationToAmbienceBrightness

AmbienceColor = ActualColor * brightness

Noise is multiplicator for brightness, NoiseAmplitude: [0, AmbienceNoiseAmplitude]

[table]
    [tr]
        [td]RadiationToAmbienceBrightness[/td]
        [td]brightness = Radiation Amount * RadiationToAmbienceBrightness [/td]
    [/tr]
    [tr]
        [td]MaxAmbienceBrightness[/td]
        [td]upper bound for brightness[/td]
    [/tr]
    [tr]
        [td]AmbienceNoiseAmplitude[/td]
        [td]upper bound for noise amplitude[/td]
    [/tr]
    [tr]
        [td]ActualColor[/td]
        [td]color of the ambient light[/td]
    [/tr]
    [tr]
        [td]PerlinNoiseFrequency[/td]
        [td]Noise frequency[/td]
    [/tr]
    [tr]
        [td]RadiationAmountOffset[/td]
        [td]Const offset to radiation amount, useful to make ambient light appear a little before the actual radiation front[/td]
    [/tr]
[/table]

[h3] MapRender [/h3]
This thing draws radiation on global map
it's 100% vanilla, it's made just to untangle color settings from vanilla RadiationParams settings

[table]
    [tr]
        [td]AreaColor[/td]
        [td]color of area covered by radiation [/td]
    [/tr]
    [tr]
        [td]BorderColor[/td]
        [td]Color of that waves on radiaiton border[/td]
    [/tr]
[/table]

[h2] RadiationEffects [/h2]
[h3] MoreMonsters [/h3]
This thing increases amount of monsters spawned in radiation

MonstersMult - multiplicator for monster count in each event

[table]
    [tr]
        [td]TooMuchEvenForMonsters[/td]
        [td]After this radiation amount monsters won't spawn, -1 to disable[/td]
    [/tr]
    [tr]
        [td]RadiationToMonstersMult[/td]
        [td]MonstersMult = radiation amount * RadiationToMonstersMult[/td]
    [/tr]
    [tr]
        [td]MaxRadiationToMonstersMult[/td]
        [td]upper bound for MonstersMult[/td]
    [/tr]
[/table]

[h3] DamageToElectronics [/h3]
This thing makes radiation damage electronics

dps = Radiation amount * RadAmountToDPS
damage =  dps * DamageInterval 

condition = isFromMainSub && isRepairable && (isPowerTransfer || isPowerContainer || isSteering || isSonar)

[table]
    [tr]
        [td]DamageInterval[/td]
        [td]yes[/td]
    [/tr]
    [tr]
        [td]RadAmountToDPS[/td]
        [td]dps = Radiation amount * RadAmountToDPS[/td]
    [/tr]
    [tr]
        [td]MaxDPS[/td]
        [td]upper bound for dps[/td]
    [/tr]
[/table]

[h3] DamageToCharacters [/h3]
This thing damages humans 

dps = Radiation amount * RadAmountToDPS - ExtraHumanHealing

damage = dps * RadiationUpdate.CharacterDamageInterval

[table]
    [tr]
        [td]RadAmountToDPS[/td]
        [td]dps = Radiation amount * RadAmountToDPS - ExtraHumanHealing[/td]
    [/tr]
    [tr]
        [td]ExtraHumanHealing[/td]
        [td]This can be used to simulate human healing for compatibility with mods where humans can't heal radiation naturally Humans will take 0 damage if dps is lower than this[/td]
    [/tr]
    [tr]
        [td]Affliction[/td]
        [td]affliction id, applied only to main limb for now[/td]
    [/tr]
[/table]

[h3] DamageToMonsters [/h3]
Instead of giving monsters jovian radiation it applies jovesrage affliction, which is a buff that increases movement speed by 1.2

[h2] RadiationProgress [/h2]
[h3] LocationsTransform [/h3]
this thing makes locations abandoned at some level of radiation instead of after n turns in radiation  

[table]
    [tr]
        [td]KeepSurroundingOutpostsAlive[/td]
        [td]if true adjacent locations won't become abandoned, this is vanilla feature actually[/td]
    [/tr]
    [tr]
        [td]CriticalOutpostRadiationAmount[/td]
        [td]At this radiation level locations become abandoned[/td]
    [/tr]
    [tr]
        [td]MinimumOutpostAmount[/td]
        [td]This amount of outpost will be kept alive on a map no matter what[/td]
    [/tr]
[/table]

[h3] RadiationMoving [/h3]
this thing is moving the radiation between rounds

[table]
    [tr]
        [td]TargetSpeedPercentageAtTheEndOfTheMap[/td]
        [td]Radiation will slowdown gradually during the cource of the campaign to compensate for longer levels later on, down to this fraction, not percentage actually kek[/td]
    [/tr]
    [tr]
        [td]StartingRadiation[/td]
        [td]Radiation will start from this amount at the start of the campaign, if set on first round will be applied instantly[/td]
    [/tr]
    [tr]
        [td]RadiationSpeed[/td]
        [td]Amount increase per minute of gameplay[/td]
    [/tr]
    [tr]
        [td]MaxTimeOnRound[/td]
        [td]after this many minutes on a level radiation will stop moving[/td]
    [/tr]
    [tr]
        [td]MinTimeOnRound[/td]
        [td]Grace period, if you leave / start new round before this many minutes radiation won't move
    Without grace period you'll have to leave instantly when quiting game, you will be punished for redocking and taking other mission
    But with Grace period too high you might be tempted to saveload abuse and farm screewdrivers when you get deadlocked[/td]
    [/tr]
[/table]

[h3] SaveAndQuitAction [/h3]
This thing makes radiation move even on save and quit, to prevent saveload abuse

[h2] RadiationProtection [/h2]
[h3] WaterRadiationBlocking [/h3]
This thing makes radiation on a level depend on depth

[table]
    [tr]
        [td]WaterRadiationBlockPerMeter[/td]
        [td]Amount of radiation blocked by meter of water
    So if this = 1, then 100 meters below radiation amount will be 100 less[/td]
    [/tr]
[/table]

[h3] HullRadiationProtection [/h3]

[table]
    [tr]
        [td]MainSub[/td]
        [td]
        Main sub protection
        This also affects DamageToElectronics
        [/td]   
    [/tr]
    [tr]
        [td]Beacon[/td]
        [td]Protection inside beacons[/td]   
    [/tr]
    [tr]
        [td]Outpost[/td]
        [td]Protection inside outposts[/td]   
    [/tr]
    [tr]
        [td]EnemySub[/td]
        [td]Protection inside enemy sub[/td]   
    [/tr]
    [tr]
        [td]Wreck[/td]
        [td]Protection inside wrecks[/td]   
    [/tr]
    [tr]
        [td]Ruins[/td]
        [td]Protection inside ruins[/td]   
    [/tr]
    [tr]
        [td]Cave[/td]
        [td]
        Protection inside caves
        Note that check for whether you are in a cave is cheap and not 100% precise
        [/td]   
    [/tr]
[/table]

[h3] HuskRadiationProtection [/h3]
This gives husks some radiation protection

[table]
    [tr]
        [td]HuskRadiationResistance[/td]
        [td]Protection[/td]
    [/tr]
    [tr]
        [td]HuskInfectionThreshold[/td]
        [td]Protection kicks in after this level of husk infection[/td]
    [/tr]
[/table]

[h3] HullRadProtectionUpgrades [/h3]
This model is required for rad protection hull upgrades to work, xml is purely decorative and doesn't do anything on its own

[table]
    [tr]
        [td]HullUpgradeProtection[/td]
        [td]Added protection per upgrade[/td]
    [/tr]
[/table]

[h3] RadiationUpdate [/h3]
Periodically applies radiation effects
it updates electronics damager, damages humans and buffs monsters

[table]
    [tr]
        [td]CharacterDamageInterval[/td]
        [td]
        interval at which monsters and humans are damaged
        This is also used in DamageToCharacters to calculate damage from dps
        [/td]
    [/tr]
[/table]