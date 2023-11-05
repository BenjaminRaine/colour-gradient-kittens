//THIS -> https://www.youtube.com/watch?v=bqyZiwXOMH0

using CGKProject.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace CGKUpdated.Models
{
    public class ImagePair
    {

        // TODO: Switch to composite key in model builder
        [Key]
        public string title { get; set; }
        public string user { get; set; }
        public DateTime addedAt { get; set; }

        public ImagePair()
        {

        }
    }
}
