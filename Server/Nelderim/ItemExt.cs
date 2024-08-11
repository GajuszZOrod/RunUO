namespace Server
{
	public partial class Item
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Stealable
		{
			get { return StealableExtension.IsStealable(this); }
			set { StealableExtension.SetStealable(this, value); }
		}
	}
}