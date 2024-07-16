//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismDataType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class PrismDataType : IHavePrimaryKey
    {
        public static readonly PrismDataTypeppt ppt = Zybach.EFModels.Entities.PrismDataTypeppt.Instance;
        public static readonly PrismDataTypetmin tmin = Zybach.EFModels.Entities.PrismDataTypetmin.Instance;
        public static readonly PrismDataTypetmax tmax = Zybach.EFModels.Entities.PrismDataTypetmax.Instance;

        public static readonly List<PrismDataType> All;
        public static readonly List<PrismDataTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, PrismDataType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, PrismDataTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static PrismDataType()
        {
            All = new List<PrismDataType> { ppt, tmin, tmax };
            AllAsDto = new List<PrismDataTypeDto> { ppt.AsDto(), tmin.AsDto(), tmax.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, PrismDataType>(All.ToDictionary(x => x.PrismDataTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, PrismDataTypeDto>(AllAsDto.ToDictionary(x => x.PrismDataTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected PrismDataType(int prismDataTypeID, string prismDataTypeName, string prismDataTypeDisplayName)
        {
            PrismDataTypeID = prismDataTypeID;
            PrismDataTypeName = prismDataTypeName;
            PrismDataTypeDisplayName = prismDataTypeDisplayName;
        }

        [Key]
        public int PrismDataTypeID { get; private set; }
        public string PrismDataTypeName { get; private set; }
        public string PrismDataTypeDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return PrismDataTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(PrismDataType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.PrismDataTypeID == PrismDataTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as PrismDataType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return PrismDataTypeID;
        }

        public static bool operator ==(PrismDataType left, PrismDataType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PrismDataType left, PrismDataType right)
        {
            return !Equals(left, right);
        }

        public PrismDataTypeEnum ToEnum => (PrismDataTypeEnum)GetHashCode();

        public static PrismDataType ToType(int enumValue)
        {
            return ToType((PrismDataTypeEnum)enumValue);
        }

        public static PrismDataType ToType(PrismDataTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case PrismDataTypeEnum.ppt:
                    return ppt;
                case PrismDataTypeEnum.tmax:
                    return tmax;
                case PrismDataTypeEnum.tmin:
                    return tmin;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum PrismDataTypeEnum
    {
        ppt = 1,
        tmin = 2,
        tmax = 3
    }

    public partial class PrismDataTypeppt : PrismDataType
    {
        private PrismDataTypeppt(int prismDataTypeID, string prismDataTypeName, string prismDataTypeDisplayName) : base(prismDataTypeID, prismDataTypeName, prismDataTypeDisplayName) {}
        public static readonly PrismDataTypeppt Instance = new PrismDataTypeppt(1, @"ppt", @"PPT");
    }

    public partial class PrismDataTypetmin : PrismDataType
    {
        private PrismDataTypetmin(int prismDataTypeID, string prismDataTypeName, string prismDataTypeDisplayName) : base(prismDataTypeID, prismDataTypeName, prismDataTypeDisplayName) {}
        public static readonly PrismDataTypetmin Instance = new PrismDataTypetmin(2, @"tmin", @"T Min");
    }

    public partial class PrismDataTypetmax : PrismDataType
    {
        private PrismDataTypetmax(int prismDataTypeID, string prismDataTypeName, string prismDataTypeDisplayName) : base(prismDataTypeID, prismDataTypeName, prismDataTypeDisplayName) {}
        public static readonly PrismDataTypetmax Instance = new PrismDataTypetmax(3, @"tmax", @"T Max");
    }
}