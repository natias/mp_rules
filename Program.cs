using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        RulesConfiguration rulesConf = new();

        Dictionary<string, RulesConfiguration.MappingDefinition> mappings = new();


        foreach (string fname in Directory.GetFiles("rules_conf/mapping_defs"))
        {
            string json = File.ReadAllText(fname);


            SingleMappingDefinition? mappingDefinition =
             JsonSerializer.Deserialize<SingleMappingDefinition>(json);

            if (null != mappingDefinition)

                mappings.Add(mappingDefinition!.Name, mappingDefinition.mappingDefinition);


        }





        rulesConf.Mappings = mappings;





        Dictionary<string, List<RulesConfiguration.Rule>> rules = new();








        foreach (string fname in Directory.GetFiles("rules_conf/rules"))
        {
            string json = File.ReadAllText(fname);


            SingleRule? singleRule =
             JsonSerializer.Deserialize<SingleRule>(json);


            if (!rules.ContainsKey(singleRule!.Name))
            {


                List<RulesConfiguration.Rule> newlist = new()
                ;
                rules[singleRule.Name] = newlist;


            }


            rules[singleRule.Name].Add(singleRule.rule);




        }






        rulesConf.Rules = rules;

        rulesConf.Version = "1.0";












        // validate the serialization



        RulesConfiguration rf2 = JsonSerializer.Deserialize<RulesConfiguration>(JsonSerializer.Serialize(rulesConf));



        Console
        .WriteLine(
            JsonSerializer.Serialize(rulesConf)






        );



        // use case for the rules




        EventProcessor ep = new EventProcessor(rf2);




        {
            Event ev = new();

            ev.EventName = "input_event_type_x2";

            ev.Properties = new Dictionary<string, string>();

            ev.Properties.Add("field1", "value1");

            ev.Properties.Add("field2", "value2");

            ev.Properties.Add("f2", "value2222");

            ev.Properties.Add("f3", "value3333");

            ev.Properties.Add("f4", "value4444");


            List<Event> events = ep.Process(ev);

            Console.WriteLine("out_events {0}", JsonSerializer.Serialize(events));
        }
    }
}