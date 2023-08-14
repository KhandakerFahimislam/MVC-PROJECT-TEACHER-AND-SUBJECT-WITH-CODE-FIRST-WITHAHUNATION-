using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Teacher_011.Models
{

    public enum Gender { Male = 1, Female }
    public class Teacher
    {
        public int TeacherId { get; set; }
        [Required, StringLength(40)]
        public string TeacherName { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        [Required, StringLength(30)]
        public string PreferSubject { get; set; }
        [Required, StringLength(30)]
        public string Picture { get; set; }
        public bool IsReadyToTeachAnySubject { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
    public class Subject
    {
        public int SubjectId { get; set; }
        [Required, StringLength(30)]
        public string SubjectName { get; set; }
        [Required, StringLength(40)]
        public string SubjcetTopic { get; set; }
        [Required]
        public int NumberOfTopic { get; set; }
        [Required, StringLength(20)]
        public string TeachingAbility { get; set; }
        [Required, ForeignKey("Teacher")]
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
    public class TeacherDbContext : DbContext
    {
        public TeacherDbContext()
        {
            Database.SetInitializer(new DbInitializer());
        }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
    }
    public class DbInitializer : DropCreateDatabaseIfModelChanges<TeacherDbContext>
    {
        protected override void Seed(TeacherDbContext context)
        {
            Teacher a = new Teacher { TeacherName = "A1", PreferSubject = "C#", BirthDate = DateTime.Parse("1976-07-11"), Gender = Gender.Male, IsReadyToTeachAnySubject = true, Picture = "e1.jpg" };
            a.Subjects.Add(new Subject { SubjectName = "SQL", SubjcetTopic = "DATABASE", NumberOfTopic = 5, TeachingAbility = "GOOD" });
            a.Subjects.Add(new Subject { SubjectName = "MVC", SubjcetTopic = "MVCCORE", NumberOfTopic = 4, TeachingAbility = "Excelent" });
            context.Teachers.Add(a);
            context.SaveChanges();
        }
    }
}