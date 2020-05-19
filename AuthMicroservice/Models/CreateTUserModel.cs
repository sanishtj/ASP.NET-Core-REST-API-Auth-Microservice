namespace AuthMicroservice.Models
{
    public class CreateTUserModel : CreateUserBaseModel
    {
        public string[] Roles { get; set; }
    }
}
