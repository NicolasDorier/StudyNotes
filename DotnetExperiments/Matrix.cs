
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Markup;
using Xunit.Sdk;

namespace StudyNotes.Signals
{
	public class Matrix
	{
		public static Matrix<T> Create<T>(T[,] values) where T : System.Numerics.INumberBase<T> => new Matrix<T>(values);

	}
	public class Matrix<T> where T : System.Numerics.INumberBase<T>
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
			if (a.Cols != b.Rows)
				throw new InvalidOperationException($"Matrix not compatible (a.Cols != b.Rows)={a.Cols} != {b.Rows}");
			//return MultiplyPairSumation(a, b);
			return MultiplyDotProduct(a, b);
		}
		public Matrix<T> Combine(Matrix<T> b, Func<T,T,T> op)
		{
			var a = this;
			if (a.Cols != b.Cols)
				throw new InvalidOperationException($"Matrix not compatible (a.Cols != b.Cols)");
			if (a.Rows != b.Rows)
				throw new InvalidOperationException($"Matrix not compatible (a.Rows != b.Rows)");
			var v = new T[a.Rows, a.Cols];
			for (int r = 0; r < a.Rows; r++)
			{
				for (int c = 0; c < a.Cols; c++)
				{
					v[r, c] = op(a[r, c], b[r, c]);
				}
			}
			return new Matrix<T>(v);
		}
		public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
		{
			return a.Combine(b, (v1, v2) => v1 + v2);
		}

		private static Matrix<T> MultiplyDotProduct(Matrix<T> a, Matrix<T> b)
		{
			var result = new Matrix<T>(new T[a.Rows, b.Cols]);
			for (int r = 0; r < result.Rows; r++)
			{
				for (int c = 0; c < result.Cols; c++)
				{
					var ra = a.SelectSingleRow(r);
					var cb = b.SelectSingleColumn(c);
					result.Values[r, c] = ra.Transpose().AsVector().DotProduct(cb.AsVector());
				}
			}
			return result;
		}

		public Matrix<T> Transpose()
		{
			var res = new T[Cols, Rows];
			for (int r = 0; r < Rows; r++)
			{
				for (int c = 0; c < Cols; c++)
				{
					res[c, r] = Values[r, c];
				}
			}
			return new Matrix<T>(res);
		}

		public Vector<T> AsVector()
		{
			if (Cols != 1)
				throw new InvalidOperationException("Cols != 1");
			var v = new T[Rows];
			for (int i = 0; i < Rows; i++)
			{
				v[i] = Values[i, 0];
			}
			return new Vector<T>(v);
		}

		public Matrix<T> SelectSingleRow(int r)
		{
			var v = new T[1, Cols];
			for (int c = 0; c < Cols; c++)
			{
				v[0, c] = Values[r, c];
			}
			return new Matrix<T>(v);
		}

		public Matrix<T> SelectSingleColumn(int c)
		{
			var v = new T[Rows, 1];
			for (int r = 0; r < Rows; r++)
			{
				v[r, 0] = Values[r, c];
			}
			return new Matrix<T>(v);
		}

		private static Matrix<T> MultiplyPairSumation(Matrix<T> a, Matrix<T> b)
		{
			var result = new Matrix<T>(new T[a.Rows, b.Cols]);
			for (int r = 0; r < result.Rows; r++)
			{
				for (int c = 0; c < result.Cols; c++)
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
					v.Append(Values[r, c]);
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

		public Matrix<T2> Modify<T2>(Func<T, T2> modif) where T2 : System.Numerics.INumberBase<T2>
		{
			var v = new T2[Rows, Cols];
			for (int r = 0; r < Rows; r++)
			{
				for (int c = 0; c < Cols; c++)
				{
					v[r, c] = modif(Values[r, c]);
				}
			}
			return new Matrix<T2>(v);
		}
	}
}
