namespace APITechTest.Players
{
    // Parameters for RegisterPlayer endpoint
    public class RegisterPlayerParameters
    {
        // Required
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string Birthdate { get; set; }

        // Optional
        public int? Points { get; set; }
        public int? Games { get; set; }
    }
}
