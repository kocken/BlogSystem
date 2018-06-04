﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public enum Value
    {
        Unevaluated, Approved, Disapproved
    }

    public class Evaluation
    {
        public int ID { get; set; }
        public Post Post { get; set; }
        public Value? Value { get; set; }
        public User EvaluatedBy { get; set; }
        public DateTime EvaluationTime { get; set; }
    }
}
