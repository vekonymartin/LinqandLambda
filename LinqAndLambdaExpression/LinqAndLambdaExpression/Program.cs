using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinqAndLambdaExpression
{
    class Program
    {
        class Shops
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public int Emp_Number { get; set; }
            public double Prod_Num { get; set; }
            public bool Prod_Food { get; set; }
            public bool IsToiletPaper { get; set; }

            public override string ToString()
            {
                return $"Shop's name: {Name}\t Shop's address: {Address}\nShop's phone number: {Phone}\n{new string('-', 20)}\n" +
                    $"Employee number : {Emp_Number}\nProduct number : {Prod_Num}\nProduct is food? : {Prod_Food}\nIs there toilet paper? : {IsToiletPaper}";
            }

            public static List<Shops> LoadDatas(string url)
            {
                XDocument xdoc = XDocument.Load(url);

                return xdoc.Descendants("shop").Select(
                    node => new Shops()
                    {
                        Name = node.Element("name")?.Value,
                        Address = node.Element("address")?.Value,
                        Phone = node.Element("phone")?.Value,
                        Emp_Number = int.Parse(node.Element("emp_num")?.Value),
                        Prod_Num = double.Parse(node.Element("product").Attribute("num")?.Value.Replace('.', ',')),
                        Prod_Food = bool.Parse(node.Element("product").Attribute("food")?.Value),
                        IsToiletPaper = bool.Parse(node.Element("product").Attribute("toilet_paper")?.Value)
                    }).ToList();
            }

            public static void SaveDatas(List<Shops> query, string url)
            {
                XDocument xdoc = new XDocument();

                XElement root = new XElement("filtered_shop");
                foreach (Shops item in query)
                {
                    root.Add(new XElement("shop"));
                    //XElement root = new XElement("filtered_shop");
                    root.Element("shop").Add
                        (
                            new XElement("name", item.Name),
                            new XElement("address", item.Address),
                            new XElement("phone", item.Phone),
                            new XElement("emp_num", item.Emp_Number),
                            new XElement("product", new XAttribute("num", item.Prod_Num), new XAttribute("food", item.Prod_Food), new XAttribute("toilet_paper", item.IsToiletPaper))
                        );
                    //xdoc.Root.Add
                    //    (
                    //        new XElement("name",item.Name),
                    //        new XElement("address",item.Address),
                    //        new XElement("phone",item.Phone),
                    //        new XElement("emp_num",item.Emp_Number),
                    //        new XElement("product", new XAttribute("num", item.Prod_Num),  new XAttribute("food",item.Prod_Food), new XAttribute("toilet_paper",item.IsToiletPaper))
                    //    );
                }

                root.Save(url);
                //xdoc.Save(url);

                Console.WriteLine("SAVE XML SUCCESSFULLY");
            }

            public static void ToConsole<T>(IEnumerable<T> input, string str)
            {
                Console.WriteLine("********");

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;

                Console.WriteLine("Feladat: {0}", str);

                Console.ResetColor();

                foreach (T item in input)
                    Console.WriteLine(item.ToString());

                Console.WriteLine("********");
            }
        }

        static string url = "shop.xml";
        static List<Shops> shops = Shops.LoadDatas(url);

        public static void LoadFile()
        {
            Console.WriteLine("*** LOAD FILE ***");

            foreach (Shops item in shops)
                Console.WriteLine(item);

            Console.WriteLine("*** END LOAD FILE ***");
        }

        #region LAMBDA EXPRESSION
        public static void ContainStreetInAddressLambdaEXP()
        {
            var street = shops.Where(x => x.Address.Contains("utca"))
                .Select(x => new
                {
                    x.Name,
                    x.Phone
                });

            Shops.ToConsole(street, "Contain Street word");
        }

        public static void ShortShopNameLambdaEXP()
        {
            var min = shops.Min(x => x.Name.Length);
            var max = shops.Max(x => x.Name.Length);

            Console.WriteLine("MIN = " + min);
            Console.WriteLine("MAX = " + max);

            //var shortname = shops.Where(x => x.Name.Length == min)
            var shortname = shops.Where(x => x.Name.Length == shops.Min(y => y.Name.Length))
                .Select(x => x);
            Shops.ToConsole(shortname, "Short shop name is");
        }

        public static void ShopNameStartWithTLetterLambdaEXP()
        {
            var startwith = shops.Where(x => x.Name.StartsWith("T"))
                .Select(x => new
                {
                    x.Name,
                    x.Phone,
                    x.IsToiletPaper
                });

            Shops.ToConsole(startwith, "Shop name start with 'T' letter");
        }

        public static void WorkMoreThan9EmployeeLambdaEXP()
        {
            var mt9e = shops.Where(x => x.Emp_Number > 9)
                .Select(x => new
                {
                    x.Name,
                    x.Address,
                    x.Emp_Number
                });

            Shops.ToConsole(mt9e, "Work than 9 employee");
        }

        #endregion

        #region LINQ QUERY

        public static void ContainStreetInAddressLinqQuery()
        {
            var street = from s in shops
                         where s.Address.Contains("utca")
                         select new { s.Name, s.Address };

            Shops.ToConsole(street, "Contain Street word");
        }

        public static void ShortShopNameLinqQuery()
        {
            //var min = shops.Min(x => x.Name.Length);

            var shortname = from s in shops
                            let min = shops.Min(x => x.Name.Length)
                            where s.Name.Equals(min)
                            select new { s.Name };

            Shops.ToConsole(shortname, "Short shop name is");
        }

        public static void ShopNameStartWithTLetterLinqQuery()
        {
            var startwith = from s in shops
                            where s.Name.StartsWith("O")
                            select new { s.Name };
            Shops.ToConsole(startwith, "Shop name start with 'T' letter");
        }

        public static void WorkMoreThan9EmployeeLinqQuery()
        {
            var mt9e = from s in shops
                       where s.Emp_Number > 9
                       select new { s.Name, s.Emp_Number };
            Shops.ToConsole(mt9e, "Work than 9 employee");
        }

        public static List<Shops> SaveWorkMoreThan9Employee()
        {
            return (from s in shops
                    where s.Emp_Number > 9
                    select s).ToList();
        }

        #endregion

        static void Main(string[] args)
        {
            LoadFile();

            #region LAMBDA EXPRESSION

            Console.WriteLine("\n" + new string('/', 50) + "\n");
            ContainStreetInAddressLambdaEXP();
            Console.WriteLine("\n" + new string('/', 50) + "\n");
            ShortShopNameLambdaEXP();
            Console.WriteLine("\n" + new string('/', 50) + "\n");
            ShopNameStartWithTLetterLambdaEXP();
            Console.WriteLine("\n" + new string('/', 50) + "\n");
            WorkMoreThan9EmployeeLambdaEXP();
            Console.WriteLine("\n" + new string('/', 50) + "\n");

            #endregion

            Console.WriteLine("\n" + new string('-', 50) + "\n");

            #region LINQ QUERY
            Console.WriteLine("\n" + new string('\\', 50) + "\n");
            ContainStreetInAddressLinqQuery();
            Console.WriteLine("\n" + new string('\\', 50) + "\n");
            ShortShopNameLambdaEXP();
            Console.WriteLine("\n" + new string('\\', 50) + "\n");
            ShopNameStartWithTLetterLinqQuery();
            Console.WriteLine("\n" + new string('\\', 50) + "\n");
            WorkMoreThan9EmployeeLinqQuery();
            Console.WriteLine("\n" + new string('\\', 50) + "\n");
            #endregion

            Shops.SaveDatas(SaveWorkMoreThan9Employee(), "mt9e.xml");
            Console.ReadLine();
        }
    }
}
