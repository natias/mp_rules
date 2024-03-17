

//service

public class EventProcessor
{





    private string calculate(Dictionary<string, string> properties, List<string> details)
    {

        Console.WriteLine("calculating properties {0} details {1}",
        
        string.Join(",", properties.Select(kv => kv.Key + "=" + kv.Value).ToArray()) 
        ,
        
        string.Join( ",", details.ToArray() )
        
        );

        String func_name=details[0];

        switch(func_name){

            case "CONCAT":
        {

            
            
            string r="";
            for( int i =1 ;i< 
            details.Count; i++)
            {

                r=r+properties[details[i]];

            }

            return r;
        }
        }


        return "123";
        //throw new NotImplementedException();
    }

    string tokenize(Dictionary<string, string> properties, List<string> details)
    {

        Console.WriteLine("tokenize properties {0} details {1}",
        
        string.Join(",", properties.Select(kv => kv.Key + "=" + kv.Value).ToArray()) 
        ,
        
        string.Join( ",", details.ToArray() )
        
        );

        return "123";
        //throw new NotImplementedException();
    }
    RulesConfiguration rulesConfigurtion;

    public EventProcessor(RulesConfiguration rulesConfigurtion)
    {
        this.rulesConfigurtion = rulesConfigurtion;
    }

    //process events

 public   List<Event> Process(Event @event)
    {

        List<Event> genaratedEvents = new();

        string? en = @event.EventName;


        if (!rulesConfigurtion.Rules.ContainsKey(en))
        {
            Console.WriteLine("no rules for event {0}", en);
            return genaratedEvents;
        }


        List<RulesConfiguration.Rule> rulz = rulesConfigurtion.Rules[en];


        bool isContionMet = true;
        rulz.ForEach((Action<RulesConfiguration.Rule>)(rule =>
        {



            foreach (KeyValuePair<string, string> condition in rule.Condition)
            {

                string fieldName = condition.Key;
                string fieldValue = condition.Value;


                if (@event.Properties.ContainsKey(fieldName) && @event.Properties[fieldName] == fieldValue)
                {
                    //condition met
                    //isContionMet = true;

                }
                else
                {
                    isContionMet = false;
                }
            }

            if (isContionMet)
            {
                RulesConfiguration.Rule.ActionEnum a = rule.Action;


                switch (a)
                {
                    case RulesConfiguration.Rule.ActionEnum.CreateSubEvent:
                        {
                            Event newEvent = new();
                            newEvent.EventName = rule.ActionOperand;
                            //newEvent.properties = new Dictionary<string, string>();
                            newEvent.Properties = @event.Properties;


                            Console.WriteLine("creating sub event {0}  for event {1}",
                             newEvent.EventName, 
                             @event.EventName);


                            genaratedEvents.AddRange(Process(newEvent));



                            break;
                        }

                    case RulesConfiguration.Rule.ActionEnum.MPEvent:
                        {


                            Event newEvent = new();

                            newEvent.EventName = rule.ActionOperand;


                            newEvent.Properties = new Dictionary<string, string>();




                            RulesConfiguration.MappingDefinition mp = rulesConfigurtion.Mappings[rule.ActionOperand];

                            foreach (KeyValuePair<string, RulesConfiguration.MappingDefinition.MappingActionDetails> mapping in mp.Mappings)
                            {




                                string fieldName = mapping.Key;


                                RulesConfiguration.MappingDefinition.MappingActionDetails mappingdetails = mapping.Value;

                                string value = "";


                                switch (mappingdetails.MappingAction)
                                {

                                    case RulesConfiguration.MappingDefinition.MappingAction.COPY:
                                        {

                                            value = @event.Properties[mappingdetails.Details[0]];
                                        }
                                        break;
                                    case RulesConfiguration.MappingDefinition.MappingAction.TOKENIZE:
                                        {

                                            value = tokenize(@event.Properties, mappingdetails.Details);
                                        }
                                        break;


                                        case RulesConfiguration.MappingDefinition.MappingAction.CALCULATE:
                                        {

                                            value = calculate(@event.Properties, mappingdetails.Details);
                                        }
                                        break;

                                }



                                newEvent.Properties.Add(fieldName, value);





                            }




                            genaratedEvents.Add(newEvent);


                            break;
                        }
                }


                //create new event

                isContionMet = false;

            }
            else
            {
                Console.WriteLine("condition not met for event {0}  rule {1}   ", @event.EventName, rule.Condition);
            }





        }));

        return genaratedEvents;
    }

}