using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AOC;

try
{
	HelperMethods.Year = 2021;
	HelperMethods.ExectuingAssembly = Assembly.GetExecutingAssembly();

	//DayOne();
	//DayTwo();
	//DayThree();
	//DayFour();
	//DayFive();
	//DaySix();
	//DaySeven();

	Console.ReadLine();

	#region Week 1
	void DayOne()
	{
		string anwser = "Day 1: ";
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
		string answer = "Day 2: ";
		string[] tmp = HelperMethods.GetInput(day: 2).Split(Environment.NewLine);
		(string, int)[] input = Enumerable.Range(0, tmp.Length)
			.Select(i => (tmp[i].Split(' ')[0], int.Parse(tmp[i].Split(' ')[1])))
			.ToArray();

		int depth = 0;
		int horizontal = 0;
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
	void DayThree()
	{
		string answer = "Day 3: ";
		string[] input = HelperMethods.GetInput(day: 3).Split(Environment.NewLine);

		int bitCount = 0;
		string gammaRate = null;
		string epsilonRate = null;
		for (int i = 0; i < input[0].Length; i++)
		{
			bitCount = 0;
			for (int j = 0; j < input.Length; j++)
			{
				bitCount += input[j][i] is '1'
					?  1
					: -1;
			}

			gammaRate += (bitCount > 0)
				? '1'
				: '0';
			epsilonRate += (bitCount < 0)
				? '1'
				: '0';
		}

		answer += $"Part 1: {Convert.ToInt32(gammaRate, fromBase: 2) * Convert.ToInt32(epsilonRate, fromBase: 2)} ";

		string scrubRating = null;
		string oxygenGenRating = null;

		scrubRating = NarrowDown(input, condition: (x) => x < 0, searchFor: '0', startPos: 0);
		oxygenGenRating = NarrowDown(input, condition: (x) => x >= 0, searchFor: '1', startPos: 0);

		answer += $"Part 2: {Convert.ToInt32(scrubRating, fromBase: 2) * Convert.ToInt32(oxygenGenRating, fromBase: 2)}";
		Console.WriteLine(answer);

		string NarrowDown(string[] array, Func<int,bool> condition,  char searchFor, int startPos)
		{
			if (array.Length is 1)
			{
				return array[0];
			}

			bitCount = 0;
			for (int j = 0; j < array.Length; j++)
			{
				bitCount += (array[j][startPos] is '1')
					?  1
					: -1;
			}

			searchFor = condition(bitCount) ? '1' : '0';

			string[] narrowedArray = Array.FindAll(array, s => s[startPos] == searchFor);
			return NarrowDown(narrowedArray, condition, searchFor, startPos + 1);
		}
	}
	void DayFour()
	{
		string answer = "Day 4: ";
		string[] input = HelperMethods.GetInput(day: 4).Split(Environment.NewLine + Environment.NewLine);

		bool bingo = false;
		int bingoNumPointer = 0;
		string[] boards = input[1..];
		string[] bingoNumbers = input[0].Split(',');

		while (!bingo)
		{			
			for (int i = 0; i < boards.Length; i++)
			{
				int[,] formattedBoard = FormatBoard(boards[i].Split(Environment.NewLine));

				if (CheckForBingoRow(formattedBoard) || CheckForBingoCol(formattedBoard))
				{
					answer += $"Part 1: {SumOfEmptySquares(formattedBoard) * int.Parse(bingoNumbers[bingoNumPointer])} ";
					bingo = true;
					break;
				};
			}

			bingoNumPointer++;
		}

		List<(int board, int bingoNumber, int total)> winOrder = new();
		while (bingoNumPointer < bingoNumbers.Length - 1)
		{
			for (int i = 0; i < boards.Length; i++)
			{
				int[,] formattedBoard = FormatBoard(boards[i].Split(Environment.NewLine));

				if ((CheckForBingoRow(formattedBoard) || CheckForBingoCol(formattedBoard)) &&
					!winOrder.Any(x => x.board == i))
				{
					winOrder.Add((board: i, bingoNumber: int.Parse(bingoNumbers[bingoNumPointer]), total: SumOfEmptySquares(formattedBoard)));
				};
			}

			bingoNumPointer++;
		}

		answer += $"Part 2: {winOrder.Last().bingoNumber * winOrder.Last().total}";

		int[,] FormatBoard(string[] board)
		{
			int[,] result = new int[5,5];
			for (int i = 0; i < 5; i++)
			{
				string[] numbers = board[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
				for (int j = 0; j < 5; j++)
				{
					result[i, j] = int.Parse(numbers[j]);
				}
			}
			return result;
		}
		bool CheckForBingoRow(int[,] board)
		{
			int[] calledNumbers = Array.ConvertAll(bingoNumbers[0..(bingoNumPointer + 1)], x => int.Parse(x));

			for (int i = 0; i < 5; i++)
			{
				bool isBingo = true;
				for (int j = 0; j < 5; j++)
				{
					if (!calledNumbers.Contains(board[i,j]))
					{
						isBingo = false;
					}
				}
				if (isBingo)
				{
					return true;
				}
			}
			return false;
		}
		bool CheckForBingoCol(int[,] board)
		{
			int[] calledNumbers = Array.ConvertAll(bingoNumbers[0..(bingoNumPointer + 1)], x => int.Parse(x));

			for (int i = 0; i < 5; i++)
			{
				bool isBingo = true;
				for (int j = 0; j < 5; j++)
				{
					if (!calledNumbers.Contains(board[j, i]))
					{
						isBingo = false;
					}
				}
				if (isBingo)
				{
					return true;
				}
			}
			return false;
		}
		int SumOfEmptySquares(int[,] board)
		{
			int total = 0;
			int[] calledNumbers = Array.ConvertAll(bingoNumbers[0..(bingoNumPointer + 1)], x => int.Parse(x));

			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (!calledNumbers.Contains(board[i, j]))
					{
						total += board[i,j];
					}
				}
			}
			return total;
		}
	}
	void DayFive()
	{
		string answer = "Day 5: ";
		string[] input = HelperMethods.GetInput(day: 5).Split(Environment.NewLine);

		const int BoardSize = 1000;

		int[,] board = new int[BoardSize, BoardSize];

		foreach (string line in input)
		{
			int[] coordinates = Array.ConvertAll(
				array: line.Split(new char[] { ',', '-', '>' }, StringSplitOptions.RemoveEmptyEntries),
				converter: x => int.Parse(x));

			const int x1 = 0;
			const int y1 = 1;
			const int x2 = 2;
			const int y2 = 3;

			int Xoffset = Math.Abs(coordinates[x2] - coordinates[x1]);
			int Yoffset = Math.Abs(coordinates[y2] - coordinates[y1]);

			if (coordinates[x1] == coordinates[x2] ||
				coordinates[y1] == coordinates[y2])
			{
				if (Xoffset is not 0)
				{
					for (int i = 0; Math.Abs(i) <= Xoffset;)
					{
						board[coordinates[y1], coordinates[x1] + i] += 1;
						i += coordinates[x1] < coordinates[x2] ? 1 : -1;
					}
				}
				if (Yoffset is not 0)
				{
					for (int i = 0; Math.Abs(i) <= Yoffset;)
					{
						board[coordinates[y1] + i, coordinates[x1]] += 1;
						i += coordinates[y1] < coordinates[y2] ? 1 : -1;
					}
				}
			}
		}
				
		answer += $"Part 1: {CalculateOverlappingPipeCount()} ";

		board = new int[BoardSize, BoardSize];
		foreach (string line in input)
		{
			int[] coordinates = Array.ConvertAll(
				array: line.Split(new char[] { ',', '-', '>' }, StringSplitOptions.RemoveEmptyEntries),
				converter: x => int.Parse(x));

			const int x1 = 0;
			const int y1 = 1;
			const int x2 = 2;
			const int y2 = 3;

			int Xoffset = Math.Abs(coordinates[x2] - coordinates[x1]);
			int Yoffset = Math.Abs(coordinates[y2] - coordinates[y1]);

			for (int i = 0, j = 0; Math.Abs(i) <= Xoffset && Math.Abs(j) <= Yoffset;)
			{
				board[coordinates[y1] + j, coordinates[x1] + i] += 1;

				if (Xoffset is not 0)
				{
					i += coordinates[x1] < coordinates[x2] ? 1 : -1;
				}
				if (Yoffset is not 0)
				{
					j += coordinates[y1] < coordinates[y2] ? 1 : -1;
				}
			}
		}

		answer += $"Part 2: {CalculateOverlappingPipeCount()}";

		int CalculateOverlappingPipeCount()
		{
			int overlappingPipeCount = 0;
			for (int i = 0; i < BoardSize; i++)
			{
				for (int j = 0; j < BoardSize; j++)
				{
					if (board[i, j] >= 2)
					{
						overlappingPipeCount++;
					}
				}
			}
			return overlappingPipeCount;
		}
	}
	void DaySix()
	{
		string answer = "Day 6: ";
		int[] input = Array.ConvertAll(
			array: HelperMethods.GetInput(day: 6).Split(','),
			converter: x => int.Parse(x));
		
		answer += $"Part 1: {SimulateLanternFish(days: 80)} ";
		answer += $"Part 2: {SimulateLanternFish(days: 256)}";

		long SimulateLanternFish(int days)
		{
			long[] fishTimers = new long[9];

			foreach (int i in input)
			{
				fishTimers[i]++;
			}

			for (int i = 0; i < days; i++)
			{
				long tmp = fishTimers[0];
				for (int j = 1; j < fishTimers.Length; j++)
				{
					fishTimers[j - 1] = fishTimers[j];
				}
				fishTimers[6] += tmp;
				fishTimers[8] = tmp;
			}

			long total = 0;
			foreach (long l in fishTimers)
			{
				total += l;
			}
			return total;
		}
	}
	void DaySeven()
	{
		string answer = "Day 7: ";
		int[] input = Array.ConvertAll(
			array: HelperMethods.GetInput(day: 7).Split(','),
			converter: x => int.Parse(x));

		int lowestFuel = int.MaxValue;
		for(int i = 0; i < input.Max(); i++)
		{
			int totalFuel = 0;
			foreach (int j in input)
			{
				totalFuel += Math.Abs(i - j);
			}
			if (totalFuel < lowestFuel)
			{
				lowestFuel = totalFuel;
			}
		}

		answer += $"Part 1: {lowestFuel} ";

		lowestFuel = int.MaxValue;
		for (int i = 0; i < input.Max(); i++)
		{
			int totalFuel = 0;
			foreach (int j in input)
			{
				int tmp = Math.Abs(i - j);

				for (int k = tmp; k > 0; k--)
				{
					totalFuel += k;
				}
			}
			if (totalFuel < lowestFuel)
			{
				lowestFuel = totalFuel;
			}
		}

		answer += $"Part 2: {lowestFuel} ";
	}
	#endregion


}
finally
{
	Console.ResetColor();
	Console.Clear();
}