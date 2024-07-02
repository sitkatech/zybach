using System.Linq;
using Zybach.Models.Abstracts;

namespace Zybach.Models.Prism;

public class PrismDataElement : ClassEnum<PrismDataElement>
{
    public string QueryValue { get; set; }

    public PrismDataElement(string val)
    {
        QueryValue = val;
    }

    public static PrismDataElement PPT = new ("ppt");
    public static PrismDataElement TMin = new ("tmin");
    public static PrismDataElement TMax = new("tmax");
    public static PrismDataElement TMean = new ("tmean");
    public static PrismDataElement TDMean = new ("tdmean");
    public static PrismDataElement VPDMin = new ("vpdmin");
    public static PrismDataElement VPDMaxn = new ("vpdmax");

    public override string ToString()
    {
        return QueryValue;
    }

    public static PrismDataElement FromString(string val)
    {
        return All.First(x => x.QueryValue == val);
    }
}