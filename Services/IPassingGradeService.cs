namespace StudentGradesApp.Services
{
    public interface IPassingGradeService
    {
        int GetPassingGrade();
        void SetPassingGrade(int grade);
    }
}