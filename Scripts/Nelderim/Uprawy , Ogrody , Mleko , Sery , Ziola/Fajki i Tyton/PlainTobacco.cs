using Server.Items;
using Server;

public class PlainTobacco : BaseTobacco
{

	[Constructable]
	public PlainTobacco() : this(1)
	{
	}

	[Constructable]
	public PlainTobacco(int amount) : base(amount)
	{
		Name = "tyton pospolity";
		Hue = 2129;
	}

	public PlainTobacco(Serial serial) : base(serial)
	{
	}

	public override void Serialize(GenericWriter writer)
	{
		base.Serialize(writer);

		writer.Write((int)0); // version
	}

	public override void Deserialize(GenericReader reader)
	{
		base.Deserialize(reader);

		int version = reader.ReadInt();
	}
}
