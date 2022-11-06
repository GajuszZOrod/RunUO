using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class   treacherousCrystal : Item
	{
		public override string DefaultName
		{
			get { return "Zdradziecki krysztal"; }
		}

		[Constructable]
		public  treacherousCrystal() : base( 0x1F19 )
		{
			Weight = 1.0;
			Hue = 0x494;
		}

		public  treacherousCrystal( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				return;
			}

			double NecroSkill = from.Skills[SkillName.Necromancy].Value;

			if ( NecroSkill < 40.0 )
			{
				from.SendMessage( "Musisz mieć przynajkmniej 40 umeijętności nekromancji, by stworzyć szkieleta." );
				return;
			}

			double scalar;

			if ( NecroSkill >= 100.0 )
				scalar = 2.3;
			else if ( NecroSkill >= 90.0 )
				scalar = 2.0;
			else if ( NecroSkill >= 80.0 )
				scalar = 1.8;
			else if ( NecroSkill >= 70.0 )
				scalar = 1.6;
			else
				scalar = 1.0;

			Container pack = from.Backpack;

			if ( pack == null )
				return;

			int res = pack.ConsumeTotal(
				new Type[]
				{
					typeof( SkeletonMageTorso ),
					typeof( SkeletonLegs )
				},
				new int[]
				{
					1,
					1
				} );

			switch ( res )
			{
				case 0:
				{
					from.SendMessage( "Musisz mieć Tułów szkieleta maga." );
					break;
				}
				case 1:
				{
					from.SendMessage( "Musisz mieć nogi szkieleta." );
					break;
				}
				default:
				{
					SkeletalMagi g = new SkeletalMagi( true, scalar );

					if ( g.SetControlMaster( from ) )
					{
						Delete();

						g.MoveToWorld( from.Location, from.Map );
						from.PlaySound( 0x241 );
					}

					break;
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}