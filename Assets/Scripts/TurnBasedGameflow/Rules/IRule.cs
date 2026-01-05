using System.Collections;

public interface IRule
{
    // Интерфейс правила, общий
    public string ID { get; set; }
    public RuleType ruleType { get; }
    public IEnumerator ExecuteRule();
    public IEnumerator ExecuteRule(int turnId, int playerId);
}
