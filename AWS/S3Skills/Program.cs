using System;
using System.Threading.Tasks;

namespace S3Skills {
	class Program {
		static async Task Main() {
			Console.WriteLine("Cloud Application -> AWS S3!");

			await new Accesser().Start();
		}
	}
}
