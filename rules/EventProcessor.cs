

//service

public class EventProcessor
{




    string tokenize(Dictionary<string, string> properties, List<string> details)
    {
        throw new NotImplementedException();
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

        string en = @event.eventName;


        if (!rulesConfigurtion.rules.ContainsKey(en))
        {
            Console.WriteLine("no rules for event {0}", en);
        //    genaratedEvents.Add(@event);
            return genaratedEvents;
        }


        List<RulesConfiguration.Rule> rulz = rulesConfigurtion.rules[en];


        bool isContionMet = true;
        rulz.ForEach(rule =>
        {


            foreach (KeyValuePair<string, string> condition in rule.condition)
            {

                string fieldName = condition.Key;
                string fieldValue = condition.Value;


                if (@event.properties.ContainsKey(fieldName) && @event.properties[fieldName] == fieldValue)
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
                RulesConfiguration.Rule.Action a = rule.action;


                switch (a)
                {
                    case RulesConfiguration.Rule.Action.CreateSubEvent:
                        {
                            Event newEvent = new Event();
                            newEvent.eventName = rule.actionOperand;
                            //newEvent.properties = new Dictionary<string, string>();
                            newEvent.properties = @event.properties;




                            genaratedEvents.AddRange(process(newEvent));



                            break;
                        }

                    case RulesConfiguration.Rule.Action.MPEvent:
                        {


                            Event newEvent = new Event();

                            newEvent.eventName = rule.actionOperand;


                            newEvent.properties = new Dictionary<string, string>();




                            RulesConfiguration.MappingDefinition mp = rulesConfigurtion.mappings[rule.actionOperand];

                            foreach (KeyValuePair<string, RulesConfiguration.MappingDefinition.MappingActionDetails> mapping in mp.mappings)
                            {




                                string fieldName = mapping.Key;


                                RulesConfiguration.MappingDefinition.MappingActionDetails mappingdetails = mapping.Value;

                                string value = "";


                                switch (mappingdetails.mappingAction)
                                {

                                    case RulesConfiguration.MappingDefinition.MappingAction.COPY:
                                        {

                                            value = @event.properties[mappingdetails.details[0]];
                                        }
                                        break;
                                    case RulesConfiguration.MappingDefinition.MappingAction.TOKENIZE:
                                        {

                                            value = tokenize(@event.properties, mappingdetails.details);
                                        }
                                        break;


                                        case RulesConfiguration.MappingDefinition.MappingAction.CALCULATE:
                                        {

                                            value = calculate(@event.properties, mappingdetails.details);
                                        }
                                        break;

                                }



                                newEvent.properties.Add(fieldName, value);





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
                Console.WriteLine("condition not met for event {0}  rule {1}   ", @event.eventName, rule.condition);
            }





        });

        return genaratedEvents;
    }

    private string calculate(Dictionary<string, string> properties, List<string> details)
    {

        Console.WriteLine("calculating properties {0} details {1}",properties.ToString(),details.ToString());

        return "123";
        //throw new NotImplementedException();
    }
}