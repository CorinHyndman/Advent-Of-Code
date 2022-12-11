using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO.Pipes;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using _2022;
using AOC;
using static System.Formats.Asn1.AsnWriter;

try
{
    HelperMethods.Year = 2022;
    HelperMethods.ExectuingAssembly = Assembly.GetExecutingAssembly();

    /// ignore make output look good :]
    string AOCString = "┌" + new string('─', 31) + "┐" + Environment.NewLine +
        string.Format("│{0,-23}{1,8}│", "Advent of Code 2022", "By Corin");
    string outputFormatString = "│ Day {0} │ Part 1: {1,-12} │ Part 2: {2,-12} │";
    string borderFormatString = string.Format(@"{{0}}{0}{{1}}{1}{{2}}{1}{{3}}", new string('─', 8), new string('─', 22));
    string thanksString = "│Thanks for checking out my repo│" + Environment.NewLine +
        "└" + new string('─', 31) + "┘";
    string AOCSpecialCaseString =  Environment.NewLine +
                               "┌───────────────┐" + Environment.NewLine +
                               "│ Special Cases │" + Environment.NewLine +
                               "└───────────────┘";

    string answers =
        AOCString + Environment.NewLine +
        string.Format(borderFormatString, '├', '┬', '┼', '┐') + Environment.NewLine +

        Day01() + Environment.NewLine +
        Day02() + Environment.NewLine +
        Day03() + Environment.NewLine +
        Day04() + Environment.NewLine +
        Day05() + Environment.NewLine +
        Day06() + Environment.NewLine +
        Day07() + Environment.NewLine +
        Day08() + Environment.NewLine +
        Day09() + Environment.NewLine +
        Day10() + Environment.NewLine +
        Day11() + Environment.NewLine +

        string.Format(borderFormatString, '├', '┴', '┼', '┘') + Environment.NewLine +
        thanksString;

    string specialAnswers =
        AOCSpecialCaseString + Environment.NewLine +

        Day10SpecialCase() + Environment.NewLine;

    Console.WriteLine(answers);
    Console.WriteLine(specialAnswers);
    Console.ReadLine();

    string Day01()
    {
        string[] input = HelperMethods.GetInput(day: 1).Split(Environment.NewLine);

        int currentCalories = 0;
        List<int> allCalories = new List<int>();
        foreach (string calorie in input)
        {
            if (calorie is "")
            {
                allCalories.Add(currentCalories);
                currentCalories = 0;
            }
            else
            {
                currentCalories += int.Parse(calorie);
            }
        }
        allCalories = allCalories.OrderByDescending(x => x).ToList();

        return string.Format(outputFormatString, "01", allCalories[0], allCalories[0] + allCalories[1] + allCalories[2]);
    }
    string Day02()
    {
        string[] input = HelperMethods.GetInput(day: 2).Split(Environment.NewLine);

        const int LOSS = 1;
        const int DRAW = 2;

        const int ROCK = 1;
        const int PAPER = 2;
        const int SCISSORS = 3; 

        Dictionary<char, int> charToValue = new()
        {
            {'A', ROCK },     {'X', ROCK },
            {'B', PAPER },    {'Y', PAPER },
            {'C', SCISSORS }, {'Z', SCISSORS },
        };

        char[] moves;
        int scoreA = 0;
        int scoreB = 0;
        int elfMove;
        int playerMove;
        int roundEnd;
        foreach (string move in input)
        {
            moves = Array.ConvertAll(move.Split(' '), x => x[0]);
            elfMove = charToValue[moves[0]];
            playerMove = charToValue[moves[1]];

            scoreA += playerMove;
            if (playerMove == elfMove)
            {
                scoreA += 3;
            }
            else if (
                playerMove == ROCK && elfMove == SCISSORS ||
                playerMove == PAPER && elfMove == ROCK    ||
                playerMove == SCISSORS && elfMove == PAPER)
            {
                scoreA += 6;
            }
        }

        int moveValue = 0;
        foreach (string move in input)
        {
            moves = Array.ConvertAll(move.Split(' '), x => x[0]);
            elfMove = charToValue[moves[0]];
            roundEnd = charToValue[moves[1]];

            scoreB += (roundEnd * 3) - 3;
            if (roundEnd == DRAW)
            {
                scoreB += elfMove;
            }
            else if (roundEnd == LOSS)
            {
                moveValue = (elfMove + 2) % 3;
                scoreB += moveValue is 0 ? 3 : moveValue;
            }
            else
            {
                moveValue = (elfMove + 1) % 3;
                scoreB += moveValue is 0 ? 3 : moveValue;
            }
        }

        return string.Format(outputFormatString, "02", scoreA, scoreB);
    }
    string Day03()
    {
        string[] input = HelperMethods.GetInput(day: 3).Split(Environment.NewLine);

        int scoreA = 0;
        int asciiValue;
        char currentChar;
        string compartmentLeft;
        string compartmentRight;
        foreach (string s in input)
        {
            compartmentLeft = s[..(s.Length / 2)];
            compartmentRight = s[(s.Length / 2)..];
            for (int i = 0; i < compartmentLeft.Length; i++)
            {
                currentChar = compartmentLeft[i];
                if (compartmentRight.Any(x => x == currentChar))
                {
                    asciiValue = (int)currentChar;
                    scoreA += asciiValue > 90 // After Z
                        ? asciiValue - 96
                        : asciiValue - 38;

                    break;
                }
            }
        }

        int scoreB = 0;
        for (int i = 0; i < input.Length; i+=3)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                currentChar = input[i][j];
                if (input[i + 1].Any(x => x == currentChar) &&
                    input[i + 2].Any(x => x == currentChar))
                {
                    asciiValue = (int)currentChar;
                    scoreB += asciiValue > 90 // After Z
                        ? asciiValue - 96
                        : asciiValue - 38;

                    break;
                }
            }
        }

        return string.Format(outputFormatString, "03", scoreA, scoreB);
    }
    string Day04()
    {
        string[] input = HelperMethods.GetInput(day: 4).Split(Environment.NewLine);

        int countOverlap = 0;
        int countContains = 0;
        foreach (string s in input)
        {
            int[] pairs = Array.ConvertAll(s.Split(',','-'), x => int.Parse(x));
            if (pairs[0] >= pairs[2] && pairs[1] <= pairs[3] ||
                pairs[2] >= pairs[0] && pairs[3] <= pairs[1])
            {
                countContains++;
            }

            if (pairs[0] >= pairs[2] && pairs[0] <= pairs[3] ||
                pairs[1] >= pairs[2] && pairs[1] <= pairs[3] ||
                pairs[2] >= pairs[0] && pairs[2] <= pairs[1] ||
                pairs[3] >= pairs[0] && pairs[3] <= pairs[1])
            {
                countOverlap++;
            }
        }

        return string.Format(outputFormatString, "04", countContains, countOverlap);
    }
    string Day05()
    {
        string[] input = HelperMethods.GetInput(day: 5).Split(Environment.NewLine);

        int crateEndPositions = Array.IndexOf(input, "") - 2;
        int stackCount = int.Parse(input[crateEndPositions + 1].Trim().Split(' ')[^1]); // get last number at end of string under crates
        Stack<char>[] crates9000 = new Stack<char>[stackCount];
        Stack<char>[] crates9001 = new Stack<char>[stackCount];
        for (int i = 0; i < crates9000.Length; i++)
        {
            crates9000[i] = new Stack<char>();
            crates9001[i] = new Stack<char>();
        }

        char crateValue;
        int stackCurrent;
        for (int i = crateEndPositions; i >= 0; i--)
        {
            stackCurrent = 0;
            for (int j = 1; j < input[i].Length; j += 4)
            {
                crateValue = input[i][j];
                if (crateValue is not ' ')
                {
                    crates9000[stackCurrent].Push(crateValue);
                    crates9001[stackCurrent].Push(crateValue);
                }
                stackCurrent++;
            }
        }

        const int COUNT = 0;
        const int FROM = 1;
        const int TO = 2;
        List<char> crateValues = new();
        char[] wordCharacters = { 'm', 'o', 'v', 'e', 'f', 'r', 'o', 'm', 't', 'o' };
        int instructionStartPosition = Array.IndexOf(input, "") + 1;
        for (int i = instructionStartPosition; i < input.Length; i++)
        {
            int[] moveInstructions = Array.ConvertAll(input[i].Split(wordCharacters,StringSplitOptions.RemoveEmptyEntries), x => int.Parse(x));

            moveInstructions[FROM]--;
            moveInstructions[TO]--;

            for (int j = 0; j < moveInstructions[COUNT]; j++)
            {
                crateValue = crates9000[moveInstructions[FROM]].Pop();
                crates9000[moveInstructions[TO]].Push(crateValue);
            }

            crateValues.Clear();
            for (int j = 0; j < moveInstructions[COUNT]; j++)
            {
                crateValues.Add(crates9001[moveInstructions[FROM]].Pop());
            }
            for (int j = crateValues.Count - 1; j >= 0; j--)
            {
                crates9001[moveInstructions[TO]].Push(crateValues[j]);
            }
        }

        string crateMover9000Message = "";
        string crateMover9001Message = "";
        for (int i = 0; i < crates9000.Length; i++)
        {
            crateMover9000Message += crates9000[i].Peek();
            crateMover9001Message += crates9001[i].Peek();
        }

        return string.Format(outputFormatString, "05", crateMover9000Message, crateMover9001Message);
    }
    string Day06()
    {
        string input = HelperMethods.GetInput(day: 6);

        int fourCharMarker = 0;
        int startOfMessageMarker = 0;
        Queue<char> uniqueChars = new();
        foreach (int i in new int[] { 4,14 })
        {
            uniqueChars.Clear();
            for (int j = 0; j < input.Length; j++)
            {
                if (uniqueChars.Count == i)
                {
                    uniqueChars.Dequeue();
                }
                uniqueChars.Enqueue(input[j]);

                if (uniqueChars.Distinct().Count() == i)
                {
                    if (i is 4)
                    {
                        fourCharMarker = j + 1;
                    }
                    else 
                    {
                        startOfMessageMarker = j + 1;
                    }
                    break;
                }
            }
        }

        return string.Format(outputFormatString, "06", fourCharMarker, startOfMessageMarker);

    }
    string Day07()
    {
        string[] input = HelperMethods.GetInput(day: 7).Split(Environment.NewLine);

        TreeNode ROOT = new ("/", 0);
        TreeNode currentNode = ROOT;

        string[] cmdParts;
        foreach (string cmd in input)
        {
            if (cmd.Contains("cd "))
            {
                cmdParts = cmd.Split(' ');
                if (cmdParts[2] is "..")
                {
                    currentNode = currentNode.Parent;
                }
                else
                {
                    currentNode = currentNode.EnterChild(cmdParts[2]);
                }
            }
            else if (cmd.Contains("dir "))
            {
                cmdParts = cmd.Split(' ');
                currentNode.AddChild(cmdParts[1], 0);
            }
            else if (cmd.Contains("$ ls")) { }
            else
            {
                cmdParts = cmd.Split(' ');
                currentNode.AddChild(cmdParts[1], int.Parse(cmdParts[0]));
            }
        }

        while (currentNode.FileName is not "/")
        {
            currentNode = currentNode.Parent;
        }

        int total = 0;
        List<int> directorySizes = currentNode.GetTotalSizes();



        return string.Format(outputFormatString, "07", "TODO", "TODO");
    }
    string Day08()
    {
        char[][] input = HelperMethods.GetInput(day: 8)
            .Split(Environment.NewLine)
            .Select(x => x.ToArray())
            .ToArray();

        const int UP = 0;
        const int DOWN = 1;
        const int LEFT = 2;
        const int RIGHT = 3;

        bool[] visibleSides;
        int currentValue;
        int row = input.Length;
        int col = input[0].Length;
        int visibleTrees = col * 2 + (row-2) * 2;
        for (int ri = 1; ri < row - 1; ri++)
        {
            for (int ci = 1; ci < col - 1; ci++)
            {
                visibleSides = new bool[] { true, true, true, true };
                currentValue = input[ri][ci];
                for (int i = 0; i < row; i++)
                {
                    if (i < ri)
                    {
                        if (input[i][ci] >= input[ri][ci])
                        {
                            visibleSides[UP] = false;
                        }
                    }
                    if(i > ri)
                    {
                        if (input[i][ci] >= input[ri][ci])
                        {
                            visibleSides[DOWN] = false;
                        }
                    }
                }
                for (int i = 0; i < col; i++)
                {
                    if (i < ci)
                    {
                        if (input[ri][i] >= input[ri][ci])
                        {
                            visibleSides[LEFT] = false;
                        }
                    }
                    if (i > ci)
                    {
                        if (input[ri][i] >= input[ri][ci])
                        {
                            visibleSides[RIGHT] = false;
                        }
                    }
                }

                if (visibleSides.Contains(true))
                {
                    visibleTrees++;
                }
            }
        }

        int maxScenicScore = 0;
        int totalScenicScore = 0;
        int[] visibleSideTreeCount;
        for (int ri = 1; ri < row - 1; ri++)
        {
            for (int ci = 1; ci < col - 1; ci++)
            {
                currentValue = input[ri][ci];
                visibleSideTreeCount = new int[4];
                for (int i = 0; i < row; i++)
                {
                    if (i < ri)
                    {
                        if (input[i][ci] >= input[ri][ci] || i == 0)
                        {
                            visibleSideTreeCount[UP] = ri - i;
                        }
                    }
                    if (i > ri)
                    {
                        if (input[i][ci] >= input[ri][ci] || i == row - 1)
                        {
                            visibleSideTreeCount[DOWN] = i - ri;
                            break;
                        }
                    }
                }
                for (int i = 0; i < col; i++)
                {
                    if (i < ci)
                    {
                        if (input[ri][i] >= input[ri][ci] || i == 0)
                        {
                            visibleSideTreeCount[LEFT] = ci - i;
                        }
                    }
                    if (i > ci)
                    {
                        if (input[ri][i] >= input[ri][ci] || i == col-1)
                        {
                            visibleSideTreeCount[RIGHT] = i - ci;
                            break;
                        }
                    }
                }

                totalScenicScore = 
                    visibleSideTreeCount[0] *
                    visibleSideTreeCount[1] *
                    visibleSideTreeCount[2] *
                    visibleSideTreeCount[3];

                if (totalScenicScore > maxScenicScore)
                {
                    maxScenicScore = totalScenicScore;
                }
            }
        }

        return string.Format(outputFormatString,"08",visibleTrees,maxScenicScore);
    }
    string Day09()
    {
        string[] input = HelperMethods.GetInput(day: 9).Split(Environment.NewLine);

        double a;
        double b;
        const int HEAD = 0;
        (int X, int Y)[] rope = new (int X, int Y)[10];
        List<(int X, int Y)> visitedPositionsA = new();
        List<(int X, int Y)> visitedPositionsB = new();
        foreach (string s in input)
        {
            string[] contents = s.Split(' ');
            char direction = contents[0][0];
            int magnitude = int.Parse(contents[1]);

            for (int i = 0; i < magnitude; i++)
            {
                switch (s[0])
                {
                    case 'U': rope[HEAD].Y--; break;
                    case 'D': rope[HEAD].Y++; break;
                    case 'L': rope[HEAD].X--; break;
                    case 'R': rope[HEAD].X++; break;
                }
                for (int j = 0; j < rope.Length - 1; j++)
                {
                    a = Math.Abs(rope[j].X - rope[j + 1].X);
                    b = Math.Abs(rope[j].Y - rope[j + 1].Y);
                    if (Math.Sqrt(a * a + b * b) > 1.5)
                    {
                        if (rope[j + 1].X == rope[j].X ||
                            rope[j + 1].Y == rope[j].Y)
                        {
                            rope[j + 1].X += (rope[j].X - rope[j + 1].X) / 2;
                            rope[j + 1].Y += (rope[j].Y - rope[j + 1].Y) / 2;
                        }
                        else
                        {
                            rope[j + 1].X += rope[j + 1].X < rope[j].X ? 1 : -1;
                            rope[j + 1].Y += rope[j + 1].Y < rope[j].Y ? 1 : -1;
                        }
                    }

                    if (j is 0 && !visitedPositionsA.Contains((rope[1].X, rope[1].Y)))
                    {
                        visitedPositionsA.Add((rope[1].X, rope[1].Y));
                    }
                    if (j is 8 && !visitedPositionsB.Contains((rope[9].X, rope[9].Y)))
                    {
                        visitedPositionsB.Add((rope[9].X, rope[9].Y));
                    }
                }
            }
        }

        return string.Format(outputFormatString, "09", visitedPositionsA.Count, visitedPositionsB.Count);
    }
    string Day10()
    {
        string[] input = HelperMethods.GetInput(day: 10).Split(Environment.NewLine);

        const int COMMAND = 0;
        const int VALUE = 1;

        bool busy = false;
        int x = 1;
        int cycleCount = 0;
        int currentInstruction = 0;
        int signalStrengthTotal = 0;
        int signalStrengthCycle = 0;
        (int LastCycle, int Value) addRegister = (0,0);
        while (currentInstruction < input.Length || busy)
        {
            cycleCount++;
            addRegister.LastCycle++;

            if (cycleCount % (20 + (40 * signalStrengthCycle)) is 0)
            {
                signalStrengthTotal += cycleCount * x;
                signalStrengthCycle++;
            }

            if (busy)
            {
                if (addRegister.LastCycle > 0)
                {
                    x += addRegister.Value;

                    addRegister.LastCycle = 0;
                    addRegister.Value = 0;
                    busy = false;
                }
            }
            else
            {
                string[] contents = input[currentInstruction].Split(' ');
                if (contents[COMMAND] is not "noop")
                {
                    addRegister.Value = int.Parse(contents[VALUE]);
                    busy = true;
                }
                currentInstruction++;
            }
        }

        return string.Format(outputFormatString,"10", signalStrengthTotal, "SPECIAL CASE");
    }
    string Day10SpecialCase()
    {
        string[] input = HelperMethods.GetInput(day: 10).Split(Environment.NewLine);

        const int COMMAND = 0;
        const int VALUE = 1;

        StringBuilder answer = new();
        bool busy = false;
        int x = 1;
        int cycleCount = 0;
        int pixelPosition = 0;
        int currentInstruction = 0;
        int signalStrengthTotal = 0;
        int signalStrengthCycle = 0;
        int relativePixelPosition = 0;
        (int LastCycle, int Value) addRegister = (0, 0);
        while (currentInstruction < input.Length || busy)
        {
            cycleCount++;
            addRegister.LastCycle++;

            if (cycleCount % (20 + (40 * signalStrengthCycle)) is 0)
            {
                signalStrengthTotal += cycleCount * x;
                signalStrengthCycle++;
            }

            relativePixelPosition = pixelPosition % 40;
            if (relativePixelPosition is 0)
            {
                answer.AppendLine();
            }
            if (relativePixelPosition >= x - 1 && relativePixelPosition <= x + 1)
            {
                answer.Append('█');
            }
            else
            {
                answer.Append(' ');
            }
            pixelPosition++;

            if (busy)
            {
                if (addRegister.LastCycle > 0)
                {
                    x += addRegister.Value;

                    addRegister.LastCycle = 0;
                    addRegister.Value = 0;
                    busy = false;
                }
            }
            else
            {
                string[] contents = input[currentInstruction].Split(' ');
                if (contents[COMMAND] is not "noop")
                {
                    addRegister.Value = int.Parse(contents[VALUE]);
                    busy = true;
                }
                currentInstruction++;
            }
        }

        // nicely output format
        string[] tempAnswer = answer.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        int answerLength = tempAnswer[0].Length + 2;
        StringBuilder output = new();

        output.AppendLine(string.Format("┌{0}┬{0}┐", new string('─',7)));
        output.AppendLine(string.Format("│{0,7}│{1,7}│", "Day 10", "Part 2"));
        output.AppendLine(string.Format("├{0}┴{0}┴{1}┐", new string('─', 7), new string('─', answerLength - 16)));
        foreach (string s in tempAnswer)
        {
            output.AppendLine(string.Format("│ {0} │", s));
        }
        output.AppendLine($"└{new string('─', answerLength)}┘");

        return output.ToString();
    }
    string Day11()
    {
        string[] input = HelperMethods.GetInput(day: 11).Split(Environment.NewLine,StringSplitOptions.RemoveEmptyEntries);
        
        return string.Format(outputFormatString, "11", "TODO", "TODO");
    }
}
finally
{
    Console.ResetColor();
    Console.Clear();
}