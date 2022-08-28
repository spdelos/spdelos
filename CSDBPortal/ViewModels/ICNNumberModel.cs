namespace CSDBPortal.ViewModels
{
    public enum ICNNumberGenerateBy
    {
        SEQUENCENUMBER = 0,
        VARCODE = 1,
        ISSUENO = 2
    }

    public class ICNNumberModel
    {
        public int ProjectId { get; set; }
        public int Number { get; set; }
    }

    public class ICNumberGenerationModel
    {
        public int ProjectId { get; set; }
        public int Count { get; set; }
        public int SequenceNumber { get; set; }
        public string VarCode { get; set; }
        public ICNNumberGenerateBy GenerateBy { get; set; }
    }
}
