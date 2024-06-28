using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionLugaresFacilities.Models
{
    public class ApiEmployeeModel
    {
            public int ID { get; set; }
            public string EmployeeNumber { get; set; }
            public string EmployeeName { get; set; }
            public string SupervisorNumber { get; set; }
            public string SupervisorName { get; set; }
            public string CostCenter { get; set; }
            public string Position { get; set; }
            public string JobGrade { get; set; }
            public string Shift { get; set; }
            public string Location { get; set; }
            public string EmployeeType { get; set; }
            public string FLSAStatus { get; set; }
            public string WorkerType { get; set; }
            public string Status { get; set; }
            public string ContingentWorkerType { get; set; }
            public string BusinessUnit { get; set; }
            public string AssignmentCategory { get; set; }
            public string VoluntaryOrInvoluntary { get; set; }
            public DateTime? FirstServiceDate { get; set; }
            public DateTime? SecondServiceDate { get; set; }
            public DateTime? InactiveDate { get; set; }
            public DateTime? CWKToEMPDate { get; set; }
            public string FirstName { get; set; }
            public string ApellidoMaterno { get; set; }
            public string ApellidoPaterno { get; set; }
            public string Gender { get; set; }
            public DateTime? BirthDate { get; set; }
            public int? Age { get; set; }
            public string City { get; set; }
            public string MaritalStatus { get; set; }
            public string Neighborhood { get; set; }
            public string Address { get; set; }
            public string Address2 { get; set; }
            public string ZipCode { get; set; }
            public string PhoneNumber { get; set; }
            public string Country { get; set; }
            public string Education { get; set; }
            public string Qualification { get; set; }
            public string RFC { get; set; }
            public string CURP { get; set; }
            public string BU { get; set; }
            public string IBT { get; set; }
            public string IBTDescription { get; set; }
            public string NSS { get; set; }
            public string JobProfile { get; set; }
            public string JobCode { get; set; }
            public int? LocationID { get; set; }

    }
}