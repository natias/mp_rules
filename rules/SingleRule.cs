public class SingleRule{


    public string Name {get; set;}


    public RulesConfiguration.Rule rule {get; set;}

    public SingleRule(string name, RulesConfiguration.Rule rule){
        this.Name = name;
        this.rule = rule;
    }
    
} 