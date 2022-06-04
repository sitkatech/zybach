//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationLowPressureValve]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class ChemigationLowPressureValve
    {
        public static readonly ChemigationLowPressureValveRubberDam RubberDam = Zybach.EFModels.Entities.ChemigationLowPressureValveRubberDam.Instance;
        public static readonly ChemigationLowPressureValveSpringLoaded SpringLoaded = Zybach.EFModels.Entities.ChemigationLowPressureValveSpringLoaded.Instance;

        public static readonly List<ChemigationLowPressureValve> All;
        public static readonly List<ChemigationLowPressureValveDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, ChemigationLowPressureValve> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, ChemigationLowPressureValveDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static ChemigationLowPressureValve()
        {
            All = new List<ChemigationLowPressureValve> { RubberDam, SpringLoaded };
            AllAsDto = new List<ChemigationLowPressureValveDto> { RubberDam.AsDto(), SpringLoaded.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, ChemigationLowPressureValve>(All.ToDictionary(x => x.ChemigationLowPressureValveID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, ChemigationLowPressureValveDto>(AllAsDto.ToDictionary(x => x.ChemigationLowPressureValveID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected ChemigationLowPressureValve(int chemigationLowPressureValveID, string chemigationLowPressureValveName, string chemigationLowPressureValveDisplayName)
        {
            ChemigationLowPressureValveID = chemigationLowPressureValveID;
            ChemigationLowPressureValveName = chemigationLowPressureValveName;
            ChemigationLowPressureValveDisplayName = chemigationLowPressureValveDisplayName;
        }

        [Key]
        public int ChemigationLowPressureValveID { get; private set; }
        public string ChemigationLowPressureValveName { get; private set; }
        public string ChemigationLowPressureValveDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return ChemigationLowPressureValveID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(ChemigationLowPressureValve other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ChemigationLowPressureValveID == ChemigationLowPressureValveID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as ChemigationLowPressureValve);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return ChemigationLowPressureValveID;
        }

        public static bool operator ==(ChemigationLowPressureValve left, ChemigationLowPressureValve right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ChemigationLowPressureValve left, ChemigationLowPressureValve right)
        {
            return !Equals(left, right);
        }

        public ChemigationLowPressureValveEnum ToEnum => (ChemigationLowPressureValveEnum)GetHashCode();

        public static ChemigationLowPressureValve ToType(int enumValue)
        {
            return ToType((ChemigationLowPressureValveEnum)enumValue);
        }

        public static ChemigationLowPressureValve ToType(ChemigationLowPressureValveEnum enumValue)
        {
            switch (enumValue)
            {
                case ChemigationLowPressureValveEnum.RubberDam:
                    return RubberDam;
                case ChemigationLowPressureValveEnum.SpringLoaded:
                    return SpringLoaded;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum ChemigationLowPressureValveEnum
    {
        RubberDam = 1,
        SpringLoaded = 2
    }

    public partial class ChemigationLowPressureValveRubberDam : ChemigationLowPressureValve
    {
        private ChemigationLowPressureValveRubberDam(int chemigationLowPressureValveID, string chemigationLowPressureValveName, string chemigationLowPressureValveDisplayName) : base(chemigationLowPressureValveID, chemigationLowPressureValveName, chemigationLowPressureValveDisplayName) {}
        public static readonly ChemigationLowPressureValveRubberDam Instance = new ChemigationLowPressureValveRubberDam(1, @"RubberDam", @"Rubber Dam");
    }

    public partial class ChemigationLowPressureValveSpringLoaded : ChemigationLowPressureValve
    {
        private ChemigationLowPressureValveSpringLoaded(int chemigationLowPressureValveID, string chemigationLowPressureValveName, string chemigationLowPressureValveDisplayName) : base(chemigationLowPressureValveID, chemigationLowPressureValveName, chemigationLowPressureValveDisplayName) {}
        public static readonly ChemigationLowPressureValveSpringLoaded Instance = new ChemigationLowPressureValveSpringLoaded(2, @"SpringLoaded", @"Spring Loaded");
    }
}