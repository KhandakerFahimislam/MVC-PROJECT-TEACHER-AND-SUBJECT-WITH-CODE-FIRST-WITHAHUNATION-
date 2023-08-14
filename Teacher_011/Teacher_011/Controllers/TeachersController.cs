using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Teacher_011.Models;
using Teacher_011.ViewModel;
using X.PagedList;

namespace Teacher_011.Controllers
{
    [Authorize]
    public class TeachersController : Controller
    {
        private readonly TeacherDbContext db = new TeacherDbContext();
        // GET: Teachers
        
        public async Task<ActionResult> Index(int pg = 1)
        {
            // var data = await db.Teachers.OrderBy(a => a.TeacherId).ToPagedListAsync(pg, 5);
            var data = await db.Teachers.OrderBy(a => a.TeacherId).ToPagedListAsync(pg, 5);

            return View(data);
        }
        public ActionResult Create()
        {
            TeacherViewModel a = new TeacherViewModel();
            a.Subjects.Add(new Subject { });
            return View(a);

        }
        [HttpPost]
        public ActionResult Create(TeacherViewModel data, string act = "")
        {
            if (act == "add")
            {
                data.Subjects.Add(new Subject { });
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                data.Subjects.RemoveAt(index);
            }
            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    var a = new Teacher
                    {
                        TeacherName = data.TeacherName,
                        BirthDate = data.BirthDate,
                        PreferSubject = data.PreferSubject,
                        Gender = data.Gender,
                        IsReadyToTeachAnySubject = data.IsReadyToTeachAnySubject

                    };
                    string ext = Path.GetExtension(data.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Server.MapPath("~/Pictures/") + fileName;
                    data.Picture.SaveAs(savePath);
                    a.Picture = fileName;
                    foreach (var q in data.Subjects)
                    {
                        a.Subjects.Add(q);
                    }
                    db.Teachers.Add(a);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(data);
        }
        public ActionResult Edit(int id)
        {
            var a = db.Teachers
              .Select(x => new TeacherEditModel
              {
                  TeacherId = x.TeacherId,
                  TeacherName = x.TeacherName,
                  BirthDate = x.BirthDate,
                  Gender = x.Gender,
                  PreferSubject = x.PreferSubject,
                  IsReadyToTeachAnySubject = x.IsReadyToTeachAnySubject,
                  Subjects = x.Subjects.ToList()

              })
              .FirstOrDefault(x => x.TeacherId == id);
            ViewBag.CurrentPic = db.Teachers.First(x => x.TeacherId == id).Picture;

            return View(a);
        }
        [HttpPost]
        public ActionResult Edit(TeacherEditModel data, string act = "")
        {
            if (act == "add")
            {
                data.Subjects.Add(new Subject { });
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                data.Subjects.RemoveAt(index);
            }
            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    var a = db.Teachers.First(x => x.TeacherId == data.TeacherId);

                    a.TeacherName = data.TeacherName;
                    a.BirthDate = data.BirthDate;
                    a.PreferSubject = data.PreferSubject;
                    a.Gender = data.Gender;
                    a.IsReadyToTeachAnySubject = data.IsReadyToTeachAnySubject;


                    if (data.Picture != null)
                    {
                        string ext = Path.GetExtension(data.Picture.FileName);
                        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Server.MapPath("~/Pictures/") + fileName;
                        data.Picture.SaveAs(savePath);
                        a.Picture = fileName;
                    }
                    db.Subjects.RemoveRange(db.Subjects.Where(x => x.TeacherId == data.TeacherId).ToList());
                    foreach (var item in data.Subjects)
                    {
                        a.Subjects.Add(new Subject
                        {
                            TeacherId = data.TeacherId,
                            SubjectName = item.SubjectName,
                            SubjcetTopic = item.SubjcetTopic,
                            NumberOfTopic = item.NumberOfTopic,
                            TeachingAbility = item.TeachingAbility
                        });
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(data);
        }
        [HttpPost]

        public ActionResult Delete(int id)
        {
            var a = db.Teachers.FirstOrDefault(x => x.TeacherId == id);
            if (a == null) return new HttpNotFoundResult();
            db.Teachers.Remove(a);
            db.SaveChanges();
            return Json(new { success = true, id=id });
        }


        
    }
}
    