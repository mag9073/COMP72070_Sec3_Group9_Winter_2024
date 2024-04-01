using Server.DataStructure;
using Server.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server.Implementations
{
    public class ParkReviewManager
    {
        // Reads all park reviews from a file, we will identified based on each reviews park name -> 
        // within each review we will look for the delimiter (|) 
        public List<ParkReviewData> ReadAllParkReviewsFromFile(string filePath)
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
                    // Add a specific part review post time format
                    string dateFormat = "MM-dd-yyyy hh:mm:ss tt";

                    reviews.Add(new ParkReviewData
                    {
                        ParkName = parkName,
                        UserName = match.Groups[1].Value.Trim(),
                        Rating = float.Parse(match.Groups[2].Value.Trim()),
                        DateOfPosting = DateTime.ParseExact(match.Groups[3].Value.Trim(), dateFormat, CultureInfo.InvariantCulture),
                        Review = match.Groups[4].Value.Trim().Replace("\n", " ") // Replace newline characters to maintain review structure
                    });
                }
            }

            return reviews;
        }

        // Overwrite All Park Reviews back to text file after it has been modified -> i.e deleted a review
        public void OverwriteAllParkReviewsToFile(string filePath, List<ParkReviewData> reviews)
        {
            StringBuilder fileContent = new StringBuilder();

            // Group reviews by ParkName to correctly format them when writing reviews back to the text file
            IEnumerable<IGrouping<string, ParkReviewData>>? groupedReviews = reviews.GroupBy(review => review.ParkName);

            foreach (IGrouping<string, ParkReviewData> group in groupedReviews)
            {

                foreach (ParkReviewData? review in group)
                {
                    fileContent.AppendLine($"ParkName: {group.Key}");
                    fileContent.AppendLine($"Username: {review.UserName} | ParkRating: {review.Rating} | DateOfPosting: {review.DateOfPosting.ToString("MM-dd-yyyy hh:mm:ss tt")} | Review: {review.Review}\n");
                }
            }

            // Finally write every back 
            File.WriteAllText(filePath, fileContent.ToString());
        }

        public void DeleteParkReviews(string parkNameToDelete, string filePath)
        {
            // https://stackoverflow.com/questions/29975219/reading-lines-from-text-file-and-add-to-liststring

            List<string> updatedContent = new List<string>();
            bool isThisReviewATargetPark = false;

            // Read all the lines from the file and store it in the memory as list
            string[] allLines = File.ReadAllLines(filePath);
            for (int i = 0; i < allLines.Length; i++)
            {
                string line = allLines[i];

                // Check if the line is the start of a new review block
                if (line.StartsWith("ParkName: "))
                {
                    // Check if the current review block is for the park we want to delete
                    isThisReviewATargetPark = line.Contains(parkNameToDelete);
                }

                // If the current line is not part of the park to delete, we add them back to a line to be written back to our file
                if (!isThisReviewATargetPark)
                {
                    updatedContent.Add(line);
                }

                // If the line is empty, we know that it should be the end of the file then
                if (string.IsNullOrWhiteSpace(line))
                {
                    isThisReviewATargetPark = false;
                }
            }

            // Write updated file without the file we park we deleted
            File.WriteAllLines(filePath, updatedContent);
        }

        public void AppendReviewDataToFile(string filePath, ParkReviewData parkReviewData)
        {
            try
            {
                // https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
                // Convert DateTime Format
                StringBuilder reviewDataBuffer = new StringBuilder();
                reviewDataBuffer.AppendLine($"ParkName: {parkReviewData.ParkName}");
                reviewDataBuffer.AppendLine($"Username: {parkReviewData.UserName} | ParkRating: {parkReviewData.Rating} | DateOfPosting: {parkReviewData.DateOfPosting.ToString("MM-dd-yyyy hh:mm:ss tt")} | Review: {parkReviewData.Review}\n");

                File.AppendAllText(filePath, reviewDataBuffer.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

    }
}
