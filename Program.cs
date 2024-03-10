using System.Text.Json;

RulesConfiguration rulesConf = new RulesConfiguration();

Dictionary<string, RulesConfiguration.MappingDefinition> mappings = new Dictionary<string, RulesConfiguration.MappingDefinition>();


foreach (string fname in Directory.GetFiles("rules_conf/mapping_defs"))
{
    String json = File.ReadAllText(fname);


    SingleMappingDefinition mappingDefinition =
     JsonSerializer.Deserialize<SingleMappingDefinition>(json);

    mappings.Add(mappingDefinition.Name, mappingDefinition.mappingDefinition);


}





rulesConf.Mappings = mappings;





Dictionary<string, List<RulesConfiguration.Rule>> rules = new Dictionary<string, List<RulesConfiguration.Rule>>();








foreach (string fname in Directory.GetFiles("rules_conf/rules"))
{
    String json = File.ReadAllText(fname);


    SingleRule singleRule =
     JsonSerializer.Deserialize<SingleRule>(json);


    if (!rules.ContainsKey(singleRule.Name))
    {


        List<RulesConfiguration.Rule> newlist = new List<RulesConfiguration.Rule>()
        ;
        rules[singleRule.Name] = newlist;


    }


    rules[singleRule.Name].Add(singleRule.rule);




}






rulesConf.Rules = rules;

rulesConf.Version = "1.0";












// validate the serialization



#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
RulesConfiguration rf2 = JsonSerializer.Deserialize<RulesConfiguration>(JsonSerializer.Serialize(rulesConf));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.



Console
.WriteLine(
    JsonSerializer.Serialize(rulesConf)
//string.Compare(JsonSerializer.Serialize(rulesConf), JsonSerializer.Serialize(rf2)






);



// use case for the rules




#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable CS8604 // Possible null reference argument.
EventProcessor ep = new EventProcessor(rf2);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore IDE0090 // Use 'new(...)'

{
    Event ev = new Event();

    ev.EventName = "input_event_type_x2";

    ev.Properties = new Dictionary<string, string>();

    ev.Properties.Add("field1", "value1");

    ev.Properties.Add("field2", "value2");



    List<Event> events = ep.process(ev);

    Console.WriteLine(JsonSerializer.Serialize(events));
}


{
    Event ev = new Event();

    ev.EventName = "input_event_type_x";

    ev.Properties = new Dictionary<string, string>();

    ev.Properties.Add("field1", "value123");

    ev.Properties.Add("field2", "value2");



    List<Event> events = ep.process(ev);

    Console.WriteLine(JsonSerializer.Serialize(events));
}



{
    Event ev = new Event();

    ev.EventName = "input_event_type_x2";

    ev.Properties = new Dictionary<string, string>();

    ev.Properties.Add("field1", "value1");

    ev.Properties.Add("field2", "value2");



    List<Event> events = ep.process(ev);

    Console.WriteLine(JsonSerializer.Serialize(events));
}