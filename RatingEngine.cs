using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;

namespace Nipendo.Evaluation
{
    /// <summary>
    /// The RatingEngine reads the policy application details from a file
    /// and produces a numeric rating value based on the details.
    /// </summary>
    public class RatingEngine
    {
        public decimal Rating { get; set; }

        public void Rate()
        {
            // log start rating
            Console.WriteLine("Starting rate.");

            Policy policy = LoadPolicy("policy.json");

            Rating = RatePolicy(policy);

            // log finish rating
            Console.WriteLine("Rating completed.");
        }

        // Load Policy from a file
        public static Policy LoadPolicy(string fileName)
        {
            // log start rating
            Console.WriteLine("Loading policy.");

            // load policy - open file policy.json
            string policyJson = File.ReadAllText(fileName);

            Policy policy = JsonConvert.DeserializeObject<Policy>(policyJson,
                new StringEnumConverter());

            return policy;
        }

        // Calculate Rating from a policy by type
        public static decimal RatePolicy(Policy policy)
        {
            Console.WriteLine("Rating " + policy.Type.ToString().ToUpper() +
                              " policy...\nValidating policy.");
            switch (policy.Type)
            {
                case PolicyType.Health:
                    return RateHealthPolicy(policy);
                case PolicyType.Travel:
                    return RateTravelPolicy(policy);
                case PolicyType.Life:
                    return RateLifePolicy(policy);
                default:
                    Console.WriteLine("Unknown policy type");
                    return 0m;
            }
        }

        // Health Policy
        public static decimal RateHealthPolicy(Policy policy)
        {
            // Conditions
            if (string.IsNullOrEmpty(policy.Gender))
            {
                Console.WriteLine("Health policy must specify Gender");
                return 0m;
            }

            // Rating logic
            decimal policyRating = 1000m;

            return policy.Gender == "Male" ? policyRating - 100m : policyRating;
        }

        // Travel Policy
        public static decimal RateTravelPolicy(Policy policy)
        {
            // Conditions
            if (string.IsNullOrEmpty(policy.Country))
            {
                Console.WriteLine("Travel policy must specify country.");
                return 0m;
            }

            if (policy.Days <= 0)
            {
                Console.WriteLine("Travel policy must specify Days.");
                return 0m;
            }

            if (policy.Days > 180)
            {
                Console.WriteLine("Travel policy cannot be more then 180 Days.");
                return 0m;
            }

            // Rating logic
            decimal policyRating = policy.Days * 2.5m;

            return policy.Country == "Italy" ? 3 * policyRating : policyRating;
        }

        // Life Policy
        public static decimal RateLifePolicy(Policy policy)
        {
            // Conditions
            DateTime todayDateTime = DateTime.Today;

            if (policy.DateOfBirth == DateTime.MinValue)
            {
                Console.WriteLine("Life policy must include Date of Birth.");
                return 0m;
            }

            if (policy.DateOfBirth < todayDateTime.AddYears(-100))
            {
                Console.WriteLine("Max eligible age for coverage is 100 years.");
                return 0m;
            }

            if (policy.Amount == 0)
            {
                Console.WriteLine("Life policy must include an Amount.");
                return 0m;
            }

            // Rating logic
            int age = todayDateTime.Year - policy.DateOfBirth.Year;

            if (policy.DateOfBirth.Month == todayDateTime.Month &&
                todayDateTime.Month < policy.DateOfBirth.Month ||
                todayDateTime.Day < policy.DateOfBirth.Day)
            {
                age--;
            }

            decimal policyRating = policy.Amount * age / 200;

            return policy.IsSmoker ? 2 * policyRating : policyRating;
        }
    }
}
