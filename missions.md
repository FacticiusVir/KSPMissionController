# Mission packages

Mission packages were introduced so that the mission directory would not
be filled with mission files. The user can not browse the mission directory in
a convenient way if there are more than 20 missions.

So I introduced mission packages. A mission package is nothing more than a
bundle of missions and some basic fields.

The missions have the same fields as before and they are handled in the same
way as before, but the user select the missions in a different way:

* First he selects the mission package
* A bigger mission package browser appears where he sees the mission package
    name, a description, and all available missions on the left side of the
    window. A mission belongs to exactly one category and the users sees the
    mission category in form of an icon. Further more he can see if all
    requirements are met to finish the mission. He can sort the missions by
    name, by reward, by category or by status (= "he can finish the mission"
    or "he can not finish the mission" because the requirement are not met,
    only the requiresMission field is being checked.)
* On the left side is the mission list in form of buttons. When he selects
    a mission, the default mission description and the mission goals
    appear on the right side of the window. The user can not finish
    a mission in this window!
* Once he has browsed through the available missions and found a mission he
    wants to finish, he presses the button "Select mission". The mission
    package browser disappears and the selected mission appears inside the
    smaller window. Only the currently selected mission can be finished!


And now the definition of a mission package (.mpkg file):

    MissionPackage
    {
        name = A *small* description of your mission package
        description = A long description of your missions, with newest changes etc.

        Mission
        {
            *normal mission definition, see below for details*
        }

        Mission
        {
            *normal mission definition, see below for details*
        }
    }


Do not worry about the order of your missions. The plugin will sort the missions by name by default,
but the user can also sort the missions by reward, and later on by category.

I will write a script that converts multiple mission files (old \*.m files) into one mission package
in order to make maintaining a mission package easier. It is easier to maintain several files instead of one,
in my opinion.

## Missions

A mission has several fields

* name: unique name for this mission. Used to identify the mission throughout the space program. Do not use the name of
    real life missions like "Apollo 11". Those names are reserved for designated mission designers.
* description: The description for this mission.
* reward: The reward in krones for finishing this mission
* requiresMission: name of another mission, that needs to be finished in order to finish this mission
    (default: "", ignored)
* repeatable: if true, the mission is repeatable. Requires a different vessel. You can't finish the
    same repeatable mission with the same vessel more than once.
* inOrder: if true, the mission goals have to be finished in the defined order (default: true)

A mission has one or multiple mission goals. After you have finished the mission, you get the reward.

Every vessel has its own mission: You can't finish the 1st mission goal with vessel `A`
and then continue to finish the 2nd mission goal with vessel `B`. Exception: EVAGoals, because an EVA is strictly speaking a vessel.

### Mission Goals

Currently there are six mission goals:

1. OrbitGoal to define an orbit
2. LandingGoal to define a landing site
3. ResourceGoal to define a required resource
4. PartGoal to define a required part
5. EVAGoal to define an EVA
6. SubMissionGoal to combine multiple goals into one goal.

### Common mission goal fields (available in *all* mission goals)

* description: A small description for this mission goal (field is optional), do not use it for huge texts
* reward: reward in krones for finishing this mission goal (default: 0)
* crewCount: amount of kerbals needed to finish the mission goals (default: 0)
* optional: makes the mission goal optional (default: false)
* throttleDown: if true, the vessel needs to throttle down in order to finish the mission goal (default: true)
* repeatable: if true, the mission goal is repeatable. Requires a different vessel. You can't finish the same repeatable mission
    with the same vessel more than once. (default: false) (do not use this field unless you have to and you know what you are doing)
* minSeconds: the minimal seconds the vessel needs to meet all requirements for this mission goal to be able to
    finish this mission goal. (default: -1, ignored) Use TIME() instruction to set high values (see below).

### OrbitGoal

Defines a certain orbit around a certain body as mission goal. All fields are optional.

Fields:

* body: orbit around body (default: Kerbin)
* minEccentricity: minimal eccentricity (default: 0, ignored)
* maxEccentricity: maximal eccentricity (default: 0, ignored)
* eccentricity: target eccentricity (do not combine it with min/maxEccentricity) (default: 0, ignored)
* eccentricityPrecision: defines the necessary precision for eccentricity. E.g. eccentricity = 0.001 and
    eccentricityPrecision is 0.1, that means that the vessels eccentricity needs to be smaller
    than 1.1 * eccentricity and bigger than 0.9 * eccentricity (default: 0.1)
* minPeA: minimal periapsis (default: 0, ignored)
* maxPeA: maximal periapsis (default: 0, ignored)
* minApA: minimal apoapsis (default: 0, ignored)
* maxApA: maximal apoapsis (default: 0, ignored)
* minLan: minimal longitude of ascending node (default: 0, ignored)
* maxLan: maximal longitude of ascending node (default: 0, ignored)
* minInclination: minimal inclination (default: 0, ignored)
* maxInclination: maximal inclination (default: 0, ignored)
* minOrbitalPeriod: minimal orbital period in seconds (use TIME instruction, default: 0, ignored)
* maxOrbitalPeriod: maximal orbital period in seconds (use TIME instruction, default: 0, ignored)
* minAltitude: minimal altitude for aviation (default: 0, ignored)
* maxAltitude: maximal altitude for aviation (default: 0, ignored)
* minSpeedOverGround: minimal horizontal speed (default: 0, ignored)
* maxSpeedOverGround: maximal horizontal speed (default: 0, ignored)

### LandingGoal

Defines a landing on a certain celestial body as mission goal.

Fields:

* body: celestial body on which the vessel needs to land (default: Kerbin)
* splashedValid: if true, then a "landing" in an ocean is fine (default: true)
* minLatitude: maximal latitude of landing spot (default: 0, ignored)
* maxLatitude: minimal latitude of landing spot (default: 0, ignored)
* minLongitude: minimal longitude of landing spot (default: 0, ignored)
* maxLongitude: maximal longitude of landing spot (default: 0, ignored)


### ResourceGoal

Defines a certain amount of a certain resource as mission goal.
Use it in combination with SubMissionGoal! See [SubMissionGoal](#submission) for more informations.

Fields:

* name: Resource name, (default: LiquidFuel)
* minAmount: minimal amount of defined resource (default: 0, ignored)
* maxAmount: maximal amount of defined resource (default: 0, ignored)


### PartGoal

Defines a mission goal that requires a certain amount of a certain part on your vessel.
Use it in combination with SubMissionGoal! See [SubMissionGoal](#submission) for more informations.

Fields:

* partName: name of the part
* partCount: minimal amount of the defined part (default: 1)
* maxPartCount: maximal amount of the defined part (default: -1, ignored)

### EVAGoal

Defines an EVA.

Fields:

* no extra fields

Usage:

    EVAGoal
    {
    }

Yes, those brackets *are necessary*!

### <a id="#submission"></a>SubMissionGoal

Contains multiple mission goals and combines them into one mission goal. Keep in mind that *all* mission goals
have to be finished at the same time. Use with caution.

Use this to define a PartGoal or ResourceGoal in combination with another goal. For example you want a satellite exploring Eve in a stable
orbit using scientic instruments. You don't want to explore Kerbin, you want to explore *Eve*. So instead of

    Mission
    {
        ...

        PartGoal
        {
            ...
        }

        OrbitGoal
        {
            ...
        }
    }

you should use

    Mission
    {
        SubMissionGoal
        {
            description = small description

            PartGoal
            {
                ...
            }

            OrbitGoal
            {
                ...
            }
        }
    }

The first mission is wrong, because I could build a vessel with scientic instruments on the lifting stage.
The scientific instruments won't make their way to Eve, because they are on the lifting stage, but I can and I will be able to finish the
first mission (not the second mission). Strictly speaking it was never required to have the scientific instruments *while* orbiting Eve
in the first mission.


Those fields are ignored in all mission goals inside the SubMissionGoal (you can use them on the SubMissionGoal though):

* repeatable
* reward
* optional


Do not laugh about the name... I will change it in the future. SubMission like in suborbital...

Fields:

* no extra fields


### Example mission

Here is the example `Mun X.m` mission file:

    Mission
    {
        name = Mun X
        description = Bring a manned space craft onto the surface of the Mun and bring him back.
        reward = 100000

        OrbitGoal
        {
            crewCount = 1
            reward = 10000
            body = Kerbin
            minPeA = 70000
            maxEccentricity = 1
        }

        OrbitGoal
        {
            crewCount = 1
            reward = 15000
            body = Mun
            minPeA = 3000
            maxEccentriciy = 1
        }

        LandingGoal
        {
            crewCount = 1
            body = Mun
        }

        OrbitGoal
        {
            reward = 15000
            crewCount = 1
            body = Mun
            minPeA = 3000
            maxEccentriciy = 1
        }

        LandingGoal
        {
            crewCount = 1
            body = Kerbin
        }
    }

## Mission file instructions

There are currently three instructions for double fields:

* RANDOM(MINMAL, MAXIMAL) generates a random double value
* ADD(fieldName, VALUE) calculates fieldName + VALUE and assignes it to the field
* TIME(aay bbd cch eem ffs) converts the time into seconds. All fields are optional. y = years, d = days, and so on.

Keep in mind that those are *instructions*. You can't combine them like `RANDOM(ADD(fieldValue, 5) ...`. This requires a
parser and it is not worth it.


## Random fields

Say you want to create a randomized mission, e.g. an orbiting mission around Kerbin. You can use the instructions `RANDOM` and `ADD`
to define your mission. Don't forget to add the `randomized = true` field, so that the users can discard the random mission and generate
another one. The mission will be generated everytime in another way if you don't set the `randomized` field.

Here is an example randomized mission:

    Mission
    {
        name = Randomized Example
        description = Bring a satellite into the defined orbit
        reward = 80000
        randomized = true

        OrbitGoal
        {
            body = Kerbin
            minApA = RANDOM(100000, 200000)
            maxApA = ADD(minApA, 20000)

            maxEccentricity = 0.01
        }
    }

The required orbit has a maximal eccentricity of 0.01 and its minimal apoapsis is somewhere between 100000m and 200000m,
and the maximal apoapsis is 20000m higher than the minimal apoapsis.


# File format

The fileformat for mission packages is restrictive:

1. One line per field
2. no quotation marks like in C or Java
3. use `{` and `}` seperately, in one line
4. case sensitive

