namespace BestChoice.API.Dtos
{
    public class UserEditDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string CurrentRole { get; set; }
        public string NewRole { get; set; }
    }
}
