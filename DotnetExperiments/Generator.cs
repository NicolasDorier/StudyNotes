using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes.Signals
{
	public class Generator
	{
		public Signal<double> Cos(int frequency)
		{
			return new Signal<double>(t => Math.Cos(Math.Tau * frequency * t));
		}
		public Signal<double> Sin(int frequency)
		{
			return new Signal<double>(t => Math.Sin(Math.Tau * frequency * t));
		}
		public Signal<Complex> Frequency(int frequency)
		{
			return new Signal<Complex>(t => Complex.FromPolarCoordinates(1.0, Math.Tau * frequency * t));
		}
		public Signal<Complex> Frequency(double frequency)
		{
			return new Signal<Complex>(t => Complex.FromPolarCoordinates(1.0, Math.Tau * frequency * t));
		}

		public Signal<double> Linear(int slope)
		{
			return new Signal<double>(t => t * slope);
		}
	}
	public class SignalFamily<T, T2> where T2 : System.Numerics.INumberBase<T2>
									 where T : System.Numerics.INumberBase<T>
	{
		private readonly Signal<T> generator;
		private Func<T, Signal<T2>> template;

		public SignalFamily(Signal<T> generator, Func<T, Signal<T2>> template)
		{
			this.generator = generator;
			this.template = template;
		}
		public Matrix<T2> ToMatrix(
			double pStart, double pEnd, int rowCount,
			double gStart, double gEnd, int colCount)
		{
			var pSteps = (pEnd - pStart) / rowCount;
			var gSteps = (gEnd - gStart) / colCount;
			var v = new T2[colCount, rowCount];
			for (int r = 0; r < colCount; r++)
			{
				var rValue = generator.Func(pStart + r * pSteps);
				var f = template(rValue);
				for (int c = 0; c < rowCount; c++)
				{
					var cValue = gStart + c * gSteps;
					v[r, c] = f.Func(cValue);
				}
			}
			return new Matrix<T2>(v);
		}
	}
	public class Signal<T> where T : System.Numerics.INumberBase<T>
	{
		public Signal(Func<double, T> func)
		{
			Func = func;
		}
		public Func<double, T> Func { get; }

		public Signal<T> Lag(double offset)
		{
			return new Signal<T>(t => Func(t + offset));
		}
		public SignalFamily<T,T2> Select<T2>(Func<T, Signal<T2>> gen) where T2 : System.Numerics.INumberBase<T2>
		{
			return new SignalFamily<T,T2>(this, gen);
		}
		public T[] Sample(double start, double end, int sampleCount)
		{
			var step = (end - start) / sampleCount;
			T[] values = new T[sampleCount];
			for (int i = 0; i < values.Length; i++)
			{
				values[i] = Func(start + step * i);
			}
			return values;
		}
	}
}
