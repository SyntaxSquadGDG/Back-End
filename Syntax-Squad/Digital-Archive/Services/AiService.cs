using Digital_Archive.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Digital_Archive.Services
{
    public class AiService
    {
        private readonly AppDbContext _context;
        public AiService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetOcr(Sfile file, IFormFile formFile)
        {
            try
            {
                string imagePath = file.Blob_Token;
                using (var httpClient = new HttpClient())
                using (var multipartContent = new MultipartFormDataContent())
                {
                    // Add the image file to the request
                    using (var fileStream = File.OpenRead(imagePath))
                    {
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // Adjust if the image is PNG
                        multipartContent.Add(fileContent, "file", Path.GetFileName(imagePath));

                        // Send POST request to FastAPI endpoint
                        var response = await httpClient.PostAsync("https://archivai-ai.azurewebsites.net/extract-text/", multipartContent);

                        // Ensure the response is successful
                        response.EnsureSuccessStatusCode();

                        // Read and parse the JSON response
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JObject.Parse(responseContent);

                        // Return the extracted text
                        string OcrResonce = jsonResponse["text"]?.ToString() ?? "No text extracted.";

                        file.Ocr = OcrResonce;

                        _context.SaveChanges();

                        return OcrResonce;
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        public async Task<List<string>> ClassifyImage(string imagePath, IFormFile formFile)
        {
            try
            {
                using (var httpClient = new HttpClient())
                using (var multipartContent = new MultipartFormDataContent())
                {
                    // Add the image file to the request
                    using (var fileStream = File.OpenRead(imagePath))
                    {
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // Adjust if the image is PNG
                        multipartContent.Add(fileContent, "file", Path.GetFileName(imagePath));

                        // Send POST request to FastAPI endpoint
                        var response = await httpClient.PostAsync("https://archivai-ai.azurewebsites.net/classify-image/", multipartContent);

                        // Ensure the response is successful
                        response.EnsureSuccessStatusCode();

                        // Read and parse the JSON response
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JObject.Parse(responseContent);

                        // Return the extracted text
                        string folderpath = jsonResponse["path"]?.ToString() ?? "No text extracted.";
                        string accu = jsonResponse["accuracy"]?.ToString() ?? "No text extracted.";


                        return new List<string> { folderpath, accu };
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<string> { $"Error: {ex.Message}"};
            }
        }
    }
}
