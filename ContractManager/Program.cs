using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

class Program
{
    private const int SMALL_LOW = 500;
    private const int SMALL_MEDIUM = 700;
    private const int SMALL_HIGH = 900;
    private const int MED_LOW = 650;
    private const int MED_MEDIUM = 850;
    private const int MED_HIGH = 1050;
    private const int LARGE_LOW = 850;
    private const int LARGE_MEDIUM = 1050;
    private const int LARGE_HIGH = 1250;
    private const int LARGE_UNLIMITED = 2000;
    private const string CONTRACTS_FILE = "contracts.txt";
    private const string ARCHIVES_FILE = "Archives.txt";
    static void Main(string[] args)
    {
        StartApp();

    } // end Main

    //  method StartApp
    private static void StartApp()
    {
        do
        {
            DisplayMenu();
            int user_choice = GetUserChoice();
            switch (user_choice)
            {
                case 1:
                    ProcessOptionOne();
                    break;
                case 2:
                    ProcessOptionTwo();
                    break;
                case 3:
                    ProcessOptionThree();
                    break;
                case 4:
                    ProcessOptionFour();
                    break;
                case 5:
                    ProcessOptionFive();
                    break;

            } // end switch

            Console.WriteLine("\n");

        } while (true);

    } // end method StartApp

    // method GetFullMonthName
    private static string GetFullMonthName(string month)
    {
        switch (month)
        {
            case "JAN": return "January";
            case "FEB": return "February";
            case "MAR": return "March";
            case "APR": return "April";
            case "MAY": return "May";
            case "JUN": return "Jun";
            case "JUL": return "July";
            case "AUG": return "August";
            case "SEP": return "September";
            case "OCT": return "October";
            case "NOV": return "November";
            default: return "December";

        } // end switch

    } // end method GetFullMonthName

    // method DisplayStatus
    private static void DisplayStatus(int option)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("************************************************");
        Console.WriteLine("*          Telephone Contract Manager          *");
        Console.WriteLine("************************************************");
        switch (option)
        {
            case 1:
                ShowWhichOptionUserChosen("one");
                break;
            case 2:
                ShowWhichOptionUserChosen("two");
                break;
            case 3:
                ShowWhichOptionUserChosen("three");
                break;
            case 4:
                ShowWhichOptionUserChosen("four");
                break;

        } // end switch

    } // end method DisplayStatus

    // method ShowWhichOptionUserChosen
    private static void ShowWhichOptionUserChosen(string main_menu_options)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        if (main_menu_options == "one") Console.WriteLine("\n***You have chosen to create a new Client***\n");
        if (main_menu_options == "two") Console.WriteLine("\n***You have chosen to view summary of clients***\n");
        if (main_menu_options == "three") Console.WriteLine("\n***You have chosen to view summary for a selected month***\n");
        if (main_menu_options == "four") Console.WriteLine("\n***You have chosen to search for clients***\n");
        Console.ResetColor();

    } // end method ShowWhichOptionUserChosen

    private static void ProcessOptionThree()
    {
        Console.Clear();
        DisplayStatus(3);
        int which_file = GetUserFileChoice(3);
        string user_choice_month = GetMonthName(which_file);
        bool file_found = false;
        int total_number_of_contracts_in_given_month = 0;
        int high_or_unlimited_data_bundle_in_given_month = 0;
        int total_number_of_large_package = 0;
        int total_cost_for_given_month = 0;
        double average_cost_for_large_package_in_given_month = 0;

        try
        {
            StreamReader reader = new StreamReader(which_file == 1 ? CONTRACTS_FILE : ARCHIVES_FILE);
            using (reader)
            {
                file_found = true;
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null) break;

                    string[] line_split = line.Split('\t');
                    string date = line_split[0];


                    if (date.IndexOf(user_choice_month, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        total_number_of_contracts_in_given_month++;
                        int data_bundle = Convert.ToInt32(line_split[2]);
                        int package = Convert.ToInt32(line_split[1]);

                        if (DataBundleType(data_bundle) == "High" || DataBundleType(data_bundle) == "Unlimited")
                            high_or_unlimited_data_bundle_in_given_month++;

                        if (PackageType(package) == "Large")
                        {
                            total_number_of_large_package++;
                            int cost_of_one_package = Convert.ToInt32(line_split[6]);
                            total_cost_for_given_month += cost_of_one_package;

                        } // end if
                    } // end if

                } // end while

            } // end using

        } // end try
        catch (FileNotFoundException ex)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File not found ! Returning to Main menu !");
            Console.ResetColor();
        } // end catch

        if (file_found)
        {
            if (total_number_of_large_package > 0)
                average_cost_for_large_package_in_given_month =
                    (total_cost_for_given_month / total_number_of_large_package) / (double)100;

            ViewSummaryOfContracts(total_number_of_contracts_in_given_month,
                high_or_unlimited_data_bundle_in_given_month, average_cost_for_large_package_in_given_month,
                user_choice_month, which_file); // view summary of all the clients
            Console.WriteLine();
        } // endif

    } // end method ProcessOptionThree


    // overloading method ViewSummaryOfContracts
    private static void ViewSummaryOfContracts(int total_contracts_in_a_month, int high_or_unlimited_contracts,
        double large_package_average, string month_name, int which_file)
    {
        Console.Clear();
        Console.WriteLine("*********************Summary of contracts******************\n");
        Console.WriteLine("File: " + (which_file == 1 ? "Contracts.txt" : "Archives.txt"));
        Console.WriteLine("Month: " + GetFullMonthName(month_name));
        // Console.WriteLine($"Total number of contracts: {total_contracts_in_a_month}");
        Console.WriteLine($"Contracts with High or Unlimited Data Bundles: {high_or_unlimited_contracts}\n");
        Console.WriteLine($"Average charge for large package: {large_package_average}\n");
        Console.WriteLine(month_name + "\n" + total_contracts_in_a_month);

        Console.WriteLine("\nRedirecting to Main menu.......");

    } // end overloading method ViewSummaryOfContracts


    // method ViewSummaryOfContracts
    private static void ViewSummaryOfContracts(int total_no_of_contracts, int contracts_high_or_unlimited, double large_package_average, Dictionary<string, int> contracts_list, int which_file)
    {
        Console.Clear();
        Console.WriteLine("*********************Summary of contracts****************************\n");
        Console.WriteLine("File: " + (which_file == 1 ? "Contracts.txt" : "Archives.txt"));
        Console.WriteLine($"\nTotal number of contracts: {total_no_of_contracts}");
        Console.WriteLine($"Contracts with High or Unlimited Data Bundles: {contracts_high_or_unlimited}");
        Console.WriteLine($"Average charge for large package: {large_package_average}");

        Console.WriteLine($"\nNumber of contracts per month: \n");
        foreach (var i in contracts_list)
            Console.Write("{0,-6}", i.Key);
        Console.WriteLine("\n" + string.Join(" ", contracts_list.Values.Select(value => value.ToString().PadRight(5))));

        Console.WriteLine("\nRedirecting to Main menu.......");
    } // end method ViewSummaryOfContracts

    // method MonthName
    private static string GetMonthName(int file_chosen)
    {
        string month_name = string.Empty;
        ShowWhichFileWasChosen(file_chosen);
        Console.Write("\nEnter month name [Jan, Feb Mar etc] : ");
        do
        {
            month_name = Console.ReadLine();
            month_name = month_name.ToUpper();
            if (month_name == string.Empty)
            {
                ShowWhichFileWasChosen(file_chosen);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nNo month entered. Enter month name [Jan, Feb Mar etc] : ");
                Console.ResetColor();

            } // end if
            else if ((month_name != "JAN" && month_name != "FEB" && month_name != "MAR" &&
                      month_name != "APR" && month_name != "MAY" && month_name != "JUN"
                      && month_name != "JUL" && month_name != "AUG" && month_name != "SEP" &&
                      month_name != "OCT" && month_name != "NOV" && month_name != "DEC"))
            {
                ShowWhichFileWasChosen(file_chosen);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nInvalid month name. Enter month name [Jan, Feb Mar etc] : ");
                Console.ResetColor();

            } // end else if
        } while (month_name == "" || (month_name != "JAN" && month_name != "FEB" && month_name != "MAR" && month_name != "APR" && month_name != "MAY" && month_name != "JUN"
                                      && month_name != "JUL" && month_name != "AUG" && month_name != "SEP" && month_name != "OCT" && month_name != "NOV" && month_name != "DEC"));

        return month_name;
    }// end method GetMonthName

    private static void ShowWhichFileWasChosen(int file_chosen)
    {
        Console.Clear();
        DisplayStatus(3);
        Console.WriteLine("File chosen: " + (file_chosen == 1 ? CONTRACTS_FILE : ARCHIVES_FILE));

    } // end method ShowWhichFileWasChosen

    private static void ProcessOptionTwo()
    {
        Console.Clear();
        DisplayStatus(2);
        int total_number_of_clients = 0;
        int total_of_high_or_unlimited_bundle = 0;
        int cost_of_all_large_packages = 0;
        int number_of_large_packages = 0;
        double average_charge_for_large_package = 0;
        bool file_found = false;
        Dictionary<string, int> client_dictionary = new Dictionary<string, int>()
        {
            {"Jan", 0},{"Feb", 0},{"Mar", 0},{"Apr", 0},{"May",0},{"Jun", 0},{"Jul", 0},{"Aug", 0},{"Sep", 0},{"Oct", 0},{"Nov", 0},{"Dec", 0}
        };

        int file_chosen = GetUserFileChoice(2); // get the file the user wants to access

        try
        {
            using (StreamReader reader = new StreamReader(file_chosen == 1 ? CONTRACTS_FILE : ARCHIVES_FILE))
            {
                file_found = true;
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null) break;

                    total_number_of_clients++;

                    string[] one_client = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries); // store a clients details in an array to access all the details

                    int data_bundle = Convert.ToInt32(one_client[2]);
                    if (DataBundleType(data_bundle) == "High" || DataBundleType(data_bundle) == "Unlimited")
                    {
                        total_of_high_or_unlimited_bundle++;

                    } // end if

                    int package_size = Convert.ToInt32(one_client[1]);
                    if (PackageType(package_size) == "Large")
                    {
                        number_of_large_packages++;
                        int cost_of_the_large_package = Convert.ToInt32(one_client[6]);

                        cost_of_all_large_packages += cost_of_the_large_package;

                    } // end if

                    // calculate the number of clients for each month
                    string date = one_client[0]; // get the date
                    string[] split_date = date.Split(new Char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    string month_name = split_date[1];

                    int value = 0;
                    if (!client_dictionary.TryGetValue(month_name, out value))
                        client_dictionary[month_name] = 1;
                    else
                        client_dictionary[month_name]++;

                } // end while

            } // end using

        } // end try
        catch (FileNotFoundException ex)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File not found ! Returning to Main menu !");

        } // end catch

        if (file_found)
        {
            if (number_of_large_packages > 0)
                average_charge_for_large_package = (cost_of_all_large_packages / number_of_large_packages) / (double)100;

            ViewSummaryOfContracts(total_number_of_clients, total_of_high_or_unlimited_bundle, average_charge_for_large_package, client_dictionary, file_chosen); // view summary of all the clients
            Console.WriteLine();

        } // end if

    } // end method ProcessOptionTwo

    // method GetUserFileChoice
    private static int GetUserFileChoice(int option)
    {
        int user_file_selection_number = 0;
        bool is_file_selection_number_correct = false;
        string user_file_selection = "";
        Console.Write("Which file -> [1: Contracts.txt 2: Archives.txt]: ");
        do
        {
            Console.ResetColor();
            user_file_selection = Console.ReadLine();
            if (user_file_selection == "")
            {
                ShowWhichOptionUserChosen(option);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("No file selected. [1: Contracts.txt 2: Archives.txt] Select one: ");
                Console.ResetColor();

            } // end if
            else
            {
                is_file_selection_number_correct = int.TryParse(user_file_selection, out user_file_selection_number);
                if (is_file_selection_number_correct)
                {
                    if (user_file_selection_number != 1 && user_file_selection_number != 2)
                    {
                        ShowWhichOptionUserChosen(option);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Incorrect value. [1: Contracts.txt 2: Archives.txt] Select one: ");
                        Console.ResetColor();
                    } // end if

                } // end if
                else
                {
                    ShowWhichOptionUserChosen(option);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Invalid value. [1: Contracts.txt 2: Archives.txt] Select one: ");
                    Console.ResetColor();
                } // end else

            } // end else
        }
        while (user_file_selection == "" || (user_file_selection_number != 1 && user_file_selection_number != 2) || !is_file_selection_number_correct);

        return user_file_selection_number;

    } // end method GetUserFileChoice

    private static void ShowWhichOptionUserChosen(int option)
    {
        Console.Clear();
        if (option == 2) DisplayStatus(2);
        else if (option == 3) DisplayStatus(3);
        else DisplayStatus(4);
    } // end method ShowWhichOptionUserChosen

    private static void ProcessOptionOne()
    {
        Contract a_contract;
        Console.Clear();
        DisplayStatus(1);

        string client_name = GetClientName();
        string reference = GetReference(client_name);
        int package = GetPackage(client_name, reference);
        int data_bundle = GetDataBundle(package, client_name, reference);
        int period = GetPeriod(reference, client_name, package, data_bundle);
        string allow_international = AllowInternationalCalls(client_name, reference, package, data_bundle, period);
        int total_cost = CalculateContractCost(reference, package, data_bundle, period, allow_international);
        string date = GetDate();
        a_contract = new Contract(date, package, data_bundle, period, allow_international, reference, total_cost, client_name);

        //  a_contract = new Contract("15-Aug-2018", 1, 2, 12, "N", "JB123N", 1146, "S Poolinggalamn");

        SaveClient(a_contract);
        Console.Clear();
        DisplayClientSummary(a_contract);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Client Saved !");

    } // end method ProcessOptionOne

    // method SaveClient
    private static void SaveClient(Contract a_contract)
    {
        using (StreamWriter writer = File.AppendText(CONTRACTS_FILE))
        {
            writer.WriteLine
            ("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}",
                a_contract.Date, a_contract.Package, a_contract.DataBundle, a_contract.Period, a_contract.AllowInternationalCall, a_contract.Reference, a_contract.MonthlyCharge, a_contract.Name);
        } // end using

    } // end method SaveClient

    // method DisplayClientSummary
    private static void DisplayClientSummary(Contract a_contract)
    {
        string box_characters = "+--------------------------------------------+";
        Console.WriteLine(box_characters);
        Console.WriteLine("{0}{1," + (box_characters.Length - 1) + "}", "|", "|");

        Console.WriteLine("{0}{1,9}: {2,-33}|", "|", "Customer", a_contract.Name);
        Console.WriteLine("{0}{0," + (box_characters.Length - 1) + "}", "|", "|");
        Console.Write("|{0,9}: {1,-2} {2, 12}: {3,-12}|", "Ref", a_contract.Reference, "Date", DateFormatted(a_contract.Date));
        Console.Write("\n|{0,9}: {1,-12} {2, 6}: {3,-12}|", "Package", PackageFormatted(a_contract.Package), "Data", BundleFormatted(a_contract.DataBundle));
        Console.Write("\n|{0,9}: {1,-10} {2, 8}: {3,-12}|", "Period", PeriodFormatted(a_contract.Period), "Type", TypeOfClientFormatted(a_contract.Reference));
        Console.WriteLine("\n{0}{1," + (box_characters.Length - 1) + "}", "|", "|");


        Console.WriteLine("|{0,9}: {1,-8}{2, -9}: {3,-12}|", "Discount", DetermineIfDiscountWasApplied(a_contract.Reference, a_contract.Period), "Intl. Calls", DetermineIfInternationaCalls(a_contract.AllowInternationalCall));
        string message = DisplayDiscountedOrNot(a_contract.MonthlyCharge, a_contract.Reference, a_contract.Period);


        Console.WriteLine("{0}{1," + (box_characters.Length - 1) + "}", "|", "|");

        int centre_of_box_characters = box_characters.Length / 2; // get the centre of the box in characters - value 1
        int centre_of_message = message.Length / 2; // get the centre of the message - value 2




        int final_space = centre_of_box_characters + centre_of_message - 1; // adding value 1 and value 2 (then subtract 1)

        Console.Write("{0}{1," + final_space + "}", "|", message); // print the first vertical pipe followed by message

        Console.Write("{0," + (box_characters.Length - final_space - 1) + "}", "|"); // print the vertical pipe at the end of the line
        Console.WriteLine("\n" + box_characters); // print the box in characters


    } // end method DisplayClientSummary

    // method DisplayDiscountedOrNot
    private static string DisplayDiscountedOrNot(double monthly_charge, string reference, int period)
    {
        string final_result_string = string.Empty;
        double charge = monthly_charge / 100;
        string charge_string = charge.ToString("C");

        if (DetermineIfDiscountWasApplied(reference, period) != "None")
        {
            final_result_string = "Discounted Monthly Charge: " + charge_string;
        } // end if
        else
        {
            final_result_string = "Monthly Charge: " + charge_string;
        } // end else

        return final_result_string;
    } // end method DisplayDiscountedOrNot

    // method DetermineIfDiscountWasApplied
    private static string DetermineIfDiscountWasApplied(string reference, int period)
    {
        if (Account_Is_Business_Type(reference)) return "10%";
        if (period == 12 || period == 18) return "5%";
        if (period == 24) return "10%";
        return "None";
    } // end method DetermineIfDiscountWasApplied

    // method DetermineIfInternationalCalls
    private static string DetermineIfInternationaCalls(string international_calls) => international_calls == "Y" ? "Yes" : "No";

    // method ProcessOptionFour
    private static void ProcessOptionFour()
    {
        Console.Clear();
        DisplayStatus(4);
        bool file_found = false;
        int user_file_chosen = GetUserFileChoice(4);
        string search_text = GetSearchText(user_file_chosen);
        //  List<Contract> list_of_contracts = new List<Contract>();
        SortedSet<Contract> list_of_contracts = new SortedSet<Contract>();
        Contract a_contract;

        try
        {
            StreamReader reader = new StreamReader(user_file_chosen == 1 ? CONTRACTS_FILE : ARCHIVES_FILE);
            using (reader)
            {
                file_found = true;
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null) break;

                    string[] line_split = line.Split('\t');
                    string date = line_split[0];
                    int package = Convert.ToInt32(line_split[1]);
                    int data_bundle = Convert.ToInt32(line_split[2]);
                    int period = Convert.ToInt32(line_split[3]);
                    string allow_international_call = line_split[4];
                    string reference = line_split[5];
                    int total_cost = Convert.ToInt32(line_split[6]);
                    string name = line_split[7];

                    for (int i = 0; i < search_text.Length; i++)
                    {
                        if ((name.IndexOf(search_text[i].ToString(), 0, StringComparison.CurrentCultureIgnoreCase) != -1) || (reference.IndexOf(search_text[i].ToString(), 0, StringComparison.CurrentCultureIgnoreCase) != -1))
                        {

                            a_contract = new Contract(date, package, data_bundle, period, allow_international_call, reference, total_cost, name);
                            list_of_contracts.Add(a_contract);

                        } // end if

                    } // end for loop

                } // end while

            } // end using
        } // end try
        catch (FileNotFoundException ex)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File not found ! Redirecting to Main menu !");
            Console.ResetColor();

        } // end catch

        if (file_found)
            PrintSearchedClient(list_of_contracts, list_of_contracts.Count);
    } // end method ProcessOptionFour

    // method PrintSearchedClient
    private static void PrintSearchedClient(SortedSet<Contract> list_of_contracts, int total_found)
    {
        Console.Clear();
        if (list_of_contracts.Count != 0)
        {
            foreach (Contract contract in list_of_contracts) DisplayClientSummary(contract);
            Console.WriteLine("Total found: " + total_found);
            Console.WriteLine("All the result(s) are shown above. ");
            Console.WriteLine("\nRedirecting back to main menu...");
        } // end if
        else
        {
            Console.WriteLine("No Client info found !");
            Console.WriteLine("\nRedirecting back to Main menu...");

        } // end else

    } // end method PrintSearchedClient

    // method GetSearchText
    private static string GetSearchText(int which_file)
    {
        Console.Clear();
        DisplayStatus(4);
        Console.WriteLine("File: " + (which_file == 1 ? CONTRACTS_FILE : ARCHIVES_FILE));
        Console.Write("\nEnter search term: ");
        string search_term;
        do
        {
            search_term = Console.ReadLine();
            if (search_term == string.Empty)
            {
                Console.Clear();
                DisplayStatus(4);
                Console.WriteLine("File: " + (which_file == 1 ? CONTRACTS_FILE : ARCHIVES_FILE));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nNo text is entered. Enter search term: ");
                Console.ResetColor();
            } // end if

        } while (search_term == "");

        return search_term;

    } // end method GetSearchText

    // method BundleFormatted
    private static string BundleFormatted(int bundle) => bundle == 1 ? "Low (1GB)" : bundle == 2 ? "Medium (4GB)" : bundle == 3 ? "High (8GB)" : "Unlimited";

    // method PackageFormatted
    private static string PackageFormatted(int package) => package == 1 ? "Small (300)" : package == 2 ? "Medium (600)" : "Large (1200)";

    // method DateFormatted
    private static string DateFormatted(string date)
    {
        string[] split_date = date.Split('-');
        return split_date[0] + " " + split_date[1] + " " + split_date[2];

    } // end method DateFormatted

    // method PeriodFormatted
    private static string PeriodFormatted(int period) => period == 1 ? (period + " Month") : period + " Months";

    // method TypeOfClientFormatted
    private static string TypeOfClientFormatted(string reference) => reference[reference.Length - 1] == 'B' ? "Business" : "Non-Business";

    // method GetDate
    private static string GetDate() => DateTime.Now.ToString("dd-MMM-yyyy");

    // method CalculatContractCost
    private static int CalculateContractCost(string reference, int package, int bundle, int period, string allow_international_calls)
    {
        double total_cost = 0;
        double discount_percentage_business_account = 0.9;
        double discount_percentage_12_to_18_months = 0.95;
        double discount_percentage_24_months = 0.9;
        double international_charge = 1.15;

        if (Account_Is_Business_Type(reference))
        {
            if (PackageType(package) == "Small")
            {
                switch (DataBundleType(bundle))
                {
                    case "Low":
                        total_cost += SMALL_LOW * discount_percentage_business_account;
                        break;
                    case "Medium":
                        total_cost += SMALL_MEDIUM * discount_percentage_business_account;
                        break;
                    case "High":
                        total_cost += SMALL_HIGH * discount_percentage_business_account;
                        break;

                } // end switch

            } // end if
            else if (PackageType(package) == "Medium")
            {
                switch (DataBundleType(bundle))
                {
                    case "Low":
                        total_cost += MED_LOW * discount_percentage_business_account;
                        break;
                    case "Medium":
                        total_cost += MED_MEDIUM * discount_percentage_business_account;
                        break;
                    case "High":
                        total_cost += MED_HIGH * discount_percentage_business_account;
                        break;

                } // end switch

            } // end else if
            else
            {
                switch (DataBundleType(bundle))
                {
                    case "Low":
                        total_cost += LARGE_LOW * discount_percentage_business_account;
                        break;
                    case "Medium":
                        total_cost += LARGE_MEDIUM * discount_percentage_business_account;
                        break;
                    case "High":
                        total_cost += LARGE_HIGH * discount_percentage_business_account;
                        break;
                    case "Unlimited":
                        total_cost += LARGE_UNLIMITED * discount_percentage_business_account;
                        break;

                } // end switch

            } // end else

            total_cost *= AllowInternationalCalls(allow_international_calls) ? international_charge : 1;

        } // end if
        else
        {
            if (PackageType(package) == "Small")
            {
                switch (DataBundleType(bundle))
                {
                    case "Low":
                        total_cost += SMALL_LOW;
                        break;
                        total_cost += SMALL_MEDIUM;
                    case "Medium":
                        break;
                    case "High":
                        total_cost += SMALL_HIGH;
                        break;
                } // end switch

            } // end if
            else if (PackageType(package) == "Medium")
            {
                switch (DataBundleType(bundle))
                {
                    case "Low":
                        total_cost += MED_LOW;
                        break;
                    case "Medium":
                        total_cost += MED_MEDIUM;
                        break;
                    case "High":
                        total_cost += MED_HIGH;
                        break;
                } // end switch

            } // end else if
            else
            {
                switch (DataBundleType(bundle))
                {
                    case "Low":
                        total_cost += LARGE_LOW;
                        break;
                    case "Medium":
                        total_cost += LARGE_MEDIUM;
                        break;
                    case "High":
                        total_cost += LARGE_HIGH;
                        break;
                    case "Unlimited":
                        total_cost += LARGE_UNLIMITED;
                        break;
                } // end switch

            } // end else

            if (period == 12 || period == 18) { total_cost *= discount_percentage_12_to_18_months; }
            else if (period == 24) { total_cost *= discount_percentage_24_months; }

            total_cost *= AllowInternationalCalls(allow_international_calls) ? international_charge : 1;

        } // end else

        return (int)total_cost;

    } // end method CalculateContractCost

    // method AllowInternationalCalls
    private static bool AllowInternationalCalls(string yes_or_no) => yes_or_no == "Y";

    // method IsAccountBusinessType
    private static bool Account_Is_Business_Type(string reference) => reference[reference.Length - 1] == 'B';
    private static string AllowInternationalCalls(string client_name, string reference, int package_number, int bundle_number, int period)
    {
        DisplayPartialInfo(client_name, reference, package_number, bundle_number, period);
        string allow_international_calls = string.Empty;
        Console.Write("\nAllow international calls [Y/N]: ");
        do
        {
            allow_international_calls = Console.ReadLine();
            if (allow_international_calls == string.Empty)
            {
                DisplayPartialInfo(client_name, reference, package_number, bundle_number, period);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nNo response entered. Allow international calls [Y/N]: ");
                Console.ResetColor();
            } // end if
            else
            {
                allow_international_calls = allow_international_calls.ToUpper();
                if (allow_international_calls != "Y" && allow_international_calls != "N")
                {
                    DisplayPartialInfo(client_name, reference, package_number, bundle_number, period);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nInvalid response, enter Y or N. Allow international calls [Y/N]: ");
                    Console.ResetColor();
                } // end if
                else break;

            } // end else

        } while (allow_international_calls == "" || (allow_international_calls != "Y" && allow_international_calls != "N"));

        return allow_international_calls.ToUpper();

    } // end method AllowInternationalCalls

    private static void DisplayPartialInfo(string client_name, string reference, int package_number, int bundle_number, int period)
    {
        Console.Clear();
        DisplayStatus(1);
        Console.WriteLine("{0,-12}: {1,-8}", "Client name", client_name);
        Console.WriteLine("{0,-12}: {1,-8}", "Reference", reference);
        Console.WriteLine("{0,-12}: {1,-8}", "Package", PackageType(package_number));
        Console.WriteLine("{0,-12}: {1,-8}", "Bundle", DataBundleType(bundle_number));
        Console.WriteLine("{0,-12}: {1,-8}", "Period", PeriodFormatted(period));

    } // end method DisplayPartialInfo
    private static void DisplayPartialInfo(string reference, string client_name, int package_number, int data_bundle_number)
    {
        Console.Clear();
        DisplayStatus(1);
        Console.WriteLine("{0,-12}: {1,-8}", "Client name", client_name);
        Console.WriteLine("{0,-12}: {1,-8}", "Reference", reference);
        Console.WriteLine("{0,-12}: {1,-8}", "Package", PackageType(package_number));
        Console.WriteLine("{0,-12}: {1,-8}", "Bundle", DataBundleType(data_bundle_number));
    }
    private static void DisplayPartialInfo(string first_name_initial)
    {
        Console.Clear();
        DisplayStatus(1);
        Console.WriteLine("{0,-8}{1,-8}", "First name initial: ", first_name_initial);
    }
    // end method GetReference
    private static void DisplayPartialInfo(int package_number, string client_name, string reference)
    {
        Console.Clear();
        DisplayStatus(1);
        Console.WriteLine("{0,-12}: {1,-8}", "Client name", client_name);
        Console.WriteLine("{0,-12}: {1,-8}", "Reference", reference);
        Console.WriteLine("{0,-12}: {1,-8}", "Package", PackageType(package_number));
    }
    private static void DisplayPartialInfo(string client_name, string reference)
    {
        Console.Clear();
        DisplayStatus(1);
        Console.WriteLine("{0,-12}: {1,-8}", "Client name", client_name);
        Console.WriteLine("{0,-12}: {1,-8}", "Reference", reference);
    }
    private static void DisplayPartialInfo(char first_name_initial)
    {
        Console.Clear();
        DisplayStatus(1);
        Console.WriteLine("{0,-8}{1,-8}", "First name initial: ", first_name_initial);
    }
    private static void DisplayPartialInfo()
    {
        Console.Clear();
        DisplayStatus(1);
    }


    private static int GetPeriod(string reference, string client_name, int package_number, int data_bundle_number)
    {
        string period_string = ""; int period = 0; bool is_period_valid = false;
        DisplayPartialInfo(reference, client_name, package_number, data_bundle_number);

        if (AccountType(reference) == 'B')
        {
            Console.Write("\nEnter period in months [12,18 24]: ");
            do
            {
                period_string = Console.ReadLine();
                if (period_string == "")
                {
                    DisplayPartialInfo(reference, client_name, package_number, data_bundle_number);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nNo period entered. Enter period in months [12,18 24]: ");
                    Console.ResetColor();

                } // end if
                else
                {
                    is_period_valid = int.TryParse(period_string, out period);
                    if (!is_period_valid)
                    {
                        DisplayPartialInfo(reference, client_name, package_number, data_bundle_number);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\nInvalid period entered. Enter period in months [12,18 24]: ");
                        Console.ResetColor();
                    } // end if
                    else
                    {
                        if (period < 12 || (period != 12 && period != 18 && period != 24))
                        {
                            if (period < 12)
                            {
                                DisplayPartialInfo(reference, client_name, package_number, data_bundle_number);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("\nAs a business customer, you must take at least 12 months deal . Enter period in months [12,18 24]: ");
                                Console.ResetColor();
                            } // end if
                            else
                            {
                                DisplayPartialInfo(reference, client_name, package_number, data_bundle_number);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("\nIncorrect period entered. Enter period in months [12,18 24]: ");
                                Console.ResetColor();

                            } // end else

                        } // end if

                    } // end else

                } // end else

            } while (period_string == "" || !is_period_valid || (period != 12 && period != 18 && period != 24) || period < 12);

        } // end if
        else
        {
            DisplayPartialInfo(reference, client_name, package_number, data_bundle_number);
            Console.Write("\nEnter period in months [1,12,18 24]: ");
            do
            {
                period_string = Console.ReadLine();
                if (period_string == "")
                {
                    DisplayPartialInfo(reference, client_name, package_number, data_bundle_number);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nNo period entered. Enter period in months [1,12,18 24]: ");
                    Console.ResetColor();
                } // end if
                else
                {
                    is_period_valid = int.TryParse(period_string, out period);
                    if (!is_period_valid)
                    {
                        DisplayPartialInfo(reference, client_name, package_number, data_bundle_number);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\nInvalid period entered. Enter period in months [1,12,18 24]: ");
                        Console.ResetColor();
                    } // end if
                    else
                    {
                        if (period != 1 && period != 12 && period != 18 && period != 24)
                        {
                            DisplayPartialInfo(reference, client_name, package_number, data_bundle_number);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("\nIncorrect period entered. Enter period in months [1,12,18 24]: ");
                            Console.ResetColor();
                        } // end if

                    } // end else

                } // end else

            } while (period_string == "" || !is_period_valid || (period != 12 && period != 18 && period != 24 && period != 1));

        } // end else

        return period;

    } // end method GetPeriod

    // method GetAccountType
    private static char AccountType(string customer_reference) => customer_reference[customer_reference.Length - 1];

    private static string GetReference(string first_name_initial)
    {
        DisplayPartialInfo(first_name_initial);
        Console.Write("\nEnter Reference: ");
        string reference = "";
        bool is_first_part_wrong = false; bool is_second_part_wrong = false; bool is_third_part_wrong = false;
        do
        {
            reference = Console.ReadLine();
            if (reference == string.Empty)
            {
                DisplayPartialInfo(first_name_initial);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nNo reference entered. Enter reference: ");
                Console.ResetColor();

            } // end if
            else if (reference.Length != 6)
            {
                DisplayPartialInfo(first_name_initial);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nReference must be 6 characters long. Enter reference: ");
                Console.ResetColor();
            } // end else
            else
            {
                is_first_part_wrong = false; is_second_part_wrong = false; is_third_part_wrong = false;

                reference = reference.ToUpper();
                string first_part = reference.Substring(0, 2);
                string second_part = reference.Substring(first_part.Length, 3);
                string third_part = reference.Substring(5);
                CheckReference(first_part, second_part, third_part, ref is_first_part_wrong, ref is_second_part_wrong, ref is_third_part_wrong);
                if (is_first_part_wrong && is_second_part_wrong && is_third_part_wrong)
                {
                    DisplayPartialInfo(first_name_initial);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nInvalid reference {0}.Enter again: ", reference);
                    Console.ResetColor();
                } // end if
                else if (is_first_part_wrong && is_second_part_wrong)
                {
                    DisplayPartialInfo(first_name_initial);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nInvalid values: ");
                    Console.ResetColor();
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(first_part + second_part);
                    Console.ResetColor();
                    Console.Write(third_part + "]");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" highlighted in red.Enter ref again: ");

                    Console.ResetColor();

                } // end else if
                else if (is_second_part_wrong && is_third_part_wrong)
                {
                    DisplayPartialInfo(first_name_initial);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nInvalid values: ");
                    Console.ResetColor();
                    Console.Write("[" + first_part);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(second_part + third_part);
                    Console.ResetColor();
                    Console.Write("]");
                    Console.Write("highlighted in red. Enter ref again: ");

                    Console.ResetColor();

                } // end else if
                else if (is_first_part_wrong && is_third_part_wrong)
                {
                    DisplayPartialInfo(first_name_initial);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nInvalid values: ");
                    Console.ResetColor();
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(first_part);
                    Console.ResetColor();
                    Console.Write(second_part);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(third_part);
                    Console.ResetColor();
                    Console.Write("]");
                    Console.Write(" highlighted in red. Enter ref again: ");
                    Console.ResetColor();

                } // end if
                else if (is_first_part_wrong)
                {
                    DisplayPartialInfo(first_name_initial);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nInvalid values: ");
                    Console.ResetColor();
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(first_part);
                    Console.ResetColor();
                    Console.Write(second_part + third_part);
                    Console.ResetColor(); ;
                    Console.Write("]");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" highlighted in red. Enter ref again: ");

                    Console.ResetColor();
                } // end else if
                else if (is_second_part_wrong)
                {
                    DisplayPartialInfo(first_name_initial);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nInvalid values: ");

                    Console.ResetColor();
                    Console.Write("[" + first_part);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(second_part);
                    Console.ResetColor();
                    Console.Write(third_part + "]");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" highlighted in red. Enter ref again: ");
                    Console.ResetColor();

                } // end else if
                else if (is_third_part_wrong)
                {
                    DisplayPartialInfo(first_name_initial);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nInvalid values: ");
                    Console.ResetColor();

                    Console.Write("[" + first_part + second_part);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(third_part);
                    Console.ResetColor();
                    Console.Write("]");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" highlighted in red. Enter ref again: ");
                    Console.ResetColor();
                } // end else if
            } // end else

        } while (reference == "" || reference.Length != 6 || is_first_part_wrong || is_second_part_wrong || is_third_part_wrong);

        return reference;

    } // end method GetReference

    // method CheckReferenceNumber
    private static void CheckReference(string first_part, string second_part, string third_part, ref bool is_first_part_wrong, ref bool is_second_part_wrong, ref bool is_third_part_wrong)
    {
        is_first_part_wrong = (first_part.Any(x => !char.IsLetter(x)));
        is_second_part_wrong = !(second_part.All(char.IsDigit));
        is_third_part_wrong = !(third_part.Contains("B") || third_part.Contains("N"));

    } // end method CheckReference

    // method GetDataBundleType
    private static string DataBundleType(int data_bundle_number)
        => data_bundle_number == 1 ? "Low" : data_bundle_number == 2 ? "Medium" : data_bundle_number == 3 ? "High" : data_bundle_number == 4 ? "Unlimited" : "None";

    private static int GetDataBundle(int package_number, string client_name, string reference)
    {
        DisplayPartialInfo(package_number, client_name, reference);
        string data_bundle_string = string.Empty;
        int data_bundle_number = 0;
        bool is_data_bundle_correct = false;
        if (PackageType(package_number) == "Large")
        {
            Console.Write("\nEnter Data Bundle [1.Low 2.Medium 3.High 4.Unlimited]: ");
            do
            {
                data_bundle_string = Console.ReadLine();
                if (data_bundle_string == string.Empty)
                {
                    DisplayPartialInfo(package_number, client_name, reference);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nNo Data bundle entered. Enter Data Bundle [1.Low 2.Medium 3.High 4.Unlimited]: ");
                    Console.ResetColor();
                } // end if
                else
                {
                    is_data_bundle_correct = int.TryParse(data_bundle_string, out data_bundle_number);
                    if (is_data_bundle_correct)
                    {
                        if (data_bundle_number != 1 && data_bundle_number != 2 && data_bundle_number != 3 && data_bundle_number != 4)
                        {
                            DisplayPartialInfo(package_number, client_name, reference);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("\nIncorrect value. Enter Data Bundle [1.Low 2.Medium 3.High 4.Unlimited]: ");
                            Console.ResetColor();

                        } // end if
                        else break;

                    } // end if
                    else
                    {
                        DisplayPartialInfo(package_number, client_name, reference);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\nInvalid value. Enter Data Bundle [1.Low 2.Medium 3.High 4.Unlimited]: ");
                        Console.ResetColor();

                    } // end else

                } // end else

            } while (data_bundle_string == String.Empty || !is_data_bundle_correct || (data_bundle_number != 1 && data_bundle_number != 2 && data_bundle_number != 3 || data_bundle_number != 4));

        } // end if
        else
        {
            DisplayPartialInfo(package_number, client_name, reference);
            Console.Write("\nEnter Data Bundle [1.Low 2.Medium 3.High]: ");
            do
            {
                data_bundle_string = Console.ReadLine();
                if (data_bundle_string == string.Empty)
                {
                    DisplayPartialInfo(package_number, client_name, reference);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nNo Data bundle entered. Enter Data Bundle [1.Low 2.Medium 3.High]: ");
                    Console.ResetColor();
                } // end if
                else
                {
                    is_data_bundle_correct = int.TryParse(data_bundle_string, out data_bundle_number);
                    if (is_data_bundle_correct)
                    {
                        if (data_bundle_number != 1 && data_bundle_number != 2 && data_bundle_number != 3)
                        {
                            DisplayPartialInfo(package_number, client_name, reference);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("\nIncorrect value. Enter Data Bundle [1.Low 2.Medium 3.High]: ");
                            Console.ResetColor();

                        } // end if
                        else break;

                    } // end if
                    else
                    {
                        DisplayPartialInfo(package_number, client_name, reference);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\nInvalid value. Enter Data Bundle [1.Low 2.Medium 3.High]: ");
                        Console.ResetColor();

                    } // end else

                } // end else

            } while (data_bundle_string == String.Empty || !is_data_bundle_correct || (data_bundle_number != 1 && data_bundle_number != 2 && data_bundle_number != 3));

        } // end else
        return data_bundle_number;

    }// end method GetDataBundle

    // method GetPackageType
    private static string PackageType(int package_number) => package_number == 1 ? "Small" : package_number == 2 ? "Medium" : package_number == 3 ? "Large" : "None";

    private static int GetPackage(string client_name, string reference)
    {
        DisplayPartialInfo(client_name, reference);
        string package_string = string.Empty;
        int package_number = 0;
        bool is_package_correct = false;
        Console.Write("\nEnter package [1.Small 2.Medium 3.Large]: ");
        do
        {
            package_string = Console.ReadLine();
            if (package_string == string.Empty)
            {
                DisplayPartialInfo(client_name, reference);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nNo package number entered. Enter package [1.Small 2.Medium 3.Large]: ");
                Console.ResetColor();
            } // end if
            else
            {
                is_package_correct = int.TryParse(package_string, out package_number);
                if (!is_package_correct)
                {
                    DisplayPartialInfo(client_name, reference);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nInvalid package entered. Enter package [1.Small 2.Medium 3.Large]: ");
                    Console.ResetColor();

                } // end if
                else
                {
                    if (package_number != 1 && package_number != 2 && package_number != 3)
                    {
                        DisplayPartialInfo(client_name, reference);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\nIncorrect package entered. Enter package [1.Small 2.Medium 3.Large]: ");
                        Console.ResetColor();

                    } // end if
                    else break;

                } // end else

            } // end else

        } while (package_string == String.Empty || !is_package_correct || (package_number != 1 && package_number != 2 && package_number != 3));

        return package_number;

    }  // end method GetPackage

    // method GetName to get name from the user
    private static string GetClientName()
    {
        // call to GetFirstNameInitial
        char first_name_initial = GetFirstNameInitial();

        // call to GetLastname
        string surname = GetSurname(first_name_initial);

        return first_name_initial + " " + surname;

    } // end method GetName

    private static string GetSurname(char first_name_initial)
    {
        DisplayPartialInfo(first_name_initial);
        string surname = string.Empty;
        Console.Write("\nEnter surname: ");
        do
        {
            Console.ResetColor();
            surname = Console.ReadLine();
            if (surname == string.Empty)
            {
                DisplayPartialInfo(first_name_initial);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nNo surname entered. Enter surname: ");
                Console.ResetColor();

            } // end if
            else
            {
                if (surname.Length < 5 || surname.Length > 24)
                {
                    DisplayPartialInfo(first_name_initial);
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (surname.Length < 5)
                    {
                        Console.Write("\nSurname is too short. Enter surname again: ");
                    } // end if
                    else if (surname.Length > 20)
                    {
                        Console.Write("\nSurname is too long. Enter surname again: ");
                    } // end else
                    Console.ResetColor();
                } // end if
            } // end else

        } while (surname == String.Empty || surname.Length < 5 || surname.Length > 24);

        return char.ToUpper(surname[0]) + surname.Substring(1).ToLower();

    } // method GetFirstNameInitial
    private static Char GetFirstNameInitial()
    {
        DisplayPartialInfo();
        bool is_first_name_initial_correct = false;
        char initial_char = ' ';
        Console.Write("Enter first name initial [A-Z]: ");
        string first_name_initial = string.Empty;

        do
        {
            first_name_initial = Console.ReadLine();
            if (first_name_initial == string.Empty)
            {
                DisplayPartialInfo();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("No response entered. Enter first name initial [A-Z]: ");
                Console.ResetColor();

            } // end if
            else
            {
                first_name_initial = first_name_initial.ToUpper();
                is_first_name_initial_correct = Char.TryParse(first_name_initial, out initial_char);
                if (!is_first_name_initial_correct || !CheckFirstNameInitialAgainstAscii(initial_char))
                {
                    DisplayPartialInfo();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Invalid response entered. Enter first name initial [A-Z]: ");
                    Console.ResetColor();
                } // end if
                else break; ;

            } // end else

        } while (!is_first_name_initial_correct || first_name_initial == String.Empty || !CheckFirstNameInitialAgainstAscii(initial_char));

        return initial_char;

    } // end method GetFirstNameInitial

    // method CheckFirstNameInitialAgainstAscii
    private static bool CheckFirstNameInitialAgainstAscii(char initial_to_check) => (initial_to_check >= 65 && initial_to_check <= 90);

    // method ProcessOptionsFive
    private static void ProcessOptionFive()
    {
        Console.WriteLine("Thank you for using Telephone Contract Manager !");
        Environment.Exit(0);


    } // end method ProcessOptionFive

    // method DisplayMenu to display the main menu screen
    private static void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("******************Welcome to Telephone Contract Manager******************");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nBelow are the options:\n");
        Console.WriteLine("1. Enter new Contract");
        Console.WriteLine("2. Display Summary of Contracts");
        Console.WriteLine("3. Display Summary of Contracts for Selected Month");
        Console.WriteLine("4. Find and display Contract");
        Console.WriteLine("5. Exit\n");
        Console.ResetColor();

    } // end method DisplayMenu

    // method GetUserChoice
    private static int GetUserChoice()
    {
        Console.Write("Enter choice: ");
        string user_selection_string = Console.ReadLine();
        int parsed_user_selection = ValidateUserSelection(user_selection_string);

        return parsed_user_selection;

    } // end method GetUserChoice

    // method ValidateUserSelection
    private static int ValidateUserSelection(string user_selection_string)
    {
        int user_selection_number = 0;
        do
        {
            if (user_selection_string == string.Empty)
            {
                Console.Clear();
                DisplayMenu();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("No selection made. Please make a selection: ");
                Console.ResetColor();

            } // end if
            else
            {
                bool try_parse_selection = int.TryParse(user_selection_string, out user_selection_number);
                if (!try_parse_selection)
                {
                    Console.Clear();
                    DisplayMenu();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Invalid choice. Please make a valid selection: ");
                    Console.ResetColor();
                } // end if
                else
                {
                    if (user_selection_number != 1 && user_selection_number != 2 && user_selection_number != 3 && user_selection_number != 4 && user_selection_number != 5)
                    {
                        Console.Clear();
                        DisplayMenu();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Incorrect choice. Please make a correct selection: ");
                        Console.ResetColor();

                    } // end if
                    else break;

                } // end else

            } // end else

            user_selection_string = Console.ReadLine();

        } while (user_selection_string == String.Empty || (user_selection_number != 1 && user_selection_number != 2 && user_selection_number != 3 && user_selection_number != 4 && user_selection_number != 5));

        return user_selection_number;

    } // end method ValidateUserSelection

} // end Class

