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
	public class ZrodloKrewDemona : ResourceVein
    {
        public override Type CropType => typeof(SurowiecKrewDemona);

		[Constructable] 
		public ZrodloKrewDemona() : base( 0x1CF3 )
		{ 
			Hue = 0;
			Name = "Krew demona";
			Stackable = true;			
		}

		public ZrodloKrewDemona( Serial serial ) : base( serial ) 
		{ 
			//m_plantedTime = DateTime.Now;	// ???
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
	
	public class SurowiecKrewDemona : ResourceCrop
    {
        public override Type ReagentType => typeof(DaemonBlood);
		
		[Constructable]
		public SurowiecKrewDemona( int amount ) : base( amount, 0x0E23 )
		{
			Hue = 0;
			Name = "Porcja krwi demona";
			Stackable = true;
		}

		[Constructable]
		public SurowiecKrewDemona() : this( 1 )
		{
		}

		public SurowiecKrewDemona( Serial serial ) : base( serial )
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