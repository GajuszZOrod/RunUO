﻿using System;
using Server.Network;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Targets;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Mobiles;

namespace Server.Items.Crops
{
	// TODO: mozliwe jest uzycie umiejetnosci TworzenieLukow, zatem mozna zwiekszyc progi umozliwiajace zbieranie
	public class ZrodloKonopia : WeedPlantZbieractwo
	{
		public static SkillName[] cannabisSkills = new SkillName[] { SkillName.Zielarstwo, SkillName.Fletching };
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack( new SurowiecKonopia(count) ); }
        public override SkillName[] SkillsRequired { get{ return cannabisSkills; } }
        //public override int CropAmount { get{ return 5; } }

		public override bool GivesSeed{ get{ return false; } }

		[Constructable] 
		public ZrodloKonopia() : base( 0x0CC3 ) //3271
		{ 
			//Hue = 263;
			Name = "Krzak konopi"; // 1032612
			Stackable = true;
		}

		public ZrodloKonopia( Serial serial ) : base( serial ) 
		{ 
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
	
	public class SurowiecKonopia : WeedCropZbieractwo
	{
        public override int AmountOfReagent(double skill) { return 12; }
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack( new CannabisFiber(count) ); }
		public override SkillName[] SkillsRequired { get{ return ZrodloKonopia.cannabisSkills; } }

		[Constructable]
		public SurowiecKonopia( int amount ) : base( amount, 0x0C5F )
		{
			//Hue = 0;
			Name = "Lodyga konopi"; // 1032615
			Stackable = true;
		}

		[Constructable]
		public SurowiecKonopia() : this( 1 )
		{
		}

		public SurowiecKonopia( Serial serial ) : base( serial )
		{
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

    public class CannabisFiber : Item
    {
        [Constructable]
		public CannabisFiber( int amount ) : base( 3166 )
		{
            Stackable = true;
            Amount = amount;
			//Hue = 0;
			Name = "Konopne wlokno"; // 1032616
            Weight = 0.15;
		}

        [Constructable]
		public CannabisFiber() : this(1)
		{
        }

		public CannabisFiber( Serial serial ) : base( serial )
		{
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