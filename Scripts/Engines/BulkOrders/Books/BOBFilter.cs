using System;

namespace Server.Engines.BulkOrders
{
	public class BOBFilter
	{
		private int m_Type;
		private int m_Quality;
		private int m_Material;
		private int m_Material2;
		private int m_Quantity;

		public bool IsDefault
		{
			get{ return ( m_Type == 0 && m_Quality == 0 && m_Material == 0 && m_Material2 == 0 && m_Quantity == 0 ); }
		}

		public void Clear()
		{
			m_Type = 0;
			m_Quality = 0;
			m_Material = 0;
			m_Material2 = 0;
			m_Quantity = 0;
		}

		public int Type
		{
			get{ return m_Type; }
			set{ m_Type = value; }
		}

		public int Quality
		{
			get{ return m_Quality; }
			set{ m_Quality = value; }
		}

		public int Material
		{
			get{ return m_Material; }
			set
			{
				m_Material = value;

				if (!BOBFilterGump.IsFletcherMaterial(m_Material))
                    m_Material2 = 0;
            }
		}

		public int Material2 {
			get { return m_Material2; }
			set
			{
				m_Material2 = value;

				if (!BOBFilterGump.IsFletcherMaterial(m_Material))
					m_Material = 0;

            }
		}

		public int Quantity
		{
			get{ return m_Quantity; }
			set{ m_Quantity = value; }
		}

		public BOBFilter()
		{
		}

		public BOBFilter( GenericReader reader )
		{
			int version = reader.ReadEncodedInt();

			m_Material2 = 0;

			switch ( version )
			{
				case 3:
				case 2: 
				{
					if(version < 3)
						m_Material2 = reader.ReadEncodedInt();
					goto case 1;
				}
				case 1:
				{
					m_Type = reader.ReadEncodedInt();
					m_Quality = reader.ReadEncodedInt();
					m_Material = reader.ReadEncodedInt();
					m_Quantity = reader.ReadEncodedInt();

					break;
				}
			}
		}

		public void Serialize( GenericWriter writer )
		{
			if ( IsDefault )
			{
				writer.WriteEncodedInt( 0 ); // version
			}
			else
			{
				writer.WriteEncodedInt( 1 ); // version

				//writer.WriteEncodedInt(m_Material2);

				writer.WriteEncodedInt( m_Type );
				writer.WriteEncodedInt( m_Quality );
				writer.WriteEncodedInt( m_Material );
				writer.WriteEncodedInt( m_Quantity );
			}
		}
	}
}