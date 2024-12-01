namespace Digital_Archive.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public bool CanOpen { get; set; }
        public bool CanRead { get; set; }
        public bool CanEdit { get; set; }

        public int? FoldedId { get; set; }
        public int? SectionId { get; set; }

        public int? UserId { get; set; }
        public int? RoleId { get; set; }

    }
}
