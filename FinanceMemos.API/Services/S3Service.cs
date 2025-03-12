using Amazon.S3.Model;
using Amazon.S3;

namespace FinanceMemos.API.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(IConfiguration configuration)
        {
            var awsOptions = configuration.GetSection("AWS");
            _bucketName = awsOptions["BucketName"];
            _s3Client = new AmazonS3Client(awsOptions["AccessKey"], awsOptions["SecretKey"], Amazon.RegionEndpoint.GetBySystemName(awsOptions["Region"]));
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = fileStream,
                    CannedACL = S3CannedACL.PublicRead
                };

                var response = await _s3Client.PutObjectAsync(request);
                return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
