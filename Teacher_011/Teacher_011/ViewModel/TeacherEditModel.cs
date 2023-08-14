using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Teacher_011.Models;

namespace Teacher_011.ViewModel
{
    public class TeacherEditModel
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
        [Required]
        public HttpPostedFileBase Picture { get; set; }
        public bool IsReadyToTeachAnySubject { get; set; }
        public virtual List<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
