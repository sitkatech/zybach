﻿namespace Zybach.Models.DataTransferObjects
{
    public partial class WaterLevelInspectionSimpleDto
    {
        public WellSimpleDto Well { get; set; }
        public string CropTypeName { get; set; }
        public UserSimpleDto Inspector { get; set; }
    }
}