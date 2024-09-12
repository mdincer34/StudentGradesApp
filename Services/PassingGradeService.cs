using StudentGradesApp.Services;

public class PassingGradeService : IPassingGradeService
{
    private static PassingGradeService? _instance;
    private static readonly object _lock = new object();
    private int _passingGrade = 60;
    private PassingGradeService() { }

    public static PassingGradeService Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new PassingGradeService();
                    }
                }
            }
            return _instance;
        }
    }

    public int GetPassingGrade() => _passingGrade;

    public void SetPassingGrade(int passingGrade)
    {
        _passingGrade = passingGrade;
    }
}