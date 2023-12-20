using HKQTravellingAuthenication.Data;
using HKQTravellingAuthenication.Models.Tour;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using System.Net;

namespace HKQTravellingAuthenication.Areas.Tour.Extension
{
	public static class ImageHandler
	{
		public static async Task DownloadAndSaveImage(string htmlString, string destinationFolder, long tourId, ApplicationDbContext data)
		{
			try
			{
				HtmlDocument doc = new HtmlDocument();
				doc.LoadHtml(htmlString);

				var imgNodes = doc.DocumentNode.SelectNodes("//img");

				if (imgNodes != null && imgNodes.Any())
				{
					foreach (var imgNode in imgNodes)
					{
						string imageUrl = imgNode.GetAttributeValue("src", "");

						if (!string.IsNullOrEmpty(imageUrl))
						{
							string fileExtension = Path.GetExtension(imageUrl);

							using (WebClient webClient = new WebClient())
							{
								string fileName = $"image_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";
								string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destinationFolder, fileName);

								int count = 1;
								while (File.Exists(fullPath))
								{
									string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
									fileName = $"{fileNameWithoutExtension}_{count}{fileExtension}";
									fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destinationFolder, fileName);
									count++;
								}

								await webClient.DownloadFileTaskAsync(imageUrl, fullPath);

								var existingTourImage = data.tourImages.SingleOrDefault(ti => ti.TourId == tourId && ti.ImageUrl == fileName);

								if (existingTourImage != null)
								{
									existingTourImage.DayNumber = 1; // Update with the appropriate DayNumber
									existingTourImage.ImageUrl = fileName;
								}
								else
								{
									var tourImage = new TourImages
									{
										DayNumber = 1, // Set the appropriate DayNumber
										ImageUrl = fileName,
										TourId = tourId
									};
									data.tourImages.Add(tourImage);
								}
							}
						}
					}

					await data.SaveChangesAsync();
					Console.WriteLine("Tất cả hình ảnh đã được tải và lưu thành công!");
				}
				else
				{
					Console.WriteLine("Không tìm thấy đường dẫn hình ảnh trong chuỗi HTML.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
			}
		}

	}
}
