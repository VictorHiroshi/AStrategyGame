using System;

public class Coordinate
{

	private int x { get;}
	private int y { get;}

	public Coordinate (int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override bool Equals(object obj)
	{
		if(obj == null)
		{
			return false;
		}

		Coordinate other = obj as Coordinate;

		if((System.Object)other == null)
		{
			return false;
		}

		return ((x == other.x) && (y == other.y));
	}

	public bool Equals(Coordinate other)
	{
		if((object)other == null)
		{
			return false;
		}

		return ((x == other.x) && (y == other.y));
	}
}


