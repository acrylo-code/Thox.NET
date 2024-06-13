namespace Thox.Models.DataModels
{
    public class UserModelList
    {
        public List<UserModel>? UserModels { get; set; }
    }

    public class UserModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
