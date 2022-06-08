namespace CSDBPortal.ViewModels
{
    public class ICNViewModel
    {
       public  List<ProjectDropDown> ProjectList { set; get; }   
    }

    public class ProjectDropDown
    {
        public int ProjecctId { get; set; }
        public string Name { get; set; }
    }
}
