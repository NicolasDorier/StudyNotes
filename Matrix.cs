
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace StudyNotes
{
	public class Matrix
	{
		public static Matrix<T> Create<T>(T[,] values) where T : INumber<T> => new Matrix<T>(values);

	}
	public class Matrix<T> where T : INumber<T>
	{
		public readonly T[,] Values;
		public readonly int Rows;
		public readonly int Cols;
		public Matrix(T[,] values)
		{
			Values = values;
			Rows = values.GetLength(0);
			Cols = values.GetLength(1);
		}
		public T this[int row, int col]
		{
			get => Values[row, col];
		}

		public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
		{
			var result = new Matrix<T>(new T[a.Cols, b.Rows]);
			for (int r = 0; r < a.Cols; r++)
			{
				
				for (int c = 0; c < b.Rows; c++)
				{
					T v = T.Zero;
					for (int ac = 0; ac < a.Cols; ac++)
					{
						v += a[r, ac] * b[ac, c];
					}
					result.Values[r, c] = v;
				}
			}
			return result;
		}

		public override string ToString()
		{
			StringBuilder v = new StringBuilder();
			for (int r = 0; r < Rows; r++)
			{
				v.Append('|');
				for (int c = 0; c < Cols; c++)
				{
					v.Append(Values[r,c]);
					if (c != Cols - 1)
						v.Append(", ");
				}
				v.Append('|');
				v.AppendLine();
			}
			return v.ToString();
		}

		public override bool Equals(object? obj)
		{
			if (obj is Matrix<T> b && Rows == b.Rows && Cols == b.Cols)
			{
				for (int r = 0; r < Rows; r++)
				{
					for (int c = 0; c < Cols; c++)
					{
						if (Values[r, c] != b[r, c])
							return false;
					}
				}
				return true;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)Values).GetHashCode(EqualityComparer<T>.Default);
		}
	}
}
