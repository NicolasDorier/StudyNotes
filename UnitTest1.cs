using System.Collections;
using System.Numerics;

namespace StudyNotes;

public class UnitTest1
{
	Generator Generator = new Generator();
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
	}
}