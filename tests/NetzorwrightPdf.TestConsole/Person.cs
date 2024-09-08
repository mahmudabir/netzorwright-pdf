namespace NetzorwrightPdf.TestConsole;
public class Person
{

    public Person(string name, int age, Gender gender)
    {
        Name = name;
        Age = age;
        Gender = gender;
    }

    public string Name { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
}

public enum Gender
{
    Other = 0,
    Male = 1,
    Female = 2,
}
