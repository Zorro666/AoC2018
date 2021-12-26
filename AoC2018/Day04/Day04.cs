using System;

/*

--- Day 4: Repose Record ---

You've sneaked into another supply closet - this time, it's across from the prototype suit manufacturing lab.
You need to sneak inside and fix the issues with the suit, but there's a guard stationed outside the lab, so this is as close as you can safely get.

As you search the closet for anything that might help, you discover that you're not the first person to want to sneak in.
Covering the walls, someone has spent an hour starting every midnight for the past few months secretly observing this guard post! They've been writing down the ID of the one guard on duty that night - the Elves seem to have decided that one guard was enough for the overnight shift - as well as when they fall asleep or wake up while at their post (your puzzle input).

For example, consider the following records, which have already been organized into chronological order:

[1518-11-01 00:00] Guard #10 begins shift
[1518-11-01 00:05] falls asleep
[1518-11-01 00:25] wakes up
[1518-11-01 00:30] falls asleep
[1518-11-01 00:55] wakes up
[1518-11-01 23:58] Guard #99 begins shift
[1518-11-02 00:40] falls asleep
[1518-11-02 00:50] wakes up
[1518-11-03 00:05] Guard #10 begins shift
[1518-11-03 00:24] falls asleep
[1518-11-03 00:29] wakes up
[1518-11-04 00:02] Guard #99 begins shift
[1518-11-04 00:36] falls asleep
[1518-11-04 00:46] wakes up
[1518-11-05 00:03] Guard #99 begins shift
[1518-11-05 00:45] falls asleep
[1518-11-05 00:55] wakes up
Timestamps are written using year-month-day hour:minute format.
The guard falling asleep or waking up is always the one whose shift most recently started.
Because all asleep/awake times are during the midnight hour (00:00 - 00:59), only the minute portion (00 - 59) is relevant for those events.

Visually, these records show that the guards are asleep at these times:

Date   ID   Minute
            000000000011111111112222222222333333333344444444445555555555
            012345678901234567890123456789012345678901234567890123456789
11-01  #10  .....####################.....#########################.....
11-02  #99  ........................................##########..........
11-03  #10  ........................#####...............................
11-04  #99  ....................................##########..............
11-05  #99  .............................................##########.....
The columns are Date, which shows the month-day portion of the relevant day; ID, which shows the guard on duty that day; and Minute, which shows the minutes during which the guard was asleep within the midnight hour.
(The Minute column's header shows the minute's ten's digit in the first row and the one's digit in the second row.) Awake is shown as ., and asleep is shown as #.

Note that guards count as asleep on the minute they fall asleep, and they count as awake on the minute they wake up.
For example, because Guard #10 wakes up at 00:25 on 1518-11-01, minute 25 is marked as awake.

If you can figure out the guard most likely to be asleep at a specific time, you might be able to trick that guard into working tonight so you can have the best chance of sneaking in.
You have two strategies for choosing the best guard/minute combination.

Strategy 1: Find the guard that has the most minutes asleep.
What minute does that guard spend asleep the most?

In the example above, Guard #10 spent the most minutes asleep, a total of 50 minutes (20+25+5), while Guard #99 only slept for a total of 30 minutes (10+10+10).
Guard #10 was asleep most during minute 24 (on two days, whereas any other minute the guard was asleep was only seen on one day).

While this example listed the entries in chronological order, your entries are in the order you found them.
You'll need to organize them before they can be analyzed.

What is the ID of the guard you chose multiplied by the minute you chose? (In the above example, the answer would be 10 * 24 = 240.)

Your puzzle answer was 99759.

--- Part Two ---

Strategy 2: Of all guards, which guard is most frequently asleep on the same minute?

In the example aboe, Guard #99 spent minute 45 asleep more than any other guard or minute - three times in total. 
(In all other cases, any guard spent any minute asleep at most twice.)

What is the ID of the guard you chose multiplied by the minute you chose? (In the above example, the answer would be 99 * 45 = 4455.)

*/

namespace Day04
{
    class Program
    {
        public enum GuardState { START_SHIFT, FALL_ASLEEP, WAKEUP };
        const int MAX_NUM_GUARDS = 1024 * 128;
        readonly static bool[] sValidGuard = new bool[MAX_NUM_GUARDS];

        const int MAX_NUM_ENTRIES = 2048;
        static int sEntriesCount;
        readonly static int[] sYears = new int[MAX_NUM_ENTRIES];
        readonly static int[] sMonths = new int[MAX_NUM_ENTRIES];
        readonly static int[] sDays = new int[MAX_NUM_ENTRIES];
        readonly static int[] sHours = new int[MAX_NUM_ENTRIES];
        readonly static int[] sMinutes = new int[MAX_NUM_ENTRIES];
        readonly static GuardState[] sGuardStates = new GuardState[MAX_NUM_ENTRIES];
        readonly static int[] sGuardIds = new int[MAX_NUM_ENTRIES];

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = GuardMostAsleepMinute();
                Console.WriteLine($"Day04 : Result1 {result1}");
                var expected = 103720;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = GuardMostAsleepPerMinute();
                Console.WriteLine($"Day04 : Result2 {result2}");
                var expected = 110913;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            for (var g = 0; g < MAX_NUM_GUARDS; ++g)
            {
                sValidGuard[g] = false;
            }

            var e = 0;
            foreach (var line in lines)
            {
                //Timestamps are written using year-month-day hour:minute format.
                //The guard falling asleep or waking up is always the one whose shift most recently started.
                //Because all asleep/awake times are during the midnight hour (00:00 - 00:59), only the minute portion (00 - 59) is relevant for those events.
                var tokens = line.Trim().Split(']');

                //'[1518-11-01 00:55'
                var dateTimeTokens = tokens[0].TrimStart('[').Trim().Split();
                var dateTokens = dateTimeTokens[0].Trim().Split('-');
                var year = int.Parse(dateTokens[0]);
                var month = int.Parse(dateTokens[1]);
                var day = int.Parse(dateTokens[2]);

                var timeTokens = dateTimeTokens[1].Trim().Split(':');
                var hour = int.Parse(timeTokens[0]);
                var minute = int.Parse(timeTokens[1]);

                //'wakes up'
                //'Guard #10 begins shift'
                //'falls asleep'
                var eventTokens = tokens[1].Trim();

                GuardState guardState;
                int guardId = int.MinValue;
                if (eventTokens == "wakes up")
                {
                    guardState = GuardState.WAKEUP;
                }
                else if (eventTokens == "falls asleep")
                {
                    guardState = GuardState.FALL_ASLEEP;
                }
                else if (eventTokens.StartsWith("Guard #") && eventTokens.EndsWith(" begins shift"))
                {
                    var guardToken = eventTokens.Split()[1].TrimStart('#').Trim();
                    guardId = int.Parse(guardToken);
                    guardState = GuardState.START_SHIFT;
                    if (guardId > MAX_NUM_GUARDS)
                    {
                        throw new InvalidProgramException($"Guard {guardId} is too large MAX:{MAX_NUM_GUARDS}");
                    }
                    sValidGuard[guardId] = true;
                }
                else
                {
                    throw new InvalidProgramException($"Unknown event {eventTokens}");
                }
                sYears[e] = year;
                sMonths[e] = month;
                sDays[e] = day;
                sHours[e] = hour;
                sMinutes[e] = minute;
                sGuardStates[e] = guardState;
                sGuardIds[e] = guardId;
                ++e;
            }
            sEntriesCount = e;

            // Sort the entries in chronological order
            for (var e1 = 0; e1 < sEntriesCount - 1; ++e1)
            {
                for (var e2 = e1 + 1; e2 < sEntriesCount; ++e2)
                {
                    if (sYears[e2] > sYears[e1])
                    {
                        continue;
                    }
                    if (sYears[e2] == sYears[e1])
                    {
                        if (sMonths[e2] > sMonths[e1])
                        {
                            continue;
                        }
                        if (sMonths[e2] == sMonths[e1])
                        {
                            if (sDays[e2] > sDays[e1])
                            {
                                continue;
                            }
                            if (sDays[e2] == sDays[e1])
                            {
                                if (sHours[e2] > sHours[e1])
                                {
                                    continue;
                                }
                                if (sHours[e2] == sHours[e1])
                                {
                                    if (sMinutes[e2] > sMinutes[e1])
                                    {
                                        continue;
                                    }
                                    if (sMinutes[e2] == sMinutes[e1])
                                    {
                                        throw new InvalidProgramException($"Identical date-times");
                                    }
                                }
                            }
                        }
                    }
                    var year = sYears[e1];
                    sYears[e1] = sYears[e2];
                    sYears[e2] = year;
                    var month = sMonths[e1];
                    sMonths[e1] = sMonths[e2];
                    sMonths[e2] = month;
                    var day = sDays[e1];
                    sDays[e1] = sDays[e2];
                    sDays[e2] = day;
                    var hour = sHours[e1];
                    sHours[e1] = sHours[e2];
                    sHours[e2] = hour;
                    var minute = sMinutes[e1];
                    sMinutes[e1] = sMinutes[e2];
                    sMinutes[e2] = minute;
                    var guard = sGuardIds[e1];
                    sGuardIds[e1] = sGuardIds[e2];
                    sGuardIds[e2] = guard;
                    var awake = sGuardStates[e1];
                    sGuardStates[e1] = sGuardStates[e2];
                    sGuardStates[e2] = awake;
                }
            }

            // Fill in the unknown guards values
            var currentGuard = int.MinValue;
            for (var e1 = 0; e1 < sEntriesCount; ++e1)
            {
                if (sGuardIds[e1] == int.MinValue)
                {
                    if (currentGuard == int.MinValue)
                    {
                        throw new InvalidProgramException($"Invalid uknown guard as the first entry");
                    }
                    sGuardIds[e1] = currentGuard;
                }
                currentGuard = sGuardIds[e1];
            }
        }

        static void LogEntries()
        {
            for (var e1 = 0; e1 < sEntriesCount; ++e1)
            {
                Console.WriteLine($"{sYears[e1]}-{sMonths[e1]}-{sDays[e1]} {sHours[e1]}:{sMinutes[e1]} Guard {sGuardIds[e1]} State {sGuardStates[e1]}");
            }
        }

        public static int GuardMostAsleepMinute()
        {
            var totalAsleepPerGuard = new int[MAX_NUM_GUARDS];
            var asleepEventPerGuard = new int[MAX_NUM_GUARDS];
            var awakePerGuard = new bool[MAX_NUM_GUARDS];
            for (var g = 0; g < MAX_NUM_GUARDS; ++g)
            {
                totalAsleepPerGuard[g] = 0;
                asleepEventPerGuard[g] = -1;
                awakePerGuard[g] = true;
            }
            var maxAsleepGuard = -1;
            var maxAsleepTime = int.MinValue;

            for (var e = 0; e < sEntriesCount; ++e)
            {
                var g = sGuardIds[e];
                var nowAwake = sGuardStates[e] == GuardState.WAKEUP;
                var nowAsleep = sGuardStates[e] == GuardState.FALL_ASLEEP;
                var wasAwake = awakePerGuard[g];
                var lastEvent = asleepEventPerGuard[g];

                if (sGuardStates[e] == GuardState.START_SHIFT)
                {
                    if (!wasAwake)
                    {
                        throw new InvalidProgramException($"Guard {g} ended shift asleep");
                    }
                }
                if (!wasAwake && nowAwake)
                {
                    // add sleep time
                    var lastHour = sHours[lastEvent];
                    var hour = sHours[e];
                    if (lastHour != hour)
                    {
                        throw new InvalidProgramException($"lastHour != hour {lastHour} != {hour}");
                    }
                    if (hour != 0)
                    {
                        throw new InvalidProgramException($"hour != 0 {hour}");
                    }

                    var lastMinute = sMinutes[lastEvent];
                    var minute = sMinutes[e];
                    var delta = minute - lastMinute;
                    totalAsleepPerGuard[g] += delta;
                    if (totalAsleepPerGuard[g] > maxAsleepTime)
                    {
                        maxAsleepTime = totalAsleepPerGuard[g];
                        maxAsleepGuard = g;
                    }
                }
                if (nowAsleep)
                {
                    asleepEventPerGuard[g] = e;
                }

                awakePerGuard[g] = !nowAsleep;
            }

            for (var g = 0; g < MAX_NUM_GUARDS; ++g)
            {
                if (sValidGuard[g] && !awakePerGuard[g])
                {
                    throw new InvalidProgramException($"Guard {g} is still asleep");
                }
            }

            // Count per minute for the most asleep guard
            var countAsleepPerMinute = new int[60];
            var startAsleepMinute = -1;
            var maxSleepsPerMinute = int.MinValue;
            var mostAsleepMinute = -1;
            for (var e = 0; e < sEntriesCount; ++e)
            {
                var g = sGuardIds[e];
                if (g != maxAsleepGuard)
                {
                    continue;
                }
                if (sGuardStates[e] == GuardState.START_SHIFT)
                {
                    startAsleepMinute = -1;
                    continue;
                }
                else if (sGuardStates[e] == GuardState.FALL_ASLEEP)
                {
                    startAsleepMinute = sMinutes[e];
                    continue;
                }
                else if (sGuardStates[e] == GuardState.WAKEUP)
                {
                    var endAsleepMinute = sMinutes[e];
                    for (var m = startAsleepMinute; m < endAsleepMinute; ++m)
                    {
                        ++countAsleepPerMinute[m];
                        if (countAsleepPerMinute[m] > maxSleepsPerMinute)
                        {
                            maxSleepsPerMinute = countAsleepPerMinute[m];
                            mostAsleepMinute = m;
                        }
                    }
                    startAsleepMinute = -1;
                }
            }

            Console.WriteLine($"Sleepiest Guard {maxAsleepGuard} Time {maxAsleepTime} Max Times {maxSleepsPerMinute} at {mostAsleepMinute}");
            return maxAsleepGuard * mostAsleepMinute;
        }

        public static int GuardMostAsleepPerMinute()
        {
            var maxAsleepGuard = -1;
            var mostAsleepMinute = -1;
            var maxSleepsPerMinute = int.MinValue;

            for (var g = 0; g < MAX_NUM_GUARDS; ++g)
            {
                // Count per minute for the guard
                var countAsleepPerMinute = new int[60];
                var startAsleepMinute = -1;
                for (var e = 0; e < sEntriesCount; ++e)
                {
                    if (g != sGuardIds[e])
                    {
                        continue;
                    }
                    if (sGuardStates[e] == GuardState.START_SHIFT)
                    {
                        startAsleepMinute = -1;
                        continue;
                    }
                    else if (sGuardStates[e] == GuardState.FALL_ASLEEP)
                    {
                        startAsleepMinute = sMinutes[e];
                        continue;
                    }
                    else if (sGuardStates[e] == GuardState.WAKEUP)
                    {
                        var endAsleepMinute = sMinutes[e];
                        for (var m = startAsleepMinute; m < endAsleepMinute; ++m)
                        {
                            ++countAsleepPerMinute[m];
                            if (countAsleepPerMinute[m] > maxSleepsPerMinute)
                            {
                                maxSleepsPerMinute = countAsleepPerMinute[m];
                                maxAsleepGuard = g;
                                mostAsleepMinute = m;
                            }
                        }
                        startAsleepMinute = -1;
                    }
                }
            }

            return maxAsleepGuard * mostAsleepMinute;
        }

        public static void Run()
        {
            Console.WriteLine("Day04 : Start");
            _ = new Program("Day04/input.txt", true);
            _ = new Program("Day04/input.txt", false);
            Console.WriteLine("Day04 : End");
        }
    }
}
