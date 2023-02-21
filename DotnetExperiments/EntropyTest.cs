using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace StudyNotes
{
	public class EntropyTest
	{
		public ITestOutputHelper Log { get; }

		public EntropyTest(ITestOutputHelper log)
		{
			Log = log;
		}

		[Fact]
		public void Test()
		{
			TestEntropy("aaaaa");
			TestEntropy("Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
			TestEntropy("All bytes", Enumerable.Range(0, 256).Select(b => (byte)b).ToArray());
			CalculateDataEntropy(new byte[] { 1, 2, 3 });
		}

		private void TestEntropy(string sentence, byte[]? data = null)
		{
			data ??= new UTF8Encoding(false, true).GetBytes(sentence);
			var entropy = CalculateDataEntropy(data);
			var expected = MathNet.Numerics.Statistics.Statistics.Entropy(data.Select(d => (double)d));
			Log.WriteLine($"{sentence}");
			Log.WriteLine($"Actual: {entropy}, Expected:{expected}");
		}

		public double CalculateDataEntropy(byte[] data)
		{
			
			var bins = new double[256];
			for (int i = 0; i < data.Length; i++)
			{
				bins[data[i]]++;
			}

			var p = Vector<double>.Build.Dense(bins.ToArray());
			// l1 normalization...
			p = p.Normalize(1.0);
			// is the same as dividing by the count: p = p * (1.0 / data.Length);

			// Now p is all the probabilities.
			for (int i = 0; i < p.Count; i++)
			{
				bins[i] = p[i] == 0 ? 0 : -Math.Log(p[i], 2.0);
			}
			// Now bins is all the informations (in bytes)
			var inf = Vector<double>.Build.Dense(bins);
			var entropyByte = p.DotProduct(inf);

			// Finally, we need to convert the information bytes into bit.
			return entropyByte;
		}
	}
}
