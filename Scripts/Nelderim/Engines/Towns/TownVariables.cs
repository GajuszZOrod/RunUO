﻿using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Nelderim.Towns
{
    public enum TownResourcesGumpPage
    {
        Information,
        Resources,
        Citizens,
        TownDevelopment,
        Building,
        Maintance,
        BuildingOnHold,
        BuildingOngoing,
        BuildingWorking
    }

    public enum TownResourcesGumpSubpages
    {
        None,
        Citizens,
        ToplistCitizens,
        OldestCitizens,
        ConsellourCitizens,
        RemoveCitizen,
        CitizenDetails,
        BuildingList
    }

    public enum TownMaintananceGumpSubpages
    {
        None,
        Budowanie,
        Zawieszone,
        Dziala,
        Podatki,
        Armia,
        Ekonomia,
        Dyplomacja,
        WydawaniePoswiecenia
    }

    public enum TownResourceType
    {
        Zloto    = 0,
        Deski    = 1,
        Sztaby   = 2,
        Skora    = 3,
        Material = 4,
        Kosci    = 5,
        Kamienie = 6,
        Piasek   = 7,
        Klejnoty = 8,
        Ziola    = 9,
        Zbroje   = 10,
        Bronie   = 11,
        Invalid  = 99
    }

    // Priority top-down
    public enum TownStatus
    { 
        Leader,
        Counsellor,
        Citizen,
        NPC, 
        Vendor,        
        Guard,
        None
    }

    public enum TownCounsellor
    {
        Prime,
        Army,
        Diplomacy,
        Economy,
        Architecture,
        None
    }

    // Nowe miasto moze byc dodane na koncu listy, musi jednak zawierac poprawny kolejny numer, 
    public enum Towns
    {
        None = 0,
        Noamuth_Quortek = 4,    // czyli Wioska Drowów
        Tasandora = 1,
        Garlan = 2,
        Twierdza = 3
    }

    public class TownResource
    {
        private TownResourceType m_resourceType;
        public TownResourceType GetType()
        {
            return m_resourceType;
        }

        public TownResource(TownResourceType nType, int nAmount, int nMaxAmount, int nDailyChange)
        {
            m_resourceType = nType;
            Amount = nAmount;
            MaxAmount = nMaxAmount;
            DailyChange = nDailyChange;
        }

        private int m_amount = 0;
        public int Amount
        {
            get { return m_amount; }
            set { m_amount = value; }
        }
        private int m_maxAmount = 0;
        public int MaxAmount
        {
            get { return m_maxAmount; }
            set { m_maxAmount = value; }
        }
        private int m_dailyChange = 0;
        public int DailyChange
        {
            get { return m_dailyChange; }
            set { m_dailyChange = value; }
        }
    }

    public class TownResources
    {
        #region Typy
        private static List<Type> m_klejnoty = new List<Type>
        {
            typeof( Amber ), typeof( Amethyst ) , typeof( Citrine ) ,
            typeof( Diamond ) , typeof( Emerald ) , typeof( Ruby ) , typeof( Sapphire ),
            typeof( StarSapphire ), typeof( Tourmaline ), typeof( BaseJewel ), typeof( ShimmeringCrystals ), typeof ( CrystallineFragments ), typeof( ObsidianStone )
        };

        private static List<Type> m_ziola = new List<Type>
        {
            typeof( SpidersSilk ), typeof( SulfurousAsh ) , typeof( Nightshade ) ,
            typeof( MandrakeRoot ) , typeof( Ginseng ) , typeof( Garlic ) , typeof( Bloodmoss ),
            typeof( BlackPearl ), typeof( BatWing ), typeof( GraveDust ), typeof( DaemonBlood ),
            typeof( NoxCrystal ), typeof( PigIron ), typeof( SpringWater ), typeof( DestroyingAngel ), typeof( PetrafiedWood ), typeof( TaintedMushroom ), typeof( HornOfTheDreadhorn )
        };
        #endregion

        List<TownResource> m_resources;
        public List<TownResource> Resources
        {
            get { return m_resources; }
            set { m_resources = value; }
        }

        public TownResources()
        {
            m_resources = new List<TownResource>();
        }

        public TownResourceType CheckResourceType(Item res, out int amount)
        {
            TownResourceType resourceType = TownResourceType.Invalid;
            Type resType = res.GetType();
            amount = 1;

            /* Dla poszczegolnych typow mozna zamienic w amount = res.Amount * 1; 
             * na amount = res.Amount * wartosc_dla_danego_typu;
             * dla dodatkowej dywersyfikacji nalezy dodac wiecej else ifow */

            if (resType == typeof(Gold))
            {
                resourceType = TownResourceType.Zloto;
                amount = res.Amount * 1;
            }
            else if (resType.IsSubclassOf(typeof(BaseWoodenBoard)) || resType.IsSubclassOf(typeof(BaseWoodenLog)))
            {
                resourceType = TownResourceType.Deski;
                amount = res.Amount * 1;
            }
            else if (resType.IsSubclassOf(typeof(BaseIngot)))
            {
                amount = res.Amount * 1;
                resourceType = TownResourceType.Sztaby;
            }
            else if (resType.IsSubclassOf(typeof(BaseLeather)) || resType.IsSubclassOf(typeof(BaseHides)))
            {
                resourceType = TownResourceType.Skora;
                amount = res.Amount * 1;
            }
            else if (resType == typeof(Cloth))
            {
                resourceType = TownResourceType.Material;
                amount = res.Amount * 1;
            }
			else if (resType == typeof(UncutCloth))
            {
                resourceType = TownResourceType.Material;
                amount = res.Amount * 1;
            }
            else if (resType == typeof(BoltOfCloth))
            {
                resourceType = TownResourceType.Material;
                amount = res.Amount * 50; // BoltOfCloth zawiera 50 Cloth
            }
            else if (resType == typeof(Bone))
            {
                resourceType = TownResourceType.Kosci;
                amount = res.Amount * 1;
            }
            else if (resType.IsSubclassOf(typeof(BaseGranite)))
            {
                resourceType = TownResourceType.Kamienie;
                amount = res.Amount * 1;
            }
            else if (resType == typeof(Sand))
            {
                resourceType = TownResourceType.Piasek;
                amount = res.Amount * 1;
            }
            else if (m_klejnoty.Contains(resType))
            {
                resourceType = TownResourceType.Klejnoty;
                amount = res.Amount * 1;
            }
            else if (resType.IsSubclassOf(typeof(BaseJewel)))
            {
                resourceType = TownResourceType.Klejnoty;
                amount = res.Amount * 1;
            }
            else if (m_ziola.Contains(resType))
            {
                resourceType = TownResourceType.Ziola;
                amount = res.Amount * 1;
            }
            else if (resType.IsSubclassOf(typeof(BaseArmor)))
            {
                resourceType = TownResourceType.Zbroje;
                amount = res.Amount * 1;
            }
            else if (resType.IsSubclassOf(typeof(BaseWeapon)))
            {
                resourceType = TownResourceType.Bronie;
                amount = res.Amount * 1;
            }
            else if (resType == typeof(CommodityDeed))
            {
                Item resT = (Item)((CommodityDeed)(res)).Commodity;
                resourceType = CheckResourceType(resT, out amount);
            }

            return resourceType;
        }

        #region Resource Amount
        
        public int ResourceAmount(TownResourceType nType)
        {
            return m_resources.Find(obj => obj.GetType() == nType).Amount;
        }

        public bool HasResource(TownResourceType nType)
        {
            return (m_resources.Find(obj => obj.GetType() == nType) != null);
        }

        public bool HasResourceAmount(TownResourceType nType, int amount)
        {
            if (HasResource(nType))
            {
                return ResourceAmount(nType) >= amount;
            }
            else 
            {
                return false;
            }
        }

        public void ResourceIncreaseAmount(TownResourceType nType, int amount)
        {
            ResourceChangeAmount(nType, amount);
        }

        public void ResourceDecreaseAmount(TownResourceType nType, int amount)
        {
            ResourceChangeAmount(nType, -amount);
        }

        private void ResourceChangeAmount(TownResourceType nType, int amount)
        {
            if (nType != TownResourceType.Invalid)
            {
                m_resources.Find(obj => obj.GetType() == nType).Amount += amount;
                if (m_resources.Find(obj => obj.GetType() == nType).Amount < 0)
                    m_resources.Find(obj => obj.GetType() == nType).Amount = 0;
            }
        }
        #endregion

        #region Max amount
        public int ResourceMaxAmount(TownResourceType nType)
        {
            return m_resources.Find(obj => obj.GetType() == nType).MaxAmount;
        }

        public void ResourceMaxAmountSet(TownResourceType nType, int max)
        {
            m_resources.Find(obj => obj.GetType() == nType).MaxAmount = max;
        }

        public void ResourceMaxAmountIncrease(TownResourceType nType, int max)
        {
            m_resources.Find(obj => obj.GetType() == nType).MaxAmount += max;
        }
        #endregion

        #region Daily change
        public int ResourceDailyChange(TownResourceType nType)
        {
            return m_resources.Find(obj => obj.GetType() == nType).DailyChange;
        }
        
        public void ResourceDailyChangeSet(TownResourceType nType, int daily)
        {
            m_resources.Find(obj => obj.GetType() == nType).DailyChange = daily;
        }

        public void ResourceDailyChangeIncrease(TownResourceType nType, int daily)
        {
            m_resources.Find(obj => obj.GetType() == nType).DailyChange += daily;
        }
        #endregion

        public bool IsResourceAcceptable(Item res, out int amount)
        {
            if (CheckResourceType(res, out amount) != TownResourceType.Invalid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsResourceAmountProper(Item res)
        {
            int m_amount = 1;
            TownResourceType m_resourceType = CheckResourceType(res, out m_amount);
            if (m_resourceType == TownResourceType.Zloto)
            {
                return true;
            }
            else
            {
                if ((ResourceAmount(m_resourceType) + m_amount) <= ResourceMaxAmount(m_resourceType)) // Czy obecny stan surowca + surowiec do dodania zmiesci sie w skarbcu
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsResourceAmountProper(TownResourceType m_resourceType, int amount)
        {
            amount = 1;
            if (m_resourceType == TownResourceType.Zloto)
            {
                return true;
            }
            else
            {
                if ((ResourceAmount(m_resourceType) + amount) <= ResourceMaxAmount(m_resourceType)) // Czy obecny stan surowca + surowiec do dodania zmiesci sie w skarbcu
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public enum TownBuildingStatus
    {
        Niedostepny = 0,
        Dostepny = 1,
        Budowanie = 2,
        Dziala = 3,
        Zawieszony = 4
    }

    public enum TownBuildingName
    {
        MiejsceSpotkan      = 0,
        DomZdziercy         = 1,
        Bank                = 2,
        DomUzdrowiciela     = 3,
        Karczma             = 4,
        Stajnia             = 5,
        WarsztatKrawiecki   = 6,
        WarsztatKowalski    = 7,
        WarsztatStolarski   = 8,
        WarsztatMajstra     = 9,
        WarsztatAlchemika   = 10,
        WarsztatLukmistrza  = 11,
        Torturownia         = 12,
        Piekarnia           = 13,
        Farma               = 14,
        Port                = 15,
        DomGornika          = 16,
        Arena               = 17,
        Palisada            = 18,
        Fosa                = 19,
        MurObronny          = 20,
        BramaDrewniana      = 21,
        BramaStalowa        = 22,
        WiezaStraznica      = 23,
        PlacTreningowy      = 24,
        Koszary             = 25,
        Twierdza            = 26,
        MostDrewniany       = 27,
        MostKamienny        = 28,
        Targowisko          = 29,
        RozbudowaTargowiska = 30,
        Kapliczka           = 31,
        Swiatynia           = 32,
        Biblioteka          = 33,
        DuszaMiasta         = 34,
        Ratusz              = 35,
        Ambasada            = 36,
        Spichlerz           = 37,
        SkladZapasow        = 38,
        Skarbiec            = 39,
        WarsztatMaga        = 40,
		ZagrodaNaOwce       = 41,
    }

    public enum TownGuards
    { 
        Straznik,
        CiezkiStraznik,
        Strzelec,
        StraznikKonny,
        StraznikMag,
        StraznikElitarny
    }

    public class TownPost
    {
        BaseNelderimGuard m_guard = null;
        public int m_x, m_y, m_z;
        Map m_map;

        Towns m_homeTown = Towns.None;
        public Towns HomeTown
        {
            get { return m_homeTown; }
            set { m_homeTown = value; }
        }

        Serial m_guardSerial;
        public Serial GuardSerial
        {
            get { return m_guardSerial; }
            set { m_guardSerial = value; }
        }

        DateTime m_activatedDate;
        public DateTime ActivatedDate
        {
            get { return m_activatedDate; }
            set { m_activatedDate = value; }
        }

        string m_postName; // Nazwa posterunku
        public string PostName
        {
            get { return m_postName; }
            set { m_postName = value; }
        }

        TownGuards m_townGuard; // Poziom straznika
        public TownGuards TownGuard
        {
            get { return m_townGuard; }
            set 
            {
                PostStatus = TownBuildingStatus.Dostepny;
                m_townGuard = value; 
            }
        }

        TownBuildingStatus m_postStatus; // Status posterunku
        public TownBuildingStatus PostStatus
        {
            get { return m_postStatus; }
            set 
            {
                if (value == TownBuildingStatus.Dziala)
                {
                    SetGuard();
                    ActivatedDate = DateTime.Now;
                    RessurectAmount = 0;
                }
                else
                {
                    RemoveGuard();
                }
                m_postStatus = value; 
            }
        }

        public void SetGuard()
        {
            RemoveGuard();
            switch (m_townGuard)
            {
                case TownGuards.Straznik:
                    m_guard = new StandardNelderimGuard();
                    break;
                case TownGuards.CiezkiStraznik:
                    m_guard = new HeavyNelderimGuard();
                    break;
                case TownGuards.Strzelec:
                    m_guard = new ArcherNelderimGuard();
                    break;
                case TownGuards.StraznikKonny:
                    m_guard = new MountedNelderimGuard();
                    break;
                case TownGuards.StraznikMag:
                    m_guard = new MageNelderimGuard();
                    break;
                case TownGuards.StraznikElitarny:
                    m_guard = new EliteNelderimGuard();
                    break;
                default:
                    break;
            }
			m_guard.Home = new Point3D(m_x, m_y, m_z);
			m_guard.RangeHome = 3;
            m_guard.MoveToWorld(new Point3D(m_x, m_y, m_z), m_map);
            m_guard.HomeRegionName = m_homeTown.ToString();
            m_guard.UpdateRegion();
            m_guardSerial = m_guard.Serial;
        }

        public bool IsGuardAlive()
        {
            if (m_guard != null && m_guard.Alive)
            {
                return true;
            }
            return false;
        }

        public void RessurectGuard()
        {
            m_ressurectAmount++;
            SetGuard();
        }

        void RemoveGuard()
        {
            if (m_guard != null && m_guard.Alive)
            {
                m_guard.Kill();
                m_guard.Corpse.Delete();
            }
            m_guard = null;
        }

        int m_ressurectAmount;
        public int RessurectAmount
        {
            get { return m_ressurectAmount; }
            set { m_ressurectAmount = value; }
        }

        public int ActiveDaysAmount
        {
            get 
            {
                DateTime m_now = DateTime.Now;
                if (m_postStatus == TownBuildingStatus.Dziala && DateTime.Now.CompareTo(m_activatedDate.AddDays(1)) == 1)
                    return (int)(m_now.Date - m_activatedDate.Date).TotalDays;
                else
                    return 0;
            }
        }

        public int RessurectPerDay
        {
            get 
            {
                if (ActiveDaysAmount <= 0)
                {
                    return 0; 
                }
                else
                {
                    return m_ressurectAmount / ActiveDaysAmount; 
                }
            }
        }

        public void UpdatePostLocation(Mobile from)
        {
                m_x = from.Location.X;
                m_y = from.Location.Y;
                m_z = from.Location.Z;
                m_guard.Home = new Point3D(m_x, m_y, m_z);
                m_guard.RangeHome = 3;
                m_guard.MoveToWorld(new Point3D(m_x, m_y, m_z), m_map);
                m_guard.HomeRegionName = m_homeTown.ToString();
                m_guard.UpdateRegion();
        }

        public void SpawnGuard()
        {
            switch (TownGuard)
            {
                case TownGuards.Straznik:
                    m_guard = new StandardNelderimGuard();
                    break;
                case TownGuards.CiezkiStraznik:
                    m_guard = new HeavyNelderimGuard();
                    break;
                case TownGuards.Strzelec:
                    m_guard = new ArcherNelderimGuard();
                    break;
                case TownGuards.StraznikKonny:
                    m_guard = new MountedNelderimGuard();
                    break;
                case TownGuards.StraznikMag:
                    m_guard = new MageNelderimGuard();
                    break;
                case TownGuards.StraznikElitarny:
                    m_guard = new EliteNelderimGuard();
                    break;
                default:
                    break;
            }
            m_guard.MoveToWorld(new Point3D(m_x, m_y, m_z), m_map);
            m_guard.UpdateRegion();
        }

        // Zwykly konstruktor
        public TownPost(string nazwa, int x, int y, int z, Towns homeTown, Map map, TownGuards tg = TownGuards.Straznik, TownBuildingStatus status = TownBuildingStatus.Dostepny)
        {
            PostName = nazwa;
            TownGuard = tg;
            ActivatedDate = DateTime.Now;
            m_x = x;
            m_y = y;
            m_z = z;
            m_map = map;
            m_homeTown = homeTown;
            PostStatus = status;
        }

        // Konstruktor dla posterunku, ktorego straznik zyje, a posterunek jest aktywny
        public TownPost(string nazwa, int x, int y, int z, Towns homeTown, Map map, TownGuards tg, DateTime dt, Serial guardSerial)
        {
            PostName = nazwa;
            TownGuard = tg;
            ActivatedDate = dt;
            m_x = x;
            m_y = y;
            m_z = z;
            m_map = map;
            m_homeTown = homeTown;
            m_postStatus = TownBuildingStatus.Dziala;
            m_guardSerial = guardSerial;
            switch (m_townGuard)
            {
                case TownGuards.Straznik:
                    m_guard = (StandardNelderimGuard)World.FindMobile(m_guardSerial);
                    break;
                case TownGuards.CiezkiStraznik:
                    m_guard = (HeavyNelderimGuard)World.FindMobile(m_guardSerial);
                    break;
                case TownGuards.Strzelec:
                    m_guard = (ArcherNelderimGuard)World.FindMobile(m_guardSerial);
                    break;
                case TownGuards.StraznikKonny:
                    m_guard = (MountedNelderimGuard)World.FindMobile(m_guardSerial);
                    break;
                case TownGuards.StraznikMag:
                    m_guard = (MageNelderimGuard)World.FindMobile(m_guardSerial);
                    break;
                case TownGuards.StraznikElitarny:
                    m_guard = (EliteNelderimGuard)World.FindMobile(m_guardSerial);
                    break;
                default:
                    break;
            }
        }

        // Konstruktor dla posterunku, ktorego straznik nie zyje, ale posterunek jest aktywny
        public TownPost(string nazwa, int x, int y, int z, Towns homeTown, Map map, TownGuards tg, DateTime dt)
        {
            PostName = nazwa;
            TownGuard = tg;
            ActivatedDate = dt;
            m_x = x;
            m_y = y;
            m_z = z;
            m_map = map;
            m_homeTown = homeTown;
            m_postStatus = TownBuildingStatus.Dziala;
            m_guard = null;
            m_guardSerial = (Serial)0;
        }
    }

    public class TownBuilding
    {
        TownResources m_resources; // Represents cost and daily change
        public TownResources Resources
        {
            get { return m_resources; }
            set { m_resources = value; }
        }

        bool m_charge = true; // If true will charge for building every day
        public bool Charge
        {
            get { return m_charge; }
            set { m_charge = value; }
        }

        List<TownBuildingName> m_dependecies; // Wymagane budynki do zbudowanie tego
        public List<TownBuildingName> Dependecies
        {
            get { return m_dependecies; }
            set { m_dependecies = value; }
        }

        public TownBuilding(TownBuildingName typBudynku, int poziom, TownBuildingStatus status, int opisID)
        {
            Resources = new TownResources();
            Dependecies = new List<TownBuildingName>();
            BuildingType = typBudynku;
            Poziom = poziom;
            Status = status;
            OpisID = opisID;
        }

        private TownBuildingName m_buildingType;
        public TownBuildingName BuildingType
        {
            get { return m_buildingType; }
            set { m_buildingType = value; }
        }

        private int m_poziom = 0;
        public int Poziom
        {
            get { return m_poziom; }
            set { m_poziom = value; }
        } 
        
        private int m_opisID = 0;
        public int OpisID
        {
            get { return m_opisID; }
            set { m_opisID = value; }
        } 

        private TownBuildingStatus m_status = 0;
        public TownBuildingStatus Status
        {
            get { return m_status; }
            set { m_status = value; }
        }
    }


    public class TownRelation
    {
        public Towns TownOfRelation;
        public int AmountOfRelation;
        public TownRelation(Towns t, int a)
        {
            this.TownOfRelation = t;
            this.AmountOfRelation = a;
        }
        // Override the ToString method:
        public override string ToString()
        {
            return (String.Format("({0} : {1})", TownOfRelation.ToString(), AmountOfRelation));
        }
    }

    public class TownLog
    {
        DateTime m_date;
        public DateTime LogDate
        {
            get { return m_date; }
            set { m_date = value; }
        }

        TownLogTypes m_type;
        public TownLogTypes LogType
        {
            get { return m_type; }
            set { m_type = value; }
        }

        int m_a;
        public int A
        {
            get { return m_a; }
            set { m_a = value; }
        }

        int m_b;
        public int B
        {
            get { return m_b; }
            set { m_b = value; }
        }

        int m_c;
        public int C
        {
            get { return m_c; }
            set { m_c = value; }
        }

        string m_text;
        public string Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        
        public TownLog(DateTime dt, TownLogTypes tlp, string txt = "", int a = 0, int b = 0, int c = 0)
        {
            this.LogDate = dt;
            this.LogType = tlp;
            this.Text = txt;
            this.A = a;
            this.B = b;
            this.C = c;
        }

        // Override the ToString method:
        public override string ToString()
        {
            string toReturn = String.Format("{0} : ", LogDate.ToString());

            switch (LogType)
            {
                case TownLogTypes.OBYWATELSTWO_NADANIE:
                    toReturn = String.Format("{0}{1} zostal obywatelem", toReturn, Text);
                    break;
                case TownLogTypes.OBYWATELSTWO_ZAKONCZONO:
                    toReturn = String.Format("{0}{1} przestal byc obywatelem", toReturn, Text);
                    break;
                case TownLogTypes.OBYWATELSTWO_STATUS_NADANO:
                    TownStatus st = (TownStatus)A;
                    string stString = "";
                    switch (st)
	                {
                        case TownStatus.Leader:
                            stString = "przedstawicielem, niech zyje dlugo!";
                            break;
                        case TownStatus.Counsellor:
                            stString = "kanclerzem";
                            break;
                        case TownStatus.Citizen:
                            stString = "obywatelem";
                            break;
                        default:
                            break;
	                }
                    toReturn = String.Format("{0}{1} zostal {2}", toReturn, Text, stString);
                    break;
                case TownLogTypes.KANCLERZ_NADANO_GLOWNY:
                    toReturn = String.Format("{0}{1} zostal kanclerzem glownym", toReturn, Text);
                    break;
                case TownLogTypes.KANCLERZ_NADANO_DYPLOMACJI:
                    toReturn = String.Format("{0}{1} zostal kanclerzem dyplomacji", toReturn, Text);
                    break;
                case TownLogTypes.KANCLERZ_NADANO_ARMII:
                    toReturn = String.Format("{0}{1} zostal kanclerzem armii", toReturn, Text);
                    break;
                case TownLogTypes.KANCLERZ_NADANO_EKONOMII:
                    toReturn = String.Format("{0}{1} zostal kanclerzem ekonomii", toReturn, Text);
                    break;
                case TownLogTypes.KANCLERZ_NADANO_BUDOWNICTWA:
                    toReturn = String.Format("{0}{1} zostal kanclerzem budownictwa", toReturn, Text);
                    break;
                case TownLogTypes.BUDYNEK_ZLECONO_BUDOWE:
                    toReturn = String.Format("{0}zlecono budowe budynku - {1}", toReturn, (TownBuildingName)A);
                    break;
                case TownLogTypes.BUDYNEK_ZAKONCZONO_BUDOWE:
                    toReturn = String.Format("{0}zakonczono budowe budynku - {1}", toReturn, (TownBuildingName)A);
                    break;
                case TownLogTypes.BUDYNEK_ZAWIESZONO_DZIALANIE:
                    toReturn = String.Format("{0}zawieszono dzialanie budynku - {1}", toReturn, (TownBuildingName)A);
                    break;
                case TownLogTypes.BUDYNEK_WZNOWIONO_DZIALANIE:
                    toReturn = String.Format("{0}wznowiono dzialanie budynku - {1}", toReturn, (TownBuildingName)A);
                    break;
                case TownLogTypes.BUDYNEK_ZNISZCZONO:
                    toReturn = String.Format("{0}budynek {1} zostal zniszczony!", toReturn, (TownBuildingName)A);
                    break;
                case TownLogTypes.PODATKI_ZMIENIONO:
                    string forWhom = "";
                    if (A == 0)
                        forWhom = "obywateli tego miasta";
                    else if (A == 1)
                        forWhom = "obywateli innych miasta";
                    else if (A == 2)
                        forWhom = "nie bedacych obywatelami zadnego miasta";
                    toReturn = String.Format("{0}podatki dla {1} zostaly zmienione na {2}%", toReturn, forWhom, B);
                    break;
                case TownLogTypes.POSTERUNEK_WYBUDOWANO:
                    toReturn = String.Format("{0}postawiono nowy posterunek", toReturn);
                    break;
                case TownLogTypes.POSTERUNEK_ZAWIESZONO:
                    toReturn = String.Format("{0}posterunek {1} zostal zawieszony w dzialaniu", toReturn, Text);
                    break;
                case TownLogTypes.POSTERUNEK_WZNOWIONO:
                    toReturn = String.Format("{0}posterunek {1} zostal wznowiony w dzialaniu", toReturn, Text);
                    break;
                case TownLogTypes.SUROWCE_WYSLANO:
                    toReturn = String.Format("{0}wyslano {1} surowca {2} do miasta {3}", toReturn, A, ((TownResourceType)B).ToString(), ((Towns)C).ToString());
                    break;
                case TownLogTypes.SUROWCE_OTRZYMANO:
                    toReturn = String.Format("{0}otrzymano {1} surowca {2} od miasta {3}", toReturn, A, ((TownResourceType)B).ToString(), ((Towns)C).ToString());
                    break;
                case TownLogTypes.RELACJE_ZMIENIONO:
                    toReturn = String.Format("{0}relacje z miastem {1} zostaly zmienione o {2}", toReturn, ((Towns)A).ToString(), B);
                    break;
                default:
                    break;
            }
            return toReturn;
        }
    }

    public enum TownLogTypes
	{
        OBYWATELSTWO_NADANIE                 = 0,
	    OBYWATELSTWO_ZAKONCZONO              = 1,
        OBYWATELSTWO_STATUS_NADANO           = 2,
        KANCLERZ_NADANO_GLOWNY               = 3,
        KANCLERZ_NADANO_DYPLOMACJI           = 4,
        KANCLERZ_NADANO_ARMII                = 5,
        KANCLERZ_NADANO_EKONOMII             = 6,
        KANCLERZ_NADANO_BUDOWNICTWA          = 7,
        BUDYNEK_ZLECONO_BUDOWE               = 8,
        BUDYNEK_ZAKONCZONO_BUDOWE            = 9,
        BUDYNEK_ZAWIESZONO_DZIALANIE         = 10,
        BUDYNEK_WZNOWIONO_DZIALANIE          = 11,
        BUDYNEK_ZNISZCZONO                   = 12,
        PODATKI_ZMIENIONO                    = 13,
        POSTERUNEK_WYBUDOWANO                = 14,
        POSTERUNEK_ZAWIESZONO                = 15,
        POSTERUNEK_WZNOWIONO                 = 16,
        SUROWCE_WYSLANO                      = 17,
        SUROWCE_OTRZYMANO                    = 18,
        RELACJE_ZMIENIONO                    = 19
	}
}
