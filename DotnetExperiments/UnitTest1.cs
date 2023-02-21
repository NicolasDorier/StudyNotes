using System.Collections;
using System.Numerics;
using System.Reflection.Emit;
using Xunit.Abstractions;

namespace StudyNotes.Signals;

public class UnitTest1
{
	Generator Generator = new Generator();

	public ITestOutputHelper Log { get; }

	public UnitTest1(ITestOutputHelper log)
	{
		Log = log;
	}
	[Fact]
	public void CanMultiply()
	{
		var a = Matrix.Create(new int[,]
		{
			{ 2, 6, 3 },
			{ 7, 1, 9 },
			{ 9, 8, 6 }
		});
		var b = Matrix.Create(new int[,]
		{
			{ 0, 3, 6 },
			{ 1, 0, 6 },
			{ 3, 9, 1 }
		});
		var expected = Matrix.Create(new int[,]
		{
			{ 15, 33, 51 },
			{ 28, 102, 57 },
			{ 26, 81, 108 }
		});
		Assert.Equal(expected, a * b);

		b = Matrix.Create(new int[,]
		{
			{ 0, 3 },
			{ 1, 0 },
			{ 3, 9 }
		});
		expected = Matrix.Create(new int[,]
		{
			{ 15, 33 },
			{ 28, 102 },
			{ 26, 81 }
		});
		Assert.Equal(expected, a * b);

		int samples = 200;
		var signal = Vector.Create<double>(Generator.Cos(1).Sample(-1, 1, samples));


		var te =
			Generator.Linear(1)
			 .Select(t => Generator.Frequency((int)t))
			 .ToMatrix(0, 100, samples, -1, 1, 100);

		var res = te * signal.AsMatrix().Modify(c => new Complex(c, 0.0));

		var tinv = Generator.Linear(1)
			 .Select(t => Generator.Frequency(-(int)t))
			 .ToMatrix(0, 100, samples, -1, 1, 100);

		var restored = (tinv * res).Modify(o => o.Real);

		Log.WriteLine(restored.ToString());
		//var inv = transform.Modify(a => 1 / a);
		//var signal2 = (inv * result).AsVector();

		//Assert.Equal(signal, signal2);
	}

	[Fact]
	public void TestStuff()
	{
		var a = Matrix.Create(new double[,]
		{
			{ 2 , 3, 5}
		});
		var b = Matrix.Create(new double[,]
		{
			{ 5 },
			{ 1 },
			{ 3 }
		});
		var res = a * b;
		Log.WriteLine(res.ToString());

		var ainv = Matrix.Create(new double[,]
		{
			{ 1.0/2.0 },
			{ 1.0/3.0 },
			{ 1.0/5.0 }
		});
		var res2 = ainv * res;
		Log.WriteLine(res2.ToString());

		var s1 = Vector.Create(Generator.Cos(1).Sample(0.0, 1, 1000));
		var s2 = Vector.Create(Generator.Sin(1).Sample(0.0, 1, 1000));
		Log.WriteLine(s1.DotProduct(s2).ToString());
	}

	[Fact]
	public void Totew()
	{
		double r = 1.0;
		var f0 = Generator.Frequency(0 / r);
		var f1 = Generator.Frequency(1 / r);
		var f2 = Generator.Frequency(2 / r);
		var f3 = Generator.Frequency(3 / r);
		var signal = Matrix.Create(new float[,]
		{
			{ 1.0f, 3.0f, 5.0f, -2.0f },
			{ 11.0f, 32.0f, 53.0f, -24.0f }
		}).Transpose();

		var m1 = Matrix.Create(new Complex[,]
		{
			{ f0.Func(0.0), f0.Func(0.25), f0.Func(0.5), f0.Func(0.75) },
			{ f1.Func(0.0), f1.Func(0.25), f1.Func(0.5), f1.Func(0.75) },
			{ f2.Func(0.0), f2.Func(0.25), f2.Func(0.5), f2.Func(0.75) },
			{ f3.Func(0.0), f3.Func(0.25), f3.Func(0.5), f3.Func(0.75) },
		});
		Log.WriteLine("m1");
		Log.WriteLine(m1.ToString());
		f0 = Generator.Frequency(-0);
		f1 = Generator.Frequency(-1 / r);
		f2 = Generator.Frequency(-2 / r);
		f3 = Generator.Frequency(-3 / r);
		var m2 = Matrix.Create(new Complex[,]
		{
			{ f0.Func(0.0), f0.Func(0.25), f0.Func(0.5), f0.Func(0.75) },
			{ f1.Func(0.0), f1.Func(0.25), f1.Func(0.5), f1.Func(0.75) },
			{ f2.Func(0.0), f2.Func(0.25), f2.Func(0.5), f2.Func(0.75) },
			{ f3.Func(0.0), f3.Func(0.25), f3.Func(0.5), f3.Func(0.75) },
		});
		Log.WriteLine("m2");
		Log.WriteLine(m2.ToString());
		Log.WriteLine((m1 * m2.Modify(v => v / 4.0)).Modify(v => v.Magnitude < 0.001 ? 0 : v.Real).ToString());
		var result = (m1 * (m2 * signal.Modify(c => C((double)c, 0.0))));
		var result2 = result.Modify(c => c.Real / 4);
		Log.WriteLine(result2.ToString());
	}

	[Fact]
	public void CanDotProduct()
	{
		var a = Vector.Create(new[]
		{
			1,
			2,
			3
		});
		var b = Vector.Create(new[]
		{
			4,
			5,
			6
		});
		Assert.Equal(32, a.DotProduct(b));

		var ac = Vector.Create(new[]
		{
			C(1, 0),
			C(2, 0),
			C(3, 0)
		});
		var bc = Vector.Create(new[]
		{
			C(4, 0),
			C(5, 0),
			C(6, 0)
		});
		Assert.Equal(C(32, 0), ac.DotProduct(bc));

		ac = Vector.Create(new[]
		{
			C(1, 1),
			C(2, 0),
			C(3, 0)
		});
		bc = Vector.Create(new[]
		{
			C(4, 0),
			C(5, 0),
			C(6, 0)
		});
		Assert.Equal(C(32, 4), ac.DotProduct(bc));
	}

	private System.Numerics.Complex C(double r, double i)
	{
		return new System.Numerics.Complex(r, i);
	}
}