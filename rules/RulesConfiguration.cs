public class RulesConfiguration
{


    public class MappingDefinition
    {

        public enum MappingAction
        {
            COPY,
            TOKENIZE,

            CALCULATE
        }

        public class MappingActionDetails
        {

            public MappingAction MappingAction { get; set; }

            public List<string>? Details { get; set; }


        }
        public Dictionary<string, MappingActionDetails>? Mappings { get; set; }






    }


    public class Rule
    {


        //each key is a field name, each value is a target value for the field to be met
        //if all fields are met, the action is executed
        public Dictionary<string, string>? Condition { get; set; }

        public enum ActionEnum
        {

            CreateSubEvent,
            MPEvent,


        }

        public ActionEnum Action { get; set; } 



        //if action is createSubEvent, this is the name of the subevent to create
        //if action is mpEvent, this is the name of mapping definition to use

        public string? ActionOperand { get; set; }




    }

    public Dictionary<string, List<Rule>>? Rules { get; set; }=default;

    public Dictionary<string, MappingDefinition>? Mappings { get; set; }=default ;




    public string? Version { get; set; }=default;
}