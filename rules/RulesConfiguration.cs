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

            public MappingAction mappingAction { get; set; }

            public List<string> details { get; set; }


        }
        public Dictionary<string, MappingActionDetails> mappings { get; set; }






    }


    public class Rule
    {


        //each key is a field name, each value is a target value for the field to be met
        //if all fields are met, the action is executed
        public Dictionary<string, string> condition { get; set; }

        public enum Action
        {

            CreateSubEvent,
            MPEvent,


        }

        public Action action { get; set; }


        //if action is createSubEvent, this is the name of the subevent to create
        //if action is mpEvent, this is the name of mapping definition to use

        public string actionOperand { get; set; }




    }

    public Dictionary<string, List<Rule>> rules { get; set; }

    public Dictionary<string, MappingDefinition> mappings { get; set; }


    public string version { get; set; }
}