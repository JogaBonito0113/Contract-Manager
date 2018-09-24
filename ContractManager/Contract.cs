using System;


public class Contract : IComparable<Contract>
{
    public string Name { get; set; }
    public int Package { get; set; }
    public int DataBundle { get; set; }
    public string Reference { get; set; }
    public int Period { get; set; }
    public string Date { get; set; }
    public string AllowInternationalCall { get; set; }
    public int MonthlyCharge { get; set; }



    public Contract(string the_date, int the_package_type, int the_data_bundle_type, int the_period, string the_allow_internatioal_call, string the_reference, int the_monthly_charge, string the_name)
    {
        this.Date = the_date;
        this.Package = the_package_type;
        this.DataBundle = the_data_bundle_type;
        this.Period = the_period;
        this.AllowInternationalCall = the_allow_internatioal_call;
        this.MonthlyCharge = the_monthly_charge;
        this.Reference = the_reference;
        this.Name = the_name;

    } // end constructor



    public int CompareTo(Contract other)
    {
        return string.Compare(this.Reference, other.Reference);

    } // end method CompareTo

} // end Class Contract

