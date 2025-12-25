using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRule
{
    public string ID { get; set; }
    public RuleType ruleType { get; }
}
