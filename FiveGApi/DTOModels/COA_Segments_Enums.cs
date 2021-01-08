using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class COA_Segments_Enums
    {
       public enum SegmentType
        {
            Company,
            Project,
            Location,
            Account,
            Party
        };
       public enum type
        {
            Asset,
            Liability,
            Revenue,
            Expense,
            OwnerEquity
        };

    }
}