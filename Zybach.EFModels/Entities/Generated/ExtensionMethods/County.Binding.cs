//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[County]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class County : IHavePrimaryKey
    {
        public static readonly CountyArthur Arthur = Zybach.EFModels.Entities.CountyArthur.Instance;
        public static readonly CountyKeith Keith = Zybach.EFModels.Entities.CountyKeith.Instance;
        public static readonly CountyLincoln Lincoln = Zybach.EFModels.Entities.CountyLincoln.Instance;
        public static readonly CountyMcPherson McPherson = Zybach.EFModels.Entities.CountyMcPherson.Instance;

        public static readonly List<County> All;
        public static readonly List<CountyDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, County> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, CountyDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static County()
        {
            All = new List<County> { Arthur, Keith, Lincoln, McPherson };
            AllAsDto = new List<CountyDto> { Arthur.AsDto(), Keith.AsDto(), Lincoln.AsDto(), McPherson.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, County>(All.ToDictionary(x => x.CountyID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, CountyDto>(AllAsDto.ToDictionary(x => x.CountyID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected County(int countyID, string countyName, string countyDisplayName)
        {
            CountyID = countyID;
            CountyName = countyName;
            CountyDisplayName = countyDisplayName;
        }

        [Key]
        public int CountyID { get; private set; }
        public string CountyName { get; private set; }
        public string CountyDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return CountyID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(County other)
        {
            if (other == null)
            {
                return false;
            }
            return other.CountyID == CountyID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as County);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return CountyID;
        }

        public static bool operator ==(County left, County right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(County left, County right)
        {
            return !Equals(left, right);
        }

        public CountyEnum ToEnum => (CountyEnum)GetHashCode();

        public static County ToType(int enumValue)
        {
            return ToType((CountyEnum)enumValue);
        }

        public static County ToType(CountyEnum enumValue)
        {
            switch (enumValue)
            {
                case CountyEnum.Arthur:
                    return Arthur;
                case CountyEnum.Keith:
                    return Keith;
                case CountyEnum.Lincoln:
                    return Lincoln;
                case CountyEnum.McPherson:
                    return McPherson;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum CountyEnum
    {
        Arthur = 1,
        Keith = 2,
        Lincoln = 3,
        McPherson = 4
    }

    public partial class CountyArthur : County
    {
        private CountyArthur(int countyID, string countyName, string countyDisplayName) : base(countyID, countyName, countyDisplayName) {}
        public static readonly CountyArthur Instance = new CountyArthur(1, @"Arthur", @"Arthur");
    }

    public partial class CountyKeith : County
    {
        private CountyKeith(int countyID, string countyName, string countyDisplayName) : base(countyID, countyName, countyDisplayName) {}
        public static readonly CountyKeith Instance = new CountyKeith(2, @"Keith", @"Keith");
    }

    public partial class CountyLincoln : County
    {
        private CountyLincoln(int countyID, string countyName, string countyDisplayName) : base(countyID, countyName, countyDisplayName) {}
        public static readonly CountyLincoln Instance = new CountyLincoln(3, @"Lincoln", @"Lincoln");
    }

    public partial class CountyMcPherson : County
    {
        private CountyMcPherson(int countyID, string countyName, string countyDisplayName) : base(countyID, countyName, countyDisplayName) {}
        public static readonly CountyMcPherson Instance = new CountyMcPherson(4, @"McPherson", @"McPherson");
    }
}