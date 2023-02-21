using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes.Signals
{
	public class Vector
	{
		public static Vector<T> Create<T>(ReadOnlySpan<T> values) where T : System.Numerics.INumberBase<T> => new Vector<T>(values);
		public static Vector<T> Create<T>(T[] values) where T : System.Numerics.INumberBase<T> => new Vector<T>(values);
	}
	public class Vector<T> where T : System.Numerics.INumberBase<T>
	{
		public readonly T[] Values;
		public readonly int Length;
		public Vector(T[] values)
		{
			Values = values;
			Length = values.Length;
		}
		public Vector(ReadOnlySpan<T> values)
		{
			Values = values.ToArray();
			Length = values.Length;
		}

		public T this[int index]
		{
			get => Values[index];
		}

		public override bool Equals(object? obj)
		{
			if (obj is Vector<T> b && Length == b.Length)
			{
				return ((IStructuralEquatable)Values).Equals(b.Values, EqualityComparer<T>.Default);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)Values).GetHashCode(EqualityComparer<T>.Default);
		}
		public override string ToString()
		{
			StringBuilder v = new StringBuilder();
			for (int r = 0; r < Length; r++)
			{
				v.Append('|');
				v.Append(Values[r]);
				v.Append('|');
				v.AppendLine();
			}
			return v.ToString();
		}

		public T DotProduct(Vector<T> vector)
		{
			if (vector.Length != Length)
				throw new InvalidOperationException("vector.Length != Length");
			T v = T.Zero;
			for (int i = 0; i < Length; i++)
			{
				v += Values[i] * vector[i];
			}
			return v;
		}

		public Matrix<T> AsMatrix()
		{
			var v = new T[Values.Length, 1];
			for (int i = 0; i < Values.Length; i++)
			{
				v[i, 0] = Values[i];
			}
			return new Matrix<T>(v);
		}
	}
}
