namespace CSDBPortal.ViewModels
{
    public class TaskViewModel
    {
        public List<TaskDmcItem> AssignedDmcs { get; set; } = new();
        public List<TaskIcnItem> AssignedIcns { get; set; } = new();
    }

    public class TaskDmcItem
    {
        public int Id { get; set; }
        public string? DMC { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string? CheckoutStatus { get; set; }
        public string? CheckedOutBy { get; set; }
        public DateTime? CheckedOutOn { get; set; }
        public bool HasXml { get; set; }
    }

    public class TaskIcnItem
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public string? CheckedInBy { get; set; }
        public DateTime? CheckedInOn { get; set; }
    }
}
