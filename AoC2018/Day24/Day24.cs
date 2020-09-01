using System;

/*

--- Day 24: Immune System Simulator 20XX ---

After a weird buzzing noise, you appear back at the man's cottage.
He seems relieved to see his friend, but quickly notices that the little reindeer caught some kind of cold while out exploring.

The portly man explains that this reindeer's immune system isn't similar to regular reindeer immune systems:

The immune system and the infection each have an army made up of several groups; each group consists of one or more identical units.
The armies repeatedly fight until only one army has units remaining.

Units within a group all have the same hit points (amount of damage a unit can take before it is destroyed), attack damage (the amount of damage each unit deals), an attack type, an initiative (higher initiative units attack first and win ties), and sometimes weaknesses or immunities.
Here is an example group:

18 units each with 729 hit points (weak to fire; immune to cold, slashing) with an attack that does 8 radiation damage at initiative 10

Each group also has an effective power: the number of units in that group multiplied by their attack damage.
The above group has an effective power of 18 * 8 = 144.
Groups never have zero or negative units; instead, the group is removed from combat.

Each fight consists of two phases: target selection and attacking.

During the target selection phase, each group attempts to choose one target.
In decreasing order of effective power, groups choose their targets; in a tie, the group with the higher initiative chooses first.
The attacking group chooses to target the group in the enemy army to which it would deal the most damage (after accounting for weaknesses and immunities, but not accounting for whether the defending group has enough units to actually receive all of that damage).

If an attacking group is considering two defending groups to which it would deal equal damage, it chooses to target the defending group with the largest effective power; if there is still a tie, it chooses the defending group with the highest initiative.
If it cannot deal any defending groups damage, it does not choose a target.
Defending groups can only be chosen as a target by one attacking group.

At the end of the target selection phase, each group has selected zero or one groups to attack, and each group is being attacked by zero or one groups.

During the attacking phase, each group deals damage to the target it selected, if any.
Groups attack in decreasing order of initiative, regardless of whether they are part of the infection or the immune system.
(If a group contains no units, it cannot attack.)

The damage an attacking group deals to a defending group depends on the attacking group's attack type and the defending group's immunities and weaknesses.
By default, an attacking group would deal damage equal to its effective power to the defending group.
However, if the defending group is immune to the attacking group's attack type, the defending group instead takes no damage; if the defending group is weak to the attacking group's attack type, the defending group instead takes double damage.

The defending group only loses whole units from damage; damage is always dealt in such a way that it kills the most units possible, and any remaining damage to a unit that does not immediately kill it is ignored.
For example, if a defending group contains 10 units with 10 hit points each and receives 75 damage, it loses exactly 7 units and is left with 3 units at full health.

After the fight is over, if both armies still contain units, a new fight begins; combat only ends once one army has lost all of its units.

For example, consider the following armies:

Immune System:
17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2
989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3

Infection:
801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1
4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4

If these armies were to enter combat, the following fights, including details during the target selection and attacking phases, would take place:

Immune System:
Group 1 contains 17 units
Group 2 contains 989 units
Infection:
Group 1 contains 801 units
Group 2 contains 4485 units

Infection group 1 would deal defending group 1 185832 damage
Infection group 1 would deal defending group 2 185832 damage
Infection group 2 would deal defending group 2 107640 damage
Immune System group 1 would deal defending group 1 76619 damage
Immune System group 1 would deal defending group 2 153238 damage
Immune System group 2 would deal defending group 1 24725 damage

Infection group 2 attacks defending group 2, killing 84 units
Immune System group 2 attacks defending group 1, killing 4 units
Immune System group 1 attacks defending group 2, killing 51 units
Infection group 1 attacks defending group 1, killing 17 units
Immune System:
Group 2 contains 905 units
Infection:
Group 1 contains 797 units
Group 2 contains 4434 units

Infection group 1 would deal defending group 2 184904 damage
Immune System group 2 would deal defending group 1 22625 damage
Immune System group 2 would deal defending group 2 22625 damage

Immune System group 2 attacks defending group 1, killing 4 units
Infection group 1 attacks defending group 2, killing 144 units
Immune System:
Group 2 contains 761 units
Infection:
Group 1 contains 793 units
Group 2 contains 4434 units

Infection group 1 would deal defending group 2 183976 damage
Immune System group 2 would deal defending group 1 19025 damage
Immune System group 2 would deal defending group 2 19025 damage

Immune System group 2 attacks defending group 1, killing 4 units
Infection group 1 attacks defending group 2, killing 143 units
Immune System:
Group 2 contains 618 units
Infection:
Group 1 contains 789 units
Group 2 contains 4434 units

Infection group 1 would deal defending group 2 183048 damage
Immune System group 2 would deal defending group 1 15450 damage
Immune System group 2 would deal defending group 2 15450 damage

Immune System group 2 attacks defending group 1, killing 3 units
Infection group 1 attacks defending group 2, killing 143 units
Immune System:
Group 2 contains 475 units
Infection:
Group 1 contains 786 units
Group 2 contains 4434 units

Infection group 1 would deal defending group 2 182352 damage
Immune System group 2 would deal defending group 1 11875 damage
Immune System group 2 would deal defending group 2 11875 damage

Immune System group 2 attacks defending group 1, killing 2 units
Infection group 1 attacks defending group 2, killing 142 units
Immune System:
Group 2 contains 333 units
Infection:
Group 1 contains 784 units
Group 2 contains 4434 units

Infection group 1 would deal defending group 2 181888 damage
Immune System group 2 would deal defending group 1 8325 damage
Immune System group 2 would deal defending group 2 8325 damage

Immune System group 2 attacks defending group 1, killing 1 unit
Infection group 1 attacks defending group 2, killing 142 units
Immune System:
Group 2 contains 191 units
Infection:
Group 1 contains 783 units
Group 2 contains 4434 units

Infection group 1 would deal defending group 2 181656 damage
Immune System group 2 would deal defending group 1 4775 damage
Immune System group 2 would deal defending group 2 4775 damage

Immune System group 2 attacks defending group 1, killing 1 unit
Infection group 1 attacks defending group 2, killing 142 units
Immune System:
Group 2 contains 49 units
Infection:
Group 1 contains 782 units
Group 2 contains 4434 units

Infection group 1 would deal defending group 2 181424 damage
Immune System group 2 would deal defending group 1 1225 damage
Immune System group 2 would deal defending group 2 1225 damage

Immune System group 2 attacks defending group 1, killing 0 units
Infection group 1 attacks defending group 2, killing 49 units
Immune System:
No groups remain.
Infection:
Group 1 contains 782 units
Group 2 contains 4434 units
In the example above, the winning army ends up with 782 + 4434 = 5216 units.

You scan the reindeer's condition (your puzzle input); the white-bearded man looks nervous.

As it stands now, how many units would the winning army have?

Your puzzle answer was 28976.

--- Part Two ---

Things aren't looking good for the reindeer.
The man asks whether more milk and cookies would help you think.

If only you could give the reindeer's immune system a boost, you might be able to change the outcome of the combat.

A boost is an integer increase in immune system units' attack damage.
For example, if you were to boost the above example's immune system's units by 1570, the armies would instead look like this:

Immune System:
17 units each with 5390 hit points (weak to radiation, bludgeoning) with
 an attack that does 6077 fire damage at initiative 2
989 units each with 1274 hit points (immune to fire; weak to bludgeoning,
 slashing) with an attack that does 1595 slashing damage at initiative 3

Infection:
801 units each with 4706 hit points (weak to radiation) with an attack
 that does 116 bludgeoning damage at initiative 1
4485 units each with 2961 hit points (immune to radiation; weak to fire,
 cold) with an attack that does 12 slashing damage at initiative 4
With this boost, the combat proceeds differently:

Immune System:
Group 2 contains 989 units
Group 1 contains 17 units
Infection:
Group 1 contains 801 units
Group 2 contains 4485 units

Infection group 1 would deal defending group 2 185832 damage
Infection group 1 would deal defending group 1 185832 damage
Infection group 2 would deal defending group 1 53820 damage
Immune System group 2 would deal defending group 1 1577455 damage
Immune System group 2 would deal defending group 2 1577455 damage
Immune System group 1 would deal defending group 2 206618 damage

Infection group 2 attacks defending group 1, killing 9 units
Immune System group 2 attacks defending group 1, killing 335 units
Immune System group 1 attacks defending group 2, killing 32 units
Infection group 1 attacks defending group 2, killing 84 units
Immune System:
Group 2 contains 905 units
Group 1 contains 8 units
Infection:
Group 1 contains 466 units
Group 2 contains 4453 units

Infection group 1 would deal defending group 2 108112 damage
Infection group 1 would deal defending group 1 108112 damage
Infection group 2 would deal defending group 1 53436 damage
Immune System group 2 would deal defending group 1 1443475 damage
Immune System group 2 would deal defending group 2 1443475 damage
Immune System group 1 would deal defending group 2 97232 damage

Infection group 2 attacks defending group 1, killing 8 units
Immune System group 2 attacks defending group 1, killing 306 units
Infection group 1 attacks defending group 2, killing 29 units
Immune System:
Group 2 contains 876 units
Infection:
Group 2 contains 4453 units
Group 1 contains 160 units

Infection group 2 would deal defending group 2 106872 damage
Immune System group 2 would deal defending group 2 1397220 damage
Immune System group 2 would deal defending group 1 1397220 damage

Infection group 2 attacks defending group 2, killing 83 units
Immune System group 2 attacks defending group 2, killing 427 units
After a few fights...

Immune System:
Group 2 contains 64 units
Infection:
Group 2 contains 214 units
Group 1 contains 19 units

Infection group 2 would deal defending group 2 5136 damage
Immune System group 2 would deal defending group 2 102080 damage
Immune System group 2 would deal defending group 1 102080 damage

Infection group 2 attacks defending group 2, killing 4 units
Immune System group 2 attacks defending group 2, killing 32 units
Immune System:
Group 2 contains 60 units
Infection:
Group 1 contains 19 units
Group 2 contains 182 units

Infection group 1 would deal defending group 2 4408 damage
Immune System group 2 would deal defending group 1 95700 damage
Immune System group 2 would deal defending group 2 95700 damage

Immune System group 2 attacks defending group 1, killing 19 units
Immune System:
Group 2 contains 60 units
Infection:
Group 2 contains 182 units

Infection group 2 would deal defending group 2 4368 damage
Immune System group 2 would deal defending group 2 95700 damage

Infection group 2 attacks defending group 2, killing 3 units
Immune System group 2 attacks defending group 2, killing 30 units
After a few more fights...

Immune System:
Group 2 contains 51 units
Infection:
Group 2 contains 40 units

Infection group 2 would deal defending group 2 960 damage
Immune System group 2 would deal defending group 2 81345 damage

Infection group 2 attacks defending group 2, killing 0 units
Immune System group 2 attacks defending group 2, killing 27 units
Immune System:
Group 2 contains 51 units
Infection:
Group 2 contains 13 units

Infection group 2 would deal defending group 2 312 damage
Immune System group 2 would deal defending group 2 81345 damage

Infection group 2 attacks defending group 2, killing 0 units
Immune System group 2 attacks defending group 2, killing 13 units
Immune System:
Group 2 contains 51 units
Infection:
No groups remain.

This boost would allow the immune system's armies to win! It would be left with 51 units.

You don't even know how you could boost the reindeer's immune system or what effect it might have, so you need to be cautious and find the smallest boost that would allow the immune system to win.

How many units does the immune system have left after getting the smallest boost it needs to win?

*/

namespace Day24
{
    class Program
    {
        const int MAX_COUNT_GROUPS = 32;
        const int MAX_IMMUNE_WEAK_COUNT = 4;
        readonly private static int[] sSide = new int[MAX_COUNT_GROUPS];
        readonly private static int[] sHP = new int[MAX_COUNT_GROUPS];
        readonly private static int[] sUnitCount = new int[MAX_COUNT_GROUPS];
        readonly private static string[,] sWeakness = new string[MAX_COUNT_GROUPS, MAX_IMMUNE_WEAK_COUNT];
        readonly private static int[] sWeaknessCount = new int[MAX_COUNT_GROUPS];
        readonly private static string[,] sImmunity = new string[MAX_COUNT_GROUPS, MAX_IMMUNE_WEAK_COUNT];
        readonly private static int[] sImmunityCount = new int[MAX_COUNT_GROUPS];
        readonly private static int[,] sAttackMultiplier = new int[MAX_COUNT_GROUPS, MAX_COUNT_GROUPS];
        readonly private static int[] sAttackPower = new int[MAX_COUNT_GROUPS];
        readonly private static string[] sAttackType = new string[MAX_COUNT_GROUPS];
        readonly private static int[] sInitiative = new int[MAX_COUNT_GROUPS];
        readonly private static int[] sInitiativeOrder = new int[MAX_COUNT_GROUPS];
        private static int sGroupCount;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = WinningArmyUnits();
                Console.WriteLine($"Day24 : Result1 {result1}");
                // 28781 : TOO LOW
                // 29150 : TOO HIGH
                var expected = 28976;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = -123;
                Console.WriteLine($"Day24 : Result2 {result2}");
                var expected = 1797;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            sGroupCount = 0;
            // Immune System:
            // 17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2 989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3
            // 
            // Infection:
            // 801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1
            // 4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4
            var expectImmuneSystem = true;
            var expectInfection = false;
            int side = -1;
            foreach (var line in lines)
            {
                if (line == "Immune System:")
                {
                    if (!expectImmuneSystem || (side != -1))
                    {
                        throw new InvalidProgramException($"Bad line '{line}' was not expecting 'Immune System:'");
                    }
                    expectImmuneSystem = false;
                    side = 0;
                }
                else if (line == "Infection:")
                {
                    if (!expectInfection || (side != -1))
                    {
                        throw new InvalidProgramException($"Bad line '{line}' was not expecting 'Infection:'");
                    }
                    expectInfection = false;
                    side = 1;
                }
                else if (line == "")
                {
                    expectInfection = true;
                    if (side != 0)
                    {
                        throw new InvalidProgramException($"Bad line '{line}' was not expecting a blank line");
                    }
                    side = -1;
                }
                else if (expectImmuneSystem || expectInfection || (side == -1))
                {
                    throw new InvalidProgramException($"Bad line '{line}' 'Immune System:' or 'Infection:' or ''");
                }
                else
                {
                    // 4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4
                    var tokens = line.Trim().Split();
                    if ((tokens[1] != "units") || (tokens[2] != "each") || (tokens[3] != "with") || (tokens[5] != "hit") || (tokens[6] != "points"))
                    {
                        throw new InvalidProgramException($"Bad line '{line}' Expected 'XXX units each with YYY hit points'");
                    }
                    var unitCount = int.Parse(tokens[0]);
                    var hp = int.Parse(tokens[4]);
                    var optionalTokens = line.Split(')');
                    var attackToken = line.Substring(line.IndexOf("with an"));
                    var weaknessCount = 0;
                    var immunityCount = 0;
                    if (optionalTokens.Length == 2)
                    {
                        var subTokens = optionalTokens[0].Trim().Split('(');
                        if (subTokens.Length != 2)
                        {
                            throw new InvalidProgramException($"Bad line '{line}' Expected 2 '(' tokens got {subTokens.Length}");
                        }
                        // immune to AAA; weak to BBB, CCC
                        // weak to AAA, BBB; immune to CCC, DDD
                        var immuneWeakTokens = subTokens[1].Trim().Split(';');

                        foreach (var immuneWeakToken in immuneWeakTokens)
                        {
                            var words = immuneWeakToken.Trim().Split();
                            if (words.Length < 3)
                            {
                                throw new InvalidProgramException($"Bad line '{line}' Expected at least 3 tokens got {words.Length}");
                            }
                            if ((words[0] != "immune") && (words[0] != "weak"))
                            {
                                throw new InvalidProgramException($"Bad line '{line}' Expected 'immune' or 'weak' got {words[0]}");
                            }
                            if (words[1] != "to")
                            {
                                throw new InvalidProgramException($"Bad line '{line}' Expected 'to' got {words[1]}");
                            }
                            var immunity = false;
                            if (words[0] == "immune")
                            {
                                immunity = true;
                            }
                            var powers = words[2..];
                            var count = powers.Length;
                            for (var p = 0; p < count; ++p)
                            {
                                var power = powers[p].TrimEnd(',');
                                if (immunity)
                                {
                                    sImmunity[sGroupCount, p] = power;
                                }
                                else
                                {
                                    sWeakness[sGroupCount, p] = power;
                                }
                            }
                            if (immunity)
                            {
                                immunityCount = count;
                            }
                            else
                            {
                                weaknessCount = count;
                            }
                        }

                        attackToken = optionalTokens[1];
                    }

                    var attackTokens = attackToken.Trim().Split();

                    // with an attack that does XXX YYY damage at initiative ZZZ
                    if ((attackTokens[0] != "with") || (attackTokens[1] != "an") || (attackTokens[2] != "attack") || (attackTokens[3] != "that") ||
                        (attackTokens[4] != "does") || (attackTokens[7] != "damage") || (attackTokens[8] != "at") || (attackTokens[9] != "initiative"))
                    {
                        throw new InvalidProgramException($"Bad line '{line}' Got '{tokens[1]}' Expected 'with an attack that does XXX YYY damage at initiative ZZZ'");
                    }
                    var attackPower = int.Parse(attackTokens[5]);
                    var attackType = attackTokens[6].Trim();
                    var initiative = int.Parse(attackTokens[10]);

                    sSide[sGroupCount] = side;
                    sUnitCount[sGroupCount] = unitCount;
                    sHP[sGroupCount] = hp;
                    sWeaknessCount[sGroupCount] = weaknessCount;
                    sImmunityCount[sGroupCount] = immunityCount;
                    sAttackPower[sGroupCount] = attackPower;
                    sAttackType[sGroupCount] = attackType;
                    sInitiative[sGroupCount] = initiative;
                    ++sGroupCount;
                }
            }
            ComputeAttackMultiplier();
            ComputeInitiativeOrder();
            OutputGroups();
        }

        private static void ComputeInitiativeOrder()
        {
            var groupCount = sGroupCount;
            for (var i = 0; i < groupCount; ++i)
            {
                sInitiativeOrder[i] = i;
            }

            for (var i = 0; i < sGroupCount - 1; ++i)
            {
                for (var j = i + 1; j < sGroupCount; ++j)
                {
                    var i1 = sInitiativeOrder[i];
                    var i2 = sInitiativeOrder[j];
                    var initiative1 = sInitiative[i1];
                    var initiative2 = sInitiative[i2];
                    if (initiative2 > initiative1)
                    {
                        sInitiativeOrder[j] = i1;
                        sInitiativeOrder[i] = i2;
                    }
                }
            }
        }

        private static void ComputeAttackMultiplier()
        {
            for (var a = 0; a < sGroupCount; ++a)
            {
                var attackType = sAttackType[a];
                for (var d = 0; d < sGroupCount; ++d)
                {
                    var attackMultiplier = 1;
                    for (var w = 0; w < sWeaknessCount[d]; ++w)
                    {
                        if (sWeakness[d, w] == attackType)
                        {
                            attackMultiplier = 2;
                            break;
                        }
                    }
                    for (var i = 0; i < sImmunityCount[d]; ++i)
                    {
                        if (sImmunity[d, i] == attackType)
                        {
                            attackMultiplier = 0;
                            break;
                        }
                    }
                    sAttackMultiplier[a, d] = attackMultiplier;
                }
            }
        }

        private static void OutputGroups()
        {
            for (var g = 0; g < sGroupCount; ++g)
            {
                Console.Write($"Group[{g}] Side:{sSide[g]} Units:{sUnitCount[g]} HP:{sHP[g]} ");
                if (sWeaknessCount[g] > 0)
                {
                    Console.Write($"Weakness: ");
                    for (var w = 0; w < sWeaknessCount[g]; ++w)
                    {
                        Console.Write($"'{sWeakness[g, w]}' ");
                    }
                }
                if (sImmunityCount[g] > 0)
                {
                    Console.Write($"Immunity: ");
                    for (var i = 0; i < sImmunityCount[g]; ++i)
                    {
                        Console.Write($"'{sImmunity[g, i]}' ");
                    }
                }
                Console.Write($"Attack:{sAttackPower[g]} '{sAttackType[g]}' ");
                Console.Write($"Initiative:{sInitiative[g]}");
                Console.WriteLine($"");
            }
        }


        private static int ComputeDamage(int attacker, int defender)
        {
            if (defender < 0)
            {
                return 0;
            }
            var attackPower = sAttackPower[attacker] * sUnitCount[attacker];
            var attackMultiplier = sAttackMultiplier[attacker, defender];
            attackPower *= attackMultiplier;
            return attackPower;
        }

        public static int WinningArmyUnits()
        {
            var groupCount = sGroupCount;
            var totalPerSide = new int[2];
            do
            {
                Console.WriteLine($"Fight");
                Fight();
                totalPerSide[0] = 0;
                totalPerSide[1] = 0;
                for (var i = 0; i < groupCount; ++i)
                {
                    totalPerSide[sSide[i]] += sUnitCount[i];
                }
            }
            while ((totalPerSide[0] > 0) && (totalPerSide[1] > 0));
            var winner = (totalPerSide[0] > 0) ? 0 : 1;
            return totalPerSide[winner];
        }

        private static void Fight()
        {
            var groupCount = sGroupCount;
            // During the target selection phase, each group attempts to choose one target.
            // In decreasing order of effective power, groups choose their targets; in a tie, the group with the higher initiative chooses first.
            // The attacking group chooses to target the group in the enemy army to which it would deal the most damage (after accounting for weaknesses and immunities, but not accounting for whether the defending group has enough units to actually receive all of that damage).

            // If an attacking group is considering two defending groups to which it would deal equal damage, it chooses to target the defending group with the largest effective power; if there is still a tie, it chooses the defending group with the highest initiative.
            // If it cannot deal any defending groups damage, it does not choose a target.
            // Defending groups can only be chosen as a target by one attacking group.
            var damage = new int[groupCount, groupCount];
            for (var a = 0; a < groupCount; ++a)
            {
                for (var d = 0; d < groupCount; ++d)
                {
                    damage[a, d] = 0;
                }
            }

            for (var a = 0; a < groupCount; ++a)
            {
                if (sUnitCount[a] == 0)
                {
                    continue;
                }
                var side = sSide[a];
                for (var d = 0; d < groupCount; ++d)
                {
                    if (sUnitCount[d] == 0)
                    {
                        continue;
                    }
                    if (sSide[d] == side)
                    {
                        continue;
                    }
                    var damageAmount = ComputeDamage(a, d);
                    damage[a, d] = damageAmount;
                }
            }

            // sort attacker by effective power then attacker initiative
            var attackTarget = new int[groupCount];
            var attackerOrder = new int[groupCount];
            for (var g = 0; g < groupCount; ++g)
            {
                attackTarget[g] = -1;
                attackerOrder[g] = g;
            }

            for (var i = 0; i < groupCount - 1; ++i)
            {
                for (var j = i + 1; j < groupCount; ++j)
                {
                    var attacker1 = attackerOrder[i];
                    var attacker2 = attackerOrder[j];
                    var effectivePower1 = sAttackPower[attacker1] * sUnitCount[attacker1];
                    var effectivePower2 = sAttackPower[attacker2] * sUnitCount[attacker2];
                    var swap = false;
                    if (effectivePower2 > effectivePower1)
                    {
                        swap = true;
                    }
                    else if (effectivePower2 == effectivePower1)
                    {
                        swap = sInitiative[attacker2] > sInitiative[attacker1];
                    }
                    if (swap)
                    {
                        attackerOrder[j] = attacker1;
                        attackerOrder[i] = attacker2;
                    }
                }
            }

            for (var i = 0; i < groupCount; ++i)
            {
                var a = attackerOrder[i];
                var target = -1;
                var maxDamage = int.MinValue;
                for (var d = 0; d < groupCount; ++d)
                {
                    var targetAvailable = true;
                    for (var j = 0; j < groupCount; ++j)
                    {
                        if (attackTarget[j] == d)
                        {
                            targetAvailable = false;
                            break;
                        }
                    }
                    if (!targetAvailable)
                    {
                        continue;
                    }
                    var dmg = damage[a, d];
                    if (dmg == 0)
                    {
                        continue;
                    }
                    if (dmg > maxDamage)
                    {
                        maxDamage = dmg;
                        target = d;
                    }
                    if (dmg == maxDamage)
                    {
                        var effectivePower1 = sAttackPower[target] * sUnitCount[target];
                        var effectivePower2 = sAttackPower[d] * sUnitCount[d];
                        if (effectivePower2 > effectivePower1)
                        {
                            target = d;
                        }
                        else if (effectivePower2 == effectivePower1)
                        {
                            if (sInitiative[d] > sInitiative[target])
                            {
                                target = d;
                            }
                        }
                    }
                }
                attackTarget[a] = target;
            }

            for (var i = 0; i < groupCount; ++i)
            {
                var a = sInitiativeOrder[i];
                var target = attackTarget[a];
                if (target >= 0)
                {
                    var dmg = ComputeDamage(a, target);
                    var units = dmg / sHP[target];
                    units = Math.Min(sUnitCount[target], units);
                    sUnitCount[target] -= units;
                    Console.WriteLine($"{a} -> {target} {dmg} {units} {sUnitCount[target]}");
                }
            }
        }

        public static void Run()
        {
            Console.WriteLine("Day24 : Start");
            _ = new Program("Day24/input.txt", true);
            _ = new Program("Day24/input.txt", false);
            Console.WriteLine("Day24 : End");
        }
    }
}
