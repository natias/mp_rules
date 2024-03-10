public class SingleMappingDefinition {


    public string Name {get; set;}


    public RulesConfiguration.MappingDefinition mappingDefinition {get; set;}

    public SingleMappingDefinition (string name, RulesConfiguration.MappingDefinition mappingDefinition){
        this.Name = name;
        this.mappingDefinition = mappingDefinition;
    }
    
} 