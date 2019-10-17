using Microsoft.Extensions.Configuration;
using Shop.DataAccess;
using Shop.Domain;
using System;
using System.IO;
using System.Linq;

/*
		1. Регистрация и вход (смс-код / email код) - сделать до 11.10 (Email есть на метаните)
		2. История покупок 
		3. Категории и товары (картинка в файловой системе)
		4. Покупка (корзина), оплата и доставка (PayPal/Qiwi/etc)
		5. Комментарии и рейтинги
		6. Поиск (пагинация - постраничность)

		Кто сделает 3 версии (Подключенный, автономный и EF) получит автомат на экзамене
*/

namespace Shop.UI
{
	class Program
	{
		static IConfigurationBuilder builder = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json", false, true);

		static IConfigurationRoot configurationRoot = builder.Build();
		static string connectionString = configurationRoot.GetConnectionString("DebugConnectionString");
		static string providerName = configurationRoot
							.GetSection("AppConfig")
							.GetChildren().Single(item => item.Key == "ProviderName")
							.Value;

		static void Main(string[] args)
		{
			//Test();
			//Registration();
			SignIn();
		}

		private static void Test()
		{
			Category category = new Category
			{
				Name = "Бытовая техника",
				ImagePath = "C:/data",
			};

			Item item = new Item
			{
				Name = "Пылесос",
				ImagePath = "C:/data/items",
				Price = 25999,
				Description = "Обычный пылесос",
				CategoryId = category.Id
			};

			User user = new User
			{
				FullName = "Иван Иванов",
				PhoneNumber = "123456",
				Email = "qwer@qwr.qwr",
				Address = "Twesd, 12",
				Password = "password",
				VerificationCode = "1234"
			};

			using (var context = new ShopContext(connectionString))
			{
				context.Users.Add(user);
				context.Items.Add(item);
				context.Categories.Add(category);
				context.SaveChanges();
			}
		}


		static void Registration()
		{
			string fullName, phoneNum, email, address, password, verCode;
			Console.WriteLine("Введите ФИО: ");
			fullName = Console.ReadLine();
			Console.WriteLine("Введите почту: ");
			email = Console.ReadLine();
			Console.WriteLine("Введите номер телефона: ");
			phoneNum = Console.ReadLine();
			Console.WriteLine("Введите адрес: ");
			address = Console.ReadLine();
			Console.WriteLine("Введите пароль: ");
			password = Console.ReadLine();
			Console.WriteLine("Введите секретный код (****): ");
			verCode = Console.ReadLine();

			User user = new User
			{
				PhoneNumber = phoneNum,
				Email = email,
				Address = address,
				Password = password,
				VerificationCode = verCode
			};
			using (var context = new ShopContext(connectionString))
			{
				context.Users.Add(user);
				context.SaveChanges();
			}
		}
		static void SignIn()
		{
			string email, password;
			Console.WriteLine("Введите почту: ");
			email = Console.ReadLine();
			Console.WriteLine("Введите пароль: ");
			password = Console.ReadLine();
			using (var context = new ShopContext(connectionString))
			{
				var user = from u in context.Users
									 where u.Email.Equals(email) & u.Password.Equals(password)
									 select u;
				//Console.WriteLine(user.Count())
				if (user.Count() == 1)
				{
					Console.WriteLine("Введите секретный код: ");
				}
			}
		}
	}
}