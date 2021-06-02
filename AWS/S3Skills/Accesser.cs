using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;

namespace S3Skills {
	public class Accesser {
		public async Task Start() {
			Console.WriteLine("Examining S3 ...");

			await ExamineS3();
			Console.ResetColor();
		}

		private async Task ExamineS3() {
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("-- Examine S3");

			var s3 = new AmazonS3Client();

			await ListObjects(s3, "Original");

			S3Bucket bucket = await FirstBucket(s3);

			//var listResponse = await s3.ListBucketsAsync();
			string sub = @"newsub01/", file = @"addfile01.txt";
			if (bucket != null) {
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.WriteLine($"-- Create a sub-directory [{sub}]");

				Console.WriteLine($"The first bucket is [{bucket.BucketName}]");
				PutObjectRequest put = new PutObjectRequest {
					BucketName = bucket.BucketName,
					Key = sub
				};

				try {
					PutObjectResponse resp = await s3.PutObjectAsync(put);
				}
				catch (Exception e) {
					Console.WriteLine($"Error during creating a subfolder: {e.Message}");
				}
			}

			await ListObjects(s3, "Second");
		}

		private async Task<S3Bucket> FirstBucket(AmazonS3Client s3) {
			var listResponse = await s3.ListBucketsAsync();
			return listResponse.Buckets.Count > 0 ? listResponse.Buckets[0] : null;
		}

		private async Task ListObjects(AmazonS3Client s3, string title) {
			Console.ForegroundColor = ConsoleColor.Yellow;
			var listResponse = await s3.ListBucketsAsync();
			Console.WriteLine($"{title}: There are ({listResponse.Buckets.Count}) bucket{(listResponse.Buckets.Count < 2 ? "" : "s")} totally.");

			try {

				foreach (var a in listResponse.Buckets) {
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"  Bucket Name: [{a.BucketName}] {"\t"} Creation Date: [{a.CreationDate}]");

					Console.ForegroundColor = ConsoleColor.Cyan;
					ListObjectsRequest request = new ListObjectsRequest { BucketName = a.BucketName };
					ListObjectsResponse response = await s3.ListObjectsAsync(request);
					foreach (var b in response.S3Objects)
						Console.WriteLine($"    Key:[{b.Key}], Modified:[{b.LastModified}], Size:[{b.Size}], Storage:[{b.StorageClass}]");
				}
			}
			catch (Exception e) {
				Console.WriteLine($"Error in list bucket/files: {e.Message}");
			}
		}
	}
}
