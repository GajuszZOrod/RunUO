using System;
using Server.Items;

namespace Server.Mobiles
{
	public class DefecationTimer : Timer
	{
		public static TimeSpan DefaultDefecationInterval => TimeSpan.FromMinutes(4);

		private Mobile m_From;
		private bool m_smallDung;

		public DefecationTimer(Mobile m) : this(m, false)
		{
		}

		public DefecationTimer(Mobile m, bool smallDung) : this(m, smallDung, DefaultDefecationInterval)
		{
		}

		public DefecationTimer(Mobile from, bool smallDung, TimeSpan interval) : base(TimeSpan.FromSeconds(Utility.Random(0, 30)), interval)
		{
			m_From = from;
			m_smallDung = smallDung;
		}

		protected override void OnTick()
		{
			if (m_From.Deleted)
			{
				Stop();
				return;
			}

			if (m_From.Map != null && m_From.Map != Map.Internal)
			{
				DungPile spawn = new DungPile(m_smallDung);
				spawn.MoveToWorld(m_From.Location, m_From.Map);
			}
		}
	}
}