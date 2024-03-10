// See https://aka.ms/new-console-template for more information
using System.Text.Json;

//Console.WriteLine("Hello, World!");


// define rules

RulesConfiguration rulesConf = new RulesConfiguration();

Dictionary<string, RulesConfiguration.MappingDefinition> mappings = new Dictionary<string, RulesConfiguration.MappingDefinition>();


RulesConfiguration.MappingDefinition md = new RulesConfiguration.MappingDefinition();


md.mappings = new Dictionary<string, RulesConfiguration.MappingDefinition.MappingActionDetails>();


RulesConfiguration.MappingDefinition.MappingActionDetails fm1 = new RulesConfiguration.MappingDefinition.MappingActionDetails();


fm1.mappingAction = RulesConfiguration.MappingDefinition.MappingAction.CALCULATE;

fm1.details = new List<string> { "CALC_FUNCTION_X", "field2" };

md.mappings.Add("fieldmapping1", fm1);






mappings.Add("mappingdef1", md);


rulesConf.mappings = mappings;





Dictionary<string, List<RulesConfiguration.Rule>> rules = new Dictionary<string, List<RulesConfiguration.Rule>>();

List<RulesConfiguration.Rule> rs = new List<RulesConfiguration.Rule>();


RulesConfiguration.Rule rule = new RulesConfiguration.Rule();

rule.condition = new Dictionary<string, string>();

rule.condition.Add("field1", "value1");





rule.action = RulesConfiguration.Rule.Action.MPEvent;

rule.actionOperand = "mappingdef1";

rs.Add(rule);









rules.Add("input_event_type_x", rs);




rulesConf.rules = rules;

rulesConf.version = "1.0";












// validate the serialization



RulesConfiguration rf2 = JsonSerializer.Deserialize<RulesConfiguration>(JsonSerializer.Serialize(rulesConf));



Console
.WriteLine(string.Compare(JsonSerializer.Serialize(rulesConf), JsonSerializer.Serialize(rf2))



);



// use case for the rules




EventProcessor ep = new EventProcessor(rf2);

{
    Event ev = new Event();

ev.eventName = "input_event_type_x";

ev.properties = new Dictionary<string, string>();

ev.properties.Add("field1", "value1");

ev.properties.Add("field2", "value2");



List<Event> events  = ep.process(ev);

Console.WriteLine(JsonSerializer.Serialize(events));
}


{
    Event ev = new Event();

ev.eventName = "input_event_type_x";

ev.properties = new Dictionary<string, string>();

ev.properties.Add("field1", "value123");

ev.properties.Add("field2", "value2");



List<Event> events  = ep.process(ev);

Console.WriteLine(JsonSerializer.Serialize(events));
}



{
    Event ev = new Event();

ev.eventName = "input_event_type_x2";

ev.properties = new Dictionary<string, string>();

ev.properties.Add("field1", "value1");

ev.properties.Add("field2", "value2");



List<Event> events  = ep.process(ev);

Console.WriteLine(JsonSerializer.Serialize(events));
}