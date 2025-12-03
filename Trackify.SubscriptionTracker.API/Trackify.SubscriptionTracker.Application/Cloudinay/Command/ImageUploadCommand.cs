using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Application.Cloudinay.Command
{
    public class ImageUploadCommand : IRequest<string>
    {
        public IFormFile File {  get; set; }
    }

    public class ImageUploadCommandHandler : IRequestHandler<ImageUploadCommand, string>
    {
        private readonly Cloudinary _cloudinay;
        public ImageUploadCommandHandler(Cloudinary cloudinay)
        {
            _cloudinay = cloudinay;   
        }
        public async Task<string> Handle(ImageUploadCommand request, CancellationToken cancellationToken)
        {
            var file = request.File;
            if (file == null || file.Length == 0) {
                throw new Exception("Invalid file");
            }

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "subscriptions"
            };

            var result = await _cloudinay.UploadAsync(uploadParams);

            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.SecureUrl.ToString();
            }

            throw new Exception("cloudinary upload failed");
        }
    }
}
