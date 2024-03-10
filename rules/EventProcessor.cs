

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

 public   List<Event> process(Event @event)
    {

        List<Event> genaratedEvents = new List<Event>();

        string? en = @event.EventName;


#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
        if (!rulesConfigurtion.Rules.ContainsKey(en))
        {
            Console.WriteLine("no rules for event {0}", en);
        //    genaratedEvents.Add(@event);
            return genaratedEvents;
        }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.


        List<RulesConfiguration.Rule> rulz = rulesConfigurtion.Rules[en];


        bool isContionMet = true;
        rulz.ForEach((Action<RulesConfiguration.Rule>)(rule =>
        {


#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (KeyValuePair<string, string> condition in rule.Condition)
            {

                string fieldName = condition.Key;
                string fieldValue = condition.Value;


#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (@event.Properties.ContainsKey(fieldName) && @event.Properties[fieldName] == fieldValue)
                {
                    //condition met
                    //isContionMet = true;

                }
                else
                {
                    isContionMet = false;
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            if (isContionMet)
            {
                RulesConfiguration.Rule.ActionEnum a = rule.Action;


                switch (a)
                {
                    case RulesConfiguration.Rule.ActionEnum.CreateSubEvent:
                        {
                            Event newEvent = new Event();
                            newEvent.EventName = rule.ActionOperand;
                            //newEvent.properties = new Dictionary<string, string>();
                            newEvent.Properties = @event.Properties;


                            Console.WriteLine("creating sub event {0}  for event {1}",
                             newEvent.EventName, 
                             @event.EventName);


                            genaratedEvents.AddRange(process(newEvent));



                            break;
                        }

                    case RulesConfiguration.Rule.ActionEnum.MPEvent:
                        {


                            Event newEvent = new Event();

                            newEvent.EventName = rule.ActionOperand;


                            newEvent.Properties = new Dictionary<string, string>();




#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                            RulesConfiguration.MappingDefinition mp = rulesConfigurtion.Mappings[rule.ActionOperand];
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                            foreach (KeyValuePair<string, RulesConfiguration.MappingDefinition.MappingActionDetails> mapping in mp.Mappings)
                            {




                                string fieldName = mapping.Key;


                                RulesConfiguration.MappingDefinition.MappingActionDetails mappingdetails = mapping.Value;

                                string value = "";


                                switch (mappingdetails.MappingAction)
                                {

                                    case RulesConfiguration.MappingDefinition.MappingAction.COPY:
                                        {

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                            value = @event.Properties[mappingdetails.Details[0]];
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                                        }
                                        break;
                                    case RulesConfiguration.MappingDefinition.MappingAction.TOKENIZE:
                                        {

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                                            value = tokenize(@event.Properties, mappingdetails.Details);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
                                        }
                                        break;


                                        case RulesConfiguration.MappingDefinition.MappingAction.CALCULATE:
                                        {

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                                            value = calculate(@event.Properties, mappingdetails.Details);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
                                        }
                                        break;

                                }



                                newEvent.Properties.Add(fieldName, value);





                            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.




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