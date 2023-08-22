using System;

namespace AFSInterview
{
	[Flags]
	public enum UnitAttributes
	{
		None = 0,
		Light = 1 << 0,
		Armored = 1 << 1,
		Mechanical = 1 << 2,
	}
}