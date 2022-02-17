using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Homework_08_DepOfWorks
{
    struct Repository
    {
        #region Поля

        /// <summary>
        /// Список отделов.
        /// </summary>
        public List<Department> listDeps;

        /// <summary>
        /// Путь к файлу.
        /// </summary>
        private readonly string path;

        /// <summary>
        /// Рандом.
        /// </summary>
        private Random rand;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор репозитория.
        /// </summary>
        /// <param name="Path">Путь к хранению</param>
        public Repository(string Path)
        {
            this.listDeps = new List<Department>();
            this.path = Path;
            this.rand = new Random();
        }

        #endregion

        #region Методы

        /// <summary>
        /// Добавление нового отдела к общему списку.
        /// </summary>
        public void CreateDep()
        {
            Department dep = new Department($"Отдел_{listDeps.Count + 1}");
            listDeps.Add(dep);
        }

        /// <summary>
        /// Добавление сотрудника в отдел.
        /// </summary>
        public void AddWorker(int index)
        {
            Worker worker = CreateWorker(listDeps[index].Workers.Count + 1); /// Создание сотрудника с ID = 1.

            listDeps[index].Workers.Add(worker); /// Добавление сотрудника в указанный отдел.

            Console.Clear();
            PrintDep(listDeps[index]);
        }

        /// <summary>
        /// Создание рандомного списка сотрудников.
        /// </summary>
        /// <param name="amountWorkers">Число сотрудников.</param>
        public void RandomListWorkers(int index)
        {
            Console.Write("Введите нужное число сотрудников: ");

            int amountWorkers = Convert.ToInt32(Console.ReadLine());

            int id = listDeps[index].Workers.Count;

            if (id < 1_000_000 - amountWorkers)
                for (int i = 0; i < amountWorkers; i++)
                {
                    listDeps[index].Workers.Add(
                        new Worker(
                            id,
                            $"Имя_{id}",
                            $"Фамилия_{id}",
                            this.rand.Next(20, 51),
                            (uint)this.rand.Next(1000, 2001),
                            this.rand.Next(1, 11)));
                    id++;
                }
            else
                Console.WriteLine("Отдел заполнен!");
        }

        /// <summary>
        /// Добавление списка сотрудников, к уже существующему.
        /// </summary>
        /// <param name="amountWorkers">Число сотрудников.</param>
        public void AddListDeps(List<Department> tempDeps)
        {
            foreach (var dep in tempDeps)
                listDeps.Add(dep);
        }

        /// <summary>
        /// Интерфейс создания сотрудника.
        /// </summary>
        /// <param name="workers"></param>
        public Worker CreateWorker(int id)
        {
            Console.Write("Введите имя сотрудника: "); string firstName = Console.ReadLine();
            Console.Write("Введите фамилию сотрудника: "); string lastName = Console.ReadLine();
            Console.Write("Введите возраст сотрудника: "); int age = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите зарплату сотрудника: "); uint salary = Convert.ToUInt32(Console.ReadLine());
            Console.Write("Введите кол-во проектов сотрудника: "); int countProjects = Convert.ToInt32(Console.ReadLine());

            Worker worker = new Worker(id, firstName, lastName, age, salary, countProjects);

            return worker;
        }

        /// <summary>
        /// Удаление сотрудника.
        /// </summary>
        /// <param name="index"Индекс отдела></param>
        public void RemoveWorker(int index) => listDeps[index].Workers.RemoveAt(SelectIndex());

        /// <summary>
        /// Изменение данных сотрудника.
        /// </summary>
        /// <param name="indexDep"Индекс отдела></param>
        public void RemoveAndCreateWorker(int index)
        {
            RemoveWorker(index);
            CreateWorker(listDeps[index].Workers.Count + 1);
        }

        /// <summary>
        /// удаление отдела из общего списка.
        /// </summary>
        public void RemoveDep(int index) => listDeps.Remove(listDeps[index]);

        public void EditDep(int index)
        {
            char contin = 'y';

            while (contin != 'n')
            {
                Console.Clear();

                PrintDep(listDeps[index]);

                Console.WriteLine("\n[1] Добавить сотрудника в отдел.\n[2] Изменить запись о сотруднике.\n[3] Удалить запись о отруднике.\n[4] Заполнить отдел сотрудниками.\n[5] Сортировка.\n[6] Выйти\n");

                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        AddWorker(index);
                        break;
                    case 2:
                        RemoveAndCreateWorker(index);
                        break;
                    case 3:
                        RemoveWorker(index);
                        break;
                    case 4:
                        RandomListWorkers(index);
                        break;
                    case 5:
                        SortWorkers(index);
                        break;
                    default:
                        contin = 'n';
                        break;
                }
            }
        }

        /// <summary>
        /// Меню выбора индекса.
        /// </summary>
        /// <returns></returns>
        public int SelectIndex()
        {
            Console.Write("Введите индекс объекта:");

            int index = Convert.ToInt32(Console.ReadLine()) - 1;
            return index;
        }

        /// <summary>
        /// Вывод загаловков.
        /// </summary>
        /// <returns></returns>
        public void PrintTitlesWorkers()
        {
            string str = "ID:,Имя:,Фамилия:,Возраст:,Зарплата:,Количество проектов:";
            string[] titles = str.Split(',');

            Console.WriteLine($"{titles[0]} {titles[1],15} {titles[2],20} {titles[3],20} {titles[4],15} {titles[5],30}\n");
        }

        /// <summary>
        /// Вывод данных об отделе.
        /// </summary>
        /// <param name="dep"></param>
        public void PrintTitlesDeps(Department dep)
        {
            Console.Write($"{dep.Name}   ");
            Console.Write($"Проектов в отделе: {dep.CountProjects}   ");
            Console.Write($"Дата создания отдела: {dep.Date}   \n");
        }

        /// <summary>
        /// Вывод на экран информации о конкретном отделе.
        /// </summary>
        public void PrintDep(Department dep)
        {
            PrintTitlesWorkers();
            foreach (var worker in dep.Workers)
                Console.WriteLine(worker.Print());

            Console.WriteLine();
        }

        /// <summary>
        /// Вывод на экран информации о списке отделов.
        /// </summary>
        /// <param name="listDep">Список отделов</param>
        public void PrintListDeps(List<Department> listDep)
        {
            foreach (var dep in listDep)
                PrintTitlesDeps(dep);
        }

        /// <summary>
        /// Интерфейс меню сортировки сотрудников.
        /// </summary>
        private void SortWorkers(int index)
        {
            Console.WriteLine("Сортировка по:\n[1] ID.\n[2] Имени.\n[3] Фамилии.\n[4] Возрасту.\n[5] Оплате труда.\n[6] Количеству проектов.\n");

            int a = Convert.ToInt32(Console.ReadLine());

            switch(a)
            {
                case 1:
                    listDeps[index].Workers.Sort(delegate (Worker x, Worker y)
                    { return x.ID.CompareTo(y.ID); });
                    break;
                case 2:
                    listDeps[index].Workers.Sort(delegate (Worker x, Worker y)
                    { return x.FirstName.CompareTo(y.FirstName); });
                    break;
                case 3:
                    listDeps[index].Workers.Sort(delegate (Worker x, Worker y)
                    { return x.LastName.CompareTo(y.LastName); });
                    break;
                case 4:
                    listDeps[index].Workers.Sort(delegate (Worker x, Worker y)
                    { return x.Age.CompareTo(y.Age); });
                    break;
                case 5:
                    listDeps[index].Workers.Sort(delegate (Worker x, Worker y)
                    { return x.Salary.CompareTo(y.Salary); });
                    break;
                case 6:
                    listDeps[index].Workers.Sort(delegate (Worker x, Worker y)
                    { return x.CountProjects.CompareTo(y.CountProjects); });
                    break;
            }
        }

        /// <summary>
        /// Интерфейс меню сортировки отделов.
        /// </summary>
        public void SortDeps()
        {
            Console.WriteLine("Сортировка по:\n[1] Имени.\n[2] Количеству проектов.\n");

            int a = Convert.ToInt32(Console.ReadLine());

            switch (a)
            {
                case 1:
                    listDeps.Sort(delegate (Department x, Department y)
                    { return x.Name.CompareTo(y.Name); });
                    break;
                case 2:
                    listDeps.Sort(delegate (Department x, Department y)
                    { return x.CountProjects.CompareTo(y.CountProjects); });
                    break;
            }
        }

        /// <summary>
        /// Сохранение данных.
        /// </summary>
        public void SaveData()
        {
            Console.WriteLine("Сохранить в формате:\n\n[1] JSON.\n[2] XML.\n");

            int a = Convert.ToInt32(Console.ReadLine());

            switch(a)
            {
                case 1:
                    SaveJson();
                    break;
                case 12:
                    SaveXml();
                    break;
            }
        }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        public void LoadData()
        {
            Console.WriteLine("Загрузить из файла в формате:\n\n[1] JSON.\n[2] XML.\n");

            int a = Convert.ToInt32(Console.ReadLine());

            switch (a)
            {
                case 1:
                    LoadJson();
                    break;
                case 12:
                    LoadXml();
                    break;
            }
        }

        /// <summary>
        /// Сохранение списка сотрудников (внутри отделов) в Json.
        /// </summary>
        private void SaveJson()
        {
            string json = JsonConvert.SerializeObject(listDeps);

            File.WriteAllText(this.path + "_departments.json", json);
        }
        
        /// <summary>
        /// Сохранение списка сотрудников (внутри отделов) в XML.
        /// </summary>
        private void SaveXml()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Department>));

            Stream fStream = new FileStream(this.path + "_departments.xml", FileMode.Create, FileAccess.Write);

            xmlSerializer.Serialize(fStream, listDeps);

            fStream.Close();
        }

        /// <summary>
        /// Загрузка json файла.
        /// </summary>
        private void LoadJson()
        {
            string json = File.ReadAllText(this.path + "_departments.json");

            List<Department> tempDeps = JsonConvert.DeserializeObject<List<Department>>(json);

            AddListDeps(tempDeps);
        }

        /// <summary>
        /// Хагрузка xml файла.
        /// </summary>
        private void LoadXml()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Department>));

            Stream fStream = new FileStream(this.path + "_departments.xml", FileMode.Open, FileAccess.Read);

            List<Department> tempDeps = xmlSerializer.Deserialize(fStream) as List<Department>;

            fStream.Close();

            AddListDeps(tempDeps);
        }

        #endregion
    }
}
