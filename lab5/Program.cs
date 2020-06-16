using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System;
using Firma;

namespace lab5
{

    class XmlWorker
    {
        public void AddWorkers()
        {

            XDocument document = new XDocument(new XElement("Workers",
                new XElement("Worker",
                   new XAttribute("Basic_number", "1"),
                   new XElement("Name", "Salabay Kostya"),
                   new XElement("Education", "Hight"),
                   new XElement("Speciality", "Software Engineering"),
                   new XElement("Registration_number", "291120000"),
                   new XElement("Date_of_commencement", "01/02/2020")),

                new XElement("Worker",
                   new XAttribute("Basic_number", "2"),
                   new XElement("Name", "Maksym Chornyi"),
                   new XElement("Education", "Hight"),
                   new XElement("Speciality", "Computer Science"),
                   new XElement("Registration_number", "131020012"),
                   new XElement("Date_of_commencement", "01/12/2019")),

                new XElement("Worker",
                   new XAttribute("Basic_number", "3"),
                   new XElement("Name", "Pasha Panchenko"),
                   new XElement("Education", "Incomplete high"),
                   new XElement("Speciality", "Cybersecurity"),
                   new XElement("Registration_number", "231019937"),
                   new XElement("Date_of_commencement", "01/03/2020")),

                new XElement("Worker",
                   new XAttribute("Basic_number", "4"),
                   new XElement("Name", "Olga Skys"),
                   new XElement("Education", "Hight"),
                   new XElement("Speciality", "Computer Science"),
                   new XElement("Registration_number", "250519962"),
                   new XElement("Date_of_commencement", "01/02/2020")),

                new XElement("Worker",
                   new XAttribute("Basic_number", "5"),
                   new XElement("Name", "Katia Orlova"),
                   new XElement("Education", "Secondary"),
                   new XElement("Speciality", "Software Engineering"),
                   new XElement("Registration_number", "270719952"),
                   new XElement("Date_of_commencement", "01/01/2020"))));

            document.Save("Workers.xml");
        }

        public void SerializeXml()
        {
            List<Salary> salary = new List<Salary>()
            {
                new Salary { Id_Registration_number = "291120000", Date_Salary="01/03/2020", Worker_Salary= "20000" },
                new Salary { Id_Registration_number = "291120000", Date_Salary="01/04/2020", Worker_Salary= "15000" },
                new Salary { Id_Registration_number = "131020012", Date_Salary="01/01/2020", Worker_Salary= "20000" },
                new Salary { Id_Registration_number = "131020012", Date_Salary="01/02/2020", Worker_Salary= "17000" },
                new Salary { Id_Registration_number = "231019937", Date_Salary="01/04/2020", Worker_Salary= "25000" },
                new Salary { Id_Registration_number = "231019937", Date_Salary="01/05/2020", Worker_Salary= "25000" },
                new Salary { Id_Registration_number = "250519962", Date_Salary="01/03/2020", Worker_Salary= "21000" },
                new Salary { Id_Registration_number = "270719952", Date_Salary="01/02/2020", Worker_Salary= "19000" }
            };

            XmlSerializer xml = new XmlSerializer(typeof(List<Salary>));
            using (FileStream file = new FileStream("Salary.xml", FileMode.OpenOrCreate))
            {
                xml.Serialize(file, salary);
            }
        }

        public List<Salary> DeserializeXml()
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Salary>));

            using (FileStream file = new FileStream("Salary.xml", FileMode.OpenOrCreate))
            {
                return (List<Salary>)xml.Deserialize(file);
            }
        }

        public void Queryes()
        {
            XDocument workers = XDocument.Load("Workers.xml");
            XDocument salary = XDocument.Load("Salary.xml");

            /*Query 1: easy select*/
            Console.WriteLine("Query 1: easy select\n");

            foreach (XElement s in salary.Element("ArrayOfSalary").Elements("Salary"))
            {
                XElement Id_Registration_number1 = s.Element("Id_Registration_number");
                XElement Date_Salary1 = s.Element("Date_Salary");
                XElement Worker_Salary1 = s.Element("Worker_Salary");

                if (Id_Registration_number1 != null && Date_Salary1 != null && Worker_Salary1 != null)
                {
                    Console.WriteLine($"Id рабочего: {Id_Registration_number1.Value}");
                    Console.WriteLine($"Дата зарплаты: {Date_Salary1.Value}");
                    Console.WriteLine($"Зарплата: {Worker_Salary1.Value}грн");
                }
                Console.WriteLine("\n----------------------------\n");
            }

            /*Query 2: select worker with hight eduacation*/
            Console.WriteLine("Query 2: select worker with hight eduacation\n");

            var query2 = from worker1 in workers.Element("Workers").Elements("Worker")
                         where worker1.Element("Education").Value == "Hight"
                         select new Worker
                         {
                             Basic_number=worker1.Attribute("Basic_number").Value,
                             Name=worker1.Element("Name").Value
                         };
               
            foreach(var item in query2)
            {
                Console.WriteLine($"{item.Basic_number}-{item.Name}");
            }

            /*Query 3: select salaries (with dates) of the worker 'Kostya Salabay'*/
            Console.WriteLine(" Query 3: select salaries (with dates) of the worker 'Kostya Salabay'.\n");

            var query3 = from p in workers.Element("Workers").Elements("Worker")
                         join s in salary.Element("ArrayOfSalary").Elements("Salary")
                         on p.Elements("Registration_number") equals s.Elements("Id_Registration_number")
                            where p.Element("Name").Value == "Kostya Salabay" 
                            select new
                            {
                                Salary = s.Element("Worker_Salary").Value,
                                Data = s.Element("Date_Salary").Value
                            };

            /*Query 4: select workers whose salaries in January are more than 20,000*/
            Console.WriteLine(" Query 4: select workers whose salaries in January are more than 20,000.\n");

            var query4 = from p in workers.Element("Workers").Elements("Worker")
                         join s in salary.Element("ArrayOfSalary").Elements("Salary") 
                         on p.Elements("Registration_number") equals s.Elements("Id_Registration_number")
                         where s.Element("Date_Salary").Value == "01/02/2020" && int.Parse(s.Element("Worker_Salary").Value) > 20000
                            select new
                            {
                                Name = p.Element("Name").Value,                                
                                Salary = s.Element("Worker_Salary").Value
                            };

            /*Query 5: group employees by education. */
            Console.WriteLine(" Query 5: group employees by education. .\n");

            var query5 = from p in workers.Element("Workers").Elements("Worker")
                            group p by p.Element("Education").Value;


            foreach (IGrouping<string, Worker> g in query5)
            {
                Console.WriteLine(g.Key);
                foreach (var t in g)
                    Console.WriteLine($"{t.Name}");
                Console.WriteLine();
            }

            /*Query 6: select average salary for workers with a degree in Software Engineering. */
            Console.WriteLine(" Query 6: select average salary for workers with a degree in Software Engineering.\n");

            var query6 = from p in workers.Element("Workers").Elements("Worker")
                         join s in salary.Element("ArrayOfSalary").Elements("Salary") 
                         on p.Elements("Registration_number") equals s.Elements("Id_Registration_number")
                         where p.Element("Specialty").Value == "Software Engineering"
                             select new
                             {
                                 Avg_salary = salary.Element("ArrayOfSalary").Elements("Salary").Average(n => int.Parse(n.Element("Worker_Salary").Value))
                             };

            foreach (var item in query6)
            {
                Console.WriteLine($"Cредняя зарплата работников со специальностью 'Software Engineering': {item.Avg_salary}грн");
                break;
            }

            /*Query 7: display the names of specialties(without repetition. */
            Console.WriteLine(" Query 7: display the names of specialties(without repetition.\n");

            var query7 = workers.Element("Workers").Elements("Worker").Select(u => u.Element("Specialty").Value).Distinct();

            foreach (var item in query7)
                Console.WriteLine($"{item}");
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            XmlWorker xmlWorker = new XmlWorker();

            xmlWorker.AddWorkers();
            xmlWorker.SerializeXml();
            List<Salary> salary = xmlWorker.DeserializeXml();
            xmlWorker.Queryes();

            Console.ReadKey();
        }
    }
}
