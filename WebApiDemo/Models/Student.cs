namespace WebApiDemo.Models
{
    public class Student
    {
        public int Studentid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedTime { get; set; }
    }
}
