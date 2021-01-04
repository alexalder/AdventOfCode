using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day16
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 16));

            Console.WriteLine(GetScanningError(input));

            Console.WriteLine(RevealTicket(input));
        }

        private static int GetScanningError(string[] input)
        {
            Dictionary<MultiRange, string> fields = GetFields(input);

            int[][] tickets = GetNearbyTickets(input);

            int scanningError = 0;

            foreach (int[] ticket in tickets)
            {
                foreach (int field in ticket)
                {
                    bool found = false;
                    foreach (MultiRange range in fields.Keys)
                    {
                        if (range.Contains(field))
                        {
                            found = true;
                            break;
                        }

                    }
                    if (!found)
                        scanningError += field;
                }

            }
            return scanningError;
        }

        private static double RevealTicket(string[] input)
        {
            Dictionary<MultiRange, string> fields = GetFields(input);

            List<int[]> allTickets = GetNearbyTickets(input).ToList();
            List<int[]> validTicketsList = new List<int[]>();
            foreach (int[] ticket in allTickets)
            {
                bool valid = true;
                foreach (int field in ticket)
                {
                    bool found = false;
                    foreach (MultiRange range in fields.Keys)
                    {
                        if (range.Contains(field))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        valid = false;

                }
                if (valid)
                    validTicketsList.Add(ticket);
            }
            int[][] validTickets = validTicketsList.ToArray();

            string[] fieldOrder = new string[20];
            Dictionary<int, List<string>> validFields = new Dictionary<int, List<string>>();

            for (int i = 0; i < fieldOrder.Length; i++)
            {
                validFields.Add(i, new List<string>());
                foreach (var field in fields)
                {
                    bool fits = true;
                    foreach (int[] ticket in validTickets)
                    {
                        if (!field.Key.Contains(ticket[i]))
                            fits = false;
                    }
                    if (fits)
                        validFields[i].Add(field.Value);
                }
            }

            while (fieldOrder.Any(x => x == null))
            {
                foreach (var field in validFields)
                {
                    if (field.Value.Count == 1)
                    {
                        fieldOrder[field.Key] = field.Value[0];

                        foreach (var otherField in validFields)
                        {
                            if (otherField.Value.Contains(fieldOrder[field.Key]))
                                otherField.Value.Remove(fieldOrder[field.Key]);
                        }
                    }
                }
            }

            double res = 1;
            int[] myTicket = GetMyTicket(input);
            for (int i = 0; i < fieldOrder.Length; i++)
            {
                if (fieldOrder[i].StartsWith("departure"))
                    res *= myTicket[i];
            }
            return res;
        }

        private static Dictionary<MultiRange, string> GetFields(string[] input)
        {
            string[] fieldsStrings = Utils.SplitInput(input, "")[0];

            Dictionary<MultiRange, string> fields = new Dictionary<MultiRange, string>();
            foreach (string fieldString in fieldsStrings)
            {
                string name = fieldString.Split(new string[] { ": " }, StringSplitOptions.None)[0];
                string values = fieldString.Split(new string[] { ": " }, StringSplitOptions.None)[1];
                fields.Add(new MultiRange(values.Split(' ')[0].Split('-'), values.Split(' ')[2].Split('-')), name);
            }

            return fields;
        }

        private static int[] GetMyTicket(string[] input)
        {
            string[] myTicketStrings = Utils.SplitInput(input, "")[1];

            int[] myTicket;
            List<int> myTicketList = new List<int>();
            foreach (string value in myTicketStrings[1].Split(','))
            {
                myTicketList.Add(int.Parse(value));
            }
            myTicket = myTicketList.ToArray();

            return myTicket;
        }

        private static int[][] GetNearbyTickets(string[] input)
        {
            string[] nearbyTicketsStrings = Utils.SplitInput(input, "")[2];
            nearbyTicketsStrings = nearbyTicketsStrings.Skip(1).ToArray();

            int[][] nearbyTickets;
            List<int[]> nearbyTicketsList = new List<int[]>();
            foreach (string ticket in nearbyTicketsStrings)
            {
                List<int> nearbyTicketList = new List<int>();
                foreach (string value in ticket.Split(','))
                {
                    nearbyTicketList.Add(int.Parse(value));
                }
                nearbyTicketsList.Add(nearbyTicketList.ToArray());
            }
            nearbyTickets = nearbyTicketsList.ToArray();

            return nearbyTickets;
        }
    }
}