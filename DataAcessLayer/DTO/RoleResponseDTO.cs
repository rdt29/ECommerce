namespace DataAcessLayer.DTO
{
    public class RoleResponseDTO
    {
        public int ID { get; set; }
        public string RoleName { get; set; }

        public List<UserRoleDTO> users { get; set; }
    }
}