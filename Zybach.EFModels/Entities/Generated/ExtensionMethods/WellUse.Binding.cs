//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellUse]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class WellUse
    {
        public static readonly WellUseIrrigation Irrigation = Zybach.EFModels.Entities.WellUseIrrigation.Instance;
        public static readonly WellUsePublicSupply PublicSupply = Zybach.EFModels.Entities.WellUsePublicSupply.Instance;
        public static readonly WellUseDomestic Domestic = Zybach.EFModels.Entities.WellUseDomestic.Instance;
        public static readonly WellUseMonitoring Monitoring = Zybach.EFModels.Entities.WellUseMonitoring.Instance;

        public static readonly List<WellUse> All;
        public static readonly List<WellUseDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, WellUse> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, WellUseDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static WellUse()
        {
            All = new List<WellUse> { Irrigation, PublicSupply, Domestic, Monitoring };
            AllAsDto = new List<WellUseDto> { Irrigation.AsDto(), PublicSupply.AsDto(), Domestic.AsDto(), Monitoring.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, WellUse>(All.ToDictionary(x => x.WellUseID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, WellUseDto>(AllAsDto.ToDictionary(x => x.WellUseID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected WellUse(int wellUseID, string wellUseName, string wellUseDisplayName)
        {
            WellUseID = wellUseID;
            WellUseName = wellUseName;
            WellUseDisplayName = wellUseDisplayName;
        }

        [Key]
        public int WellUseID { get; private set; }
        public string WellUseName { get; private set; }
        public string WellUseDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return WellUseID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(WellUse other)
        {
            if (other == null)
            {
                return false;
            }
            return other.WellUseID == WellUseID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as WellUse);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return WellUseID;
        }

        public static bool operator ==(WellUse left, WellUse right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WellUse left, WellUse right)
        {
            return !Equals(left, right);
        }

        public WellUseEnum ToEnum => (WellUseEnum)GetHashCode();

        public static WellUse ToType(int enumValue)
        {
            return ToType((WellUseEnum)enumValue);
        }

        public static WellUse ToType(WellUseEnum enumValue)
        {
            switch (enumValue)
            {
                case WellUseEnum.Domestic:
                    return Domestic;
                case WellUseEnum.Irrigation:
                    return Irrigation;
                case WellUseEnum.Monitoring:
                    return Monitoring;
                case WellUseEnum.PublicSupply:
                    return PublicSupply;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum WellUseEnum
    {
        Irrigation = 1,
        PublicSupply = 2,
        Domestic = 3,
        Monitoring = 4
    }

    public partial class WellUseIrrigation : WellUse
    {
        private WellUseIrrigation(int wellUseID, string wellUseName, string wellUseDisplayName) : base(wellUseID, wellUseName, wellUseDisplayName) {}
        public static readonly WellUseIrrigation Instance = new WellUseIrrigation(1, @"Irrigation", @"Irrigation");
    }

    public partial class WellUsePublicSupply : WellUse
    {
        private WellUsePublicSupply(int wellUseID, string wellUseName, string wellUseDisplayName) : base(wellUseID, wellUseName, wellUseDisplayName) {}
        public static readonly WellUsePublicSupply Instance = new WellUsePublicSupply(2, @"Public Supply", @"Public Supply");
    }

    public partial class WellUseDomestic : WellUse
    {
        private WellUseDomestic(int wellUseID, string wellUseName, string wellUseDisplayName) : base(wellUseID, wellUseName, wellUseDisplayName) {}
        public static readonly WellUseDomestic Instance = new WellUseDomestic(3, @"Domestic", @"Domestic");
    }

    public partial class WellUseMonitoring : WellUse
    {
        private WellUseMonitoring(int wellUseID, string wellUseName, string wellUseDisplayName) : base(wellUseID, wellUseName, wellUseDisplayName) {}
        public static readonly WellUseMonitoring Instance = new WellUseMonitoring(4, @"Monitoring", @"Monitoring");
    }
}