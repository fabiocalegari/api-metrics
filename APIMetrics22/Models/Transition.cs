using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIMetrics22.Models
{
    [Table("listener_transition")]
    public class Transition
    {
        private readonly APIMetricsContext _context;
        private TimeSpan _timeInTransition;

        public Transition(APIMetricsContext context)
        {
            _context = context;
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("issue_id"), ForeignKey("Issue")]
        public int IssueId { get; set; }
        [Column("data_evento")]
        public DateTime DataEvento { get; set; }
        [Column("transicao_origem")]
        public string TransicaoOrigem { get; set; }
        [Column("transicao_destino")]
        public string TransicaoDestino { get; set; }

        public double DaysInTransition
        {
            get
            {
                return TimeInTransition.TotalDays;
            }
        }

        public double HoursInTransition
        {
            get
            {
                return TimeInTransition.TotalHours;
            }
        }

        public TimeSpan TimeInTransition
        {
            get
            {
                if (!_timeInTransition.Equals(new TimeSpan()))
                {
                    return _timeInTransition;
                }

                var transitionAnterior = _context.Transition.Where(m => m.IssueId == IssueId && m.TransicaoDestino == TransicaoOrigem).FirstOrDefault();
                if( transitionAnterior != null && transitionAnterior.DataEvento != null && DataEvento != null)
                {
                    _timeInTransition = DataEvento.Subtract(transitionAnterior.DataEvento);
                }
                return _timeInTransition;
            }
        }
    }
}
