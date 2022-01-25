using Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public Guid? UniqueGuid { get; set; } = Guid.NewGuid();

        public DateTime? DateCreated { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public string DateCreatedAgo
        {
            get
            {
                var date = DateCreated.GetValueOrDefault().TimeAgo();

                return date;
            }
        }
    }
}
