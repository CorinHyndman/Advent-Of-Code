using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AOC;

try
{
	HelperMethods.Year = 2021;
	HelperMethods.ExectuingAssembly = Assembly.GetExecutingAssembly();

	string answers =
		//Day01() + Environment.NewLine +
		//Day02() + Environment.NewLine +
		//Day03() + Environment.NewLine +
		//Day04() + Environment.NewLine +
		//Day05() + Environment.NewLine +
		//Day06() + Environment.NewLine +
		//Day07() + Environment.NewLine +
		//Day08() + Environment.NewLine +
		//Day09() + Environment.NewLine +
		//Day10() + Environment.NewLine +
		//Day11() + Environment.NewLine +
		Day12() + Environment.NewLine;

	Console.WriteLine(answers);

	Console.ReadLine();

	#region Week 1
	string Day01()
	{
		string answer = "Day  1: ";
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

		answer += $"Part 1: {count} ";

		count = 0;
		int A = input[0] + input[1] + input[2];
		for (int i = 1; i < input.Length - 2; i++)
		{
			int B = input[i] + input[i + 1] + input[i + 2];

			if (A < B)
			{
				count++;
			}

			A = B;
		}

		answer += $"Part 2: {count}";
		return answer;
	}
	string Day02()
	{
		string answer = "Day  2: ";
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
			}
		}

		answer += $"Part 2: {horizontal * depth}";
		return answer;
	}
	string Day03()
	{
		string answer = "Day  3: ";
		string[] input = HelperMethods.GetInput(day: 3).Split(Environment.NewLine);

		int bitCount = 0;
		string gammaRate = null;
		string epsilonRate = null;
		for (int i = 0; i < input[0].Length; i++)
		{
			bitCount = 0;
			for (int j = 0; j < input.Length; j++)
			{
				bitCount += input[j][i] is '1' ? 1 : -1;
			}

			gammaRate += (bitCount > 0) ? '1' : '0';
			epsilonRate += (bitCount < 0) ? '1' : '0';
		}

		
		string scrubRating = NarrowDown(input, condition: (x) => x < 0, searchFor: '0', startPos: 0);
		string oxygenGenRating = NarrowDown(input, condition: (x) => x >= 0, searchFor: '1', startPos: 0);

		answer += $"Part 1: {Convert.ToInt32(gammaRate, fromBase: 2) * Convert.ToInt32(epsilonRate, fromBase: 2)} " +
				  $"Part 2: {Convert.ToInt32(scrubRating, fromBase: 2) * Convert.ToInt32(oxygenGenRating, fromBase: 2)}";
		return answer;

		string NarrowDown(string[] array, Func<int, bool> condition, char searchFor, int startPos)
		{
			if (array.Length is 1)
			{
				return array[0];
			}

			bitCount = 0;
			for (int j = 0; j < array.Length; j++)
			{
				bitCount += (array[j][startPos] is '1') ? 1 : -1;
			}

			searchFor = condition(bitCount) ? '1' : '0';

			string[] narrowedArray = Array.FindAll(array, s => s[startPos] == searchFor);
			return NarrowDown(narrowedArray, condition, searchFor, startPos + 1);
		}
	}
	string Day04()
	{
		string answer = "Day  4: ";
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
		return answer;

		int[,] FormatBoard(string[] board)
		{
			int[,] result = new int[5, 5];
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
					if (!calledNumbers.Contains(board[i, j]))
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
						total += board[i, j];
					}
				}
			}
			return total;
		}
	}
	string Day05()
	{
		string answer = "Day  5: ";
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
		return answer;

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
	string Day06()
	{
		string answer = "Day  6: ";
		int[] input = Array.ConvertAll(
			array: HelperMethods.GetInput(day: 6).Split(','),
			converter: x => int.Parse(x));

		answer += $"Part 1: {SimulateLanternFish(days: 80)} Part 2: {SimulateLanternFish(days: 256)}";
		return answer;

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
	string Day07()
	{
		string answer = "Day  7: ";
		int[] input = Array.ConvertAll(
			array: HelperMethods.GetInput(day: 7).Split(','),
			converter: x => int.Parse(x));

		int lowestFuel = int.MaxValue;
		for (int i = 0; i < input.Max(); i++)
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
		return answer;
	}
	#endregion

	#region Week 2
	string Day08()
	{
		string answer = "Day  8: ";
		string[] input = HelperMethods.GetInput(day: 8).Split(Environment.NewLine);

		int count = 0;
		foreach (string s in input)
		{
			string[] signalOutput = s.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

			foreach (string signal in signalOutput)
			{
				if (signal.Length is 2 || signal.Length is 3 ||
					signal.Length is 4 || signal.Length is 7)
				{

					count++;
				}
			}
		}

		answer += $"Part 1: {count} ";

		int total = 0;
		foreach (string s in input)
		{
			string[] signalInput = s.Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
			string[] signalOutput = s.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

			//  0000
			// 1    2
			// 1    2
			//  3333
			// 4    5
			// 4    5
			//  6666  = array layout
			string[] signals = new string[10];
			char[] decodedSignals = new char[7];

			signals[1] = signalInput.First(s => s.Length is 2);
			signals[4] = signalInput.First(s => s.Length is 4);
			signals[7] = signalInput.First(s => s.Length is 3);
			signals[8] = signalInput.First(s => s.Length is 7);

			// Find [0] using 7-1
			decodedSignals[0] = signals[7].Where(s => !signals[1].Contains(s)).First();

			// Find [3] by comparing 4 against 0,6,9
			string[] sixSignals = signalInput.Where(s => s.Length is 6).ToArray();
			char[] unknownFourSides = signals[4].Where(s => !signals[1].Contains(s)).ToArray();
			foreach (char c in unknownFourSides)
			{
				for (int i = 0; i < 3; i++)
				{
					if (!sixSignals[i].Contains(c))
					{
						decodedSignals[3] = c;
					}
				}
			}

			// Find [1] using 4
			decodedSignals[1] = unknownFourSides.Where(s => decodedSignals[3] != s).First();

			// Find [6] using 4 + [0] against 9
			string tmp = signals[4];
			tmp += decodedSignals[0];
			foreach (string signal in sixSignals)
			{
				bool flag = true;
				foreach (char c in tmp)
				{
					if (!signal.Contains(c))
					{
						flag = false;
					}
				}
				if (flag)
				{
					decodedSignals[6] = signal.Where(s => !tmp.Contains(s)).First();
				}
			}

			// Find [4] using 9 - 8
			tmp = signals[4];
			tmp += decodedSignals[0];
			tmp += decodedSignals[6];
			decodedSignals[4] = signals[8].Where(s => !tmp.Contains(s)).First();

			// Find [2] using 8 - 6
			tmp = null;
			tmp += decodedSignals[0];
			tmp += decodedSignals[1];
			tmp += decodedSignals[3];
			tmp += decodedSignals[4];
			tmp += decodedSignals[6];

			foreach (string signal in sixSignals)
			{
				bool flag = true;
				foreach (char c in tmp)
				{
					if (!signal.Contains(c))
					{
						flag = false;
					}
				}
				if (flag)
				{
					tmp = signal;
				}
			}

			decodedSignals[2] = signals[8].Where(s => !tmp.Contains(s)).First();

			// Find [5]
			tmp = null;
			tmp += decodedSignals[0];
			tmp += decodedSignals[1];
			tmp += decodedSignals[2];
			tmp += decodedSignals[3];
			tmp += decodedSignals[4];
			tmp += decodedSignals[6];
			decodedSignals[5] = signals[8].Where(s => !tmp.Contains(s)).First();

			Dictionary<char, char> remapping = new()
			{
				{ decodedSignals[0], 'a' },
				{ decodedSignals[1], 'b' },
				{ decodedSignals[2], 'c' },
				{ decodedSignals[3], 'd' },
				{ decodedSignals[4], 'e' },
				{ decodedSignals[5], 'f' },
				{ decodedSignals[6], 'g' },
			};

			string output = null;
			foreach (string signal in signalOutput)
			{
				string remappedSignal = null;
				foreach (char c in signal)
				{
					remapping.TryGetValue(c, out char a);
					remappedSignal += a;
				}

				remappedSignal = new string(remappedSignal.OrderBy(s => s).ToArray());
				
				output += (remappedSignal) switch
				{
					"abcefg"  => '0',
					"cf"      => '1',
					"acdeg"   => '2',
					"acdfg"   => '3',
					"bcdf"    => '4',
					"abdfg"   => '5',
					"abdefg"  => '6',
					"acf"     => '7',
					"abcdefg" => '8',
					_ => '9',
				};
			}
			total += int.Parse(output);
		}
		answer += $"Part 2: {total}";
		return answer;
	}
	string Day09()
	{
		string answer = "Day  9: ";
		string input = HelperMethods.GetInput(day: 9);
		char[] inputArray = input.Replace(Environment.NewLine, null).ToCharArray();
		int inputWidth = input.Split(Environment.NewLine)[0].Length;

		int totalLowPoints = 0;
		for (int i = 0; i < inputArray.Length; i++)
		{
			int height = (int)inputArray[i] - 48;
			int leftOffset = i % inputWidth is 0 ? int.MaxValue : 1;
			int rightOffset = i % inputWidth == inputWidth - 1 ? int.MaxValue : 1;
			if ((inputArray.TryGetElementAt(i + rightOffset, out char a) && height >= (int)a - 48) ||
				(inputArray.TryGetElementAt(i - leftOffset, out char b) && height >= (int)b - 48) ||
				(inputArray.TryGetElementAt(i + inputWidth, out char c) && height >= (int)c - 48) ||
				(inputArray.TryGetElementAt(i - inputWidth, out char d) && height >= (int)d - 48))
			{
				continue;
			}
			totalLowPoints += 1 + height;
		}

		answer += $"Part 1: {totalLowPoints} ";

		List<int> basinSizes = new();
		List<int> visitedPositions = new();
		for (int i = 0; i < inputArray.Length; i++)
		{
			int height = (int)inputArray[i] - 48;
			int leftOffset = i % inputWidth is 0 ? int.MaxValue : 1;
			int rightOffset = i % inputWidth == inputWidth - 1 ? int.MaxValue : 1;
			if ((inputArray.TryGetElementAt(i + rightOffset, out char a) && height >= (int)a - 48) ||
				(inputArray.TryGetElementAt(i - leftOffset, out char b) && height >= (int)b - 48) ||
				(inputArray.TryGetElementAt(i + inputWidth, out char c) && height >= (int)c - 48) ||
				(inputArray.TryGetElementAt(i - inputWidth, out char d) && height >= (int)d - 48))
			{
				continue;
			}
			basinSizes.Add(CheckBasinSize(position: i, height));
		}

		int total = 1;
		basinSizes = basinSizes.OrderByDescending(x => x).ToList();
		for (int i = 0; i < 3; i++)
		{
			total *= basinSizes[i];
		}

		answer += $"Part 2: {total}";
		return answer;

		int CheckBasinSize(int position, int height)
		{
			int basinCount = 0;

			if (!visitedPositions.Contains(position))
			{
				basinCount = 1;
				visitedPositions.Add(position);
			}

			if (inputArray[position] is '9')
			{
				return 0;
			}

			int leftOffset = position % inputWidth is 0 ? int.MaxValue : 1;
			int rightOffset = position % inputWidth == inputWidth - 1 ? int.MaxValue : 1;
			if (inputArray.TryGetElementAt(position + rightOffset, out char a) && height < (int)a - 48)
			{
				basinCount += CheckBasinSize(position + rightOffset, (int)a - 48);
			}
			if (inputArray.TryGetElementAt(position - leftOffset, out char b) && height < (int)b - 48)
			{
				basinCount += CheckBasinSize(position - leftOffset, (int)b - 48);
			}
			if (inputArray.TryGetElementAt(position + inputWidth, out char c) && height < (int)c - 48)
			{
				basinCount += CheckBasinSize(position + inputWidth, (int)c - 48);
			}
			if (inputArray.TryGetElementAt(position - inputWidth, out char d) && height < (int)d - 48)
			{
				basinCount += CheckBasinSize(position - inputWidth, (int)d - 48);
			}
			return basinCount;
		}
	}
	string Day10()
	{
		string answer = "Day 10: ";
		string[] input = HelperMethods.GetInput(day: 10).Split(Environment.NewLine);

		string openings = "([{<";
		string closings = ")]}>";

		int index = 0;
		long points = 0;
		Stack<char> openingChunks = new();
		foreach (string chunk in input)
		{
            foreach (char c in chunk)
            {
                if (openings.Contains(c))
                {
					openingChunks.Push(c);
                }
                else
                {
					index = openings.IndexOf(openingChunks.Pop());
                    if (closings[index] != c)
                    {
						points += c switch
						{
							')' => 3,
							']' => 57,
							'}' => 1197,
							'>' => 25137,
							_ => throw new Exception("Invalid character found in input"),
						};
						break;
                    }
                }
            }
			openingChunks.Clear();
		}
		answer += $"Part 1: {points} ";
				
		List<long> totalPoints = new();
		foreach (string chunk in input)
		{
			for (int i = 0; i < chunk.Length; i++)
			{
				if (openings.Contains(chunk[i]))
				{
					openingChunks.Push(chunk[i]);
				}
				else
				{
					index = openings.IndexOf(openingChunks.Pop());
					if (closings[index] != chunk[i])
					{
						openingChunks.Clear();
						break;
					}
				}
			}

			points = 0;
            while (openingChunks.Count > 0)
			{			
				index = openings.IndexOf(openingChunks.Pop());

				points *= 5;
				points += closings[index] switch
                {
                    ')' => 1,
                    ']' => 2,
                    '}' => 3,
                    '>' => 4,
                    _ => throw new Exception("Invalid character found in input"),
                };
			}
            if (points > 0)
            {
				totalPoints.Add(points);
            }
		}
		totalPoints.Sort();
		answer += $"Part 2: {totalPoints[totalPoints.Count / 2]}";

		return answer;
	}
	string Day11()
	{
		string answer = "Day 11: ";
		string[] inputArray = HelperMethods.GetInput(day: 11).Split(Environment.NewLine);

		int width = inputArray[0].Length;
		int height = inputArray.Length;

		int[,] input = ParseRawInput(inputArray);

		int stepCount = 100;
		int totalFlashes = 0;
		List<(int, int)> poppedOctos = new();
		for (int s = 0; s < stepCount; s++)
		{
			poppedOctos.Clear();
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					input[i, j]++;
				}
			}
			bool containsPops = true;
			while (containsPops)
			{
				containsPops = input.ForeachElementThatMeetsCriteria(
					criteria: x => x.value > 9 && !poppedOctos.Contains(x.position),
					action: (a, b) => Pop(a, b));
			}
			foreach ((int i, int j) in poppedOctos)
			{
				input[i, j] = 0;
				totalFlashes++;
			}
		}

		answer += $"Part 1: {totalFlashes} ";

		input = ParseRawInput(inputArray);
		for (int s = 0; s < int.MaxValue; s++)
		{
			poppedOctos.Clear();
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					input[i, j]++;
				}
			}
			bool containsPops = true;
			while (containsPops)
			{
				containsPops = input.ForeachElementThatMeetsCriteria(
					criteria: x => x.value > 9 && !poppedOctos.Contains(x.position),
					action: (a, b) => Pop(a, b));
			}
			foreach ((int i, int j) in poppedOctos)
			{
				input[i, j] = 0;
				totalFlashes++;
			}

			if (poppedOctos.Count == width * height)
			{
				answer += $"Part 2: {++s}";
				return answer;
			}
		}
		return answer + "Part 2: N/A";

		void Pop(int a, int b)
		{
			poppedOctos.Add((a, b));
			if (input.TryGetElementAt(a + 1, b, out _)) input[a + 1, b]++;
			if (input.TryGetElementAt(a - 1, b, out _)) input[a - 1, b]++;
			if (input.TryGetElementAt(a, b + 1, out _)) input[a, b + 1]++;
			if (input.TryGetElementAt(a, b - 1, out _)) input[a, b - 1]++;
			if (input.TryGetElementAt(a + 1, b - 1, out _)) input[a + 1, b - 1]++;
			if (input.TryGetElementAt(a - 1, b + 1, out _)) input[a - 1, b + 1]++;
			if (input.TryGetElementAt(a + 1, b + 1, out _)) input[a + 1, b + 1]++;
			if (input.TryGetElementAt(a - 1, b - 1, out _)) input[a - 1, b - 1]++;
		}
		int[,] ParseRawInput(string[] rawInput)
		{
			int position = 0;
			int[,] input = new int[height, width];
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					input[position, j] = (int)char.GetNumericValue(inputArray[i][j]);
				}
				position++;
			}
			return input;
		}
	}
	string Day12()
    {
		string answer = "Day 12: ";
		string[] inputArray = HelperMethods.GetInput(day: 12).Split(Environment.NewLine);
	}
	#endregion
}
finally
{
	Console.ResetColor();
	Console.Clear();
}