using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3Skills {
	public class Accesser {
		public async Task Start() {
			Console.WriteLine("Examining S3 ...");

			await GetList();
		}

		private async Task GetList() {
			Console.WriteLine("-- GetList");
			//create an s3 client object
			var s3 = new AmazonS3Client();

			var listResponse = await s3.ListBucketsAsync();
			Console.WriteLine($"There are ({listResponse.Buckets.Count}) buckets totally.");
			foreach(var a in listResponse.Buckets)
				Console.WriteLine($"  {a.BucketName}");
		}
	}
}
