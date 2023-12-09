#region References

using System.Collections.Generic;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class SzemranyTyp : NBaseTalkingNPC
	{
		private static readonly Dictionary<Race, List<Action>> _Actions = new Dictionary<Race, List<Action>>
		{
			{ Race.DefaultRace, new List<Action> { m => m.Emote("*Rozgląda się uważnie w koło*") } },
			{
				Tamael.Instance, new List<Action>
				{
					m => m.Emote("*Podrzuca broń w dłoni*"),
					m => m.Emote("*Chowa nerwowo za pas drobny pakunek*"),
					m => m.Say("Słyszałeś że Malaer zabił Elfa w zaułku, a potem zrobił mu to co Soteriosowi?"),
					m => m.Say("Nie ma Cie... Ale już! *Odgonił ręką*"),
					m => m.Say("Dawniej to nas namiestnik dymał, a po śmierci spokój był..."),
					m => m.Emote("*Szura powoli nogą po ziemi rozglądając się w koło*"),
					m =>
					{
						m.Say("Jak sie nie ma co sie pragnie... to sie kradnie co popadnie...");
						m.Emote("*Szepcze coś pod nosem*");
					},
					m => m.Say("Zajebać Ci?"),
					m =>
					{
						m.Say("Tyle tej straży... na to maja pieniądze...");
						m.Emote("*Wzdycha ciężko*");
					},
					m => m.Say("Won mi stąd..."),
					m =>
					{
						m.Say("Pilnuj sie... bywa tu całkiem...Niebezpiecznie");
						m.Emote("*Uśmiecha się szyderczo*");
					},
					m =>
					{
						m.Say("Nic nie widziałeś... rozumiesz?");
						m.Emote("*Spluwa na ziemie*");
					},
					m => m.Say("Oni tak zawsze mówią, a później nic nie robią."),
					m => m.Say("Jarle precz, knypki precz, w dłonie miecz, wrogów siecz!"),
					m => m.Say("Słyszałem, że jak chcesz Elfa, to zapisz się na listę u nekromantów"),
				}
			},
			{
				Jarling.Instance, new List<Action>
				{
					m => m.Say("Słyszałeś że Malaer zabił Elfa w zaułku, a potem zrobił mu to co Soteriosowi?"),
					m => m.Say("Dawniej to nas namiestnik dymał, a po śmierci spokój był..."),
					m => m.Say("Jak nie kupujesz towaru to spierdalaj..."),
					m => m.Emote("*Rozgląda się nerwowo*"),
					m => m.Emote("*Gwiżdże cicho pod nosem*"),
					m => m.Say("Nie zrozum mnie źle, lubię cię i szanuję. Ale nie mogę ci pomóc w tej misji. To sprawa między tobą a tym magiem. Nie chcę się wtrącać w wasze kumoterstwo."),
					m => m.Say("Czy my sie aby nie znamy...? Ty sukinsynu..."),
					m => m.Say("To twoja ostatnia szansa, żeby odejść stąd o własnych siłach..."),
					m => m.Emote("*Powolnym i spokojnym ruchem chowa coś za pazuchę*"),
					m => m.Say("Pachnie tu gównem... nie to co na Północy..."),
					m => m.Say("Masz jakiś problem?!"),
					m => m.Say("Słyszałem, że jak chcesz Elfa, to zapisz się na listę u nekromantów"),
					m => m.Say("Pilnuj swojego nosa... Dobrze radzę."),
				}
			},
			{
				Krasnolud.Instance, new List<Action>
				{
					m => m.Say("Rozpierdole Ci łeb jak czaszkę smoka!"),
					m =>
					{
						m.Emote("*Zerka na swoją broń*");
						m.Say("Krasnoludzka robota to nie jest, ale i tak rozjebie Ci tym łeb");
					},
					m => m.Say("Nie chce Cie tu widzieć... Won!"),
					m =>
					{
						m.Say("Dawniej to nas namiestnik dymał, a po śmierci spokój był...");
						m.Emote("*Splunął pod nogi*");
					},
					m => m.Say("Słyszałeś że Malaer zabił Elfa w zaułku, a potem zrobił mu to co Soteriosowi?"),
					m => m.Say("Krasnal?! Który to powiedział?! Stólić pyski bo pozabijam!"),
					m => m.Say("Pan stworzył Krasnoludy z ognia i ziemi, patrząc na Ciebie budulcem było gówno..."),
					m => m.Say("Dawniej to przynajmniej mogłeś mieć nadzieję na Elfkę w łóżku, a jak kochać się ze szkieletem!?"),
					m => m.Emote("*Nuci po cichu melodię*"),
					m => m.Say("Przywalę Ci w mordę!"),
					m => m.Say("A niech mnie, myślałem, że nie żyjesz... Szkoda..."),
					m => m.Say("Słyszałem, że jak chcesz Elfa, to zapisz się na listę u nekromantów"),
					m => m.Say("Prosisz się o połamaną czaszkę!"),
					m => m.Say("Sam się kurwa pchasz na nóż, spierdalaj..."),
				}
			}
		};

		protected override Dictionary<Race, List<Action>> NpcActions
		{
			get { return _Actions; }
		}

		[Constructable]
		public SzemranyTyp(int gender) : base("- Szemrany Typ")
		{
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			if (Utility.RandomBool())
			{
				EquipItem(new Cloak(GetRandomHue()));
			}

			EquipItem(Loot.RandomWeapon());
		}

		public SzemranyTyp(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
