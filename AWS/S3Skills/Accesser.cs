using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace S3Skills {
	public class Accesser {
		public async Task Start() {
			Console.WriteLine("Examining S3 ...");

			await LowLevelAPI();
			Console.ResetColor();
		}

		private async Task LowLevelAPI() {
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("-- Examine S3 Low-Level APIs");

			var s3 = new AmazonS3Client();

			await DisplayObjects(s3, "Original");

			string sub = @"newsub01/", dataPath = @"\Data\", fileName = @"subtitleAddSub.scc";

			S3Bucket bucket = await FirstBucket(s3);
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

			await DisplayObjects(s3, "Second");

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("-- Examine S3 Low-Level APIs");

			bucket = await FirstBucket(s3);
			if (bucket != null) {
				Console.ForegroundColor = ConsoleColor.DarkYellow;

				string basic = AppDomain.CurrentDomain.BaseDirectory;
				int index = basic.IndexOf("\\AWS");
				StringBuilder builder = new StringBuilder(basic.Substring(0, index));
				builder.Append(dataPath).Append(fileName);
				Console.WriteLine($"{dataPath}, {fileName}, {builder}");

				Console.WriteLine($"The first bucket is [{bucket.BucketName}]");
				Console.WriteLine($"-- Copy file [{builder}] to [{sub + fileName}]");

				FileInfo file = new FileInfo(builder.ToString());

				PutObjectRequest put = new PutObjectRequest {
					InputStream = file.OpenRead(),
					BucketName = bucket.BucketName,
					Key = sub + fileName
				};

				try {
					PutObjectResponse resp = await s3.PutObjectAsync(put);
				}
				catch (Exception e) {
					Console.WriteLine($"Error during copying a file: {e.Message}");
				}
			}

			await DisplayObjects(s3, "File Copied");

			//bucket = await FirstBucket(s3);
			//await DisplayObjects(s3, "File Copied");


			//bucket = await FirstBucket(s3);
			//await DisplayObjects(s3, "File Deleted");
		}

		private async Task<S3Bucket> FirstBucket(AmazonS3Client s3) {
			var listResponse = await s3.ListBucketsAsync();
			return listResponse.Buckets.Count > 0 ? listResponse.Buckets[0] : null;
		}

		private async Task DisplayObjects(AmazonS3Client s3, string title) {
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
