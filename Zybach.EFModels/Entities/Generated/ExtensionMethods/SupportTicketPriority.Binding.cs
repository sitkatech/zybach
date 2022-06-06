//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicketPriority]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class SupportTicketPriority
    {
        public static readonly SupportTicketPriorityHigh High = Zybach.EFModels.Entities.SupportTicketPriorityHigh.Instance;
        public static readonly SupportTicketPriorityMedium Medium = Zybach.EFModels.Entities.SupportTicketPriorityMedium.Instance;
        public static readonly SupportTicketPriorityLow Low = Zybach.EFModels.Entities.SupportTicketPriorityLow.Instance;

        public static readonly List<SupportTicketPriority> All;
        public static readonly List<SupportTicketPriorityDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, SupportTicketPriority> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, SupportTicketPriorityDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static SupportTicketPriority()
        {
            All = new List<SupportTicketPriority> { High, Medium, Low };
            AllAsDto = new List<SupportTicketPriorityDto> { High.AsDto(), Medium.AsDto(), Low.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, SupportTicketPriority>(All.ToDictionary(x => x.SupportTicketPriorityID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, SupportTicketPriorityDto>(AllAsDto.ToDictionary(x => x.SupportTicketPriorityID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected SupportTicketPriority(int supportTicketPriorityID, string supportTicketPriorityName, string supportTicketPriorityDisplayName, int sortOrder)
        {
            SupportTicketPriorityID = supportTicketPriorityID;
            SupportTicketPriorityName = supportTicketPriorityName;
            SupportTicketPriorityDisplayName = supportTicketPriorityDisplayName;
            SortOrder = sortOrder;
        }

        [Key]
        public int SupportTicketPriorityID { get; private set; }
        public string SupportTicketPriorityName { get; private set; }
        public string SupportTicketPriorityDisplayName { get; private set; }
        public int SortOrder { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return SupportTicketPriorityID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(SupportTicketPriority other)
        {
            if (other == null)
            {
                return false;
            }
            return other.SupportTicketPriorityID == SupportTicketPriorityID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as SupportTicketPriority);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return SupportTicketPriorityID;
        }

        public static bool operator ==(SupportTicketPriority left, SupportTicketPriority right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SupportTicketPriority left, SupportTicketPriority right)
        {
            return !Equals(left, right);
        }

        public SupportTicketPriorityEnum ToEnum => (SupportTicketPriorityEnum)GetHashCode();

        public static SupportTicketPriority ToType(int enumValue)
        {
            return ToType((SupportTicketPriorityEnum)enumValue);
        }

        public static SupportTicketPriority ToType(SupportTicketPriorityEnum enumValue)
        {
            switch (enumValue)
            {
                case SupportTicketPriorityEnum.High:
                    return High;
                case SupportTicketPriorityEnum.Low:
                    return Low;
                case SupportTicketPriorityEnum.Medium:
                    return Medium;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum SupportTicketPriorityEnum
    {
        High = 1,
        Medium = 2,
        Low = 3
    }

    public partial class SupportTicketPriorityHigh : SupportTicketPriority
    {
        private SupportTicketPriorityHigh(int supportTicketPriorityID, string supportTicketPriorityName, string supportTicketPriorityDisplayName, int sortOrder) : base(supportTicketPriorityID, supportTicketPriorityName, supportTicketPriorityDisplayName, sortOrder) {}
        public static readonly SupportTicketPriorityHigh Instance = new SupportTicketPriorityHigh(1, @"High", @"High", 10);
    }

    public partial class SupportTicketPriorityMedium : SupportTicketPriority
    {
        private SupportTicketPriorityMedium(int supportTicketPriorityID, string supportTicketPriorityName, string supportTicketPriorityDisplayName, int sortOrder) : base(supportTicketPriorityID, supportTicketPriorityName, supportTicketPriorityDisplayName, sortOrder) {}
        public static readonly SupportTicketPriorityMedium Instance = new SupportTicketPriorityMedium(2, @"Medium", @"Medium", 20);
    }

    public partial class SupportTicketPriorityLow : SupportTicketPriority
    {
        private SupportTicketPriorityLow(int supportTicketPriorityID, string supportTicketPriorityName, string supportTicketPriorityDisplayName, int sortOrder) : base(supportTicketPriorityID, supportTicketPriorityName, supportTicketPriorityDisplayName, sortOrder) {}
        public static readonly SupportTicketPriorityLow Instance = new SupportTicketPriorityLow(3, @"Low", @"Low", 30);
    }
}