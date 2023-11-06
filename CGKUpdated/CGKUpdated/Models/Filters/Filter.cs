using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Drawing;
using System.Drawing.Imaging;

namespace CGKUpdated.Models.Filters
{
    public abstract class Filter
    {
        public abstract void ApplyFilter(Bitmap original);       
    }
}
