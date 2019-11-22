using System;
using System.Collections.Generic;
using System.Text;

namespace OutboxPatternExample.OutboxProcessor
{
    public class OutboxRecord
    {
        public int Id { get; set; }
        public string RequestMessage { get; set; }
        public DateTime QueuedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string ProcessedStatus { get; set; }
        public string ErrorMessage { get; set; }
    }
}
