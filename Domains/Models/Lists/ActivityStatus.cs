using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Lists
{
    public class ActivitiesStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityStatusId { get; set; }
        public bool ActivityStatus { get; set; }  
        public int IsGroup { get; set; }
        public int IsAllow { get; set; }
    }
}
