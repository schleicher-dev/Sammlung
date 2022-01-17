namespace Sammlung.CommandLine.SourceGenerator.Execution
{
    public class ResultCode
    {
        public static implicit operator int(ResultCode code) => code.Value;
        
        
        public static readonly ResultCode RegularTermination = 
            new ResultCode(0, "Application terminated regularly", true);

        public bool DenotesSuccess { get; }
        public int Value { get; }
        public string Description { get; }

        private ResultCode(int value, string description, bool denotesSuccess)
        {
            Value = value;
            Description = description;
            DenotesSuccess = denotesSuccess;
        }

        public override string ToString() => $"[ResultCode: {Description}  {Value}]";
    }
}