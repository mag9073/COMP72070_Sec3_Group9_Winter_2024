using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogiPark.MVVM.Model
{
    public class ParkReviewManager
    {
        [ProtoContract]
        public class ParkReviewData
        {
            [ProtoMember(1)]
            public string ParkName = String.Empty;

            [ProtoMember(2)]
            public string UserName = String.Empty;

            [ProtoMember(3)]
            public float Rating = float.MinValue;

            [ProtoMember(4)]
            public DateTime DateOfPosting = DateTime.MinValue;

            [ProtoMember(5)]
            public string Review = String.Empty;

            public string GetUserName()
            {
                return this.UserName;
            }

            public float GetParkRating()
            {
                return this.Rating;
            }

            public DateTime GetDateOfPosting()
            {
                return this.DateOfPosting;
            }

            public string GetReview()
            {
                return this.Review;
            }

            public void SetUserName(string userName)
            {
                this.UserName = userName;
            }

            public void SetParkRating(float parkRating)
            {
                this.Rating = parkRating;
            }

            public void SetDateOfPosting(DateTime dateOfPosting)
            {
                this.DateOfPosting = dateOfPosting;
            }

            public void SetReview(string review)
            {
                this.Review = review;
            }

            public byte[] SerializeToByteArray()
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, this);
                    return stream.ToArray();
                }
            }

            public ParkReviewData deserializeLoginData(byte[] buffer)
            {
                using (MemoryStream memStream = new MemoryStream(buffer))
                {
                    return Serializer.Deserialize<ParkReviewData>(memStream);
                }
            }

            // Reads all park reviews from a file, assuming a simple delimited format
            public static List<ParkReviewData> ReadAllParkReviewsFromFile(string filePath)
            {
                List<ParkReviewData> reviews = new List<ParkReviewData>();
                string fileContent = File.ReadAllText(filePath);

                // Split the content by "ParkName:" as each section starts with it
                string[] parkSections = Regex.Split(fileContent, @"ParkName:\s*");

                foreach (var section in parkSections)
                {
                    if (string.IsNullOrWhiteSpace(section)) continue; // Skip empty sections

                    // Extract the park name and the rest of the section separately
                    string[] lines = section.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    string parkName = lines[0].Trim();
                    string reviewLines = string.Join("\n", lines.Skip(1)); // Re-join the lines for further processing

                    // Match the pattern for each review within a section
                    string reviewPattern = @"Username: (.*?) \| ParkRating: (.*?) \| DateOfPosting: (.*?) \| Review: (.*?)\n\n";

                    foreach (Match match in Regex.Matches(reviewLines + "\n\n", reviewPattern, RegexOptions.Singleline))
                    {
                        reviews.Add(new ParkReviewData
                        {
                            ParkName = parkName,
                            UserName = match.Groups[1].Value.Trim(),
                            Rating = float.Parse(match.Groups[2].Value.Trim()),
                            DateOfPosting = DateTime.ParseExact(match.Groups[3].Value.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            Review = match.Groups[4].Value.Trim().Replace("\n", " ") // Replace newline characters to maintain review integrity
                        });
                    }
                }

                return reviews;
            }
        }
    }
}
