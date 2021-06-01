using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;

namespace S3Skills {
	public class Accesser {
		public async Task Start() {
			Console.WriteLine("Examining S3 ...");

			await GetList();
			Console.ResetColor();
		}

		private async Task GetList() {
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("-- GetList");

			var s3 = new AmazonS3Client();

			var listResponse = await s3.ListBucketsAsync();
			Console.WriteLine($"There are ({listResponse.Buckets.Count}) bucket{(listResponse.Buckets.Count < 2 ? "" : "s")} totally.");

			try {

				foreach (var a in listResponse.Buckets) {
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"  Bucket Name: [{a.BucketName}] {"\t"} Creation Date: [{a.CreationDate}]");

					Console.ForegroundColor = ConsoleColor.Cyan;
					ListObjectsRequest request = new ListObjectsRequest { BucketName = a.BucketName };
					ListObjectsResponse response = await s3.ListObjectsAsync(request);
					foreach(var b in response.S3Objects)
						Console.WriteLine($"    Key:[{b.Key}], Modified:[{b.LastModified}], Size:[{b.Size}], Storage:[{b.StorageClass}]");
				}
			}
			catch(Exception e) {
				Console.WriteLine($"Error in list bucket/files: {e.Message}");
			}
		}
	}
}
