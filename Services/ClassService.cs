﻿using DocumentFormat.OpenXml.VariantTypes;
using IMS.Models;
using NuGet.DependencyResolver;

namespace IMS.Services
{
    public class ClassService : IClassService
    {
        private readonly IMSContext _context;
        public ClassService(IMSContext context)
        {
            _context = context;
        }
        public bool ClassExist(string className)
        {
            var user = _context.Classes.FirstOrDefault(c => c.Name == className);
            if (user != null) return true; else return false;
            
        }
        public IEnumerable<Assignment> GetAssignments(int subjectid)
        {
            var assignments = _context.Assignments
       .Where(a => a.SubjectId == subjectid)
       .ToList();
            return assignments;
        }
        public bool AddStudentToClass(int classId, string email)
        {
            Class @class = _context.Classes.Include(c => c.Students).FirstOrDefault(c=> c.Id ==classId);
            var student = _context.Users.FirstOrDefault(u=> u.Email == email);

            if (@class == null || student == null)
            {
                return false; 
            }

            if (@class.Students.Contains(student))
            {
                return false;
            }

            @class.Students.Add(student);

            _context.SaveChanges();

            return true; 
        }
        public bool RemoveStudentFromClass(int classId, string email)
        {
            var @class = _context.Classes.Include(c=>c.Students).FirstOrDefault(c => c.Id == classId);
            var student = _context.Users.FirstOrDefault(u => u.Email == email);

         

            @class.Students.Remove(student);

            _context.SaveChanges();

            return true; 
        }

        public List<Milestone> GetMilestone(int id)
        {
            var milestone = _context.Milestones.Where(m => m.ClassId == id && m.ProjectId == null).ToList();
            return milestone;
        }

        public List<Milestone> GetMilestoneByProject(int id)
        {
            Project p = _context.Projects.FirstOrDefault(p => p.Id == id);
            List<Milestone> milestone = _context.Milestones.Where(m => m.ProjectId == id).ToList();
            milestone.AddRange(_context.Milestones.Where(m => m.ClassId == p.ClassId && m.ProjectId == null).ToList());
            return milestone;
        }

        public IEnumerable<Class> GetClasses()
        {
            return _context.Classes.Include(c => c.Students).Include(c => c.Teacher).Include(c => c.Subject).ToList();
        }
        public IEnumerable<User> GetAllTeachers()
        {
          
            var teachers = _context.Classes
                .Include(c => c.Teacher) 
                .Where(c => c.Teacher != null)
                .Select(c => c.Teacher)
                .Distinct()
                .ToList();

            return teachers;
        }
        public Class GetClass(int id)
        {
            return _context.Classes.Include(c => c.Subject).Include(c => c.Students).Include(c => c.Teacher).Include(c => c.Milestones).FirstOrDefault(c => c.Id == id);
        }
        public IEnumerable<User> GetStudent(int classId) 
        {
            var students = _context.Classes.Where(c => c.Id == classId).SelectMany(c => c.Students).ToList();
            return students;
        }
        public int GetTotalStudentsInClass(int classId)
        {
            var classItem = _context.Classes.Include(c => c.Students).FirstOrDefault(c => c.Id == classId);
            if (classItem != null)
            {
                return classItem.Students.Count();
            }
            return 0;
        }
        public int GetTeacherIdByNameAndEmail(string name)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == name || u.Email == name);
            if (user != null) { return user.Id; }
            else { return 0; }
        }
        public int GetSubjectId(string name)
        {
            var subject = _context.Subjects.FirstOrDefault(u => u.Name == name);
            return subject != null ? subject.Id : 0;
        }
        public void AddClass(Class Class) 
        {
            try
            {
                _context.Classes.Add(Class);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public void UpdateCLass(Class Class)
        {
            try
            {
                _context.Update(Class);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public User GetTeacherById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
        public Subject GetSubjectById(int id)
        {
            return _context.Subjects.FirstOrDefault(s => s.Id == id);
        }
        public List<Subject> GetSubjects()
        {
            return _context.Subjects.ToList();
        }

        public IEnumerable<Class> GetClassesByStudent(int studentId)
        {
            return _context.Users.Include(u => u.ClassesNavigation).FirstOrDefault(u => u.Id == studentId).ClassesNavigation;
        }

        public IEnumerable<Class> GetClassesByTeacher(int teacherId)
        {
            return _context.Users.Include(u => u.Classes).ThenInclude(c => c.Students).FirstOrDefault(u => u.Id == teacherId).Classes;
        }

        public List<User> GetStudentInClass(int classId)
        {
            List<User> user = new List<User>();
            user = _context.Classes.Include(c => c.Students).FirstOrDefault(c => c.Id == classId).Students.ToList();
            return user;
        }
    }
}
            
        
    
