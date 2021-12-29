using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class WaterQualityInspections
    {
        public static IQueryable<WaterQualityInspection> ListImpl(ZybachDbContext dbContext)
        {
            return dbContext.WaterQualityInspections
                .Include(x => x.Well)
                .Include(x => x.WaterQualityInspectionType)
                .Include(x => x.CropType)
                .Include(x => x.InspectorUser)
                .ThenInclude(x => x.Role)
                .AsNoTracking();
        }

        public static List<WaterQualityInspectionSimpleDto> ListAsSimpleDto(ZybachDbContext dbContext)
        {
            return ListImpl(dbContext).OrderByDescending(x => x.InspectionDate).ThenBy(x => x.Well.WellRegistrationID).Select(x => x.AsSimpleDto()).ToList();
        }

        public static WaterQualityInspectionSimpleDto GetByIDAsSimpleDto(ZybachDbContext dbContext, int waterQualityInspectionID)
        {
            return ListImpl(dbContext).SingleOrDefault(x => x.WaterQualityInspectionID == waterQualityInspectionID)?.AsSimpleDto();
        }

        public static WaterQualityInspectionSimpleDto CreateWaterQualityInspection(ZybachDbContext dbContext,
            WaterQualityInspectionUpsertDto waterQualityInspectionUpsert, int wellID)
        {
            var waterQualityInspection = new WaterQualityInspection
            {
                WellID = wellID,
                WaterQualityInspectionTypeID = waterQualityInspectionUpsert.WaterQualityInspectionTypeID,
                InspectionDate = waterQualityInspectionUpsert.InspectionDate,
                InspectorUserID = waterQualityInspectionUpsert.InspectorUserID,
                Temperature = waterQualityInspectionUpsert.Temperature,
                PH = waterQualityInspectionUpsert.PH,
                Conductivity = waterQualityInspectionUpsert.Conductivity,
                FieldAlkilinity = waterQualityInspectionUpsert.FieldAlkilinity,
                FieldNitrates = waterQualityInspectionUpsert.FieldNitrates,
                LabNitrates = waterQualityInspectionUpsert.LabNitrates,
                Salinity = waterQualityInspectionUpsert.Salinity,
                MV = waterQualityInspectionUpsert.MV,
                Sodium = waterQualityInspectionUpsert.Sodium,
                Calcium = waterQualityInspectionUpsert.Calcium,
                Magnesium = waterQualityInspectionUpsert.Magnesium,
                Potassium = waterQualityInspectionUpsert.Potassium,
                HydrogenCarbonate = waterQualityInspectionUpsert.HydrogenCarbonate,
                CalciumCarbonate = waterQualityInspectionUpsert.CalciumCarbonate,
                Sulfate = waterQualityInspectionUpsert.Sulfate,
                Chloride = waterQualityInspectionUpsert.Chloride,
                SiliconDioxide = waterQualityInspectionUpsert.SiliconDioxide,
                CropTypeID = waterQualityInspectionUpsert.CropTypeID,
                PreWaterLevel = waterQualityInspectionUpsert.PreWaterLevel,
                PostWaterLevel = waterQualityInspectionUpsert.PostWaterLevel,
                InspectionNotes = waterQualityInspectionUpsert.InspectionNotes
            };

            dbContext.WaterQualityInspections.Add(waterQualityInspection);
            dbContext.SaveChanges();
            dbContext.Entry(waterQualityInspection).Reload();
            return GetByIDAsSimpleDto(dbContext, waterQualityInspection.WaterQualityInspectionID);
        }

        public static void UpdateWaterQualityInspection(ZybachDbContext dbContext, WaterQualityInspection waterQualityInspection, WaterQualityInspectionUpsertDto waterQualityInspectionUpsert, int wellID)
        {
            waterQualityInspection.WellID = wellID;
            waterQualityInspection.WaterQualityInspectionTypeID = waterQualityInspectionUpsert.WaterQualityInspectionTypeID;
            waterQualityInspection.InspectionDate = waterQualityInspectionUpsert.InspectionDate;
            waterQualityInspection.InspectorUserID = waterQualityInspectionUpsert.InspectorUserID;
            waterQualityInspection.Temperature = waterQualityInspectionUpsert.Temperature;
            waterQualityInspection.PH = waterQualityInspectionUpsert.PH;
            waterQualityInspection.Conductivity = waterQualityInspectionUpsert.Conductivity;
            waterQualityInspection.FieldAlkilinity = waterQualityInspectionUpsert.FieldAlkilinity;
            waterQualityInspection.FieldNitrates = waterQualityInspectionUpsert.FieldNitrates;
            waterQualityInspection.LabNitrates = waterQualityInspectionUpsert.LabNitrates;
            waterQualityInspection.Salinity = waterQualityInspectionUpsert.Salinity;
            waterQualityInspection.MV = waterQualityInspectionUpsert.MV;
            waterQualityInspection.Sodium = waterQualityInspectionUpsert.Sodium;
            waterQualityInspection.Calcium = waterQualityInspectionUpsert.Calcium;
            waterQualityInspection.Magnesium = waterQualityInspectionUpsert.Magnesium;
            waterQualityInspection.Potassium = waterQualityInspectionUpsert.Potassium;
            waterQualityInspection.HydrogenCarbonate = waterQualityInspectionUpsert.HydrogenCarbonate;
            waterQualityInspection.CalciumCarbonate = waterQualityInspectionUpsert.CalciumCarbonate;
            waterQualityInspection.Sulfate = waterQualityInspectionUpsert.Sulfate;
            waterQualityInspection.Chloride = waterQualityInspectionUpsert.Chloride;
            waterQualityInspection.SiliconDioxide = waterQualityInspectionUpsert.SiliconDioxide;
            waterQualityInspection.CropTypeID = waterQualityInspectionUpsert.CropTypeID;
            waterQualityInspection.PreWaterLevel = waterQualityInspectionUpsert.PreWaterLevel;
            waterQualityInspection.PostWaterLevel = waterQualityInspectionUpsert.PostWaterLevel;
            waterQualityInspection.InspectionNotes = waterQualityInspectionUpsert.InspectionNotes;
            
            dbContext.SaveChanges();
        }

        public static WaterQualityInspection GetByID(ZybachDbContext dbContext, int waterQualityInspectionID)
        {
            return dbContext.WaterQualityInspections.SingleOrDefault(x => x.WaterQualityInspectionID == waterQualityInspectionID);
        }
    }
}