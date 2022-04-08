using System;
using Server;

namespace Server.Items
{
	public class OrcChieftainHelm : OrcHelm
	{
        public override int LabelNumber { get { return 1065844; } } // Helm Kapitana Orkow
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance{ get{ return 23; } }
		public override int BaseFireResistance{ get{ return 1; } }
		public override int BaseColdResistance{ get{ return 23; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 5; } }


		[Constructable]
		public OrcChieftainHelm()
		{
			Hue = 0x2a3;

			Attributes.Luck = 100;
			Attributes.RegenHits = 3;

			if( Utility.RandomBool() )
				Attributes.BonusHits = 30;
			else
				Attributes.AttackChance = 30;
		}

		public OrcChieftainHelm( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if (version < 1 && Hue == 0x3f) /* Pigmented? */
			{
				Hue = 0x2a3;
			}
		}
	}
}
