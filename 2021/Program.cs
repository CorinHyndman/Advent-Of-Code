using System;
using System.Linq;
using System.Reflection;
using AOC;

try
{
	HelperMethods.Year = 2021;
	HelperMethods.ExectuingAssembly = Assembly.GetExecutingAssembly();

	DayOne();
	DayTwo();

	Console.ReadLine();

	void DayOne()
	{
		string anwser = null;
		int[] input = Array.ConvertAll(
			array: HelperMethods.GetInput(day: 1).Split(Environment.NewLine),
			converter: x => int.Parse(x));

		int count = 0;
		for (int i = 0; i < input.Length - 1;)
		{
			if (input[i] < input[++i])
			{
				count++;
			}
		}

		anwser += $"Part 1: {count} ";

		count = 0;
		int A = input[0] + input[1] + input[2];
		for (int i = 1; i < input.Length - 2; i++)
		{
			int B = input[i] + input[i+1] + input[i+2];

			if (A < B)
			{
				count++;
			}
			
			A = B;
		}

		anwser += $"Part 2: {count}";
	}
	void DayTwo()
	{
		string[] tmp = HelperMethods.GetInput(day: 2).Split(Environment.NewLine);
		(string, int)[] input = Enumerable.Range(0, tmp.Length)
			.Select(i => (tmp[i].Split(' ')[0], int.Parse(tmp[i].Split(' ')[1])))
			.ToArray();

		int depth = 0;
		int horizontal = 0;
		string answer = null;
		foreach ((string Direction, int Value) in input)
		{
			switch (Direction)
			{
				case "up": horizontal -= Value; break;
				case "down": horizontal += Value; break;
				case "forward": depth += Value; break;
				default: Console.WriteLine("Invalid Input Detected"); return;
			}
		}

		answer += $"Part 1: {horizontal * depth} ";

		int aim = depth = horizontal = 0;
		foreach ((string Direction, int Value) in input)
		{
			switch (Direction)
			{
				case "up": aim -= Value; break;
				case "down": aim += Value; break;
				case "forward":
					horizontal += Value;
					depth += aim * Value;
					break;

				default: Console.WriteLine("Invalid Input Detected"); return;
			}
		}

		answer += $"Part 2: {horizontal * depth}";
	}
}
finally
{
	Console.ResetColor();
	Console.Clear();
}