namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Certificate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public ICollection<Advisor> Advisors { get; set; }
    }
}
