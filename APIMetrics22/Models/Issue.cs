using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIMetrics22.Models
{
    [Table("listener_issue")]
    public class Issue
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("chave")]
        public string Key { get; set; }
        [Column("data_criacao")]
        public DateTime CreationDate { get; set; }
        [Column("issue_type")]
        public string IssueType { get; set; }
        [Column("projeto")]
        public string Project { get; set; }
    }
}
