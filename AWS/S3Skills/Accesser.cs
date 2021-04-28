using Amazon.S3;
using System;
using System.Threading.Tasks;

namespace S3Skills {
	public class Accesser {
		public async Task Start() {
			Console.WriteLine("Examining S3 ...");

			await GetList();
		}

		private async Task GetList() {
			Console.WriteLine("-- GetList");

			var s3 = new AmazonS3Client();

			var listResponse = await s3.ListBucketsAsync();
			Console.WriteLine($"There are ({listResponse.Buckets.Count}) buckets totally.");
			foreach(var a in listResponse.Buckets)
				Console.WriteLine($"  Name:{a.BucketName} / Date:{a.CreationDate}");
		}
	}
}
