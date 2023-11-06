using CGKUpdated.Models;
using CGKUpdated.Models.Filters;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace CGKUpdated.Models
{
    public class ImagePairViewModel
    {

        // TODO: Switch to composite key in model builder
        [Key]
        public string title { get; set; }
        public string user { get; set; }
        public string filter { get; set; }
        public IFormFile original { get; set; }

        public ImagePairViewModel()
        {

        }

    }
}
