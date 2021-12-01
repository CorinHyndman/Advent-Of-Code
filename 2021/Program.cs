using System;
using System.Reflection;
using AOC;

try
{
	HelperMethods.Year = 2021;
	HelperMethods.ExectuingAssembly = Assembly.GetExecutingAssembly();

	DayOne();


	Console.ReadLine();

	void DayOne()
	{
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

		//Console.WriteLine(count);

		count = 0;
		int windowSize = 2;
		int A = input[0] + input[1] + input[2];
		for (int i = 1; i < input.Length - windowSize; i++)
		{
			int B = input[i] + input[i+1] + input[i+2];

			if (B > A)
			{
				count++;
			}
			
			A = B;
		}

		//Console.WriteLine(count);
	}
}
finally
{
	Console.ResetColor();
	Console.Clear();
}