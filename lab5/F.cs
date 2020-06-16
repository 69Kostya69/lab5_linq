using System;
using System.Collections.Generic;
using System.Text;

namespace Firma
{

    public class Worker
    {
        public string Name { get; set; }
        public string Basic_number { get; set; }
        public string Registration_number { get; set; }
        public string Education { get; set; }
        public string Specialty { get; set; }
        public string Date_of_commencement { get; set; }

        public Worker() { }
        public Worker(string name, string basic_number,
                        string registration_number, string education, string specialty, string date_of_commencement)
        {
            Name = name;           
            Basic_number = basic_number;
            Registration_number = registration_number;
            Education = education;
            Specialty = specialty;
            Date_of_commencement = date_of_commencement;
        }
    }

    public class Salary
    {
        public string Id_Registration_number { get; set; }
        public string Date_Salary { get; set; }
        public string Worker_Salary { get; set; }

        public Salary() { }
        public Salary(string id_Registration_number, string date_Salary, string worker_Salary)
        {
            Id_Registration_number = id_Registration_number;
            Date_Salary = date_Salary;
            Worker_Salary = worker_Salary;
        }
    }
    
}
