using Nelderim;
using System.Collections.Generic;

namespace Server
{
	class StealableExtension : NExtension<StealableExtensiontInfo>
	{
		public static bool IsStealable(Item item)
		{
			return Get(StealableExtensiontInfo.Singleton.GetInstance()).IsStealable(item);
		}
		public static void SetStealable(Item item, bool stealable)
		{
			Get(StealableExtensiontInfo.Singleton.GetInstance()).SetStealable(item, stealable);
		}

		private static string ModuleName = "Stealables";

		public static void Initialize()
		{
			EventSink.WorldSave += new WorldSaveEventHandler(Save);
			Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}
	}

	class StealableExtensiontInfo : NExtensionInfo
	{
		public class Singleton : Item
		{
			private static Singleton m_Instance;
			public static Singleton GetInstance()
			{
				if (m_Instance == null)
					m_Instance = new Singleton();
				return m_Instance;
			}
			private Singleton()
			{
				Map = Map.Internal;
				Location = new Point3D(1, 1, 1);
			}

			public Singleton(Serial serial) : base(serial)
			{
			}

			public override void Serialize(GenericWriter writer)
			{
				writer.Write((int)0); // version
			}

			public override void Deserialize(GenericReader reader)
			{
				int version = reader.ReadInt();

				m_Instance = this;
			}
		}

		private HashSet<Item> m_Stealables = new HashSet<Item>(); // list of all stealable items

		public bool IsStealable(Item item)
		{
			return item != null && m_Stealables != null && m_Stealables.Contains(item);
		}

		public void SetStealable(Item item, bool stealable)
		{
			if (item == null)
				return;

			if (m_Stealables == null)
				m_Stealables = new HashSet<Item>();

			if (stealable)
				m_Stealables.Add(item);
			else
				m_Stealables.Remove(item);
		}

		public StealableExtensiontInfo()
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write((int)0); // version

			writer.Write((int)m_Stealables.Count);
			foreach (Item item in m_Stealables)
				writer.Write((Item)item);

			writer.Write((Item)Singleton.GetInstance());
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();

			int count = reader.ReadInt();
			for (int i = 0; i < count; ++i)
				m_Stealables.Add(reader.ReadItem());
		}
	}
}