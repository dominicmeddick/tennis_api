namespace APITechTest.Match
{
    // Parameters for RegisterMatch endpoint
    public class RegisterMatchParameters
    {
        public string WinnerFirstName { get; set; }
        public string WinnerLastName { get; set; }

        public string LoserFirstName { get; set; }
        public string LoserLastName { get; set; }
    }
}
