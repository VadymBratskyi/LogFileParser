using System;
using System.Collections.Generic;

namespace NeuraNetworks
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			int[] arrInt = new int[5] {0,1,2,3,4};

			var rr = new List<int>(arrInt);

			rr.ForEach(r => Console.WriteLine(r));

			Console.ReadKey();
		}
	}
}
